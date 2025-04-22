using NskAppModelLibrary.Context;
using Microsoft.AspNetCore.Mvc.Rendering;
using NskAppModelLibrary.Models;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using System.Text;
using CoreLibrary.Core.Consts;
using System.Data;
using NskWeb.Common.Models;
using NLog.Targets;

namespace NskWeb.Areas.F105.Models.D105070
{
    /// <summary>
    /// 類別設定
    /// </summary>
    [Serializable]
    public class D105070RuibetsuSettei : BasePager<D105070RuibetsuSetteiRecord>
    {
        /// <summary>メッセージエリア５</summary>
        public string MessageArea5 { get; set; } = string.Empty;

        /// <summary>引受区分ドロップダウンリスト選択値</summary>
        public List<SelectListItem> RuiKbnLists { get; set; } = new();
        /// <summary>引受方式ドロップダウンリスト選択値</summary>
        public List<SelectListItem> HikiukeHoushikiLists { get; set; } = new();
        /// <summary>補償割合ドロップダウンリスト選択値</summary>
        public List<SelectListItem> HoshoWariaiLists { get; set; } = new();
        /// <summary>一筆半損特約ドロップダウンリスト選択値</summary>
        public List<SelectListItem> IppitsuHansonTokuyakuLists { get; set; } = new();
        /// <summary>担い手区分ドロップダウンリスト選択値</summary>
        public List<SelectListItem> NinaiteKbnLists { get; set; } = new();
        /// <summary>収穫量確認方法ドロップダウンリスト選択値</summary>
        public List<SelectListItem> SyukakuryoKakuninHouhouLists { get; set; } = new();

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public D105070RuibetsuSettei()
        {
        }

        /// <summary>
        /// ドロップダウンリスト初期化
        /// </summary>
        /// <param name="dbContext">DBコンテキスト</param>
        /// <param name="session">セッション情報</param>
        public void InitializeDropdonwList(NskAppContext dbContext, BaseSessionInfo session)
        {
            D105070SessionInfo sessionInfo = (D105070SessionInfo)session;

            // ２．３．１２．[類区分]ドロップダウンリスト項目を取得する。
            // 	(1) m_00020_類名称テーブルより、類区分、類名称を取得する。
            // 	(2) 取得した結果をドロップダウンリストの項目として設定する。
            RuiKbnLists = new();
            RuiKbnLists.AddRange(dbContext.M00020類名称s.Where(m =>
                (m.共済目的コード == sessionInfo.KyosaiMokutekiCd))?.
                OrderBy(m => m.類区分).
                Select(m => new SelectListItem($"{m.類区分} {m.類名称}", $"{m.類区分}")));

            // ２．３．１３．[引受方式]ドロップダウンリスト項目を取得する。		
            // 	(1) m_10170_選択引受方式テーブルより、引受方式、引受方式名称を取得する。
            // 	(2) 取得した結果をドロップダウンリストの項目として設定する。
            HikiukeHoushikiLists = new();
            HikiukeHoushikiLists.AddRange(dbContext.M10170選択引受方式s.Where(m =>
                (m.組合等コード == sessionInfo.KumiaitoCd) &&
                (m.共済目的コード == sessionInfo.KyosaiMokutekiCd))?.
                OrderBy(m => m.引受方式).
                Select(m => new SelectListItem($"{m.引受方式} {m.引受方式名称}", $"{m.引受方式}")));

            // ２．３．１４．[補償割合]ドロップダウンリスト項目を取得する。	
            // 	(1) m_20030_補償割合名称テーブルより、補償割合コード、補償割合短縮名称を取得する。
            // 	(2) 取得した結果をドロップダウンリストの項目として設定する。
            HoshoWariaiLists = new();
            HoshoWariaiLists.AddRange(dbContext.M20030補償割合名称s.
                OrderBy(m => m.補償割合コード).
                Select(m => new SelectListItem($"{m.補償割合コード} {m.補償割合短縮名称}", $"{m.補償割合コード}")));

            // ２．３．１５．[一筆半損特約]ドロップダウンリスト項目を取得する。	
            // 	(1) m_10100_特約区分名称テーブルより、特約区分、特約区分名称を取得する。
            // 	(2) 取得した結果をドロップダウンリストの項目として設定する。
            IppitsuHansonTokuyakuLists = new();
            IppitsuHansonTokuyakuLists.AddRange(dbContext.M10100特約区分名称s.
                OrderBy(m => m.特約区分).
                Select(m => new SelectListItem($"{m.特約区分} {m.特約区分名称}", $"{m.特約区分}")));

            // ２．３．１６．[担い手]ドロップダウンリスト項目を取得する。		
            // 	(1) m_00180_担手農家区分名称テーブルより、担手農家区分、担手農家区分名称を取得する。
            // 	(2) 取得した結果をドロップダウンリストの項目として設定する。
            NinaiteKbnLists = new();
            NinaiteKbnLists.AddRange(dbContext.M00180担手農家区分名称s.
                OrderBy(m => m.担手農家区分).
                Select(m => new SelectListItem($"{m.担手農家区分} {m.担手農家区分名称}", $"{m.担手農家区分}")));

            // ２．３．１７．[収穫量確認方法]ドロップダウンリスト項目を取得する。		
            // (1) m_00070_収穫量確認方法名称テーブルから収穫量確認方法、収穫量確認方法名称を取得する。
            // (2) 取得した結果をドロップダウンリストの項目として設定する。
            SyukakuryoKakuninHouhouLists = new();
            SyukakuryoKakuninHouhouLists.AddRange(dbContext.M00070収穫量確認方法名称s.
                OrderBy(m => m.収穫量確認方法).
                Select(m => new SelectListItem($"{m.収穫量確認方法} {m.収穫量確認方法名称}", $"{m.収穫量確認方法}")));
        }


        /// <summary>
        /// 類別設定を取得する
        /// </summary>
        /// <param name="dbContext">DBコンテキスト</param>
        /// <param name="session">セッション情報</param>
        /// <returns>検索情報</returns>
        public override List<D105070RuibetsuSetteiRecord> GetResult(NskAppContext dbContext, BaseSessionInfo session)
        {
            D105070SessionInfo sessionInfo = (D105070SessionInfo)session;

            List<D105070RuibetsuSetteiRecord> records = new();

            records.AddRange(dbContext.T11030個人設定類s.Where(x =>
                (x.組合等コード == sessionInfo.KumiaitoCd) &&
                (x.年産 == sessionInfo.Nensan) &&
                (x.共済目的コード == sessionInfo.KyosaiMokutekiCd) &&
                (x.組合員等コード == sessionInfo.KumiaiintoCd))?.
                Select(x => new D105070RuibetsuSetteiRecord()
                {
                    RuiKbn = x.類区分,
                    HikiukeHoushiki = x.引受方式,
                    HoshoWariai = x.補償割合コード,
                    FuhoWariai = x.付保割合,
                    IppitsuHansonTokuyaku = x.特約区分,
                    NinaiteKbn = x.担手農家区分,
                    EinoShiharaiIgai = x.営農対象外フラグ == CoreConst.FLG_ON,
                    SyukakuryoKakuninHouhou = x.収穫量確認方法,
                    ZensousaiKijunTansyu = x.全相殺基準単収,
                    Xmin = x.Xmin
                }));

            return records;
        }

        /// <summary>
        /// 画面入力値をこのこのクラスに反映する
        /// </summary>
        /// <param name="src">コピー元</param>
        public void ApplyInput(D105070RuibetsuSettei src)
        {
            this.MessageArea5 = src.MessageArea5;
            this.DispRecords = src.DispRecords;
        }

        /// <summary>
        /// t_11030_個人設定類の対象レコードを削除する。
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="sessionInfo"></param>
        /// <param name="delRecords"></param>
        /// <returns></returns>
        public int DeleteRuibetsu(ref NskAppContext dbContext, D105070SessionInfo sessionInfo, ref List<D105070RuibetsuSetteiRecord> delRecords)
        {
            int delCount = 0;

            // t_11030_個人設定類の対象レコードを削除する。
            StringBuilder delKojinSetteiRui = new();
            delKojinSetteiRui.Append("DELETE FROM t_11030_個人設定類 ");
            delKojinSetteiRui.Append("WHERE ");
            delKojinSetteiRui.Append("     組合等コード   = @組合等コード ");
            delKojinSetteiRui.Append(" AND 年産           = @年産 ");
            delKojinSetteiRui.Append(" AND 共済目的コード = @共済目的コード ");
            delKojinSetteiRui.Append(" AND 組合員等コード = @組合員等コード ");
            delKojinSetteiRui.Append(" AND 類区分         = @類区分 ");
            delKojinSetteiRui.Append(" AND 引受区分       = @引受区分 ");
            delKojinSetteiRui.Append(" AND xmin           = @xmin ");

            foreach (D105070RuibetsuSetteiRecord target in delRecords)
            {
                if (!target.Xmin.HasValue)
                {
                    // xmin nullは処理対象外
                    continue;
                }
                // 引受区分取得
                string hikiukeKbn = GetHikiukeKbn(dbContext, sessionInfo.KyosaiMokutekiCd, target.RuiKbn);

                List<NpgsqlParameter> delParams =
                [
                    new("組合等コード", sessionInfo.KumiaitoCd),
                    new("年産", sessionInfo.Nensan),
                    new("共済目的コード", sessionInfo.KyosaiMokutekiCd),
                    new("組合員等コード", sessionInfo.KumiaiintoCd),
                    new("類区分", string.IsNullOrEmpty(target.RuiKbn) ? DBNull.Value : target.RuiKbn),
                    new("引受区分", string.IsNullOrEmpty(hikiukeKbn) ? DBNull.Value : hikiukeKbn),
                ];
                NpgsqlParameter xminParam = new("xmin", NpgsqlTypes.NpgsqlDbType.Xid) { Value = target.Xmin };
                delParams.Add(xminParam);
                int cnt = dbContext.Database.ExecuteSqlRaw(delKojinSetteiRui.ToString(), delParams);
                if (cnt == 0)
                {
                    throw new DBConcurrencyException();
                }
                delCount += cnt;
            }

            // t_11050_個人用途別の対象レコードを削除する。
            StringBuilder delKojinYoutobetsu = new();
            delKojinYoutobetsu.Append("DELETE FROM t_11050_個人用途別 ");
            delKojinYoutobetsu.Append("WHERE ");
            delKojinYoutobetsu.Append("     組合等コード   = @組合等コード ");
            delKojinYoutobetsu.Append(" AND 年産           = @年産 ");
            delKojinYoutobetsu.Append(" AND 共済目的コード = @共済目的コード ");
            delKojinYoutobetsu.Append(" AND 組合員等コード = @組合員等コード ");
            delKojinYoutobetsu.Append(" AND 類区分         = @類区分 ");

            foreach (D105070RuibetsuSetteiRecord target in delRecords)
            {
                if (!target.Xmin.HasValue)
                {
                    // xmin nullは処理対象外
                    continue;
                }
                List<NpgsqlParameter> delParams =
                [
                    new("組合等コード", sessionInfo.KumiaitoCd),
                    new("年産", sessionInfo.Nensan),
                    new("共済目的コード", sessionInfo.KyosaiMokutekiCd),
                    new("組合員等コード", sessionInfo.KumiaiintoCd),
                    new("類区分", string.IsNullOrEmpty(target.RuiKbn) ? DBNull.Value : target.RuiKbn),
                ];
                delCount += dbContext.Database.ExecuteSqlRaw(delKojinYoutobetsu.ToString(), delParams); ;
            }

            return delCount;
        }

        /// <summary>
        /// t_11030_個人設定類の対象レコードを更新する。
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="sessionInfo"></param>
        /// <param name="userId"></param>
        /// <param name="sysDateTime"></param>
        /// <param name="ruibetsuUpdRecords"></param>
        /// <returns></returns>
        public int UpdateRuibetsu(ref NskAppContext dbContext, D105070SessionInfo sessionInfo, string userId, DateTime sysDateTime, ref List<D105070RuibetsuSetteiRecord> ruibetsuUpdRecords)
        {
            int updCount = 0;

            // t_11030_個人設定類の対象レコードを更新する。
            StringBuilder updRuibetsu = new();
            updRuibetsu.Append("UPDATE t_11030_個人設定類 SET ");
            updRuibetsu.Append("  引受方式          = @引受方式 ");
            updRuibetsu.Append(" ,特約区分          = @特約区分 ");
            updRuibetsu.Append(" ,補償割合コード    = @補償割合コード ");
            updRuibetsu.Append(" ,付保割合          = @付保割合 ");
            updRuibetsu.Append(" ,種類区分          = @種類区分 ");
            updRuibetsu.Append(" ,共済金額選択順位  = @共済金額選択順位 ");
            updRuibetsu.Append(" ,収穫量確認方法    = @収穫量確認方法 ");
            updRuibetsu.Append(" ,全相殺基準単収    = @全相殺基準単収 ");
            updRuibetsu.Append(" ,更新日時          = @システム日時 ");
            updRuibetsu.Append(" ,更新ユーザid      = @ユーザID ");
            updRuibetsu.Append("WHERE ");
            updRuibetsu.Append("     組合等コード   = @組合等コード ");
            updRuibetsu.Append(" AND 年産           = @年産 ");
            updRuibetsu.Append(" AND 共済目的コード = @共済目的コード ");
            updRuibetsu.Append(" AND 組合員等コード = @組合員等コード ");
            updRuibetsu.Append(" AND 類区分         = @類区分 ");
            updRuibetsu.Append(" AND 引受区分       = @引受区分 ");
            updRuibetsu.Append(" AND xmin           = @xmin ");

            foreach (D105070RuibetsuSetteiRecord target in ruibetsuUpdRecords)
            {
                if (!target.Xmin.HasValue)
                {
                    // xmin nullは処理対象外
                    continue;
                }
                // 引受区分取得
                string hikiukeKbn = GetHikiukeKbn(dbContext, sessionInfo.KyosaiMokutekiCd, target.RuiKbn);

                List<NpgsqlParameter> updParams =
                [
                    new("組合等コード", sessionInfo.KumiaitoCd),
                    new("年産", sessionInfo.Nensan),
                    new("共済目的コード", sessionInfo.KyosaiMokutekiCd),
                    new("組合員等コード", sessionInfo.KumiaiintoCd),

                    new("類区分", string.IsNullOrEmpty(target.RuiKbn) ? DBNull.Value : target.RuiKbn),
                    new("引受区分", string.IsNullOrEmpty(hikiukeKbn) ? DBNull.Value : hikiukeKbn),
                    new("引受方式", string.IsNullOrEmpty(target.HikiukeHoushiki) ? DBNull.Value : target.HikiukeHoushiki),
                    new("特約区分", string.IsNullOrEmpty(target.IppitsuHansonTokuyaku) ? DBNull.Value : target.IppitsuHansonTokuyaku),
                    new("補償割合コード", string.IsNullOrEmpty(target.HoshoWariai) ? DBNull.Value : target.HoshoWariai),
                    new("種類区分", DBNull.Value),
                    new("付保割合", target.FuhoWariai.HasValue ? target.FuhoWariai : DBNull.Value),
                    new("収穫量確認方法", string.IsNullOrEmpty(target.SyukakuryoKakuninHouhou) ? DBNull.Value : target.SyukakuryoKakuninHouhou),
                    new("全相殺基準単収", target.ZensousaiKijunTansyu.HasValue ? target.ZensousaiKijunTansyu : DBNull.Value),
                    new("システム日時", sysDateTime),
                    new("ユーザID", userId),
                ];
                NpgsqlParameter xminParam = new("xmin", NpgsqlTypes.NpgsqlDbType.Xid) { Value = target.Xmin };
                updParams.Add(xminParam);
                int cnt = dbContext.Database.ExecuteSqlRaw(updRuibetsu.ToString(), updParams);
                if (cnt == 0)
                {
                    throw new DBConcurrencyException();
                }
                updCount += cnt;
            }

            // t_11050_個人用途別の対象レコードを更新する。
            StringBuilder updKojinYoutobetsu = new();
            updKojinYoutobetsu.Append("UPDATE t_11050_個人用途別 SET ");
            updKojinYoutobetsu.Append("  共済金額選択順位  = @共済金額選択順位 ");
            updKojinYoutobetsu.Append(" ,更新日時          = @システム日時 ");
            updKojinYoutobetsu.Append(" ,更新ユーザid      = @ユーザID ");
            updKojinYoutobetsu.Append("WHERE ");
            updKojinYoutobetsu.Append("     組合等コード   = @組合等コード ");
            updKojinYoutobetsu.Append(" AND 年産           = @年産 ");
            updKojinYoutobetsu.Append(" AND 共済目的コード = @共済目的コード ");
            updKojinYoutobetsu.Append(" AND 組合員等コード = @組合員等コード ");
            updKojinYoutobetsu.Append(" AND 類区分         = @類区分 ");

            foreach (D105070RuibetsuSetteiRecord target in ruibetsuUpdRecords)
            {
                if (!target.Xmin.HasValue)
                {
                    // xmin nullは処理対象外
                    continue;
                }

                List<NpgsqlParameter> updParams =
                [
                    new("組合等コード", sessionInfo.KumiaitoCd),
                    new("年産", sessionInfo.Nensan),
                    new("共済目的コード", sessionInfo.KyosaiMokutekiCd),
                    new("組合員等コード", sessionInfo.KumiaiintoCd),
                    new("類区分", string.IsNullOrEmpty(target.RuiKbn) ? DBNull.Value : target.RuiKbn),

                    new("共済金額選択順位", DBNull.Value),
                    new("システム日時", sysDateTime),
                    new("ユーザID", userId),
                ];
                updCount += dbContext.Database.ExecuteSqlRaw(updKojinYoutobetsu.ToString(), updParams); ;
            }
            return updCount;
        }

        /// <summary>
        /// t_11030_個人設定類の対象レコードを登録する。
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="sessionInfo"></param>
        /// <param name="userId"></param>
        /// <param name="sysDateTime"></param>
        /// <param name="ruibetsuAddRecords"></param>
        /// <returns></returns>
        public int AppendRuibetsu(ref NskAppContext dbContext, D105070SessionInfo sessionInfo, string userId, DateTime sysDateTime, ref List<D105070RuibetsuSetteiRecord> ruibetsuAddRecords)
        {
            int addCount = 0;

            // t_11030_個人設定類の対象レコードを登録する。
            List<T11030個人設定類> addRuibetsuRecs = new();
            foreach (D105070RuibetsuSetteiRecord target in ruibetsuAddRecords)
            {
                // 引受区分取得
                string hikiukeKbn = GetHikiukeKbn(dbContext, sessionInfo.KyosaiMokutekiCd, target.RuiKbn);

                T11030個人設定類 addRec = new()
                {
                    組合等コード = sessionInfo.KumiaitoCd,
                    年産 = (short)sessionInfo.Nensan,
                    共済目的コード = sessionInfo.KyosaiMokutekiCd,
                    組合員等コード = sessionInfo.KumiaiintoCd,
                    類区分 = target.RuiKbn,
                    引受区分 = hikiukeKbn,
                    引受方式 = target.HikiukeHoushiki,
                    特約区分 = target.IppitsuHansonTokuyaku,
                    補償割合コード = target.HoshoWariai,
                    付保割合 = null,
                    種類区分 = null,
                    作付時期 = null,
                    田畑区分 = null,
                    共済金額選択順位 = null,
                    収穫量確認方法 = target.SyukakuryoKakuninHouhou,
                    全相殺基準単収 = target.ZensousaiKijunTansyu,
                    営農対象外フラグ = target.EinoShiharaiIgai ? CoreConst.FLG_ON : CoreConst.FLG_OFF,
                    担手農家区分 = target.NinaiteKbn,
                    全相殺受託者等名称 = null,
                    備考 = null,
                    登録日時 = sysDateTime,
                    登録ユーザid = userId,
                    更新日時 = sysDateTime,
                    更新ユーザid = userId,
                };
                addRuibetsuRecs.Add(addRec);
            }
            dbContext.T11030個人設定類s.AddRange(addRuibetsuRecs);
            dbContext.SaveChanges();
            addCount += addRuibetsuRecs.Count;

            return addCount;
        }

        /// <summary>
        /// 更新対象レコード取得
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="session"></param>
        /// <returns></returns>
        public override List<D105070RuibetsuSetteiRecord> GetUpdateRecs(ref NskAppContext dbContext, BaseSessionInfo session)
        {
            D105070SessionInfo sessionInfo = (D105070SessionInfo)session;

            List<D105070RuibetsuSetteiRecord> updRecs = new();

            // 検索結果取得
            List<D105070RuibetsuSetteiRecord> dbResults = GetResult(dbContext, sessionInfo);

            // 検索結果と画面入力値を比較
            foreach (D105070RuibetsuSetteiRecord dispRec in DispRecords)
            {
                // 追加行、削除行以外を対象とする
                if (dispRec is BasePagerRecord pagerRec && !pagerRec.IsNewRec && !pagerRec.IsDelRec)
                {
                    D105070RuibetsuSetteiRecord dbRec = dbResults.SingleOrDefault(x =>
                        (x.RuiKbn == dispRec.RuiKbn)
                    );

                    // DB検索結果と比較し差分ありの場合、更新対象とする
                    if ((dbRec is not null) && !dispRec.Compare(dbRec))
                    {
                        // 画面入力を更新対象として追加
                        updRecs.Add(dispRec);
                    }
                }
            }

            return updRecs;
        }

        /// <summary>
        /// 類区分に応じた引受区分を取得する。
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="ruiKbn"></param>
        /// <returns></returns>
        private string GetHikiukeKbn(NskAppContext dbContext, string kyosaiMokutekiCd, string ruiKbn)
        {
            string hikiukeKbn = string.Empty;
            M00020類名称 ruiMst = dbContext.M00020類名称s.SingleOrDefault(x =>
                (x.共済目的コード == kyosaiMokutekiCd) &&
                (x.類区分 == ruiKbn));

            if (ruiMst is not null)
            {
                M10090引受区分名称 hikiukeMst = dbContext.M10090引受区分名称s.SingleOrDefault(x =>
                    (x.共済目的コード == ruiMst.共済目的コード) &&
                    (x.種類区分 == ruiMst.種類区分) &&
                    (x.作付時期 == ruiMst.作付時期)
                );

                hikiukeKbn = hikiukeMst?.引受区分 ?? string.Empty;
            }

            return hikiukeKbn;
        }

        /// <summary>
        /// 類区分ドロップダウンリスト選択値から名称を取得する
        /// </summary>
        /// <param name="ruiKbn">類区分（選択値）</param>
        /// <returns></returns>
        public string GetRuiKbnNm(string ruiKbn)
        {
            SelectListItem item = RuiKbnLists.FirstOrDefault(x => x.Value == ruiKbn);
            return item?.Text.Split(" ")[1] ?? string.Empty;
        }

        /// <summary>
        /// 引受方式ドロップダウンリスト選択値から名称を取得する
        /// </summary>
        /// <param name="hikiukehoushikiCd">引受方式（選択値）</param>
        /// <returns></returns>
        public string GetHikiukeHoushikiNm(string hikiukehoushikiCd)
        {
            SelectListItem item = HikiukeHoushikiLists.FirstOrDefault(x => x.Value == hikiukehoushikiCd);
            return item?.Text.Split(" ")[1] ?? string.Empty;
        }

        /// <summary>
        /// [選択共済金額]ドロップダウンリスト項目を取得する。
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="sessionInfo"></param>
        public void UpdateDropdownListAll(NskAppContext dbContext, D105070SessionInfo sessionInfo)
        {
            // ※類別設定の件数分繰り返す
            foreach (D105070RuibetsuSetteiRecord record in DispRecords)
            {
                // 引受区分取得
                string hikiukeKbn = GetHikiukeKbn(dbContext, sessionInfo.KyosaiMokutekiCd, record.RuiKbn);

                record.InitializeDropdonwList(dbContext, sessionInfo, hikiukeKbn);
            }
        }
    }
}