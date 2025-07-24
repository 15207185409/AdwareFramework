using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Reflection;
using System.Text;
using Newtonsoft.Json;
using UnityEngine;

namespace XXLFramework
{
	/// <summary>
	/// 保存指定文件到本地
	/// </summary>
	public class StorageUtility 
    {
        #region 保存数据到本地文件夹（.csv格式）

        public static void JsonToCsv<T>(List<T> t)
        {
            DataTable dataTable = JsonToDataTable<T>(JsonConvert.SerializeObject(t));
            DataTableToExcel(dataTable,FileUtility.SaveProject("csv"));
        }

        private static DataTable JsonToDataTable<T>(string json)
        {
            DataTable dataTable = new DataTable();
            DataTable result;
            try
            {
                List<T> arrayList = JsonConvert.DeserializeObject<List<T>>(json);
                if (arrayList.Count <= 0)
                {
                    result = dataTable;
                    return result;
                }

                foreach (T item in arrayList)
                {
                    Dictionary<string, object> dictionary = GetFields<T>(item);
                    if (dataTable.Columns.Count == 0) 
                    {
                        foreach (string current in dictionary.Keys)
                        {
                            dataTable.Columns.Add(current, dictionary[current].GetType());
                        }
                    }

                    DataRow dataRow = dataTable.NewRow();
                    foreach (string current in dictionary.Keys)
                    {
                        dataRow[current] = dictionary[current];
                    }
                    dataTable.Rows.Add(dataRow);

                }
            }
            catch 
            {
         
            }

            result = dataTable;
            return result;
        }

        /// <summary>
        /// 获取类的字段
        /// </summary>
        /// <param name="t"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        private static Dictionary<string, object> GetFields<T>(T t)
        {
            Dictionary<string, object> keyValues = new Dictionary<string, object>();
            if (t == null)
            {
                return keyValues;
            }

            FieldInfo[] fieldInfos =
                t.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
            if (fieldInfos.Length<=0)
            {
                return keyValues;
            }

            foreach (FieldInfo item in fieldInfos)
            {
                string name = item.Name;
                object value = item.GetValue(t);
                if (item.FieldType.IsValueType||item.FieldType.Name.StartsWith("String"))
                {
                    keyValues.Add(name,value);
                }
                else
                {
                    Dictionary<string, object> subKeyValues = GetFields<object>(value);
                    foreach (KeyValuePair<string,object> temp in subKeyValues)
                    {
                        keyValues.Add(temp.Key,temp.Value);
                    }

                }
            }

            return keyValues;
        }

        /// <summary>
        /// 选择文件保存路径
        /// </summary>
        /// <param name="table"></param>
        /// <param name="file"></param>
        private static void DataTableToExcel(DataTable table,string file)
        {
            if (File.Exists(file))
            {
                File.Delete(file);
            }

            if (table.Columns.Count <= 0) 
            {
                Debug.Log("数据列表长度为0");
                return;
            }

            string tietle = "";
            FileStream fs = new FileStream(file, FileMode.OpenOrCreate);
            StreamWriter sw = new StreamWriter(new BufferedStream(fs), Encoding.UTF8);
            for (int i = 0; i < table.Columns.Count; i++)
            {
                tietle += table.Columns[i].ColumnName + ",";
            }

            tietle = tietle.Substring(0, tietle.Length - 1) + "\n";
            sw.Write(tietle);
            foreach (DataRow row in table.Rows)
            {
                string line = "";
                for (int i = 0; i < table.Columns.Count; i++)
                {
                    line += row[i].ToString().Trim() + ",";
                }

                line = line.Substring(0, line.Length - 1)+"\n";
                sw.Write(line);
            }
            sw.Close();
            fs.Close();
            
        }

        #endregion

        #region 保存截图到指定文件
        /// <summary>
        /// 保存截图到指定文件
        /// </summary>
        public static void ScreenShotFile()
        {
            ScreenCapture.CaptureScreenshot(FileUtility.SaveProject(".png"));
        }
        #endregion
    }
}

