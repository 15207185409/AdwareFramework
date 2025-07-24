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
using System.Collections.Generic;
using System.IO;

namespace XXLFramework
{
	public class UIPanelDesignerTemplate
    {
        public static void Write(string name, string scriptsFolder, string scriptNamespace, BindCodeInfo panelCodeInfo,
            UIKitSetting uiKitSettingData)
        {
            var scriptFile = string.Format(scriptsFolder + "/{0}.Designer.cs",name);

            var writer = File.CreateText(scriptFile);

            var root = new RootCode()
                .Using("UnityEngine")
                .Using("UnityEngine.UI")
                .EmptyLine()
                .Namespace(string.IsNullOrWhiteSpace(scriptNamespace)
                    ? uiKitSettingData.Namespace
                    : scriptNamespace, ns =>
                {
                    ns.Custom(string.Format("// Generate Id:{0}",Guid.NewGuid().ToString()));
                    ns.Class(name, null, true, false, (classScope) =>
                    {
                        classScope.Custom("public const string Name = \"" + name + "\";");
                        classScope.EmptyLine();

                        foreach (var bindInfo in panelCodeInfo.BindInfos)
                        {
                            classScope.Custom("[SerializeField]");
                            classScope.Custom($"public {bindInfo.TypeName} {bindInfo.MemberName};");
                        }

                        classScope.EmptyLine();

                        classScope.Custom("private " + name + "Data mData = null;");

                        classScope.EmptyLine();

                        classScope.CustomScope("public " + name + "Data Data", false,
                            (property) =>
                            {
                                property.CustomScope("get", false, (getter) => { getter.Custom("return mData;"); });
                            });

                        classScope.EmptyLine();

						//foreach (var item in panelCodeInfo.BindInfos)
						//{
      //                      UnityEngine.Debug.Log($"{item.MemberName},{item.TypeName}");
      //                  }
                        
                        classScope.CustomScope("protected override void RecordInitValue()", false,
                            function =>
                            {
                                List<string> initValues = GetInitValues(panelCodeInfo.BindInfos);
								foreach (var item in initValues)
								{
                                    function.Custom(item);
                                }
                            });

                        classScope.CustomScope("protected override void Reset()", false,
                            function =>
                            {
                                List<string> resetValues = GetResetValues(panelCodeInfo.BindInfos);
                                foreach (var item in resetValues)
                                {
                                    function.Custom(item);
                                }
                            });

                    });
                });

            var codeWriter = new FileCodeWriter(writer);
            root.Gen(codeWriter);
            codeWriter.Dispose();
        }



		private static List<string> GetInitValues(List<BindInfo> bindInfos)
		{
            List<string> result = new List<string>();
            foreach (var bindInfo in bindInfos)
            {
                //tmp
				if (bindInfo.TypeName == "TMP.TextMeshProUGUI")
				{
                    result.Add($"InitValues.Add(\"{bindInfo.MemberName}\", {bindInfo.MemberName}.GetComponent<TMP.TextMeshProUGUI>().text);");
                }
                else if (bindInfo.TypeName == "TMPro.TextMeshProUGUI")
                {
                    result.Add($"InitValues.Add(\"{bindInfo.MemberName}\", {bindInfo.MemberName}.GetComponent<TMPro.TextMeshProUGUI>().text);");
                }
                else if (bindInfo.TypeName == "TMPro.TextMeshPro")
                {
                    result.Add($"InitValues.Add(\"{bindInfo.MemberName}\", {bindInfo.MemberName}.GetComponent<TMPro.TextMeshPro>().text);");
                }
                else if (bindInfo.TypeName == "TMPro.TMP_InputField")
                {
                    result.Add($"InitValues.Add(\"{bindInfo.MemberName}\", {bindInfo.MemberName}.GetComponent<TMPro.TMP_InputField>().text);");
                }
                else if (bindInfo.TypeName == "TMPro.TMP_Dropdown")
                {
                    result.Add($"InitValues.Add(\"{bindInfo.MemberName}\", {bindInfo.MemberName}.GetComponent<TMPro.TMP_Dropdown>().value.ToString());");
                }
                //ugui
                else if (bindInfo.TypeName == "UnityEngine.UI.Text")
                {
                    result.Add($"InitValues.Add(\"{bindInfo.MemberName}\", {bindInfo.MemberName}.GetComponent<Text>().text);");
                }
                else if (bindInfo.TypeName == "UnityEngine.UI.Dropdown")
                {
                    result.Add($"InitValues.Add(\"{bindInfo.MemberName}\", {bindInfo.MemberName}.GetComponent<Dropdown>().value.ToString());");
                }
                else if (bindInfo.TypeName == "UnityEngine.UI.Toggle")
                {
                    result.Add($"InitValues.Add(\"{bindInfo.MemberName}\", {bindInfo.MemberName}.GetComponent<Toggle>().isOn.ToString());");
                }
                else if (bindInfo.TypeName == "UnityEngine.UI.Slider")
                {
                    result.Add($"InitValues.Add(\"{bindInfo.MemberName}\", {bindInfo.MemberName}.GetComponent<Slider>().value.ToString());");
                }
            }

            return result;
        }

        private static List<string> GetResetValues(List<BindInfo> bindInfos)
        {
            List<string> result = new List<string>();
            foreach (var bindInfo in bindInfos)
            {
                if (bindInfo.TypeName == "TMP.TextMeshProUGUI" || bindInfo.TypeName == "TMPro.TextMeshProUGUI" ||
                    bindInfo.TypeName == "TMPro.TextMeshPro" || bindInfo.TypeName == "TMPro.TMP_InputField" ||
                    bindInfo.TypeName == "UnityEngine.UI.Text")
                {
                    result.Add($"{bindInfo.MemberName}.text = InitValues[\"{bindInfo.MemberName}\"];");
                }
                else if (bindInfo.TypeName == "TMPro.TMP_Dropdown" || bindInfo.TypeName == "UnityEngine.UI.Dropdown")
                {
                    result.Add($"{bindInfo.MemberName}.value = int.Parse(InitValues[\"{bindInfo.MemberName}\"]);");
                }
                else if (bindInfo.TypeName == "UnityEngine.UI.Toggle")
                {
                    result.Add($"{bindInfo.MemberName}.isOn = bool.Parse(InitValues[\"{bindInfo.MemberName}\"]);");
                }
                else if (bindInfo.TypeName == "UnityEngine.UI.Slider")
                {
                    result.Add($"{bindInfo.MemberName}.value = float.Parse(InitValues[\"{bindInfo.MemberName}\"]);");
                }
            }

            return result;
        }
    }
}