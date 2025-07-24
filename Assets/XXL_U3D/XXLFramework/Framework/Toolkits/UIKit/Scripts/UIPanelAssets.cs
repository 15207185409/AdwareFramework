using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace XXLFramework
{
    [CreateAssetMenu(fileName = "UIPanelAssets", menuName = "New UIPanelAssets")]
    public class UIPanelAssets : ScriptableObject
    {

        public List<BasePanel> UIPanels; //UI面板资源

        public BasePanel GetUIPanel(System.Type panelType)
        {
            foreach (var item in UIPanels)
            {
                if (panelType == item.GetType())
                {
                    return item;
                }
            }
            Debug.LogError($"未找到UI面板资源{panelType}");
            return null;
        }

        public BasePanel GetUIPanel(string panelName) 
        {
			foreach (var item in UIPanels)
			{
				if (panelName == item.name)
				{
                    return item;
				}
			}
            Debug.LogError($"未找到UI面板资源{panelName}");
            return null;
        }

    }
}
