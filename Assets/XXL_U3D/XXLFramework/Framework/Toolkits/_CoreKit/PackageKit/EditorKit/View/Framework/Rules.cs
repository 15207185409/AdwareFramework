/****************************************************************************
 * Copyright (c) 2015 - 2022  UNDER MIT License
 * 
 * http://XXLFramework.cn
 * https://github.com//XXLFramework
 * https://gitee.com//XXLFramework
 ****************************************************************************/

using System;
using UnityEngine;

namespace XXLFramework
{
    public interface ICanClick<T>
    {
        T OnClick(Action action);
    }

    public interface IHasRect<T>
    {
        T Rect(Rect rect);
        T Position(Vector2 position);
        T Position(float x, float y);
        T Size(float width, float height);
        T Size(Vector2 size);
        T Width(float width);
        T Height(float height);
    }

    public interface IHasText<T>
    {
        T Text(string labelText);
    }

    public interface IHasTextGetter<T>
    {
        T Text(Func<string> textGetter);
    }
}