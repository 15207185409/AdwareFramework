/****************************************************************************
 * Copyright (c) 2016 ~ 2022  UNDER MIT License
 * 

 ****************************************************************************/

namespace XXLFramework
{
    using System.IO;

    public class UIPanelTemplate
    {
        public static void Write(string name, string srcFilePath, string scriptNamespace,
            UIKitSetting uiKitSettingData)
        {
            var scriptFile = srcFilePath;

            if (File.Exists(scriptFile))
            {
                return;
            }

            var writer = File.CreateText(scriptFile);

            var codeWriter = new FileCodeWriter(writer);


            var rootCode = new RootCode()
                .Using("UnityEngine")
                .Using("UnityEngine.UI")
                .Using("TMPro")
                .EmptyLine()
                .Namespace(scriptNamespace, nsScope =>
                {
                    nsScope.Class(name + "Data", "BasePanelData", false, false, classScope => { });

                    nsScope.Class(name, "BasePanel", true, false, classScope =>
                    {
                        classScope.CustomScope("private void Start()", false,
                           function =>
                           {
                               function.Custom("// please add init code here");
                           });

                        classScope.EmptyLine();
                        classScope.CustomScope("protected override void OnInit(IPanelData uiData = null)", false,
                            function =>
                            {
                                function.Custom($"mData = uiData as {name}Data ?? new {name}Data();");
                                function.Custom("// 当需要用到外部数据初始化Panel时在此初始化");
                            });

                        classScope.EmptyLine();
                        classScope.CustomScope("protected override void OnOpen(IPanelData uiData = null)", false,
                            function => 
                            {
                                function.Custom("if (uiData!=null)");
                                function.Custom("{");
                                function.Custom($"  mData = uiData as {name}Data;");
                                function.Custom("}");
                            });

                        classScope.EmptyLine();
                        classScope.CustomScope("protected override void OnClose(bool destroy)", false,
                            function => { });
                    });
                });

            rootCode.Gen(codeWriter);
            codeWriter.Dispose();
        }
    }
}