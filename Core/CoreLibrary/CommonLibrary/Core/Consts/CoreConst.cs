using System.ComponentModel;

namespace CoreLibrary.Core.Consts
{
    /// <summary>
    /// 定数定義
    /// </summary>
    /// <remarks>
    /// 作成日：2017/10/31
    /// 作成者：MATSUBAYASHI Atsushi
    /// </remarks>
    public class CoreConst
    {
        /// <summary>
        /// ヘッダーパターンID（1:基本）
        /// </summary>
        public const string HEADER_PATTERN_ID_1 = "1";

        /// <summary>
        /// ヘッダーパターンID（2:別ウィンドウ等）
        /// </summary>
        public const string HEADER_PATTERN_ID_2 = "2";

        /// <summary>
        /// ヘッダーパターンID（3:ヘッダ無し）
        /// </summary>
        public const string HEADER_PATTERN_ID_3 = "3";

        /// <summary>
        /// ヘッダーパターンID（4:別ウィンドウ（閉じるなし））
        /// </summary>
        public const string HEADER_PATTERN_ID_4 = "4";

        /// <summary>
        /// ドロップダウンリストの表示モード
        /// </summary>
        public enum DisplayMode
        {
            KbnCd,         // 区分コード
            KbnNm,         // 区分名称
            KbnCdNm        // 区分コード名称
        }

        /// <summary>
        /// 非表示フラグ（0:表示）
        /// </summary>
        public const string HIDDEN_FLG_0 = "0";

        /// <summary>
        /// 非表示フラグ（1:非表示）
        /// </summary>
        public const string HIDDEN_FLG_1 = "1";

        /// <summary>
        /// 画面マスタのキャッシュキー
        /// </summary>
        public const string M_SCREEN_CACHE = "MScreenCache";

        /// <summary>
        /// 元号マスタのキャッシュキー
        /// </summary>
        public const string M_GENGO_CACHE = "MGengoCache";

        /// <summary>
        /// 汎用区分マスタのキャッシュキー
        /// </summary>
        public const string M_HANYOKUBUN_CACHE = "MHanyokubunCache";

        /// <summary>
        /// 名称マスタのキャッシュキー
        /// </summary>
        public const string M_MEISHO_CACHE = "MMeishoCache";

        /// <summary>
        /// メッセージマスタのキャッシュキー
        /// </summary>
        public const string M_MESSAGE_CACHE = "MMessageCache";

        /// <summary>
        /// ヘルプメッセージマスタのキャッシュキー
        /// </summary>
        public const string M_HELP_MESSAGE_CACHE = "MHelpMessageCache";

        /// <summary>
        /// 都道府県マスタのキャッシュキー
        /// </summary>
        public const string M_TODOFUKEN_CACHE = "MTodofukenCache";

        /// <summary>
        /// システム設定値マスタのキャッシュキー
        /// </summary>
        public const string M_CORE_CONFIG_CACHE = "MCoreConfigCache";

        /// <summary>
        /// 帳票処理管理マスタのキャッシュキー
        /// </summary>
        public const string M_REPORT_KANRI_CACHE = "MReportKanriCache";

        /// <summary>
        /// メニューマスタのキャッシュキー
        /// </summary>
        public const string M_MENU_CACHE = "MMenuCache";

        /// <summary>
        /// ヘルプメニューマスタのキャッシュキー
        /// </summary>
        public const string M_HELP_MENU_CACHE = "MHelpMenuCache";

        /// <summary>
        /// 年度マスタのキャッシュキー
        /// </summary>
        public const string M_NENDO_CACHE = "MNendoCache";

        /// <summary>
        /// 入力方法PDFマスタのキャッシュキー
        /// </summary>
        public const string M_NYURYOKUHOHO_PDF_CACHE = "MNyuryokuhohoPdfCache";

        /// <summary>
        /// ログイン時遷移先マスタのキャッシュキー
        /// </summary>
        public const string M_LOGIN_TRANSITION_CACHE = "MLoginTransitionCache";

        /// <summary>
        /// メッセージマスタのキャッシュキー
        /// システム共通向け
        /// </summary>
        public const string M_SYSTEM_MESSAGE_CACHE = "MSystemMessageCache";

        /// <summary>
        /// ロガー：開始文字列
        /// </summary>
        public const string LOG_START_KEYWORD = "BEGIN";

        /// <summary>
        /// ロガー：終了文字列
        /// </summary>
        public const string LOG_END_KEYWORD = "END  ";

        /// <summary>
        /// ロガー：処理時間 開始文字列
        /// </summary>
        public const string LOG_TIMER_START_MESSAGE = "処理時間:";

        /// <summary>
        /// ロガー：処理時間 終了文字列
        /// </summary>
        public const string LOG_TIMER_END_MESSAGE = "(ミリ秒)";

        /// <summary>
        /// ロガー：サーバ環境変数からクライアント側IPアドレスを取得するキー
        /// </summary>
        public const string LOG_X_FORWARDED_FOR = "HTTP_X_FORWARDED_FOR";

        /// <summary>
        /// ロガー：内部例外の取得最大深度
        /// </summary>
        public const int LOG_MAX_INNER_EXCEPTION = 10;

        /// <summary>
        /// ASPXAUTH（認証クッキー）のセッションID
        /// </summary>
        public const string ASPXAUTH = ".ASPXAUTH";

        /// <summary>
        /// リクエスト時のセッションID
        /// </summary>
        public const string X_REQ_SESSION_ID = "X-Req-SessionId";

        /// <summary>
        /// リクエスト時のSSO用認可コード
        /// </summary>
        public const string X_REQ_SSO_AUTHORIZATION_CODE = "X-Req-AuthorizationCode";

        /// <summary>
        /// ファイル暗号化/復号化：AESブロックサイズ(単位：bit)
        /// 128bit固定
        /// </summary>
        public const int AES_BLOCK_SIZE_BIT = 128;

        /// <summary>
        /// ファイル暗号化/復号化：AESキーサイズ(単位：bit)
        /// 選択可能範囲：128bit,192bit,256bit
        /// </summary>
        public const int AES_KEY_SIZE_BIT = 256;

        /// <summary>
        /// ファイル暗号化/復号化：Saltサイズ(単位：Byte)
        /// </summary>
        public const int AES_SALT_SIZE_BYTE = AES_KEY_SIZE_BIT / 8;

        /// <summary>
        /// ファイル暗号化/復号化：システム設定値マスタ検索キー
        /// key:phrase
        /// </summary>
        public const string CORE_CONFIG_KEY_PHRASE = "phrase";

        /// <summary>
        /// 左メニュー操作フラグ("true"/"false")
        /// </summary>
        public const string IS_LEFT_SUB_MENU = "isLeftSubMenu";

        /// <summary>
        /// 子画面のウィンドウID
        /// </summary>
        public const string CHILD_WINDOW_ID = "cw";

        /// <summary>
        /// ページトークン
        /// </summary>
        public const string PAGE_TOKEN = "pt";

        /// <summary>
        /// メニューリスト
        /// （セッション）
        /// </summary>
        public const string SESS_MENU_LIST = "_D9000_MENU_LIST";

        /// <summary>
        /// ヘルプメニューリスト
        /// （セッション）
        /// </summary>
        public const string SESS_HELP_MENU_LIST = "_D9000_HELP_MENU_LIST";

        /// <summary>
        /// パンくず：セッションキー
        /// </summary>
        public const string SESS_BREADCRUMB = "_D9000_BREADCRUMB";

        /// <summary>
        /// ログインユーザー（職員）
        /// （セッション）
        /// </summary>
        public const string SESS_LOGIN_USER = "_D9000_LOGIN_USER";

        /// <summary>
        /// ログインユーザー（職員）のDB接続情報
        /// （セッション）
        /// </summary>
        public const string SESS_DB_CONN = "_D9000_DB_CONN";

        /// <summary>
        /// ログインユーザー（職員）の画面機能権限
        /// （セッション）
        /// </summary>
        public const string SESS_SCREEN_KINO_KENGEN = "_D9000_SCREEN_KINO_KENGEN";

        /// <summary>
        /// ログインユーザー（職員）の利用可能支所一覧
        /// （セッション）
        /// </summary>
        public const string SESS_SHISHO_GROUP = "_D9000_SESS_SHISHO_GROUP";

        /// <summary>
        /// ログインユーザー（申請者）
        /// （セッション）
        /// </summary>
        //public const string SESS_SHINSEISHA = "_D9000_SHINSEISHA";

        /// <summary>
        /// 経営体ＩＤと紐づく農業者ＩＤ
        /// （セッション）
        /// </summary>
        public const string SESS_NOGYOSHA_ID = "_D9000_NOGYOSHA_ID";

        /// <summary>
        /// 画面モード
        /// （セッション）
        /// </summary>
        public const string SESS_SCREEN_MODE = "_D9000_SCREEN_MODE";

        /// <summary>
        /// 経営体API使用履歴登録用データ
        /// （セッション）
        /// </summary>
        //public const string SESS_KEIEITAI_API_SHIYOU_RIREKI = "_D9000_KEIEITAI_API_SHIYOU_RIREKI";

        /// <summary>
        /// 共通機能中に発生した例外データ
        /// （セッション）
        /// </summary>
        public const string SESS_COMMON_EXCEPTION = "_D9000_SESS_COMMON_EXCEPTION";

        /// <summary>
        /// 共通機能中に発生した例外データの日時
        /// （セッション）
        /// </summary>
        public const string SESS_COMMON_ERROR_TIME = "_D9000_SESS_COMMON_ERROR_TIME";

        /// <summary>
        /// セッションに積んだキー名
        /// （セッション）
        /// </summary>
        public const string SESS_KEYS = "_SESS_KEYS";

        /// <summary>
        /// 取得インスタンスURL利用フラグ
        /// </summary>
        //public const string GET_INSTANCEURL_USE_FLAG = "GetInstanceUrlUseFlag";

        /// <summary>
        /// 画面の表示モード
        /// </summary>
        public enum ScreenMode
        {
            None,
            PC,
            Tablet,
            SmartPhone
        }

        /// <summary>
        /// 画面表示モード(pc)のMD5
        /// </summary>
        public const string SCREEN_MODE_PC_HASH = "bc54f4d60f1cec0f9a6cb70e13f2127a";

        /// <summary>
        /// 画面表示モード(tablet)のMD5
        /// </summary>
        public const string SCREEN_MODE_TABLET_HASH = "c4e0ba5422383082417c5f96ab121575";

        /// <summary>
        /// システム設定ファイルにある「システム時間フラグ」のタグ名
        /// </summary>
        public const string CLIENT_VALIDATION_ENABLED = "ClientValidationEnabled";

        /// <summary>
        /// システム設定ファイルにある「ウィルスチェックの待ち時間」のタグ名
        /// </summary>
        public const string UPLOAD_WAIT_TIME = "UploadWaitTime";

        /// <summary>
        /// システム設定ファイルにある「DBデフォルトスキーマ」のタグ名
        /// </summary>
        public const string DEFAULT_SCHEMA = "DefaultSchema";

        /// <summary>
        /// システム設定ファイルにある「DBコマンドタイムアウト」のキー名
        /// </summary>
        public const string COMMAND_TIMEOUT = "CommandTimeout";

        /// <summary>
        /// システム設定ファイルにある「業務アプリのシステム区分」のキー名
        /// </summary>
        public const string APP_ENV_SYSTEM_KBN = "SystemKbn";

        /// <summary>
        /// システム設定ファイルにある「実行環境」のタグ名
        /// </summary>
        public const string APP_ENV = "AppEnv";

        /// <summary>
        /// システム設定ファイルにある「システム時間フラグ」のタグ名
        /// </summary>
        public const string SYS_DATE_TIME_FLAG = "SysDateTimeFlag";

        /// <summary>
        /// システム設定ファイルにある「システム時間ファイルパス」のタグ名
        /// </summary>
        public const string SYS_DATE_TIME_PATH = "SysDateTimePath";

        /// <summary>
        /// システム設定ファイルにある「アップロードファイルの一時保存フォルダ」のタグ名
        /// </summary>
        public const string UPLOAD_TEMP_FOLDER = "UploadTempFolder";

        /// <summary>
        /// システム設定ファイルにある「帳票出力フォルダ」のタグ名
        /// </summary>
        public const string REPORT_OUTPUT_FOLDER = "ReportOutputFolder";

        /// <summary>
        /// システム設定ファイルにある「申請者用（オンラインユーザ）フラグ」のタグ名
        /// </summary>
        //public const string SHINSEI_FLAG = "ShinseiFlag";

        /// <summary>
        /// システム設定ファイルにある「簡易ログイン画面表示フラグ」のタグ名
        /// </summary>
        public const string D0000_DISPLAY_FLAG = "D0000DisplayFlag";

        /// <summary>
        /// システム設定ファイルにある「経営体情報外部ファイル取得フラグ」のタグ名
        /// </summary>
        //public const string KEIEITAI_FILE_READ_FLAG = "KeieitaiFileReadFlag";

        /// <summary>
        /// システム設定ファイルにある「経営体情報外部ファイル取得パス」のタグ名
        /// </summary>
        //public const string KEIEITAI_FILE_READ_PATH = "KeieitaiFileReadPath";

        /// <summary>
        /// システム設定ファイルにある「API用インスタンスURL」のタグ名
        /// </summary>
        //public const string API_INSTANCE_URL = "API_InstanceUrl";

        /// <summary>
        /// システム設定ファイルにある「API用インスタンスURL」のタグ名
        /// </summary>
        //public const string API_CLIENT_ID = "API_ClientId";

        /// <summary>
        /// システム設定ファイルにある「API用コンシューマ秘密鍵」のタグ名
        /// </summary>
        //public const string API_CLIENT_SECRET = "API_ClientSecret";

        /// <summary>
        /// システム設定ファイルにある「API用ユーザ名」のタグ名
        /// </summary>
        //public const string API_USER_NAME = "API_UserName";

        /// <summary>
        /// システム設定ファイルにある「API用パスワード」のタグ名
        /// </summary>
        //public const string API_PASSWORD = "API_Password";

        /// <summary>
        /// システム設定ファイルにある「API用シークレットトークン」のタグ名
        /// </summary>
        //public const string API_SECRET_TOKEN = "API_SecretToken";

        /// <summary>
        /// システム設定ファイルにある「SSO用インスタンスURL」のタグ名
        /// </summary>
        //public const string SSO_INSTANCE_URL = "SSO_InstanceUrl";

        /// <summary>
        /// システム設定ファイルにある「SSO用コンシューマ鍵」のタグ名
        /// </summary>
        //public const string SSO_CLIENT_ID = "SSO_ClientId";

        /// <summary>
        /// システム設定ファイルにある「SSO用コンシューマ秘密鍵」のタグ名
        /// </summary>
        //public const string SSO_CLIENT_SECRET = "SSO_ClientSecret";

        /// <summary>
        /// システム設定ファイルにある「SSO用リダイレクトURL」のタグ名
        /// </summary>
        //public const string SSO_REDIRECT_URL = "SSO_RedirectUrl";

        /// <summary>
        /// システム設定ファイルにある「SSO用nonce値固定フラグ」のタグ名
        /// </summary>
        //public const string SSO_FIXED_NONCE_FLAG = "SSO_FixedNonceFlag";

        /// <summary>
        /// システム設定ファイルにある「SSO用リダイレクトURL」のタグ名
        /// </summary>
        //public const string SSO_FIXED_NONCE_VALUE = "SSO_FixedNonceValue";

        /// <summary>
        /// システム設定ファイルにある「IF00015_認可コード取得 URLパーツ」のタグ名
        /// </summary>
        //public const string IF00015_URL_PARTS = "IF00015UrlParts";

        /// <summary>
        /// システム設定ファイルにある「IF00016_アクセストークン取得（SSO） URLパーツ」のタグ名
        /// </summary>
        //public const string IF00016_URL_PARTS = "IF00016UrlParts";

        /// <summary>
        /// システム設定ファイルにある「IF00018_ユーザ属性情報（eMAFF情報）取得 URLパーツ」のタグ名
        /// </summary>
        //public const string IF00018_URL_PARTS = "IF00018UrlParts";

        /// <summary>
        /// システム設定ファイルにある「IF00019_アクセストークン取得（外部API） URLパーツ」のタグ名
        /// </summary>
        //public const string IF00019_URL_PARTS = "IF00019UrlParts";

        /// <summary>
        /// システム設定ファイルにある「IF00020_経営体情報取得 URLパーツ」のタグ名
        /// </summary>
        //public const string IF00020_URL_PARTS = "IF00020UrlParts";

        /// <summary>
        /// システム設定ファイルにある「帳票PDF一時出力フォルダ」のタグ名
        /// </summary>
        public const string PRINT_TEMP_FOLDER = "PrintTempFolder";

        /// <summary>
        /// システム設定ファイルにある「認証クッキータイムアウト時間」のキー名
        /// </summary>
        public const string AUTHENTICATION_COOKIE_TIMEOUT = "Authentication_Cookie_Timeout";

        /// <summary>
        /// システム設定ファイルにある「セッションタイムアウト時間」のキー名
        /// </summary>
        public const string SESSION_STATE_TIMEOUT = "SessionState_Timeout";

        /// <summary>
        /// システム設定ファイルにある「CSV一時出力フォルダ」のキー名
        /// </summary>
        public const string CSV_TEMP_FOLDER = "CsvTempFolder";

        /// <summary>
        /// システム設定ファイルにある「即時CSV出力上限件数デフォルト値」のキー名
        /// </summary>
        public const string MAX_CNT_DEFAULT = "MaxCntDefault";

        /// <summary>
        /// システム設定ファイルにあるZIP書庫のMAXサイズのタグ名
        /// </summary>
        public const string ZIP_MAX_SIZE_TAG_NAME = "ZipMaxSize";

        /// <summary>
        /// システム設定ファイルにある帳票一時出力フォルダのタグ名
        /// </summary>
        public const string PRINT_TEMP_FOLDER_TAG_NAME = "PrintTempFolder";

        /// <summary>
        /// ドロップダウンリストの区切り文字
        /// </summary>
        public const string SEPARATOR = " ";

        /// <summary>
        /// ページサイズ
        /// </summary>
        public const int PAGE_SIZE = 10;

        /// <summary>
        /// ページャーのブロックID(初期表示位置)
        /// </summary>
        public const string PAGER_DIV_ID = "result";

        /// <summary>
        /// 検索結果のブロックID(初期表示位置)
        /// </summary>
        public const string SEARCH_RESULT_DIV_ID = "result";

        /// <summary>
        /// 遷移元画面を判別する引数screen
        /// </summary>
        public const string SCREEN_PAGE_FROM = "screen";

        /// <summary>
        /// 遷移元画面を判別するregex
        /// </summary>
        public const string SCREEN_PAGE_FROM_REGEX_MATCH = @"^[D][0-9]+$";

        /// <summary>
        /// システム選択画面から、収入保険システム遷移した後の遷移元画面の引数syuhoScreenFrom
        /// </summary>
        //public const string SYUHO_SCREEN_FROM = "syuhoScreenFrom";

        /// <summary>
        /// 改行文字列
        /// </summary>
        public const string NEW_LINE_SEPARATOR = "\r\n";

        /// <summary>
        /// パラメータの名前と値のセパレータ
        /// </summary>
        public const string PARAM_NAME_VALUE_SEPARATOR = ":";

        /// <summary>
        /// パラメータのセパレータ
        /// </summary>
        public const string PARAM_SEPARATOR = "; ";

        /// <summary>
        /// 記号：.
        /// </summary>
        public const string SYMBOL_DOT = ".";

        /// <summary>
        /// 記号：,
        /// </summary>
        public const string SYMBOL_COMMA = ",";

        /// <summary>
        /// 記号：※
        /// </summary>
        public const string SYMBOL_KOME = "※";

        /// <summary>
        /// 記号：、
        /// </summary>
        public const string SYMBOL_TOUTEN = "、";

        /// <summary>
        /// 半角スペース
        /// </summary>
        public const string HALF_WIDTH_SPACE = " ";

        /// <summary>
        /// 全角スペース
        /// </summary>
        public const string FULL_WIDTH_SPACE = "　";

        /// <summary>
        /// 記号：_
        /// </summary>
        public const string SYMBOL_UNDERSCORE = "_";

        /// <summary>
        /// ファイル拡張子:".csv"
        /// </summary>
        public const string FILE_EXTENSION_CSV = ".csv";

        /// <summary>
        /// ファイル拡張子:".zip"
        /// </summary>
        public const string FILE_EXTENSION_ZIP = ".zip";

        /// <summary>
        /// ソート順
        /// </summary>
        public enum SortOrder
        {
            [Description("降順")]
            DESC,
            [Description("昇順")]
            ASC,
        }

        /// <summary>
        /// システム利用者区分要素
        /// </summary>
        public enum SystemRiyoKbn
        {
            [Description("全国連合会職員")]
            Zenkokuren = 1,
            [Description("都道府県連合会職員")]
            Todofuken = 2,
            [Description("組合等")]
            Kumiaito = 3,
            [Description("支所")]
            Shisho = 4,
            [Description("運用業者")]
            Unyougyousya = 5,
            [Description("申請者")]
            Shinseisha = 6
        }

        /// <summary>
        /// システム利用者管理区分要素
        /// </summary>
        public enum SystemKanriKbn
        {
            [Description("一般職員")]
            IppanShokuin = 1,
            [Description("管理職員")]
            KanriShokuin = 2,
            [Description("システム管理者")]
            SystemKanrisha = 3,
            [Description("パートタイマー")]
            PartTimer = 4,
            [Description("ヘルプデスク")]
            HelpDesk = 5
        }

        /// <summary>
        /// ユーザ管理権限要素
        /// </summary>
        public enum UserKanriKengen
        {
            [Description("false")]
            False = 0,
            [Description("true")]
            True = 1
        }

        /// <summary>
        /// 利用権限要素
        /// </summary>
        public enum RiyoKengen
        {
            [Description("false")]
            False = 0,
            [Description("true")]
            True = 1
        }

        /// <summary>
        /// 参照更新権限要素
        /// </summary>
        public enum ReferenceUpdateKengen
        {
            [Description("false")]
            False = 0,
            [Description("true")]
            True = 1
        }

        /// <summary>
        /// システム区分
        /// </summary>
        public static class SystemKbn
        {
            [Description("加入状況管理")]
            public const string Kjk = "01";
            [Description("農作物共済")]
            public const string Nsk = "02";
            [Description("建物共済")]
            public const string Sml = "03";
            [Description("農業者情報管理")]
            public const string Fim = "04";
            [Description("収入保険")]
            public const string Syn = "50";
            [Description("家畜共済（共通申請）")]
            public const string KyotsuKtk = "51";
            [Description("園芸施設共済（共通申請）")]
            public const string KyotsuEng = "52";
            [Description("農作物共済（共通申請連携）")]
            public const string KyotsuRenkeiNsk = "53";
            [Description("畑作物共済（共通申請連携）")]
            public const string KyotsuRenkeiHat = "54";
            [Description("果樹共済（共通申請連携）")]
            public const string KyotsuRenkeiKju = "55";
            [Description("ベースアプリケーション")]
            public const string BAS = "88";
        }

        /// <summary>
        /// 文字コード
        /// </summary>
        public enum CharacterCode
        {
            [Description("UTF-8")]
            UTF8 = 1,
            [Description("Shift_JIS")]
            SJIS = 2,
        }

        /// <summary>
        /// フラグ　ON
        /// </summary>
        public const string FLG_ON = "1";

        /// <summary>
        /// フラグ　OFF
        /// </summary>
        public const string FLG_OFF = "0";

        /// <summary>
        /// セパレータ
        /// </summary>
        public enum Separator
        {
            [Description("コンマ")]
            COMMA = 1,
            [Description("タブ")]
            TAB = 2,
        }
    }
}