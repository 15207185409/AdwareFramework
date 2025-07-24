/****************************************************************************
 * Copyright (c) 2015 - 2022  UNDER MIT License
 * 
 * http://XXLFramework.cn
 * https://github.com//XXLFramework
 * https://gitee.com//XXLFramework
 ****************************************************************************/

#if UNITY_EDITOR
using System.Collections;
using System.IO;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;

namespace XXLFramework
{
    internal class InstallPackageCommand : AbstractCommand
    {
        private readonly PackageRepository mRequestPackageData;

        private void OnProgressChanged(float progress)
        {
			EditorUtility.DisplayProgressBar("插件更新",
				string.Format("插件下载中 {0:P2}", progress), progress);
		}

        public InstallPackageCommand(PackageRepository requestPackageData)
        {
            mRequestPackageData = requestPackageData;
        }

        protected override  void OnExecute()
        {
            var tempFile = "Assets/" + mRequestPackageData.name + ".unitypackage";

            Debug.Log(mRequestPackageData.downloadUrl + ">>>>>>:");

            EditorUtility.DisplayProgressBar("插件更新", "插件下载中 ...", 0.1f);

			EditorHttp.Download(mRequestPackageData.downloadUrl, response =>
			{
				if (response.Type == ResponseType.SUCCEED)
				{
					File.WriteAllBytes(tempFile, response.Bytes);

					EditorUtility.ClearProgressBar();

					AssetDatabase.ImportPackage(tempFile, false);

					File.Delete(tempFile);

					AssetDatabase.Refresh();

					Debug.Log("PackageManager:插件下载成功");


					this.GetModel<LocalPackageVersionModel>()
						.Reload();
				}
				else
				{
					EditorUtility.ClearProgressBar();

					EditorUtility.DisplayDialog(mRequestPackageData.name,
						"插件安装失败," + response.Error + ";", "OK");
				}
			}, OnProgressChanged);
		}

	}
}
#endif