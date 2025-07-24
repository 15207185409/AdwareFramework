using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

namespace XXLFramework
{
	public class EventTriggerPlus : EventTrigger
	{
		public class TriggerPlusEvent : UnityEvent<PointerEventData>
		{

		}
		public class EntryPlus
		{
			public EventTriggerPlusType eventID = EventTriggerPlusType.SingleClick;
			public TriggerPlusEvent callback = new TriggerPlusEvent();
		}

		[FormerlySerializedAs("delegates")]
		[SerializeField]
		private List<EntryPlus> m_Delegates;

		public List<EntryPlus> TriggerPluss
		{
			get
			{
				if (m_Delegates == null)
				{
					m_Delegates = new List<EntryPlus>();
				}

				return m_Delegates;
			}
			set
			{
				m_Delegates = value;
			}
		}

		protected EventTriggerPlus()
		{
		}

		private void Execute(EventTriggerPlusType id, PointerEventData pointerData)
		{
			int i = 0;
			for (int count = TriggerPluss.Count; i < count; i++)
			{
				EntryPlus entryPlus = TriggerPluss[i];
				if (entryPlus.eventID == id && entryPlus.callback != null)
				{
					entryPlus.callback.Invoke(pointerData);
				}
			}
		}


		public override void OnPointerClick(PointerEventData eventData)
		{
			clickCount++;
			if (!isDetectClick)
			{
				StartCoroutine(ResetPointerClick(eventData));
			}
		}

		private IEnumerator ResetPointerClick(PointerEventData eventData)
		{
			isDetectClick = true;
			yield return new WaitForSeconds(doubleClickInterval);
			if (!isLongPress && !isDrag)
			{
				if (clickCount == 1)
				{
					Execute(EventTriggerPlusType.SingleClick, eventData);
				}
				else
				{
					Execute(EventTriggerPlusType.DoubleClick, eventData);
				}
			}
			clickCount = 0;
			isLongPress = false;
			isDetectClick = false;

		}

		private float doubleClickInterval = 0.2f; //双击间隔
		private float longPressTime = 0.3f; //长按时间
		private bool isLongPress = false; //是否长按
		private int clickCount = 0;			//鼠标点击次数
		private bool isDetectClick = false; //是否正在检测点击
		float pressTime = 0; //长按时间
		bool isDrag = false; //是否正在拖拽
		Vector2 lastDownPos; //上次点击的位置坐标

		public override void OnPointerDown(PointerEventData eventData)
		{
			base.OnPointerDown(eventData);
			pressTime = Time.time;
			isDrag = false;
			lastDownPos = eventData.position;
		}

		public override void OnPointerUp(PointerEventData eventData)
		{
			base.OnPointerUp(eventData);
			Vector2 currentPos = eventData.position;
			float offset = Vector2.Distance(currentPos, lastDownPos);
			if (offset>= 10)
			{
				isDrag = true;
			}
			if ((Time.time-pressTime)>=longPressTime && !eventData.dragging)
			{
				isLongPress = true;
				Execute(EventTriggerPlusType.LongPress, eventData);
			}
		}

	}

	public enum EventTriggerPlusType
	{
		SingleClick,
		DoubleClick,
		LongPress
	}
}
