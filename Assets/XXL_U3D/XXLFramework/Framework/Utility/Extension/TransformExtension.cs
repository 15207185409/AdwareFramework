using UnityEngine;

namespace XXLFramework
{
	public static class TransformExtension
	{
		/// <summary>
		/// 添加collider
		/// </summary>
		/// <param name="transform"></param>
		public static void AddCollider(this Transform transform, bool convex = false)
		{
			MeshRenderer[] meshRenderers = transform.GetComponentsInChildren<MeshRenderer>();
			foreach (var item in meshRenderers)
			{
				item.GetOrAddComponent<MeshCollider>().convex = convex;
			}
		}

		public static Transform FindDeep(this Transform FatherTrans, string childName)
		{
			if (childName == "")
				return null;

			Transform child = FatherTrans.Find(childName);
			if (child != null)
				return child;

			Transform go = null;
			for (int i = 0; i < FatherTrans.childCount; i++)
			{
				child = FatherTrans.GetChild(i);
				go = FindDeep(child, childName);
				if (go != null)
					return go;
			}
			return null;
		}

	}
}
