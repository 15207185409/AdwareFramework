/****************************************************************************
 * Copyright (c) 2015 - 2022  UNDER MIT License
 * 
 * http://qframework.cn
 * https://github.com//XXLFramework
 * https://gitee.com//XXLFramework
 ****************************************************************************/

#if UNITY_EDITOR
using UnityEngine;

namespace XXLFramework
{
    internal class OpenDetailCommand : AbstractCommand
    {
        private readonly PackageRepository mPackageRepository;

        public OpenDetailCommand(PackageRepository packageRepository)
        {
            mPackageRepository = packageRepository;
        }

        protected override void OnExecute()
        {
            Application.OpenURL("https://xxlframework.cn/package/detail/" + mPackageRepository.id);
        }
    }
}
#endif