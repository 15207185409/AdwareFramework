/****************************************************************************
 * Copyright (c) 2015 - 2022  UNDER MIT License
 * 

 ****************************************************************************/

#if UNITY_EDITOR
using System.Reflection;
using System.Text;

namespace XXLFramework
{
    public class PropertyAPIRenderInfo
    {
        private readonly PropertyInfo mPropertyInfo;

        public PropertyAPIRenderInfo(PropertyInfo propertyInfo)
        {
            mPropertyInfo = propertyInfo;
        }


        public void BuildString(StringBuilder builder)
        {
            var exampleCodeAttribute = mPropertyInfo.GetCustomAttribute<APIExampleCodeAttribute>();

            var description = string.Empty;

            if (LocaleKitEditor.IsCN.Value)
            {
                description = mPropertyInfo.GetAttribute<APIDescriptionCNAttribute>().Description;
            }
            else
            {
                description = mPropertyInfo.GetAttribute<APIDescriptionENAttribute>(false).Description;
            }

            builder
                .Append("|").Append(mPropertyInfo.PropertyType.Name + " " + mPropertyInfo.Name).Append("|")
                .Append(description).AppendLine("|")
                .AppendLine("|-|-|")
                .Append("|").Append(APIDocLocale.ExampleCode).Append("|").AppendLine(" |")
                .AppendLine("```")
                .AppendLine(exampleCodeAttribute.Code)
                .AppendLine("```")
                .AppendLine("--------");
        }
    }
}
#endif