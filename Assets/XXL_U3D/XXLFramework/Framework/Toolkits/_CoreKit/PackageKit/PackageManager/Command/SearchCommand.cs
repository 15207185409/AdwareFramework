/****************************************************************************
 * Copyright (c) 2015 - 2022  UNDER MIT License
 * 
 * http://XXLFramework.cn
 * https://github.com//XXLFramework
 * https://gitee.com//XXLFramework
 ****************************************************************************/

#if UNITY_EDITOR
using System.Linq;

namespace XXLFramework
{
    internal class SearchCommand : AbstractCommand
    {
        private readonly string mKey;
        
        public SearchCommand(string key)
        {
            mKey = key.ToLower();
        }

        protected override void OnExecute()
        {
            var model = this.GetModel<PackageManagerModel>();
            var categoryIndex = PackageManagerState.CategoryIndex.Value;
            var categories = PackageManagerState.Categories.Value;
            var accessRightIndex = PackageManagerState.AccessRightIndex.Value;
            
            var repositories = model
                .Repositories
                .Where(p => p.name.ToLower().Contains(mKey))
                .Where(p=>categoryIndex == 0 || p.type.ToString() == categories[categoryIndex])
                .Where(p=>accessRightIndex == 0 || 
                          accessRightIndex == 1 && p.accessRight == "public" ||
                          accessRightIndex == 2 && p.accessRight == "private"
                )
                .OrderBy(p=>p.name)
                .ToList();

            PackageManagerState.PackageRepositories.Value = repositories;
        }
    }
}
#endif