using CoreLibrary.Core.Utility;
using Microsoft.EntityFrameworkCore;
using ModelLibrary.Models;

namespace ModelLibrary.Context
{
    /// <summary>
    /// システム共通DBに接続するDBコンテキストクラス
    /// </summary>
    public class SystemCommonContext : ContextBase
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="commandTimeout">コンテキスト操作のタイムアウト値(秒)
        /// 値が未指定、または、0未満の値場合、設定ファイルからタイムアウト値を取得する</param>
        public SystemCommonContext(int commandTimeout = 0) : 
            base(ConfigUtil.GetConnectionString("SystemCommonConnection"), ConfigUtil.Get("DefaultSchema_SystemCommon"), commandTimeout)
        {
            ;
        }

        /// <summary>
        /// システム接続情報マスタ
        /// </summary>
        public DbSet<MSystemConnection> MSystemConnections { get; set; }

        /// <summary>
        /// メッセージマスタ
        /// </summary>
        public DbSet<MMessage> MMessages { get; set; }

        /// <summary>
        /// バッチ予約
        /// </summary>
        public DbSet<TBatchYoyaku> TBatchYoyakus { get; set; }

        /// <summary>
        /// 都道府県マスタ
        /// </summary>
        public DbSet<MTodofuken> MTodofukens { get; set; }

        /// <summary>
        /// 組合等マスタ
        /// </summary>
        public DbSet<MKumiaito> MKumiaitos { get; set; }

        /// <summary>
        /// 名称M支所
        /// </summary>
        public DbSet<MShishoNm> MShishoNms { get; set; }

        /// <summary>
        /// APIパスフレーズマスタ
        /// </summary>
        public DbSet<MApiPassphrase> MApiPassphrases { get; set; }

        /// <summary>
        /// バッチダウンロードファイル
        /// </summary>
        public DbSet<TBatchDownloadFile> TBatchDownloadFiles { get; set; }

        /// <summary>
        /// 汎用区分マスタ
        /// </summary>
        public DbSet<MHanyokubun> MHanyokubuns { get; set; }

        /// <summary>
        /// 名称M共済事業
        /// </summary>
        public DbSet<MKyosaiJigyo> MKyosaiJigyos { get; set; }

        /// <summary>
        /// 予約処理マスタ
        /// </summary>
        public DbSet<MYoyaku> MYoyakus { get; set; }

        /// <summary>
        /// バッチ実行上限数マスタ
        /// </summary>
        public DbSet<MBatchRunMax> MBatchRunMaxs { get; set; }

        /// <summary>
        /// バッチ実行可能時間帯
        /// </summary>
        public DbSet<MBatchRunTime> MBatchRunTimes { get; set; }

        /// <summary>
        /// 都道府県別バッチ実行上限数マスタ
        /// </summary>
        public DbSet<MTodofukenBatchRunMax> MTodofukenBatchRunMaxs { get; set; }

        /// <summary>
        /// 即時CSV出力条件件数マスタ
        /// </summary>
        public DbSet<MImmedCsvMaxCount> MImmedCsvMaxCounts { get; set; }

        /// <summary>
        /// バッチCSV取得条件
        /// </summary>
        public DbSet<TBatchCsvQuery> TBatchCsvQuerys { get; set; }

        /// <summary>
        /// 定時バッチ起動管理
        /// </summary>
        public DbSet<TScheBatchRunManage> TScheBatchRunManages { get; set; }

        /// <summary>
        /// システムロック
        /// </summary>
        public DbSet<TSysLock> TSysLocks { get; set; }

        /// <summary>
        /// データロック
        /// </summary>
        public DbSet<TDataLock> TDataLocks { get; set; }

        /// <summary>
        /// バックアップ対象情報
        /// </summary>
        public DbSet<MBackupInfo> MBackupInfos { get; set; }

        /// <summary>
        /// 一時ダンプファイル取得
        /// </summary>
        public DbSet<WDumpFileDownload> WDumpFileDownloads { get; set; }
    }
}
