/****************************************************************************
 * Copyright (c) 2015 - 2022  UNDER MIT License
 * 
 ****************************************************************************/

using System.Collections.Generic;

namespace XXLFramework
{

    internal class PackageTypeConfigModel : AbstractModel
    {
        private Dictionary<string, string> mTypeName2FullName = new Dictionary<string, string>()
        {
            {"fm", "Framework"},
            {"p", "Plugin"},
            {"s", "Shader"},
            {"agt", "Example/Demo"},
            {"master", "Master"},
        };

        public string GetFullTypeName(string typeName)
        {
            if (mTypeName2FullName.ContainsKey(typeName))
            {
                return mTypeName2FullName[typeName];
            }

            return typeName;
        }

        protected override void OnInit()
        {
            
        }
    }
}