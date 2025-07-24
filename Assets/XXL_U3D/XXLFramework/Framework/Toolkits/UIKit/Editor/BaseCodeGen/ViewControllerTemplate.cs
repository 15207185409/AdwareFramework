/****************************************************************************
 * Copyright (c) 2015 ~ 2022  UNDER MIT LICENSE
 * 

 ****************************************************************************/

#if UNITY_EDITOR
using UnityEditor;

namespace XXLFramework
{
    [InitializeOnLoad]
    public class ViewControllerTemplate : ICodeGenTemplate
    {
        static ViewControllerTemplate()
        {
            CodeGenKit.RegisterTemplate(nameof(ViewController), new ViewControllerTemplate());
        }
        
        public CodeGenTask CreateTask(BindGroup bindGroup)
        {
            var viewController = bindGroup.As<ViewController>();

            return new CodeGenTask()
            {
                IsPanel = false,
                GameObject = viewController.gameObject,
                From = GameObjectFrom.Scene,
                ScriptsFolder = viewController.ScriptsFolder,
                PrefabFolder = viewController.PrefabFolder,
                GeneratePrefab = viewController.GeneratePrefab,
                ScriptName = viewController.ScriptName,
                Namespace = viewController.Namespace
            };
        }
    }
}
#endif