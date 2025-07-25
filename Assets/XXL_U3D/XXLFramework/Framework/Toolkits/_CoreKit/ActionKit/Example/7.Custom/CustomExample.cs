using UnityEngine;

namespace XXLFramework.Example
{
    public class CustomExample : MonoBehaviour
    {
        class SomeData
        {
            public int ExecuteCount = 0;
        }

        private void Start()
        {
            ActionKit.Custom(a =>
            {
                a
                    .OnStart(() => { Debug.Log("OnStart"); })
                    .OnExecute(dt =>
                    {
                        Debug.Log("OnExecute");

                        a.Finish();
                    })
                    .OnFinish(() => { Debug.Log("OnFinish"); });
            }).Start(this);
            
            // OnStart
            // OnExecute
            // OnFinish

            ActionKit.Custom<SomeData>(a =>
                {
                    a
                        .OnStart(() =>
                        {
                            a.Data = new SomeData()
                            {
                                ExecuteCount = 0
                            };
                        })
                        .OnExecute(dt =>
                        {
                            Debug.Log(a.Data.ExecuteCount);
                            a.Data.ExecuteCount++;

                            if (a.Data.ExecuteCount >= 5)
                            {
                                a.Finish();
                            }
                        }).OnFinish(() => { Debug.Log("Finished"); });
                })
                .Start(this);
            
            // 0
            // 1
            // 2
            // 3
            // 4
            // Finished

            // 还支持 Sequence、Repeat、Spawn 等
            // Also support sequence repeat spawn
            // ActionKit.Sequence()
            //     .Custom(c =>
            //     {
            //         c.OnStart(() => c.Finish());
            //     }).Start(this);
        }
    }
}