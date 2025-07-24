using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace XXLFramework
{
	public abstract class AbstractGameEntry : MonoBehaviour
	{
		private void Awake()
		{
			Init();
		}
		void Init()
		{
			GameArchitecture.InitArchitecture(); //初始化架构
			UIKit.Init();       //初始化UIKit
			OnInit();
		}
		protected abstract UniTaskVoid OnInit();

	}
}
