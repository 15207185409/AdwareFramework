/****************************************************************************
 * Copyright (c) 2015 - 2022  UNDER MIT License
 * 

 ****************************************************************************/

using System;
using UnityEngine;

namespace XXLFramework
{
    internal class MonoUpdateActionExecutor : MonoBehaviour, IActionExecutor
    {
        private Action mOnUpdate = () => { };
        
        public void Execute(IAction action,Action<IAction> onFinish = null)
        {
            if (action.Status == ActionStatus.Finished) action.Reset();
            if (this.UpdateAction(action, 0, onFinish)) return;

            void OnUpdate()
            {
                if (this.UpdateAction(action,Time.deltaTime,onFinish))
                {
                    mOnUpdate -= OnUpdate;
                }
            }

            mOnUpdate += OnUpdate;
        }

        private void Update()
        {
            mOnUpdate?.Invoke();
        }
    }

    public static class MonoUpdateActionExecutorExtension
    {
        public static IAction ExecuteByUpdate<T>(this T self, IAction action,Action<IAction> onFinish = null) where T : MonoBehaviour
        {
            if (action.Status == ActionStatus.Finished) action.Reset();
            self.gameObject.GetOrAddComponent<MonoUpdateActionExecutor>().Execute(action,onFinish);
            return action;
        }
    }
}