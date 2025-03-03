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
//using NskWeb.Areas.F105.Models.D105070;
using NLog.Targets;
using System.Data;

namespace NskWeb.Areas.F105.Models.D105030
{
    /// <summary>
    /// 組合員等毎設定
    /// </summary>
    [Serializable]
    public class D105030KumiaiintoSettei
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

        /// <summary>xmin</summary>
        public uint? Xmin { get; set; }

        /// <summary>隠し項目：組合員等毎設定有無</summary>
        public bool Exists { get; set; } = false;

        /// <summary>加入形態ドロップダウンリスト選択値</summary>
        public List<SelectListItem> KanyuKeitaiList { get; set; } = new();


        /// <summary>
        /// ドロップダウンリスト初期化
        /// </summary>
        /// <param name="dbContext"></param>
        public void InitializeDropdonwList(NskAppContext dbContext)
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
        }

        /// <summary>
        /// 組合員等毎設定を取得する。
        /// DB検索仕様書 No.17
        /// </summary>
        /// <param name="dbContext"></param>
        public void GetKumiaitoSettei(NskAppContext dbContext, D105030SessionInfo sessionInfo)
        {
            // 組合員等毎設定を取得する。
            StringBuilder query = new();
            query.Append(" SELECT ");         
            query.Append($"   T1.未加入フラグ   As \"{nameof(D105030KumiaiintoSettei.KanyuState)}\" ");
            query.Append($"  ,T1.加入形態       As \"{nameof(D105030KumiaiintoSettei.KanyuKeitai)}\" ");
            query.Append($"  ,T1.継続特約フラグ As \"{nameof(D105030KumiaiintoSettei.JidoKeizoku)}\" ");
            query.Append($"  ,CASE WHEN T2.引受解除日付 IS NOT NULL THEN '1' ");
            query.Append($"   ELSE '0' END      As \"{nameof(D105030KumiaiintoSettei.Kaijo)}\" ");
            query.Append($"  ,T2.引受解除日付   As \"{nameof(D105030KumiaiintoSettei.KaijoHiduke)}\" ");
            query.Append($"  ,T1.地域集団コード As \"{nameof(D105030KumiaiintoSettei.ChiikiSyudanCd)}\" ");
            query.Append($"  ,T3.hojin_full_nm  As \"{nameof(D105030KumiaiintoSettei.ChiikiSyudanNm)}\" ");//氏名または法人名 As {nameof(D105030KumiaiintoSettei.ChiikiSyudanNm)} ");
            query.Append($"  ,cast('' || T1.xmin as integer) As \"{nameof(D105030KumiaiintoSettei.Xmin)}\" ");
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
                new NpgsqlParameter("組合等コード", sessionInfo.KumiaitoCd),
                new NpgsqlParameter("年産", sessionInfo.Nensan),
                new NpgsqlParameter("共済目的コード", sessionInfo.KyosaiMokutekiCd),
                new NpgsqlParameter("組合員等コード", sessionInfo.KumiaiintoCd),
            ];

            Query17Result? result = dbContext.Database.SqlQueryRaw<Query17Result>(query.ToString(), queryParams)?.SingleOrDefault();
            Exists = result is not null;

            if (Exists)
            {
                this.KanyuState = int.TryParse(result.KanyuState, out int state) ? (F105Const.KanyuStateType?)state : null;
                this.KanyuKeitai = result.KanyuKeitai;
                this.JidoKeizoku = int.TryParse(result.JidoKeizoku, out int jidoKeizoku) ? (jidoKeizoku == 1) : false;
                this.Kaijo = int.TryParse(result.Kaijo, out int kaijo) ? kaijo == 1 ? "解除" : "未解除" : "未解除";
                this.KaijoHiduke = result.KaijoHiduke;
                this.ChiikiSyudanCd = result.ChiikiSyudanCd;
                this.ChiikiSyudanNm = result.ChiikiSyudanNm;
            }
        }

        /// <summary>
        /// DB検索仕様書 No.17 検索結果
        /// </summary>
        private class Query17Result
        {
            /// <summary>加入状況</summary>
            public string? KanyuState { get; set; } = string.Empty;
            /// <summary>加入形態</summary>
            public string? KanyuKeitai { get; set; } = string.Empty;
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
        }

        /// <summary>
        /// 画面入力値をこのこのクラスに反映する
        /// </summary>
        /// <param name="src"></param>
        public void ApplyInput(D105030KumiaiintoSettei src)
        {
            this.MessageArea4 = src.MessageArea4;
            this.KanyuState = src.KanyuState;
            this.KanyuKeitai = src.KanyuKeitai;
            this.JidoKeizoku = src.JidoKeizoku; 
            this.Kaijo = src.Kaijo;
            this.KaijoHiduke = src.KaijoHiduke;
            this.ChiikiSyudanCd = src.ChiikiSyudanCd;
            this.ChiikiSyudanNm = src.ChiikiSyudanNm;
            this.Exists = src.Exists;
        }

        /// <summary>
        /// t_11020_個人設定解除の登録を行う。
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="sessionInfo"></param>
        /// <returns></returns>
        public int RegistKanyuKaijo(ref NskAppContext dbContext, D105030SessionInfo sessionInfo, string userId, DateTime sysDateTime)
        {
            // t_00010_引受回テーブルから引受回を取得する。
            short? maxHikiukeKai = dbContext.T00010引受回s.Where(m =>
                (m.組合等コード == sessionInfo.KumiaitoCd) &&
                (m.年産 == sessionInfo.Nensan) &&
                (m.共済目的コード == sessionInfo.KyosaiMokutekiCd))?.
                Max(m => m.引受回);

            T11020個人設定解除 cancelRec = new()
            {
                組合等コード = sessionInfo.KumiaitoCd,
                年産 = (short)sessionInfo.Nensan,
                共済目的コード = sessionInfo.KyosaiMokutekiCd,
                組合員等コード = sessionInfo.KumiaiintoCd,
                解除引受回 = (short)maxHikiukeKai,
                解除申出日付 = sysDateTime,
                引受解除日付 = null,
                引受解除返還賦課金額 = null,
                解除理由コード = null,
                登録日時 = sysDateTime,
                登録ユーザid = userId,
                更新日時 = sysDateTime,
                更新ユーザid = userId,
            };
            dbContext.T11020個人設定解除s.Add(cancelRec);

            return dbContext.SaveChanges();
        }

        /// <summary>
        /// t_11010_個人設定の登録を行う。
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="sessionInfo"></param>
        /// <param name="userId"></param>
        /// <param name="sysDateTime"></param>
        /// <returns></returns>
        public int RegistKojinSettei(ref NskAppContext dbContext, D105030SessionInfo sessionInfo, string userId, DateTime sysDateTime)
        {
            T11010個人設定 addRec = new()
            {
                組合等コード = sessionInfo.KumiaitoCd,
                年産 = (short)sessionInfo.Nensan,
                共済目的コード = sessionInfo.KyosaiMokutekiCd,
                組合員等コード = sessionInfo.KumiaiintoCd,
                加入形態 = KanyuKeitai,
                交付申請者管理コード = null,
                共済金額改定時コード = null,
                未加入フラグ = $"{(short)KanyuState}",
                継続特約フラグ = JidoKeizoku ? CoreConst.FLG_ON : CoreConst.FLG_OFF,
                地域集団コード = ChiikiSyudanCd,
                登録日時 = sysDateTime,
                登録ユーザid = userId,
                更新日時 = sysDateTime,
                更新ユーザid = userId
            };
            dbContext.T11010個人設定s.Add(addRec);

            return dbContext.SaveChanges();
        }

        /// <summary>
        /// t_11010_個人設定の更新を行う。
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="sessionInfo"></param>
        /// <param name="userId"></param>
        /// <param name="sysDateTime"></param>
        /// <returns></returns>
        public int UpdateKojinSettei(ref NskAppContext dbContext, D105030SessionInfo sessionInfo, string userId, DateTime sysDateTime)
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
            updRuibetsu.Append(" ,継続特約フラグ    = @継続特約フラグ ");
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
                new NpgsqlParameter("組合等コード", sessionInfo.KumiaitoCd),
                new NpgsqlParameter("年産", sessionInfo.Nensan),
                new NpgsqlParameter("共済目的コード", sessionInfo.KyosaiMokutekiCd),
                new NpgsqlParameter("組合員等コード", sessionInfo.KumiaiintoCd),

                new NpgsqlParameter("加入形態", KanyuKeitai),
                new NpgsqlParameter("未加入フラグ", $"{(int)KanyuState}"),
                new NpgsqlParameter("継続特約フラグ", JidoKeizoku ? CoreConst.FLG_ON : CoreConst.FLG_OFF),
                new NpgsqlParameter("システム日時", sysDateTime),
                new NpgsqlParameter("ユーザID", userId),
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
    }
}