using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace XXLFramework
{
	//透明和实化的转化
	public class EmptyToFullModel : MonoBehaviour
	{

		public List<MeshRenderer> m_meshRenders = new List<MeshRenderer>();

		public List<Material> m_Materials = new List<Material>();
		public List<Material> m_MaterialList = new List<Material>();
		public List<RenderingMode> listRenderingMode = new List<RenderingMode>(); //存储之前的渲染模式

		public float emptyDegree = 0.1f;

		// Use this for initialization
		void Awake()
		{
			//Init();
		}

		public void Init()
		{
			try
			{
				if (m_Materials == null)
				{
					m_Materials = new List<Material>();
				}

				m_meshRenders.AddRange(transform.GetComponentsInChildren<MeshRenderer>());

				foreach (MeshRenderer myMeshRender in m_meshRenders)
				{
					m_Materials = myMeshRender.materials.ToList();
					for (int i = 0; i < m_Materials.Count; i++)
					{
						m_MaterialList.Add(m_Materials[i]);
						listRenderingMode.Add(GetRenderingMode(m_Materials[i]));
					}
				}
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
				throw;
			}
		}

		/// <summary>
		/// 获取材质球上的RenderingMode
		/// </summary>
		/// <param name="material"></param>
		/// <returns></returns>
		private RenderingMode GetRenderingMode(Material material)
		{
			string renderType = material.GetTag("RenderType", true);
			//Debug.Log(renderType);
			if (renderType.Equals("Opaque"))
			{
				return RenderingMode.Opaque;
			}
			else if (renderType.Equals("Cutout"))
			{
				return RenderingMode.Cutout;
			}
			else if (renderType.Equals("Fade"))
			{
				return RenderingMode.Fade;
			}
			else if (renderType.Equals("Transparent"))
			{
				return RenderingMode.Transparent;
			}
			else
			{
				return RenderingMode.Opaque;
			}

		}

		//虚化物体
		public void EmptyModel(float during = 0f)
		{
			if (during <= 0)
			{
				for (int i = 0; i < m_MaterialList.Count; i++)
				{
					if (m_MaterialList[i].color != null)
					{
						m_MaterialList[i].color = new Color(m_MaterialList[i].color.r, m_MaterialList[i].color.g, m_MaterialList[i].color.b, emptyDegree);
						SetMaterialRenderingMode(m_MaterialList[i], RenderingMode.Fade);
					}

				}
			}
			else
			{
				StartCoroutine("EmptyModelCoroutine", during);
			}
		}

		//虚化物体过程
		private IEnumerator EmptyModelCoroutine(float during)
		{
			float timer = 0;
			while (timer <= during)
			{
				timer += Time.deltaTime;
				float alph = Mathf.Lerp(1, emptyDegree, timer / during);
				Debug.Log("alph值:" + alph);
				for (int i = 0; i < m_MaterialList.Count; i++)
				{
					m_MaterialList[i].color = new Color(m_MaterialList[i].color.r, m_MaterialList[i].color.g, m_MaterialList[i].color.b, alph);
					SetMaterialRenderingMode(m_MaterialList[i], RenderingMode.Fade);
				}
				yield return null;
			}
		}

		//实化物体
		public void FullModel(float during = 0)
		{
			if (during <= 0)
			{
				for (int i = 0; i < m_MaterialList.Count; i++)
				{
					m_MaterialList[i].color = new Color(m_MaterialList[i].color.r, m_MaterialList[i].color.g, m_MaterialList[i].color.b, 1);
					SetMaterialRenderingMode(m_MaterialList[i], listRenderingMode[i]);
				}
			}
			else
			{
				StartCoroutine("FullModelCoroutine", during);
			}
		}

		//实化物体协程
		private IEnumerator FullModelCoroutine(float during)
		{
			float timer = 0;
			while (timer <= during)
			{
				timer += Time.deltaTime;
				float alph = Mathf.Lerp(emptyDegree, 1, timer / during);
				//Debug.Log("alph值:" + alph);
				for (int i = 0; i < m_MaterialList.Count; i++)
				{
					m_MaterialList[i].color = new Color(m_MaterialList[i].color.r, m_MaterialList[i].color.g, m_MaterialList[i].color.b, alph);
					SetMaterialRenderingMode(m_MaterialList[i], RenderingMode.Fade);
				}
				yield return null;
			}
			for (int i = 0; i < m_MaterialList.Count; i++)
			{
				m_MaterialList[i].color = new Color(m_MaterialList[i].color.r, m_MaterialList[i].color.g, m_MaterialList[i].color.b, 1);
				SetMaterialRenderingMode(m_MaterialList[i], listRenderingMode[i]);
			}
		}


		public void VisiuableModel(GameObject model)
		{
			List<MeshRenderer> m_meshRenders_ = new List<MeshRenderer>();
			List<SkinnedMeshRenderer> SkinnedMeshRenderer = new List<SkinnedMeshRenderer>();
			m_meshRenders_.AddRange(model.transform.GetComponentsInChildren<MeshRenderer>());
			SkinnedMeshRenderer.AddRange(model.transform.GetComponentsInChildren<SkinnedMeshRenderer>());
			foreach (MeshRenderer meshRenderer in m_meshRenders_)
			{
				Material material = meshRenderer.material;
				material.color = new Color(material.color.r, material.color.g, material.color.b, 1f);
				SetMaterialRenderingMode(material, RenderingMode.Fade);
			}
			foreach (SkinnedMeshRenderer meshRenderer in SkinnedMeshRenderer)
			{
				Material material = meshRenderer.material;
				material.color = new Color(material.color.r, material.color.g, material.color.b, 1f);
				SetMaterialRenderingMode(material, RenderingMode.Fade);
			}
		}


		public enum RenderingMode
		{
			Opaque,
			Cutout,
			Fade,
			Transparent,
		}
		//设置材质RenderingMode 并且可设置透明度
		public void SetMaterialRenderingMode(Material material, RenderingMode renderingMode)
		{
			//Debug.Log(material.name);
			switch (renderingMode)
			{
				case RenderingMode.Opaque:
					material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
					material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
					material.SetInt("_ZWrite", 1);
					material.DisableKeyword("_ALPHATEST_ON");
					material.DisableKeyword("_ALPHABLEND_ON");
					material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
					material.renderQueue = -1;
					material.SetInt("_Mode", 0);
					break;
				case RenderingMode.Cutout:
					material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
					material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
					material.SetInt("_ZWrite", 1);
					material.EnableKeyword("_ALPHATEST_ON");
					material.DisableKeyword("_ALPHABLEND_ON");
					material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
					material.renderQueue = 2450;
					material.SetInt("_Mode", 1);
					break;
				case RenderingMode.Fade:
					material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
					material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
					material.SetInt("_ZWrite", 0);
					material.DisableKeyword("_ALPHATEST_ON");
					material.EnableKeyword("_ALPHABLEND_ON");
					material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
					material.renderQueue = 3000;
					material.SetInt("_Mode", 2);
					break;
				case RenderingMode.Transparent:
					material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
					material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
					material.SetInt("_ZWrite", 0);
					material.DisableKeyword("_ALPHATEST_ON");
					material.DisableKeyword("_ALPHABLEND_ON");
					material.EnableKeyword("_ALPHAPREMULTIPLY_ON");
					material.renderQueue = 3000;
					material.SetInt("_Mode", 3);
					break;
			}
		}

	}
}
