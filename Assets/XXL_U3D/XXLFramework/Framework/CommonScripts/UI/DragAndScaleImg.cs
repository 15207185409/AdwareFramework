using System;
using System.Collections;
using System.Collections.Generic;
using XXLFramework;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
//将此脚本挂载在要放大缩小的图片上，父物体为一个遮罩，可实现子物体在父物体下放大缩小，自由拖拽
namespace XXLFramework
{
    /// <summary>
    /// 实现图片放大缩小，拖拽
    /// </summary>
    public class DragAndScaleImg : MonoBehaviour, IBeginDragHandler, IDragHandler,IPointerEnterHandler, IPointerExitHandler
    {

        Vector3 offest;
        bool CanDrag = false;
        private RectTransform rect; //rectTransform
        private float minX, maxX, minY, maxY;
        private float scale = 0f;
        bool pointerEnter = false;

        void Awake()
        {
            rect = GetComponent<RectTransform>();
            scale = UIManager.Instance.transform.localScale.x;
        }

        private float localScale = 1f;

        private void Update()
        {
			if (!pointerEnter)
			{
                return;
			}
            var dv = Input.GetAxis("Mouse ScrollWheel");
            if (dv == 0)
            {
                return;
            }

            localScale += dv;
            if (localScale > 10)
            {
                localScale = 10;
            }

            if (localScale < 1)
            {
                localScale = 1;
            }

            transform.localScale = new Vector3(localScale, localScale, localScale);
            SetDragRange();
            SetPos();
        }

        //刚开始拖拽时第一下触发这个函数
        public void OnBeginDrag(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Left &&
                RectTransformUtility.ScreenPointToWorldPointInRectangle(rect, eventData.position,
                    eventData.enterEventCamera, out Vector3 globalMousePos))
            {
                SetDragRange();
                //计算偏移
                offest = rect.position - globalMousePos;
            }
        }

        //拖拽过程中一直触发
        public void OnDrag(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Left &&
                RectTransformUtility.ScreenPointToWorldPointInRectangle(rect, eventData.position,
                    eventData.pressEventCamera, out Vector3 globalMousePos))
            {
                rect.position = DragRangeLimit(globalMousePos + offest);
            }
        }

        void SetPos()
        {
            if (transform.position.x < minX)
            {
                transform.PositionX(minX);
            }

            if (transform.position.x > maxX)
            {
                transform.PositionX(maxX);
            }

            if (transform.position.y < minY)
            {
                transform.PositionY(minY);
            }

            if (transform.position.y > maxY)
            {
                transform.PositionY(maxY);
            }
        }

        void SetDragRange()
        {
            //Debug.Log($"transform.position:{transform.position},partent:{transform.parent.position}");
            minX = transform.parent.position.x - rect.rect.width * scale * transform.localScale.x / 2 +
                   rect.rect.width * scale / 2;
            maxX = transform.parent.position.x + rect.rect.width * scale * transform.localScale.x / 2 -
                   rect.rect.width * scale / 2;
            minY = transform.parent.position.y - rect.rect.height * scale * transform.localScale.x / 2 +
                   rect.rect.height * scale / 2;
            maxY = transform.parent.position.y + rect.rect.height * scale * transform.localScale.x / 2 -
                   rect.rect.height * scale / 2;
        }

        Vector3 DragRangeLimit(Vector3 pos)
        {
            pos.x = Mathf.Clamp(pos.x, minX, maxX);
            pos.y = Mathf.Clamp(pos.y, minY, maxY);
            return pos;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            pointerEnter = true;
        }

        public void OnPointerExit(PointerEventData eventData)
		{
            pointerEnter = false;
		}

	
	}
}
