using UnityEditor;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;
using UnityEngine;
using Sirenix.Utilities;

namespace XXLFramework
{
	public class GameAssetsOdinEditorWindow : OdinMenuEditorWindow
	{
		[MenuItem("XXLFramework/GameConfig %g")]
		private static void OpenWindow()
		{
			var window = GetWindow<GameAssetsOdinEditorWindow>();
			window.position = GUIHelper.GetEditorWindowRect().AlignCenter(1080, 720);
			window.titleContent = new GUIContent("项目数据配置");
		}

		protected override OdinMenuTree BuildMenuTree()
		{
			var tree = new OdinMenuTree();
			//这里的第一个参数为窗口名字，第二个参数为指定目录，第三个参数为需要什么类型，第四个参数为是否在家该文件夹下的子文件夹
			tree.AddAllAssetsAtPath("游戏资源", "Assets/XXL_U3D", typeof(UIPanelAssets), true);
			tree.Add("数据配置", new ConfigDataEditor());
			return tree;
		}
	}

}