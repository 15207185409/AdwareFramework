/****************************************************************************
 * Copyright (c) 2015 - 2022  UNDER MIT License
 * 
 * http://XXLFramework.cn
 * https://github.com//XXLFramework
 * https://gitee.com//XXLFramework
 ****************************************************************************/

using System.Collections.Generic;

namespace XXLFramework
{
    public abstract class IMGUIAbstractLayout : IMGUIAbstractView, IMGUILayout
    {
        protected List<IMGUIView> Children = new List<IMGUIView>();

        public IMGUILayout AddChild(IMGUIView view)
        {
            Children.Add(view);
            view.Parent = this;
            return this;
        }

        public void RemoveChild(IMGUIView view)
        {
            this.PushCommand(() =>
            {
                Children.Remove(view);
                view.Parent = null;
            });

            view.Dispose();
        }

        public void Clear()
        {
            this.Children.ForEach(view =>
            {
                view.Parent = null;
                view.Dispose();
            });

            this.Children.Clear();
        }

        public override void Refresh()
        {
            Children.ForEach(view => view.Refresh());
            base.Refresh();
        }

        protected override void OnGUI()
        {
            OnGUIBegin();

            foreach (var child in Children)
            {
                child.DrawGUI();
            }

            OnGUIEnd();
        }

        protected virtual void OnGUIBegin()
        {
        }

        protected virtual void OnGUIEnd()
        {
        }
    }
    

}