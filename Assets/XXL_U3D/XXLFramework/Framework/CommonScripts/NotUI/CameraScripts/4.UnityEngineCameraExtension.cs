/****************************************************************************
 * Copyright (c) 2015 - 2022  UNDER MIT License
 * 

 ****************************************************************************/

using DG.Tweening;
using UnityEngine;

namespace XXLFramework
{
#if UNITY_EDITOR
    [ClassAPI("00.FluentAPI.Unity", "UnityEngine.Camera", 4)]
    [APIDescriptionCN("UnityEngine.Camera 静态扩展")]
    [APIDescriptionEN("UnityEngine.Camera extension")]
#endif
    public static class UnityEngineCameraExtension
    {
		public static void FocusTarget(this Camera camera, Transform target, Vector3 offset, float duration = 0)
		{
			Vector3 position = target.position + offset;
			Vector3 eulerAngle = Quaternion.LookRotation(-offset).eulerAngles;
			if (camera.GetComponent<Free_Camera>()!=null)
			{
				camera.GetComponent<Free_Camera>().OnLocate(position, eulerAngle, duration);
			}
			else
			{
				camera.transform.DOMove(position, duration);
				camera.transform.DORotate(eulerAngle, duration);
			}
			
		}

	}
}