/****************************************************************************
 * Copyright (c) 2020.10  Under Mit License
 * 
 * https://xxlframework.cn
 * https://github.com//XXLFramework
 * https://gitee.com//XXLFramework
 ****************************************************************************/

#if UNITY_EDITOR
using System.IO;
using UnityEditor;
using UnityEngine;

namespace XXLFramework
{
    internal class UpdatePackageCommand : AbstractCommand
    {
        public UpdatePackageCommand(PackageRepository packageRepository, PackageVersion installedVersion)
        {
            mPackageRepository = packageRepository;
            mInstalledVersion = installedVersion;
        }

        private readonly PackageRepository mPackageRepository;
        private readonly PackageVersion mInstalledVersion;

        protected override void OnExecute()
        {
            foreach (var installedVersionIncludeFileOrFolder in mInstalledVersion.IncludeFileOrFolders)
            {
                var path = Application.dataPath.Replace("Assets", installedVersionIncludeFileOrFolder);
                
                if (Directory.Exists(path))
                {
                    Directory.Delete(path, true);
                }
                else if (File.Exists(path))
                {
                    File.Delete(path);
                }
                else if (Directory.Exists(path + "/"))
                {
                    Directory.Delete(path + "/");
                }
            }

            RenderEndCommandExecutor.PushCommand(() =>
            {
                AssetDatabase.Refresh();

                EditorWindow.GetWindow<PackageKitWindow>().Close();

                this.SendCommand(new InstallPackageCommand(mPackageRepository));
            });
        }
    }
}
#endif