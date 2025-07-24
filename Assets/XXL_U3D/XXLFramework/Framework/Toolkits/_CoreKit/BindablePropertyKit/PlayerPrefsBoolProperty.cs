/****************************************************************************
 * Copyright (c) 2016 - 2022  UNDER MIT License
 * 

 ****************************************************************************/

using UnityEngine;

namespace XXLFramework
{
    public class PlayerPrefsBooleanProperty : BindableProperty<bool>
    {
        public PlayerPrefsBooleanProperty(string saveKey, bool defaultValue = false)
        {
            mValue = PlayerPrefs.GetInt(saveKey, defaultValue ? 1 : 0) == 1;

            this.Register(value => PlayerPrefs.SetInt(saveKey, value ? 1 : 0));
        }
    }
}