/****************************************************************************
 * Copyright (c) 2016 ~ 2022  UNDER MIT LICENSE
 * 

 ****************************************************************************/

using System.Collections.Generic;

#if UNITY_EDITOR
namespace XXLFramework
{
	public class CodeGenKit : Architecture<CodeGenKit>
	{
		private static readonly Dictionary<string, ICodeGenTemplate> mTemplates = new Dictionary<string, ICodeGenTemplate>();
		public static void RegisterTemplate(string templateName, ICodeGenTemplate codeGenTemplate)
		{
			if (mTemplates.ContainsKey(templateName))
			{
				mTemplates[templateName] = codeGenTemplate;
			}
			else
			{
				mTemplates.Add(templateName, codeGenTemplate);
			}
		}

		public static ICodeGenTemplate GetTemplate(string templateName)
		{
			return mTemplates.TryGetValue(templateName, out var template) ? template : null;
		}

		protected override void Init()
		{

		}

		public static void Generate(BindGroup bindGroup)
		{
			var task = GetTemplate(bindGroup.TemplateName).CreateTask(bindGroup);
			Generate(task);
		}

		public static void Generate(CodeGenTask task)
		{
			CodeGenKitPipeline.Default.Generate(task);
		}

		private static CodeGenKitSetting setting;
		public static CodeGenKitSetting Setting
		{
			get
			{
				if (setting == null)
				{
					setting = CodeGenKitSetting.Load();
				}
				return setting;
			}
			private set { }
		}
	}
}
#endif