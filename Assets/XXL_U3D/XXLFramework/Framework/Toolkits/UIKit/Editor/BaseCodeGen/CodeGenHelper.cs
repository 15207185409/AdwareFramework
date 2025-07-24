/****************************************************************************
 * Copyright (c) 2015 ~ 2022  UNDER MIT LICENSE
 * 

 ****************************************************************************/

#if UNITY_EDITOR
using System.Text;
using UnityEngine;

namespace XXLFramework
{
    public static class CodeGenHelper
    {                  
        public static string PathToParent(Transform trans, string parentName)
        {
            var retValue = new StringBuilder(trans.name);

            while (trans.parent != null)
            {
                if (trans.parent.name.Equals(parentName))
                {
                    break;
                }

                
                retValue = trans.parent.name.Builder().Append("/").Append(retValue);

                trans = trans.parent;
            }

            return retValue.ToString();
        }
    }
}
#endif