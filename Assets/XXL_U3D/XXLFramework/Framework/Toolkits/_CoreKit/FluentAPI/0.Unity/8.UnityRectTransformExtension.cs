using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;

namespace XXLFramework
{
	public static class RectTransformExtension
	{
        public static Vector2 GetPosInRootTrans(this RectTransform selfRectTransform, Transform rootTrans)
        {
            return RectTransformUtility.CalculateRelativeRectTransformBounds(rootTrans, selfRectTransform).center;
        }

        public static RectTransform AnchorPosX(this RectTransform selfRectTrans, float anchorPosX)
        {
            var anchorPos = selfRectTrans.anchoredPosition;
            anchorPos.x = anchorPosX;
            selfRectTrans.anchoredPosition = anchorPos;
            return selfRectTrans;
        }

        public static RectTransform AnchorPosY(this RectTransform selfRectTrans, float anchorPosY)
        {
            var anchorPos = selfRectTrans.anchoredPosition;
            anchorPos.y = anchorPosY;
            selfRectTrans.anchoredPosition = anchorPos;
            return selfRectTrans;
        }

        public static RectTransform SetSizeWidth(this RectTransform selfRectTrans, float sizeWidth)
        {
            var sizeDelta = selfRectTrans.sizeDelta;
            sizeDelta.x = sizeWidth;
            selfRectTrans.sizeDelta = sizeDelta;
            return selfRectTrans;
        }

        public static RectTransform SetSizeHeight(this RectTransform selfRectTrans, float sizeHeight)
        {
            var sizeDelta = selfRectTrans.sizeDelta;
            sizeDelta.y = sizeHeight;
            selfRectTrans.sizeDelta = sizeDelta;
            return selfRectTrans;
        }


		public static Vector2 GetWorldSize(this RectTransform selfRectTrans)
        {
            return RectTransformUtility.CalculateRelativeRectTransformBounds(selfRectTrans).size;
        }

        public static void Spread(this RectTransform selfRectTrans)
        {
			selfRectTrans.offsetMin = Vector2.zero;
			selfRectTrans.offsetMax = Vector2.zero;
			selfRectTrans.anchoredPosition = Vector3.zero;
			selfRectTrans.anchorMin = Vector2.zero;
			selfRectTrans.anchorMax = Vector2.one;
			selfRectTrans.localScale = Vector3.one;
		}


        /// <summary>
        /// 打开面板带动画
        /// </summary>
        /// <param name="rect">面板</param>
        /// <param name="isOpen">是否打开</param>
        /// <param name="during">持续时间</param>
        public static void OpenAni(this RectTransform rect, float during = 0.15f)
        {
            rect.gameObject.SetActive(true);
            rect.localScale = new Vector3(1, 0, 1);
            rect.DOScale(Vector3.one, during);
        }

        /// <summary>
        /// 关闭面板带动画
        /// </summary>
        /// <param name="rect">面板</param>
        /// <param name="isOpen">是否打开</param>
        /// <param name="during">持续时间</param>
        public static void CloseAni(this RectTransform rect, float during = 0.15f)
        {
            rect.gameObject.SetActive(true);
            rect.localScale = Vector3.one;
            rect.DOScale(new Vector3(1, 0, 1), during).OnComplete(() => { rect.gameObject.SetActive(false); });
        }
    }
}
