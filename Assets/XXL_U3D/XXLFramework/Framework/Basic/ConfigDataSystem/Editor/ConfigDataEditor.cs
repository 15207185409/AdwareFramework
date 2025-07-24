using Newtonsoft.Json.Linq;
using OfficeOpenXml;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace XXLFramework
{
	public class ConfigDataEditor
	{
		public ConfigDataEditor()
		{
			configPath = ConfigSettingData.Load();
			ExcelPath = configPath.ExcelPath;
			ScriptsPath = configPath.ScriptsPath;
		}

		public static ConfigSettingData configPath;


		[FolderPath(ParentFolder = "")]
		public string ExcelPath;

		[FolderPath(ParentFolder = "")]
		public string ScriptsPath;


		[Sirenix.OdinInspector.Button("生成资源")]
		public void GenerateAsset()
		{
			if (!string.IsNullOrEmpty(ExcelPath))
			{
				configPath.ExcelPath = ExcelPath;
			}
			if (!string.IsNullOrEmpty(ScriptsPath))
			{
				configPath.ScriptsPath = ScriptsPath;
			}
			ReadExcel();
		}

		[Sirenix.OdinInspector.Button("保存设置")]
		public void SaveSetting()
		{
			if (!string.IsNullOrEmpty(ExcelPath))
			{
				configPath.ExcelPath = ExcelPath;
			}
			if (!string.IsNullOrEmpty(ScriptsPath))
			{
				configPath.ScriptsPath = ScriptsPath;
			}
			configPath.Save();
		}

		[Sirenix.OdinInspector.Button("删除所有Json文件")]
		public static void ClearAllJsonFiles()
		{
			string fullPath = $"{Application.streamingAssetsPath}/{configPath.JsonPath}";
			if (Directory.Exists(fullPath))
			{
				DirectoryInfo direction = new DirectoryInfo(fullPath);
				FileInfo[] files = direction.GetFiles("*", SearchOption.AllDirectories);

				Debug.Log(files.Length);

				for (int i = 0; i < files.Length; i++)
				{
					if (files[i].Name.EndsWith(".meta"))
					{
						continue;
					}

					string FilePath = fullPath + "/" + files[i].Name;
					File.Delete(FilePath);
				}
			}
			AssetDatabase.Refresh();
			Debug.Log("Json文件清空完成");
		}


		public void ReadExcel()
		{
			//dataList.Clear();
			configPath.ExcelPath = configPath.ExcelPath.Replace('\\', '/');
			if (Directory.Exists(configPath.ExcelPath))
			{
				//获取指定目录下所有的文件
				DirectoryInfo direction = new DirectoryInfo(configPath.ExcelPath);
				FileInfo[] files = direction.GetFiles("*", SearchOption.AllDirectories);
				Debug.Log("fileCount:" + files.Length);
				List<string> classNames = new List<string>();
				for (int i = 0; i < files.Length; i++)
				{
					if (files[i].Name.EndsWith(".meta") || !files[i].Name.EndsWith(".xlsx") ||
						files[i].Name.StartsWith("~$"))
					{
						continue;
					}

					Debug.Log("FullName:" + files[i].FullName);

					LoadData(files[i].FullName, ref classNames);
				}
				CreatConfigManager(classNames);
			}
			else
			{
				Debug.LogError("ReadExcel configPath not Exists!");
			}
		}


		/// <summary>
		/// 读取表格并保存脚本及json
		/// </summary>
		static void LoadData(string filePath, ref List<string> classNames)
		{
			//ClearAllJsonFiles();
			Debug.Log("json路径:" + filePath);
			FileInfo fileInfo = new FileInfo(filePath);
			//指定EPPlus使用非商业证书
			ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
			ExcelPackage excelPackage = new ExcelPackage(fileInfo);
			ExcelWorksheets workSheets = excelPackage.Workbook.Worksheets;
			foreach (var item in workSheets)
			{
				classNames.Add(item.Name);
				CreateTemplate(item);
				CreateJson(item, workSheets);
			}
			AssetDatabase.Refresh();
			Debug.Log("创建资源完成");
		}

		/// <summary>
		/// 生成实体类模板
		/// </summary>
		static void CreateTemplate(ExcelWorksheet worksheet)
		{
			if (!Directory.Exists(configPath.ScriptsPath))
			{
				Directory.CreateDirectory(configPath.ScriptsPath);
			}

			string field = "";
			for (int i = 1; i <= worksheet.Dimension.Columns; i++)
			{
				string typeStr = GetValue(worksheet, 2, i); 
				if (JudgeIsCommen(typeStr))
				{
					typeStr = typeStr.ToLower();
				}
				else
				{
					typeStr += "Data";
				}


				if (typeStr.Contains("[]"))
				{
					typeStr = typeStr.Replace("[]", "");
					typeStr = string.Format(" List<{0}>", typeStr);
				}

				string nameStr = GetValue(worksheet, 3, i); 
				if (string.IsNullOrEmpty(typeStr) || string.IsNullOrEmpty(nameStr)) continue;
				field += $"\t/// <summary> {worksheet.Cells[1, i].Value} </summary>\r";
				field += "\tpublic " + typeStr + " " + nameStr + " { get; set; }\r";
			}

			string fileName = worksheet.Name;
			//string[] tmpName = fileName.Split('-');
			//string tmp = tmpName[0];
			string tempStr = CreateUtilityClassTemplate(fileName, field, configPath.JsonPath);

			File.WriteAllText($"{configPath.ScriptsPath}/{fileName}.cs", tempStr);
			Debug.Log("生成实体类：" + fileName);

		}

		private static bool JudgeIsCommen(string typeStr)
		{
			string typeStrToLower = typeStr.ToLower();
			if (typeStrToLower == "string" || typeStrToLower == "int" || typeStrToLower == "float" || typeStrToLower == "bool" ||
				typeStrToLower == "string[]" || typeStrToLower == "int[]" || typeStrToLower == "float[]" || typeStrToLower == "bool[]")
			{
				return true;
			}
			else
			{
				return false;
			}
		}

		private static string CreateUtilityClassTemplate(string className, string propertys, string jsonPath)
		{
			StringBuilder result = new StringBuilder();
			result.AppendLine("using System.Collections.Generic;");
			result.AppendLine("using Newtonsoft.Json;");
			result.AppendLine("using UnityEngine;");
			result.AppendLine("using Cysharp.Threading.Tasks;\n");
			result.AppendLine("namespace XXLFramework{");
			result.AppendLine("\t[System.Serializable]");
			result.AppendLine($"\tpublic class {className}Data{{");
			result.AppendLine($"\t\t{propertys}");
			result.AppendLine("\t}\n");

			result.AppendLine($"\tpublic class {className}");
			result.AppendLine("\t{");
			result.AppendLine(
				$"\t\tpublic static string JsonPath = Application.streamingAssetsPath +  \"{jsonPath}/{className}.json\";");
			result.AppendLine($"\t\tpublic static {className} config {{ get; set; }}");
			result.AppendLine("\t\tpublic string version { get; set; }");
			result.AppendLine($"\t\tpublic List<{className}Data> datas {{ get; set; }}");
			result.AppendLine($"\t\tpublic static async UniTask Init()");
			result.AppendLine("\t\t{");
			result.AppendLine($"\t\t\tstring json = await Utility.GetJsonTxt(JsonPath);");
			result.AppendLine($"\t\t\tif (json.IsNotNullAndEmpty()) ");
			result.AppendLine("\t\t\t{");
			result.AppendLine($"\t\t\t\tconfig = JsonConvert.DeserializeObject<{className}>(json);");
			result.AppendLine("\t\t\t}");
			result.AppendLine("\t\t}\n");
			result.AppendLine($"\t\tpublic static {className}Data Get(int id){{");
			result.AppendLine($"\t\t\tforeach (var item in config.datas){{");
			result.AppendLine($"\t\t\t\tif (item.ID == id){{");
			result.AppendLine($"\t\t\t\t\treturn item;");
			result.AppendLine($"\t\t\t\t}}");
			result.AppendLine($"\t\t\t}}");
			result.AppendLine($"\t\t\treturn null;");
			result.AppendLine($"\t\t}}");
			result.AppendLine($"\t}}");
			result.AppendLine("}");

			return result.ToString();
		}
		
		/// <summary>
		/// 生成管理模板类
		/// </summary>
		/// <param name="ClassNameList"></param>
		static void CreatConfigManager(List<string> ClassNameList)
		{
			if (!Directory.Exists(configPath.ScriptsPath))
			{
				Directory.CreateDirectory(configPath.ScriptsPath);
			}

			//string className = "ConfigDataManager";
			
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("using Cysharp.Threading.Tasks;\n");
			stringBuilder.AppendLine("namespace XXLFramework");
			stringBuilder.AppendLine("{");
			stringBuilder.AppendLine("\tpublic class ConfigDataManager{");
			stringBuilder.AppendLine("\t\tpublic static async UniTask InitConfig(){");
			for (int i = 0; i < ClassNameList.Count; i++)
			{
				stringBuilder.AppendLine($"\t\t\t await {ClassNameList[i]}.Init();");
			}
			stringBuilder.AppendLine("\t\t}");
			stringBuilder.AppendLine("\t}");
			stringBuilder.AppendLine("}");

			File.WriteAllText($"{configPath.ScriptsPath}/ConfigDataManager.cs", stringBuilder.ToString());
		}

		/// <summary>
		/// 生成json文件
		/// </summary>
		static void CreateJson(ExcelWorksheet worksheet, ExcelWorksheets workSheets)
		{
			JArray array = CreateJArray(worksheet, workSheets);

			JObject o = new JObject();
			o["datas"] = array;
			o["version"] = DateTime.Now.ToString("yyyyMMdd");
			string fileName = worksheet.Name;
			string fullConfigPath = $"{Application.streamingAssetsPath}/{configPath.JsonPath}";
			if (!Directory.Exists(fullConfigPath))
			{
				Directory.CreateDirectory(fullConfigPath);
			}

			File.WriteAllText($"{fullConfigPath}/{fileName}.json", o.ToString());
		}

		private static JArray CreateJArray(ExcelWorksheet worksheet, ExcelWorksheets workSheets)
		{
			JArray array = new JArray();

			// 获取表格有多少列 
			int columns = worksheet.Dimension.Columns;
			// 获取表格有多少行 
			int rows = worksheet.Dimension.Rows;

			//第一行为表头，第二行为类型 ，第三行为字段名 不读取
			for (int i = 4; i <= rows; i++)
			{
				List<TableData> dataList = new List<TableData>();
				for (int j = 1; j <= columns; j++)
				{
					// 获取表格中指定行指定列的数据 
					string value = GetValue(worksheet, i, j); 

					TableData tempData = new TableData(worksheet.Name, i, j, GetValue(worksheet,2,j), 
						GetValue(worksheet,3,j), value);

					dataList.Add(tempData);
				}

				if (dataList != null && dataList.Count > 0)
				{
					JObject jObjItem = GetJObject(dataList, workSheets);

					if (jObjItem != null)
					{
						array.Add(jObjItem);
					}
				}
			}

			return array;
		}

		private static string GetValue(ExcelWorksheet worksheet,int row, int col)
		{
			object result = worksheet.Cells[row, col].Value;
			if(result == null)
			{
				Debug.LogWarning($"{worksheet.Name},第{row}行第{col}列为空");
				return string.Empty;
			}
			else
			{
				return result.ToString();
			}
		}

		private static JObject GetJObject(List<TableData> dataList, ExcelWorksheets worksheets)
		{
			JObject result = new JObject();
			foreach (var item in dataList)
			{
				if (item.Type.Contains("[]")) 
				{
					JArray jArray = new JArray();

					if (item.Value.IsNullOrEmpty())
					{
						result[item.FieldName] = jArray;
						continue;
					}
				}
				try
				{					
					if (JudgeIsCommen(item.Type))
					{
						switch (item.Type)
						{
							case "string":
								result[item.FieldName] = GetValue<string>(item.Value);
								break;
							case "int":
								result[item.FieldName] = GetValue<int>(item.Value);
								break;
							case "float":
								result[item.FieldName] = GetValue<float>(item.Value);
								break;
							case "bool":
								result[item.FieldName] = GetValue<bool>(item.Value);
								break;
							case "string[]":
								result[item.FieldName] = new JArray(GetList<string>(item.Value, '$'));
								break;
							case "int[]":
								result[item.FieldName] = new JArray(GetList<int>(item.Value, '$'));
								break;
							case "float[]":
								result[item.FieldName] = new JArray(GetList<float>(item.Value, '$'));
								break;
							case "bool[]":
								result[item.FieldName] = new JArray(GetList<bool>(item.Value, '$'));
								break;
						}
					}
					else
					{
						Debug.Log(item.Type);
						ExcelWorksheet worksheet = GetWorkSheet(item.Type, worksheets);

						if (item.Type.Contains("[]"))
						{
							JArray jArray = new JArray();

							if (item.Value.IsNullOrEmpty())
							{
								result[item.FieldName] = jArray;
								continue;
							}
							else
							{
								string[] idList = item.Value.Split('$');

								if (idList == null)
								{
									result[item.FieldName] = jArray;
									continue;
								}

								foreach (var id in idList)
								{
									int rowIndex = 0;
									for (int j = 1; j <= worksheet.Dimension.Rows; j++)
									{
										if (id == GetValue(worksheet, j, 1))
										{
											rowIndex = j;
											break;
										}
									}
									List<TableData> tableDatas = new List<TableData>();
									for (int k = 1; k <= worksheet.Dimension.Columns; k++)
									{
										TableData tempData = new TableData(worksheet.Name, rowIndex, k, GetValue(worksheet, 2, k), GetValue(worksheet, 3, k), GetValue(worksheet, rowIndex, k));

										tableDatas.Add(tempData);
									}
									JObject obj = GetJObject(tableDatas, worksheets);
									jArray.Add(obj);
								}
								result[item.FieldName] = jArray;
							}
					
						}
						else
						{
							if (item.Value == null)
							{
								result[item.FieldName] = new JObject();
								continue;
							}

							int rowIndex = 0;
							for (int j = 1; j <= worksheet.Dimension.Rows; j++)
							{
								if (item.Value == GetValue(worksheet,j,1))
								{
									rowIndex = j;
									break;
								}
							}

							List<TableData> tableDatas = new List<TableData>();

							for (int k = 1; k <= worksheet.Dimension.Columns; k++)
							{
								TableData tempData = new TableData();								
								tempData.FieldName = GetValue(worksheet, 3, k);
								tempData.Type = GetValue(worksheet, 2, k);
								tempData.Value = GetValue(worksheet, rowIndex, k);
								tempData.ColumnIndex = k;

								tableDatas.Add(tempData);
							}
							result[item.FieldName] = GetJObject(tableDatas, worksheets);
						}
					}
				}
				catch (Exception)
				{
					Debug.LogError(item.ToString());
					throw;
				}
			}
			return result;
		}

		private static ExcelWorksheet GetWorkSheet(string workSheetName, ExcelWorksheets excelWorksheets)
		{
			string name = TrimBracket(workSheetName);
			foreach (var item in excelWorksheets)
			{
				if (item.Name == name)
				{
					return item;
				}
			}
			Debug.LogError($"未找到表{name}");
			return null;
		}

		private static string TrimBracket(string value)
		{
			string result = value.Replace("[]", "");
			return result;
		}


		/// <summary>
		/// 字符串拆分列表
		/// </summary>
		static List<T> GetList<T>(string str, char spliteChar)
		{
			string[] ss = str.Split(spliteChar);
			int length = ss.Length;
			List<T> arry = new List<T>(ss.Length);
			for (int i = 0; i < length; i++)
			{
				try
				{
					arry.Add(GetValue<T>(ss[i]));
				}
				catch (Exception e)
				{
					Debug.Log($"当前错误的字符为第{i}个,错误字符为{ss[i]}");
					throw;
				}
			}

			return arry;
		}

		static T GetValue<T>(object value)
		{
			return (T)Convert.ChangeType(value, typeof(T));
		}




		public struct TableData
		{
			public string ExcelWorksheetName; //表名称
			public int RowIndex; //横轴索引
			public int ColumnIndex; //纵轴索引
			public string Type; //数据类型
			public string FieldName; //变量名称
			public string Value; //值

			public TableData(string sheetName, int rowIndex, int columnIndex, string type, string fieldName, string value)
			{
				this.ExcelWorksheetName = sheetName;
				this.RowIndex = rowIndex;
				this.ColumnIndex = columnIndex;
				this.Type = type;
				this.FieldName = fieldName;
				this.Value = value;
			}

			public override string ToString()
			{
				return $"表名称:{ExcelWorksheetName},行:{RowIndex},列:{ColumnIndex},数据类型{Type},变量名称:{FieldName},值:{Value}";
			}
		}

		[Serializable]
		public class ConfigSettingData
		{
			static string mConfigSavedDir
			{
				get
				{
					string path = ($"{Application.streamingAssetsPath}/ConfigData/");
					if (!Directory.Exists(path))
					{
						Directory.CreateDirectory(path);
					}

					return path;
				}
			}

			private const string mConfigSavedFileName = "Config.json";
			public string ExcelPath = ($"Assets/XXL_U3D/Game/ConfigData/Excel");
			public string JsonPath = "/ConfigData/Json";

			public string ScriptsPath = $"Assets/XXL_U3D/Game/ConfigData/Scripts";

			public static ConfigSettingData Load()
			{

				if (!File.Exists($"{mConfigSavedDir}/{mConfigSavedFileName}"))
				{
					using (var fileStream = File.Create(mConfigSavedDir + mConfigSavedFileName))
					{
						fileStream.Close();
					}
				}

				var configSettinData =
					JsonUtility.FromJson<ConfigSettingData>(File.ReadAllText(mConfigSavedDir + mConfigSavedFileName));

				if (configSettinData == null)
				{
					configSettinData = new ConfigSettingData();
					configSettinData.Save();
				}

				return configSettinData;
			}

			public void Save()
			{
				string filePath = mConfigSavedDir + mConfigSavedFileName;
				File.WriteAllText(mConfigSavedDir + mConfigSavedFileName, JsonUtility.ToJson(this));
				Debug.Log("JSON位置:" + mConfigSavedDir);
				AssetDatabase.Refresh();
			}
		}
	}
}

