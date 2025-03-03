using CoreLibrary.Core.Utility;
using Microsoft.EntityFrameworkCore;
using ModelLibrary.Models;

namespace ModelLibrary.Context
{
    /// <summary>
    /// 事業共通DBに接続するDBコンテキストクラス
    /// </summary>
    public class JigyoCommonContext : ContextBase
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="commandTimeout">コンテキスト操作のタイムアウト値(秒)
        /// 値が未指定、または、0未満の値場合、設定ファイルからタイムアウト値を取得する</param>
        public JigyoCommonContext(int commandTimeout = 0) : 
            base(ConfigUtil.GetConnectionString("JigyoCommonConnection"), ConfigUtil.Get("DefaultSchema_JigyoCommon"), commandTimeout)
        {
            ;
        }

        /// <summary>
        /// システム設定値マスタ
        /// </summary>
        public DbSet<MCoreConfig> MCoreConfigs { get; set; }

        /// <summary>
        /// 元号マスタ
        /// </summary>
        public DbSet<MGengo> MGengos { get; set; }

        /// <summary>
        /// 汎用区分マスタ
        /// </summary>
        public DbSet<MHanyokubun> MHanyokubuns { get; set; }

        /// <summary>
        /// ヘルプメニューマスタ
        /// </summary>
        public DbSet<MHelpMenu> MHelpMenus { get; set; }

        /// <summary>
        /// ヘルプメッセージマスタ
        /// </summary>
        public DbSet<MHelpMessage> MHelpMessages { get; set; }

        /// <summary>
        /// 名称マスタ
        /// </summary>
        public DbSet<MMeisho> MMeishos { get; set; }

        /// <summary>
        /// メニューマスタ
        /// </summary>
        public DbSet<MMenu> MMenus { get; set; }

        /// <summary>
        /// メッセージマスタ
        /// </summary>
        public DbSet<MMessage> MMessages { get; set; }

        /// <summary>
        /// 農畜品目マスタ　削除予定
        /// </summary>
        public DbSet<MNcHimmoku> MNcHimmokus { get; set; }

        /// <summary>
        /// 農畜種類マスタ　削除予定
        /// </summary>
        public DbSet<MNcShurui> MNcShuruis { get; set; }

        /// <summary>
        /// 農畜用途マスタ
        /// </summary>
        public DbSet<MNcYoto> MNcYotos { get; set; }

        /// <summary>
        /// 年度マスタ
        /// </summary>
        public DbSet<MNendo> MNendos { get; set; }

        /// <summary>
        /// 名称M大地区
        /// </summary>
        public DbSet<MDaichikuNm> MDaichikuNms { get; set; }

        /// <summary>
        /// 組合等マスタ
        /// </summary>
        public DbSet<MKumiaito> MKumiaitos { get; set; }

        /// <summary>
        /// 名称M市町村
        /// </summary>
        public DbSet<MShichosonNm> MShichosonNms { get; set; }

        /// <summary>
        /// 名称M支所
        /// </summary>
        public DbSet<MShishoNm> MShishoNms { get; set; }

        /// <summary>
        /// 名称M小地区
        /// </summary>
        public DbSet<MShochikuNm> MShochikuNms { get; set; }

        /// <summary>
        /// 帳票処理管理マスタ
        /// </summary>
        public DbSet<MReportKanri> MReportKanris { get; set; }

        /// <summary>
        /// 画面マスタ
        /// </summary>
        public DbSet<MScreen> MScreens { get; set; }

        /// <summary>
        /// 支所別農畜品目マスタ　削除予定
        /// </summary>
        public DbSet<MShishoNcHimmoku> MShishoNcHimmokus { get; set; }

        /// <summary>
        /// 支所別農畜種類マスタ　削除予定
        /// </summary>
        public DbSet<MShishoNcShurui> MShishoNcShuruis { get; set; }

        /// <summary>
        /// 支所別農畜用途マスタ　削除予定
        /// </summary>
        public DbSet<MShishoNcYoto> MShishoNcYotos { get; set; }

        /// <summary>
        /// 都道府県マスタ
        /// </summary>
        public DbSet<MTodofuken> MTodofukens { get; set; }

        /// <summary>
        /// 入力方法PDFマスタ
        /// </summary>
        public DbSet<MNyuryokuhohoPdf> MNyuryokuhohoPdfs { get; set; }

        /// <summary>
        /// ログイン時遷移先マスタ
        /// </summary>
        public DbSet<MLoginTransition> MLoginTransitions { get; set; }
    }
}
