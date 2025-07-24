/****************************************************************************
 * Copyright (c) 2015 - 2022  UNDER MIT License
 * 
 * https://xxlframework.cn
 * https://github.com//XXLFramework
 * https://gitee.com//XXLFramework
 ****************************************************************************/

#if UNITY_EDITOR
using UnityEditor;

namespace XXLFramework
{
    internal class PackageMaker : Architecture<PackageMaker>
    {
        protected override void Init()
        {
            
        }

        [MenuItem("Assets/@XPM/Make Folder Package To Folder Same Path")]
        public static void MakeFolderPackageToFolderSamePath()
        {
            var activeObject = Selection.activeObject;
            var assetPath = AssetDatabase.GetAssetPath(activeObject);
            AssetDatabase.ExportPackage(assetPath,
                assetPath.GetFolderPath() + "/" + activeObject.name + ".unitypackage", ExportPackageOptions.Recurse);
            AssetDatabase.Refresh();
        }

        [MenuItem("Assets/@XPM/Make Folder Package To Folder Same Path",true)]
        public static bool MakeFolderPackageToFolderSamePathCheck()
        {
            var activeObject = Selection.activeObject;
            if (!activeObject) return false;
            var assetPath = AssetDatabase.GetAssetPath(activeObject);
            return AssetDatabase.IsValidFolder(assetPath);
        }
    }
}
#endif