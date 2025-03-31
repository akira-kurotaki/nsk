using NskAppModelLibrary.FimModels;
using Microsoft.EntityFrameworkCore;
using ModelLibrary.Context;

namespace NskAppModelLibrary.Context
{
    /// <summary>
    /// FimView向けDBコンテキスト
    /// </summary>
    public class FimContext : JigyoContext
    {
        public FimContext(string connectionString, string defaultSchema, int commandTimeout = 0) : 
            base(connectionString, defaultSchema, commandTimeout)
        {
        }

        public FimContext(string connectionString, string defaultSchema, string userId, string ipAddress, int commandTimeout = 0) : 
            base(connectionString, defaultSchema, userId, ipAddress, commandTimeout)
        {
        }

        /// <summary>
        /// 名称M徴収区分
        /// </summary>
        public DbSet<VChoshuKbn> VChoshuKbns { get; set; }
        
        /// <summary>
        /// 名称M徴収理由
        /// </summary>
        public DbSet<VChoshuRiyu> VChoshuRiyus { get; set; }
        
        /// <summary>
        /// コード設定マスタ
        /// </summary>
        public DbSet<VCodeConfig> VCodeConfigs { get; set; }
        
        /// <summary>
        /// 名称M大地区
        /// </summary>
        //public DbSet<VDaichikuNm> VDaichikuNms { get; set; }
        
        /// <summary>
        /// 外部金融機関マスタ
        /// </summary>
        public DbSet<VGaibuKinyukikan> VGaibuKinyukikans { get; set; }
        
        /// <summary>
        /// 汎用区分マスタ
        /// </summary>
        //public DbSet<VHanyokubun> VHanyokubuns { get; set; }
        
        /// <summary>
        /// ヘルプメッセージマスタ
        /// </summary>
        public DbSet<VHelpMessage> VHelpMessages { get; set; }
        
        /// <summary>
        /// 口座振替委託者マスタ
        /// </summary>
        public DbSet<VKozaFurikaeItakusha> VKozaFurikaeItakushas { get; set; }
        
        /// <summary>
        /// 組合員等割賦課金単価マスタ
        /// </summary>
        public DbSet<VKumiaiintoFukakinCost> VKumiaiintoFukakinCosts { get; set; }
        
        /// <summary>
        /// 組合等マスタ
        /// </summary>
        //public DbSet<VKumiaito> VKumiaitos { get; set; }
        
        /// <summary>
        /// 組合等金融機関マスタ
        /// </summary>
        public DbSet<VKumiaitoKinyukikan> VKumiaitoKinyukikans { get; set; }
        
        /// <summary>
        /// 名称M共済事業
        /// </summary>
        public DbSet<VKyosaiJigyo> VKyosaiJigyos { get; set; }
        
        /// <summary>
        /// 名称M共済目的
        /// </summary>
        public DbSet<VKyosaiMokutekito> VKyosaiMokutekitos { get; set; }
        
        /// <summary>
        /// メッセージマスタ
        /// </summary>
        public DbSet<VMessage> VMessages { get; set; }
        
        /// <summary>
        /// 画面機能権限マスタ
        /// </summary>
        //public DbSet<VScreenKinoKengen> VScreenKinoKengens { get; set; }
        
        /// <summary>
        /// 画面操作権限マスタ
        /// </summary>
        //public DbSet<VScreenSosaKengen> VScreenSosaKengens { get; set; }
        
        /// <summary>
        /// 世帯管理マスタ
        /// </summary>
        public DbSet<VSetaikanri> VSetaikanris { get; set; }
        
        /// <summary>
        /// 名称M市町村
        /// </summary>
        //public DbSet<VShichosonNm> VShichosonNms { get; set; }
        
        /// <summary>
        /// 資源用共済事業マスタ
        /// </summary>
        public DbSet<VShigenKyosaiJigyo> VShigenKyosaiJigyos { get; set; }
        
        /// <summary>
        /// 資源用共済目的等マスタ
        /// </summary>
        public DbSet<VShigenKyosaiMokutekito> VShigenKyosaiMokutekitos { get; set; }
        
        /// <summary>
        /// 支所グループマスタ
        /// </summary>
        //public DbSet<VShishoGroup> VShishoGroups { get; set; }
        
        /// <summary>
        /// 支所グループ詳細マスタ
        /// </summary>
        //public DbSet<VShishoGroupShosai> VShishoGroupShosais { get; set; }
        
        /// <summary>
        /// 名称M支所
        /// </summary>
        //public DbSet<VShishoNm> VShishoNms { get; set; }
        
        /// <summary>
        /// 名称M小地区
        /// </summary>
        //public DbSet<VShochikuNm> VShochikuNms { get; set; }
        
        /// <summary>
        /// 職員マスタ
        /// </summary>
        //public DbSet<VSyokuin> VSyokuins { get; set; }
        
        /// <summary>
        /// 転送ファイルマスタ
        /// </summary>
        public DbSet<VTensoFile> VTensoFiles { get; set; }
        
        /// <summary>
        /// 転送区分マスタ
        /// </summary>
        public DbSet<VTensoFileKbn> VTensoFileKbns { get; set; }
        
        /// <summary>
        /// 都道府県マスタ
        /// </summary>
        //public DbSet<VTodofuken> VTodofukens { get; set; }
        
        /// <summary>
        /// 問合せ事業細目マスタ
        /// </summary>
        public DbSet<VToiawaseJigyoSaimoku> VToiawaseJigyoSaimokus { get; set; }
        
        /// <summary>
        /// 加入状況（農作物共済）
        /// </summary>
        public DbSet<VKanyuJokyoNosakumotsu> VKanyuJokyoNosakumotsus { get; set; }
        
        /// <summary>
        /// 加入状況（収入保険）
        /// </summary>
        public DbSet<VKanyuJokyoShunyuhoken> VKanyuJokyoShunyuhokens { get; set; }
        
        /// <summary>
        /// 加入状況（収入保険）取得管理
        /// </summary>
        public DbSet<VKanyuJokyoShunyuhokenShutokuKanri> VKanyuJokyoShunyuhokenShutokuKanris { get; set; }
        
        /// <summary>
        /// 加入状況（建物共済）
        /// </summary>
        public DbSet<VKanyuJokyoTatemono> VKanyuJokyoTatemonos { get; set; }
        
        /// <summary>
        /// 組合等印影
        /// </summary>
        public DbSet<VKumiaitoInei> VKumiaitoIneis { get; set; }
        
        /// <summary>
        /// 共済加入資源_見込情報
        /// </summary>
        public DbSet<VKyosaiKanyuMikomi> VKyosaiKanyuMikomis { get; set; }
        
        /// <summary>
        /// 農業者情報
        /// </summary>
        //public DbSet<VNogyosha> VNogyoshas { get; set; }
        
        /// <summary>
        /// 農業者備考
        /// </summary>
        public DbSet<VNogyoshaBiko> VNogyoshaBikos { get; set; }
        
        /// <summary>
        /// 農業者管理コード等変換管理
        /// </summary>
        public DbSet<VNogyoshaCdHenkanKanri> VNogyoshaCdHenkanKanris { get; set; }
        
        /// <summary>
        /// 農業者関連取込履歴
        /// </summary>
        public DbSet<VNogyoshaKanrenTorikomiRireki> VNogyoshaKanrenTorikomiRirekis { get; set; }
        
        /// <summary>
        /// 農業者金融機関
        /// </summary>
        public DbSet<VNogyoshaKinyukikan> VNogyoshaKinyukikans { get; set; }
        
        /// <summary>
        /// 農業者共通申請利用管理
        /// </summary>
        public DbSet<VNogyoshaKyotsuKanri> VNogyoshaKyotsuKanris { get; set; }
        
        /// <summary>
        /// QA面談記録
        /// </summary>
        public DbSet<VQaMendanKiroku> VQaMendanKirokus { get; set; }
        
        /// <summary>
        /// 掛金等徴収共済金支払状況（農作物共済）
        /// </summary>
        public DbSet<VShiharaiJokyoNosakumotsu> VShiharaiJokyoNosakumotsus { get; set; }
        
        /// <summary>
        /// 掛金等徴収共済金支払状況（収入保険）
        /// </summary>
        public DbSet<VShiharaiJokyoShunyuhoken> VShiharaiJokyoShunyuhokens { get; set; }
        
        /// <summary>
        /// 掛金等徴収共済金支払状況（建物共済）
        /// </summary>
        public DbSet<VShiharaiJokyoTatemono> VShiharaiJokyoTatemonos { get; set; }
        
        /// <summary>
        /// 収入保険資源_見込情報
        /// </summary>
        public DbSet<VShunyuhokenMikomi> VShunyuhokenMikomis { get; set; }
        
        /// <summary>
        /// 転送管理
        /// </summary>
        public DbSet<VTensoKanri> VTensoKanris { get; set; }

    }
}
