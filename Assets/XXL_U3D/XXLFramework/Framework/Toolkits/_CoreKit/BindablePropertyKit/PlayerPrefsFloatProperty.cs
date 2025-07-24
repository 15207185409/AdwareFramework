/****************************************************************************
 * Copyright (c) 2016 - 2022  UNDER MIT License
 * 

 ****************************************************************************/

using UnityEngine;

namespace XXLFramework
{
    public class PlayerPrefsFloatProperty : BindableProperty<float>
    {
        public PlayerPrefsFloatProperty(string saveKey, float defaultValue = 0.0f)
        {
            mValue =  PlayerPrefs.GetFloat(saveKey, defaultValue);

            this.Register(value => PlayerPrefs.SetFloat(saveKey, value));
        }
    }
}