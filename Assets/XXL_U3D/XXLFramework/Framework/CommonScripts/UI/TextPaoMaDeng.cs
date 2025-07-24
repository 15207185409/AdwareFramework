using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace XXLFramework
{
	public class TextPaoMaDeng : MonoBehaviour
	{
		private Mask Mask;
		private TextMeshProUGUI ContentTxt1;
		private TextMeshProUGUI ContentTxt2;

		private bool isInit = false;
		private float maskWidth; //遮罩宽度
		private float maskHeight; //遮罩高度
		private float txtWidth; //文本宽度
		private float txtHeight; //文本高度
		private float moveEndPos; //移动结束位置
		private float Timer = 0; //计时器时间

		[Header("是否自动运行")]
		public bool AutoRun = false;

		[Header("延迟滚动时间")]
		[Range(0, 5)]
		public float DelayTime = 0;

		[Header("文本框滚动的初始相对位置")]
		[Range(0, 1)]
		public float TxtInitRelativetPos = 1; 

		[Header("移动方向")]
		public TxtMoveDirection Direction = TxtMoveDirection.左移;

		[Header("文字移动速度")]
		[Range(10, 300)]
		public int Speed = 200; //文字移动速度，默认每秒走200

		[Header("两条文字间的间隔距离")]
		[Range(0, 500)]
		public int IntervalDistance = 200;

		[Header("文本框小于Mask长度时是否需要滚动")]
		public bool MoveWhenShort = false;

		[Header("是否自动匹配TextMeshPro的宽度或者高度")]
		public bool AddContentSizeFitter = true;

		private void Start()
		{
			if (AutoRun)
			{
				Init();
				SetContent(ContentTxt1.text);
			}
		}

		private void Init()
		{
			if (!isInit)
			{
				isInit = true;
				Mask = GetComponentInChildren<Mask>();
				ContentTxt1 = GetComponentInChildren<TextMeshProUGUI>();
				

				//txtBeginPos = ContentTxt1.transform.localPosition;
				maskWidth = Mask.GetComponent<RectTransform>().rect.width;
				maskHeight = Mask.GetComponent<RectTransform>().rect.height;

				Vector2 startPivot = Mask.GetComponent<RectTransform>().pivot;
				Mask.GetComponent<RectTransform>().pivot = new Vector2(0, 1);
				Mask.transform.localPosition += new Vector3(-startPivot.x * maskWidth, (1-startPivot.y) * maskHeight);
				ContentTxt1.GetComponent<RectTransform>().pivot = new Vector2(0, 1);

				if (AddContentSizeFitter)
				{
					if (ContentTxt1.GetComponent<ContentSizeFitter>() == null)
					{
						ContentTxt1.gameObject.AddComponent<ContentSizeFitter>();
					}
					if (Direction == TxtMoveDirection.左移)
					{
						ContentTxt1.GetComponent<ContentSizeFitter>().horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
						ContentTxt1.GetComponent<ContentSizeFitter>().verticalFit = ContentSizeFitter.FitMode.Unconstrained;
					}
					else if (Direction == TxtMoveDirection.上移)
					{
						ContentTxt1.GetComponent<ContentSizeFitter>().horizontalFit = ContentSizeFitter.FitMode.Unconstrained;
						ContentTxt1.GetComponent<ContentSizeFitter>().verticalFit = ContentSizeFitter.FitMode.PreferredSize;
					}
				}

				ContentTxt2 = Instantiate(ContentTxt1, Mask.transform);
			}
		}

		public void SetContent(string content)
		{
			Init();
			Timer = 0;
			gameObject.SetActive(true);
			ContentTxt1.text = content;
			ContentTxt2.text = content;
			if (AddContentSizeFitter)
			{
				LayoutRebuilder.ForceRebuildLayoutImmediate(ContentTxt1.rectTransform);
				LayoutRebuilder.ForceRebuildLayoutImmediate(ContentTxt2.rectTransform);
			}
			
			txtWidth = ContentTxt1.GetComponent<RectTransform>().rect.width;
			//Debug.Log($"文本宽度:{txtWidth},{ContentTxt1.GetComponent<RectTransform>().rect.width}");
			txtHeight = ContentTxt1.GetComponent<RectTransform>().rect.height;

			if (Direction == TxtMoveDirection.左移)
			{
				moveEndPos = -txtWidth;
				if (txtWidth > maskWidth || MoveWhenShort)
				{
					ContentTxt1.gameObject.SetActive(true);
					ContentTxt2.gameObject.SetActive(true);
					ContentTxt1.transform.localPosition = new Vector2(maskWidth*TxtInitRelativetPos, 0);
					ContentTxt2.transform.localPosition = new Vector3(maskWidth * TxtInitRelativetPos + txtWidth + IntervalDistance, 0);
				}
				else
				{
					ContentTxt1.gameObject.SetActive(true); ;
					ContentTxt2.gameObject.SetActive(false);
					ContentTxt1.transform.localPosition = new Vector3(0, 0);
				}
			}
			else if (Direction == TxtMoveDirection.上移)
			{
				moveEndPos = txtHeight;
				if (txtHeight > maskHeight || MoveWhenShort)
				{
					ContentTxt1.gameObject.SetActive(true);
					ContentTxt2.gameObject.SetActive(true);
					ContentTxt1.transform.localPosition = new Vector2(0, -maskHeight*TxtInitRelativetPos);
					ContentTxt2.transform.localPosition = new Vector3(0, -maskHeight * TxtInitRelativetPos - txtHeight - IntervalDistance);
				}
				else
				{
					ContentTxt1.gameObject.SetActive(true); 
					ContentTxt2.gameObject.SetActive(false);
					ContentTxt1.transform.localPosition = new Vector3(0, 0);
				}
			}
		}

		private void Update()
		{
			if (DelayTime>0 && Timer<DelayTime)
			{
				Timer += Time.deltaTime;
			}
			//if (Input.GetKeyDown(KeyCode.Space))
			//{
			//	SetContent(ContentTxt1.text);
			//}
		}

		private void FixedUpdate()
		{
			if (isInit && (DelayTime<=0 || Timer>DelayTime))
			{
				//Debug.Log($"文本宽度:{ContentTxt1.GetComponent<RectTransform>().rect.width}");
				if (Direction == TxtMoveDirection.左移)
				{
					if (txtWidth > maskWidth || MoveWhenShort)
					{
						ContentTxt1.transform.Translate(-Speed * Time.deltaTime, 0, 0);
						ContentTxt2.transform.Translate(-Speed * Time.deltaTime, 0, 0);
						if (ContentTxt1.transform.localPosition.x < moveEndPos)
						{
							ContentTxt1.transform.localPosition = new Vector2(ContentTxt2.transform.localPosition.x + txtWidth + IntervalDistance, 0);
						}
						if (ContentTxt2.transform.localPosition.x < moveEndPos)
						{
							ContentTxt2.transform.localPosition = new Vector2(ContentTxt1.transform.localPosition.x + txtWidth + IntervalDistance, 0);
						}
					}
				}
				else if (Direction == TxtMoveDirection.上移)
				{
					if (txtHeight > maskHeight || MoveWhenShort)
					{
						ContentTxt1.transform.Translate(0, Speed * Time.deltaTime, 0);
						ContentTxt2.transform.Translate(0, Speed * Time.deltaTime, 0);
						if (ContentTxt1.transform.localPosition.y > moveEndPos)
						{
							ContentTxt1.transform.localPosition = new Vector2(0, ContentTxt2.transform.localPosition.y - (txtHeight + IntervalDistance));
						}
						if (ContentTxt2.transform.localPosition.y > moveEndPos)
						{
							ContentTxt2.transform.localPosition = new Vector2(0, ContentTxt1.transform.localPosition.y - (txtHeight + IntervalDistance));
						}
					}
				}
			}
			
		}



	}

	public enum TxtMoveDirection
	{
		左移, 上移
	}


}



