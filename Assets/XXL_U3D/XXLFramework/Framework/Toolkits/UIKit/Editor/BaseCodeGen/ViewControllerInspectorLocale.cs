/****************************************************************************
 * Copyright (c) 2015 ~ 2022  UNDER MIT LICENSE
 * 

 ****************************************************************************/

#if UNITY_EDITOR
namespace XXLFramework
{
    public class ViewControllerInspectorLocale
    {
        public bool CN
        {
            get => LocaleKitEditor.IsCN.Value;
            set => LocaleKitEditor.IsCN.Value = value;
        }

        public string CodegenPart => CN ? " 代码生成设置" : " Code Generate Setting";
        public string DragBindDescription =>
           CN ? "请将要绑定的物体拖到下边区域" : "drag sceneObj below";
        public string BindInfo => CN ? "绑定信息" : "BindInfo";
        public string Namespace => CN ? "命名空间:" : "Namespace :";
        public string ScriptName => CN ? "生成脚本名:" : "Script name:";
        public string ScriptsFolder => CN ? "脚本生成目录:" : "Scripts Generate Folder:";
        public string GeneratePrefab => CN ? "生成 Prefab" : "Generate Prefab";
        public string PrefabGenerateFolder => CN ? "Prefab 生成目录:" : "Prefab Generate Folder:";
        public string OpenScript => CN ? " 打开脚本" : "Open Script File";
        public string SelectScript => CN ? " 选择脚本" : "Select Script File";
        public string Generate => CN ? " 生成代码" : " Generate Code";

        public string DragDescription =>
            CN ? "请将要生成脚本的文件夹拖到下边区域 或 自行填写目录到上一栏中" : "drag file or folder below or write in above";
        
        public string PrefabDragDescription =>
            CN ? "请将要生成 Prefab 的文件夹拖到下边区域 或 自行填写目录到上一栏中" : "drag file or folder below or write in above";
    }
}
#endif