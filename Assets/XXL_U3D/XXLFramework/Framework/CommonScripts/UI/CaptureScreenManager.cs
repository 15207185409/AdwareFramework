using System;
using System.Collections;
using UnityEngine;

namespace XXLFramework
{
	//截图管理类
    public class CaptureScreenManager : MonoBehaviour
    {
		private static CaptureScreenManager mInstance;
		public static CaptureScreenManager Instance
		{
			get
			{
				if (!mInstance)
				{
					mInstance = FindObjectOfType<CaptureScreenManager>();
				}

				if (!mInstance)
				{
					mInstance = new GameObject("CaptureScreenManager").AddComponent<CaptureScreenManager>(); 
					DontDestroyOnLoad(mInstance);
				}
				return mInstance;
			}
		}

		public static void CaptureScreen(RectTransform rectTransform, Action<Texture2D> callBack = null)
		{
			Instance.StartCoroutine(Instance.CaptureScreenCor(rectTransform, callBack));
		}


		private  IEnumerator CaptureScreenCor(RectTransform rectTransform, Action<Texture2D> callBack = null)
		{
			float width = rectTransform.rect.width;
			float height = rectTransform.rect.height;
			float scale = rectTransform.GetComponentInParent<Canvas>().transform.localScale.x;
			width *= scale;
			height *= scale;

			Vector2 pivot = rectTransform.pivot;
			Vector2 off = new Vector2(rectTransform.rect.width * pivot.x * scale,
				rectTransform.rect.height * pivot.y * scale);
			Vector2 screenPoint = (Vector2)rectTransform.position - off;
			Rect rect = new Rect(screenPoint.x, screenPoint.y, width, height);
			yield return new WaitForEndOfFrame();
			Texture2D shot = new Texture2D((int)rect.width, (int)rect.height, TextureFormat.RGB24, false);
			shot.ReadPixels(rect, 0, 0);
			shot.Apply();

			callBack?.Invoke(shot);
		}


	}
}
