using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace XXLFramework
{
	public class PanelTable : UIKitTable<BasePanel>
	{
		public UIKitTableIndex<string, BasePanel> GameObjectNameIndex =
			new UIKitTableIndex<string, BasePanel>(panel => panel.gameObject.name);

		public UIKitTableIndex<Type, BasePanel> TypeIndex = new UIKitTableIndex<Type, BasePanel>(panel => panel.GetType());


		public BasePanel GetPanel(PanelKey panelKey)
		{
			if (panelKey.GameObjName.IsNotNullAndEmpty())
			{
				return GameObjectNameIndex.Get(panelKey.GameObjName);
			}
			if (panelKey.PanelType != null)
			{
				return TypeIndex.Get(panelKey.PanelType);
			}
			Debug.Log($"不存在{panelKey.ToString()}");
			return null;
		}

		protected override bool OnAdd(BasePanel item)
		{
			bool addName = GameObjectNameIndex.Add(item);
			bool addType = TypeIndex.Add(item);

			if (addName || addType)
			{
				return true;
			}
			else
			{
				return false;
			}
		}

		protected override bool OnRemove(BasePanel item)
		{
			bool removeName = GameObjectNameIndex.Remove(item);
			bool removeType = TypeIndex.Remove(item);

			if (removeName || removeType)
			{
				return true;
			}
			else
			{
				return false;
			}
		}

		protected override void OnClear()
		{
			GameObjectNameIndex.Clear();
			TypeIndex.Clear();
		}


		public override IEnumerator<BasePanel> GetEnumerator()
		{
			return GameObjectNameIndex.Dictionary.Values.GetEnumerator();
		}

		protected override void OnDispose()
		{
			GameObjectNameIndex.Dispose();
			TypeIndex.Dispose();

			GameObjectNameIndex = null;
			TypeIndex = null;
		}

	}
}
