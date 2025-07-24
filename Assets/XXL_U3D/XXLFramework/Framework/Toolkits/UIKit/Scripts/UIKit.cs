/****************************************************************************
 * Copyright (c) 2015 - 2022  UNDER MIT License
 * 

 ****************************************************************************/

using System;

namespace XXLFramework
{
#if UNITY_EDITOR
	[ClassAPI("01.UIKit", "UIKit", 0, "UIKit")]
	[APIDescriptionCN("界面管理方案")]
	[APIDescriptionEN("UI Managements Solution")]
	[APIExampleCode("UIKit工具")]
#endif
	public class UIKit
	{
		public static UIManager Manager => UIManager.Instance;

		public static void Init()
		{
			Manager.Init();
		}


#if UNITY_EDITOR
		[MethodAPI]
		[APIDescriptionCN("获取容器")]
		[APIDescriptionEN("GetContainer")]
		[APIExampleCode("UIKit.GetContainer<SecondContainer>()")]
#endif
		/// <summary>
		/// 获取容器，默认打开关闭面板那些使用的都是当前容器，如果要另外容器的面板进行操作，可以使用此方法
		/// </summary>
		/// <returns></returns>
		public static PanelContainer GetContainer<T>() where T : PanelContainer
		{
			return UIManager.Instance.GetContainer<T>();
		}

#if UNITY_EDITOR
		[MethodAPI]
		[APIDescriptionCN("打开面板")]
		[APIDescriptionEN("OpenPanel")]
		[APIExampleCode("UIKit.OpenPanel<Panel1>()")]
#endif
		/// <summary>
		/// 打开面板
		/// </summary>
		/// <typeparam name="T">面板类型</typeparam>
		/// <param name="level">面板等级</param>
		/// <param name="uiData">数据</param>
		/// <param name="isReset">是否重置数据</param>
		/// <returns></returns>
		public static T OpenPanel<T>(PanelLevel level = PanelLevel.Common, IPanelData uiData = null, bool isReset = false) where T : BasePanel
		{
			return UIManager.Instance.CurrentContainer.OpenPanel<T>(level, uiData, isReset);
		}

		public static T OpenPanel<T>(IPanelData uiData, PanelLevel level = PanelLevel.Common, bool isReset = false) where T : BasePanel
		{
			return UIManager.Instance.CurrentContainer.OpenPanel<T>(level, uiData, isReset);
		}

		public static T OpenPanel<T>(bool isReset) where T : BasePanel
		{
			return UIManager.Instance.CurrentContainer.OpenPanel<T>(PanelLevel.Common, null, isReset);
		}

		/// <summary>
		/// 打开面板
		/// </summary>
		/// <typeparam name="T">面板类型</typeparam>
		/// <param name="level">面板等级</param>
		/// <param name="uiData">数据</param>
		/// <param name="gameObjName">具体名字</param>
		/// <returns></returns>
		public static T OpenPanel<T>(string gameObjName, PanelLevel level = PanelLevel.Common, IPanelData uiData = null, bool isReset = false) where T : BasePanel
		{
			return UIManager.Instance.CurrentContainer.OpenPanel<T>(gameObjName, level, uiData, isReset);
		}

#if UNITY_EDITOR
		[MethodAPI]
		[APIDescriptionCN("关闭面板")]
		[APIDescriptionEN("ClosePanel")]
		[APIExampleCode("UIKit.ClosePanel<Panel1>()")]
#endif
		/// <summary>
		/// 关闭面板
		/// </summary>
		/// <typeparam name="T">面板类型</typeparam>
		/// <param name="gameObjName">面板名字</param>
		/// <param name="destroy">是否销毁</param>
		public static void ClosePanel<T>(string gameObjName = null, bool destroy = false) where T : BasePanel
		{
			UIManager.Instance.CurrentContainer.ClosePanel<T>(gameObjName, destroy);
		}

#if UNITY_EDITOR
		[MethodAPI]
		[APIDescriptionCN("关闭面板")]
		[APIDescriptionEN("ClosePanel")]
		[APIExampleCode("UIKit.ClosePanel<Panel1>()")]
#endif
		public static void ClosePanel(BasePanel panel, bool destroy = false)
		{
			UIManager.Instance.CurrentContainer.ClosePanel(panel, destroy);
		}


#if UNITY_EDITOR
		[MethodAPI]
		[APIDescriptionCN("面板入栈")]
		[APIDescriptionEN("PushPanel")]
		[APIExampleCode("UIKit.PushPanel<Panel1>()")]
#endif
		/// <summary>
		/// 打开面板并入栈
		/// </summary>
		/// <typeparam name="T">面板类型</typeparam>
		/// <param name="level">面板等级</param>
		/// <param name="uiData">数据</param>
		/// <param name="isReset">是否重置数据</param>
		/// <returns></returns>
		public static T PushPanel<T>(PanelLevel level = PanelLevel.Common, IPanelData uiData = null, bool isReset = false) where T : BasePanel
		{
			T panel = UIManager.Instance.CurrentContainer.OpenPanel<T>(level, uiData, isReset);
			UIManager.Instance.CurrentContainer.PushPanel(panel);
			return panel;
		}

		public static T PushPanel<T>(IPanelData uiData, PanelLevel level = PanelLevel.Common, bool isReset = false) where T : BasePanel
		{
			T panel = UIManager.Instance.CurrentContainer.OpenPanel<T>(level, uiData, isReset);
			UIManager.Instance.CurrentContainer.PushPanel(panel);
			return panel;
		}

		public static T PushPanel<T>(bool isReset) where T : BasePanel
		{
			T panel = UIManager.Instance.CurrentContainer.OpenPanel<T>(PanelLevel.Common, null, isReset);
			UIManager.Instance.CurrentContainer.PushPanel(panel);
			return panel;
		}

		/// <summary>
		/// 打开面板
		/// </summary>
		/// <typeparam name="T">面板类型</typeparam>
		/// <param name="level">面板等级</param>
		/// <param name="uiData">数据</param>
		/// <param name="gameObjName">具体名字</param>
		/// <returns></returns>
		public static T PushPanel<T>(string gameObjName, PanelLevel level = PanelLevel.Common, IPanelData uiData = null, bool isReset = false) where T : BasePanel
		{
			T panel = UIManager.Instance.CurrentContainer.OpenPanel<T>(gameObjName, level, uiData, isReset);
			UIManager.Instance.CurrentContainer.PushPanel(panel);
			return panel;
		}

#if UNITY_EDITOR
		[MethodAPI]
		[APIDescriptionCN("面板出栈")]
		[APIDescriptionEN("PopPanel")]
		[APIExampleCode("UIKit.PopPanel()")]
#endif
		public static void PopPanel()
		{
			UIManager.Instance.CurrentContainer.PopPanel();
		}

#if UNITY_EDITOR
		[MethodAPI]
		[APIDescriptionCN("清空堆栈")]
		[APIDescriptionEN("ClearStack")]
		[APIExampleCode("UIKit.ClearStack()")]
#endif
		public static void ClearStack()
		{
			UIManager.Instance.CurrentContainer.ClearStack();
		}


#if UNITY_EDITOR
		[MethodAPI]
		[APIDescriptionCN("关闭所有面板并清空堆栈")]
		[APIDescriptionEN("CloseAllPanel")]
		[APIExampleCode("UIKit.CloseAllPanel()")]
#endif
		public static void CloseAllPanel() 
		{
			UIManager.Instance.CurrentContainer.CloseAllPanel();
		}

#if UNITY_EDITOR
		[MethodAPI]
		[APIDescriptionCN("获取面板")]
		[APIDescriptionEN("GetPanel")]
		[APIExampleCode("UIKit.GetPanel<Panel1>()")]
#endif
		/// <summary>
		/// 获取面板
		/// </summary>
		/// <typeparam name="T">面板类型</typeparam>
		/// <param name="gameObjName">面板名字</param>
		/// <returns></returns>
		public static T GetPanel<T>(string gameObjName = null) where T : BasePanel
		{
			var retPanel = UIManager.Instance.CurrentContainer.GetPanel<T>(gameObjName);
			return retPanel;
		}

#if UNITY_EDITOR
		[MethodAPI]
		[APIDescriptionCN("添加容器")]
		[APIDescriptionEN("AddContainer")]
		[APIExampleCode("UIKit.AddContainer(container)")]
#endif
		public static void AddContainer(PanelContainer container)
		{
			UIManager.Instance.AddContainer(container);
		}

#if UNITY_EDITOR
		[MethodAPI]
		[APIDescriptionCN("移除容器")]
		[APIDescriptionEN("RemoveContainer")]
		[APIExampleCode("")]
#endif
		public static void RemoveContainer<T>() where T : PanelContainer
		{
			UIManager.Instance.RemoveContainer<T>();
		}

#if UNITY_EDITOR
		[MethodAPI]
		[APIDescriptionCN("移除容器")]
		[APIDescriptionEN("RemoveContainer")]
		[APIExampleCode("")]
#endif
		public static void RemoveContainer<T>(T t) where T : PanelContainer
		{
			UIManager.Instance.RemoveContainer(t);
		}

#if UNITY_EDITOR
		[MethodAPI]
		[APIDescriptionCN("切换容器")]
		[APIDescriptionEN("ChangeContainer")]
		[APIExampleCode("")]
#endif
		public static void ChangeContainer<T>() where T : PanelContainer
		{
			UIManager.Instance.ChangeContainer<T>();
		}

	
	}
}