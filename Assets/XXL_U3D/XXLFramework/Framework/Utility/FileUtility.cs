
using System;
using System.Runtime.InteropServices;
using UnityEditor;
using UnityEngine;

namespace XXLFramework
{
	/// <summary>
	/// 路径管理工具，调用window打开路径和保存路径
	/// </summary>
	public class FileUtility
	{
		/// <summary>
		/// 返回要保存文件的路径
		/// </summary>
		/// <param name="FileRType">保存文件的格式</param>
		/// <param name="fileName">保存文件的名称</param>
		/// <returns></returns>
		public static string SaveProject(string FileRType, string fileName = "测试文件")
		{
			SaveFileDlg pth = new SaveFileDlg();
			pth.structSize = Marshal.SizeOf(pth);
			pth.filter = "All files (*.*)|*.*"; ;//文件类型
			pth.file = new string(new char[256]);
			pth.maxFile = pth.file.Length;
			pth.file = fileName;//保存文件的默认名字  
								//fileName就是默认值， pth.file = fileName这个语句需要放在 pth.maxFile = pth.file.Length语句后面，要不然会一直报错。
			pth.fileTitle = new string(new char[64]);
			pth.maxFileTitle = pth.fileTitle.Length;
			pth.initialDir = Application.dataPath;  // 文件的默认保存路径
			pth.title = "保存文件";//
			pth.defExt = FileRType;
			pth.flags = 0x00080000 | 0x00001000 | 0x00000800 | 0x00000200 | 0x00000008;
			if (SaveFileDialog.GetSaveFileName(pth))
			{
				string filepath = pth.file;//选择需要保存的文件路径;  
				filepath = filepath.Replace('\\', '/');
				Debug.Log($"路径为:{filepath}");
				return filepath;
			}

			return "";
		}

		/// <summary>
		/// 打开项目
		/// </summary>
		public static string OpenProject(string type)
		{
			OpenFileDlg pth = new OpenFileDlg();
			pth.structSize = Marshal.SizeOf(pth);
			//pth.filter       = "All files (*.*)|*.*";
			pth.filter = "文件(*." + type + ")\0*." + type + "";//筛选文件类型
			pth.file = new string(new char[256]);
			pth.maxFile = pth.file.Length;
			pth.fileTitle = new string(new char[64]);
			pth.maxFileTitle = pth.fileTitle.Length;
			//默认路径
			pth.initialDir = Application.streamingAssetsPath.Replace('/', '\\');  // default path
			pth.title = $"选择{type}";
			pth.defExt = type;
			pth.flags = 0x00080000 | 0x00001000 | 0x00000800 | 0x00000200 | 0x00000008;
			if (OpenFileDialog.GetOpenFileName(pth))
			{
				string filepath = pth.file; //选择的文件路径;  
				Debug.Log(filepath);
				return filepath;
			}
			return string.Empty;
		}

		private class OpenFileDialog
		{
			[DllImport("Comdlg32.dll", SetLastError = true, ThrowOnUnmappableChar = true, CharSet = CharSet.Auto)]
			public static extern bool GetOpenFileName([In, Out] OpenFileDlg ofd);
		}
		private class SaveFileDialog
		{
			[DllImport("Comdlg32.dll", SetLastError = true, ThrowOnUnmappableChar = true, CharSet = CharSet.Auto)]
			public static extern bool GetSaveFileName([In, Out] SaveFileDlg ofd);
		}
		//调用系统函数
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
		private class OpenFileDlg : FileDlg
		{

		}
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
		private class SaveFileDlg : FileDlg
		{

		}

		//[特性(布局种类、有序、字符集、自动)]
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
		private class FileDlg
		{
			public int structSize = 0;
			public IntPtr dlgOwner = IntPtr.Zero;
			public IntPtr instance = IntPtr.Zero;
			public String filter = null;
			public String customFilter = null;
			public int maxCustFilter = 0;
			public int filterIndex = 0;
			public String file = null;
			public int maxFile = 0;
			public String fileTitle = null;
			public int maxFileTitle = 0;
			public String initialDir = null;
			public String title = null;
			public int flags = 0;
			public short fileOffset = 0;
			public short fileExtension = 0;
			public String defExt = null;
			public IntPtr custData = IntPtr.Zero;
			public IntPtr hook = IntPtr.Zero;
			public String templateName = null;
			public IntPtr reservedPtr = IntPtr.Zero;
			public int reservedInt = 0;
			public int flagsEx = 0;
		}
	}

}
