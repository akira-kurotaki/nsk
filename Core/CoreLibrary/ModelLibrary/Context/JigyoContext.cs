using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using ModelLibrary.Models;

namespace ModelLibrary.Context
{
    /// <summary>
    /// 事業システムコンテキストクラスベース
    /// 
    /// 作成日：
    /// 作成者：
    /// </summary>
    public class JigyoContext : ContextBase
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="connectionString">接続文字列</param>
        /// <param name="defaultSchema">デフォルトスキーマ</param>
        /// <param name="commandTimeout">コンテキスト操作のタイムアウト値(秒)
        /// 値が未指定、または、0未満の値場合、設定ファイルからタイムアウト値を取得する</param>
        public JigyoContext(string connectionString, string defaultSchema, int commandTimeout = 0) : 
            base(connectionString, defaultSchema, commandTimeout)
        {
            ;
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="connectionString">接続文字列</param>
        /// <param name="defaultSchema">デフォルトスキーマ</param>
        /// <param name="userId">ユーザID</param>
        /// <param name="ipAddress">IPアドレス</param>
        /// <param name="commandTimeout">コンテキスト操作のタイムアウト値(秒)
        /// 値が未指定、または、0未満の値場合、設定ファイルからタイムアウト値を取得する</param>
        public JigyoContext(string connectionString, string defaultSchema, string userId, string ipAddress, int commandTimeout = 0) :
            base(connectionString, defaultSchema, userId, ipAddress, commandTimeout)
        {
            ;
        }

        #region OnConfiguring
        /// <summary>
        /// ログ出力設定、接続先設定
        /// </summary>
        /// <param name="optionsBuilder"></param>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.ReplaceService<IModelCacheKeyFactory, DynamicModelCacheKeyFactory>();
        }
        #endregion

        /// <summary>
        /// 職員マスタ
        /// </summary>
        public DbSet<VSyokuin> VSyokuins { get; set; }

        /// <summary>
        /// 農業者情報
        /// </summary>
        public DbSet<VNogyosha> VNogyoshas { get; set; }

        /// <summary>
        /// 都道府県マスタ
        /// </summary>
        public DbSet<VTodofuken> VTodofukens { get; set; }

        /// <summary>
        /// 組合等マスタ
        /// </summary>
        public DbSet<VKumiaito> VKumiaitos { get; set; }

        /// <summary>
        /// 名称M支所
        /// </summary>
        public DbSet<VShishoNm> VShishoNms { get; set; }

        /// <summary>
        /// 名称M大地区
        /// </summary>
        public DbSet<VDaichikuNm> VDaichikuNms { get; set; }

        /// <summary>
        /// 名称M小地区
        /// </summary>
        public DbSet<VShochikuNm> VShochikuNms { get; set; }

        /// <summary>
        /// 名称M市町村
        /// </summary>
        public DbSet<VShichosonNm> VShichosonNms { get; set; }

        /// <summary>
        /// 画面機能権限マスタ
        /// </summary>
        public DbSet<VScreenKinoKengen> VScreenKinoKengens { get; set; }

        /// <summary>
        /// 画面操作権限マスタ
        /// </summary>
        public DbSet<VScreenSosaKengen> VScreenSosaKengens { get; set; }

        /// <summary>
        /// 支所グループマスタ
        /// </summary>
        public DbSet<VShishoGroup> VShishoGroups { get; set; }

        /// <summary>
        /// 支所グループ詳細マスタ
        /// </summary>
        public DbSet<VShishoGroupShosai> VShishoGroupShosais { get; set; }

        /// <summary>
        /// 汎用区分マスタ
        /// </summary>
        public DbSet<VHanyokubun> VHanyokubuns { get; set; }

        /// <summary>
        /// 帳票条件
        /// </summary>
        public DbSet<TReportJouken> TReportJoukens { get; set; }
    }
}