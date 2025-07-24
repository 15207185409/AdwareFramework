/****************************************************************************

 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 * THE SOFTWARE.
 ****************************************************************************/

using System;
using System.IO;

namespace XXLFramework
{
    public class ViewControDesignerTemplate
    {
        public static void Write(string name, string scriptsFolder, string scriptNamespace, BindCodeInfo panelCodeInfo,
            CodeGenKitSetting codeGenKitSetting)
        {
            var scriptFile = string.Format(scriptsFolder + "/{0}.Designer.cs",name);

            var writer = File.CreateText(scriptFile);

            var root = new RootCode()
                .Using("UnityEngine")
                .EmptyLine()
                .Namespace(string.IsNullOrWhiteSpace(scriptNamespace)
                    ? codeGenKitSetting.Namespace
                    : scriptNamespace, ns =>
                {
                    ns.Custom(string.Format("// Generate Id:{0}",Guid.NewGuid().ToString()));
                    ns.Class(name, null, true, false, (classScope) =>
                    {
                        foreach (var bindInfo in panelCodeInfo.BindInfos)
                        {
                            classScope.Custom("[SerializeField]");
                            classScope.Custom($"public {bindInfo.TypeName} {bindInfo.MemberName};");
                        }
                    });
                });

            var codeWriter = new FileCodeWriter(writer);
            root.Gen(codeWriter);
            codeWriter.Dispose();
        }
    }
}