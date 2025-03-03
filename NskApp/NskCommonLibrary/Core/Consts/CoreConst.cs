using System.ComponentModel;

namespace NskCommonLibrary.Core.Consts
{
    /// <summary>
    /// 定数定義
    /// </summary>
    /// <remarks>
    /// 作成日：2025/1/31
    /// 作成者：ICS
    /// </remarks>
    public class CoreConst
    {
        /// <summary>
        /// バッチプログラムが処理成功：0
        /// </summary>
        public const short BATCH_EXECUT_SUCCESS = 0;

        /// <summary>
        /// バッチプログラムが処理失敗：1
        /// </summary>
        public const short BATCH_EXECUT_FAILED = 1;

        /// <summary>
        /// ステータス：成功
        /// </summary>
        public const string STATUS_SUCCESS = "03";

        /// <summary>
        /// ステータス：エラー
        /// </summary>
        public const string STATUS_ERROR = "99";

        /// <summary>
        /// エラーログ文字列
        /// </summary>
        public const string ERROR_LOG_UPDATE_BATCH_YOYAKU_STS = "バッチ実行状況更新失敗 バッチID：{0}、ステータス：{1}、エラー情報：{2}";

        /// <summary>
        /// ログ文字列
        /// </summary>
        public const string SUCCESS_LOG_UPDATE_BATCH_YOYAKU_STS = "バッチ実行状況更新成功 バッチID：{0}、ステータス：{1}、エラー情報：{2}";

        /// <summary>
        /// appsettings.jsonの「ZIPファイル格納先パス」のキー名
        /// </summary>
        public const string DEFAULT_SCHEMA_SYSTEM_COMMON = "DefaultSchema_SystemCommon";

        /// <summary>
        /// appsettings.jsonの「ZIPファイル格納先パス」のキー名
        /// </summary>
        public const string DEFAULT_SCHEMA_JIGYO_COMMON = "DefaultSchema_JigyoCommon";

        /// <summary>
        /// appsettings.jsonの「ZIPファイル格納先パス」のキー名
        /// </summary>
        public const string CSV_OUTPUT_FOLDER = "CsvOutputFolder";

        /// <summary>
        /// appsettings.jsonの「データ一時出力フォルダパス」のキー名
        /// </summary>
        public const string FILE_TEMP_FOLDER_PATH = "FileTempFolderPath";

        /// <summary>
        /// ファイル拡張子:".txt"
        /// </summary>
        public const string FILE_EXTENSION_TXT = ".txt";

        /// <summary>
        /// 細目データの範囲レコードの抽出区分（全件：１）
        /// </summary>
        public const string TYUSHUTU_KUBUN_ZENKEN = "1";

        /// <summary>
        /// 細目データの範囲レコードの抽出区分（組合員等選択：２）
        /// </summary>
        public const string TYUSHUTU_KUBUN_KUMIAIINTO_SENTAKU = "2";

        /// <summary>
        /// 細目データの範囲レコードの抽出区分（地区選択：３）
        /// </summary>
        public const string TYUSHUTU_KUBUN_CHIKU_SENTAKU = "3";

        /// <summary>
        /// 細目データの範囲レコードの抽出区分（市町村選択：４）
        /// </summary>
        public const string TYUSHUTU_KUBUN_SHITYOSON_SENTAKU = "4";

        /// <summary>
        /// 共済目的コード番号
        /// </summary>
        public enum KyosaiMokutekiCdNumber
        {
            [Description("水稲")]
            Suitou = 11,
            [Description("陸稲")]
            Rikutou = 20,
            [Description("麦")]
            Mugi = 30
        }

        /// <summary>
        /// 負担金交付区分番号
        /// </summary>
        public enum FutankinKofuKbnNumber
        {
            [Description("稲")]
            Ine = 11,
            [Description("麦")]
            Mugi = 30
        }
    }
}