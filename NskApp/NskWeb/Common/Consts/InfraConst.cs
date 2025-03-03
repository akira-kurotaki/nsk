namespace NskWeb.Common.Consts
{
    /// <summary>
    /// NskWeb定数定義
    /// </summary>
    /// <remarks>
    /// 作成日：2018/09/14
    /// 作成者：MATSUBAYASHI Atsushi
    /// </remarks>
    public class InfraConst
    {
        /// <summary>
        /// メニューリスト
        /// （セッション）
        /// </summary>
        //public const string SESS_MENU_LIST = "_D9000_MENU_LIST";

        /// <summary>
        /// ヘルプメニューリスト
        /// （セッション）
        /// </summary>
        //public const string SESS_HELP_MENU_LIST = "_D9000_HELP_MENU_LIST";

        /// <summary>
        /// システム設定ファイルにある「ログインURL」のタグ名
        /// </summary>
        public const string LOGIN_URL = "LoginUrl";

        /// <summary>
        /// システム設定ファイルにある「収入保険システム 遷移先URL」のタグ名
        /// </summary>
        public const string SYN_D0001_TRANSITIONURL = "Syn_D0001_TransitionUrl";

        /// <summary>
        /// システム設定ファイルにある「農業者管理システム 遷移先URL」のタグ名
        /// </summary>
        public const string FIM_D0001_TRANSITIONURL = "Fim_D0001_TransitionUrl";

        /// <summary>
        /// システム設定ファイルにある「事業アプリケーションベース 遷移先URL」のキー名
        /// </summary>
        public const string BASEAPP_D0001_TRANSITIONURL = "BaseApp_D0001_TransitionUrl";

        /// <summary>
        /// メニューリンクのリクエスト文字列(ScreenId)
        /// </summary>
        public const string MENU_REQUEST_QUERYSTRING = "si";

        /// <summary>
        /// メニューリンクの処理リクエスト文字列(OpeId)
        /// </summary>
        public const string MENU_OPE_REQUEST_QUERYSTRING = "oi";

        /// <summary>
        /// メニューリンクからのPOSTデータキー：処理リクエスト文字列(OpeId)
        /// </summary>
        public const string MENU_POST_OPEID = "OpeId";

        /// <summary>
        /// システム設定ファイルにある「ユーザ情報変更URL」のタグ名
        /// </summary>
        public const string USER_INFO_MODIFY_URL = "UserInfoModifyUrl";

        /// <summary>
        /// システム設定ファイルにある「ログイン試行最大回数」のタグ名
        /// </summary>
        public const string LOGIN_TRAIAL_MAX_CNT = "LoginTraialMaxCnt";

        /// <summary>
        /// システム設定ファイルにある「D0401アップロード可能な拡張子」のキー名
        /// </summary>
        public const string D0401_UPLOAD_FILE_EXTENSION = "D0401UploadFileExtension";

        /// <summary>
        /// システム設定ファイルにある「D0401アップロードファイルの許容サイズ（単位：Byte）」のキー名
        /// </summary>
        public const string D0401_UPLOAD_FILE_MAX_SIZE = "D0401UploadFileMaxSize";

        /// <summary>
        /// システム設定ファイルにある「D0401アップロードファイルの許容サイズ（表示用）」のキー名
        /// </summary>
        public const string D0401_UPLOAD_FILE_MAX_DISP_SIZE = "D0401UploadFileMaxDispSize";

        /// <summary>
        /// システム設定ファイルにある「D0401アップロードファイルの一時保存フォルダ」のキー名
        /// </summary>
        public const string D0401_UPLOAD_TEMP_FOLDER = "D0401UploadTempFolder";

        /// <summary>
        /// システム設定ファイルにある「D0401アップロードファイルの保存先」のキー名
        /// </summary>
        public const string D0401_UPLOAD_FOLDER = "D0401UploadFolder";

    }
}