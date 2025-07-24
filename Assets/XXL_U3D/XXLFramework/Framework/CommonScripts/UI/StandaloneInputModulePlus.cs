using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

namespace XXLFramework
{
	public class StandaloneInputModulePlus : StandaloneInputModule
	{
		private static StandaloneInputModulePlus mInstance;
		private static StandaloneInputModulePlus Instance
		{
			get
			{
				if (mInstance == null)
				{
					mInstance = FindObjectOfType<StandaloneInputModulePlus>();
				}
				return mInstance;
			}
		}

		public static PointerEventData GetPointerData(PointerId pointerId)
		{
			int id = -1;
			switch (pointerId)
			{
				case PointerId.Left:
					id = -1;
					break;
				case PointerId.Right:
					id = -2;
					break;
				case PointerId.Middle:
					id = -3;
					break;
				case PointerId.Touch:
					id = -4;
					break;
				default:
					break;
			}
			if (Instance.m_PointerData.ContainsKey(id))
			{
				return Instance.m_PointerData[id];
			}
			return null;
		}

		public static GameObject PointerEnterObj
		{
			get
			{
				if (Instance.m_PointerData.ContainsKey(-1))
				{
					return Instance.m_PointerData[-1].pointerEnter;
				}
				return null;
			}
		}

		public static T PointerEnterType<T>() where T : Component
		{
			if (PointerEnterObj && PointerEnterObj.GetComponentInParent<T>() != null)
			{
				return PointerEnterObj.GetComponentInParent<T>();
			}
			else
			{
				return null;
			}
		}

		//检测当前鼠标是否在UI上
		public static bool IsRacastUI()
		{
			if (PointerEnterObj && PointerEnterObj.GetComponent<RectTransform>() != null)
			{
				return true;
			}
			else
			{
				return false;
			}
		}
	}

	public enum PointerId
	{
		Left, Right, Middle, Touch
	}
}