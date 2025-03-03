using NskAppModelLibrary.Models;
using Microsoft.EntityFrameworkCore;
using ModelLibrary.Context;

namespace NskAppModelLibrary.Context
{
    /// <summary>
    /// ベースアプリケーション向けDBコンテキスト
    /// </summary>
    public class NskAppContext : JigyoContext
    {
        public NskAppContext(string connectionString, string defaultSchema, int commandTimeout = 0) : 
            base(connectionString, defaultSchema, commandTimeout)
        {
        }

        public NskAppContext(string connectionString, string defaultSchema, string userId, string ipAddress, int commandTimeout = 0) : 
            base(connectionString, defaultSchema, userId, ipAddress, commandTimeout)
        {
        }

        /// <summary>
        /// 加入者情報
        /// </summary>
        public DbSet<TKanyusha> TKanyushas { get; set; }

        /// <summary>
        /// 一時連携加入者情報
        /// </summary>
        public DbSet<WRenkeiKanyusha> WRenkeiKanyushas { get; set; }

        /// <summary>
        /// 取込管理
        /// </summary>
        public DbSet<TTorikomiManage> TTorikomiManages { get; set; }

        /// <summary>
        /// 取込対象マスタ
        /// </summary>
        public DbSet<MTorikomi> MTorikomis { get; set; }

        /// <summary>
        /// 操作履歴
        /// </summary>
        public DbSet<TSosaRireki> TSosaRirekis { get; set; }

        #region NSKテーブル

        /// <summary>
        /// m_00010_共済目的名称
        /// </summary>
        public DbSet<M00010共済目的名称> M00010共済目的名称s { get; set; }
        
        /// <summary>
        /// m_00020_類名称
        /// </summary>
        public DbSet<M00020類名称> M00020類名称s { get; set; }
        
        /// <summary>
        /// m_00030_区分名称
        /// </summary>
        public DbSet<M00030区分名称> M00030区分名称s { get; set; }
        
        /// <summary>
        /// m_00040_田畑名称
        /// </summary>
        public DbSet<M00040田畑名称> M00040田畑名称s { get; set; }
        
        /// <summary>
        /// m_00050_端数処理名称
        /// </summary>
        public DbSet<M00050端数処理名称> M00050端数処理名称s { get; set; }
        
        /// <summary>
        /// m_00060_全相殺計算方法名称
        /// </summary>
        public DbSet<M00060全相殺計算方法名称> M00060全相殺計算方法名称s { get; set; }
        
        /// <summary>
        /// m_00070_収穫量確認方法名称
        /// </summary>
        public DbSet<M00070収穫量確認方法名称> M00070収穫量確認方法名称s { get; set; }
        
        /// <summary>
        /// m_00090_営農継続単価
        /// </summary>
        public DbSet<M00090営農継続単価> M00090営農継続単価s { get; set; }
        
        /// <summary>
        /// m_00100_修正限界
        /// </summary>
        public DbSet<M00100修正限界> M00100修正限界s { get; set; }
        
        /// <summary>
        /// m_00110_品種係数
        /// </summary>
        public DbSet<M00110品種係数> M00110品種係数s { get; set; }
        
        /// <summary>
        /// m_00120_料率
        /// </summary>
        public DbSet<M00120料率> M00120料率s { get; set; }
        
        /// <summary>
        /// m_00130_産地別銘柄名称設定
        /// </summary>
        public DbSet<M00130産地別銘柄名称設定> M00130産地別銘柄名称設定s { get; set; }
        
        /// <summary>
        /// m_00140_名称
        /// </summary>
        public DbSet<M00140名称> M00140名称s { get; set; }
        
        /// <summary>
        /// m_00150_帳票管理
        /// </summary>
        public DbSet<M00150帳票管理> M00150帳票管理s { get; set; }
        
        /// <summary>
        /// m_00170_統計単位地域
        /// </summary>
        public DbSet<M00170統計単位地域> M00170統計単位地域s { get; set; }
        
        /// <summary>
        /// m_00180_担手農家区分名称
        /// </summary>
        public DbSet<M00180担手農家区分名称> M00180担手農家区分名称s { get; set; }
        
        /// <summary>
        /// m_00190_大量データ対象データ
        /// </summary>
        public DbSet<M00190大量データ対象データ> M00190大量データ対象データs { get; set; }
        
        /// <summary>
        /// t_00010_引受回
        /// </summary>
        public DbSet<T00010引受回> T00010引受回s { get; set; }
        
        /// <summary>
        /// t_00020_引受確定
        /// </summary>
        public DbSet<T00020引受確定> T00020引受確定s { get; set; }
        
        /// <summary>
        /// t_00030_当初評価確定
        /// </summary>
        public DbSet<T00030当初評価確定> T00030当初評価確定s { get; set; }
        
        /// <summary>
        /// t_00040_報告回
        /// </summary>
        public DbSet<T00040報告回> T00040報告回s { get; set; }
        
        /// <summary>
        /// t_00050_交付回
        /// </summary>
        public DbSet<T00050交付回> T00050交付回s { get; set; }
        
        /// <summary>
        /// t_00060_請求回
        /// </summary>
        public DbSet<T00060請求回> T00060請求回s { get; set; }
        
        /// <summary>
        /// t_01010_ポータル表示情報_引受情報
        /// </summary>
        public DbSet<T01010ポータル表示情報引受情報> T01010ポータル表示情報引受情報s { get; set; }
        
        /// <summary>
        /// t_01020_ポータル表示情報_被害情報
        /// </summary>
        public DbSet<T01020ポータル表示情報被害情報> T01020ポータル表示情報被害情報s { get; set; }
        
        /// <summary>
        /// t_01050_バッチ条件
        /// </summary>
        public DbSet<T01050バッチ条件> T01050バッチ条件s { get; set; }
        
        /// <summary>
        /// t_01060_大量データ受入履歴
        /// </summary>
        public DbSet<T01060大量データ受入履歴> T01060大量データ受入履歴s { get; set; }
        
        /// <summary>
        /// t_01070_大量データ取込履歴
        /// </summary>
        public DbSet<T01070大量データ取込履歴> T01070大量データ取込履歴s { get; set; }
        
        /// <summary>
        /// t_01080_大量データ受入_エラーリスト
        /// </summary>
        public DbSet<T01080大量データ受入エラーリスト> T01080大量データ受入エラーリストs { get; set; }
        
        /// <summary>
        /// m_10010_組合等設定
        /// </summary>
        public DbSet<M10010組合等設定> M10010組合等設定s { get; set; }
        
        /// <summary>
        /// m_10020_分割単当減収量入力設定
        /// </summary>
        public DbSet<M10020分割単当減収量入力設定> M10020分割単当減収量入力設定s { get; set; }
        
        /// <summary>
        /// m_10030_危険段階共済掛金区分
        /// </summary>
        public DbSet<M10030危険段階共済掛金区分> M10030危険段階共済掛金区分s { get; set; }
        
        /// <summary>
        /// m_10040_参酌係数
        /// </summary>
        public DbSet<M10040参酌係数> M10040参酌係数s { get; set; }
        
        /// <summary>
        /// m_10050_基準単収
        /// </summary>
        public DbSet<M10050基準単収> M10050基準単収s { get; set; }
        
        /// <summary>
        /// m_10060_収量等級
        /// </summary>
        public DbSet<M10060収量等級> M10060収量等級s { get; set; }
        
        /// <summary>
        /// m_10070_統計単収引受
        /// </summary>
        public DbSet<M10070統計単収引受> M10070統計単収引受s { get; set; }
        
        /// <summary>
        /// m_10080_引受方式名称
        /// </summary>
        public DbSet<M10080引受方式名称> M10080引受方式名称s { get; set; }
        
        /// <summary>
        /// m_10090_引受区分名称
        /// </summary>
        public DbSet<M10090引受区分名称> M10090引受区分名称s { get; set; }
        
        /// <summary>
        /// m_10100_特約区分名称
        /// </summary>
        public DbSet<M10100特約区分名称> M10100特約区分名称s { get; set; }
        
        /// <summary>
        /// m_10110_用途区分名称
        /// </summary>
        public DbSet<M10110用途区分名称> M10110用途区分名称s { get; set; }
        
        /// <summary>
        /// m_10120_用途区分選択
        /// </summary>
        public DbSet<M10120用途区分選択> M10120用途区分選択s { get; set; }
        
        /// <summary>
        /// m_10130_種類区分名称
        /// </summary>
        public DbSet<M10130種類区分名称> M10130種類区分名称s { get; set; }
        
        /// <summary>
        /// m_10140_種類名称
        /// </summary>
        public DbSet<M10140種類名称> M10140種類名称s { get; set; }
        
        /// <summary>
        /// m_10150_解除理由名称
        /// </summary>
        public DbSet<M10150解除理由名称> M10150解除理由名称s { get; set; }
        
        /// <summary>
        /// m_10160_賦課金引受方式名称
        /// </summary>
        public DbSet<M10160賦課金引受方式名称> M10160賦課金引受方式名称s { get; set; }
        
        /// <summary>
        /// m_10170_選択引受方式
        /// </summary>
        public DbSet<M10170選択引受方式> M10170選択引受方式s { get; set; }
        
        /// <summary>
        /// m_10180_単位当たり共済金額設定
        /// </summary>
        public DbSet<M10180単位当たり共済金額設定> M10180単位当たり共済金額設定s { get; set; }
        
        /// <summary>
        /// m_10190_契約価格等設定
        /// </summary>
        public DbSet<M10190契約価格等設定> M10190契約価格等設定s { get; set; }
        
        /// <summary>
        /// m_10195_契約価格等設定_規格別
        /// </summary>
        public DbSet<M10195契約価格等設定規格別> M10195契約価格等設定規格別s { get; set; }
        
        /// <summary>
        /// m_10200_組合等単当共済金額
        /// </summary>
        public DbSet<M10200組合等単当共済金額> M10200組合等単当共済金額s { get; set; }
        
        /// <summary>
        /// m_10210_単当共済金額用途
        /// </summary>
        public DbSet<M10210単当共済金額用途> M10210単当共済金額用途s { get; set; }
        
        /// <summary>
        /// m_10220_地区別設定
        /// </summary>
        public DbSet<M10220地区別設定> M10220地区別設定s { get; set; }
        
        /// <summary>
        /// m_10230_危険段階
        /// </summary>
        public DbSet<M10230危険段階> M10230危険段階s { get; set; }
        
        /// <summary>
        /// m_10240_危険段階地域別設定
        /// </summary>
        public DbSet<M10240危険段階地域別設定> M10240危険段階地域別設定s { get; set; }
        
        /// <summary>
        /// m_10250_賦課金_組合設定
        /// </summary>
        public DbSet<M10250賦課金組合設定> M10250賦課金組合設定s { get; set; }
        
        /// <summary>
        /// m_10260_賦課金_ランク共通
        /// </summary>
        public DbSet<M10260賦課金ランク共通> M10260賦課金ランク共通s { get; set; }
        
        /// <summary>
        /// m_10270_賦課金詳細
        /// </summary>
        public DbSet<M10270賦課金詳細> M10270賦課金詳細s { get; set; }
        
        /// <summary>
        /// m_10280_用途課税名称
        /// </summary>
        public DbSet<M10280用途課税名称> M10280用途課税名称s { get; set; }
        
        /// <summary>
        /// t_10010_賦課金エラー情報
        /// </summary>
        public DbSet<T10010賦課金エラー情報> T10010賦課金エラー情報s { get; set; }
        
        /// <summary>
        /// m_20010_被害判定名称
        /// </summary>
        public DbSet<M20010被害判定名称> M20010被害判定名称s { get; set; }
        
        /// <summary>
        /// m_20020_被害区分名称
        /// </summary>
        public DbSet<M20020被害区分名称> M20020被害区分名称s { get; set; }
        
        /// <summary>
        /// m_20030_補償割合名称
        /// </summary>
        public DbSet<M20030補償割合名称> M20030補償割合名称s { get; set; }
        
        /// <summary>
        /// m_20040_平均単収差計算名称
        /// </summary>
        public DbSet<M20040平均単収差計算名称> M20040平均単収差計算名称s { get; set; }
        
        /// <summary>
        /// m_20050_支払対象区分
        /// </summary>
        public DbSet<M20050支払対象区分> M20050支払対象区分s { get; set; }
        
        /// <summary>
        /// m_20060_計算割合
        /// </summary>
        public DbSet<M20060計算割合> M20060計算割合s { get; set; }
        
        /// <summary>
        /// m_20070_計算単位換算表
        /// </summary>
        public DbSet<M20070計算単位換算表> M20070計算単位換算表s { get; set; }
        
        /// <summary>
        /// m_20080_計算単位換算表類
        /// </summary>
        public DbSet<M20080計算単位換算表類> M20080計算単位換算表類s { get; set; }
        
        /// <summary>
        /// m_20090_共済事故種類
        /// </summary>
        public DbSet<M20090共済事故種類> M20090共済事故種類s { get; set; }
        
        /// <summary>
        /// m_20100_災害種類
        /// </summary>
        public DbSet<M20100災害種類> M20100災害種類s { get; set; }
        
        /// <summary>
        /// m_20110_受託者等
        /// </summary>
        public DbSet<M20110受託者等> M20110受託者等s { get; set; }
        
        /// <summary>
        /// m_20120_階層区分
        /// </summary>
        public DbSet<M20120階層区分> M20120階層区分s { get; set; }
        
        /// <summary>
        /// m_20130_評価地区
        /// </summary>
        public DbSet<M20130評価地区> M20130評価地区s { get; set; }
        
        /// <summary>
        /// m_20140_抜取調査班
        /// </summary>
        public DbSet<M20140抜取調査班> M20140抜取調査班s { get; set; }
        
        /// <summary>
        /// m_20150_抜取班連携
        /// </summary>
        public DbSet<M20150抜取班連携> M20150抜取班連携s { get; set; }
        
        /// <summary>
        /// m_20160_政府保険認定区分
        /// </summary>
        public DbSet<M20160政府保険認定区分> M20160政府保険認定区分s { get; set; }
        
        /// <summary>
        /// m_20170_政府保険認定区分初期値
        /// </summary>
        public DbSet<M20170政府保険認定区分初期値> M20170政府保険認定区分初期値s { get; set; }
        
        /// <summary>
        /// m_20180_出荷評価地区名称
        /// </summary>
        public DbSet<M20180出荷評価地区名称> M20180出荷評価地区名称s { get; set; }
        
        /// <summary>
        /// m_20190_出荷階層区分名称
        /// </summary>
        public DbSet<M20190出荷階層区分名称> M20190出荷階層区分名称s { get; set; }
        
        /// <summary>
        /// m_20200_出荷評価地区コード設定
        /// </summary>
        public DbSet<M20200出荷評価地区コード設定> M20200出荷評価地区コード設定s { get; set; }
        
        /// <summary>
        /// m_20210_引受方式別判定
        /// </summary>
        public DbSet<M20210引受方式別判定> M20210引受方式別判定s { get; set; }
        
        /// <summary>
        /// m_20220_分割耕地判定名称
        /// </summary>
        public DbSet<M20220分割耕地判定名称> M20220分割耕地判定名称s { get; set; }
        
        /// <summary>
        /// m_00200_換算係数計算方法名称
        /// </summary>
        public DbSet<M00200換算係数計算方法名称> M00200換算係数計算方法名称s { get; set; }
        
        /// <summary>
        /// m_00210_様式文言
        /// </summary>
        public DbSet<M00210様式文言> M00210様式文言s { get; set; }
        
        /// <summary>
        /// m_00220_共済目的対応
        /// </summary>
        public DbSet<M00220共済目的対応> M00220共済目的対応s { get; set; }
        
        /// <summary>
        /// m_00230_営農対象名称
        /// </summary>
        public DbSet<M00230営農対象名称> M00230営農対象名称s { get; set; }
        
        /// <summary>
        /// m_00240_メニュー
        /// </summary>
        public DbSet<M00240メニュー> M00240メニューs { get; set; }
        
        /// <summary>
        /// m_20230_一筆全半損判定名称
        /// </summary>
        public DbSet<M20230一筆全半損判定名称> M20230一筆全半損判定名称s { get; set; }
        
        /// <summary>
        /// t_50010_農作物実績マスタ
        /// </summary>
        public DbSet<T50010農作物実績マスタ> T50010農作物実績マスタs { get; set; }
        
        /// <summary>
        /// t_11010_個人設定
        /// </summary>
        public DbSet<T11010個人設定> T11010個人設定s { get; set; }
        
        /// <summary>
        /// t_11020_個人設定解除
        /// </summary>
        public DbSet<T11020個人設定解除> T11020個人設定解除s { get; set; }
        
        /// <summary>
        /// t_11030_個人設定類
        /// </summary>
        public DbSet<T11030個人設定類> T11030個人設定類s { get; set; }
        
        /// <summary>
        /// t_11040_個人料率
        /// </summary>
        public DbSet<T11040個人料率> T11040個人料率s { get; set; }
        
        /// <summary>
        /// t_11050_個人用途別
        /// </summary>
        public DbSet<T11050個人用途別> T11050個人用途別s { get; set; }
        
        /// <summary>
        /// t_11060_基準収穫量設定
        /// </summary>
        public DbSet<T11060基準収穫量設定> T11060基準収穫量設定s { get; set; }
        
        /// <summary>
        /// t_11070_基準収穫量設定_規格別
        /// </summary>
        public DbSet<T11070基準収穫量設定規格別> T11070基準収穫量設定規格別s { get; set; }
        
        /// <summary>
        /// t_11090_引受耕地
        /// </summary>
        public DbSet<T11090引受耕地> T11090引受耕地s { get; set; }
        
        /// <summary>
        /// t_11100_引受gis
        /// </summary>
        public DbSet<T11100引受gis> T11100引受giss { get; set; }
        
        /// <summary>
        /// t_12010_引受結果
        /// </summary>
        public DbSet<T12010引受結果> T12010引受結果s { get; set; }
        
        /// <summary>
        /// t_12020_引受チェック
        /// </summary>
        public DbSet<T12020引受チェック> T12020引受チェックs { get; set; }
        
        /// <summary>
        /// t_12030_用途チェック
        /// </summary>
        public DbSet<T12030用途チェック> T12030用途チェックs { get; set; }
        
        /// <summary>
        /// t_12040_組合員等別引受情報
        /// </summary>
        public DbSet<T12040組合員等別引受情報> T12040組合員等別引受情報s { get; set; }
        
        /// <summary>
        /// t_12050_組合員等別引受用途
        /// </summary>
        public DbSet<T12050組合員等別引受用途> T12050組合員等別引受用途s { get; set; }
        
        /// <summary>
        /// t_12060_産地別銘柄別引受情報
        /// </summary>
        public DbSet<T12060産地別銘柄別引受情報> T12060産地別銘柄別引受情報s { get; set; }
        
        /// <summary>
        /// t_12070_産地別銘柄別引受情報_規格別
        /// </summary>
        public DbSet<T12070産地別銘柄別引受情報規格別> T12070産地別銘柄別引受情報規格別s { get; set; }
        
        /// <summary>
        /// t_12080_組合員等別共済金額設定
        /// </summary>
        public DbSet<T12080組合員等別共済金額設定> T12080組合員等別共済金額設定s { get; set; }
        
        /// <summary>
        /// t_12090_組合員等別徴収情報
        /// </summary>
        public DbSet<T12090組合員等別徴収情報> T12090組合員等別徴収情報s { get; set; }
        
        /// <summary>
        /// t_12100_基準単収修正量
        /// </summary>
        public DbSet<T12100基準単収修正量> T12100基準単収修正量s { get; set; }
        
        /// <summary>
        /// t_12110_基準収穫量集計表
        /// </summary>
        public DbSet<T12110基準収穫量集計表> T12110基準収穫量集計表s { get; set; }
        
        /// <summary>
        /// t_12120_組合員等別賦課金情報
        /// </summary>
        public DbSet<T12120組合員等別賦課金情報> T12120組合員等別賦課金情報s { get; set; }
        
        /// <summary>
        /// t_12130_最低付保割合
        /// </summary>
        public DbSet<T12130最低付保割合> T12130最低付保割合s { get; set; }
        
        /// <summary>
        /// t_12140_引受耕地削除データ保持
        /// </summary>
        public DbSet<T12140引受耕地削除データ保持> T12140引受耕地削除データ保持s { get; set; }
        
        /// <summary>
        /// t_13010_組合等引受_合計部
        /// </summary>
        public DbSet<T13010組合等引受合計部> T13010組合等引受合計部s { get; set; }
        
        /// <summary>
        /// t_13020_組合等引受_危険段階毎明細部
        /// </summary>
        public DbSet<T13020組合等引受危険段階毎明細部> T13020組合等引受危険段階毎明細部s { get; set; }
        
        /// <summary>
        /// t_13030_組合等引受_告示単価毎明細部
        /// </summary>
        public DbSet<T13030組合等引受告示単価毎明細部> T13030組合等引受告示単価毎明細部s { get; set; }
        
        /// <summary>
        /// t_13040_組合等引受_危険段階毎明細部_pq
        /// </summary>
        public DbSet<T13040組合等引受危険段階毎明細部Pq> T13040組合等引受危険段階毎明細部Pqs { get; set; }
        
        /// <summary>
        /// t_13050_支所別引受集計情報
        /// </summary>
        public DbSet<T13050支所別引受集計情報> T13050支所別引受集計情報s { get; set; }
        
        /// <summary>
        /// t_13060_引受通知書
        /// </summary>
        public DbSet<T13060引受通知書> T13060引受通知書s { get; set; }
        
        /// <summary>
        /// t_14010_規模別分布
        /// </summary>
        public DbSet<T14010規模別分布> T14010規模別分布s { get; set; }
        
        /// <summary>
        /// t_14060_面積区分情報
        /// </summary>
        public DbSet<T14060面積区分情報> T14060面積区分情報s { get; set; }
        
        /// <summary>
        /// t_14070_規模別面積区分情報
        /// </summary>
        public DbSet<T14070規模別面積区分情報> T14070規模別面積区分情報s { get; set; }
        
        /// <summary>
        /// t_15010_交付徴収
        /// </summary>
        public DbSet<T15010交付徴収> T15010交付徴収s { get; set; }
        
        /// <summary>
        /// t_15020_組合等交付
        /// </summary>
        public DbSet<T15020組合等交付> T15020組合等交付s { get; set; }
        
        /// <summary>
        /// t_19010_大量データ受入_異動申告ok
        /// </summary>
        public DbSet<T19010大量データ受入異動申告ok> T19010大量データ受入異動申告oks { get; set; }
        
        /// <summary>
        /// t_19020_大量データ受入_基準収穫量ok
        /// </summary>
        public DbSet<T19020大量データ受入基準収穫量ok> T19020大量データ受入基準収穫量oks { get; set; }
        
        /// <summary>
        /// t_19030_大量データ受入_組合員等別補償割合等ok
        /// </summary>
        public DbSet<T19030大量データ受入組合員等別補償割合等ok> T19030大量データ受入組合員等別補償割合等oks { get; set; }
        
        /// <summary>
        /// t_19040_大量データ受入_組合員等別類別一筆半損特約等o
        /// </summary>
        public DbSet<T19041大量データ受入組合員等別類別一筆半損特約等ok> T19041大量データ受入組合員等別類別一筆半損特約等oks { get; set; }
        
        /// <summary>
        /// t_19050_大量データ受入_組合員等類別設定ok
        /// </summary>
        public DbSet<T19050大量データ受入組合員等類別設定ok> T19050大量データ受入組合員等類別設定oks { get; set; }
        
        /// <summary>
        /// t_19070_大量データ受入_加入申込書ok
        /// </summary>
        public DbSet<T19070大量データ受入加入申込書ok> T19070大量データ受入加入申込書oks { get; set; }
        
        /// <summary>
        /// t_19080_大量データ受入_組合員等類別平均単収ok
        /// </summary>
        public DbSet<T19080大量データ受入組合員等類別平均単収ok> T19080大量データ受入組合員等類別平均単収oks { get; set; }
        
        /// <summary>
        /// t_21010_組合員等申告_全相殺
        /// </summary>
        public DbSet<T21010組合員等申告全相殺> T21010組合員等申告全相殺s { get; set; }
        
        /// <summary>
        /// t_21020_組合員等申告_インデックス
        /// </summary>
        public DbSet<T21020組合員等申告インデックス> T21020組合員等申告インデックスs { get; set; }
        
        /// <summary>
        /// t_21030_組合員等申告_pq
        /// </summary>
        public DbSet<T21030組合員等申告Pq> T21030組合員等申告Pqs { get; set; }
        
        /// <summary>
        /// t_21040_悉皆評価
        /// </summary>
        public DbSet<T21040悉皆評価> T21040悉皆評価s { get; set; }
        
        /// <summary>
        /// t_21050_悉皆評価_計算結果
        /// </summary>
        public DbSet<T21050悉皆評価計算結果> T21050悉皆評価計算結果s { get; set; }
        
        /// <summary>
        /// t_21060_共済事故等情報入力
        /// </summary>
        public DbSet<T21060共済事故等情報入力> T21060共済事故等情報入力s { get; set; }
        
        /// <summary>
        /// t_21070_特例悉皆評価
        /// </summary>
        public DbSet<T21070特例悉皆評価> T21070特例悉皆評価s { get; set; }
        
        /// <summary>
        /// t_21080_農単抜取調査
        /// </summary>
        public DbSet<T21080農単抜取調査> T21080農単抜取調査s { get; set; }
        
        /// <summary>
        /// t_21090_売渡全数調査
        /// </summary>
        public DbSet<T21090売渡全数調査> T21090売渡全数調査s { get; set; }
        
        /// <summary>
        /// t_21100_施設計量
        /// </summary>
        public DbSet<T21100施設計量> T21100施設計量s { get; set; }
        
        /// <summary>
        /// t_21110_施設全数調査
        /// </summary>
        public DbSet<T21110施設全数調査> T21110施設全数調査s { get; set; }
        
        /// <summary>
        /// t_21120_施設搬入収穫量
        /// </summary>
        public DbSet<T21120施設搬入収穫量> T21120施設搬入収穫量s { get; set; }
        
        /// <summary>
        /// t_21130_税務申告全数調査
        /// </summary>
        public DbSet<T21130税務申告全数調査> T21130税務申告全数調査s { get; set; }
        
        /// <summary>
        /// t_21140_統計単収評価
        /// </summary>
        public DbSet<T21140統計単収評価> T21140統計単収評価s { get; set; }
        
        /// <summary>
        /// t_21150_出荷収穫量評価
        /// </summary>
        public DbSet<T21150出荷収穫量評価> T21150出荷収穫量評価s { get; set; }
        
        /// <summary>
        /// t_21160_耕地面積分割評価情報
        /// </summary>
        public DbSet<T21160耕地面積分割評価情報> T21160耕地面積分割評価情報s { get; set; }
        
        /// <summary>
        /// t_21170_出荷数量等調査野帳情報
        /// </summary>
        public DbSet<T21170出荷数量等調査野帳情報> T21170出荷数量等調査野帳情報s { get; set; }
        
        /// <summary>
        /// t_21180_出荷数量等調査野帳情報_規格別
        /// </summary>
        public DbSet<T21180出荷数量等調査野帳情報規格別> T21180出荷数量等調査野帳情報規格別s { get; set; }
        
        /// <summary>
        /// t_21190_自家保有数量情報
        /// </summary>
        public DbSet<T21190自家保有数量情報> T21190自家保有数量情報s { get; set; }
        
        /// <summary>
        /// t_21200_出荷評価地区別生産数量入力
        /// </summary>
        public DbSet<T21200出荷評価地区別生産数量入力> T21200出荷評価地区別生産数量入力s { get; set; }
        
        /// <summary>
        /// t_21210_評価チェック
        /// </summary>
        public DbSet<T21210評価チェック> T21210評価チェックs { get; set; }
        
        /// <summary>
        /// t_22010_組合員等別仮渡情報
        /// </summary>
        public DbSet<T22010組合員等別仮渡情報> T22010組合員等別仮渡情報s { get; set; }
        
        /// <summary>
        /// t_22015_組合員等別仮渡情報類
        /// </summary>
        public DbSet<T22015組合員等別仮渡情報類> T22015組合員等別仮渡情報類s { get; set; }
        
        /// <summary>
        /// t_22020_仮渡計算組合員等別
        /// </summary>
        public DbSet<T22020仮渡計算組合員等別> T22020仮渡計算組合員等別s { get; set; }
        
        /// <summary>
        /// t_22030_仮渡計算耕地別
        /// </summary>
        public DbSet<T22030仮渡計算耕地別> T22030仮渡計算耕地別s { get; set; }
        
        /// <summary>
        /// t_22040_仮渡し設定
        /// </summary>
        public DbSet<T22040仮渡し設定> T22040仮渡し設定s { get; set; }
        
        /// <summary>
        /// t_22050_仮渡評価計算チェック_pq
        /// </summary>
        public DbSet<T22050仮渡評価計算チェックPq> T22050仮渡評価計算チェックPqs { get; set; }
        
        /// <summary>
        /// t_23010_検見抜取評価
        /// </summary>
        public DbSet<T23010検見抜取評価> T23010検見抜取評価s { get; set; }
        
        /// <summary>
        /// t_23020_実測抜取評価
        /// </summary>
        public DbSet<T23020実測抜取評価> T23020実測抜取評価s { get; set; }
        
        /// <summary>
        /// t_23030_平均単収差選択_抜取班
        /// </summary>
        public DbSet<T23030平均単収差選択抜取班> T23030平均単収差選択抜取班s { get; set; }
        
        /// <summary>
        /// t_23040_平均単収差選択_調整班
        /// </summary>
        public DbSet<T23040平均単収差選択調整班> T23040平均単収差選択調整班s { get; set; }
        
        /// <summary>
        /// t_23050_平均単収差
        /// </summary>
        public DbSet<T23050平均単収差> T23050平均単収差s { get; set; }
        
        /// <summary>
        /// t_23060_平均単収差_抜取班集計
        /// </summary>
        public DbSet<T23060平均単収差抜取班集計> T23060平均単収差抜取班集計s { get; set; }
        
        /// <summary>
        /// t_23070_平均単収差_検見単収
        /// </summary>
        public DbSet<T23070平均単収差検見単収> T23070平均単収差検見単収s { get; set; }
        
        /// <summary>
        /// t_23080_平均単収差_実測単収
        /// </summary>
        public DbSet<T23080平均単収差実測単収> T23080平均単収差実測単収s { get; set; }
        
        /// <summary>
        /// t_23090_調整班平均単収差
        /// </summary>
        public DbSet<T23090調整班平均単収差> T23090調整班平均単収差s { get; set; }
        
        /// <summary>
        /// t_23100_調整班平均単収差_引受方式集計
        /// </summary>
        public DbSet<T23100調整班平均単収差引受方式集計> T23100調整班平均単収差引受方式集計s { get; set; }
        
        /// <summary>
        /// t_23110_調整班平均単収差_検見単収
        /// </summary>
        public DbSet<T23110調整班平均単収差検見単収> T23110調整班平均単収差検見単収s { get; set; }
        
        /// <summary>
        /// t_23120_単当修正量
        /// </summary>
        public DbSet<T23120単当修正量> T23120単当修正量s { get; set; }
        
        /// <summary>
        /// t_23130_調後悉皆評価
        /// </summary>
        public DbSet<T23130調後悉皆評価> T23130調後悉皆評価s { get; set; }
        
        /// <summary>
        /// t_24010_組合員等別損害情報
        /// </summary>
        public DbSet<T24010組合員等別損害情報> T24010組合員等別損害情報s { get; set; }
        
        /// <summary>
        /// t_24020_当初計算経過筆
        /// </summary>
        public DbSet<T24020当初計算経過筆> T24020当初計算経過筆s { get; set; }
        
        /// <summary>
        /// t_24030_当初計算経過組合員等
        /// </summary>
        public DbSet<T24030当初計算経過組合員等> T24030当初計算経過組合員等s { get; set; }
        
        /// <summary>
        /// t_24040_特例組合員等別損害情報
        /// </summary>
        public DbSet<T24040特例組合員等別損害情報> T24040特例組合員等別損害情報s { get; set; }
        
        /// <summary>
        /// t_24050_組合等当初評価
        /// </summary>
        public DbSet<T24050組合等当初評価> T24050組合等当初評価s { get; set; }
        
        /// <summary>
        /// t_24060_耕地別分割評価情報
        /// </summary>
        public DbSet<T24060耕地別分割評価情報> T24060耕地別分割評価情報s { get; set; }
        
        /// <summary>
        /// t_24070_耕地別一筆全損半損評価情報
        /// </summary>
        public DbSet<T24070耕地別一筆全損半損評価情報> T24070耕地別一筆全損半損評価情報s { get; set; }
        
        /// <summary>
        /// t_24080_規格別収穫量等配分計算
        /// </summary>
        public DbSet<T24080規格別収穫量等配分計算> T24080規格別収穫量等配分計算s { get; set; }
        
        /// <summary>
        /// t_24090_規格別収穫量等配分計算_規格別
        /// </summary>
        public DbSet<T24090規格別収穫量等配分計算規格別> T24090規格別収穫量等配分計算規格別s { get; set; }
        
        /// <summary>
        /// t_24100_産地別銘柄別評価情報
        /// </summary>
        public DbSet<T24100産地別銘柄別評価情報> T24100産地別銘柄別評価情報s { get; set; }
        
        /// <summary>
        /// t_24110_産地別銘柄別評価情報_規格別
        /// </summary>
        public DbSet<T24110産地別銘柄別評価情報規格別> T24110産地別銘柄別評価情報規格別s { get; set; }
        
        /// <summary>
        /// t_24120_組合員等類別評価情報
        /// </summary>
        public DbSet<T24120組合員等類別評価情報> T24120組合員等類別評価情報s { get; set; }
        
        /// <summary>
        /// t_24130_組合員等別評価情報
        /// </summary>
        public DbSet<T24130組合員等別評価情報> T24130組合員等別評価情報s { get; set; }
        
        /// <summary>
        /// t_24140_地区別評価情報
        /// </summary>
        public DbSet<T24140地区別評価情報> T24140地区別評価情報s { get; set; }
        
        /// <summary>
        /// t_24150_組合等当初評価高情報
        /// </summary>
        public DbSet<T24150組合等当初評価高情報> T24150組合等当初評価高情報s { get; set; }
        
        /// <summary>
        /// t_24160_組合等損害評価書情報
        /// </summary>
        public DbSet<T24160組合等損害評価書情報> T24160組合等損害評価書情報s { get; set; }
        
        /// <summary>
        /// t_24165_損害評価書情報集計
        /// </summary>
        public DbSet<T24165損害評価書情報集計> T24165損害評価書情報集計s { get; set; }
        
        /// <summary>
        /// t_24170_政府再保険認定区分別当初評価高情報
        /// </summary>
        public DbSet<T24170政府再保険認定区分別当初評価高情報> T24170政府再保険認定区分別当初評価高情報s { get; set; }
        
        /// <summary>
        /// t_24175_当初評価高情報集計
        /// </summary>
        public DbSet<T24175当初評価高情報集計> T24175当初評価高情報集計s { get; set; }
        
        /// <summary>
        /// t_24180_政府再保険認定区分類区分別損害評価書情報
        /// </summary>
        public DbSet<T24180政府再保険認定区分類区分別損害評価書情報> T24180政府再保険認定区分類区分別損害評価書情報s { get; set; }
        
        /// <summary>
        /// t_24200_産地別銘柄別評価情報_営農
        /// </summary>
        public DbSet<T24200産地別銘柄別評価情報営農> T24200産地別銘柄別評価情報営農s { get; set; }
        
        /// <summary>
        /// t_24210_産地別銘柄別評価情報_営農_規格別
        /// </summary>
        public DbSet<T24210産地別銘柄別評価情報営農規格別> T24210産地別銘柄別評価情報営農規格別s { get; set; }
        
        /// <summary>
        /// t_24220_組合員等類別評価情報_営農
        /// </summary>
        public DbSet<T24220組合員等類別評価情報営農> T24220組合員等類別評価情報営農s { get; set; }
        
        /// <summary>
        /// t_24230_産地別銘柄別評価情報_一筆全半損_営農
        /// </summary>
        public DbSet<T24230産地別銘柄別評価情報一筆全半損営農> T24230産地別銘柄別評価情報一筆全半損営農s { get; set; }
        
        /// <summary>
        /// t_24240_産地別銘柄別評価情報_一筆全半損_営農_規格別
        /// </summary>
        public DbSet<T24240産地別銘柄別評価情報一筆全半損営農規格別> T24240産地別銘柄別評価情報一筆全半損営農規格別s { get; set; }
        
        /// <summary>
        /// t_24250_調後組合員等別損害情報
        /// </summary>
        public DbSet<T24250調後組合員等別損害情報> T24250調後組合員等別損害情報s { get; set; }
        
        /// <summary>
        /// t_24260_調後組合等当初評価
        /// </summary>
        public DbSet<T24260調後組合等当初評価> T24260調後組合等当初評価s { get; set; }
        
        /// <summary>
        /// t_24270_支所別当初評価集計
        /// </summary>
        public DbSet<T24270支所別当初評価集計> T24270支所別当初評価集計s { get; set; }
        
        /// <summary>
        /// t_24280_調後支所別当初評価集計
        /// </summary>
        public DbSet<T24280調後支所別当初評価集計> T24280調後支所別当初評価集計s { get; set; }
        
        /// <summary>
        /// t_24290_支所別当初評価高情報
        /// </summary>
        public DbSet<T24290支所別当初評価高情報> T24290支所別当初評価高情報s { get; set; }
        
        /// <summary>
        /// t_25010_組合員等別免責情報
        /// </summary>
        public DbSet<T25010組合員等別免責情報> T25010組合員等別免責情報s { get; set; }
        
        /// <summary>
        /// t_25020_組合員等別支払情報
        /// </summary>
        public DbSet<T25020組合員等別支払情報> T25020組合員等別支払情報s { get; set; }
        
        /// <summary>
        /// T_25030_組合員等別支払情報明細
        /// </summary>
        public DbSet<T25030組合員等別支払情報明細> T25030組合員等別支払情報明細s { get; set; }
        
        /// <summary>
        /// t_26010_保険金
        /// </summary>
        public DbSet<T26010保険金> T26010保険金s { get; set; }
        
        /// <summary>
        /// t_26020_保険金_引受方式明細
        /// </summary>
        public DbSet<T26020保険金引受方式明細> T26020保険金引受方式明細s { get; set; }
        
        /// <summary>
        /// t_29010_大量データ受入_分割情報OK
        /// </summary>
        public DbSet<T29010大量データ受入分割情報ok> T29010大量データ受入分割情報oks { get; set; }
        
        /// <summary>
        /// t_29020_大量データ受入_出荷数量等調査野帳OK
        /// </summary>
        public DbSet<T29020大量データ受入出荷数量等調査野帳ok> T29020大量データ受入出荷数量等調査野帳oks { get; set; }
        
        /// <summary>
        /// t_29030_大量データ受入_自家保有数量OK
        /// </summary>
        public DbSet<T29030大量データ受入自家保有数量ok> T29030大量データ受入自家保有数量oks { get; set; }
        
        /// <summary>
        /// t_29040_大量データ受入_組合員等別類別連合会認定区分O
        /// </summary>
        public DbSet<T29040大量データ受入組合員等別類別連合会認定区分o> T29040大量データ受入組合員等別類別連合会認定区分os { get; set; }
        
        /// <summary>
        /// t_29050_大量データ受入_一筆半損情報OK
        /// </summary>
        public DbSet<T29050大量データ受入一筆半損情報ok> T29050大量データ受入一筆半損情報oks { get; set; }
        
        /// <summary>
        /// t_29060_大量データ受入_農単申告抜取調査ok
        /// </summary>
        public DbSet<T29060大量データ受入農単申告抜取調査ok> T29060大量データ受入農単申告抜取調査oks { get; set; }
        
        /// <summary>
        /// t_29080_大量データ受入_全相殺損害評価野帳ok
        /// </summary>
        public DbSet<T29080大量データ受入全相殺損害評価野帳ok> T29080大量データ受入全相殺損害評価野帳oks { get; set; }
        
        /// <summary>
        /// t_29090_大量データ受入_全相殺組合員等類別収穫量ok
        /// </summary>
        public DbSet<T29090大量データ受入全相殺組合員等類別収穫量ok> T29090大量データ受入全相殺組合員等類別収穫量oks { get; set; }
        
        /// <summary>
        /// t_29100_大量データ受入_全相殺施設搬入調査ok
        /// </summary>
        public DbSet<T29100大量データ受入全相殺施設搬入調査ok> T29100大量データ受入全相殺施設搬入調査oks { get; set; }
        
        /// <summary>
        /// t_29110_大量データ受入_全相殺売渡数量調査ok
        /// </summary>
        public DbSet<T29110大量データ受入全相殺売渡数量調査ok> T29110大量データ受入全相殺売渡数量調査oks { get; set; }
        
        /// <summary>
        /// t_29120_大量データ受入_全相殺税務申告調査ok
        /// </summary>
        public DbSet<T29120大量データ受入全相殺税務申告調査ok> T29120大量データ受入全相殺税務申告調査oks { get; set; }
        
        /// <summary>
        /// w_00120_料率
        /// </summary>
        public DbSet<W00120料率> W00120料率s { get; set; }
        
        /// <summary>
        /// w_10230_危険段階
        /// </summary>
        public DbSet<W10230危険段階> W10230危険段階s { get; set; }
        
        /// <summary>
        /// w_10240_危険段階地域別設定
        /// </summary>
        public DbSet<W10240危険段階地域別設定> W10240危険段階地域別設定s { get; set; }
        
        /// <summary>
        /// w_11040_個人料率
        /// </summary>
        public DbSet<W11040個人料率> W11040個人料率s { get; set; }
        
        /// <summary>
        /// v_30020_口座振替結果
        /// </summary>
        public DbSet<V30020口座振替結果> V30020口座振替結果s { get; set; }

        #endregion
    }
}
