using CM3D2.Files;
using CM3D2.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using UnityEngine;
using UnityInjector;
using UnityInjector.Attributes;

namespace CM3D2.ModAssist.Plugin
{
    [PluginFilter("CM3D2x64"),
    PluginFilter("CM3D2x86"),
    PluginName("CM3D2 ModAssist"),
    PluginVersion("0.0.0.12")]
    public class ModAssist : PluginBase
    {
        static string className = "ModAssist";
        public const string Version = "0.0.0.12";
        readonly string basePath = Path.GetFullPath(@".\Mod\");
        readonly string[] nodeSelection = new string[] { "■", "非表示", "表示" };

        Logger logger = new Logger(className);

        int targetScene = 5;
        bool maEnabled = false;

        int selectorWinId = 990;
        int mainWinId = 991;

        bool showMainWindow = false;

        Rect selectorWinRect;
        Rect mainWinRect;
        Rect fileBrowserRect;
        FileBrowser fileBrowser;

        readonly int fontSize = GUIUtil.FixPx(14);
        readonly int spacerWidth = GUIUtil.FixPx(14);
        readonly GUILayoutOption labelWidth;
        readonly GUILayoutOption textHeight;

        Vector2 scrollPos = Vector2.zero;
        int selectedItem = 0;

        GUIContent[] wearMpns;
        LayoutComboBox wearMpnsCombo;
        GUIStyle listStyle = new GUIStyle();

        GUIStyle buttonStyle = new GUIStyle("button");
        GUIStyle textStyle = new GUIStyle("textField");
        GUIStyle areaStyle = new GUIStyle("textArea");
        GUIStyle labelStyle = new GUIStyle("label");
        GUIStyle toggleStyle = new GUIStyle("toggle");
        GUIStyle boxStyle = new GUIStyle("box");
        GUIStyle winStyle = new GUIStyle("box");
        GUIStyle toolbarStyle = new GUIStyle("button");
        GUIStyle selectionStyle = new GUIStyle("button");

        List<ModelFile> addItems;
        Dictionary<string, bool> delItems;
        Dictionary<string, int> nodes;
        Dictionary<string, bool> masks;
        List<MateFile> mateFiles;
        List<MenuFile> resourceFiles;

        MenuFile menuFile;
        string outputPath = @"assist\";
        int filenameType = 0;
        string filenamePrefix = "";
        bool allowOverride = true;
        bool allowSamename = false;

        GameObject goButton;
        Maid maid;

        public ModAssist()
        {
            labelWidth = GUILayout.Width(GUIUtil.FixPx(120));
            textHeight = GUILayout.Height(fontSize * 1.8f);
            // GUI初期設定
            buttonStyle.fontSize = fontSize;
            textStyle.fontSize = fontSize;
            areaStyle.fontSize = fontSize;
            labelStyle.fontSize = fontSize;
            toggleStyle.fontSize = fontSize;
            toolbarStyle.fontSize = fontSize;
            selectionStyle.fontSize = fontSize;

            toggleStyle.onNormal.textColor = Color.green;
            toggleStyle.onHover.textColor = Color.green;
            toolbarStyle.onNormal.textColor = Color.green;
            toolbarStyle.onHover.textColor = Color.green;
            buttonStyle.onNormal.textColor = Color.green;
            buttonStyle.onHover.textColor = Color.green;
            selectionStyle.onNormal.textColor = Color.green;
            selectionStyle.onHover.textColor = Color.green;

            wearMpns = new GUIContent[Params.WearMPNs.Count()];
            int i = 0;
            foreach (var mpn in Params.WearMPNs.Keys)
            {
                wearMpns[i] = new GUIContent(mpn);

                i++;
            }

            listStyle.normal.textColor = Color.white;
            listStyle.fontSize = fontSize;
            listStyle.onHover.background =
            listStyle.hover.background = new Texture2D(2, 2);
            listStyle.padding.left =
            listStyle.padding.right =
            listStyle.padding.top =
            listStyle.padding.bottom = 4;

            wearMpnsCombo = new LayoutComboBox(wearMpns[0], wearMpns, buttonStyle, boxStyle, listStyle);
        }

        // 可視状態を設定
        void setState(bool b)
        {
            enabled = b;
            maEnabled = b;

            // 歯車メニュー内のアイコンの枠の色を指定
            if (b)
            {
                GearMenu.Buttons.SetFrameColor(goButton, Color.blue);
            }
            else
            {
                GearMenu.Buttons.ResetFrameColor(goButton);
            }
        }

        // 歯車メニュー内のアイコンが押された時のコールバック
        void onGearMenuButton()
        {
            setState(!maEnabled);
        }

        void Awake()
        {
            showMainWindow = false;
            maEnabled = false;
        }

        void OnLevelWasLoaded(int level)
        {
            if (targetScene != level)
            {
                return;
            }
            goButton = GearMenu.Buttons.Add(this, iconData, (go) => { onGearMenuButton(); });
            selectorWinRect = new Rect(Screen.width - GUIUtil.FixPx(310), GUIUtil.FixPx(10), GUIUtil.FixPx(300), Screen.height * 0.8f);
            mainWinRect = new Rect(Screen.width / 2 - GUIUtil.FixPx(300), GUIUtil.FixPx(10), GUIUtil.FixPx(600), Screen.height * 0.8f);
            fileBrowserRect = new Rect(Screen.width / 2 - GUIUtil.FixPx(300), GUIUtil.FixPx(100), GUIUtil.FixPx(600), GUIUtil.FixPx(500));
            maid = GameMain.Instance.CharacterMgr.GetMaid(0);
            outputPath = @"assist\";
            maEnabled = false;
        }

        private void Update()
        {
            if (targetScene != Application.loadedLevel || !maEnabled)
            {
                return;
            }
            if (selectorWinRect.Contains(Input.mousePosition)
                || (showMainWindow && mainWinRect.Contains(Input.mousePosition)))
            {
                GameMain.Instance.MainCamera.SetControl(false);
            }
            else
            {
                GameMain.Instance.MainCamera.SetControl(true);
            }
            if (maid == null)
                maid = GameMain.Instance.CharacterMgr.GetMaid(0);
        }

        private void OnGUI()
        {
            if (targetScene != Application.loadedLevel || !maEnabled)
            {
                return;
            }
            winStyle.fontSize = fontSize;
            winStyle.alignment = TextAnchor.UpperRight;
            selectorWinRect = GUI.Window(selectorWinId, selectorWinRect, onSelectMPN, ModAssist.Version, winStyle);
            if (showMainWindow)
            {
                mainWinRect = GUI.Window(mainWinId, mainWinRect, onMain, ModAssist.Version, winStyle);
            }
            if (fileBrowser != null)
            {
                fileBrowserRect = GUI.ModalWindow(14, fileBrowserRect, doFileBrowser, ModAssist.Version, winStyle);
            }
        }

        void doFileBrowser(int winId)
        {
            fileBrowser.OnGUI();
            GUI.DragWindow();
        }

        IFileSelectable targetFile;

        void openFileBrowser(IFileSelectable file)
        {
            targetFile = file;
            fileBrowser = new FileBrowser(
                new Rect(0, 0, fileBrowserRect.width, fileBrowserRect.height),
                "ファイル選択",
                FileSelectedCallback
            );
            fileBrowser.SelectionPatterns = Params.Extensions[file.fileType];
        }

        protected void FileSelectedCallback(string path)
        {
            fileBrowser = null;
            if (string.IsNullOrEmpty(path))
            {
                return;
            }
            logger.DebugLog(path);
            string filename = Path.GetFileName(path);
            targetFile.filename = filename;
        }

        void onSelectMPN(int winId)
        {
            GUILayout.BeginVertical(GUI.skin.box);
            GUILayout.Label("MOD Assist", labelStyle);
            foreach (var mpnkey in Params.WearMPNs.Keys)
            {
                if (GUILayout.Button(Params.WearMPNs[mpnkey], buttonStyle))
                {
                    MaidProp prop = maid.GetProp(mpnkey);
                    menuFile = new MenuFile();
                    if (!menuFile.LoadFile(prop.strFileName))
                    {
                        return;
                    }
                    if (loadMenu())
                    {
                        showMainWindow = true;
                    }
                }
            }
            GUILayout.EndVertical();
            GUI.DragWindow();
        }
        /// <summary>
        /// メニューファイルを読み込む
        /// </summary>
        /// <returns></returns>
        bool loadMenu()
        {
            nodes = new Dictionary<string, int>();
            foreach (var nodekey in Params.Nodenames.Keys)
            {
                if (menuFile.delNodes.Contains(nodekey))
                {
                    nodes.Add(nodekey, 1);
                }
                else if (menuFile.showNodes.Contains(nodekey))
                {
                    nodes.Add(nodekey, 2);
                }
                else
                {
                    nodes.Add(nodekey, 0);
                }
            }
            masks = new Dictionary<string, bool>();
            foreach (var slotkey in Params.Slotnames.Keys)
            {
                if (menuFile.maskItems.Contains(slotkey))
                {
                    masks.Add(slotkey, true);
                }
                else
                {
                    masks.Add(slotkey, false);
                }
            }

            addItems = new List<ModelFile>();
            foreach (var target in menuFile.addItems)
            {
                if (Params.Slotnames.Keys.Contains(target[1]))
                {
                    ModelFile model = new ModelFile();
                    if (model.LoadFile(target[0]))
                    {
                        model.category = menuFile.category;
                        model.slotname = target[1];
                        if (Params.AttachPoints.ContainsKey(target[1]))
                        {
                            //手持ちアイテム
                            model.addItemType = ModelFile.AddItemType.handItem;
                            model.attachSlot = MenuFile.AttachBoneString;
                            model.attachName = Params.AttachPoints[target[1]];
                        }
                        else if (target.Length == 4 && target[2] == MenuFile.AttachBoneString)
                        {
                            //ボーンにアタッチ
                            model.addItemType = ModelFile.AddItemType.attachBone;
                            model.attachSlot = MenuFile.AttachBoneString;
                            model.attachName = target[3];
                        }
                        else if (target.Length == 5 && target[2] == MenuFile.AttachString)
                        {
                            //アタッチ
                            model.addItemType = ModelFile.AddItemType.attach;
                            model.attachSlot = target[3];
                            model.attachName = target[4];
                        }
                        addItems.Add(model);
                    }
                }
            }

            mateFiles = new List<MateFile>();
            foreach (var target in menuFile.materials)
            {
                if (Params.Slotnames.Keys.Contains(target[0]))
                {
                    int matno;
                    if (int.TryParse(target[1], out matno))
                    {
                        MateFile mate = new MateFile();
                        if (mate.LoadFile(target[2]))
                        {
                            mate.matno = matno;
                            mate.category = menuFile.category;
                            mate.slotname = target[0];
                            mateFiles.Add(mate);
                        }
                    }
                }
            }

            resourceFiles = new List<MenuFile>();
            foreach (var target in menuFile.resources)
            {
                MenuFile menu = new MenuFile();
                if (menu.LoadFile(target[1]))
                {
                    menu.resourceName = target[0];
                    resourceFiles.Add(menu);
                }
            }


            delItems = new Dictionary<string, bool>();
            foreach (var slot in Params.Slotnames.Keys)
            {
                // 削除アイテムのチェック
                bool exists = false;
                foreach (var target in menuFile.items)
                {
                    if (target.Contains(slot) && target.Contains("_del"))
                    {
                        exists = true;
                        break;
                    }
                }
                delItems.Add(slot, exists);
            }
            scrollPos = Vector2.zero;
            wearMpnsCombo.SelectedItemIndex = 0;
            foreach (var category in wearMpns)
            {
                if (category.text == menuFile.category)
                {
                    break;
                }
                wearMpnsCombo.SelectedItemIndex++;
            }
            return true;
        }

        void onMain(int winId)
        {
            GUILayout.BeginVertical(GUI.skin.box);
            GUILayout.Label("MOD Assist");
            selectedItem = GUILayout.Toolbar(selectedItem,
                new string[] { "基本情報", "マテリアル", "モデル", "アイテム削除", "ノード", "マスク" }, toolbarStyle);
            GUILayout.BeginHorizontal(GUI.skin.box);
            GUILayout.Label("ベース", labelStyle, labelWidth);
            GUILayout.Label(menuFile.baseMenuFile.category, labelStyle, labelWidth);
            GUILayout.Label(menuFile.baseMenuFile.filename, labelStyle);
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();
            switch (selectedItem)
            {
                case 0:
                    // 基本情報
                    onBasicInfo();
                    break;
                case 1:
                    // マテリアル
                    onMaterials();
                    break;
                case 2:
                    // モデル
                    onModels();
                    break;
                case 3:
                    // アイテム削除
                    onDelItems();
                    break;
                case 4:
                    // ノード
                    onNodes();
                    break;
                case 5:
                    // マスク
                    onMasks();
                    break;
            }
            GUILayout.BeginVertical(GUI.skin.box);
            GUILayout.BeginHorizontal();
            //TODO
            //            if (GUILayout.Button("元をコピー", buttonStyle))
            //            {
            //                doExportOriginal();
            //            }
            if (GUILayout.Button("エクスポート", buttonStyle))
            {
                doExport();
            }
            if (GUILayout.Button("閉じる", buttonStyle))
            {
                showMainWindow = false;
            }
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();
            GUI.DragWindow();
        }

        void doExportOriginal()
        {

        }

        void doExport()
        {
            // メニューファイル作成
            menuFile.basePath = basePath;
            menuFile.outputPath = outputPath;
            menuFile.allowOverwrite = allowOverride;
            menuFile.allowSamename = allowSamename;
            // アイテム削除の反映
            menuFile.items = new List<string>();
            foreach (var key in delItems.Keys)
            {
                if (delItems[key])
                {
                    menuFile.items.Add(Params.GetDelMenu(key));
                }
            }

            // ノード消去・表示の反映
            menuFile.delNodes = new List<string>();
            menuFile.showNodes = new List<string>();
            foreach (var key in nodes.Keys)
            {
                switch (nodes[key])
                {

                    case 1://消去
                        menuFile.delNodes.Add(key);
                        break;
                    case 2://表示
                        menuFile.showNodes.Add(key);
                        break;
                }
            }

            menuFile.maskItems = new List<string>();
            foreach (var key in masks.Keys)
            {
                if (masks[key])
                {
                    menuFile.maskItems.Add(key);
                }
            }
            menuFile.materials = new List<string[]>();
            menuFile.addItems = new List<string[]>();
            if (filenameType == 1)
            {
                // プリフィクスからファイル名を設定
                menuFile.filename = Params.GetFilename(Params.FileType.menu, filenamePrefix);
                menuFile.icons = Params.GetFilename(Params.FileType.icon, filenamePrefix);
                if (mateFiles != null)
                {
                    foreach (var mate in mateFiles)
                    {
                        if (mate.export)
                        {
                            if (mateFiles.Count() == 1)
                            {
                                mate.filename = Params.GetFilename(Params.FileType.mate, filenamePrefix);
                            }
                            else
                            {
                                mate.filename = Params.GetFilename(Params.FileType.mate, filenamePrefix, mate.matno);
                            }
                        }
                        menuFile.materials.Add(new string[] { mate.slotname, mate.matno.ToString(), mate.FilenameWithExtension });
                    }
                }
                foreach (var resource in menuFile.resources)
                {
                    switch (resource[0])
                    {
                        case MenuFile.ResourceMekureString:
                            resource[1] = Params.GetFilename(Params.FileType.mekureMenu, filenamePrefix);
                            break;
                        case MenuFile.ResourceMekureBackString:
                            resource[1] = Params.GetFilename(Params.FileType.mekureBackMenu, filenamePrefix);
                            break;
                        case MenuFile.ResourceZurashiString:
                            resource[1] = Params.GetFilename(Params.FileType.zurashiMenu, filenamePrefix);
                            break;
                    }
                }
                if (addItems != null)
                {
                    for (int i = 0; i < addItems.Count(); i++)
                    {
                        var model = addItems[i];
                        if (model.export)
                        {
                            if (addItems.Count() == 1)
                            {
                                model.filename = Params.GetFilename(Params.FileType.model, filenamePrefix);
                            }
                            else
                            {
                                model.filename = Params.GetFilename(Params.FileType.model, filenamePrefix, i);
                            }
                        }
                        string[] item;
                        switch (model.addItemType)
                        {
                            case ModelFile.AddItemType.attach:
                                item = new string[] {
                                    model.FilenameWithExtension,
                                    model.slotname,
                                    MenuFile.AttachString,
                                    model.attachSlot,
                                    model.attachName
                                };
                                break;
                            case ModelFile.AddItemType.attachBone:
                                item = new string[] {
                                    model.FilenameWithExtension,
                                    model.slotname,
                                    MenuFile.AttachBoneString,
                                    model.attachName
                                };
                                break;
                            default:
                                item = new string[] {
                                    model.FilenameWithExtension,
                                    model.slotname
                                };
                                break;
                        }
                        menuFile.addItems.Add(item);
                    }
                }
            }
            else
            {
                // 指定したファイル名を使用する

                // マテリアル
                if (mateFiles != null)
                {
                    foreach (var mate in mateFiles)
                    {
                        menuFile.materials.Add(new string[] { mate.category, mate.matno.ToString(), mate.FilenameWithExtension });
                    }
                }

                // additem(モデル)
                if (addItems != null)
                {
                    foreach (var model in addItems)
                    {
                        menuFile.addItems.Add(new string[] { model.FilenameWithExtension, model.category });
                    }
                }
            }

            // menu書き出し
            menuFile.WriteMenu();

            // アイコン
            if (menuFile.iconFile != null)
            {
                menuFile.iconFile.basePath = basePath;
                menuFile.iconFile.outputPath = menuFile.outputPath;
                menuFile.iconFile.category = menuFile.category;
                menuFile.iconFile.basename = menuFile.basename;
                menuFile.iconFile.filename = menuFile.icons;
                menuFile.iconFile.allowOverwrite = allowOverride;
                menuFile.iconFile.allowSamename = allowSamename;
                menuFile.iconFile.WriteTex();
            }

            // マテリアル
            if (mateFiles != null)
            {
                foreach (var mate in mateFiles)
                {
                    if (!mate.export)
                    {
                        continue;
                    }
                    mate.basePath = basePath;
                    mate.outputPath = menuFile.outputPath;
                    mate.name = mate.FilenameWithoutExtension;
                    mate.allowOverwrite = allowOverride;
                    mate.allowSamename = allowSamename;

                    int count = 0;
                    foreach (var key in mate.texs.Keys)
                    {
                        var tex = mate.texs[key];
                        if (!tex.export || tex.texFile == null || tex.type != TexParams.TexTypeTex2d)
                        {
                            continue;
                        }
                        tex.texFile.basePath = basePath;
                        tex.texFile.outputPath = menuFile.outputPath;
                        tex.texFile.allowOverwrite = allowOverride;
                        tex.texFile.allowSamename = allowSamename;

                        if (filenameType == 1)
                        {
                            if (mate.texs.Count() == 1)
                            {
                                tex.texFile.filename = Params.GetFilename(Params.FileType.tex, filenamePrefix + key);
                            }
                            else
                            {
                                tex.texFile.filename = Params.GetFilename(Params.FileType.tex, filenamePrefix + key, count);
                            }
                        }
                        tex.filename = tex.texFile.FilenameWithoutExtension;
                        tex.texFile.category = mate.category;
                        tex.texFile.basename = mate.basename;
                        tex.texFile.WriteTex();
                        count++;
                    }

                    if (mate.pmatFile != null && mate.pmatFile.export)
                    {
                        if (filenameType == 1)
                        {
                            mate.pmatFile.basePath = basePath;
                            mate.pmatFile.outputPath = menuFile.outputPath;
                            mate.pmatFile.allowOverwrite = allowOverride;
                            mate.pmatFile.allowSamename = allowSamename;
                            mate.pmatFile.filename = Params.GetFilename(Params.FileType.pmat, mate.FilenameWithoutExtension);
                        }
                        mate.pmatFile.name = mate.pmatFile.filename;
                        mate.pmatFile.WritePmat();
                    }
                    // mate書き出し
                    mate.WriteMate();
                }
            }

            // モデル
            if (addItems != null)
            {
                foreach (var model in addItems)
                {
                    if (model.export)
                    {
                        // 対象のもののみ書き出し
                        model.basePath = basePath;
                        model.outputPath = menuFile.outputPath;
                        model.allowOverwrite = allowOverride;
                        model.allowSamename = allowSamename;
                        model.WriteModel();
                    }
                }
            }
            NDebug.MessageBox("エクスポート", "エクスポート完了しました。");
        }

        void onBasicInfo()
        {
            GUILayout.BeginVertical(GUI.skin.box);
            {
                GUILayout.BeginHorizontal();
                {
                    GUILayout.Label("出力先", labelStyle, labelWidth);
                    GUILayout.Label("Mod\\", labelStyle, GUILayout.Width(GUIUtil.FixPx(50)));
                    outputPath = GUILayout.TextField(outputPath, textStyle, textHeight);
                }
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                {
                    GUILayout.Label("出力オプション", labelStyle, labelWidth);
                    allowOverride = GUILayout.Toggle(allowOverride, "上書きする", toggleStyle);
                    allowSamename = GUILayout.Toggle(allowSamename, "同名を許可する", toggleStyle);
                }
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                {
                    GUILayout.Label("ファイル名の指定", labelStyle);
                    filenameType = GUILayout.Toolbar(filenameType,
                        new string[] { "直接指定する", "プリフィクスを使用" }, toolbarStyle);
                }
                GUILayout.EndHorizontal();

                if (filenameType == 1)
                {
                    filenamePrefix = drawTextField(filenamePrefix, "プリフィクス");
                }
                else
                {
                    menuFile.filename = drawFileField(menuFile.filename, "ファイル名", menuFile);
                }
                scrollPos = GUILayout.BeginScrollView(scrollPos);
                menuFile.name = drawTextField(menuFile.name, "名前");
                menuFile.setumei = drawTextArea(menuFile.setumei, "説明");
                menuFile.priority = drawTextField(menuFile.priority, "表示順");
                menuFile.icons = drawTextField(menuFile.icons, "アイコン");
                GUILayout.BeginHorizontal();
                GUILayout.Label("カテゴリ", labelStyle, labelWidth);
                int selected = wearMpnsCombo.Show();
                menuFile.category = wearMpns[selected].text;
                GUILayout.EndHorizontal();
                GUILayout.EndScrollView();
            }
            GUILayout.EndVertical();
        }

        private Material getMaterial(string slotname, int matno)
        {
            TBody body = maid.body0;
            List<TBodySkin> goSlot = body.goSlot;
            int index = (int)global::TBody.hashSlotName[slotname];
            global::TBodySkin tBodySkin = goSlot[index];
            GameObject obj = tBodySkin.obj;
            if (obj == null)
            {
                return null;
            }
            List<Material> materialList = new List<Material>();
            Transform[] componentsInChildren = obj.transform.GetComponentsInChildren<Transform>(true);
            for (int i = 0; i < componentsInChildren.Length; i++)
            {
                Transform transform = componentsInChildren[i];
                Renderer renderer = transform.renderer;
                if (renderer != null && renderer.material != null && matno < renderer.materials.Length)
                {
                    return renderer.materials[matno];
                }
            }
            return null;
        }

        void onMaterials()
        {
            scrollPos = GUILayout.BeginScrollView(scrollPos);
            foreach (var mate in mateFiles)
            {
                GUILayout.BeginVertical(GUI.skin.box);
                {
                    drawLabel(mate.slotname + ":" + mate.name + ":" + mate.matno.ToString(), "■ベース：");
                    mate.export = GUILayout.Toggle(mate.export, "エクスポート対象とする", toggleStyle);
                    if (mate.export)
                    {
                        mate.filename = drawFileField(mate.filename, "ファイル名", mate, 1);
                        mate.name = drawTextField(mate.name, "名前", 1);
                        mate.shader1 = drawTextField(mate.shader1, "シェーダー1", 1);
                        mate.shader2 = drawTextField(mate.shader2, "シェーダー2", 1);
                        mate.pmatFile.export = drawToggle(mate.pmatFile.export, "  pmatファイルをエクスポート対象とする", 1);
                        mate.pmatFile.name = drawTextField(mate.pmatFile.name, "pmatファイル名", 1);
                        Material material = getMaterial(mate.slotname, mate.matno);
                        int renderQueue = material.renderQueue;
                        renderQueue = (int)drawSlider(renderQueue, 0, 4000,
                            String.Format("RQ:{0}", renderQueue), labelStyle, 2);
                        material.SetFloat("_SetManualRenderQueue", renderQueue);
                        material.renderQueue = renderQueue;

                        List<String> keys = new List<string>(mate.texs.Keys);
                        foreach (var key in keys)
                        {
                            var tex = mate.texs[key];
                            if (tex.type != TexParams.TexTypeTex2d)
                            {
                                continue;
                            }
                            GUILayout.BeginVertical(GUI.skin.box);
                            drawLabel("▶" + tex.propertyName);
                            tex.export = drawToggle(tex.export, "  このテクスチャをエクスポート対象とする", 2);
                            tex.filename = drawFileField(mate.texs[key].filename, "ファイル名", tex, 2);
                            Vector2 offset = tex.offset;
                            offset.x = drawSlider(offset.x, 0, 2, 
                                string.Format("{0}:{1:F2}", "offset.x", offset.x), labelStyle, 2);
                            offset.y = drawSlider(offset.y, 0, 2,
                                string.Format("{0}:{1:F2}", "offset.y", offset.y), labelStyle, 2);
                            if (offset != tex.offset)
                            {
                                tex.offset = offset;
                                material.SetTextureOffset(tex.propertyName, offset);
                            }
                            Vector2 scale = tex.scale;
                            scale.x = drawSlider(scale.x, 0, 2, string.Format("{0}:{1:F2}", "scale.x", scale.x), labelStyle, 2);
                            scale.y = drawSlider(scale.y, 0, 2, string.Format("{0}:{1:F2}", "scale.y", scale.y), labelStyle, 2);
                            if (scale != tex.scale)
                            {
                                tex.scale = scale;
                                material.SetTextureScale(tex.propertyName, scale);
                            }
                            GUILayout.EndVertical();
                        }
                        List<string> colKeys = new List<string>(mate.colors.Keys);
                        foreach (var key in colKeys)
                        {
                            GUILayout.BeginVertical(GUI.skin.box);
                            var col = mate.colors[key];
                            drawLabel(key, 1);
                            col.r = drawSlider(col.r, 0, 3, string.Format("{0}:{1:F2}", "R", col.r), labelStyle, 2);
                            col.g = drawSlider(col.g, 0, 3, string.Format("{0}:{1:F2}", "G", col.g), labelStyle, 2);
                            col.b = drawSlider(col.b, 0, 3, string.Format("{0}:{1:F2}", "B", col.b), labelStyle, 2);
                            col.a = drawSlider(col.a, 0, 3, string.Format("{0}:{1:F2}", "A", col.a), labelStyle, 2);
                            if (col != mate.colors[key])
                            {
                                mate.colors[key] = col;
                                foreach (var slot in Params.MPNtoSlotname[mate.category])
                                {
                                    maid.body0.ChangeCol(slot, mate.matno, key, col);
                                }
                            }
                            GUILayout.EndVertical();
                        }
                        List<string> floatKeys = new List<string>(mate.parameters.Keys);
                        GUILayout.BeginVertical(GUI.skin.box);
                        foreach (var key in floatKeys)
                        {
                            var param = mate.parameters[key];
                            param = drawSlider(param, 0, 2, string.Format("{0}:{1:F2}", key, param), labelStyle, 2);
                            mate.parameters[key] = param;
                        }
                        GUILayout.EndVertical();
                    }
                }
                GUILayout.EndVertical();
            }
            GUILayout.EndScrollView();
        }

        void onModels()
        {
            scrollPos = GUILayout.BeginScrollView(scrollPos);
            foreach (var item in addItems)
            {
                GUILayout.BeginVertical(GUI.skin.box);
                {
                    drawLabel(item.slotname + "  " + item.filename, "ベース：");
                    item.export = drawToggle(item.export, "エクスポート対象とする", 1);
                    if (item.export)
                    {
                        item.filename = drawFileField(item.filename, "ファイル名", item, 1);
                    }
                }
                GUILayout.EndVertical();
            }
            if (menuFile.resources != null && menuFile.resources.Count() > 0)
            {
                GUILayout.BeginVertical(GUI.skin.box);
                foreach (var resource in resourceFiles)
                {
                    resource.filename = drawFileField(resource.filename, resource.resourceName, resource);
                }
                GUILayout.EndVertical();
            }

            GUILayout.EndScrollView();
        }

        void onDelItems()
        {
            GUILayout.BeginVertical(GUI.skin.box);
            {
                scrollPos = GUILayout.BeginScrollView(scrollPos);
                List<string> keys = new List<string>(delItems.Keys);
                foreach (string key in keys)
                {
                    bool b = delItems[key];
                    delItems[key] = GUILayout.Toggle(delItems[key], " " + key + "  " + Params.Slotnames[key], toggleStyle);
                    if (b != delItems[key])
                    {
                        Menu.SetMaidItemTemp(maid, Params.GetDelMenu(key), false);
                        maid.body0.FixMaskFlag();
                        maid.body0.FixVisibleFlag(false);
                        maid.AllProcPropSeqStart();
                    }
                }
                GUILayout.EndScrollView();
            }
            GUILayout.EndVertical();

        }

        void onNodes()
        {
            GUILayout.BeginVertical(GUI.skin.box);
            {
                scrollPos = GUILayout.BeginScrollView(scrollPos);
                List<string> keys = new List<string>(nodes.Keys);
                foreach (string key in keys)
                {
                    GUILayout.BeginHorizontal();
                    int b = nodes[key];
                    nodes[key] = GUILayout.Toolbar(nodes[key], nodeSelection,
                        toolbarStyle, GUILayout.Width(GUIUtil.FixPx(160)));
                    if (b != nodes[key])
                    {
                        switch (nodes[key])
                        {
                            case 1://ノード削除
                                maid.body0.SetVisibleNodeSlot(menuFile.category, false, key);
                                break;
                            case 2:// ノード表示
                                maid.body0.SetVisibleNodeSlot(menuFile.category, true, key);
                                break;
                        }
                        maid.body0.FixMaskFlag();
                        maid.body0.FixVisibleFlag(false);
                        maid.AllProcPropSeqStart();
                    }
                    GUILayout.Label(key, labelStyle);
                    GUILayout.Label("(" + Params.Nodenames[key] + "?)", labelStyle);
                    GUILayout.EndHorizontal();
                }
                GUILayout.EndScrollView();
            }
            GUILayout.EndVertical();
        }

        void onMasks()
        {
            GUILayout.BeginVertical(GUI.skin.box);
            {
                scrollPos = GUILayout.BeginScrollView(scrollPos);
                List<string> keys = new List<string>(masks.Keys);
                foreach (string key in keys)
                {
                    bool b = masks[key];
                    masks[key] = GUILayout.Toggle(masks[key], " " + key + "  " + Params.Slotnames[key], toggleStyle);
                    if (b != masks[key])
                    {
                        maid.body0.AddMask(menuFile.category, key);
                        maid.body0.FixMaskFlag();
                        maid.body0.FixVisibleFlag(false);
                        maid.AllProcPropSeqStart();
                    }
                }
                GUILayout.EndScrollView();
            }
            GUILayout.EndVertical();
        }

        void spacer(int indent)
        {
            if (indent > 0)
            {
                GUILayout.Label("", labelStyle, GUILayout.Width(spacerWidth * indent));
            }
        }

        void drawLabel(string label, int indent = 0)
        {
            GUILayout.BeginHorizontal();
            spacer(indent);
            GUILayout.Label(label, labelStyle);
            GUILayout.EndHorizontal();
        }

        void drawLabel(string value, string label, int indent = 0)
        {
            GUILayout.BeginHorizontal();
            spacer(indent);
            GUILayout.Label(label, labelStyle, labelWidth);
            GUILayout.Label(value, labelStyle);
            GUILayout.EndHorizontal();
        }

        bool drawToggle(bool value, string label, int indent = 0)
        {
            GUILayout.BeginHorizontal();
            spacer(indent);
            value = GUILayout.Toggle(value, label, toggleStyle);
            GUILayout.EndHorizontal();
            return value;
        }

        string drawTextField(string value, string label, int indent = 0)
        {
            GUILayout.BeginHorizontal();
            spacer(indent);
            GUILayout.Label(label, labelStyle, labelWidth);
            string val = GUILayout.TextField(value, textStyle, textHeight);
            GUILayout.EndHorizontal();
            return val;
        }

        int drawIntField(int value, string label, int indent = 0)
        {
            GUILayout.BeginHorizontal();
            spacer(indent);
            GUILayout.Label(label, labelStyle, labelWidth);
            string val = value.ToString();
            val = GUILayout.TextField(val, textStyle, textHeight);
            GUILayout.EndHorizontal();
            int num;
            if (int.TryParse(val, out num))
            {
                return num;
            }
            return value;
        }

        float drawFloatField(float value, string label, int indent = 0)
        {
            GUILayout.BeginHorizontal();
            spacer(indent);
            GUILayout.Label(label, labelStyle, labelWidth);
            string val = value.ToString();
            val = GUILayout.TextField(val, textStyle, textHeight);
            GUILayout.EndHorizontal();
            float num;
            if (float.TryParse(val, out num))
            {
                return num;
            }
            return value;
        }

        string drawTextArea(string value, string label, int indent = 0)
        {
            GUILayout.BeginHorizontal();
            spacer(indent);
            GUILayout.Label(label, labelStyle, labelWidth);
            string val = GUILayout.TextArea(value, areaStyle, GUILayout.Height(fontSize * 9));
            GUILayout.EndHorizontal();
            return val;
        }

        float drawSlider(float value, float min, float max, string label, GUIStyle lstyle, int indent = 0)
        {
            GUILayout.BeginHorizontal();
            spacer(indent);
            GUILayout.Label(label, labelStyle, labelWidth);
            float val = GUILayout.HorizontalSlider(value, min, max);
            GUILayout.EndHorizontal();
            return val;
        }

        string drawFileField(string value, string label, IFileSelectable file, int indent = 0)
        {
            GUILayout.BeginHorizontal();
            spacer(indent);
            GUILayout.Label(label, labelStyle, labelWidth);
            string val = GUILayout.TextField(value, textStyle, textHeight);
            if (GUILayout.Button("...", buttonStyle, GUILayout.Width(fontSize * 2)))
            {
                openFileBrowser(file);
            }
            if (GUILayout.Button("適用", buttonStyle, GUILayout.Width(fontSize * 4)))
            {
                file.LoadFile(file.FilenameWithExtension);
                Maid maid = GameMain.Instance.CharacterMgr.GetMaid(0);

                switch (file.fileType)
                {
                    case Params.FileType.menu:
                        loadMenu();
                        Menu.ProcScript(maid, file.FilenameWithExtension);
                        maid.body0.FixMaskFlag();
                        maid.body0.FixVisibleFlag(false);
                        maid.AllProcPropSeqStart();
                        break;
                    case Params.FileType.mate:
                        var mate = (MateFile)file;
                        foreach (var slot in Params.MPNtoSlotname[mate.category])
                        {
                            maid.body0.ChangeMaterial(slot, mate.matno, mate.FilenameWithExtension);
                        }
                        break;
                    case Params.FileType.model:
                        var model = (ModelFile)file;
                        foreach (var slot in Params.MPNtoSlotname[model.category])
                        {
                            if (slot == "body")
                            {
                                maid.body0.LoadBody_R(model.FilenameWithExtension, maid);
                            }
                            else
                            {
                                maid.body0.AddItem(slot, model.FilenameWithExtension);
                            }
                        }
                        maid.body0.FixMaskFlag();
                        maid.body0.FixVisibleFlag(false);
                        maid.AllProcPropSeqStart();
                        break;
                    case Params.FileType.tex:
                        break;
                    case Params.FileType.image:
                        var tex = (TexParams)file;
                        if (tex.texFile != null)
                        {
                            if (tex.texFile.LoadFile(tex.FilenameWithExtension))
                            {
                                foreach (var slot in Params.MPNtoSlotname[tex.mateFile.category])
                                {
                                    maid.body0.ChangeTex(slot, tex.mateFile.matno, tex.propertyName, tex.filename, null);
                                }
                            }
                        }
                        break;
                }
            }
            GUILayout.EndHorizontal();
            return val;
        }
        readonly byte[] iconData =
        {
0x89,0x50,0x4e,0x47,0xd,0xa,0x1a,0xa,0x0,0x0,0x0,0xd,0x49,0x48,0x44,0x52,0x0,0x0,0x0,0x20,0x0,0x0,0x0,0x20,0x8,0x2,0x0,0x0,0x0,0xfc,0x18,0xed,0xa3,0x0,0x0,0x0,0x1,0x73,0x52,0x47,0x42,0x0,0xae,0xce,0x1c,0xe9,0x0,0x0,0x0,0x4,0x67,0x41,0x4d,0x41,0x0,0x0,0xb1,0x8f,0xb,0xfc,0x61,0x5,0x0,0x0,0x0,0x9,0x70,0x48,0x59,0x73,0x0,0x0,0xe,0xc3,0x0,0x0,0xe,0xc3,0x1,0xc7,0x6f,0xa8,0x64,0x0,0x0,0x0,0x18,0x74,0x45,0x58,0x74,0x53,0x6f,0x66,0x74,0x77,0x61,0x72,0x65,0x0,0x70,0x61,0x69,0x6e,0x74,0x2e,0x6e,0x65,0x74,0x20,0x34,0x2e,0x30,0x2e,0x36,0xfc,0x8c,0x63,0xdf,0x0,0x0,0x2,0xf2,0x49,0x44,0x41,0x54,0x48,0x4b,0xad,0xd6,0x87,0x52,0xdb,0x50,0x10,0x5,0x50,0xff,0xff,0x57,0xbd,0xd0,0x27,0xf4,0x12,0x7a,0xef,0xbd,0xc,0x10,0xaa,0x71,0x8e,0xbd,0x2f,0x8a,0x6,0xdb,0xa,0x20,0xee,0xc,0x8c,0xb5,0x5a,0xdd,0xfb,0xb6,0x4a,0x8d,0x56,0xab,0xf5,0xf6,0xf6,0xf6,0xfa,0xfa,0xfa,0xfc,0xfc,0xfc,0xf8,0xf8,0xf8,0xf0,0xf0,0xf0,0xbb,0x1e,0x30,0x3c,0x3d,0x3d,0xbd,0xbc,0xbc,0x34,0x9b,0x4d,0xcc,0xd,0x7f,0xae,0x6f,0x6e,0x6e,0xe,0xe,0xe,0xd6,0xd6,0xd6,0x96,0x96,0x96,0x7e,0xd5,0xc3,0xf2,0xf2,0xf2,0xd6,0xd6,0xd6,0xe9,0xe9,0xe9,0xdd,0xdd,0x9d,0x73,0x37,0xe8,0x60,0x5f,0x5f,0x5f,0x1f,0x1d,0x1d,0x1d,0x1a,0x1a,0x1a,0x18,0x18,0xf8,0x51,0xf,0x83,0x83,0x83,0xc3,0xc3,0xc3,0xb3,0xb3,0xb3,0x87,0x87,0x87,0xa2,0x69,0xc8,0xcc,0xfe,0xfe,0xfe,0xc8,0xc8,0x88,0xff,0x52,0x44,0xb3,0x3e,0xae,0xae,0xae,0x16,0x17,0x17,0xe7,0xe7,0xe7,0x2f,0x2f,0x2f,0x1b,0x48,0xa5,0x85,0xa6,0xf4,0x49,0x97,0x92,0xd4,0x87,0xac,0xc8,0xd2,0xe4,0xe4,0xa4,0x20,0xb2,0xc0,0xf8,0xf8,0xb8,0x50,0xf2,0xfd,0xef,0x80,0x8a,0xca,0x92,0xff,0x6d,0x81,0x85,0x85,0x85,0xe9,0xe9,0xe9,0x42,0x20,0xf4,0xdd,0x66,0x14,0xa6,0x5a,0x85,0x3d,0x70,0x7c,0x7c,0x2c,0x7c,0xb7,0xfc,0xd7,0x2a,0xd9,0xda,0x5,0x67,0xc7,0xb0,0xbb,0xbb,0xdb,0x50,0x7,0xbf,0x66,0x66,0x66,0xa,0x1,0x19,0x4c,0x25,0xec,0xec,0xec,0x84,0x1d,0xfc,0xce,0xd6,0x94,0x94,0xcd,0xe1,0xf2,0x8d,0x2e,0x1c,0x1d,0x1d,0xa1,0x75,0xd0,0x86,0xd4,0x63,0x2f,0xb,0xc0,0xc4,0xc4,0x44,0xa6,0x49,0x49,0x7c,0x61,0x3c,0x39,0x39,0xc9,0xa6,0x94,0xb4,0xca,0xbb,0xc8,0xde,0x81,0x0,0xce,0xde,0x2,0x52,0xe4,0xf9,0xcc,0x94,0xd2,0xd8,0xd8,0x18,0xe3,0xbb,0xb0,0xb4,0x75,0x38,0xf7,0x43,0x95,0xc0,0xc5,0xc5,0x45,0xa6,0xf9,0xb,0xb5,0x32,0x1f,0xf9,0xa2,0x83,0xea,0xe3,0x43,0x95,0x80,0x51,0xcc,0x34,0x7d,0xa0,0xc2,0xe1,0x59,0x81,0xbe,0x2,0xc6,0xc4,0x61,0x61,0x63,0x63,0x23,0xf3,0x75,0x50,0x56,0xdd,0xde,0xde,0xe,0x96,0xa,0xf4,0x15,0xd0,0x5e,0x28,0x4c,0xc6,0xf5,0xf5,0x75,0xd0,0x81,0xe6,0x31,0xe7,0xf9,0x22,0xa5,0xdb,0xdb,0xdb,0x60,0xa9,0x40,0x5f,0x1,0xe3,0x87,0xe2,0xfc,0xfc,0x5c,0xfb,0x6,0x1d,0x31,0xf6,0xa2,0xaf,0xcc,0xbc,0x2e,0xe8,0x90,0x54,0xa1,0x87,0x80,0x9d,0xea,0x3a,0x58,0xac,0x3f,0x4e,0x7e,0xd0,0x43,0x27,0x27,0x61,0x7,0xeb,0x6c,0x75,0x75,0x55,0x10,0xd5,0x7b,0xa5,0x87,0xc0,0xde,0xde,0x5e,0x41,0xa1,0xe5,0x39,0xe9,0x45,0xa3,0x84,0x2b,0xec,0x5,0x38,0x18,0xe,0xf,0xf2,0x9,0x79,0xe7,0xe0,0xac,0x7e,0x1d,0xf2,0x36,0x7a,0xa7,0x48,0x10,0x32,0x93,0x5d,0x4a,0xc0,0xc2,0xcd,0x8a,0xd7,0x3f,0x52,0x4,0x82,0xe0,0xec,0xd6,0xe6,0xe6,0x66,0x96,0x4d,0x49,0x9d,0xc2,0x1f,0xfa,0xd6,0xa0,0x2,0x72,0x12,0x32,0x1e,0x76,0xe,0x97,0x8e,0x4c,0x2c,0xd3,0xa7,0x34,0x37,0x37,0x97,0x5d,0xbf,0x26,0xd0,0x8d,0xa2,0x66,0x1,0xf3,0x9f,0x6f,0x7c,0x8b,0x40,0x2c,0xf,0x1b,0xcd,0xd6,0xb,0x1,0x28,0xd6,0x5f,0x5d,0x1,0x44,0x78,0xbd,0x45,0x94,0xa7,0x3c,0x83,0x54,0xc3,0xa1,0x96,0x0,0x52,0xd5,0x46,0xa7,0xd4,0xb1,0xf7,0x83,0x1d,0xf0,0x86,0x4f,0x2d,0x1,0xbc,0x99,0xaf,0xb,0x9a,0x2a,0x7c,0xbe,0x2e,0x10,0x3b,0xc3,0xe,0xf7,0x8d,0x83,0xce,0x9c,0x97,0x17,0xad,0xd7,0x5c,0xb8,0xfd,0x13,0xd0,0x70,0xda,0xeb,0x23,0x2,0xf2,0x8e,0x34,0x88,0x8a,0x7d,0x67,0xc,0x7d,0xef,0x84,0x11,0x14,0xe6,0xfe,0xfe,0x9e,0x9d,0x80,0xd4,0x59,0x62,0xf9,0xa5,0x3f,0x35,0x35,0xf5,0x5f,0x1,0x6e,0x99,0x26,0xa5,0x95,0x95,0x95,0x30,0x96,0xdf,0x7d,0x60,0x2c,0x62,0xcd,0xa8,0x8d,0x73,0xb,0xb7,0x2d,0x20,0x2e,0xa7,0xf8,0x48,0x8a,0x84,0x2b,0xa5,0xe5,0x95,0xe0,0xbd,0xcf,0x52,0x20,0x5b,0x5b,0x2d,0xaf,0x7b,0x87,0x6e,0xb,0xe0,0x95,0x4a,0xca,0x36,0x68,0xf9,0xc9,0x3a,0xa0,0xa4,0x11,0xa4,0xdd,0xd8,0x37,0x90,0x9e,0x9d,0x9d,0x9,0xc7,0x17,0x8a,0x14,0xd7,0x87,0xad,0x27,0x99,0xd8,0xbc,0xb2,0x54,0xa8,0xfd,0xf1,0x4b,0xb0,0xf8,0xda,0x31,0x3b,0x81,0x9f,0x9f,0x47,0x3c,0x68,0xb3,0x3a,0xae,0xac,0xf8,0x6e,0x94,0x9e,0xb6,0x80,0x20,0x54,0x42,0x71,0x84,0x42,0x49,0x3,0x80,0x2a,0x7d,0x16,0xf1,0xa0,0x55,0x8f,0xda,0x57,0x1,0xf6,0x66,0xb3,0xf9,0x7,0xf1,0x24,0xe3,0x58,0x77,0x1c,0xd1,0x74,0x0,0x0,0x0,0x0,0x49,0x45,0x4e,0x44,0xae,0x42,0x60,0x82
        };
    }

}
