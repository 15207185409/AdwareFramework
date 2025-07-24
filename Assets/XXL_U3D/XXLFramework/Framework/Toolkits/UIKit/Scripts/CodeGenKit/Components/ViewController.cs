/****************************************************************************

 * Copyright (c) 2015 ~ 2022  UNDER MIT LICENSE
 * 

 ****************************************************************************/


using System.Collections.Generic;
using UnityEngine;

namespace XXLFramework
{
	public class ViewController : BindGroup
	{
#if UNITY_EDITOR
		public override string TemplateName { get { return typeof(ViewController).Name; }}
#endif
	}
}