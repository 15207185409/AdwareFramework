using System;

namespace XXLFramework
{
    public class PanelInfo : IPoolType, IPoolable
    {
        public Type PanelType;
        public string GameObjName;
        public PanelLevel Level = PanelLevel.Common;

        public static PanelInfo Allocate(Type panelType, string gameObjName, PanelLevel level)
        {
            var panelInfo = SafeObjectPool<PanelInfo>.Instance.Allocate();

            panelInfo.PanelType = panelType;
            panelInfo.GameObjName = gameObjName;
            panelInfo.Level = level;
            return panelInfo;
        }

        public void Recycle2Cache()
        {
            SafeObjectPool<PanelInfo>.Instance.Recycle(this);
        }

        public void OnRecycled()
        {
            PanelType = null;
            GameObjName = null;
        }

        public bool IsRecycled { get; set; }
    }
}