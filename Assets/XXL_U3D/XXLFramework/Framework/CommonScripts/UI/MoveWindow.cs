using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace XXLFramework
{
	public class MoveWindow : MonoBehaviour
	{
		private RectTransform Rect;
		float Pivot = 1f;
		public Button ArrowBtn;
		public MoveDirection MoveDirection = MoveDirection.Left;
		private bool IsStartState = true; //是否初始状态

		public float duration = 0.1f;

		// Use this for initialization
		void Start()
		{
			Rect = GetComponent<RectTransform>();
			ArrowBtn.onClick.AddListener(OnArrowClick);
		}

		private void OnArrowClick()
		{
			switch (MoveDirection)
			{
				case MoveDirection.Left:
					if (IsStartState)
					{
						Rect.DOPivotX(Pivot, duration).OnComplete(() =>
						{
							ArrowBtn.transform.localScale = new Vector3(-1, 1, 1);
						});
					}
					else
					{
						Rect.DOPivotX(0, duration).OnComplete(() =>
						{
							ArrowBtn.transform.localScale = new Vector3(1, 1, 1);
						});
					}
					break;
				case MoveDirection.Right:
					if (IsStartState)
					{
						Rect.DOPivotX(1 - Pivot, duration).OnComplete(() =>
						{
							ArrowBtn.transform.localScale = new Vector3(-1, 1, 1);
						});
					}
					else
					{
						Rect.DOPivotX(1, duration).OnComplete(() =>
						{
							ArrowBtn.transform.localScale = new Vector3(1, 1, 1);
						});
					}
					break;
				case MoveDirection.Up:
					if (IsStartState)
					{
						Rect.DOPivotY(1 - Pivot, duration).OnComplete(() =>
						{
							ArrowBtn.transform.localScale = new Vector3(-1, 1, 1);
						});
					}
					else
					{
						Rect.DOPivotY(1, duration).OnComplete(() =>
						{
							ArrowBtn.transform.localScale = new Vector3(-1, 1, 1);
						});
					}
					break;
				case MoveDirection.Down:
					if (IsStartState)
					{
						Rect.DOPivotY(Pivot, duration).OnComplete(() =>
						{
							ArrowBtn.transform.localScale = new Vector3(1, 1, 1);
						});
					}
					else
					{
						Rect.DOPivotY(0, duration).OnComplete(() =>
						{
							ArrowBtn.transform.localScale = new Vector3(-1, 1, 1);
						});
					}
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}

			IsStartState = !IsStartState;
		}

		public void Move(bool isShow)
		{
			if (isShow)
			{
				if (!IsStartState)
				{
					OnArrowClick();
				}
			}
			else
			{
				if (IsStartState)
				{
					OnArrowClick();
				}
			}
		}
	}

	public enum MoveDirection
	{
		Left, Right, Up, Down
	}
}