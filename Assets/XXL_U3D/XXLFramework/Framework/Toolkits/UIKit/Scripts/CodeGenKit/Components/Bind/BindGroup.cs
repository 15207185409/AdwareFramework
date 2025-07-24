using System.Collections.Generic;
using UnityEngine;

namespace XXLFramework
{
	public abstract class BindGroup : MonoBehaviour
	{
		protected Dictionary<string, string> InitValues = new Dictionary<string, string>();
#if UNITY_EDITOR
		public abstract string TemplateName { get;}

		[HideInInspector] public string Namespace = string.Empty;

		[HideInInspector] public string ScriptName;

		[HideInInspector] public string ScriptsFolder = string.Empty;

		[HideInInspector] public bool GeneratePrefab = false;

		[HideInInspector] public string PrefabFolder = string.Empty;

		[HideInInspector]
		public List<BindDetail> BindDetails = new List<BindDetail>();

		public void AddBindObj(BindDetail item)
		{
			bool isContainBind = JudgeContainBindObj(item);
			if (!isContainBind)
			{
				BindDetails.Add(item);
			}
		}

		private bool JudgeContainBindObj(BindDetail bind)
		{
			foreach (var item in BindDetails)
			{
				if (item.BindObj == bind.BindObj)
				{
					return true;
				}
			}
			return false;
		}

		public void RemoveBindObj(BindDetail item)
		{
			if (BindDetails.Contains(item))
			{
				BindDetails.Remove(item);
			}
		}
#endif

		protected virtual void RecordInitValue(){ }

		protected virtual void Reset(){ }

	}
	
}
