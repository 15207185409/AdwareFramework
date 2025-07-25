
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace XXLFramework
{
    [Serializable]
    public class ReleaseItem
    {
        public ReleaseItem()
        {
        }

        public ReleaseItem(string version, string content, string author, DateTime date, string packageId = "")
        {
            this.version = version;
            this.content = content;
            this.author = author;
            this.date = date.ToString("yyyy 年 MM 月 dd 日 HH:mm");
            PackageId = packageId;
        }

        public string version   = "";
        public string content   = "";
        public string author    = "";
        public string date      = "";
        public string PackageId = "";


        public int VersionNumber
        {
            get
            {
                if (string.IsNullOrEmpty(version))
                {
                    return 0;
                }

                var numbersStr = version.Split('.');

                var retNumber = numbersStr[2].ParseToInt();
                retNumber += numbersStr[1].ParseToInt() * 100;
                retNumber += numbersStr[0].ParseToInt() * 10000;

                return retNumber;
            }
        }
    }

    [Serializable]
    public class Readme
    {
        public List<ReleaseItem> items;

        public ReleaseItem GetItem(string version)
        {
            if (items == null || items.Count == 0)
            {
                return null;
            }

            return items.First(s => s.version == version);
        }

        public void AddReleaseNote(ReleaseItem pluginReadme)
        {
            if (items == null)
            {
                items = new List<ReleaseItem> {pluginReadme};
            }
            else
            {
                bool exist = false;
                foreach (var item in items)
                {
                    if (item.version == pluginReadme.version)
                    {
                        item.content = pluginReadme.content;
                        item.author = pluginReadme.author;
                        exist = true;
                        break;
                    }
                }

                if (!exist)
                {
                    items.Add(pluginReadme);
                }
            }
        }
    }


    public enum PackageType
    {
        FrameworkModule, 
        Editor, 
        Runtime, 
        ThirdPlugin
    }

    public enum PackageAccessRight
    {
        Public,
        Private,
    }

    [Serializable]
    public class PackageVersion
    {
        public string Id;

        public string Name
        {
            get
            {
                if (!string.IsNullOrEmpty(InstallPath))
                {
                    var name = InstallPath.Replace("\\", "/");
                    var dirs = name.Split('/');
                    return dirs[dirs.Length - 2];
                }

                return string.Empty;
            }
        }

        public string Version = "0.0.0";

        public PackageType Type;

        public PackageAccessRight AccessRight;

        public int VersionNumber
        {
            get
            {
                var numbersStr = Version.Split('.');

                var retNumber = numbersStr[2].ParseToInt();
                retNumber += numbersStr[1].ParseToInt() * 1000;
                retNumber += numbersStr[0].ParseToInt() * 1000000;

                return retNumber;
            }
        }

        public string DownloadUrl;

        public string InstallPath = "AAssets/XXL_U3D/XXLFramework/FunctionPlugins/";
        
        public List<string> IncludeFileOrFolders = new List<string>();

        public string DocUrl;

        public ReleaseItem Readme = new ReleaseItem();

        public void Save()
        {
            var json = JsonUtility.ToJson(this,true);

            if (!Directory.Exists(InstallPath))
            {
                Directory.CreateDirectory(InstallPath);
            }

            File.WriteAllText(InstallPath + "/PackageVersion.json", json);
        }

        public static PackageVersion Load(string filePath)
        {
            if (filePath.EndsWith("/"))
            {
                filePath += "PackageVersion.json";
            }
            else if (!filePath.EndsWith("PackageVersion.json"))
            {
                filePath += "/PackageVersion.json";
            }

            return JsonUtility.FromJson<PackageVersion>(File.ReadAllText(filePath));
        }
    }

    public static class PackageKitExtension
    {
        /// <summary>
        /// 解析成数字类型
        /// </summary>
        /// <param name="selfStr"></param>
        /// <param name="defaulValue"></param>
        /// <returns></returns>
        public static int ParseToInt(this string selfStr, int defaulValue = 0)
        {
            var retValue = defaulValue;
            return int.TryParse(selfStr, out retValue) ? retValue : defaulValue;
        }
    }
}