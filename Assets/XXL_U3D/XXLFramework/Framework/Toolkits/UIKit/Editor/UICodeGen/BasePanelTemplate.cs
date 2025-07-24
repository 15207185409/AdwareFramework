/****************************************************************************
 * Copyright (c) 2015 ~ 2022  UNDER MIT LICENSE
 * 

 ****************************************************************************/

#if UNITY_EDITOR
using UnityEditor;

namespace XXLFramework
{
    [InitializeOnLoad]
    public class BasePanelTemplate : ICodeGenTemplate
    {
        static BasePanelTemplate()
        {
            CodeGenKit.RegisterTemplate(nameof(BasePanel), new BasePanelTemplate());
        }
        
        public CodeGenTask CreateTask(BindGroup bindGroup)
        {
            var panel = bindGroup.As<BasePanel>();

            return new CodeGenTask()
            {
                IsPanel = true,
                GameObject = panel.gameObject,
                From = GameObjectFrom.Scene,
                ScriptsFolder = panel.ScriptsFolder,
                PrefabFolder = panel.PrefabFolder,
                GeneratePrefab = panel.GeneratePrefab,
                ScriptName = panel.ScriptName,
                Namespace = panel.Namespace
            };
        }
    }
}
#endif