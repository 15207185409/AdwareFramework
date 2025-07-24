/****************************************************************************
 * Copyright (c) 2019 Gwaredd Mountain UNDER MIT License
 * Copyright (c) 2022  UNDER MIT License
 *
 * https://github.com/gwaredd/UnityMarkdownViewer
 * http://XXLFramework.cn
 * https://github.com//XXLFramework
 * https://gitee.com//XXLFramework
 ****************************************************************************/

using UnityEngine;

namespace XXLFramework
{
    internal interface IMDActions
    {
        Texture FetchImage(string url);
        void SelectPage(string url);
    }
}