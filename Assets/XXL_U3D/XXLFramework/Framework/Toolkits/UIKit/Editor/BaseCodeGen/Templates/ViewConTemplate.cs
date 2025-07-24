/****************************************************************************
 * Copyright (c) 2016 ~ 2022  UNDER MIT License
 * 

 ****************************************************************************/

namespace XXLFramework
{
    using System.IO;

    public class ViewConTemplate
    {
        public static void Write(string name, string srcFilePath, string scriptNamespace,
            CodeGenKitSetting codeGenKitSetting)
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
                .EmptyLine()
                .Namespace(scriptNamespace, nsScope =>
                {
                    nsScope.Class(name, "ViewController", true, false, classScope =>
                    {
                        classScope.CustomScope("private void Start()", false,
                           function =>
                           {
                               function.Custom("// please add init code here");
                           });

                        classScope.EmptyLine();
                    });
                });

            rootCode.Gen(codeWriter);
            codeWriter.Dispose();
        }
    }
}