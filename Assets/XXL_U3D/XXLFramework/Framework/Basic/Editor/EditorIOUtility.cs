using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XXLFramework
{
    public class EditorIOUtility 
    {
        public static string GetAssetsPath(string fullPath)
        {
            if (!string.IsNullOrEmpty(fullPath))
            {
                // 将选择的文件夹路径转换为相对于Assets的路径
                string relativePath = "Assets/" + fullPath.Replace(Application.dataPath, "").Replace("\\", "/").TrimStart('/');

                // 输出或处理选择的相对路径
                Debug.Log("Selected folder path (relative): " + relativePath);

                return relativePath;
			}
			else
			{
                Debug.LogError("路径为空:!");
                return "";
            }
             
        }
    }
}