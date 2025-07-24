using System;
using UnityEngine;

namespace XXLFramework
{
	public class DragObjItem : MonoBehaviour
	{
		public bool CanDrag = false;

		public LockDirect LockDirect = LockDirect.Y; //锁定轴向不能在此方向中移动
		bool isDragging = false;
		Vector3 startPos;
        Vector3 endPos;
        Vector3 offset;

		public Action<DragObjItem> BegeinDragEvent;
		public Action<DragObjItem> DragEvent;
		public Action<DragObjItem> EndDragEvent;

		public void SetLockDirect(LockDirect lockDirect)
		{
			this.LockDirect = lockDirect;
		}

		private void OnMouseDown()
        {
			if (CanDrag)
			{
				//记录起始位置
				//因为我们的物体cube所处的是世界空间 鼠标是屏幕空间
				//需要将鼠标的屏幕空间转换成世界空间
				startPos = MyScreenPointToWorldPoint(Input.mousePosition, transform);
				BegeinDragEvent?.Invoke(this);
			}
			
        }

        private void OnMouseDrag()
        {
			if (CanDrag)
			{
				isDragging = true;
				endPos = MyScreenPointToWorldPoint(Input.mousePosition, transform);
				//计算偏移量
				offset = endPos - startPos;
				//让cube移动
				switch (LockDirect)
				{
					case LockDirect.X:
						transform.position += new Vector3(0, offset.y, offset.z);
						break;
					case LockDirect.Y:
						transform.position += new Vector3(offset.x, 0, offset.z);
						break;
					case LockDirect.Z:
						transform.position += new Vector3(offset.x, offset.y, 0);
						break;
					case LockDirect.无:
						transform.position += offset;
						break;
					default:
						break;
				}

				//这一次拖拽的终点变成了下一次拖拽的起点  
				startPos = endPos;
				DragEvent?.Invoke(this);
			}
		}

		private void OnMouseUp()
		{
			if (CanDrag && isDragging)
			{
				isDragging = false;
				EndDragEvent?.Invoke(this);
			}
		}

		Vector3 MyScreenPointToWorldPoint(Vector3 ScreenPoint, Transform target)
		{
			//1 得到物体在主相机的xx方向
			Vector3 dir = (target.position - Camera.main.transform.position);
			//2 计算投影 (计算单位向量上的法向量)
			Vector3 norVec = Vector3.Project(dir, Camera.main.transform.forward);
			//返回世界空间
			return Camera.main.ViewportToWorldPoint
				(
				   new Vector3(
					   ScreenPoint.x / Screen.width,
					   ScreenPoint.y / Screen.height,
					   norVec.magnitude
				   )
				);
		}

	}
	public enum LockDirect
	{
		X,Y,Z,无
	}
}
