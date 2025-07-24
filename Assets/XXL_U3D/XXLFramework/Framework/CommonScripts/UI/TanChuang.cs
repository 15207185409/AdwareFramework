using UnityEngine;
using UnityEngine.UI;

namespace XXLFramework
{
	public class TanChuang : MonoBehaviour
	{
		private Button[] CloseButtons;

		// Start is called before the first frame update
		void Start()
		{
			CloseButtons = GetComponentsInChildren<Button>();
			foreach (var item in CloseButtons)
			{
				item.onClick.AddListener(Hide);
			}
		}

		public void Show()
		{
			GetComponent<RectTransform>().OpenAni();
		}

		public void Hide()
		{
			GetComponent<RectTransform>().CloseAni();
		}

	}
}