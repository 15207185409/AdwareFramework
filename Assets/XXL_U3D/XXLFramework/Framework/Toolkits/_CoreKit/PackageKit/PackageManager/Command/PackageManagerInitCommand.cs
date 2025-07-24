/****************************************************************************
 * Copyright (c) 2015 - 2022  UNDER MIT License
 * 
 * http://XXLFramework.cn
 * https://github.com//XXLFramework
 * https://gitee.com//XXLFramework
 ****************************************************************************/

#if UNITY_EDITOR
using System.Linq;
using UnityEditor;

namespace XXLFramework
{
    internal class PackageManagerInitCommand : AbstractCommand
    {
        protected override void OnExecute()
        {
            var model = this.GetModel<PackageManagerModel>();
            var server = this.GetModel<PackageManagerServer>();
            var installedPackageVersionsModel = this.GetModel<LocalPackageVersionModel>();
            installedPackageVersionsModel.Reload();
            
            PackageManagerState.PackageRepositories.Value = model.Repositories.OrderBy(p => p.name).ToList();
            this.SendCommand<UpdateCategoriesFromModelCommand>();
            
            server.GetAllRemotePackageInfoV5((list, categories) =>
            {
                if (list != null && categories != null)
                {
                    model.Repositories = list.OrderBy(p => p.name).ToList();
                    PackageManagerState.PackageRepositories.Value = model.Repositories;
                    this.SendCommand<UpdateCategoriesFromModelCommand>();
                }
                else
                {
                    EditorUtility.DisplayDialog("服务器请求失败", "请检查网络或排查问题", "确定");
                }
            });
        }
    }
}
#endif