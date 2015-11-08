using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;

namespace CM3D2.Files
{
    public class Logger
    {
        string id;

        public Logger(string id)
        {
            this.id = id;
        }

        public void DebugLog(params string[] message)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(id);
            sb.Append(":");
            for (int i = 0; i < message.Length; i++)
            {
                if (i > 0)
                {
                    sb.Append(":");
                }
                sb.Append(message[i]);
            }
            Debug.Log(sb.ToString());
        }

        public void ErrorLog(params string[] message)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(id);
            sb.Append(":");
            for (int i = 0; i < message.Length; i++)
            {
                if (i > 0)
                {
                    sb.Append(":");
                }
                sb.Append(message[i]);
            }
            Debug.LogError(sb.ToString());
        }

    }

    /// <summary>
    /// 各種パラメーター
    /// </summary>
    public class Params
    {
        public const string ReturnString = "《改行》";
        public const string ExtentionOfMenu = ".menu";
        public const string ExtentionOfMate = ".mate";
        public const string ExtentionOfModel = ".model";
        public const string ExtentionOfTex = ".tex";
        public const string ExtentionOfPmat = ".pmat";
        public const string FormatOfDelMenu = "_I_{0}_del.menu";
        public const string FormatOfMenu = "{0}_i_.menu";
        public const string FormatOfZurashiMenu = "{0}_zurashi_i_.menu";
        public const string FormatOfMekureMenu = "{0}_mekure_i_.menu";
        public const string FormatOfMekureBackMenu = "{0}_mekure_back_i_.menu";
        public const string FormatOfMate = "{0}{1}.mate";
        public const string FormatOfModel = "{0}{1}.model";
        public const string FormatOfTex = "{0}{1}.tex";
        public const string FormatOfIcon = "{0}_i_.tex";
        public const string FormatOfPmat = "{0}{1}.pmat";

        /// <summary>
        /// ファイル種類
        /// </summary>
        public enum FileType
        {
            menu,
            delMenu,
            zurashiMenu,
            mekureMenu,
            mekureBackMenu,
            mate,
            model,
            tex,
            icon,
            image,
            pmat
        }

        /// <summary>
        /// ファイル種類ごとの選択可能な拡張子
        /// </summary>
        public static readonly Dictionary<FileType, string[]> Extensions = new Dictionary<FileType, string[]>()
        {
            { FileType.menu, new string[] { @"*.menu" } },
            { FileType.mate, new string[] { @"*.mate" } },
            { FileType.model, new string[] { @"*.model" } },
            { FileType.tex, new string[] { @"*.tex" } },
            { FileType.icon, new string[] { @"*.tex" } },
            { FileType.image, new string[] { @"*.tex", @"*.png" } },
            { FileType.pmat, new string[] { @"*.pmat" } },
        };

        /// <summary>
        /// メニューフォルダー
        /// </summary>
        public static readonly Dictionary<string, string> MenuFoloders = new Dictionary<string, string>()
        {
            {"HEAD", "頭"},
            {"BODY", "身体"},
            {"DRESS", "服装"},
        };

        /// <summary>
        /// 衣装MPN
        /// </summary>
        public static readonly Dictionary<string, string> WearMPNs = new Dictionary<string, string>()
        {
            {"accha", "歯" },
            {"hairaho", "アホ毛" },
            {"acchat", "帽子" },
            {"headset", "ヘッドドレス" },
            {"wear", "トップス" },
            {"skirt", "ボトムス" },
            {"onepiece", "ワンピース" },
            {"mizugi", "水着" },
            {"bra", "ブラジャー" },
            {"panz", "パンツ" },
            {"stkg", "靴下" },
            {"shoes", "靴" },
            {"acckami", "アクセ：前髪" },
            {"megane", "アクセ：メガネ" },
            {"acchead", "アクセ：アイマスク" },
            {"glove", "アクセ：手袋" },
            {"acchana", "アクセ：鼻" },
            {"accmimi", "アクセ：耳" },
            {"acckubi", "アクセ：ネックレス" },
            {"acckubiwa", "アクセ：チョーカー" },
            {"acckamisub", "アクセ：リボン" },
            {"accnip", "アクセ：乳首" },
            {"accude", "アクセ：腕" },
            {"accheso", "アクセ：へそ" },
            {"accashi", "アクセ：足首" },
            {"accsenaka", "アクセ：背中" },
            {"accshippo", "アクセ：しっぽ" },
            {"accxxx", "アクセ：前穴" },
            {"handitem", "手持ちアイテム" },
            {"accanl", "アクセ：アナル" },
            {"accvag", "アクセ：ヴァギナ" },
        };

        /// <summary>
        /// MPNに対応するスロット
        /// </summary>
        public static readonly Dictionary<string, string[]> MPNtoSlotname = new Dictionary<string, string[]>()
        {

            {"accha", new string[] { "accHa" } },
            {"hairaho", new string[] {"hairAho" } },
            {"acchat", new string[] {"accHat" }},
            {"headset", new string[] {"headset" }},
            {"wear", new string[] {"wear" }},
            {"skirt", new string[] {"skirt" }},
            {"onepiece", new string[] {"onepiece" }},
            {"mizugi", new string[] {"mizugi" }},
            {"bra", new string[] {"bra" }},
            {"panz", new string[] {"panz" }},
            {"stkg", new string[] {"stkg" }},
            {"shoes", new string[] {"shoes" }},
            {"acckami", new string[] {"accKami_1_", "accKami_2_", "accKami_3_" }},
            {"megane", new string[] {"megane" }},
            {"acchead", new string[] {"accHead" }},
            {"glove",new string[] { "glove" }},
            {"acchana", new string[] {"accHana" }},
            {"accmimi", new string[] {"accMiMiL", "accMiMiR" }},
            {"acckubi", new string[] {"accKubi" }},
            {"acckubiwa", new string[] {"accKubiwa" }},
            {"acckamisub", new string[] {"accKamiSubL", "accKamiSubR" }},
            {"accnip", new string[] { "accNipL", "accNipR" } },
            {"accude", new string[] {"accUde" }},
            {"accheso", new string[] {"accHeso" }},
            {"accashi", new string[] {"accAshi"} },
            {"accsenaka", new string[] {"accSenaka" }},
            {"accshippo", new string[] {"accShippo"} },
            {"accxxx", new string[] {"accXXX" }},
            {"handitem", new string[] { "HandItemL", "HandItemR" } },
            {"accanl", new string[] {"accAnl"} },
            {"accvag", new string[] {"accVag" } },
        };

        /// <summary>
        /// スロット
        /// </summary>
        public static readonly Dictionary<string, string> Slotnames = new Dictionary<string, string>()
        {
            {"body","身体"},
            {"chikubi","乳首"},
            {"underhair","アンダーヘア"},
            {"head","頭"},
            {"eye","目"},
            {"accHa","歯"},
            {"hairF","前髪"},
            {"hairR","後髪"},
            {"hairS","横髪"},
            {"hairT","エクステ毛"},
            {"hairAho","アホ毛"},
            {"accHat","帽子"},
            {"headset","ヘッドドレス"},
            {"wear","トップス"},
            {"skirt","ボトムス"},
            {"onepiece","ワンピース"},
            {"mizugi","水着"},
            {"bra","ブラジャー"},
            {"panz","パンツ"},
            {"stkg","靴下"},
            {"shoes","靴"},
            {"accKami_1_","アクセ：前髪"},
            {"accKami_2_","アクセ：前髪：左"},
            {"accKami_3_","アクセ：前髪：右"},
            {"megane","アクセ：メガネ"},
            {"accHead","アクセ：アイマスク"},
            {"glove","アクセ：手袋"},
            {"accHana","アクセ：鼻"},
            {"accMiMiL","アクセ：左耳"},
            {"accMiMiR","アクセ：右耳"},
            {"accKubi","アクセ：ネックレス"},
            {"accKubiwa","アクセ：チョーカー"},
            {"accKamiSubL","アクセ：（左）リボン"},
            {"accKamiSubR","アクセ：右リボン"},
            {"accNipL","アクセ：左乳首"},
            {"accNipR","アクセ：右乳首"},
            {"accUde","アクセ：腕"},
            {"accHeso","アクセ：へそ"},
            {"accAshi","アクセ足首"},
            {"accSenaka","アクセ：背中"},
            {"accShippo","アクセ：しっぽ"},
            {"accXXX","アクセ：前穴"},
            {"seieki_naka","精液：中"},
            {"seieki_hara","精液：腹"},
            {"seieki_face","精液：顔"},
            {"seieki_mune","精液：胸"},
            {"seieki_hip","精液：尻"},
            {"seieki_ude","精液：腕"},
            {"seieki_ashi","精液：足"},
            {"HandItemL","手持アイテム：左"},
            {"HandItemR","手持ちアイテム：右"},
            {"kubiwa","首輪？"},
            {"kousoku_upper","拘束具：上？"},
            {"kousoku_lower","拘束具：下？"},
            {"accAnl","アナルバイブ？"},
            {"accVag","バイブ？"},
            {"chinko","チ○コ"}
        };

        /// <summary>
        /// ノード
        /// </summary>
        public static Dictionary<string, string> Nodenames = new Dictionary<string, string>()
        {
            {"Bip01", "全身"},
            {"Head", "頭"},
            {"Neck_SCL_", "首"},
            {"Mune_L_sub", "左胸上"},
            {"Mune_L", "左胸下"},
            {"Mune_R_sub", "右胸上"},
            {"Mune_R", "右胸下"},
            {"Pelvis_SCL_", "骨盤"},
            {"Spine_SCL_", "脊椎"},
            {"Spine1_SCL_", "腰中"},
            {"Spine0a_SCL_", "腹部"},
            {"Spine1a_SCL_", "胸部"},
            {"Hip_L", "左尻"},
            {"Hip_R", "右尻"},
            {"momotwist_L", "左前腿"},
            {"momoniku_L", "左後腿"},
            {"momotwist2_L", "左前腿下部"},
            {"L Thigh_SCL_", "左ふくらはぎ"},
            {"L Calf_SCL_", "左足首"},
            {"L Toe0", "左足小指付け根"},
            {"L Toe01", "左足小指先"},
            {"L Toe1", "左足中指付け根"},
            {"L Toe11", "左足中指先"},
            {"L Toe2", "左足親指付け根"},
            {"L Toe21", "左足親指先"},
            {"momotwist_R", "右前腿"},
            {"momoniku_R", "右後腿"},
            {"momotwist2_R", "右前腿下部"},
            {"R Thigh_SCL_", "右ふくらはぎ"},
            {"R Calf_SCL_", "右足首"},
            {"R Toe0", "右足小指付け根"},
            {"R Toe01", "右足小指先"},
            {"R Toe1", "右足中指付け根"},
            {"R Toe11", "右足中指先"},
            {"R Toe2", "右足親指付け根"},
            {"R Toe21", "右足親指先"},
            {"L Clavicle_SCL_", "左鎖骨"},
            {"Kata_L", "左肩"},
            {"Kata_L_nub", "左肩上腕"},
            {"Uppertwist_L", "左上腕A"},
            {"Uppertwist1_L", "左上腕B"},
            {"L UpperArm", "左上腕"},
            {"L Forearm", "左肘"},
            {"Foretwist1_L", "左前腕"},
            {"Foretwist_L", "左手首"},
            {"L Hand", "左手"},
            {"L Finger0", "左親指付け根"},
            {"L Finger01", "左親指関節"},
            {"L Finger02", "左親指先"},
            {"L Finger1", "左人指し指付け根"},
            {"L Finger11", "左人指し指関節"},
            {"L Finger12", "左人指し指先"},
            {"L Finger2", "左中指付け根"},
            {"L Finger21", "左中指関節"},
            {"L Finger22", "左中指先"},
            {"L Finger3", "左薬指付け根"},
            {"L Finger31", "左薬指関節"},
            {"L Finger32", "左薬指先"},
            {"L Finger4", "左小指付け根"},
            {"L Finger41", "左小指関節"},
            {"L Finger42", "左小指先"},
            {"R Clavicle_SCL_", "右鎖骨"},
            {"Kata_R", "右肩"},
            {"Kata_R_nub", "右肩上腕"},
            {"Uppertwist_R", "右上腕A"},
            {"Uppertwist1_R", "右上腕B"},
            {"R UpperArm", "右上腕"},
            {"R Forearm", "右肘"},
            {"Foretwist1_R", "右前腕"},
            {"Foretwist_R", "右手首"},
            {"R Hand", "右手"},
            {"R Finger0", "右親指付け根"},
            {"R Finger01", "右親指関節"},
            {"R Finger02", "右親指先"},
            {"R Finger1", "右人指し指付け根"},
            {"R Finger11", "右人指し指関節"},
            {"R Finger12", "右人指し指先"},
            {"R Finger2", "右中指付け根"},
            {"R Finger21", "右中指関節"},
            {"R Finger22", "右中指先"},
            {"R Finger3", "右薬指付け根"},
            {"R Finger31", "右薬指関節"},
            {"R Finger32", "右薬指先"},
            {"R Finger4", "右小指付け根"},
            {"R Finger41", "右小指関節"},
            {"R Finger42", "右小指先"},
        };

        /// <summary>
        /// 特殊なアタッチ位置
        /// </summary>
        public static Dictionary<string, string> AttachPoints = new Dictionary<string, string>()
        {
            { "handitemr", "_IK_handR" },
            { "handiteml", "_IK_handL" },
        };

        /// <summary>
        /// 削除用item(xxx_del.menuの形式)のファイル名を取得する
        /// </summary>
        /// <param name="slot">スロット名</param>
        /// <returns>削除用メニューファイル名</returns>
        public static string GetDelMenu(string slot)
        {
            return string.Format(FormatOfDelMenu, slot);
        }

        /// <summary>
        /// ファイル名を取得する
        /// </summary>
        /// <param name="fileType">ファイル種類</param>
        /// <param name="prefix">プリフィックス</param>
        /// <param name="num">連番</param>
        /// <returns>ファイル名</returns>
        public static string GetFilename(FileType fileType, string prefix, int? num = null)
        {
            switch (fileType)
            {
                case FileType.menu:
                    return string.Format(FormatOfMenu, prefix);
                case FileType.delMenu:
                    return string.Format(FormatOfDelMenu, prefix);
                case FileType.zurashiMenu:
                    return string.Format(FormatOfZurashiMenu, prefix);
                case FileType.mekureMenu:
                    return string.Format(FormatOfMekureMenu, prefix);
                case FileType.mekureBackMenu:
                    return string.Format(FormatOfMekureBackMenu, prefix);
                case FileType.mate:
                    if (num == null)
                    {
                        return string.Format(FormatOfMate, prefix, string.Empty);
                    }
                    return string.Format(FormatOfMate, prefix, "_" + num.ToString());
                case FileType.model:
                    if (num == null)
                    {
                        return string.Format(FormatOfModel, prefix, string.Empty);
                    }
                    return string.Format(FormatOfModel, prefix, "_" + num.ToString());
                case FileType.icon:
                    return string.Format(FormatOfIcon, prefix);
                case FileType.tex:
                    if (num == null)
                    {
                        return string.Format(FormatOfTex, prefix, string.Empty);
                    }
                    return string.Format(FormatOfTex, prefix, "_" + num.ToString());
                case FileType.pmat:
                    if (num == null)
                    {
                        return string.Format(FormatOfPmat, prefix, string.Empty);
                    }
                    return string.Format(FormatOfPmat, prefix, "_" + num.ToString());
            }
            return null;
        }
    }

    /// <summary>
    /// ファイル操作ユーティリティー
    /// </summary>
    class FileUtil
    {
        /// <summary>
        /// ファイルの存在チェック
        /// </summary>
        /// <param name="filename">ファイル名</param>
        /// <returns></returns>
        public static bool FileExists(string filename)
        {
            using (AFileBase aFileBase = global::GameUty.FileOpen(filename))
            {
                if (!aFileBase.IsValid())
                {
                    return false;
                }
            }
            return true;
        }
    }

    /// <summary>
    /// ファイルインターフェース
    /// </summary>
    interface IFileSelectable
    {
        string filename
        { get; set; }

        string FilenameWithoutExtension
        { get; }
        string FilenameWithExtension
        { get; }
        Params.FileType fileType
        { get; }

        bool LoadFile(string filename);
    }

    /// <summary>
    /// メニューファイル
    /// </summary>
    class MenuFile : IFileSelectable
    {
        Logger logger = new Logger("MenuFile");
        // {0}:menuFolder, {1}:category
        const string baseAssetPathFormat = "assets/menu/menu/{0}/{1}/";

        public Params.FileType fileType
        {
            get
            {
                return Params.FileType.menu;
            }
        }

        public string FilenameWithoutExtension
        {
            get
            {
                if (filename == null)
                {
                    return filename;
                }
                if (Path.GetExtension(filename) != null && Path.GetExtension(filename) == Params.ExtentionOfMenu)
                {
                    return Path.GetFileNameWithoutExtension(filename);
                }
                return filename;
            }
        }

        public string FilenameWithExtension
        {
            get
            {
                if (filename == null)
                {
                    return filename;
                }
                if (Path.GetExtension(filename) != null && Path.GetExtension(filename) == Params.ExtentionOfMenu)
                {
                    return filename;
                }
                return filename + Params.ExtentionOfMenu;
            }
        }

        public bool allowOverwrite
        { get; set; } = false;

        public bool allowSamename
        { get; set; } = false;

        const string menuHeaderString = "CM3D2_MENU";

        const string menuFolderString = "メニューフォルダ";
        const string catnoString = "catno";
        const string addAttributeString = "属性追加";
        const string iconsString = "icons";
        const string priorityString = "priority";
        const string nameString = "name";
        const string setumeiString = "setumei";
        const string onclickmenuString = "onclickmenu";
        const string itemString = "アイテム";
        const string itemConditionString = "アイテム条件";
        const string itemParameterString = "アイテムパラメータ";
        const string halfOffString = "半脱ぎ";
        const string resourceReferenceString = "リソース参照";
        const string setString = "set";
        const string setnameString = "setname";
        const string setslotitemString = "setslotitem";
        const string additemString = "additem";
        const string saveitemString = "saveitem";
        const string categoryString = "category";
        const string maskitemString = "maskitem";
        const string delitemString = "delitem";
        const string delNodeStartString = "消去ノード設定開始";
        const string delNodeString = "node消去";
        const string showNodeString = "node表示";
        const string delPartsNodeString = "パーツnode消去";
        const string showPartsNodeString = "パーツnode表示";
        const string delNodeEndString = "消去ノード設定終了";
        const string colorString = "color";
        const string mancolorString = "mancolor";
        const string texString = "tex";
        const string changeTextureString = "テクスチャ変更";
        const string propString = "prop";
        const string multiplyTextureString = "テクスチャ乗算";
        const string mulTextureString = "テクスチャ合成";
        const string changeMaterialString = "マテリアル変更";
        const string attachPointString = "アタッチポイントの設定";
        const string blendsetString = "blendset";
        const string paramsetString = "paramset";
        const string commenttypeString = "commenttype";

        public const string ResourceMekureString = "めくれスカート";
        public const string ResourceMekureBackString = "めくれスカート後ろ";
        public const string ResourceZurashiString = "パンツずらし";
        public const string AttachString = "アタッチ";
        public const string AttachBoneString = "ボーンにアタッチ";

        // Load時のメニュー内容
        public MenuFile baseMenuFile;

        public string basePath
        { get; set; } = string.Empty;

        public string outputPath
        { get; set; } = string.Empty;

        public string filename
        { get; set; } = string.Empty;

        public int version
        { get; set; } = 1000;

        public string assetPath
        { get; set; } = string.Empty;

        public string headerName
        { get; set; } = string.Empty;

        public string headerCategory
        { get; set; } = string.Empty;

        public string headerSetumei
        { get; set; } = string.Empty;

        public string basename
        { get; set; }

        public string menuFolder
        { get; set; } = string.Empty;

        public string category
        { get; set; } = string.Empty;

        public string catno
        { get; set; } = string.Empty;

        public string priority
        { get; set; } = string.Empty;

        public string name
        { get; set; } = string.Empty;

        public string setumei
        { get; set; } = string.Empty;

        public string icons
        { get; set; } = string.Empty;

        public TexFile iconFile
        { get; set; }

        public List<string[]> itemParams
        { get; set; }

        public List<string[]> addAttributes
        { get; set; }

        public List<string> items
        { get; set; }

        public List<string> itemConditions
        { get; set; }

        public List<string[]> addItems
        { get; set; }

        public List<string> maskItems
        { get; set; }

        public List<string[]> materials
        { get; set; }

        public List<string> delNodes
        { get; set; }

        public List<string> showNodes
        { get; set; }

        public List<string[]> delPartsNodes
        { get; set; }

        public List<string[]> showPartsNodes
        { get; set; }

        public List<string[]> resources
        { get; set; }

        public List<string[]> texs
        { get; set; }

        public string resourceName
        { get; set; } = string.Empty;

        private string getOutputPath()
        {
            return Path.Combine(basePath, outputPath);
        }

        private string getAssetPath()
        {
            return String.Format(baseAssetPathFormat, menuFolder, category) + filename + ".txt";
        }

        public MenuFile()
        {
            reset();
        }

        void reset()
        {
            this.itemParams = new List<string[]>();
            this.addAttributes = new List<string[]>();
            this.items = new List<string>();
            this.itemConditions = new List<string>();
            this.addItems = new List<string[]>();
            this.maskItems = new List<string>();
            this.materials = new List<string[]>();
            this.delNodes = new List<string>();
            this.showNodes = new List<string>();
            this.delPartsNodes = new List<string[]>();
            this.showPartsNodes = new List<string[]>();
            this.resources = new List<string[]>();
            this.texs = new List<string[]>();
        }

        public bool LoadFile(string filename)
        {
            reset();
            if (String.IsNullOrEmpty(Path.GetExtension(filename)) || Path.GetExtension(filename) != Params.ExtentionOfMenu)
            {
                filename += Params.ExtentionOfMenu;
            }
            this.baseMenuFile = new MenuFile();
            this.filename = Path.GetFileNameWithoutExtension(filename);
            this.baseMenuFile.filename = Path.GetFileNameWithoutExtension(filename);

            byte[] cd = null;
            try
            {
                using (AFileBase aFileBase = global::GameUty.FileOpen(filename))
                {
                    if (!aFileBase.IsValid())
                    {
                        logger.ErrorLog("アイテムメニューファイルが見つかりません。", filename);
                        return false;
                    }
                    cd = aFileBase.ReadAll();
                }
            }
            catch (Exception ex2)
            {
                logger.ErrorLog("アイテムメニューファイルが読み込めませんでした。", filename, ex2.Message);
                return false;
            }
            try
            {
                using (BinaryReader binaryReader = new BinaryReader(new MemoryStream(cd), Encoding.UTF8))
                {
                    string text = binaryReader.ReadString();
                    if (text != menuHeaderString)
                    {
                        logger.ErrorLog("例外: ヘッダーファイルが不正です。" + text);
                        return false;
                    }
                    version = binaryReader.ReadInt32();

                    assetPath = binaryReader.ReadString();

                    headerName = binaryReader.ReadString();

                    headerCategory = binaryReader.ReadString();

                    headerSetumei = binaryReader.ReadString().Replace(Params.ReturnString, "\n");

                    int num2 = (int)binaryReader.ReadInt32();
                    while (true)
                    {
                        byte b = binaryReader.ReadByte();
                        int size = (int)b;
                        if (size == 0)
                        {
                            break;
                        }
                        string[] param = new string[size];
                        for (int i = 0; i < size; i++)
                        {
                            param[i] = binaryReader.ReadString();
                        }
                        string[] p1 = new string[size - 1];
                        string[] p2 = new string[size - 1];
                        for (int i = 0; i < size - 1; i++)
                        {
                            p1[i] = param[i + 1];
                            p2[i] = param[i + 1];
                        }
                        switch (param[0])
                        {
                            case menuFolderString:
                                baseMenuFile.menuFolder = param[1];
                                menuFolder = param[1];
                                break;
                            case categoryString:
                                baseMenuFile.category = param[1].ToLower();
                                category = param[1].ToLower();
                                break;
                            case catnoString:
                                baseMenuFile.catno = param[1];
                                catno = param[1];
                                break;
                            case addAttributeString:
                                baseMenuFile.addAttributes.Add(p1);
                                addAttributes.Add(p2);
                                break;
                            case priorityString:
                                baseMenuFile.priority = param[1];
                                priority = param[1];
                                break;
                            case nameString:
                                baseMenuFile.name = param[1];
                                name = param[1];
                                break;
                            case setumeiString:
                                baseMenuFile.setumei = param[1].Replace(Params.ReturnString, "\n");
                                setumei = param[1].Replace(Params.ReturnString, "\n");
                                break;
                            case iconsString:
                                baseMenuFile.icons = param[1];
                                icons = param[1];
                                iconFile = new TexFile();
                                iconFile.LoadFile(icons);
                                baseMenuFile.iconFile = new TexFile();
                                baseMenuFile.iconFile.LoadFile(icons);
                                break;
                            case itemParameterString:
                                baseMenuFile.itemParams.Add(p1);
                                itemParams.Add(p2);
                                break;
                            case itemString:
                                baseMenuFile.items.Add(param[1]);
                                items.Add(param[1]);
                                break;
                            case additemString:
                                baseMenuFile.addItems.Add(p1);
                                addItems.Add(p2);
                                break;
                            case maskitemString:
                                baseMenuFile.maskItems.Add(param[1]);
                                maskItems.Add(param[1]);
                                break;
                            case changeMaterialString:
                                baseMenuFile.materials.Add(p1);
                                materials.Add(p2);
                                break;
                            case delNodeString:
                                baseMenuFile.delNodes.Add(param[1]);
                                delNodes.Add(param[1]);
                                break;
                            case showNodeString:
                                baseMenuFile.showNodes.Add(param[1]);
                                showNodes.Add(param[1]);
                                break;
                            case delPartsNodeString:
                                baseMenuFile.delPartsNodes.Add(p1);
                                delPartsNodes.Add(p2);
                                break;
                            case showPartsNodeString:
                                baseMenuFile.showPartsNodes.Add(p1);
                                showPartsNodes.Add(p2);
                                break;
                            case resourceReferenceString:
                                baseMenuFile.resources.Add(p1);
                                resources.Add(p2);
                                break;
                            case texString:
                            case changeTextureString:
                                baseMenuFile.texs.Add(p1);
                                texs.Add(p2);
                                break;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Debug.LogException(e);
                return false;
            }
            return true;
        }

        public bool WriteMenu(string path, string filename)
        {
            this.outputPath = path;
            this.filename = filename;
            return WriteMenu();
        }

        public bool WriteMenu(string filename)
        {
            this.filename = filename;
            return WriteMenu();
        }

        public bool WriteMenu()
        {
            if (!validate())
            {
                return false;
            }

            string outputPath = getOutputPath();
            string outfile = Path.Combine(outputPath, filename);
            if (String.IsNullOrEmpty(Path.GetExtension(outfile)) || Path.GetExtension(outfile) != Params.ExtentionOfMenu)
            {
                outfile += Params.ExtentionOfMenu;
            }
            if (!Directory.Exists(outputPath))
            {
                Directory.CreateDirectory(outputPath);
            }
            try
            {
                using (MemoryStream headerMs = new MemoryStream())
                using (MemoryStream dataMs = new MemoryStream())
                using (BinaryWriter headerWriter = new BinaryWriter(headerMs))
                using (BinaryWriter dataWriter = new BinaryWriter(dataMs))
                {
                    headerWriter.Write(menuHeaderString);
                    headerWriter.Write(version);
                    headerWriter.Write(getAssetPath());
                    headerWriter.Write(headerName);
                    headerWriter.Write(headerCategory);
                    headerWriter.Write(headerSetumei.Replace("\n", Params.ReturnString));

                    dataWriter.Write((byte)2);
                    dataWriter.Write(menuFolderString);
                    dataWriter.Write(menuFolder);

                    dataWriter.Write((byte)2);
                    dataWriter.Write(categoryString);
                    dataWriter.Write(category);

                    dataWriter.Write((byte)2);
                    dataWriter.Write(catnoString);
                    dataWriter.Write(catno);

                    if (addAttributes != null)
                    {
                        foreach (var item in addAttributes)
                        {
                            dataWriter.Write((byte)(item.Length + 1));
                            dataWriter.Write(addAttributeString);
                            foreach (var val in item)
                            {
                                dataWriter.Write(val);
                            }
                        }
                    }

                    dataWriter.Write((byte)2);
                    dataWriter.Write(priorityString);
                    dataWriter.Write(priority);

                    dataWriter.Write((byte)2);
                    dataWriter.Write(nameString);
                    dataWriter.Write(name);

                    dataWriter.Write((byte)2);
                    dataWriter.Write(setumeiString);
                    dataWriter.Write(setumei.Replace("\n", Params.ReturnString));

                    dataWriter.Write((byte)2);
                    dataWriter.Write(iconsString);
                    dataWriter.Write(icons);

                    dataWriter.Write((byte)1);
                    dataWriter.Write(onclickmenuString);

                    if (items != null)
                    {
                        foreach (var item in items)
                        {
                            dataWriter.Write((byte)2);
                            dataWriter.Write(itemString);
                            dataWriter.Write(item);
                        }
                    }
                    if (itemParams != null)
                    {
                        foreach (var item in itemParams)
                        {
                            dataWriter.Write((byte)(item.Length + 1));
                            dataWriter.Write(itemParameterString);
                            foreach (var val in item)
                            {
                                dataWriter.Write(val);
                            }
                        }
                    }
                    if (texs != null)
                    {
                        foreach (var item in texs)
                        {
                            dataWriter.Write((byte)(item.Length + 1));
                            dataWriter.Write(itemParameterString);
                            foreach (var val in item)
                            {
                                dataWriter.Write(val);
                            }
                        }
                    }
                    if (addItems != null)
                    {
                        foreach (var item in addItems)
                        {
                            dataWriter.Write((byte)(item.Length + 1));
                            dataWriter.Write(additemString);
                            foreach (var val in item)
                            {
                                dataWriter.Write(val);
                            }
                        }
                    }
                    if (maskItems != null)
                    {
                        foreach (var val in maskItems)
                        {
                            dataWriter.Write((byte)2);
                            dataWriter.Write(maskitemString);
                            dataWriter.Write(val);
                        }
                    }
                    if (materials != null)
                    {
                        foreach (var item in materials)
                        {
                            dataWriter.Write((byte)(item.Length + 1));
                            dataWriter.Write(changeMaterialString);
                            foreach (var val in item)
                            {
                                dataWriter.Write(val);
                            }
                        }
                    }

                    if (resources != null)
                    {
                        foreach (var item in resources)
                        {
                            dataWriter.Write((byte)(item.Length + 1));
                            dataWriter.Write(resourceReferenceString);
                            foreach (var val in item)
                            {
                                dataWriter.Write(val);
                            }
                        }
                    }

                    dataWriter.Write((byte)1);
                    dataWriter.Write(delNodeStartString);

                    if (delNodes != null)
                    {
                        foreach (var val in delNodes)
                        {
                            dataWriter.Write((byte)(2));
                            dataWriter.Write(delNodeString);
                            dataWriter.Write(val);
                        }
                    }

                    if (showNodes != null)
                    {
                        foreach (var item in showNodes)
                        {
                            dataWriter.Write((byte)(2));
                            dataWriter.Write(showNodeString);
                            dataWriter.Write(item);
                        }
                    }

                    if (delPartsNodes != null)
                    {
                        foreach (var item in delPartsNodes)
                        {
                            dataWriter.Write((byte)(item.Length + 1));
                            dataWriter.Write(delPartsNodeString);
                            foreach (var val in item)
                            {
                                dataWriter.Write(val);
                            }
                        }
                    }
                    if (showPartsNodes != null)
                    {
                        foreach (var item in showPartsNodes)
                        {
                            dataWriter.Write((byte)(item.Length + 1));
                            dataWriter.Write(showPartsNodeString);
                            foreach (var val in item)
                            {
                                dataWriter.Write(val);
                            }
                        }
                    }

                    dataWriter.Write((byte)1);
                    dataWriter.Write(delNodeEndString);

                    dataWriter.Write((char)0);

                    using (BinaryWriter writer = new BinaryWriter(File.OpenWrite(outfile)))
                    {
                        writer.Write(headerMs.ToArray());
                        writer.Write((int)dataMs.Length);
                        writer.Write(dataMs.ToArray());
                    }
                }
            }
            catch (Exception e)
            {
                Debug.LogException(e);
                return false;
            }
            return true;
        }

        private bool validate()
        {
            if (String.IsNullOrEmpty(outputPath))
            {
                NDebug.MessageBox("エラー", "出力先フォルダがセットされていません");
                return false;
            }
            if (String.IsNullOrEmpty(filename))
            {
                NDebug.MessageBox("エラー", "ファイル名がセットされていません");
                return false;
            }
            if (String.IsNullOrEmpty(name))
            {
                NDebug.MessageBox("エラー", "名前がセットされていません");
                return false;
            }
            char[] invalidPathChars = Path.GetInvalidPathChars();
            if (invalidPathChars.Any(ch => outputPath.Contains(ch)))
            {
                NDebug.MessageBox("エラー", "出力先フォルダに不正な文字が含まれています");
                return false;
            }
            char[] invalidFileNameChars = Path.GetInvalidFileNameChars();
            if (invalidFileNameChars.Any(ch => filename.Contains(ch)))
            {
                NDebug.MessageBox("エラー", "ファイル名に不正な文字が含まれています");
                return false;
            }
            if (!allowOverwrite)
            {
                if (File.Exists(Path.Combine(getOutputPath(), filename)))
                {
                    NDebug.MessageBox("エラー", "既にファイルが存在します");
                }
            }
            if (!allowSamename)
            {
                if (FileUtil.FileExists(filename))
                {
                    NDebug.MessageBox("エラー", "既に同名のmenuが存在します");
                    return false;
                }
            }
            return true;
        }
    }

    class MateFile : IFileSelectable
    {
        public Params.FileType fileType
        {
            get
            {
                return Params.FileType.mate;
            }
        }

        public string FilenameWithoutExtension
        {
            get
            {
                if (filename == null)
                {
                    return filename;
                }
                if (Path.GetExtension(filename) != null && Path.GetExtension(filename) == Params.ExtentionOfMate)
                {
                    return Path.GetFileNameWithoutExtension(filename);
                }
                return filename;
            }
        }

        public string FilenameWithExtension
        {
            get
            {
                if (filename == null)
                {
                    return filename;
                }
                if (Path.GetExtension(filename) != null && Path.GetExtension(filename) == Params.ExtentionOfMate)
                {
                    return filename;
                }
                return filename + Params.ExtentionOfMate;
            }
        }

        Logger logger = new Logger("MateFile");

        const string materialHeaderString = "CM3D2_MATERIAL";

        // {0}:category, {1}:basename
        const string baseTextureAssetPathFormat = "Assets/texture/texture/{0}/{1}/";
        const string baseToonAssetPath = "Assets/texture/texture/toon/";

        MateFile baseMateFile;

        public bool export
        { get; set; } = false;

        public int matno
        { get; set; } = -1;

        public string basePath
        { get; set; } = string.Empty;

        public bool allowOverwrite
        { get; set; } = false;

        public bool allowSamename
        { get; set; } = false;

        public string category
        { get; set; } = string.Empty;

        public string slotname
        { get; set; } = string.Empty;

        public string basename
        { get; set; } = string.Empty;

        public string outputPath
        { get; set; } = string.Empty;

        public string filename
        { get; set; } = string.Empty;

        public int version
        { get; set; } = 1000;

        public string name
        { get; set; } = string.Empty;

        public PmatFile pmatFile
        { get; set; }

        public string shader1
        { get; set; } = string.Empty;

        public string shader2
        { get; set; } = string.Empty;

        public Dictionary<string, TexParams> texs
        { get; set; }

        public Dictionary<string, Color> colors
        { get; set; }

        public Dictionary<string, Vector4> vecs
        { get; set; }

        public Dictionary<string, float> parameters
        { get; set; }

        private string getOutputPath()
        {
            return Path.Combine(basePath, outputPath);
        }

        public bool LoadFile(string filename)
        {
            if (String.IsNullOrEmpty(Path.GetExtension(filename)) || Path.GetExtension(filename) != Params.ExtentionOfMate)
            {
                filename += Params.ExtentionOfMate;
            }
            baseMateFile = new MateFile();
            this.filename = Path.GetFileNameWithoutExtension(filename);
            baseMateFile.filename = Path.GetFileNameWithoutExtension(filename);
            byte[] cd = null;
            try
            {
                using (AFileBase aFileBase = global::GameUty.FileOpen(filename))
                {
                    if (!aFileBase.IsValid())
                    {
                        logger.ErrorLog("マテリアルファイルが見つかりません。", filename);
                        return false;
                    }
                    cd = aFileBase.ReadAll();
                }
            }
            catch (Exception ex2)
            {
                logger.ErrorLog("マテリアルファイルが読み込めませんでした。", filename, ex2.Message);
                return false;
            }

            using (BinaryReader binaryReader = new BinaryReader(new MemoryStream(cd), Encoding.UTF8))
            {
                string text = binaryReader.ReadString();
                if (text != materialHeaderString)
                {
                    logger.ErrorLog("例外: ヘッダーファイルが不正です。" + text);
                    return false;
                }
                version = binaryReader.ReadInt32();
                baseMateFile.version = version;
                name = binaryReader.ReadString();
                baseMateFile.name = name;
                string pmat = binaryReader.ReadString();
                pmatFile = new PmatFile();
                pmatFile.LoadFile(pmat);
                shader1 = binaryReader.ReadString();
                baseMateFile.shader1 = shader1;
                shader2 = binaryReader.ReadString();
                baseMateFile.shader2 = shader2;

                while (true)
                {
                    string key = binaryReader.ReadString();
                    if (key == "end")
                    {
                        break;
                    }

                    string propertyName = binaryReader.ReadString();
                    if (key == "tex")
                    {
                        TexParams tex = new TexParams();
                        tex.mateFile = this;
                        tex.propertyName = propertyName;
                        tex.type = binaryReader.ReadString();
                        if (tex.type == "null")
                        {
                        }
                        else if (tex.type == TexParams.TexTypeTex2d)
                        {
                            tex.filename = binaryReader.ReadString();
                            tex.assets = binaryReader.ReadString();

                            Vector2 offset;
                            offset.x = binaryReader.ReadSingle();
                            offset.y = binaryReader.ReadSingle();
                            tex.offset = offset;

                            Vector2 scale;
                            scale.x = binaryReader.ReadSingle();
                            scale.y = binaryReader.ReadSingle();
                            tex.scale = scale;

                            tex.texFile = new TexFile();
                            tex.texFile.LoadFile(tex.filename);
                        }
                        else if (tex.type == TexParams.TexTypeTexRT)
                        {
                            tex.text7 = binaryReader.ReadString();
                            tex.text8 = binaryReader.ReadString();
                        }
                        if (texs == null)
                        {
                            texs = new Dictionary<string, TexParams>();
                        }
                        else if (texs.ContainsKey(propertyName))
                        {
                            continue;
                        }
                        texs.Add(propertyName, tex);
                    }
                    else if (key == "col")
                    {
                        Color color;
                        color.r = binaryReader.ReadSingle();
                        color.g = binaryReader.ReadSingle();
                        color.b = binaryReader.ReadSingle();
                        color.a = binaryReader.ReadSingle();
                        if (colors == null)
                        {
                            colors = new Dictionary<string, Color>();
                        }
                        else if (colors.ContainsKey(propertyName))
                        {
                            continue;
                        }
                        colors.Add(propertyName, color);
                    }
                    else if (key == "vec")
                    {
                        Vector4 vector;
                        vector.x = binaryReader.ReadSingle();
                        vector.y = binaryReader.ReadSingle();
                        vector.z = binaryReader.ReadSingle();
                        vector.w = binaryReader.ReadSingle();
                        if (vecs == null)
                        {
                            vecs = new Dictionary<string, Vector4>();
                        }
                        else if (vecs.ContainsKey(propertyName))
                        {
                            continue;
                        }
                        vecs.Add(propertyName, vector);
                    }
                    else if (key == "f")
                    {
                        float value = binaryReader.ReadSingle();
                        if (parameters == null)
                        {
                            parameters = new Dictionary<string, float>();
                        }
                        else if (parameters.ContainsKey(propertyName))
                        {
                            continue;
                        }
                        parameters.Add(propertyName, value);
                    }
                    else
                    {
                        logger.ErrorLog("マテリアルが読み込めません。不正なマテリアルプロパティ型です ", key);
                        return false;
                    }

                }
            }
            return true;
        }

        public bool WriteMate(string path, string filename)
        {
            this.outputPath = path;
            this.filename = filename;
            return WriteMate();
        }

        public bool WriteMate(string filename)
        {
            this.filename = filename;
            return WriteMate();
        }

        public bool WriteMate()
        {
            if (!validate())
            {
                return false;
            }

            string outputPath = getOutputPath();
            string outfile = Path.Combine(outputPath, filename);
            if (String.IsNullOrEmpty(Path.GetExtension(outfile)) || Path.GetExtension(outfile) != Params.ExtentionOfMate)
            {
                outfile += Params.ExtentionOfMate;
            }
            if (!Directory.Exists(outputPath))
            {
                Directory.CreateDirectory(outputPath);
            }
            try
            {
                using (BinaryWriter writer = new BinaryWriter(File.OpenWrite(outfile)))
                {
                    writer.Write(materialHeaderString);
                    writer.Write(version);
                    writer.Write(name);
                    writer.Write(pmatFile.FilenameWithoutExtension);
                    writer.Write(shader1);
                    writer.Write(shader2);
                    if (texs != null)
                    {
                        foreach (var propertyName in texs.Keys)
                        {
                            var tex = texs[propertyName];
                            writer.Write("tex");
                            writer.Write(propertyName);
                            writer.Write(tex.type);
                            if (tex.type == TexParams.TexTypeTex2d)
                            {
                                writer.Write(tex.filename);
                                writer.Write(tex.assets);
                                writer.Write(tex.offset.x);
                                writer.Write(tex.offset.y);
                                writer.Write(tex.scale.x);
                                writer.Write(tex.scale.y);
                            }
                            else if (tex.type == TexParams.TexTypeTexRT)
                            {
                                writer.Write(tex.text7);
                                writer.Write(tex.text8);
                            }
                        }
                    }
                    if (colors != null)
                    {
                        foreach (var propertyName in colors.Keys)
                        {
                            var color = colors[propertyName];
                            writer.Write("col");
                            writer.Write(propertyName);
                            writer.Write(color.r);
                            writer.Write(color.g);
                            writer.Write(color.b);
                            writer.Write(color.a);
                        }
                    }
                    if (vecs != null)
                    {
                        foreach (var propertyName in vecs.Keys)
                        {
                            var vec = vecs[propertyName];
                            writer.Write("vec");
                            writer.Write(propertyName);
                            writer.Write(vec.x);
                            writer.Write(vec.y);
                            writer.Write(vec.z);
                            writer.Write(vec.w);
                        }
                    }
                    if (parameters != null)
                    {
                        foreach (var propertyName in parameters.Keys)
                        {
                            var f = parameters[propertyName];
                            writer.Write("f");
                            writer.Write(propertyName);
                            writer.Write(f);
                        }
                    }
                    writer.Write("end");
                }
            }
            catch (Exception e)
            {
                Debug.LogException(e);
                return false;
            }
            return true;
        }

        private bool validate()
        {
            if (String.IsNullOrEmpty(outputPath))
            {
                NDebug.MessageBox("エラー", "出力先フォルダがセットされていません");
                return false;
            }
            if (String.IsNullOrEmpty(filename))
            {
                NDebug.MessageBox("エラー", "ファイル名がセットされていません");
                return false;
            }
            if (String.IsNullOrEmpty(name))
            {
                NDebug.MessageBox("エラー", "名前がセットされていません");
                return false;
            }
            if (String.IsNullOrEmpty(shader1))
            {
                NDebug.MessageBox("エラー", "シェーダー1がセットされていません");
                return false;
            }
            if (String.IsNullOrEmpty(shader2))
            {
                NDebug.MessageBox("エラー", "シェーダー2がセットされていません");
                return false;
            }
            char[] invalidPathChars = Path.GetInvalidPathChars();
            if (invalidPathChars.Any(ch => outputPath.Contains(ch)))
            {
                NDebug.MessageBox("エラー", "出力先フォルダに不正な文字が含まれています");
                return false;
            }
            char[] invalidFileNameChars = Path.GetInvalidFileNameChars();
            if (invalidFileNameChars.Any(ch => filename.Contains(ch)))
            {
                NDebug.MessageBox("エラー", "ファイル名に不正な文字が含まれています");
                return false;
            }
            if (!allowOverwrite)
            {
                if (File.Exists(Path.Combine(getOutputPath(), filename)))
                {
                    NDebug.MessageBox("エラー", "既にファイルが存在します");
                    return false;
                }
            }
            if (!allowSamename)
            {
                if (FileUtil.FileExists(filename))
                {
                    NDebug.MessageBox("エラー", "既に同名のmaterialが存在します");
                    return false;
                }
            }
            return true;
        }
    }

    class TexParams : IFileSelectable
    {
        public const string TexTypeTex2d = "tex2d";
        public const string TexTypeTexRT = "texRT";

        public Params.FileType fileType
        {
            get
            {
                return Params.FileType.image;
            }
        }

        public string FilenameWithoutExtension
        {
            get
            {
                if (filename == null)
                {
                    return filename;
                }
                if (Path.GetExtension(filename) != null && Path.GetExtension(filename) == Params.ExtentionOfTex)
                {
                    return Path.GetFileNameWithoutExtension(filename);
                }
                return filename;
            }
        }

        public string FilenameWithExtension
        {
            get
            {
                if (filename == null)
                {
                    return filename;
                }
                if (Path.GetExtension(filename) != null && Path.GetExtension(filename) == Params.ExtentionOfTex)
                {
                    return filename;
                }
                return filename + Params.ExtentionOfTex;
            }
        }
        public MateFile mateFile
        { get; set; }

        public TexFile texFile
        { get; set; }

        public bool export
        { get; set; } = false;

        public string propertyName
        { get; set; } = string.Empty;

        public string type
        { get; set; } = string.Empty;

        public string filename
        { get; set; } = string.Empty;

        public string assets
        { get; set; } = string.Empty;

        public Vector2 offset
        { get; set; } = Vector2.zero;

        public Vector2 scale
        { get; set; } = Vector2.zero;

        public string text7
        { get; set; } = string.Empty;

        public string text8
        { get; set; } = string.Empty;

        public bool LoadFile(string filename)
        {
            //TODO
            return false;
        }
    }

    class TexFile : IFileSelectable
    {
        public Params.FileType fileType
        {
            get
            {
                return Params.FileType.tex;
            }
        }

        public string FilenameWithoutExtension
        {
            get
            {
                if (filename == null)
                {
                    return filename;
                }
                if (Path.GetExtension(filename) != null && Path.GetExtension(filename) == Params.ExtentionOfTex)
                {
                    return Path.GetFileNameWithoutExtension(filename);
                }
                return filename;
            }
        }

        public string FilenameWithExtension
        {
            get
            {
                if (filename == null)
                {
                    return filename;
                }
                if (Path.GetExtension(filename) != null && Path.GetExtension(filename) == Params.ExtentionOfTex)
                {
                    return filename;
                }
                return filename + Params.ExtentionOfTex;
            }
        }

        Logger logger = new Logger("TexFile");

        const string texHeaderString = "CM3D2_TEX";

        const string assetsFormat = "assets/texture/texture/{0}/{1}/";

        Texture2D texture;

        TexFile baseTexFile;

        public bool export
        { get; set; } = false;

        public string basePath
        { get; set; } = string.Empty;

        public string category
        { get; set; } = string.Empty;

        public string basename
        { get; set; } = string.Empty;

        public bool allowOverwrite
        { get; set; } = false;

        public bool allowSamename
        { get; set; } = false;

        public string outputPath
        { get; set; } = string.Empty;

        public string filename
        { get; set; } = string.Empty;

        public int version
        { get; set; } = 1000;

        public string assets
        { get; set; } = string.Empty;


        private string getOutputPath()
        {
            return Path.Combine(basePath, outputPath);
        }

        public bool SetPNG(string filepath)
        {
            byte[] img = UTY.LoadImage(filepath);
            texture = new Texture2D(2, 2, TextureFormat.RGBA32, false);
            texture.LoadImage(img);
            filename = Path.GetFileNameWithoutExtension(filepath);

            return true;
        }

        public bool LoadFile(string filename)
        {
            if (String.IsNullOrEmpty(Path.GetExtension(filename)) || Path.GetExtension(filename) != Params.ExtentionOfTex)
            {
                filename += Params.ExtentionOfTex;
            }
            baseTexFile = new TexFile();
            this.filename = Path.GetFileNameWithoutExtension(filename);
            baseTexFile.filename = Path.GetFileNameWithoutExtension(filename);

            byte[] cd = null;
            try
            {
                using (AFileBase aFileBase = global::GameUty.FileOpen(filename))
                {
                    if (!aFileBase.IsValid())
                    {
                        logger.ErrorLog("texファイルが見つかりません。", filename);
                        return false;
                    }
                    cd = aFileBase.ReadAll();
                }
            }
            catch (Exception ex2)
            {
                logger.ErrorLog("texファイルが読み込めませんでした。", filename, ex2.Message);
                return false;
            }
            try
            {
                using (BinaryReader binaryReader = new BinaryReader(new MemoryStream(cd), Encoding.UTF8))
                {
                    string header = binaryReader.ReadString();
                    if (header != texHeaderString)
                    {
                        logger.ErrorLog("例外: ヘッダーファイルが不正です。" + header);
                        return false;
                    }
                    version = binaryReader.ReadInt32();
                    assets = binaryReader.ReadString();
                    baseTexFile.assets = assets;
                    int size = binaryReader.ReadInt32();
                    byte[] data = new byte[Math.Max(500000, size)];
                    if (data.Length < size)
                    {
                        data = new byte[size];
                    }
                    binaryReader.Read(data, 0, size);
                    texture = new Texture2D(2, 2, TextureFormat.RGBA32, false);
                    texture.LoadImage(data);
                }
            }
            catch (Exception e)
            {
                Debug.Log(e);
                return false;
            }

            return true;
        }

        public bool WriteTex(string path, string filename)
        {
            this.outputPath = path;
            this.filename = filename;
            return WriteTex();
        }

        public bool WriteTex(string filename)
        {
            this.filename = filename;
            return WriteTex();
        }

        public bool WriteTex()
        {
            if (!validate())
            {
                return false;
            }
            string outputPath = getOutputPath();
            string outfile = Path.Combine(outputPath, filename);
            if (String.IsNullOrEmpty(Path.GetExtension(outfile)) || Path.GetExtension(outfile) != Params.ExtentionOfTex)
            {
                outfile += Params.ExtentionOfTex;
            }
            if (!Directory.Exists(outputPath))
            {
                Directory.CreateDirectory(outputPath);
            }
            try
            {
                byte[] cd = texture.EncodeToPNG();
                using (BinaryWriter writer = new BinaryWriter(File.OpenWrite(outfile)))
                {
                    writer.Write(texHeaderString);
                    writer.Write(version);
                    assets = string.Format(assetsFormat, category, basename);
                    writer.Write(assets);
                    writer.Write(cd.Length);
                    writer.Write(cd);
                }
            }
            catch (Exception ex2)
            {
                logger.ErrorLog(ex2.Message);
                return false;
            }
            return true;
        }

        private bool validate()
        {
            if (String.IsNullOrEmpty(outputPath))
            {
                NDebug.MessageBox("エラー", "出力先フォルダがセットされていません");
                return false;
            }
            if (String.IsNullOrEmpty(filename))
            {
                NDebug.MessageBox("エラー", "ファイル名がセットされていません");
                return false;
            }
            if (texture == null)
            {
                NDebug.MessageBox("エラー", "textureがセットされていません");
                return false;
            }
            char[] invalidPathChars = Path.GetInvalidPathChars();
            if (invalidPathChars.Any(ch => outputPath.Contains(ch)))
            {
                NDebug.MessageBox("エラー", "出力先フォルダに不正な文字が含まれています");
                return false;
            }
            char[] invalidFileNameChars = Path.GetInvalidFileNameChars();
            if (invalidFileNameChars.Any(ch => filename.Contains(ch)))
            {
                NDebug.MessageBox("エラー", "ファイル名に不正な文字が含まれています");
                return false;
            }
            if (!allowOverwrite)
            {
                if (File.Exists(Path.Combine(getOutputPath(), filename)))
                {
                    NDebug.MessageBox("エラー", "既にファイルが存在します");
                    return false;
                }
            }
            if (!allowSamename)
            {
                if (FileUtil.FileExists(filename))
                {
                    NDebug.MessageBox("エラー", "既に同名のtextureが存在します");
                    return false;
                }
            }
            return true;
        }
    }

    class ModelFile : IFileSelectable
    {
        public enum AddItemType
        {
            normal,
            attach,
            attachBone,
            handItem
        }

        public Params.FileType fileType
        {
            get
            {
                return Params.FileType.model;
            }
        }

        public string FilenameWithoutExtension
        {
            get
            {
                if (filename == null)
                {
                    return filename;
                }
                if (Path.GetExtension(filename) != null && Path.GetExtension(filename) == Params.ExtentionOfModel)
                {
                    return Path.GetFileNameWithoutExtension(filename);
                }
                return filename;
            }
        }

        public string FilenameWithExtension
        {
            get
            {
                if (filename == null)
                {
                    return filename;
                }
                if (Path.GetExtension(filename) != null && Path.GetExtension(filename) == Params.ExtentionOfModel)
                {
                    return filename;
                }
                return filename + Params.ExtentionOfModel;
            }
        }

        Logger logger = new Logger("ModelFile");

        const string modelHeaderString = "CM3D2_MODEL";

        ModelFile baseModelFile;

        public bool export
        { get; set; } = false;

        public AddItemType addItemType
        { get; set; } = AddItemType.normal;

        public string basePath
        { get; set; } = string.Empty;

        public string category
        { get; set; } = string.Empty;

        public string slotname
        { get; set; } = string.Empty;

        public string attachSlot
        { get; set; } = string.Empty;

        public string attachName
        { get; set; } = string.Empty;

        public string basename
        { get; set; } = string.Empty;

        public bool allowOverwrite
        { get; set; } = false;

        public bool allowSamename
        { get; set; } = false;

        public string outputPath
        { get; set; } = string.Empty;

        public string filename
        { get; set; } = string.Empty;

        public int version
        { get; set; } = 1000;

        private string getOutputPath()
        {
            return Path.Combine(basePath, outputPath);
        }

        public bool WriteModel(string path, string filename)
        {
            this.outputPath = path;
            this.filename = filename;
            return WriteModel();
        }

        public bool WriteModel(string filename)
        {
            this.filename = filename;
            return WriteModel();
        }

        public bool LoadFile(string filename)
        {
            if (String.IsNullOrEmpty(Path.GetExtension(filename)) || Path.GetExtension(filename) != Params.ExtentionOfModel)
            {
                filename += Params.ExtentionOfModel;
            }
            baseModelFile = new ModelFile();
            this.filename = Path.GetFileNameWithoutExtension(filename);
            baseModelFile.filename = Path.GetFileNameWithoutExtension(filename);

            // TODO shaderの変更ぐらい実装したい
            return true;
        }

        public bool WriteModel()
        {
            if (!validate())
            {
                return false;
            }
            string outputPath = getOutputPath();
            string outfile = Path.Combine(outputPath, filename);
            if (String.IsNullOrEmpty(Path.GetExtension(outfile)) || Path.GetExtension(outfile) != Params.ExtentionOfModel)
            {
                outfile += Params.ExtentionOfModel;
            }
            if (!Directory.Exists(outputPath))
            {
                Directory.CreateDirectory(outputPath);
            }
            try
            {
                using (AFileBase aFileBase = global::GameUty.FileOpen(baseModelFile.filename + Params.ExtentionOfModel))
                {
                    if (!aFileBase.IsValid())
                    {
                        logger.ErrorLog("Modelファイルが見つかりません。", filename);
                        return false;
                    }
                    byte[] cd = aFileBase.ReadAll();
                    using (BinaryWriter writer = new BinaryWriter(File.OpenWrite(outfile)))
                    {
                        writer.Write(cd);
                    }
                }
            }
            catch (Exception ex2)
            {
                logger.ErrorLog("Modelファイルが読み込めませんでした。", filename, ex2.Message);
                return false;
            }
            return true;
        }

        private bool validate()
        {
            if (String.IsNullOrEmpty(outputPath))
            {
                NDebug.MessageBox("エラー", "出力先フォルダがセットされていません");
                return false;
            }
            if (String.IsNullOrEmpty(filename))
            {
                NDebug.MessageBox("エラー", "ファイル名がセットされていません");
                return false;
            }
            char[] invalidPathChars = Path.GetInvalidPathChars();
            if (invalidPathChars.Any(ch => outputPath.Contains(ch)))
            {
                NDebug.MessageBox("エラー", "出力先フォルダに不正な文字が含まれています");
                return false;
            }
            char[] invalidFileNameChars = Path.GetInvalidFileNameChars();
            if (invalidFileNameChars.Any(ch => filename.Contains(ch)))
            {
                NDebug.MessageBox("エラー", "ファイル名に不正な文字が含まれています");
                return false;
            }
            if (!allowOverwrite)
            {
                if (File.Exists(Path.Combine(getOutputPath(), filename)))
                {
                    NDebug.MessageBox("エラー", "既にファイルが存在します");
                    return false;
                }
            }
            if (!allowSamename)
            {
                if (FileUtil.FileExists(filename))
                {
                    NDebug.MessageBox("エラー", "既に同名のmodelが存在します");
                    return false;
                }
            }
            return true;
        }
    }

    class PmatFile : IFileSelectable
    {
        public Params.FileType fileType
        {
            get
            {
                return Params.FileType.pmat;
            }
        }

        public string FilenameWithoutExtension
        {
            get
            {
                if (filename == null)
                {
                    return filename;
                }
                if (Path.GetExtension(filename) != null && Path.GetExtension(filename) == Params.ExtentionOfPmat)
                {
                    return Path.GetFileNameWithoutExtension(filename);
                }
                return filename;
            }
        }

        public string FilenameWithExtension
        {
            get
            {
                if (filename == null)
                {
                    return filename;
                }
                if (Path.GetExtension(filename) != null && Path.GetExtension(filename) == Params.ExtentionOfPmat)
                {
                    return filename;
                }
                return filename + Params.ExtentionOfPmat;
            }
        }

        Logger logger = new Logger("PmatFile");

        const string pmatHeaderString = "CM3D2_PMATERIAL";

        PmatFile basePmatFile;

        public bool export
        { get; set; } = false;

        public string basePath
        { get; set; } = string.Empty;

        public bool allowOverwrite
        { get; set; } = false;

        public bool allowSamename
        { get; set; } = false;

        public string outputPath
        { get; set; } = string.Empty;

        public string filename
        { get; set; } = string.Empty;

        public int version
        { get; set; } = 1000;

        public string name
        { get; set; } = string.Empty;

        public float value
        { get; set; } = 2000;

        public string shader
        { get; set; } = "CM3D2/Toony_Lighted_Trans";

        public string getOutputPath()
        {
            return Path.Combine(basePath, outputPath);
        }

        public bool LoadFile(string filename)
        {
            if (String.IsNullOrEmpty(Path.GetExtension(filename)) || Path.GetExtension(filename) != Params.ExtentionOfPmat)
            {
                filename += Params.ExtentionOfPmat;
            }
            basePmatFile = new PmatFile();
            this.filename = Path.GetFileNameWithoutExtension(filename);
            basePmatFile.filename = Path.GetFileNameWithoutExtension(filename);

            byte[] cd = null;
            try
            {
                using (AFileBase aFileBase = global::GameUty.FileOpen(filename))
                {
                    if (!aFileBase.IsValid())
                    {
                        logger.DebugLog("pmatファイルが見つかりません。", filename);
                        return false;
                    }
                    cd = aFileBase.ReadAll();
                }
            }
            catch (Exception ex2)
            {
                logger.ErrorLog("pmatファイルが見つかりません。", filename, ex2.Message);
                return false;
            }
            try
            {
                using (BinaryReader binaryReader = new BinaryReader(new MemoryStream(cd), Encoding.UTF8))
                {
                    string header = binaryReader.ReadString();
                    if (header != pmatHeaderString)
                    {
                        logger.ErrorLog("例外: ヘッダーファイルが不正です。" + header);
                        return false;
                    }
                    version = binaryReader.ReadInt32();
                    basePmatFile.version = version;
                    int hashKey = binaryReader.ReadInt32();
                    name = binaryReader.ReadString();
                    basePmatFile.name = name;
                    value = binaryReader.ReadSingle();
                    basePmatFile.value = value;
                    shader = binaryReader.ReadString();
                    basePmatFile.shader = shader;
                }
            }
            catch (Exception e)
            {
                Debug.Log(e);
                return false;
            }

            return true;
        }

        public bool WritePmat(string path, string filename)
        {
            this.outputPath = path;
            this.filename = filename;
            return WritePmat();
        }

        public bool WritePmat(string filename)
        {
            this.filename = filename;
            return WritePmat();
        }

        public bool WritePmat()
        {
            if (!validate())
            {
                return false;
            }
            string outputPath = getOutputPath();
            string outfile = Path.Combine(outputPath, filename);
            if (String.IsNullOrEmpty(Path.GetExtension(outfile)) || Path.GetExtension(outfile) != Params.ExtentionOfPmat)
            {
                outfile += Params.ExtentionOfPmat;
            }
            if (!Directory.Exists(outputPath))
            {
                Directory.CreateDirectory(outputPath);
            }
            try
            {
                using (BinaryWriter writer = new BinaryWriter(File.OpenWrite(outfile)))
                {
                    writer.Write(pmatHeaderString);
                    writer.Write(version);
                    writer.Write(name.GetHashCode());
                    writer.Write(name);
                    writer.Write(value);
                    writer.Write(shader);
                }
            }
            catch (Exception e)
            {
                Debug.LogException(e);
                return false;
            }
            return true;
        }

        private bool validate()
        {
            if (String.IsNullOrEmpty(outputPath))
            {
                NDebug.MessageBox("エラー", "出力先フォルダがセットされていません");
                return false;
            }
            if (String.IsNullOrEmpty(filename))
            {
                NDebug.MessageBox("エラー", "ファイル名がセットされていません");
                return false;
            }
            if (String.IsNullOrEmpty(name))
            {
                NDebug.MessageBox("エラー", "名前がセットされていません");
                return false;
            }
            if (String.IsNullOrEmpty(shader))
            {
                NDebug.MessageBox("エラー", "ファイル名に不正な文字が含まれています");
                logger.ErrorLog("シェーダーがセットされていません");
                return false;
            }
            char[] invalidPathChars = Path.GetInvalidPathChars();
            if (invalidPathChars.Any(ch => outputPath.Contains(ch)))
            {
                NDebug.MessageBox("エラー", "出力先フォルダに不正な文字が含まれています");
                return false;
            }
            char[] invalidFileNameChars = Path.GetInvalidFileNameChars();
            if (invalidFileNameChars.Any(ch => filename.Contains(ch)))
            {
                NDebug.MessageBox("エラー", "ファイル名に不正な文字が含まれています");
                return false;
            }
            if (!allowOverwrite)
            {
                if (File.Exists(Path.Combine(getOutputPath(), filename)))
                {
                    NDebug.MessageBox("エラー", "既にファイルが存在します");
                }
            }
            if (!allowSamename)
            {
                string[] list = global::GameUty.FileSystem.GetList("prioritymaterial", AFileSystemBase.ListType.AllFile);
                if (list.Contains(name))
                {
                    NDebug.MessageBox("エラー", "既に同名のpmatが存在します");
                    return false;
                }
            }
            return true;
        }
    }
}
