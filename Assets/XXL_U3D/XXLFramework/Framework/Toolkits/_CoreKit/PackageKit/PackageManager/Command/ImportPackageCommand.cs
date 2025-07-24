/****************************************************************************
 * Copyright (c) 2015 - 2022  UNDER MIT License
 * 
 * http://XXLFramework.cn
 * https://github.com//XXLFramework
 * https://gitee.com//XXLFramework
 ****************************************************************************/

#if UNITY_EDITOR
using UnityEditor;

namespace XXLFramework
{
    internal class ImportPackageCommand : AbstractCommand
    {
        private readonly PackageRepository mPackageRepository;

        public ImportPackageCommand(PackageRepository packageRepository)
        {
            mPackageRepository = packageRepository;
        }

        protected override void OnExecute()
        {
            EditorWindow.GetWindow<PackageKitWindow>().Close();

            this.SendCommand(new InstallPackageCommand(mPackageRepository));
        }
    }
}
#endif