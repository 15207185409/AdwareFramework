using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace XXLFramework
{
    public static class UIExtension 
    {
       public static void AddEndEditListen(this TMP_InputField inputField, float minValue, float maxValue)
		{
			inputField.onEndEdit.AddListener(content =>
			{
				if (!string.IsNullOrEmpty(content))
				{
					float value = float.Parse(content);
					if (value < minValue || value > maxValue)
					{
						inputField.text = "";
					}
				}
			});
		}

		public static void SetTexture(this RawImage rawImage,  Texture2D texture)
		{
			float rawImgRatio = rawImage.GetComponent<RectTransform>().rect.width / rawImage.GetComponent<RectTransform>().rect.height;
			float textureRatio = texture.width / texture.height;
			if (rawImgRatio > textureRatio)
			{
				float height = rawImage.GetComponent<RectTransform>().rect.height;
				rawImage.GetComponent<RectTransform>().SetRectTransformSize(new Vector2(texture.width * height / texture.height, height));
			}
			else
			{
				float width = rawImage.GetComponent<RectTransform>().rect.width;
				rawImage.GetComponent<RectTransform>().SetRectTransformSize(new Vector2(width, texture.height * width / texture.width));
			}
			rawImage.texture = texture;
		}

		public static void SetSprite(this Image image, Sprite sprite)
		{
			float imgRadio = image.GetComponent<RectTransform>().rect.width / image.GetComponent<RectTransform>().rect.height;
			float spriteRatio = sprite.rect.width / sprite.rect.height;
			if (imgRadio > spriteRatio)
			{
				float height = image.GetComponent<RectTransform>().rect.height;
				image.GetComponent<RectTransform>().SetRectTransformSize(new Vector2(sprite.rect.width * height / sprite.rect.height, height));
			}
			else
			{
				float width = image.GetComponent<RectTransform>().rect.width;

				image.GetComponent<RectTransform>().SetRectTransformSize(new Vector2(width, sprite.rect.height * width / sprite.rect.width));
			}
			image.sprite = sprite;
		}


		public static void SetRectTransformSize<T>(this T trans, Vector2 newSize) where T:Component
		{
			RectTransform rectTransform = trans.GetComponent<RectTransform>();
			Vector2 oldSize = rectTransform.rect.size;
			Vector2 deltaSize = newSize - oldSize;
			rectTransform.offsetMin = rectTransform.offsetMin - new Vector2(deltaSize.x * rectTransform.pivot.x, deltaSize.y * rectTransform.pivot.y);
			rectTransform.offsetMax = rectTransform.offsetMax + new Vector2(deltaSize.x * (1f - rectTransform.pivot.x), deltaSize.y * (1f - rectTransform.pivot.y));
		}

		//获取当前选择哪一个
		public static int GetTogSelectIndex(this List<Toggle> togList)
		{
			int result = -1;
			for (int i = 0; i < togList.Count; i++)
			{
				if (togList[i].isOn)
				{
					result = i;
					break;
				}
			}
			return result;
		}

	}
}
