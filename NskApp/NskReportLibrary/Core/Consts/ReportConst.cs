using System.Drawing;

namespace NskReportLibrary.Core.Consts
{
    /// <summary>
    /// 定数定義
    /// 
    /// 作成日：2017/12/30
    /// 作成者：KAN RI
    /// </summary>
    public class ReportConst
    {
        /// <summary>
        /// 処理結果：成功
        /// </summary>
        public const int RESULT_SUCCESS = 0;

        /// <summary>
        /// 処理結果：失敗
        /// </summary>
        public const int RESULT_FAILED = 1;

        /// <summary>
        /// リアル帳票：0
        /// </summary>
        public const int REPORT_REALTIME = 0;

        /// <summary>
        /// バッチ帳票：1
        /// </summary>
        public const int REPORT_BATCH = 1;

        /// <summary>
        /// パラメータ和名：予約ID
        /// </summary>
        public const string PARAM_NAME_YOYAKU_ID = "予約ID";

        /// <summary>
        /// パラメータ和名：帳票制御ID
        /// </summary>
        public const string PARAM_NAME_REPORT_CONTROL_ID = "帳票制御ID";

        /// <summary>
        /// パラメータ和名：ユーザID
        /// </summary>
        public const string PARAM_NAME_USER_ID = "ユーザID";

        /// <summary>
        /// パラメータ和名：条件ID
        /// </summary>
        public const string PARAM_NAME_JOUKEN_ID = "条件ID";

        /// <summary>
        /// パラメータ和名：都道府県コード
        /// </summary>
        public const string PARAM_NAME_TODOFUKEN_CD = "都道府県コード";

        /// <summary>
        /// パラメータ和名：組合等コード
        /// </summary>
        public const string PARAM_NAME_KUMIAITO_CD = "組合等コード";

        /// <summary>
        /// パラメータ和名：支所コード
        /// </summary>
        public const string PARAM_NAME_SHISHO_CD = "支所コード";

        /// <summary>
        /// パラメータ和名：利用可能支所一覧
        /// </summary>
        public const string PARAM_NAME_SHISHO_LIST_CD = "利用可能支所一覧";

        /// <summary>
        /// パラメータ和名：帳票パス
        /// </summary>
        public const string PARAM_NAME_FILE_PATH = "帳票パス";

        /// <summary>
        /// パラメータ和名：バッチID
        /// </summary>
        public const string PARAM_NAME_BATCH_ID = "バッチID";

        /// <summary>
        /// パラメータ和名：バッチフラグ
        /// </summary>
        public const string PARAM_NAME_BATCH_FLAG = "バッチフラグ";

        /// <summary>
        /// パラメータ和名：出力条件
        /// </summary>
        public const string PARAM_NAME_OUTPUT_JOUKEN = "出力条件";

        /// <summary>
        /// パラメータ和名：出力対象データ
        /// </summary>
        public const string PARAM_NAME_OUTPUT_DATA = "出力対象データ";

        /// <summary>
        /// パラメータ和名：印字パターン
        /// </summary>
        public const string PARAM_NAME_PRINT_PATTERN = "印字パターン";

        /// <summary>
        /// パラメータ和名：出力年月日
        /// </summary>
        public const string PARAM_NAME_OUTPUT_YMD = "固定出力項目（出力年月日）";

        /// <summary>
        /// パラメータ和名：出力時分
        /// </summary>
        public const string PARAM_NAME_OUTPUT_HM = "固定出力項目（出力時分）";

        /// <summary>
        /// パラメータ和名：作成年月日
        /// </summary>
        public const string PARAM_NAME_SAKUSEI_YMD = "作成年月日";

        /// <summary>
        /// パラメータ和名：タイトル
        /// </summary>
        public const string PARAM_NAME_TITLE = "タイトル";

        /// <summary>
        /// パラメータ和名：固定出力項目（発行年月日）
        /// </summary>
        public const string PARAM_NAME_HAKKOU_YMD = "固定出力項目（発行年月日）";

        /// <summary>
        /// パラメータ和名：固定出力項目（発行時分）
        /// </summary>
        public const string PARAM_NAME_HAKKOU_HM = "固定出力項目（発行時分）";

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
        /// 単一帳票出力
        /// </summary>
        public const int SINGLE_REPORT = 0;

        /// <summary>
        /// 複数帳票出力
        /// </summary>
        public const int MULTI_REPORT = 1;

        /// <summary>
        /// システム設定ファイルにある帳票一時出力フォルダのタグ名
        /// </summary>
        public const string PRINT_TEMP_FOLDER_TAG_NAME = "PrintTempFolder";

        /// <summary>
        /// 帳票拡張子
        /// </summary>
        public const string REPORT_EXTENSION = ".pdf";

        /// <summary>
        /// 一覧CSV拡張子
        /// </summary>
        public const string CSV_LIST_EXTENSION = ".csv";

        /// <summary>
        /// 刷り色・網掛けの背景色
        /// </summary>
        public static Color REPORT_BACKCOLOR_GRAY = Color.FromArgb(178, 178, 178);

        /// <summary>
        /// 帳票ページ番号のフォント書体
        /// </summary>
        public const string REPORT_PAGE_NUM_FONT_TYPE = "ＭＳ ゴシック";

        /// <summary>
        /// 帳票ページ番号のフォントサイズ
        /// </summary>
        public const float REPORT_PAGE_NUM_FONT_SIZE = 10f;

        /// <summary>
        /// 帳票一意キーコードのフォントサイズ
        /// </summary>
        public const float REPORT_PRIMARY_KEY_FONT_SIZE = 10f;

        /// <summary>
        /// 帳票の下部余白_標準：0.787402インチ = 2CM（余白1.5CM + ページ番号エリア0.5CM）
        /// </summary>
        public const float REPORT_BOTTOM_MARGIN_STANDARD = 0.787402f;

        /// <summary>
        /// 帳票の下部余白：0.787402インチ = 1.5CM（余白1CM + ページ番号エリア0.5CM）
        /// </summary>
        public const float REPORT_BOTTOM_MARGIN_FIFTEEN = 0.590551f;

        /// <summary>
        /// 帳票の下部余白：0.511811インチ = 1.3CM（余白0.8CM + ページ番号エリア0.5CM）
        /// </summary>
        public const float REPORT_BOTTOM_MARGIN_THIRTEEN = 0.511811f;

        /// <summary>
        /// 帳票の下部余白：0.393701インチ = 1.0CM（余白0.5CM + ページ番号エリア0.5CM）
        /// </summary>
        public const float REPORT_BOTTOM_MARGIN_TEN = 0.393701f;

        /// <summary>
        /// 帳票の右部余白：0.196850インチ = 0.5CM（印刷業者一意コード描画エリア0.5CM）
        /// </summary>
        public const float REPORT_RIGHT_MARGIN_FIVE = 0.196850f;

        /// <summary>
        /// 帳票の上部余白：0.196850インチ = 0.5CM（印刷業者一意コード描画エリア0.5CM）
        /// </summary>
        public const float REPORT_TOP_MARGIN_FIVE = 0.196850f;

        /// <summary>
        /// テキスト領域の高さ：0.196850インチ = 0.5CM
        /// </summary>
        public const float TEXT_AREA_HEIGHT_FIVE = 0.196850f;

        /// <summary>
        /// メソッドの開始ログ文字列
        /// {0}: 帳票出力 OR 帳票制御 OR 帳票作成
        /// {1}: 条件ID
        /// {2}: 機能名
        /// {3}: パラメータ
        /// </summary>
        public const string METHOD_BEGIN_LOG = "BEGIN {0} {1}。{2}{3}";

        /// <summary>
        /// メソッドの終了ログ文字列
        /// {0}: 帳票出力 OR 帳票制御 OR 帳票作成
        /// {1}: 条件ID
        /// {2}: 機能名 OR 処理時間
        /// </summary>
        public const string METHOD_END_LOG = "END   {0} {1}。{2}";

        /// <summary>
        /// エラーログ文字列
        /// {0}: 条件ID
        /// {1}: エラーメッセージ
        /// </summary>
        public const string ERROR_LOG = "{0}。{1}";

        /// <summary>
        /// 成功ログ文字列
        /// {0}: 条件ID
        /// {1}: メッセージ
        /// </summary>
        public const string SUCCESS_LOG = "{0}。{1}";

        /// <summary>
        /// リアルタイム帳票、又はバッチ帳票プロジェクトのクラスの和名
        /// </summary>
        public const string CLASS_NM_REPORT_OUTPUT = "帳票出力";

        /// <summary>
        /// 制御クラスの和名
        /// </summary>
        public const string CLASS_NM_CONTROLLER = "帳票制御";

        /// <summary>
        /// 帳票作成クラスの和名
        /// </summary>
        public const string CLASS_NM_CREATOR = "帳票作成";

        /// <summary>
        /// システム設定ファイルにあるZIP書庫のMAXサイズのタグ名
        /// </summary>
        public const string ZIP_MAX_SIZE_TAG_NAME = "ZipMaxSize";

        /// <summary>
        /// システム設定ファイルにあるレポートプロジェクトのパース
        /// </summary>
        public const string PATH_BASE_REPORT_MAIN = "PathNskReportMain";

        /// <summary>
        /// システム設定ファイルにある「H001処理分割件数」のタグ名
        /// </summary>
        public const string H001_SPLIT_COUNT = "H001SplitCount";

        /// <summary>
        /// システム設定ファイルにある「H002処理分割件数」のタグ名
        /// </summary>
        public const string H002_SPLIT_COUNT = "H002SplitCount";

        /// <summary>
        /// フラグON
        /// </summary>
        public const string FLG_ON = "1";

        /// <summary>
        /// フラグOFF
        /// </summary>
        public const string FLG_OFF = "0";
    }
}