/****************************************************************************
 * Copyright (c) 2019 Gwaredd Mountain UNDER MIT License
 * Copyright (c) 2022  UNDER MIT License
 *
 * https://github.com/gwaredd/UnityMarkdownViewer
 * http://XXLFramework.cn
 * https://github.com//XXLFramework
 * https://gitee.com//XXLFramework
 ****************************************************************************/


#if UNITY_EDITOR
#if UNITY_2018_1_OR_NEWER

using UnityEngine;
#if UNITY_2020_2_OR_NEWER
using UnityEditor.AssetImporters;
#else
using UnityEditor.Experimental.AssetImporters;
#endif
using System.IO;

namespace XXLFramework
{
    [ScriptedImporter(1, "markdown")]
    public class MDAssetImporter : ScriptedImporter
    {
        public override void OnImportAsset(AssetImportContext ctx)
        {
            var md = new TextAsset(File.ReadAllText(ctx.assetPath));
            ctx.AddObjectToAsset("main", md);
            ctx.SetMainObject(md);
        }
    }
}

#endif
#endif