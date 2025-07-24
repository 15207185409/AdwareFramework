/****************************************************************************
 * Copyright (c) 2015 ~ 2022  UNDER MIT License
 *
 * http://qframework.cn
 * https://github.com//XXLFramework
 * https://gitee.com//XXLFramework
 ****************************************************************************/

#if UNITY_EDITOR
namespace XXLFramework
{
    internal class UrlHelper
    {
        public static string PackageUrl(PackageRepository p)
        {
            return "https://xxlframework.cn/package/detail/" + p.name;
        }
    }
}
#endif