/****************************************************************************
 * Copyright (c) 2016 ~ 2022  UNDER MIT LICENSE
 * 
 ****************************************************************************/

#if UNITY_EDITOR
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace XXLFramework
{
    [PackageKitGroup("XXLFramework")]
    [PackageKitRenderOrder(6)]
    [DisplayNameCN("使用范例")]
    [DisplayNameEN("Use Example")]
    public class UseExample : IPackageKitView
    {
        public class UseExampleItem
        {
            public string FolderName;
            public string FileName;
            public string FilePath;
        }

        public class UseExampleItemGroup
        {
            public string FolderName;
            public bool Open;
            public List<UseExampleItem> Items { get; set; }

            public bool IsRoot;
        }

        private List<UseExampleItem> mViews = null;
        private List<UseExampleItemGroup> mGroups = null;

        private VerticalSplitView mSplitView = null;
        private Rect mLeftRect;
        private Rect mRightRect;
        private IMGUILayout mLeftLayout;
        private IMGUILayout mRightLayout;

        private UseExampleItem mSelectedView = null;

        private MDViewer mMarkdownViewer;

        public void Init()
        {
            //EditorApplication.update += Update;
            mViews = new List<UseExampleItem>();

            var positionMarkForLoad = Resources.Load<TextAsset>("EditorUseExample/PositionMarkForLoad");

            var path = AssetDatabase.GetAssetPath(positionMarkForLoad);
            var folderPath = path.GetFolderPath();
            var folderName = folderPath.GetFileName();
            var markdownFilePaths = Directory.GetFiles(folderPath, "*.md", SearchOption.AllDirectories);

            mMarkdownViewer = new MDViewer(Resources.Load<GUISkin>("Skin/MarkdownSkinQS"), path, "");

            foreach (var filePath in markdownFilePaths)
            {
                mViews.Add(new UseExampleItem()
                {
                    FileName = filePath.GetFileNameWithoutExtend(),
                    FolderName = filePath.GetFolderPath().GetFileName(),
                    FilePath = filePath,
                });
            }


            if (mViews.Count > 0)
            {
                mSelectedView = mViews.First();
                mMarkdownViewer.UpdateText(AssetDatabase.LoadAssetAtPath<TextAsset>(mSelectedView.FilePath).text);
            }

            mGroups = mViews.GroupBy(v => v.FolderName).OrderBy(g =>
            {
                var number = g.Key.Split('.').First();
                if (int.TryParse(number, out var order))
                {
                    return order;
                }

                return -1;
            }).Select(g => new UseExampleItemGroup()
            {
                FolderName = g.Key,
                IsRoot = g.Key == folderName,
                Items = g.ToList()
            }).ToList();

            // 创建双屏
            mSplitView = new VerticalSplitView(180)
            {
                FirstPan = rect =>
                {
                    mLeftRect = rect;
                    mLeftLayout.DrawGUI();
                },
                SecondPan = rect =>
                {
                    mRightRect = rect;
                    mRightLayout.DrawGUI();
                }
            };

            var scrollPos = Vector2.zero;

            mLeftLayout = EasyIMGUI.Area().WithRectGetter(() => mLeftRect)
                .AddChild(EasyIMGUI.Custom().OnGUI(() =>
                {
                    GUILayout.BeginHorizontal();


                    GUILayout.BeginVertical();
                    GUILayout.Space(20);
                    GUILayout.EndVertical();

                    if (mSplitView.Expand.Value)
                    {
                        GUILayout.FlexibleSpace();

                        if (GUILayout.Button("<"))
                        {
                            mSplitView.Expand.Value = false;
                        }
                    }

                    GUILayout.EndHorizontal();
                }))
                .AddChild(EasyIMGUI.Custom().OnGUI(() =>
                {
                    scrollPos = GUILayout.BeginScrollView(scrollPos);

                    foreach (var UseExampleItemGroup in mGroups)
                    {
                        if (UseExampleItemGroup.IsRoot)
                        {
                            foreach (var UseExampleItem in UseExampleItemGroup.Items)
                            {
                                GUILayout.BeginVertical("box");

                                GUILayout.BeginHorizontal();
                                {
                                    GUILayout.Label(UseExampleItem.FileName);
                                    GUILayout.FlexibleSpace();
                                }
                                GUILayout.EndHorizontal();

                                GUILayout.EndVertical();

                                var rect = GUILayoutUtility.GetLastRect();

                                if (Equals(mSelectedView, UseExampleItem))
                                {
                                    GUI.Box(rect, "", mSelectionRect);
                                }

                                if (rect.Contains(Event.current.mousePosition) &&
                                    Event.current.type == EventType.MouseUp)
                                {
                                    mSelectedView = UseExampleItem;
                                    var textAsset = AssetDatabase.LoadAssetAtPath<TextAsset>(mSelectedView.FilePath);
                                    mMarkdownViewer.UpdateText(textAsset.text);
                                    mMarkdownViewer.MarkdownFilePath = mSelectedView.FilePath;
                                    mMarkdownViewer.ResetScrollPos();
                                    Event.current.Use();
                                }
                            }
                        }
                        else
                        {
                            GUILayout.BeginVertical("box");

                            if (EditorGUILayout.Foldout(UseExampleItemGroup.Open, UseExampleItemGroup.FolderName,
                                    true))
                            {
                                UseExampleItemGroup.Open = true;
                                GUILayout.EndVertical();

                                foreach (var UseExampleItem in UseExampleItemGroup.Items)
                                {
                                    GUILayout.BeginVertical("box");

                                    GUILayout.BeginHorizontal();
                                    {
                                        GUILayout.Space(20);
                                        GUILayout.Label(UseExampleItem.FileName);
                                        GUILayout.FlexibleSpace();
                                    }
                                    GUILayout.EndHorizontal();

                                    GUILayout.EndVertical();

                                    var rect = GUILayoutUtility.GetLastRect();

                                    if (Equals(mSelectedView, UseExampleItem))
                                    {
                                        GUI.Box(rect, "", mSelectionRect);
                                    }

                                    if (rect.Contains(Event.current.mousePosition) &&
                                        Event.current.type == EventType.MouseUp)
                                    {
                                        mSelectedView = UseExampleItem;
                                        var textAsset =
                                            AssetDatabase.LoadAssetAtPath<TextAsset>(mSelectedView.FilePath);
                                        mMarkdownViewer.UpdateText(textAsset.text);
                                        mMarkdownViewer.MarkdownFilePath = mSelectedView.FilePath;
                                        mMarkdownViewer.ResetScrollPos();
                                        Event.current.Use();
                                    }
                                }
                            }
                            else
                            {
                                UseExampleItemGroup.Open = false;
                                GUILayout.EndVertical();
                            }
                        }
                    }


                    GUILayout.EndScrollView();

                    if (GUILayout.Button(LocaleKitEditor.IsCN.Value ? "导出" : "Export"))
                    {
                        var builder = new StringBuilder();
                        foreach (var UseExampleItemGroup in mGroups)
                        {
                            builder.Append("# " + UseExampleItemGroup.FolderName);
                            builder.AppendLine();
                            foreach (var UseExampleItem in UseExampleItemGroup.Items)
                            {
                                var content = File.ReadAllText(UseExampleItem.FilePath);
                                builder.Append(content);
                                builder.AppendLine();
                            }
                        }

                        var framework = PackageKit.Interface.GetModel<LocalPackageVersionModel>().GetByName("Framework");


                        var UseExampleText = LocaleKitEditor.IsCN.Value ? "使用指南 " : "UseExample";
                        
                        var savedPath = EditorUtility.SaveFilePanel($"XXLFramework {framework.Version} {UseExampleText}", Application.dataPath,
                            $"XXLFramework {framework.Version} {UseExampleText}", "md");

                        File.WriteAllText(savedPath, builder.ToString());

                        EditorUtility.RevealInFinder(savedPath);
                    }

                    GUILayout.Space(5);
                }));


            mRightLayout = EasyIMGUI.Area().WithRectGetter(() => mRightRect)
                .AddChild(EasyIMGUI.Custom().OnGUI(() =>
                {
                    GUILayout.BeginHorizontal();


                    if (!mSplitView.Expand.Value)
                    {
                        if (GUILayout.Button(">"))
                        {
                            mSplitView.Expand.Value = true;
                        }

                        GUILayout.FlexibleSpace();
                    }

                    GUILayout.BeginVertical();
                    GUILayout.Space(20);
                    GUILayout.EndVertical();

                    GUILayout.EndHorizontal();
                }))
                .AddChild(EasyIMGUI.Custom().OnGUI(() =>
                {
                    var lastRect = GUILayoutUtility.GetLastRect();
                    mMarkdownViewer.DrawWithRect(new Rect(lastRect.x, lastRect.y + lastRect.height + 5,
                        mRightRect.width - 220, mRightRect.height - lastRect.y - lastRect.height));
                }));

            EditorWindow = EditorWindow.focusedWindow;
        }

        private void Update()
        {
            if (mMarkdownViewer != null && mMarkdownViewer.Update())
            {
                EditorWindow.Repaint();
            }
        }

        private static GUIStyle mSelectionRect = "SelectionRect";

        public EditorWindow EditorWindow { get; set; }


		public void OnGUI()
        {
            var r = GUILayoutUtility.GetLastRect();
            mSplitView.OnGUI(new Rect(new Vector2(0, r.yMax),
                new Vector2(EditorWindow.position.width, EditorWindow.position.height - r.height)));
        }

        public void OnDestroy()
        {
            //EditorApplication.update -= Update;
            mMarkdownViewer = null;
            EditorWindow = null;
        }

		public void OnUpdate()
		{
		}

		public void OnWindowGUIEnd()
		{
		}

		public void OnDispose()
		{
		}

		public void OnShow()
		{
		}

		public void OnHide()
		{
		}
	}
}
#endif