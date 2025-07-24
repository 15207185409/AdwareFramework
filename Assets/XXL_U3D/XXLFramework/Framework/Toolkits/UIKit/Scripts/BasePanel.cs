/****************************************************************************

 ****************************************************************************/

using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace XXLFramework
{
	/// <summary>
	/// 每个 UIPanel 对应的Data
	/// </summary>
	public interface IPanelData
	{
	}

	public class BasePanelData : IPanelData
	{
	}
	public enum PanelLevel
	{
		Bg, //背景层UI
		Common, //普通层UI
		PopUI, //弹出层UI
	}


	[RequireComponent(typeof(RectTransform), typeof(CanvasGroup))]
	public class BasePanel :  BindGroup
	{

#if UNITY_EDITOR
		public override string TemplateName { get { return typeof(BasePanel).Name; } }
#endif
		private bool isInit; //是否初始化
		public RectTransform RectTransform { get { return GetComponent<RectTransform>(); } }
		private CanvasGroup canvasGroup;
		public PanelInfo Info;
		public IPanelContainer BelongContainer;

		public Action OnClosed;


		private void Awake()
		{
			canvasGroup = this.GetOrAddComponent<CanvasGroup>();
		}

		public virtual void Init(IPanelData uiData = null)
		{
			if (!isInit)
			{
				isInit = true;
				OnInit(uiData);
				RecordInitValue();
			}
		}

		protected virtual void OnInit(IPanelData data = null)
		{

		}

		public void Open(IPanelData data = null, bool isReset = false)
		{
			this.Show();
			if (BelongContainer==null)
			{
				BelongContainer = UIKit.Manager.CurrentContainer;
			}
			OnOpen(data);
			if (isReset)
			{
				Reset();
				OnReset();
			}
		}

		protected virtual void OnOpen(IPanelData data = null)
		{

		}

		protected void CloseSelf(bool destroy = false)
		{
			if (BelongContainer!=null)
			{
				BelongContainer.ClosePanel(this, destroy);
			}
			else
			{
				Close(destroy);
			}
		}

		public void Close(bool destroy = false)
		{
			this.Hide();

			OnClose(destroy);
			OnClosed?.Invoke();
			if (destroy)
			{
				Info.Recycle2Cache();
				Destroy(gameObject);
			}
		}

		protected virtual void OnDestroy()
		{
			if (BelongContainer != null)
			{
				BelongContainer.RemovePanel(this);
			}
		}

		protected virtual void OnClose(bool destroy)
		{

		}

		public virtual void Freeze()
		{
			canvasGroup.blocksRaycasts = false;
		}
		public virtual void ReFreeze()
		{
			canvasGroup.blocksRaycasts = true;
		}

		protected virtual void OnReset()
		{

		}
	
	}

	public struct PanelKey
	{
		public Type PanelType;
		public string GameObjName;
		public PanelKey(Type type, string gameObjName)
		{
			this.PanelType = type;
			this.GameObjName = gameObjName;
		}
	}



}
