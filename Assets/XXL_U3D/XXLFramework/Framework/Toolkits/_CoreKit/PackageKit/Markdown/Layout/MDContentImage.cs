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
using UnityEngine;

namespace XXLFramework
{
    internal class MDContentImage : MDContent
    {
        public string URL;
        public string Alt;

        public MDContentImage(GUIContent payload, MDStyle style, string link)
            : base(payload, style, link)
        {
        }


        public override void Update(MDContext context, float leftWidth)
        {
            Payload.image = context.FetchImage(URL);
            Payload.text = null;

            if (Payload.image == null)
            {
                context.Apply(Style);
                var text = !string.IsNullOrEmpty(Alt) ? Alt : URL;
                Payload.text = $"[{text}]";
            }

            var size = context.CalcSize(Payload);

            var offset = 40;
            if ((leftWidth - offset) < size.x)
            {
                var aspect = size.y / size.x;
                Location.size = new Vector2(leftWidth - offset, (leftWidth - offset) * aspect);
            }
            else
            {
                Location.size = size;
            }
        }
    }
}
#endif