using UnityEngine;
using UnityEditor;

namespace XXLFramework
{
    //定制Serializable类的每个实例的GUI
    [CustomPropertyDrawer(typeof(BindDetail))]
    public class BindDetailDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            //创建一个属性包装器，用于将常规GUI控件与SerializedProperty一起使用
            using (new EditorGUI.PropertyScope(position, label, property))
            {              
                //输入框高度，默认一行的高度
                position.height = EditorGUIUtility.singleLineHeight;

				Rect bindObjRect = new Rect(position)
				{
					width = 150,
					height = position.height
				};

				Rect ComponentNamesRect = new Rect(position)
				{
					width = 200,
					x = position.x+160
				};

				//找到每个属性的序列化值
				SerializedProperty BindObjProperty = property.FindPropertyRelative("BindObj");
				SerializedProperty ComponentNamesProperty = property.FindPropertyRelative("ComponentNames");
				SerializedProperty ComponentNameIndexProperty = property.FindPropertyRelative("ComponentNameIndex");
				SerializedProperty ComponentNameProperty = property.FindPropertyRelative("ComponentName");

				//GUILayout.BeginHorizontal();
				EditorGUI.ObjectField(bindObjRect, BindObjProperty.objectReferenceValue, typeof(Component), false);

				string[] ComponentNameList = new string[ComponentNamesProperty.arraySize];
				for (int i = 0; i < ComponentNamesProperty.arraySize; i++)
				{
					string name = ComponentNamesProperty.GetArrayElementAtIndex(i).stringValue;
					ComponentNameList[i] = name;
				}
				EditorGUI.BeginChangeCheck();
				ComponentNameIndexProperty.intValue = EditorGUI.Popup(ComponentNamesRect, ComponentNameIndexProperty.intValue, ComponentNameList);
				if (EditorGUI.EndChangeCheck())
				{
					ComponentNameProperty.stringValue = ComponentNameList[ComponentNameIndexProperty.intValue];
				}
				//GUILayout.EndHorizontal();

			}
        }
    }
}
