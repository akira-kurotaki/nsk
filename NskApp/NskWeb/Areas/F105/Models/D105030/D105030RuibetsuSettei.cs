using NskAppModelLibrary.Context;
using Microsoft.AspNetCore.Mvc.Rendering;
using NskAppModelLibrary.Models;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using System.Text;
using NskWeb.Areas.F105.Consts;
using System.Data;

namespace NskWeb.Areas.F105.Models.D105030
{
    /// <summary>
    /// 類別設定
    /// </summary>
    [Serializable]
    public class D105030RuibetsuSettei : D105030Pager<D105030RuibetsuSetteiRecord>
    {
        /// <summary>メッセージエリア５</summary>
        public string MessageArea5 { get; set; } = string.Empty;
        /// <summary>メッセージエリア６</summary>
        public string MessageArea6 { get; set; } = string.Empty;


        /// <summary>引受区分ドロップダウンリスト選択値</summary>
        public List<SelectListItem> HikiukeKbnList { get; set; } = new();
        /// <summary>引受方式ドロップダウンリスト選択値</summary>
        public List<SelectListItem> HikiukeHoushikiList { get; set; } = new();
        /// <summary>補償割合ドロップダウンリスト選択値</summary>
        public List<SelectListItem> HoshoWariaiList { get; set; } = new();
        /// <summary>一筆半損特約ドロップダウンリスト選択値</summary>
        public List<SelectListItem> IppitsuHansonTokuyakuList { get; set; } = new();
        /// <summary>選択共済金額ドロップダウンリスト選択値</summary>
        public List<SelectListItem> SelectKyosaiKingakuList { get; set; } = new();
        /// <summary>危険段階区分ドロップダウンリスト選択値</summary>
        public List<SelectListItem> KikenDankaiKbnList { get; set; } = new();
        /// <summary>収穫量確認方法ドロップダウンリスト選択値</summary>
        public List<SelectListItem> SyukakuryoKakuninHouhouList { get; set; } = new();

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public D105030RuibetsuSettei()
        {
        }

        /// <summary>
        /// ドロップダウンリスト初期化
        /// </summary>
        /// <param name="dbContext"></param>
        public void InitializeDropdonwList(NskAppContext dbContext, D105030SessionInfo sessionInfo)
        {
            // ２．１２．[引受区分] ドロップダウンリスト項目を取得する。		
            // 	(1) m_10090_引受区分名称テーブルより、共済目的コードが「引受区分（水稲）」に一致する引受区分、引受区分名称を取得する。
            // 	(2) 取得した結果をドロップダウンリストの項目として設定する。
            HikiukeKbnList = new();
            HikiukeKbnList.AddRange(dbContext.M10090引受区分名称s.Where(m =>
                (m.共済目的コード == sessionInfo.KyosaiMokutekiCd))?.
                OrderBy(m => m.引受区分).
                Select(m => new SelectListItem($"{m.引受区分} {m.引受区分名称}", $"{m.引受区分}")));

            // ２．１３．[引受方式] ドロップダウンリスト項目を取得する。		
            // 	(1) m_10080_引受方式名称テーブルより、引受方式、引受方式短縮名を取得する。
            // 	(2) 取得した結果をドロップダウンリストの項目として設定する。
            HikiukeHoushikiList = new();
            string[] hikiukeHoushikis = ["2", "3", "5", "6"];
            HikiukeHoushikiList.AddRange(dbContext.M10080引受方式名称s.Where(m =>
                (hikiukeHoushikis.Contains(m.引受方式)))?.
                OrderBy(m => m.引受方式).
                Select(m => new SelectListItem($"{m.引受方式} {m.引受方式短縮名}", $"{m.引受方式}")));

            // ２．１４．[補償割合] ドロップダウンリスト項目を取得する。		
            // 	(1) m_20030_補償割合名称テーブルより、補償割合コード、補償割合短縮名称を取得する。
            // 	(2) 取得した結果をドロップダウンリストの項目として設定する。
            HoshoWariaiList = new();
            HoshoWariaiList.AddRange(dbContext.M20030補償割合名称s.
                OrderBy(m => m.補償割合コード).
                Select(m => new SelectListItem($"{m.補償割合コード} {m.補償割合短縮名称}", $"{m.補償割合コード}")));

            // ２．１５．[特約] ドロップダウンリスト項目を取得する。		
            // 	(1) m_10100_特約区分名称テーブルより、特約区分、特約区分名称を取得する。
            // 	(2) 取得した結果をドロップダウンリストの項目として設定する。
            IppitsuHansonTokuyakuList = new();
            IppitsuHansonTokuyakuList.AddRange(dbContext.M10100特約区分名称s.
                OrderBy(m => m.特約区分).
                Select(m => new SelectListItem($"{m.特約区分} {m.特約区分名称}", $"{m.特約区分}")));

            // ２．１６．[選択共済金額] ドロップダウンリスト項目を取得する。		
            // 	(1) m_10210_単当共済金額用途テーブル、単当共済金額順位、単当共済金額を取得する。
            // 	(2) 取得した結果をドロップダウンリストの項目として設定する。
            //      m_10210_単当共済金額用途.課税単価区分 = 0、かつm_10210_単当共済金額用途.推奨フラグ = 1のレコードを順位0として
            //      取得したデータを順位0となるようにドロップダウンリストに設定。
            //      それ以降にm_10210_単当共済金額用途.課税単価区分 = 0のレコードを追加で設定
            SelectKyosaiKingakuList = new();
            SelectKyosaiKingakuList.AddRange(dbContext.M10210単当共済金額用途s.Where(m =>
                (m.組合等コード == sessionInfo.KumiaitoCd) &&
                (m.年産 == sessionInfo.Nensan) &&
                (m.共済目的コード == sessionInfo.KyosaiMokutekiCd) &&
                (m.課税単価区分 == "0") &&
                (m.推奨フラグ == "1"))?.
                OrderBy(m => m.単当共済金額順位).
                Select(m => new SelectListItem($"0 {m.単当共済金額}", $"{m.単当共済金額順位}")));
            SelectKyosaiKingakuList.AddRange(dbContext.M10210単当共済金額用途s.Where(m =>
                (m.組合等コード == sessionInfo.KumiaitoCd) &&
                (m.年産 == sessionInfo.Nensan) &&
                (m.共済目的コード == sessionInfo.KyosaiMokutekiCd))?.
                OrderBy(m => m.単当共済金額順位).
                Select(m => new SelectListItem($"{m.単当共済金額順位} {m.単当共済金額}", $"{m.単当共済金額順位}")));
            //TODO: 種類区分
            //(m.共済目的コード == sessionInfo.KyosaiMokutekiCd) &&
            //(m.種類区分 == sessionInfo.SyuruiKbn))?.

            // ２．１７．[危険段階区分] ドロップダウンリスト項目を取得する。		
            // (1) m_10230_危険段階テーブルより、危険段階区分を取得する。
            // (2) 取得した結果をドロップダウンリストの項目として設定する。
            KikenDankaiKbnList = new();
            KikenDankaiKbnList.AddRange(dbContext.M10230危険段階s.Where(m =>
                (m.組合等コード == sessionInfo.KumiaitoCd) &&
                (m.年産 == sessionInfo.Nensan) &&
                (m.共済目的コード == sessionInfo.KyosaiMokutekiCd) &&
                (m.合併時識別コード == (sessionInfo.GappeiShikibetsuCd ?? string.Empty)))?.
                OrderBy(m => m.危険段階区分).
                Select(m => new SelectListItem($"{m.危険段階区分}", $"{m.危険段階区分}")));

            // ２．１９．[収穫量確認方法] ドロップダウンリスト項目を取得する。		
            // (1) m_00070_収穫量確認方法名称テーブルから収穫量確認方法、収穫量確認方法名称を取得する。
            // (2) 取得した結果をドロップダウンリストの項目として設定する。
            SyukakuryoKakuninHouhouList = new();
            SyukakuryoKakuninHouhouList.AddRange(dbContext.M00070収穫量確認方法名称s.
                OrderBy(m => m.収穫量確認方法).
                Select(m => new SelectListItem($"{m.収穫量確認方法} {m.収穫量確認方法名称}", $"{m.収穫量確認方法}")));
        }


        /// <summary>
        /// 類別設定を取得する
        /// </summary>
        /// <param name="dbContext">DBコンテキスト</param>
        /// <param name="sessionInfo">セッション情報</param>
        /// <returns>検索情報</returns>
        public override List<D105030RuibetsuSetteiRecord> GetResult(NskAppContext dbContext, D105030SessionInfo sessionInfo)
        {
            StringBuilder query = new();
            query.Append(" SELECT ");
            query.Append($"   T1.引受区分             As \"{nameof(D105030RuibetsuSetteiRecord.HikiukeKbn)}\" ");  
            query.Append($"  ,T1.引受方式             As \"{nameof(D105030RuibetsuSetteiRecord.HikiukeHoushiki)}\" ");
            query.Append($"  ,T1.補償割合コード       As \"{nameof(D105030RuibetsuSetteiRecord.HoshoWariai)}\" ");   
            query.Append($"  ,T1.付保割合             As \"{nameof(D105030RuibetsuSetteiRecord.FuhoWariai)}\" ");
            query.Append($"  ,T1.特約区分             As \"{nameof(D105030RuibetsuSetteiRecord.IppitsuHansonTokuyaku)}\" ");
            query.Append($"  ,T1.共済金額選択順位     As \"{nameof(D105030RuibetsuSetteiRecord.SelectKyosaiKingaku)}\" ");
            query.Append($"  ,T2.個人危険段階区分     As \"{nameof(D105030RuibetsuSetteiRecord.KikenDankaiKbn)}\" ");
            query.Append($"  ,T1.収穫量確認方法       As \"{nameof(D105030RuibetsuSetteiRecord.SyukakuryoKakuninHouhou)}\" ");
            query.Append($"  ,T1.全相殺基準単収       As \"{nameof(D105030RuibetsuSetteiRecord.ZensousaiKijunTansyu)}\" ");
            query.Append($"  ,cast('' || T1.xmin as integer) As \"{nameof(D105030RuibetsuSetteiRecord.Xmin)}\" ");
            query.Append($" FROM t_11030_個人設定類 T1 ");   
            query.Append(" LEFT OUTER JOIN t_11040_個人料率 T2 ");
            query.Append(" ON    T1.組合等コード   = T2.組合等コード ");
            query.Append("  AND  T1.年産           = T2.年産 ");
            query.Append("  AND  T1.共済目的コード = T2.共済目的コード ");
            query.Append("  AND  T1.組合員等コード = T2.組合員等コード ");
            query.Append("  AND  T1.類区分         = T2.類区分 ");
            query.Append(" WHERE ");
            query.Append("       T1.組合等コード   = @組合等コード ");
            query.Append("  AND  T1.年産           = @年産 ");
            query.Append("  AND  T1.共済目的コード = @共済目的コード ");
            query.Append("  AND  T1.組合員等コード = @組合員等コード ");

            NpgsqlParameter[] queryParams =
            [
                new NpgsqlParameter("組合等コード", sessionInfo.KumiaitoCd),
                new NpgsqlParameter("年産", sessionInfo.Nensan),
                new NpgsqlParameter("共済目的コード", sessionInfo.KyosaiMokutekiCd),
                new NpgsqlParameter("組合員等コード", sessionInfo.KumiaiintoCd),
            ];

            List<D105030RuibetsuSetteiRecord> records = new();
            records.AddRange(dbContext.Database.SqlQueryRaw<D105030RuibetsuSetteiRecord>(query.ToString(), queryParams));

            return records;
        }

        /// <summary>
        /// 画面入力値をこのこのクラスに反映する
        /// </summary>
        /// <param name="src">コピー元</param>
        public void ApplyInput(D105030RuibetsuSettei src)
        {
            this.MessageArea5 = src.MessageArea5;
            this.MessageArea6 = src.MessageArea6;
            this.DispRecords = src.DispRecords;
        }

        /// <summary>
        /// t_11030_個人設定類の対象レコードを削除する。
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="sessionInfo"></param>
        /// <param name="delRecords"></param>
        /// <returns></returns>
        public int DeleteRuibetsu(ref NskAppContext dbContext, D105030SessionInfo sessionInfo, ref List<D105030RuibetsuSetteiRecord> delRecords)
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

            foreach (D105030RuibetsuSetteiRecord target in delRecords)
            {
                if (!target.Xmin.HasValue)
                {
                    // xmin nullは処理対象外
                    continue;
                }
                // 類区分取得
                RuiKbn? ruiKbn = GetRuiKbn(sessionInfo.KyosaiMokutekiCd, target.HikiukeKbn, target.HikiukeHoushiki);

                List<NpgsqlParameter> delParams =
                [
                    new NpgsqlParameter("組合等コード", sessionInfo.KumiaitoCd),
                    new NpgsqlParameter("年産", sessionInfo.Nensan),
                    new NpgsqlParameter("共済目的コード", sessionInfo.KyosaiMokutekiCd),
                    new NpgsqlParameter("組合員等コード", sessionInfo.KumiaiintoCd),
                    new NpgsqlParameter("類区分", string.IsNullOrEmpty(ruiKbn?.RuiKbnValue) ? DBNull.Value : ruiKbn?.RuiKbnValue),
                    new NpgsqlParameter("引受区分", string.IsNullOrEmpty(target.HikiukeKbn) ? DBNull.Value : target.HikiukeKbn),
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
        public int UpdateRuibetsu(ref NskAppContext dbContext, D105030SessionInfo sessionInfo, string userId, DateTime sysDateTime, ref List<D105030RuibetsuSetteiRecord> ruibetsuUpdRecords)
        {
            int updCount = 0;

            // t_11030_個人設定類の対象レコードを更新する。
            StringBuilder updRuibetsu = new();
            updRuibetsu.Append("UPDATE t_11030_個人設定類 SET ");
            updRuibetsu.Append("  組合等コード      = @組合等コード ");
            updRuibetsu.Append(" ,年産              = @年産 ");
            updRuibetsu.Append(" ,共済目的コード    = @共済目的コード ");
            updRuibetsu.Append(" ,組合員等コード    = @組合員等コード ");
            updRuibetsu.Append(" ,類区分            = @類区分 ");
            updRuibetsu.Append(" ,引受区分          = @引受区分 ");
            updRuibetsu.Append(" ,引受方式          = @引受方式 ");
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
            updRuibetsu.Append(" AND 耕地番号       = @耕地番号 ");
            updRuibetsu.Append(" AND 分筆番号       = @分筆番号 ");
            updRuibetsu.Append(" AND 類区分         = @類区分 ");
            updRuibetsu.Append(" AND xmin           = @xmin ");

            foreach (D105030RuibetsuSetteiRecord target in ruibetsuUpdRecords)
            {
                if (!target.Xmin.HasValue)
                {
                    // xmin nullは処理対象外
                    continue;
                }
                // 類区分取得
                RuiKbn? ruiKbn = GetRuiKbn(sessionInfo.KyosaiMokutekiCd, target.HikiukeKbn, target.HikiukeHoushiki);

                List<NpgsqlParameter> updParams =
                [
                    new NpgsqlParameter("組合等コード", sessionInfo.KumiaitoCd),
                    new NpgsqlParameter("年産", sessionInfo.Nensan),
                    new NpgsqlParameter("共済目的コード", sessionInfo.KyosaiMokutekiCd),
                    new NpgsqlParameter("組合員等コード", sessionInfo.KumiaiintoCd),

                    new NpgsqlParameter("類区分", string.IsNullOrEmpty(ruiKbn?.RuiKbnValue) ? DBNull.Value : ruiKbn?.RuiKbnValue),
                    new NpgsqlParameter("引受区分", string.IsNullOrEmpty(target.HikiukeKbn) ? DBNull.Value : target.HikiukeKbn),
                    new NpgsqlParameter("引受方式", string.IsNullOrEmpty(target.HikiukeHoushiki) ? DBNull.Value : target.HikiukeHoushiki),
                    new NpgsqlParameter("特約区分", string.IsNullOrEmpty(target.IppitsuHansonTokuyaku) ? DBNull.Value : target.IppitsuHansonTokuyaku),
                    new NpgsqlParameter("補償割合コード", string.IsNullOrEmpty(target.HoshoWariai) ? DBNull.Value : target.HoshoWariai),
                    new NpgsqlParameter("種類区分", DBNull.Value),
                    new NpgsqlParameter("共済金額選択順位", target.SelectKyosaiKingaku.HasValue ? target.SelectKyosaiKingaku.Value : DBNull.Value),
                    new NpgsqlParameter("システム日時", sysDateTime),
                    new NpgsqlParameter("ユーザID", userId),
                ];
                switch (sessionInfo.KyosaiMokutekiCd)
                {
                    case F105Const.KYOSAI_MOKUTEKI_SUITO:
                        updParams.Add(new NpgsqlParameter("付保割合", target.FuhoWariai));
                        updParams.Add(new NpgsqlParameter("収穫量確認方法", string.IsNullOrEmpty(target.SyukakuryoKakuninHouhou) ? DBNull.Value : target.SyukakuryoKakuninHouhou));
                        updParams.Add(new NpgsqlParameter("全相殺基準単収", string.IsNullOrEmpty(target.ZensousaiKijunTansyu) ? DBNull.Value : target.ZensousaiKijunTansyu));
                        break;
                    case F105Const.KYOSAI_MOKUTEKI_RIKUTO:
                        updParams.Add(new NpgsqlParameter("付保割合", DBNull.Value));
                        updParams.Add(new NpgsqlParameter("収穫量確認方法", DBNull.Value));
                        updParams.Add(new NpgsqlParameter("全相殺基準単収", DBNull.Value));
                        break;
                }
                NpgsqlParameter xminParam = new("xmin", NpgsqlTypes.NpgsqlDbType.Xid) { Value = target.Xmin };
                updParams.Add(xminParam);
                int cnt = dbContext.Database.ExecuteSqlRaw(updRuibetsu.ToString(), updParams);
                if (cnt == 0)
                {
                    throw new DBConcurrencyException();
                }
                updCount += cnt;
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
        public int AppendRuibetsu(ref NskAppContext dbContext, D105030SessionInfo sessionInfo, string userId, DateTime sysDateTime, ref List<D105030RuibetsuSetteiRecord> ruibetsuAddRecords)
        {
            int addCount = 0;

            // t_11030_個人設定類の対象レコードを登録する。
            List<T11030個人設定類> addRuibetsuRecs = new();
            foreach (D105030RuibetsuSetteiRecord target in ruibetsuAddRecords)
            {
                // 類区分取得
                RuiKbn? ruiKbn = GetRuiKbn(sessionInfo.KyosaiMokutekiCd, target.HikiukeKbn, target.HikiukeHoushiki);

                T11030個人設定類 addRec = new()
                {
                    組合等コード = sessionInfo.KumiaitoCd,
                    年産 = (short)sessionInfo.Nensan,
                    共済目的コード = sessionInfo.KyosaiMokutekiCd,
                    組合員等コード = sessionInfo.KumiaiintoCd,
                    類区分 = ruiKbn?.RuiKbnValue,
                    引受区分 = target.HikiukeKbn,
                    引受方式 = target.HikiukeHoushiki,
                    特約区分 = target.IppitsuHansonTokuyaku,
                    補償割合コード = target.HoshoWariai,
                    付保割合 = null,
                    種類区分 = null,
                    作付時期 = null,
                    田畑区分 = null,
                    共済金額選択順位 = target.SelectKyosaiKingaku,
                    収穫量確認方法 = null,
                    全相殺基準単収 = null,
                    営農対象外フラグ = null,
                    担手農家区分 = null,
                    全相殺受託者等名称 = null,
                    備考 = null,
                    登録日時 = sysDateTime,
                    登録ユーザid = userId,
                    更新日時 = sysDateTime,
                    更新ユーザid = userId,
                };
                switch (sessionInfo.KyosaiMokutekiCd)
                {
                    case F105Const.KYOSAI_MOKUTEKI_SUITO:
                        addRec.付保割合 = target.FuhoWariai;
                        addRec.収穫量確認方法 = target.SyukakuryoKakuninHouhou;
                        //addRec.全相殺基準単収 = target.ZensousaiKijunTansyu;
                        break;
                    case F105Const.KYOSAI_MOKUTEKI_RIKUTO:
                        addRec.付保割合 = null;
                        addRec.収穫量確認方法 = null;
                        addRec.全相殺基準単収 = null;
                        break;
                }
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
        /// <param name="sessionInfo"></param>
        /// <returns></returns>
        public override List<D105030RuibetsuSetteiRecord> GetUpdateRecs(ref NskAppContext dbContext, D105030SessionInfo sessionInfo)
        {
            List<D105030RuibetsuSetteiRecord> updRecs = new();

            // 検索結果取得
            List<D105030RuibetsuSetteiRecord> dbResults = GetResult(dbContext, sessionInfo);

            // 検索結果と画面入力値を比較
            foreach (D105030RuibetsuSetteiRecord dispRec in DispRecords)
            {
                // 追加行、削除行以外を対象とする
                if (dispRec is D105030PagerRecord pagerRec && !pagerRec.IsNewRec && !pagerRec.IsDelRec)
                {
                    D105030RuibetsuSetteiRecord dbRec = dbResults.SingleOrDefault(x =>
                        (x.HikiukeKbn == dispRec.HikiukeKbn)
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
        /// 共済目的コード、引受区分、引受方式に応じた類区分を取得する。
        /// </summary>
        /// <param name="kyosaiMokutekiCd">共済目的コード</param>
        /// <param name="hikiukeKbn">類区分</param>
        /// <param name="hikiukeHoushiki">引受方式</param>
        /// <returns></returns>
        private RuiKbn? GetRuiKbn(string kyosaiMokutekiCd, string hikiukeKbn, string hikiukeHoushiki)
        {
            // 類区分取得
            RuiKbn? ruiKbn = _ruiKbns.SingleOrDefault(x =>
                (x.KyosaiMokutekiCd == kyosaiMokutekiCd) &&
                (x.HikiukeKbn == hikiukeKbn) &&
                (x.HikiukeHoushiki == hikiukeHoushiki)
            );
            return ruiKbn;
        }

        /// <summary>
        /// DB更新仕様書_削除No.3 デシジョンテーブル参照
        /// </summary>
        private readonly List<RuiKbn> _ruiKbns =
        [
            //   共済目的                         引受区分 引受方式 類区分
            new (F105Const.KYOSAI_MOKUTEKI_SUITO, "11",    "2",     "1"),
            new (F105Const.KYOSAI_MOKUTEKI_SUITO, "11",    "3",     "1"),
            new (F105Const.KYOSAI_MOKUTEKI_SUITO, "11",    "5",     "1"),
            new (F105Const.KYOSAI_MOKUTEKI_SUITO, "12",    "2",     "2"),
            new (F105Const.KYOSAI_MOKUTEKI_SUITO, "12",    "3",     "2"),
            new (F105Const.KYOSAI_MOKUTEKI_SUITO, "12",    "5",     "2"),
            new (F105Const.KYOSAI_MOKUTEKI_SUITO, "13",    "2",     "3"),
            new (F105Const.KYOSAI_MOKUTEKI_SUITO, "13",    "3",     "3"),
            new (F105Const.KYOSAI_MOKUTEKI_SUITO, "13",    "5",     "3"),
            new (F105Const.KYOSAI_MOKUTEKI_SUITO, "21",    "2",     "4"),
            new (F105Const.KYOSAI_MOKUTEKI_SUITO, "21",    "3",     "4"),
            new (F105Const.KYOSAI_MOKUTEKI_SUITO, "21",    "5",     "4"),
            new (F105Const.KYOSAI_MOKUTEKI_SUITO, "22",    "2",     "5"),
            new (F105Const.KYOSAI_MOKUTEKI_SUITO, "22",    "3",     "5"),
            new (F105Const.KYOSAI_MOKUTEKI_SUITO, "22",    "5",     "5"),
            new (F105Const.KYOSAI_MOKUTEKI_SUITO, "23",    "2",     "6"),
            new (F105Const.KYOSAI_MOKUTEKI_SUITO, "23",    "3",     "6"),
            new (F105Const.KYOSAI_MOKUTEKI_SUITO, "23",    "5",     "6"),
            new (F105Const.KYOSAI_MOKUTEKI_SUITO, "11",    "6",     "7"),
            new (F105Const.KYOSAI_MOKUTEKI_SUITO, "13",    "6",     "7"),
            new (F105Const.KYOSAI_MOKUTEKI_SUITO, "21",    "6",     "7"),
            new (F105Const.KYOSAI_MOKUTEKI_SUITO, "23",    "6",     "7"),
            new (F105Const.KYOSAI_MOKUTEKI_RIKUTO, "",     "",      "1"),
        ];

        /// <summary>
        /// 類区分設定クラス
        /// </summary>
        private class RuiKbn
        {
            /// <summary>共済目的コード</summary>
            public string KyosaiMokutekiCd { get; set; } = string.Empty;
            /// <summary>引受区分</summary>
            public string HikiukeKbn { get; set; } = string.Empty;
            /// <summary>引受方式</summary>
            public string HikiukeHoushiki { get; set; } = string.Empty;
            /// <summary>類区分</summary>
            public string RuiKbnValue { get; set; } = string.Empty;

            /// <summary>
            /// コンストラクタ
            /// </summary>
            /// <param name="kyosaiMokutekiCd">共済目的コード</param>
            /// <param name="hikiukeKbn">引受区分</param>
            /// <param name="hikiukeHoushiki">引受方式</param>
            /// <param name="value">類区分</param>
            public RuiKbn(string kyosaiMokutekiCd, string hikiukeKbn, string hikiukeHoushiki, string value)
            {
                KyosaiMokutekiCd = kyosaiMokutekiCd;
                HikiukeHoushiki = hikiukeHoushiki;
                HikiukeKbn = hikiukeKbn;
                RuiKbnValue = value;
            }
        }
    }
}