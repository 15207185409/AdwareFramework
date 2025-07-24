using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using DG.Tweening;
using System.Text;
using System.Linq;
using UnityEditor;
using UnityEngine.Networking;
using Cysharp.Threading.Tasks;

namespace XXLFramework
{
	public static class Utility
	{

		public static void QuitAPP()
		{
#if UNITY_EDITOR
			EditorApplication.isPlaying = false;
#else
	Application.Quit();
#endif
		}

		/// <summary>
		/// 根据时间返回文本
		/// </summary>
		/// <param name="time">单位为秒</param>
		/// <returns></returns>
		public static string GetTxtByTime(int time)
		{
			string result = "";
			int hour = time / 3600;
			int minute = (time % 3600) / 60;
			int second = time % 60;
			string hourStr = hour < 10 ? "0" + hour : hour.ToString();
			string minStr = minute < 10 ? "0" + minute : minute.ToString();
			string secStr = second < 10 ? "0" + second : second.ToString();
			result = string.Format("{0}:{1}:{2}", hourStr, minStr, secStr);
			return result;
		}

		/// <summary>
		/// 根据值和单位返回显示的值
		/// </summary>
		/// <param name="value"></param>
		/// <param name="danWei"></param>
		/// <returns></returns>
		public static string GetValueTxt(float value, int weiShu = 2, string danWei = "")
		{
			if (danWei == "%")
			{
				return (value.ToString("p"));
			}
			else
			{
				return Math.Round(value, weiShu).ToString();

			}
		}

		public static void OpenDirectory(string path)
		{
			if (string.IsNullOrEmpty(path)) return;

			path = path.Replace("/", "\\");
			if (!Directory.Exists(path))
			{
				Debug.LogError("No Directory: " + path);
				return;
			}
			//可能360不信任
			System.Diagnostics.Process.Start("explorer.exe", path);
		}


		/// <summary>
		///  用于动态生成物体的时候数量和要生成的父物体下的子物体数量一样
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="listData">数据</param>
		/// <param name="parent">父物体</param>
		/// <param name="prefab">预制体</param>
		public static void MatchCloneCount<T>(List<T> listData, Transform parent, GameObject prefab)
		{
			if (parent.childCount < listData.Count)
			{
				int count = listData.Count - parent.childCount;
				for (int i = 0; i < count; i++)
				{
					GameObject g = GameObject.Instantiate(prefab, parent) as GameObject;
					g.SetActive(true);
					g.transform.localScale = Vector3.one;
				}
			}
			else if (parent.childCount > listData.Count)
			{
				int count = parent.childCount - listData.Count;
				for (int i = parent.childCount - 1; i > listData.Count - 1; i--)
				{
					parent.GetChild(i).gameObject.SetActive(false);
				}
			}
		}

		/// <summary>
		/// 得到正确的位置
		/// </summary>
		/// <param name="sizi">大小</param>
		/// <returns></returns>
		public static Vector2 GetRightPosByMousePos(RectTransform rectTransform)
		{
			var Scale = rectTransform.GetComponentInParent<Canvas>().GetComponent<RectTransform>().localScale.x;
			Vector2 size = rectTransform.rect.size;
			Vector2 result = Input.mousePosition;

			result.x = Mathf.Clamp(Input.mousePosition.x + size.x / 2, size.x / 2, Screen.width / Scale - size.x / 2);
			result.y = Mathf.Clamp(Input.mousePosition.y - size.y / 2, size.y / 2, Screen.height / Scale - size.y / 2);

			//Debug.Log($"鼠标位置:{Input.mousePosition},物体大小:{size},物体位置：{rectTransform.position}");
			return result;
		}




		public static Texture2D Base64ToTexture2D(string Base64STR)
		{
			Texture2D pic = new Texture2D(Screen.width, Screen.height, TextureFormat.RGBA32, false);
			byte[] data = System.Convert.FromBase64String(Base64STR);
			pic.LoadImage(data);
			return pic;
		}
		public static string Texture2DToBase64(Texture2D t2dSource)
		{
			//Texture2D texture2D = DeCompress(t2dSource);
			byte[] bytesArr = t2dSource.EncodeToJPG();
			string strbaser64 = Convert.ToBase64String(bytesArr);
			return strbaser64;
		}

		/// <summary>
		/// 上传实验报告时，需要附加前面的属性才能正确显示图片
		/// </summary>
		/// <param name="textureBase64"></param>
		/// <returns></returns>
		public static string AppendTextureBase64(string textureBase64)
		{
			if (string.IsNullOrEmpty(textureBase64))
			{
				return string.Empty;
			}
			else
			{
				return "data:image/png;base64," + textureBase64;
			}

		}

		/// <summary>
		/// 判断两个集合是否有交集
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="list1"></param>
		/// <param name="list2"></param>
		/// <returns></returns>
		public static bool IsArrayIntersection<T>(List<T> list1, List<T> list2)
		{
			List<T> t = list1.Distinct().ToList();

			var exceptArr = t.Except(list2).ToList();

			if (exceptArr.Count < t.Count)
			{
				return true;
			}
			else
			{
				return false;
			}
		}

		/// <summary>
		/// 求两个集合的并集
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="list1"></param>
		/// <param name="list2"></param>
		/// <returns></returns>
		public static List<T> UnionLists<T>(List<T> list1, List<T> list2)
		{
			List<T> unionjiList = list1.Union(list2).ToList();
			return unionjiList;
		}

		public static void ShowFadeEffect(CanvasGroup canvasGroup, float time, Action act)
		{
			Debug.Log("转场景效果");

			canvasGroup.alpha = 0;
			canvasGroup.DOFade(1, time / 2).OnComplete(() =>
			{
				if (act != null)
				{
					act();
				}
				canvasGroup.DOFade(0, time / 2);
			});
		}

		public static void WriteReport(string content, string path, string reportName)
		{
#if UNITY_STANDALONE || UNITY_EDITOR
			if (!Directory.Exists(path))
			{
				Directory.CreateDirectory(path);
			}
			string fullPath = Path.Combine(path, reportName);
			if (!File.Exists(fullPath))
			{
				FileStream fs = File.Create(fullPath);
				fs.Close();
			}

			File.WriteAllText(fullPath, content, Encoding.UTF8);

			Debug.Log("写入报告成功");
#endif
		}

		/// <summary>
		/// 判断某点是否在某框内，例如当前鼠标位置
		/// </summary>
		public static bool CheckIsContainsPoint(RectTransform rect, Vector3 point)
		{
			Rect spaceRect = rect.rect;
			Vector3 spacePos = rect.position;
			spaceRect.x = spaceRect.x * rect.lossyScale.x + spacePos.x;
			spaceRect.y = spaceRect.y * rect.lossyScale.y + spacePos.y;
			spaceRect.width = spaceRect.width * rect.lossyScale.x;
			spaceRect.height = spaceRect.height * rect.lossyScale.y;
			return spaceRect.Contains(point);
		}

		/// <summary>
		/// 动态生成 html 字符串
		/// </summary>
		/// <param name="datas">用户数据</param>
		/// <param name="htmlPath">html模板路径</param>
		/// <returns></returns>
		public static string CreateHtml(List<string> datas, string htmlModelContent)
		{
			// 获取模板的物理路径
			//string ModelPath = htmlPath + "Model.html";

			StringBuilder data;
			if (string.IsNullOrEmpty(htmlModelContent))
			{
				Debug.LogError("模板文件不存在或者编码不是 utf-8");
				return "";
			}

			try
			{
				data = new StringBuilder(htmlModelContent);

				for (int i = 0; i < datas.Count; i++)
				{
					data = data.Replace("$form[" + i + "]$", datas[i]);
				}
			}
			catch (System.Exception e)
			{
				return "";
			}

			return data.ToString();
		}

		/// <summary>
		/// 获取json文件
		/// </summary>
		/// <param name="jsonPath"></param>
		/// <returns></returns>
		public static string GetJsonTxtByResource(string jsonPath)
		{
			Debug.Log(jsonPath);
			TextAsset textAsset = Resources.Load<TextAsset>(jsonPath);
			if (textAsset != null)
			{
				// 读取文本内容  
				string textContent = textAsset.text;
				//Debug.Log(textContent);
				return textContent;
			}
			else
			{
				Debug.LogError("Failed to load text asset.");
				return string.Empty;
			}
		}


		/// <summary>
		/// 获取json文件
		/// </summary>
		/// <param name="jsonPath"></param>
		/// <returns></returns>
		public static async UniTask<string> GetJsonTxt(string jsonPath)
		{
			var webRequest = UnityWebRequest.Get(jsonPath);
			try
			{
				var result = await webRequest.SendWebRequest();
				if (result.isNetworkError || result.isHttpError)
				{
					Debug.Log(result.error);
					return "";
				}
				else
				{
					return result.downloadHandler.text;
				}
			}
			catch (Exception)
			{
				return "";
			}

		}

		/// <summary>
		/// 获取json文件
		/// </summary>
		/// <param name="jsonPath"></param>
		/// <returns></returns>
		public static async UniTask<string> GetJsonTxtByStreamingAssets(string jsonPath)
		{

			var webRequest = UnityWebRequest.Get(jsonPath);
			try
			{
				var result = await webRequest.SendWebRequest();
				if (result.isNetworkError || result.isHttpError)
				{
					Debug.Log(result.error);
					return "";
				}
				else
				{
					return result.downloadHandler.text;
				}
			}
			catch (Exception)
			{
				return "";
			}

		}


		public static Texture2D Base64ToTexture2D(string Base64STR, int width, int height)
		{
			Texture2D pic = new Texture2D(width, height, TextureFormat.RGBA32, false);
			byte[] data = System.Convert.FromBase64String(Base64STR);
			pic.LoadImage(data);
			return pic;
		}


		/// <summary>
		/// 得到摄像机的位置和角度
		/// </summary>
		/// <param name="target">目标</param>
		/// <param name="offset">偏移</param>
		/// <returns></returns>
		public static Vector3 GetCameraView(Transform target, Vector3 offset, out Vector3 eulerAngle)
		{
			Vector3 position = target.position + offset;
			eulerAngle = Quaternion.LookRotation(-offset).eulerAngles;
			return position;
		}


	}
}

