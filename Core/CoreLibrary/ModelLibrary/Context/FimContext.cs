using CoreLibrary.Core.Utility;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using ModelLibrary.Models;

namespace ModelLibrary.Context
{
    /// <summary>
    /// 農業者情報管理スキーマに接続するDBコンテキスト
    /// </summary>
    public class FimContext : ContextBase
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="connectionString">接続文字列</param>
        /// <param name="defaultSchema">デフォルトスキーマ</param>
        /// <param name="commandTimeout">コンテキスト操作のタイムアウト値(秒)
        /// 値が未指定、または、0未満の値場合、設定ファイルからタイムアウト値を取得する</param>
        public FimContext(int commandTimeout = 0) :
            base(ConfigUtil.GetConnectionString("FimConnection"), ConfigUtil.Get("DefaultSchema_Fim"), commandTimeout)
        {
            ;
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="connectionString">接続文字列</param>
        /// <param name="defaultSchema">デフォルトスキーマ</param>
        /// <param name="commandTimeout">コンテキスト操作のタイムアウト値(秒)
        /// 値が未指定、または、0未満の値場合、設定ファイルからタイムアウト値を取得する</param>
        public FimContext(string connectionString, string defaultSchema, int commandTimeout = 0) :
            base(connectionString, defaultSchema, commandTimeout)
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

        public DbSet<MSyokuin> MSyokuins { get; set; }
        public DbSet<MScreenKinoKengen> MScreenKinoKengens { get; set; }
        public DbSet<MScreenSosaKengen> MScreenSosaKengens { get; set; }

        public DbSet<MShishoGroup> MShishoGroups { get; set; }
        public DbSet<MShishoGroupShosai> MShishoGroupShosais { get; set; }
        public DbSet<MShishoNm> MShishoNms { get; set; }


        public DbSet<TNogyosha> TNogyoshas { get; set; }   }
    }
