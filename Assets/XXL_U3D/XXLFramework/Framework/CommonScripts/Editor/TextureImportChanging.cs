using UnityEditor;
using UnityEngine;

public class TextureImportChanging : EditorWindow
{
	enum MaxSize
	{
		Size_32 = 32,
		Size_64 = 64,
		Size_128 = 128,
		Size_256 = 256,
		Size_512 = 512,
		Size_1024 = 1024,
		Size_2048 = 2048,
		Size_4096 = 4096,
		Size_8192 = 8192,
	}

	enum ChangeTextureMode
	{
		压缩,
		扩大,
		统一修改
	}

	private static TextureImporterFormat textureImporterFormat
	{
		get
		{
#if UNITY_WEBGL
			return TextureImporterFormat.DXT5Crunched;
#else
            return TextureImporterFormat.Automatic;
#endif
		}
	}
	// ----------------------------------------------------------------------------  
	static TextureImporterType textureType = TextureImporterType.Sprite;
	/// <summary>
	/// 格式
	/// </summary>
	static TextureImporterFormat textureFormat = textureImporterFormat;
	MaxSize textureSize = MaxSize.Size_1024;
	/// <summary>
	/// 压缩
	/// </summary>
	TextureImporterCompression textureCompression = TextureImporterCompression.Uncompressed;

	/// <summary>
	/// 修改Texture的模式，选择压缩模式只会修改尺寸大于设定尺寸的图片，
	/// 选择扩大模式时只会修改尺寸小于设定尺寸的图片，选择统一修改模式时会统一设置为设定尺寸
	/// </summary>
	static ChangeTextureMode ChangeMode = ChangeTextureMode.压缩;

	static TextureImportChanging window;
	[@MenuItem("XXLFramework/设置图片格式")]
	private static void Init()
	{
		Rect wr = new Rect(0, 0, 400, 400);
		window = (TextureImportChanging)EditorWindow.GetWindowWithRect(typeof(TextureImportChanging), wr, false, "图片格式设置");
		window.Show();
	}

	private void OnGUI()
	{
		EditorGUILayout.Space();
		EditorGUILayout.HelpBox("设置选中图片或选中路径下的图片属性", MessageType.Info);
		EditorGUILayout.Space();

		textureCompression = (TextureImporterCompression)EditorGUILayout.EnumPopup("压缩:", textureCompression);

		EditorGUILayout.Space();
		EditorGUILayout.HelpBox("如果有导入自动设置图片脚本,下面方法未必执行\n 统一OverrideSize值,则所选图片的该值都设置为所选大小,否则为图片的宽高最大值", MessageType.Info);
		EditorGUILayout.Space();
		ChangeMode = (ChangeTextureMode)EditorGUILayout.EnumPopup("尺寸修改模式:", ChangeMode);
		textureSize = (MaxSize)EditorGUILayout.EnumPopup("尺寸:", textureSize);

		EditorGUILayout.Space();

		if (GUILayout.Button("设置"))
		{
			TextureImporterPlatformSettings t = new TextureImporterPlatformSettings();

			t.format = textureFormat;

			t.maxTextureSize = (int)textureSize;
			t.textureCompression = textureCompression;

			SelectedChangeTextureFormatSettings(t, textureType, GetSelectedTextures());
		}

	}


	void SelectedChangeTextureFormatSettings(TextureImporterPlatformSettings _t, TextureImporterType _type, Object[] arr)
	{

		Object[] textures = arr;
		if (window == null)
			Init();
		if (textures != null)
		{
			if (textures.Length < 1)
			{
				window.ShowNotification(new GUIContent("找不到图片!"));
				return;
			}
		}
		else
		{
			window.ShowNotification(new GUIContent("请选中图片或路径!"));
			return;
		}
		Selection.objects = new Object[0];
		int i = 0;
		foreach (Texture2D texture in textures)
		{
			string path = AssetDatabase.GetAssetPath(texture);

			if (path.Substring(path.Length - 3, 3) == "dds")
				continue;
			string[] pathArr = path.Split('/');

			TextureImporter textureImporter = AssetImporter.GetAtPath(path) as TextureImporter;

			int size = GetSize(textureImporter.maxTextureSize, _t.maxTextureSize);


			textureImporter.allowAlphaSplitting = _t.allowsAlphaSplitting;

			textureImporter.textureCompression = _t.textureCompression;

			textureImporter.maxTextureSize = size;
			textureImporter.spritePackingTag = pathArr[pathArr.Length - 2];

			//TextureImporterPlatformSettings webglPlatFormSetting = textureImporter.GetPlatformTextureSettings("WebGl");
			//webglPlatFormSetting.overridden = true;
			//webglPlatFormSetting.format = GetFormat(textureImporter, "WebGl");
			//webglPlatFormSetting.maxTextureSize = _t.maxTextureSize;
			//textureImporter.SetPlatformTextureSettings(webglPlatFormSetting);

			//TextureImporterPlatformSettings pcPlatFormSetting = textureImporter.GetPlatformTextureSettings("PC");
			//pcPlatFormSetting.overridden = true;
			//pcPlatFormSetting.format = GetFormat(textureImporter, "PC");
			//pcPlatFormSetting.maxTextureSize = _t.maxTextureSize;
			//textureImporter.SetPlatformTextureSettings(pcPlatFormSetting);


			ShowProgress((float)i / (float)textures.Length, textures.Length, i);
			i++;
			AssetDatabase.ImportAsset(path);
		}
		AssetDatabase.Refresh();
		EditorUtility.ClearProgressBar();
		textures = null;
	}


	private int GetSize(int selfSize, int setSize)
	{
		int result = setSize;
		
		switch (ChangeMode)
		{
			case ChangeTextureMode.压缩:
				result = selfSize > setSize ? setSize : selfSize;
				break;
			case ChangeTextureMode.扩大:
				result = selfSize < setSize ? setSize : selfSize;
				break;
			case ChangeTextureMode.统一修改:
				break;
			default:
				break;
		}
		Debug.Log($"selfsize:{(MaxSize)selfSize},setSize:{(MaxSize)setSize},result:{(MaxSize)result}");
		return result;
	}


	public static void ShowProgress(float val, int total, int cur)
	{
		EditorUtility.DisplayProgressBar("设置图片中...", string.Format("请稍等({0}/{1}) ", cur, total), val);
	}


	static Object[] GetSelectedTextures()
	{
		return Selection.GetFiltered(typeof(Texture2D), SelectionMode.DeepAssets);
	}

	static TextureImporterFormat GetFormat(TextureImporter item, string platformName)
	{
		TextureImporterFormat format = textureFormat;
		bool isNormalMap = item.textureType == TextureImporterType.NormalMap;
		switch (platformName)
		{
			case "WebGl":
				format = isNormalMap || item.DoesSourceTextureHaveAlpha() ? TextureImporterFormat.DXT5Crunched : TextureImporterFormat.DXT1Crunched;
				break;
			case "PC":
				format = isNormalMap || item.DoesSourceTextureHaveAlpha() ? TextureImporterFormat.DXT5 : TextureImporterFormat.DXT1;
				break;
			default:
				break;
		}
		return format;

	}

	void OnInspectorUpdate()
	{
		Repaint();
	}

}