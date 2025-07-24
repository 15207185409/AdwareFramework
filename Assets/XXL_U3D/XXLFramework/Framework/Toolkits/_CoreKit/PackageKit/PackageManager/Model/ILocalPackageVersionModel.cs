/****************************************************************************
 * Copyright (c) 2015 - 2022  UNDER MIT License
 * 
 * http://XXLFramework.cn
 * https://github.com//XXLFramework
 * https://gitee.com//XXLFramework
 ****************************************************************************/

#if UNITY_EDITOR
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace XXLFramework
{
	internal interface ILocalPackageVersionModel : IModel
	{
		InstalledPackageVersionTable PackageVersionsTable { get; }

		void Reload();

		PackageVersion GetByName(string name);
	}

	public class InstalledPackageVersionTable : Table<PackageVersion>
    {
        public TableIndex<string, PackageVersion> NameIndex =
            new TableIndex<string, PackageVersion>(p => p.Name);

        protected override void OnAdd(PackageVersion item)
        {
            NameIndex.Add(item);
        }

        protected override void OnRemove(PackageVersion item)
        {
            NameIndex.Remove(item);
        }

        protected override void OnClear()
        {
            NameIndex.Clear();
        }

        public override IEnumerator<PackageVersion> GetEnumerator()
        {
            return NameIndex.Dictionary.SelectMany(n => n.Value).GetEnumerator();
        }

        protected override void OnDispose()
        {
            NameIndex.Dispose();
            NameIndex = null;
        }
    }

    public class LocalPackageVersionModel : AbstractModel, ILocalPackageVersionModel
    {
        private InstalledPackageVersionTable packageVersionsTable;
        public InstalledPackageVersionTable PackageVersionsTable { get { return packageVersionsTable; } private set { packageVersionsTable = value; } }

        public LocalPackageVersionModel()
        {
            packageVersionsTable = new InstalledPackageVersionTable();

            Reload();
        }

        public void Reload()
        {
            PackageVersionsTable.Clear();

            foreach (var fileName in AssetDatabase.FindAssets("PackageVersion t:TextAsset")
                         .Select(guid => AssetDatabase.GUIDToAssetPath(guid))
                         .Where(path => path.EndsWith(".json")))
            {
                var text = File.ReadAllText(fileName);
                var packageVersion = JsonUtility.FromJson<PackageVersion>(text);

                PackageVersionsTable.Add(packageVersion);
            }
        }

        public PackageVersion GetByName(string name)
        {
            return PackageVersionsTable.NameIndex.Get(name).FirstOrDefault();
        }

        protected override void OnInit()
        {
        }
    }
}
#endif