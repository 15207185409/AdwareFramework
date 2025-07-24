#if UNITY_EDITOR
using System;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace XXLFramework
{
    public static class UploadPackage
    {
        private static string UPLOAD_URL
        {
            get { return $"{UrlSetting.BaseUrl}/Package/upload"; }
        }

        public static void DoUpload(PackageVersion packageVersion, Action<QFrameworkServerResultFormat<PackageRepository>> onUploadPackage)
        {
            EditorUtility.DisplayProgressBar("插件上传", "打包中...", 0.1f);

            var fileName = $"{packageVersion.Name}.unitypackage";
            var fullPath = ExportPaths(fileName, packageVersion.IncludeFileOrFolders.ToArray());
            var file = File.ReadAllBytes(fullPath);

            var form = new WWWForm();
            form.AddField("UserName", User.Username.Value);
            form.AddField("Password", User.Password.Value);
            form.AddField("PackageName", packageVersion.Name);
            form.AddField("Version", packageVersion.Version);
            form.AddBinaryData("File", file);
            form.AddField("Description", packageVersion.Readme.content);
            form.AddField("InstallPath", packageVersion.InstallPath);

            form.AddField("AccessRight", packageVersion.AccessRight.ToString().ToLower());
            form.AddField("DocUrl", packageVersion.DocUrl);

            form.AddField("Type", packageVersion.Type.ToString());

			Debug.Log(fullPath);

            EditorUtility.DisplayProgressBar("插件上传", "上传中...", 0.2f);

            EditorHttp.Post(UPLOAD_URL, form, (response) =>
            {
                if (response.Type == ResponseType.SUCCEED)
                {
                    EditorUtility.ClearProgressBar();
                    Debug.Log(response.Text);

                    var responseJson =
                       JsonUtility.FromJson<QFrameworkServerResultFormat<PackageRepository>>(response.Text);

                    onUploadPackage(responseJson);


                    File.Delete(fullPath);
                }
                else
                {
                    EditorUtility.ClearProgressBar();
                    EditorUtility.DisplayDialog("插件上传", string.Format("上传失败!{0}", response.Error), "确定");
                    File.Delete(fullPath);
                }
            });
        }

        private static readonly string EXPORT_ROOT_DIR = Path.Combine(Application.dataPath, "../");

        public static string ExportPaths(string exportPackageName, params string[] paths)
        {
            var filePath = Path.Combine(EXPORT_ROOT_DIR, exportPackageName);

            AssetDatabase.ExportPackage(paths, filePath, ExportPackageOptions.Recurse);
            AssetDatabase.Refresh();
            return filePath;
        }
    }
}
#endif