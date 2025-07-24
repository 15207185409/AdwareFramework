namespace XXLFramework
{
	public interface IPanelContainer
    {
        T OpenPanel<T>(PanelLevel level = PanelLevel.Common,IPanelData data = null, bool isReset = false) where T : BasePanel;
        T OpenPanel<T>(string gameObjName, PanelLevel level = PanelLevel.Common, IPanelData data = null, bool isReset = false) where T : BasePanel;
        void ClosePanel(BasePanel panel, bool destroy = false);
        void ClosePanel<T>(string gameObjName = null, bool destroy = false) where T : BasePanel;
        T GetPanel<T>(string gameObjName = null) where T : BasePanel;
        void RemovePanel(BasePanel panel);
    }
}
