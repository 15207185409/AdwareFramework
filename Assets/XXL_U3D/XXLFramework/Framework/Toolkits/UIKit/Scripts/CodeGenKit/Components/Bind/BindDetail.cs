using UnityEngine;
using UnityEngine.UI;
using System;
using System.Linq;

namespace XXLFramework
{
	[Serializable]
	public class BindDetail
	{
		public GameObject BindObj;
		public string[] ComponentNames;
		public int ComponentNameIndex;
		public string ComponentName;

		public BindDetail(GameObject bindObj)
		{
			this.BindObj = bindObj;
			var components = bindObj.GetComponents<Component>();
			this.ComponentNames = components.Select(c => c.GetType().FullName).ToArray();
			ComponentName = GetDefaultComponentName(BindObj);
			this.ComponentNameIndex = this.ComponentNames.ToList()
				.FindIndex((componentName) => componentName.Contains(ComponentName));

			if (ComponentNameIndex == -1 || ComponentNameIndex >= ComponentNames.Length)
			{
				ComponentNameIndex = 0;
			}
		}

		public void ChangeComponent(int componentNameIndex)
		{
			ComponentName = ComponentNames[componentNameIndex];
		}

		public void Refresh()
		{
			var components = BindObj.GetComponents<Component>();
			this.ComponentNames = components.Select(c => c.GetType().FullName).ToArray();
			this.ComponentNameIndex = this.ComponentNames.ToList()
				.FindIndex((componentName) => componentName.Contains(ComponentName));

			if (ComponentNameIndex == -1 || ComponentNameIndex >= ComponentNames.Length)
			{
				ComponentNameIndex = 0;
			}
		}

		/// <summary>
		/// 组件获得优先级，可以节省一部分操作时间
		/// </summary>
		/// <returns></returns>
		string GetDefaultComponentName(GameObject bindObj)
		{
			if (bindObj.GetComponent<ViewController>()) return bindObj.GetComponent<ViewController>().GetType().FullName;


			if (bindObj.GetComponent("SkeletonAnimation")) return "SkeletonAnimation";
			if (bindObj.GetComponent<ScrollRect>()) return "UnityEngine.UI.ScrollRect";
			if (bindObj.GetComponent<InputField>()) return "UnityEngine.UI.InputField";

			// text mesh pro supported
			if (bindObj.GetComponent("TMP.TextMeshProUGUI")) return "TMP.TextMeshProUGUI";
			if (bindObj.GetComponent("TMPro.TextMeshProUGUI")) return "TMPro.TextMeshProUGUI";
			if (bindObj.GetComponent("TMPro.TextMeshPro")) return "TMPro.TextMeshPro";
			if (bindObj.GetComponent("TMPro.TMP_InputField")) return "TMPro.TMP_InputField";
			if (bindObj.GetComponent("TMPro.TMP_Dropdown")) return "TMPro.TMP_Dropdown";

			// ugui bind
			if (bindObj.GetComponent<Dropdown>()) return "UnityEngine.UI.Dropdown";
			if (bindObj.GetComponent<Button>()) return "UnityEngine.UI.Button";
			if (bindObj.GetComponent<Text>()) return "UnityEngine.UI.Text";
			if (bindObj.GetComponent<RawImage>()) return "UnityEngine.UI.RawImage";
			if (bindObj.GetComponent<Toggle>()) return "UnityEngine.UI.Toggle";
			if (bindObj.GetComponent<Slider>()) return "UnityEngine.UI.Slider";
			if (bindObj.GetComponent<Scrollbar>()) return "UnityEngine.UI.Scrollbar";
			if (bindObj.GetComponent<Image>()) return "UnityEngine.UI.Image";
			if (bindObj.GetComponent<ToggleGroup>()) return "UnityEngine.UI.ToggleGroup";

			// other
			if (bindObj.GetComponent<Rigidbody>()) return "Rigidbody";
			if (bindObj.GetComponent<Rigidbody2D>()) return "Rigidbody2D";

			if (bindObj.GetComponent<BoxCollider2D>()) return "BoxCollider2D";
			if (bindObj.GetComponent<BoxCollider>()) return "BoxCollider";
			if (bindObj.GetComponent<CircleCollider2D>()) return "CircleCollider2D";
			if (bindObj.GetComponent<SphereCollider>()) return "SphereCollider";
			if (bindObj.GetComponent<MeshCollider>()) return "MeshCollider";

			if (bindObj.GetComponent<Collider>()) return "Collider";
			if (bindObj.GetComponent<Collider2D>()) return "Collider2D";

			if (bindObj.GetComponent<Animator>()) return "Animator";
			if (bindObj.GetComponent<Canvas>()) return "Canvas";
			if (bindObj.GetComponent<Camera>()) return "Camera";
			if (bindObj.GetComponent("Empty4Raycast")) return "XXLFramework.Empty4Raycast";
			if (bindObj.GetComponent<RectTransform>()) return "RectTransform";
			if (bindObj.GetComponent<MeshRenderer>()) return "MeshRenderer";

			if (bindObj.GetComponent<SpriteRenderer>()) return "SpriteRenderer";

			return "Transform";
		}


	}
}
