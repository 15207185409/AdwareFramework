using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace XXLFramework
{
    public class DragWindow : MonoBehaviour, IBeginDragHandler, IDragHandler
    {
        // 另一个UI
        private RectTransform container;

        RectTransform rt;
        Canvas canvas;

        // 位置偏移量
        Vector3 offset = Vector3.zero;
        // 最小、最大X、Y坐标
        float minX, maxX, minY, maxY;

        void Start()
        {
            rt = GetComponent<RectTransform>();
            container = transform.parent.GetComponent<RectTransform>();
            canvas = GetComponentInParent<Canvas>();
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (RectTransformUtility.ScreenPointToWorldPointInRectangle(rt, eventData.position, eventData.enterEventCamera, out Vector3 globalMousePos))
            {
                // 计算偏移量
                offset = rt.position - globalMousePos;
                // 设置拖拽范围
                SetDragRange();
            }
        }

        public void OnDrag(PointerEventData eventData)
        {
            // 将屏幕空间上的点转换为位于给定RectTransform平面上的世界空间中的位置
            if (RectTransformUtility.ScreenPointToWorldPointInRectangle(rt, eventData.position, eventData.pressEventCamera, out Vector3 globalMousePos))
            {
                rt.position = DragRangeLimit(globalMousePos + offset);
            }
        }

        // 设置最大、最小坐标
        void SetDragRange()
        {
            Debug.Log("全局缩放" + rt.lossyScale);
            Debug.Log("container全局缩放" + container.lossyScale);

            // 最小x坐标 = 容器当前x坐标 - 容器轴心距离左边界的距离 + UI轴心距离左边界的距离
            minX = container.position.x
                - container.pivot.x * container.rect.width * canvas.scaleFactor
                + rt.rect.width * canvas.scaleFactor *  rt.pivot.x;

            // 最大x坐标 = 容器当前x坐标 + 容器轴心距离右边界的距离 - UI轴心距离右边界的距离
            maxX = container.position.x
                + (1 - container.pivot.x) * container.rect.width*canvas.scaleFactor
                - rt.rect.width * canvas.scaleFactor * (1 - rt.pivot.x);

            // 最小y坐标 = 容器当前y坐标 - 容器轴心距离底边的距离 + UI轴心距离底边的距离
            minY = container.position.y
                - container.pivot.y * container.rect.height*canvas.scaleFactor
                + rt.rect.height * canvas.scaleFactor * rt.pivot.y;

            // 最大y坐标 = 容器当前x坐标 + 容器轴心距离顶边的距离 - UI轴心距离顶边的距离
            maxY = container.position.y
                + (1 - container.pivot.y) * container.rect.height*canvas.scaleFactor
                - rt.rect.height * canvas.scaleFactor * (1 - rt.pivot.y);

            Debug.Log($"minX:{minX},maxX:{maxX},minY:{minY},maxY:{maxY}");

        }

        // 限制坐标范围
        Vector3 DragRangeLimit(Vector3 pos)
        {
            pos.x = Mathf.Clamp(pos.x, minX, maxX);
            pos.y = Mathf.Clamp(pos.y, minY, maxY);
            return pos;
        }
    }
}