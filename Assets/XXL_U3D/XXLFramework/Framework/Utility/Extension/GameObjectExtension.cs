using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace XXLFramework
{
    public static class GameObjectExtension
    {
		//添加点击事件
		public static void AddTriggerPlusListener(this Component obj, EventTriggerPlusType type, UnityAction<PointerEventData> call)
		{
			EventTriggerPlus trigger = obj.GetOrAddComponent<EventTriggerPlus>();

			EventTriggerPlus.EntryPlus myclick = new EventTriggerPlus.EntryPlus
			{
				eventID = type,
			};

			// ?.操作符是c#6.0提供的新特性，需要将工程升级到.net4.x,具体可以自己查阅相关信息
			myclick.callback.AddListener(call);
			trigger.TriggerPluss.Add(myclick);
		}

		public static void RemoveTriggerPlusListener(this Component obj, EventTriggerPlusType triggerType)
		{
			EventTriggerPlus trigger = obj.GetComponent<EventTriggerPlus>();
			if (trigger.IsNull())
			{
				return;
			}

			foreach (var entry in trigger.TriggerPluss)
			{
				if (entry.eventID == triggerType)
				{
					entry.callback.RemoveAllListeners();
				}
			}
		}

		//添加点击事件
		public static void AddTriggerListener(this Component obj, EventTriggerType type, UnityAction<PointerEventData> call)
		{
			EventTrigger trigger = obj.GetOrAddComponent<EventTrigger>();

			EventTrigger.Entry myclick = new EventTrigger.Entry
			{
				eventID = type,
			};

			// ?.操作符是c#6.0提供的新特性，需要将工程升级到.net4.x,具体可以自己查阅相关信息
			myclick.callback.AddListener((BaseEventData data) => { call?.Invoke(data as PointerEventData); });
			trigger.triggers.Add(myclick);
		}

		public static void RemoveTriggerListener(this Component obj, EventTriggerType triggerType)
		{
			EventTrigger trigger = obj.GetComponent<EventTrigger>();
			if (trigger.IsNull())
			{
				return;
			}

			foreach (var entry in trigger.triggers)
			{
				if (entry.eventID == triggerType)
				{
					entry.callback.RemoveAllListeners();
				}
			}
		}


		public static Vector3 GetScreenPosition(this GameObject target)
		{
			RectTransform canvasRtm = Object.FindObjectOfType<Canvas>().GetComponent<RectTransform>();
			float width = canvasRtm.sizeDelta.x;
			float height = canvasRtm.sizeDelta.y;
			Vector3 pos = Camera.main.WorldToScreenPoint(target.transform.position);
			pos.x *= width / Screen.width;
			pos.y *= height / Screen.height;
			return pos;
		}


	}
}
