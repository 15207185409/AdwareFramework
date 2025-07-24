using System;
using UnityEngine;
using UnityEngine.UI;

namespace XXLFramework
{
	public class TogglesPanel : MonoBehaviour
	{
		public Toggle[] Toggles;
		public RectTransform[] Contents;

		private void Start()
		{
			for (int i = 0; i < Toggles.Length; i++)
			{
				int index = i;
				Toggles[index].onValueChanged.AddListener(flag => { OnLeftToggleValueChange(flag, index); });
			}

			Toggles[0].isOn = true;
			OnLeftToggleValueChange(true, 0);
		}

		private void OnLeftToggleValueChange(bool isOn, int index)
		{
			if (isOn)
			{
				for (int i = 0; i < Contents.Length; i++)
				{
					if (i == index)
					{
						Contents[i].Show();
					}
					else
					{
						Contents[i].Hide();
					}
				}

			}
		}

		internal void ChangeTog(int index)
		{
			Toggles[index].isOn = true;
		}
	}
}