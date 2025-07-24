using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace XXLFramework
{
	public abstract class PanelContainer : MonoBehaviour, IPanelContainer, IContainerState
	{
		[Header("卸载场景时是否销毁")]
		public bool DestroyWhenUnloadScene = false;

		protected PanelTable panelTable = new PanelTable();
		protected Stack<BasePanel> panelStack = new Stack<BasePanel>(); //面板堆栈

		private RectTransform Bg;
		private RectTransform Common;
		private RectTransform PopUI;

		public Action OnEnter;
		public Action OnExit;

		public BasePanel FirstOpenPanel; //首次打开的面板
		bool isInit = false;

		private void Awake()
		{
			Init();
		}

		public virtual void Init()
		{
			if (isInit)
			{
				return;
			}
			isInit = true;
			if (!DestroyWhenUnloadScene)
			{
				DontDestroyOnLoad(gameObject);
			}
			List<BasePanel> panelList = GetAllPanelsInChildrenExceptSelf();
			AddPanels(panelList);
			CreateLevelPanel();
			UIKit.AddContainer(this);
			if (FirstOpenPanel != null)
			{
				FirstOpenPanel.Open();
			}
		}

		private List<BasePanel> GetAllPanelsInChildrenExceptSelf()
		{
			List<BasePanel> list = gameObject.GetComponentsInChildren<BasePanel>(true).ToList();
			list = list.Where(p => p.gameObject != gameObject).ToList(); ;
			return list;
		}

		protected void CreateLevelPanel()
		{
			Bg = GetLevelPanel(PanelLevel.Bg);
			Common = GetLevelPanel(PanelLevel.Common);
			PopUI = GetLevelPanel(PanelLevel.PopUI);
		}

		RectTransform GetLevelPanel(PanelLevel level)
		{
			RectTransform rect = new GameObject(level.ToString()).AddComponent<RectTransform>();
			rect.SetParent(transform);
			rect.Spread();
			return rect;
		}


		public T OpenPanel<T>(PanelLevel level = PanelLevel.Common, IPanelData data = null, bool isReset = false) where T : BasePanel
		{
			BasePanel panel = GetPanel<T>();
			if (panel != null)
			{
				PanelInfo info = PanelInfo.Allocate(panel.GetType(), panel.name, level);
				OpenPanel(panel, info, data, false, isReset);
			}
			else
			{
				panel = UIManager.Instance.CreateUIPanel<T>();
				if (panel != null)
				{
					PanelInfo info = PanelInfo.Allocate(panel.GetType(), panel.name, level);
					OpenPanel(panel, info, data, true, isReset);
				}
			}

			return panel as T;
		}

		public T OpenPanel<T>(string gameObjName, PanelLevel level = PanelLevel.Common, IPanelData data = null, bool isReset = false) where T : BasePanel
		{
			BasePanel panel = GetPanel<T>(gameObjName);
			if (panel != null)
			{
				PanelInfo info = PanelInfo.Allocate(panel.GetType(), panel.name, level);
				OpenPanel(panel, info, data, false, isReset);
			}
			else
			{
				panel = UIManager.Instance.CreateUIPanel<T>(gameObjName);
				if (panel != null)
				{
					PanelInfo info = PanelInfo.Allocate(panel.GetType(), panel.name, level);
					OpenPanel(panel, info, data, true, isReset);
				}
			}
			return panel as T;
		}

		/// <summary>
		/// 首次打开面板
		/// </summary>
		/// <param name="panel"></param>
		/// <param name="info"></param>
		/// <param name="data"></param>
		/// <param name="loadFromAsset">如果从资源库中加载，要设置位置</param>
		void OpenPanel(BasePanel panel, PanelInfo info, IPanelData data, bool loadFromAsset, bool isReset)
		{
			panel.Info = info;
			panel.BelongContainer = this;
			if (loadFromAsset)
			{
				Vector3 oldLocalScale = panel.RectTransform.localScale;
				Vector3 anchoredPosition = panel.RectTransform.anchoredPosition;
				Vector2 sizeDelta = panel.RectTransform.sizeDelta;
				SetLevelOfPanel(info.Level, panel);
				panel.RectTransform.localScale = oldLocalScale;
				panel.RectTransform.anchoredPosition = anchoredPosition;
				panel.RectTransform.sizeDelta = sizeDelta;
				panelTable.Add(panel);
				panel.Init(data);
			}
			else
			{
				panel.Init(data);
			}
			panel.Open(data, isReset);
		}

		public void PushPanel(BasePanel panel)
		{
			if (panelStack==null)
			{
				panelStack = new Stack<BasePanel>();
			}
			if (panelStack.Count>0)
			{
				BasePanel topPanel = panelStack.Peek();
				topPanel.Close();
			}
			panel.Open();
			panelStack.Push(panel);
		}

		public void PopPanel()
		{
			if (panelStack != null)
			{
				if (panelStack.Count >= 2)
				{
					BasePanel topPanel = panelStack.Peek();
					topPanel.Close();
					//出栈处理
					panelStack.Pop();
					//出栈后，下一个窗体做“重新显示”处理。
					BasePanel nextPanel = panelStack.Peek();
					nextPanel.Open();
				}
				else if (panelStack.Count == 1)
				{
					BasePanel topPanel = panelStack.Peek();
					topPanel.Close();
					//出栈处理
					panelStack.Pop();
				}
			}
		}

		private void SetLevelOfPanel(PanelLevel level, BasePanel panel)
		{
			switch (level)
			{
				case PanelLevel.Bg:
					panel.RectTransform.SetParent(Bg);
					break;
				case PanelLevel.Common:
					panel.RectTransform.SetParent(Common);
					break;
				case PanelLevel.PopUI:
					panel.RectTransform.SetParent(PopUI);
					break;
			}
		}

		public void ClosePanel(BasePanel panel, bool destroy)
		{
			if (destroy)
			{
				panelTable.Remove(panel);
			}
			panel.Close(destroy);
		}


		public void ClosePanel<T>(string panelName = null, bool destroy = false) where T : BasePanel
		{
			BasePanel panel = GetPanel<T>(panelName);
			if (panel != null)
			{
				if (destroy)
				{
					panelTable.Remove(panel);
				}
				panel.Close(destroy);
			}
			else
			{
				Debug.Log($"未找到面板:{typeof(T)},Name:{panelName}");
			}
		}



		public T GetPanel<T>(string gameObjName = null) where T : BasePanel
		{
			PanelKey key = new PanelKey(typeof(T), gameObjName);
			var retPanel = panelTable.GetPanel(key);
			return retPanel as T;
		}

		public void AddPanels(List<BasePanel> panelList)
		{
			//Debug.Log($"添加{panelList.Count}个panel");
			foreach (var item in panelList)
			{
				panelTable.Add(item);
				item.BelongContainer = this;
			}
			//Debug.Log($"当前共{panelTable.GameObjectNameIndex.Dictionary.Count}个面板");
		}

		//清空堆栈
		public void ClearStack()
		{
			foreach (var item in panelStack)
			{
				item.Close();
			}
			panelStack.Clear();
		}

		public void CloseAllPanel()
		{
			panelStack.Clear();
			foreach (var item in panelTable)
			{
				item.Close();
			}
		}

		public void RemovePanel(BasePanel panel)
		{
			panelTable.Remove(panel);
		}

		private void OnDestroy()
		{
			if (DestroyWhenUnloadScene)
			{
				UIKit.RemoveContainer(this);
			}
		}

		public virtual void Enter()
		{
			this.Show();
			OnEnter?.Invoke();
		}

		public virtual void Exit()
		{
			this.Hide();
			OnExit?.Invoke();
		}

	}
}
