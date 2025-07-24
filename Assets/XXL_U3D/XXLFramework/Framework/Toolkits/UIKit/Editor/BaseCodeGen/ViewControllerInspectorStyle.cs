/****************************************************************************
 * Copyright (c) 2016 ~ 2022  UNDER MIT LICENSE
 * 

 ****************************************************************************/

using System;
using UnityEngine;

namespace XXLFramework
{
    public class ViewControllerInspectorStyle
    {
        public readonly Lazy<GUIStyle> BigTitleStyle = new Lazy<GUIStyle>(() => new GUIStyle(GUI.skin.label)
        {
            fontStyle = FontStyle.Bold,
            fontSize = 15
        });
    }
}