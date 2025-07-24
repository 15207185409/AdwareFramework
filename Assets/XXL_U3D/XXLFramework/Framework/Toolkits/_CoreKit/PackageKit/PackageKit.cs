/****************************************************************************
 * Copyright (c) 2015 - 2022  UNDER MIT License
 * 
 * http://XXLFramework.cn
 * https://github.com//XXLFramework
 * https://gitee.com//XXLFramework
 ****************************************************************************/

#if UNITY_EDITOR
namespace XXLFramework
{
    public class PackageKit : Architecture<PackageKit>
    {

        protected override void Init()
        {

            // 包类型
            RegisterModel<PackageTypeConfigModel>(new PackageTypeConfigModel());
            
            // 已安装类型
            RegisterModel<LocalPackageVersionModel>(new LocalPackageVersionModel());
            RegisterModel<PackageManagerModel>(new PackageManagerModel());
            RegisterModel<PackageManagerServer>(new PackageManagerServer());
        }
    }
}
#endif