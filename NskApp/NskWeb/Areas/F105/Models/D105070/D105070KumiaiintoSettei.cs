using NskAppModelLibrary.Context;
using CoreLibrary.Core.Extensions;
using CoreLibrary.Core.Validator;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Npgsql;
using Microsoft.EntityFrameworkCore;
using NskAppModelLibrary.Models;
using CoreLibrary.Core.Consts;
using NskWeb.Areas.F105.Consts;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using NskWeb.Areas.F105.Models.D105030;

namespace NskWeb.Areas.F105.Models.D105070
{
    /// <summary>
    /// 組合員等毎設定
    /// </summary>
    [Serializable]
    public class D105070KumiaiintoSettei
    {
        /// <summary>
        /// メッセージエリア4
        /// </summary>
        public string MessageArea4 { get; set; } = "メッセージエリア４";


        /// <summary>加入状況</summary>
        [Display(Name = "加入状況")]
        [Required]
        public F105Const.KanyuStateType? KanyuState { get; set; } = F105Const.KanyuStateType.NO_MEMBER;
        /// <summary>加入形態</summary>
        [Display(Name = "加入形態")]
        [Required]
        public string? KanyuKeitai { get; set; } = string.Empty;

        /// <summary>交付申請者管理コード</summary>
        [Display(Name = "交付申請者管理コード")]
        [WithinDigitLength(13)]
        public string KoufuShinseishaKanriCd { get; set; } = string.Empty;
        /// <summary>共済金額改定時コード</summary>
        [Display(Name = "共済金額改定時コード")]
        public string KyosaiKingakuKaiteijiCd { get; set; } = string.Empty;

        /// <summary>自動継続特約</summary>
        [Display(Name = "自動継続特約")]
        public bool JidoKeizoku { get; set; } = false;
        /// <summary>解除</summary>
        [Display(Name = "解除")]
        public string? Kaijo { get; set; } = string.Empty;
        /// <summary>解除日付</summary>
        [Display(Name = "解除日付")]
        public DateTime? KaijoHiduke { get; set; } = null;

        /// <summary>地域集団コード</summary>
        [Display(Name = "地域集団コード")]
        [Numeric]
        [WithinDigitLength(13)]
        public string? ChiikiSyudanCd { get; set; } = string.Empty;
        /// <summary>地域集団名</summary>
        public string? ChiikiSyudanNm { get; set; } = string.Empty;


        /// <summary>共通申請割引方法</summary>
        [Display(Name = "共通申請割引方法")]
        [Required]
        public string KyotuShinseiWaribikiHouho { get; set; } = string.Empty;

        /// <summary>割引割合</summary>
        [Display(Name = "割引割合")]
        [Numeric]
        [WithinDigitLength(3)]
        [DisplayFormat(DataFormatString = "{0:0}", ApplyFormatInEditMode = true)]
        public decimal? WaribikiWariai { get; set; }
        /// <summary>割引金額</summary>
        [Display(Name = "割引金額")]
        [Numeric]
        [WithinDigitLength(8)]
        [DisplayFormat(DataFormatString = "{0:0}", ApplyFormatInEditMode = true)]
        public decimal? WaribikiKingaku { get; set; }

        /// <summary>割増割合</summary>
        [Display(Name = "割増割合")]
        [Numeric]
        [WithinDigitLength(3)]
        [DisplayFormat(DataFormatString = "{0:0}", ApplyFormatInEditMode = true)]
        public decimal? WarimashiWariai { get; set; }
        /// <summary>割増金額</summary>
        [Display(Name = "割増金額")]
        [Numeric]
        [WithinDigitLength(8)]
        [DisplayFormat(DataFormatString = "{0:0}", ApplyFormatInEditMode = true)]
        public decimal? WarimashiKingaku { get; set; }

        /// <summary>共通申請等割引理由</summary>
        [Display(Name = "共通申請等割引理由")]
        [WithinStringLength(20)]
        public string KyotuShinseitoWaribikiRiyu { get; set; } = string.Empty;

        /// <summary>xmin</summary>
        public uint? Xmin { get; set; }

        /// <summary>隠し項目：組合員等毎設定有無</summary>
        public bool Exists { get; set; } = false;

        /// <summary>加入形態ドロップダウンリスト選択値</summary>
        public List<SelectListItem> KanyuKeitaiList { get; set; } = new();

        /// <summary>共済金額改定時コード]ドロップダウンリスト選択値</summary>
        public List<SelectListItem> KyosaiKingakuKaiteijiList { get; set; } = new();

        /// <summary>共通申請割引方法ドロップダウンリスト選択値</summary>
        public List<SelectListItem> KyotuShinseitoWaribikiHouhoList { get; set; } = new();

        /// <summary>
        /// ドロップダウンリスト初期化
        /// </summary>
        /// <param name="dbContext"></param>
        public void InitializeDropdonwList(NskAppContext dbContext, D105070SessionInfo sessionInfo)
        {
            // ２．３．１０．[加入形態] ドロップダウンリスト項目を設定する。
            // 	(1) 「加入形態」ドロップダウンリスト項目に以下を設定する。
            // 	・「1:個人」
            // 	・「2:農作物共済資格団体」
            // 	・「10：法人」
            KanyuKeitaiList =
            [
                new($"{(int)F105Const.KanyuKeitaiType.Kojin} {F105Const.KanyuKeitaiType.Kojin.ToDescription()}", $"{(int)F105Const.KanyuKeitaiType.Kojin}"),
                new($"{(int)F105Const.KanyuKeitaiType.NSK} {F105Const.KanyuKeitaiType.NSK.ToDescription()}", $"{(int)F105Const.KanyuKeitaiType.NSK}"),
                new($"{(int)F105Const.KanyuKeitaiType.Hojin} {F105Const.KanyuKeitaiType.Hojin.ToDescription()}", $"{(int)F105Const.KanyuKeitaiType.Hojin}"),
            ];

            // ２．３．１１．[共済金額改定時コード]ドロップダウンリスト項目を設定する。
            //  (1) 「共済金額改定時コード」ドロップダウンリスト項目に以下を設定する。
            //  ・「a：変更後の共済限度額と同額」
            //  ・「b：変更しない」
            //  ・「c：改定時に申請する。」
            KyosaiKingakuKaiteijiList =
            [
                new($"{(int)F105Const.KyosaiKingakuKaiteiKbn.Dogaku} {F105Const.KyosaiKingakuKaiteiKbn.Dogaku.ToDescription()}", $"{(int)F105Const.KyosaiKingakuKaiteiKbn.Dogaku}"),
                new($"{(int)F105Const.KyosaiKingakuKaiteiKbn.None} {F105Const.KyosaiKingakuKaiteiKbn.None.ToDescription()}", $"{(int)F105Const.KyosaiKingakuKaiteiKbn.None}"),
                new($"{(int)F105Const.KyosaiKingakuKaiteiKbn.Shinsei} {F105Const.KyosaiKingakuKaiteiKbn.Shinsei.ToDescription()}", $"{(int)F105Const.KyosaiKingakuKaiteiKbn.Shinsei}"),
            ];

            // ２．２４．[共通申請割引方法]ドロップダウンリスト項目を取得する。
            // (1)	m_10290_共通申請等割引方法テーブルから通申請等割引方法コード、通申請等割引方法名称を取得する。
            // (2)	取得した結果をドロップダウンリストの項目として設定する。
            GetKyotuShinseitoWaribikiHouhoList(dbContext, sessionInfo);
        }


        /// <summary>
        /// [共通申請割引方法]ドロップダウンリスト項目を取得する
        /// DB検索仕様書 No.25
        /// </summary>
        /// <param name="dbContext"></param>
        private void GetKyotuShinseitoWaribikiHouhoList(NskAppContext dbContext, D105070SessionInfo sessionInfo)
        {
            KyotuShinseitoWaribikiHouhoList = new();
            StringBuilder query = new();
            query.Append(" SELECT ");
            query.Append($"    共通申請等割引方法コード As \"{nameof(Query25Result.KyotuShinseitoWaribikiHohoCd)}\" ");
            query.Append($"  , 共通申請等割引方法名称 As \"{nameof(Query25Result.KyotuShinseitoWaribikiHohoNm)}\" ");
            query.Append(" FROM ");
            query.Append("  m_10290_共通申請等割引方法 ");
            query.Append(" WHERE ");
            query.Append("      組合等コード   = @組合等コード ");
            query.Append("  AND 共済目的コード = @共済目的コード ");
            query.Append("  AND 年産           = @年産 ");
            query.Append(" ORDER BY 共通申請等割引方法コード ");
            NpgsqlParameter[] queryParams =
            [
                new ("組合等コード", sessionInfo.KumiaitoCd),
                new ("年産", sessionInfo.Nensan),
                new ("共済目的コード", sessionInfo.KyosaiMokutekiCd),
                new ("組合員等コード", sessionInfo.KumiaiintoCd),
            ];

            List<Query25Result> results = new();
            results.AddRange(dbContext.Database.SqlQueryRaw<Query25Result>(query.ToString(), queryParams));
            KyotuShinseitoWaribikiHouhoList.AddRange(results.Select(x =>
                new SelectListItem($"{x.KyotuShinseitoWaribikiHohoCd} {x.KyotuShinseitoWaribikiHohoNm}", x.KyotuShinseitoWaribikiHohoCd)));
        }

        /// <summary>
        /// DB検索仕様書 No.25 検索結果
        /// </summary>
        private class Query25Result
        {
            /// <summary>共通申請等割引方法コード</summary>
            public string KyotuShinseitoWaribikiHohoCd { get; set; } = string.Empty;
            /// <summary>共通申請等割引方法名称</summary>
            public string KyotuShinseitoWaribikiHohoNm { get; set; } = string.Empty;
        }

        /// <summary>
        /// 組合員等毎設定を取得する。
        /// DB検索仕様書 No.19
        /// </summary>
        /// <param name="dbContext"></param>
        public void GetKumiaitoSettei(NskAppContext dbContext, D105070SessionInfo sessionInfo)
        {
            // 組合員等毎設定を取得する。
            StringBuilder query = new();
            query.Append(" SELECT ");         
            query.Append($"   T1.未加入フラグ   As \"{nameof(D105070KumiaiintoSettei.KanyuState)}\" ");
            query.Append($"  ,T1.加入形態       As \"{nameof(D105070KumiaiintoSettei.KanyuKeitai)}\" ");
            query.Append($"  ,T1.交付申請者管理コード As \"{nameof(D105070KumiaiintoSettei.KoufuShinseishaKanriCd)}\" ");
            query.Append($"  ,T1.共済金額改定時コード As \"{nameof(D105070KumiaiintoSettei.KyosaiKingakuKaiteijiCd)}\" ");
            query.Append($"  ,T1.継続特約フラグ As \"{nameof(D105070KumiaiintoSettei.JidoKeizoku)}\" ");
            query.Append($"  ,CASE WHEN T2.引受解除日付 IS NOT NULL THEN '1' ");
            query.Append($"   ELSE '0' END      As \"{nameof(D105070KumiaiintoSettei.Kaijo)}\" "); 
            query.Append($"  ,T2.引受解除日付   As \"{nameof(D105070KumiaiintoSettei.KaijoHiduke)}\" ");
            query.Append($"  ,T1.地域集団コード As \"{nameof(D105070KumiaiintoSettei.ChiikiSyudanCd)}\" ");
            query.Append($"  ,T3.hojin_full_nm  As \"{nameof(D105070KumiaiintoSettei.ChiikiSyudanNm)}\" ");//氏名または法人名 As {nameof(D105070KumiaiintoSettei.ChiikiSyudanNm)} ");
            query.Append($"  ,T1.共通申請等割引方法コード As \"{nameof(D105070KumiaiintoSettei.KyotuShinseiWaribikiHouho)}\" ");
            query.Append($"  ,T1.割引割合       As \"{nameof(D105070KumiaiintoSettei.WaribikiWariai)}\" ");
            query.Append($"  ,T1.割引金額       As \"{nameof(D105070KumiaiintoSettei.WaribikiKingaku)}\" ");
            query.Append($"  ,T1.割増割合       As \"{nameof(D105070KumiaiintoSettei.WarimashiWariai)}\" ");
            query.Append($"  ,T1.割増金額       As \"{nameof(D105070KumiaiintoSettei.WarimashiKingaku)}\" ");
            query.Append($"  ,T1.共通申請等割引理由 As \"{nameof(D105070KumiaiintoSettei.KyotuShinseitoWaribikiRiyu)}\" ");
            query.Append($"  ,cast('' || T1.xmin as integer) As \"{nameof(D105070KumiaiintoSettei.Xmin)}\" ");
            query.Append($" FROM t_11010_個人設定 T1 ");
            query.Append(" LEFT OUTER JOIN t_11020_個人設定解除 T2");  
            query.Append(" ON    T1.組合等コード   = T2.組合等コード ");
            query.Append("  AND  T1.年産           = T2.年産 ");
            query.Append("  AND  T1.共済目的コード = T2.共済目的コード ");
            query.Append("  AND  T1.組合員等コード = T2.組合員等コード ");
            query.Append(" LEFT OUTER JOIN v_nogyosha T3");// 農業者情報 T3 ");   
            query.Append(" ON T1.地域集団コード = T3.kumiaiinto_cd ");  // 組合員等コード ");
            query.Append(" WHERE ");
            query.Append("       T1.組合等コード   = @組合等コード ");
            query.Append("  AND  T1.年産           = @年産 ");
            query.Append("  AND  T1.共済目的コード = @共済目的コード ");
            query.Append("  AND  T1.組合員等コード = @組合員等コード ");
            query.Append("");

            NpgsqlParameter[] queryParams =
            [
                new ("組合等コード", sessionInfo.KumiaitoCd),
                new ("年産", sessionInfo.Nensan),
                new ("共済目的コード", sessionInfo.KyosaiMokutekiCd),
                new ("組合員等コード", sessionInfo.KumiaiintoCd),
            ];

            Query19Result? result = dbContext.Database.SqlQueryRaw<Query19Result>(query.ToString(), queryParams)?.SingleOrDefault();
            Exists = result is not null;

            if (Exists)
            {
                this.KanyuState = int.TryParse(result.KanyuState, out int state) ? (F105Const.KanyuStateType?)state : null;
                this.KanyuKeitai = result.KanyuKeitai;
                this.KoufuShinseishaKanriCd = result.KoufuShinseishaKanriCd;
                this.KyosaiKingakuKaiteijiCd = result.KyosaiKingakuKaiteijiCd;
                this.JidoKeizoku = int.TryParse(result.JidoKeizoku, out int jidoKeizoku) ? (jidoKeizoku == 1) : false;
                this.Kaijo = int.TryParse(result.Kaijo, out int kaijo) ? kaijo == 1 ? "解除" : "未解除" : "未解除";
                this.KaijoHiduke = result.KaijoHiduke;
                this.ChiikiSyudanCd = result.ChiikiSyudanCd;
                this.ChiikiSyudanNm = result.ChiikiSyudanNm;

                this.KyotuShinseiWaribikiHouho = result.KyotuShinseiWaribikiHouho;
                this.WaribikiWariai = result.WaribikiWariai;
                this.WaribikiKingaku = result.WaribikiKingaku;
                this.WarimashiWariai = result.WarimashiWariai;
                this.WarimashiKingaku = result.WarimashiKingaku;
                this.KyotuShinseitoWaribikiRiyu = result.KyotuShinseitoWaribikiRiyu;

                this.Xmin = result.Xmin;
            }
        }

        /// <summary>
        /// DB検索仕様書 No.19 検索結果
        /// </summary>
        private class Query19Result
        {
            /// <summary>加入状況</summary>
            public string? KanyuState { get; set; } = string.Empty;
            /// <summary>加入形態</summary>
            public string? KanyuKeitai { get; set; } = string.Empty;
            /// <summary>交付申請者管理コード</summary>
            public string? KoufuShinseishaKanriCd { get; set; } = string.Empty;
            /// <summary>共済金額改定時コード</summary>
            public string? KyosaiKingakuKaiteijiCd { get; set; } = string.Empty;
            /// <summary>自動継続特約</summary>
            public string? JidoKeizoku { get; set; } = string.Empty;
            /// <summary>解除</summary>
            public string? Kaijo { get; set; } = string.Empty;
            /// <summary>解除日付</summary>
            public DateTime? KaijoHiduke { get; set; } = null;

            /// <summary>地域集団コード</summary>
            public string? ChiikiSyudanCd { get; set; } = string.Empty;
            /// <summary>地域集団名</summary>
            public string? ChiikiSyudanNm { get; set; } = string.Empty;

            /// <summary>xmin</summary>
            public uint? Xmin { get; set; }

            /// <summary>共通申請等割引方法コード</summary>
            public string? KyotuShinseiWaribikiHouho { get; set; } = string.Empty;
            /// <summary>割引割合</summary>
            public decimal? WaribikiWariai { get; set; }
            /// <summary>割引金額</summary>
            public decimal? WaribikiKingaku { get; set; }
            /// <summary>割増割合</summary>
            public decimal? WarimashiWariai { get; set; }
            /// <summary>割増金額</summary>
            public decimal? WarimashiKingaku { get; set; }
            /// <summary>共通申請等割引理由</summary>
            public string? KyotuShinseitoWaribikiRiyu { get; set; } = string.Empty;
        }

        /// <summary>
        /// 画面入力値をこのこのクラスに反映する
        /// </summary>
        /// <param name="src"></param>
        public void ApplyInput(D105070KumiaiintoSettei src)
        {
            this.MessageArea4 = src.MessageArea4;
            this.KanyuState = src.KanyuState;
            this.KanyuKeitai = src.KanyuKeitai;
            this.KoufuShinseishaKanriCd = src.KoufuShinseishaKanriCd;
            this.KyosaiKingakuKaiteijiCd = src.KyosaiKingakuKaiteijiCd;
            this.JidoKeizoku = src.JidoKeizoku;
            this.Kaijo = src.Kaijo;
            this.KaijoHiduke = src.KaijoHiduke;
            this.ChiikiSyudanCd = src.ChiikiSyudanCd;
            this.ChiikiSyudanNm = src.ChiikiSyudanNm;
            this.KyotuShinseiWaribikiHouho = src.KyotuShinseiWaribikiHouho;
            this.WaribikiWariai = src.WaribikiWariai;
            this.WaribikiKingaku = src.WaribikiKingaku;
            this.WarimashiWariai = src.WarimashiWariai;
            this.WarimashiKingaku = src.WarimashiKingaku;
            this.KyotuShinseitoWaribikiRiyu = src.KyotuShinseitoWaribikiRiyu;
            this.Exists = src.Exists;
            this.Xmin = src.Xmin;
        }

        ///// <summary>
        ///// t_11020_個人設定解除の登録を行う。
        ///// </summary>
        ///// <param name="dbContext"></param>
        ///// <param name="sessionInfo"></param>
        ///// <returns></returns>
        //public int RegistKanyuKaijo(ref NskAppContext dbContext, D105070SessionInfo sessionInfo, string userId, DateTime sysDateTime)
        //{
        //    // t_00010_引受回テーブルから引受回を取得する。
        //    short? maxHikiukeKai = dbContext.T00010引受回s.Where(m =>
        //        (m.組合等コード == sessionInfo.KumiaitoCd) &&
        //        (m.年産 == sessionInfo.Nensan) &&
        //        (m.共済目的コード == sessionInfo.KyosaiMokutekiCd))?.
        //        Max(m => m.引受回);

        //    T11020個人設定解除 cancelRec = new()
        //    {
        //        組合等コード = sessionInfo.KumiaitoCd,
        //        年産 = (short)sessionInfo.Nensan,
        //        共済目的コード = sessionInfo.KyosaiMokutekiCd,
        //        組合員等コード = sessionInfo.KumiaiintoCd,
        //        解除引受回 = (short)maxHikiukeKai,
        //        解除申出日付 = sysDateTime,
        //        引受解除日付 = null,
        //        引受解除返還賦課金額 = null,
        //        解除理由コード = null,
        //        登録日時 = sysDateTime,
        //        登録ユーザid = userId,
        //        更新日時 = sysDateTime,
        //        更新ユーザid = userId,
        //    };
        //    dbContext.T11020個人設定解除s.Add(cancelRec);

        //    return dbContext.SaveChanges();
        //}

        /// <summary>
        /// t_11010_個人設定の登録を行う。
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="sessionInfo"></param>
        /// <param name="userId"></param>
        /// <param name="sysDateTime"></param>
        /// <returns></returns>
        public int RegistKojinSettei(ref NskAppContext dbContext, D105070SessionInfo sessionInfo, string userId, DateTime sysDateTime)
        {
            //T11010個人設定 addRec = new()
            //{
            //    組合等コード = sessionInfo.KumiaitoCd,
            //    年産 = (short)sessionInfo.Nensan,
            //    共済目的コード = sessionInfo.KyosaiMokutekiCd,
            //    組合員等コード = sessionInfo.KumiaiintoCd,
            //    加入形態 = KanyuKeitai,
            //    交付申請者管理コード = this.KoufuShinseishaKanriCd,
            //    共済金額改定時コード = this.KyosaiKingakuKaiteijiCd,
            //    未加入フラグ = $"{(short)KanyuState}",
            //    継続特約フラグ = JidoKeizoku ? CoreConst.FLG_ON : CoreConst.FLG_OFF,
            //    地域集団コード = ChiikiSyudanCd,
            //    登録日時 = sysDateTime,
            //    登録ユーザid = userId,
            //    更新日時 = sysDateTime,
            //    更新ユーザid = userId
            //};
            //dbContext.T11010個人設定s.Add(addRec);

            //return dbContext.SaveChanges();
            StringBuilder addQuery = new();
            addQuery.Append(" INSERT INTO t_11010_個人設定 ( ");
            addQuery.Append("   組合等コード ");
            addQuery.Append("  ,年産 ");
            addQuery.Append("  ,共済目的コード ");
            addQuery.Append("  ,組合員等コード ");
            addQuery.Append("  ,加入形態 ");
            addQuery.Append("  ,交付申請者管理コード ");
            addQuery.Append("  ,共済金額改定時コード ");
            addQuery.Append("  ,未加入フラグ ");
            addQuery.Append("  ,継続特約フラグ ");
            addQuery.Append("  ,地域集団コード ");
            addQuery.Append("  ,共通申請等割引方法コード ");
            addQuery.Append("  ,割引割合 ");
            addQuery.Append("  ,割引金額 ");
            addQuery.Append("  ,割増割合 ");
            addQuery.Append("  ,割増金額 ");
            addQuery.Append("  ,共通申請等割引理由 ");
            addQuery.Append("  ,登録日時 ");
            addQuery.Append("  ,登録ユーザid ");
            addQuery.Append("  ,更新日時 ");
            addQuery.Append("  ,更新ユーザid ");
            addQuery.Append(" ) VALUES ( ");
            addQuery.Append("   @組合等コード ");
            addQuery.Append("  ,@年産 ");
            addQuery.Append("  ,@共済目的コード ");
            addQuery.Append("  ,@組合員等コード ");
            addQuery.Append("  ,@加入形態 ");
            addQuery.Append("  ,@交付申請者管理コード ");
            addQuery.Append("  ,@共済金額改定時コード ");
            addQuery.Append("  ,@未加入フラグ ");
            addQuery.Append("  ,@継続特約フラグ ");
            addQuery.Append("  ,@地域集団コード ");
            addQuery.Append("  ,@共通申請等割引方法コード ");
            addQuery.Append("  ,@割引割合 ");
            addQuery.Append("  ,@割引金額 ");
            addQuery.Append("  ,@割増割合 ");
            addQuery.Append("  ,@割増金額 ");
            addQuery.Append("  ,@共通申請等割引理由 ");
            addQuery.Append("  ,@登録日時 ");
            addQuery.Append("  ,@登録ユーザid ");
            addQuery.Append("  ,@更新日時 ");
            addQuery.Append("  ,@更新ユーザid ");
            addQuery.Append(" ) ");

            NpgsqlParameter[] addParams =
            [
                new ("組合等コード", sessionInfo.KumiaitoCd),
                new ("年産", sessionInfo.Nensan),
                new ("共済目的コード", sessionInfo.KyosaiMokutekiCd),
                new ("組合員等コード", sessionInfo.KumiaiintoCd),
                new ("加入形態", KanyuKeitai),
                new ("交付申請者管理コード", string.IsNullOrEmpty(KoufuShinseishaKanriCd) ? DBNull.Value : KoufuShinseishaKanriCd),
                new ("共済金額改定時コード", string.IsNullOrEmpty(KyosaiKingakuKaiteijiCd) ? DBNull.Value : KyosaiKingakuKaiteijiCd),
                new ("未加入フラグ", $"{(short)KanyuState}"),
                new ("継続特約フラグ", JidoKeizoku ? CoreConst.FLG_ON : CoreConst.FLG_OFF),
                new ("地域集団コード", string.IsNullOrEmpty(ChiikiSyudanCd) ? DBNull.Value : ChiikiSyudanCd),
                new ("共通申請等割引方法コード", KyotuShinseiWaribikiHouho),
                new ("割引割合", !WaribikiWariai.HasValue ? DBNull.Value : WaribikiWariai),
                new ("割引金額", !WaribikiKingaku.HasValue ? DBNull.Value : WaribikiKingaku),
                new ("割増割合", !WarimashiWariai.HasValue ? DBNull.Value : WarimashiWariai),
                new ("割増金額", !WarimashiKingaku.HasValue ? DBNull.Value : WarimashiKingaku),
                new ("共通申請等割引理由", string.IsNullOrEmpty(KyotuShinseitoWaribikiRiyu) ? DBNull.Value : KyotuShinseitoWaribikiRiyu),
                new ("登録日時", sysDateTime),
                new ("登録ユーザid", userId),
                new ("更新日時", sysDateTime),
                new ("更新ユーザid", userId),
            ];
            return dbContext.Database.ExecuteSqlRaw(addQuery.ToString(), addParams);
        }

        /// <summary>
        /// t_11010_個人設定の更新を行う。
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="sessionInfo"></param>
        /// <param name="userId"></param>
        /// <param name="sysDateTime"></param>
        /// <returns></returns>
        public int UpdateKojinSettei(ref NskAppContext dbContext, D105070SessionInfo sessionInfo, string userId, DateTime sysDateTime)
        {
            int updCount = 0;

            if (!Xmin.HasValue)
            {
                // xmin nullは処理対象外
                return 0;
            }

            // t_11030_個人設定類の対象レコードを更新する。
            StringBuilder updRuibetsu = new();
            updRuibetsu.Append("UPDATE t_11010_個人設定 SET ");
            updRuibetsu.Append("  組合等コード      = @組合等コード ");
            updRuibetsu.Append(" ,年産              = @年産 ");
            updRuibetsu.Append(" ,共済目的コード    = @共済目的コード ");
            updRuibetsu.Append(" ,組合員等コード    = @組合員等コード ");
            updRuibetsu.Append(" ,加入形態          = @加入形態 ");
            updRuibetsu.Append(" ,未加入フラグ      = @未加入フラグ ");
            updRuibetsu.Append(" ,交付申請者管理コード = @交付申請者管理コード ");
            updRuibetsu.Append(" ,共済金額改定時コード = @共済金額改定時コード ");
            updRuibetsu.Append(" ,継続特約フラグ    = @継続特約フラグ ");
            updRuibetsu.Append(" ,地域集団コード    = @地域集団コード ");
            updRuibetsu.Append(" ,共通申請等割引方法コード = @共通申請等割引方法 ");
            updRuibetsu.Append(" ,割引割合          = @割引割合 ");
            updRuibetsu.Append(" ,割引金額          = @割引金額 ");
            updRuibetsu.Append(" ,割増割合          = @割増割合 ");
            updRuibetsu.Append(" ,割増金額          = @割増金額 ");
            updRuibetsu.Append(" ,共通申請等割引理由 = @共通申請等割引理由 ");
            updRuibetsu.Append(" ,更新日時          = @システム日時 ");
            updRuibetsu.Append(" ,更新ユーザid      = @ユーザID ");
            updRuibetsu.Append("WHERE ");
            updRuibetsu.Append("     組合等コード   = @組合等コード ");
            updRuibetsu.Append(" AND 年産           = @年産 ");
            updRuibetsu.Append(" AND 共済目的コード = @共済目的コード ");
            updRuibetsu.Append(" AND 組合員等コード = @組合員等コード ");
            updRuibetsu.Append(" AND xmin           = @xmin ");

            List<NpgsqlParameter> updParams =
            [
                new ("組合等コード", sessionInfo.KumiaitoCd),
                new ("年産", sessionInfo.Nensan),
                new ("共済目的コード", sessionInfo.KyosaiMokutekiCd),
                new ("組合員等コード", sessionInfo.KumiaiintoCd),

                new ("加入形態", KanyuKeitai),
                new ("未加入フラグ", $"{(int)KanyuState}"),
                new ("交付申請者管理コード", string.IsNullOrEmpty(KoufuShinseishaKanriCd) ? DBNull.Value : KoufuShinseishaKanriCd),
                new ("共済金額改定時コード", string.IsNullOrEmpty(KyosaiKingakuKaiteijiCd) ? DBNull.Value : KyosaiKingakuKaiteijiCd),
                new ("継続特約フラグ", JidoKeizoku ? CoreConst.FLG_ON : CoreConst.FLG_OFF),
                new ("地域集団コード", string.IsNullOrEmpty(ChiikiSyudanCd) ? DBNull.Value : ChiikiSyudanCd),

                new ("共通申請等割引方法", KyotuShinseiWaribikiHouho),
                new ("割引割合", !WaribikiWariai.HasValue ? DBNull.Value : WaribikiWariai),
                new ("割引金額", !WaribikiKingaku.HasValue ? DBNull.Value : WaribikiKingaku),
                new ("割増割合", !WarimashiWariai.HasValue ? DBNull.Value : WarimashiWariai),
                new ("割増金額", !WarimashiKingaku.HasValue ? DBNull.Value : WarimashiKingaku),
                new ("共通申請等割引理由", string.IsNullOrEmpty(KyotuShinseitoWaribikiRiyu) ? DBNull.Value : KyotuShinseitoWaribikiRiyu),

                new ("システム日時", sysDateTime),
                new ("ユーザID", userId),
            ];
            NpgsqlParameter xminParam = new("xmin", NpgsqlTypes.NpgsqlDbType.Xid) { Value = Xmin };
            updParams.Add(xminParam);
            int cnt = dbContext.Database.ExecuteSqlRaw(updRuibetsu.ToString(), updParams);
            if (cnt == 0)
            {
                throw new DBConcurrencyException();
            }
            updCount += cnt;

            return updCount;
        }

        /// <summary>
        /// 共通申請割引方法取得
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="sessionInfo"></param>
        public void GetWaribikiHouhou(NskAppContext dbContext, D105070SessionInfo sessionInfo)
        {
            StringBuilder query = new();
            if (KyotuShinseiWaribikiHouho != "99")
            {
                // 選択値が「99：個人設定」以外の場合
                query.Append(" SELECT ");
                query.Append($"    割引割合           As \"{nameof(Query28Result.WaribikiWariai)}\" ");
                query.Append($"  , 割引金額           As \"{nameof(Query28Result.WaribikiKingaku)}\" ");
                query.Append($"  , 割増割合           As \"{nameof(Query28Result.WarimashiWariai)}\" ");
                query.Append($"  , 割増金額           As \"{nameof(Query28Result.WarimashiKingaku)}\" ");
                query.Append($"  , ''                 As \"{nameof(Query28Result.KyotuShinseitoWaribikiRiyu)}\" ");
                query.Append(" FROM ");
                query.Append("  m_10290_共通申請等割引方法 ");
                query.Append(" WHERE ");
                query.Append("      組合等コード   = @組合等コード ");
                query.Append("  AND 共済目的コード = @共済目的コード ");
                query.Append("  AND 年産           = @年産 ");
                query.Append("  AND 共通申請等割引方法コード = @共通申請等割引方法コード ");
            }
            else
            {
                // 選択値が「99：個人設定」の場合、
                query.Append(" SELECT ");
                query.Append($"    割引割合           As \"{nameof(Query28Result.WaribikiWariai)}\" ");
                query.Append($"  , 割引金額           As \"{nameof(Query28Result.WaribikiKingaku)}\" ");
                query.Append($"  , 割増割合           As \"{nameof(Query28Result.WarimashiWariai)}\" ");
                query.Append($"  , 割増金額           As \"{nameof(Query28Result.WarimashiKingaku)}\" ");
                query.Append($"  , 共通申請等割引理由 As \"{nameof(Query28Result.KyotuShinseitoWaribikiRiyu)}\" ");
                query.Append(" FROM ");
                query.Append("  t_11010_個人設定 ");
                query.Append(" WHERE ");
                query.Append("      組合等コード   = @組合等コード ");
                query.Append("  AND 共済目的コード = @共済目的コード ");
                query.Append("  AND 年産           = @年産 ");
                query.Append("  AND 組合員等コード = @組合員等コード ");
            }
            NpgsqlParameter[] queryParams =
            [
                new ("組合等コード", sessionInfo.KumiaitoCd),
                new ("年産", (short)sessionInfo.Nensan),
                new ("共済目的コード", sessionInfo.KyosaiMokutekiCd),
                new ("共通申請等割引方法コード", string.IsNullOrEmpty(KyotuShinseiWaribikiHouho) ? DBNull.Value : KyotuShinseiWaribikiHouho),
                new ("組合員等コード", sessionInfo.KumiaiintoCd),
            ];

            List<Query28Result> results = new();
            results.AddRange(dbContext.Database.SqlQueryRaw<Query28Result>(query.ToString(), queryParams));
            Query28Result? result = results.SingleOrDefault();
            WaribikiWariai = null;
            WaribikiKingaku = null;
            WarimashiWariai = null;
            WarimashiKingaku = null;
            KyotuShinseitoWaribikiRiyu = string.Empty;
            if (result is not null)
            {
                WaribikiWariai = result.WaribikiWariai;
                WaribikiKingaku = result.WaribikiKingaku;
                WarimashiWariai = result.WarimashiWariai;
                WarimashiKingaku = result.WarimashiKingaku;
                KyotuShinseitoWaribikiRiyu = result.KyotuShinseitoWaribikiRiyu;
            }
        }

        /// <summary>
        /// DB検索仕様書 No.28 検索結果
        /// </summary>
        private class Query28Result
        {
            /// <summary>割引割合</summary>
            public decimal? WaribikiWariai { get; set; }
            /// <summary>割引金額</summary>
            public decimal? WaribikiKingaku { get; set; }
            /// <summary>割増割合</summary>
            public decimal? WarimashiWariai { get; set; }
            /// <summary>割増金額</summary>
            public decimal? WarimashiKingaku { get; set; }
            /// <summary>共通申請等割引理由</summary>
            public string? KyotuShinseitoWaribikiRiyu { get; set; } = string.Empty;
        }
    }
}