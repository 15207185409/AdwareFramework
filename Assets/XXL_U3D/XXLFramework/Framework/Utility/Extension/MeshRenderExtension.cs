using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XXLFramework
{
	public static class MeshRenderExtension
	{
		public static void SetMaterial(this MeshRenderer meshRenderer,Material replaceMat, int index)
		{
			Material[] mats = meshRenderer.materials;
			mats[index] = replaceMat;
			meshRenderer.materials = mats;
		}

		public static void SetMaterials(this MeshRenderer meshRenderer, Material[] replaceMats)
		{
			meshRenderer.materials = replaceMats;
		}
	}
}