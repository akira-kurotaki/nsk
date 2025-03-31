using CoreLibrary.Core.Consts;
using CoreLibrary.Core.Extensions;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using NskAppModelLibrary.Context;
using NskAppModelLibrary.Models;
using NskWeb.Areas.F105.Consts;
using NskWeb.Common.Models;
using System.Data;
using System.Text;

namespace NskWeb.Areas.F105.Models.D105030
{
    /// <summary>
    /// 引受情報検索結果
    /// </summary>
    [Serializable]
    public class D105030HikiukeSearchResult : BasePager<D105030HikiukeRecord>
    {
        /// <summary>種類ドロップダウンリスト選択値</summary>
        public List<SelectListItem> SyuruiList { get; set; } = new();
        /// <summary>区分ドロップダウンリスト選択値</summary>
        public List<SelectListItem> KbnList { get; set; } = new();
        ///// <summary>市町村ドロップダウンリスト選択値</summary>
        //public List<SelectListItem> ShichosonList { get; set; } = new();
        ///// <summary>品種ドロップダウンリスト選択値</summary>
        //public List<SelectListItem> HinsyuList { get; set; } = new();
        ///// <summary>産地銘柄ドロップダウンリスト選択値</summary>
        //public List<SelectListItem> SanchiMeigaraList { get; set; } = new();
        /// <summary>田畑ドロップダウンリスト選択値</summary>
        public List<SelectListItem> TahataList { get; set; } = new();
        /// <summary>収量等級ドロップダウンリスト選択値</summary>
        public List<SelectListItem> SyuryoTokyuList { get; set; } = new();
        /// <summary>受委託者区分ドロップダウンリスト選択値</summary>
        public List<SelectListItem> JuitakusyaKbnList { get; set; } = new();

        public D105030SearchCondition SearchCondition { get; set; } = new();

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public D105030HikiukeSearchResult()
        {
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="searchCondition"></param>
        public D105030HikiukeSearchResult(D105030SearchCondition searchCondition)
        {
            SearchCondition = searchCondition;
            DisplayCount = SearchCondition.DisplayCount ?? CoreConst.PAGE_SIZE;
        }

        /// <summary>
        /// ドロップダウンリスト初期化
        /// </summary>
        /// <param name="dbContext"></param>
        public void InitializeDropdonwList(NskAppContext dbContext, D105030SessionInfo sessionInfo)
        {
            //２．４．[種類] ドロップダウンリスト項目を取得する。		
            //	(1) m_10140_種類名称テーブルより、種類コード、種類名称を取得する。
            //	(2) 取得した結果をドロップダウンリストの項目として設定する。
            SyuruiList = new();
            SyuruiList.AddRange(dbContext.M10140種類名称s.Where(m => m.共済目的コード == sessionInfo.KyosaiMokutekiCd)?.
                OrderBy(m => m.種類コード).
                Select(m => new SelectListItem($"{m.種類コード} {m.種類名称}", $"{m.種類コード}")));

            //２．５．[区分] ドロップダウンリスト項目を取得する。		
            //	(1) m_00030_区分名称テーブルより、区分コード、区分名称を取得する。
            //	(2) 取得した結果をドロップダウンリストの項目として設定する。
            KbnList = new();
            KbnList.AddRange(dbContext.M00030区分名称s.Where(m =>
                (m.組合等コード == sessionInfo.KumiaitoCd) &&
                (m.年産 == sessionInfo.Nensan) &&
                (m.共済目的コード == sessionInfo.KyosaiMokutekiCd) )?.
                OrderBy(m => m.区分コード).
                Select(m => new SelectListItem($"{m.区分コード} {m.区分名称}", $"{m.区分コード}")));

            ////２．６．[市町村] ドロップダウンリスト項目を取得する。		
            ////	(1) 名称M市町村テーブルより、市町村コード、市町村名を取得する。
            ////	(2) 取得した結果をドロップダウンリストの項目として設定する。
            //ShichosonList = new();
            //ShichosonList.AddRange(dbContext.VShichosonNms.Where(m =>
            //    (m.KumiaitoCd == sessionInfo.KumiaitoCd) &&
            //    (m.TodofukenCd == sessionInfo.TodofukenCd))?.
            //    OrderBy(m => m.ShichosonCd).
            //    Select(m => new SelectListItem($"{m.ShichosonCd} {m.ShichosonNm}", $"{m.ShichosonCd}")));

            ////２．７．[産地銘柄] ドロップダウンリスト項目を取得する。		
            ////	(1) m_00130_産地別銘柄名称設定テーブルより、産地別銘柄コード、産地別銘柄名称を取得する。
            ////	(2) 取得した結果をドロップダウンリストの項目として設定する。
            //SanchiMeigaraList = new();
            //SanchiMeigaraList.AddRange(dbContext.M00130産地別銘柄名称設定s.Where(m =>
            //    (m.組合等コード == sessionInfo.KumiaitoCd) &&
            //    (m.年産 == sessionInfo.Nensan) &&
            //    (m.共済目的コード == sessionInfo.KyosaiMokutekiCd))?.
            //    OrderBy(m => m.産地別銘柄コード).
            //    Select(m => new SelectListItem($"{m.産地別銘柄コード} {m.産地別銘柄名称}", $"{m.産地別銘柄コード}")));

            ////２．８．[品種] ドロップダウンリスト項目を取得する。		
            ////	(1) m_00110_品種係数テーブルより、品種コード、品種名等を取得する。
            ////	(2) 取得した結果をドロップダウンリストの項目として設定する。
            //HinsyuList = new();
            //HinsyuList.AddRange(dbContext.M00110品種係数s.Where(m =>
            //    (m.組合等コード == sessionInfo.KumiaitoCd) &&
            //    (m.年産 == sessionInfo.Nensan) &&
            //    (m.共済目的コード == sessionInfo.KyosaiMokutekiCd))?.
            //    OrderBy(m => m.品種コード).
            //    Select(m => new SelectListItem($"{m.品種コード} {m.品種名等}", $"{m.品種コード}")));

            //２．９．[田畑] ドロップダウンリスト項目を取得する。		
            //	(1) m_00040_田畑名称テーブルより、田畑区分、田畑名称を取得する。
            //	(2) 取得した結果をドロップダウンリストの項目として設定する。
            TahataList = new();
            TahataList.AddRange(dbContext.M00040田畑名称s.
                OrderBy(m => m.田畑区分).
                Select(m => new SelectListItem($"{m.田畑区分} {m.田畑名称}", $"{m.田畑区分}")));

            //２．１０．[収量等級] ドロップダウンリスト項目を取得する。		
            //	(1) m_10060_収量等級テーブルより、収量等級コード、収量を取得する。
            //	(2) 取得した結果をドロップダウンリストの項目として設定する。
            SyuryoTokyuList = new();
            SyuryoTokyuList.AddRange(dbContext.M10060収量等級s.Where(m =>
                (m.組合等コード == sessionInfo.KumiaitoCd) &&
                (m.年産 == sessionInfo.Nensan) &&
                (m.共済目的コード == sessionInfo.KyosaiMokutekiCd))?.
                OrderBy(m => m.収量等級コード).
                Select(m => new SelectListItem($"{m.収量等級コード} {m.収量}", $"{m.収量等級コード}")));


            // ２．１１．[受委託者区分] ドロップダウンリスト項目を設定する。		
            // 	(1) 「受託者区分」ドロップダウンリスト項目に以下を設定する。
            JuitakusyaKbnList =
            [
                new($"{(int)F105Const.JuitakusyaKbn.None} {F105Const.JuitakusyaKbn.None.ToDescription()}", $"{(int)F105Const.JuitakusyaKbn.None}"),
                new($"{(int)F105Const.JuitakusyaKbn.Jutaku} {F105Const.JuitakusyaKbn.Jutaku.ToDescription()}", $"{(int)F105Const.JuitakusyaKbn.Jutaku}"),
                new($"{(int)F105Const.JuitakusyaKbn.Itaku} {F105Const.JuitakusyaKbn.Itaku.ToDescription()}", $"{(int)F105Const.JuitakusyaKbn.Itaku}"),
            ];
        }


        /// <summary>
        /// 引受情報を取得する。
        /// </summary>
        /// <param name="dbContext">DBコンテキスト</param>
        /// <param name="session"></param>
        /// <returns>検索情報</returns>
        public override List<D105030HikiukeRecord> GetResult(NskAppContext dbContext, BaseSessionInfo session)
        {
            D105030SessionInfo sessionInfo = (D105030SessionInfo)session;

            // ２．２．２．１．引受情報を取得する。	
            // t_11090_引受耕地テーブル、t_11100_引受gisテーブルから
            // [画面：地番]、[画面：耕地番号（開始）]、[画面：耕地番号（終了）] に該当する引受情報を取得する。

            StringBuilder query = new();
            List<NpgsqlParameter> queryParams =
            [
                new("組合等コード", sessionInfo.KumiaitoCd),
                new("年産", sessionInfo.Nensan),
                new("共済目的コード", sessionInfo.KyosaiMokutekiCd),
                new("組合員等コード", sessionInfo.KumiaiintoCd),
                new("都道府県コード", sessionInfo.TodofukenCd)
            ];

            query.Append(" SELECT ");
            query.Append($"   T1.耕地番号         As \"{nameof(D105030HikiukeRecord.KouchiNo)}\" ");
            query.Append($"  ,T1.地名地番         As \"{nameof(D105030HikiukeRecord.ChimeiChiban)}\" ");
            query.Append($"  ,T1.耕地面積         As \"{nameof(D105030HikiukeRecord.HonchiMenseki)}\" ");
            query.Append($"  ,T1.引受面積         As \"{nameof(D105030HikiukeRecord.HikukeMenseki)}\" ");
            query.Append($"  ,T1.転作等面積       As \"{nameof(D105030HikiukeRecord.TensakutoMenseki)}\" ");
            query.Append($"  ,T1.種類コード       As \"{nameof(D105030HikiukeRecord.Syurui)}\" ");
            query.Append($"  ,T1.区分コード       As \"{nameof(D105030HikiukeRecord.Kbn)}\" ");
            query.Append($"  ,T1.統計市町村コード As \"{nameof(D105030HikiukeRecord.Shichoson)}\" ");
            query.Append($"  ,M3.shichoson_nm     As \"{nameof(D105030HikiukeRecord.ShichosonNm)}\" ");
            query.Append($"  ,T1.品種コード       As \"{nameof(D105030HikiukeRecord.Hinsyu)}\" ");
            query.Append($"  ,M1.品種名等         As \"{nameof(D105030HikiukeRecord.HinsyuNm)}\" ");
            query.Append($"  ,T1.産地別銘柄コード As \"{nameof(D105030HikiukeRecord.SanchiMeigara)}\" ");
            query.Append($"  ,M2.産地別銘柄名称   As \"{nameof(D105030HikiukeRecord.SanchiMeigaraNm)}\" ");
            query.Append($"  ,T1.田畑区分         As \"{nameof(D105030HikiukeRecord.Tahata)}\" ");
            query.Append($"  ,T1.収量等級コード   As \"{nameof(D105030HikiukeRecord.SyuryoTokyu)}\" ");
            query.Append($"  ,T1.分筆番号         As \"{nameof(D105030HikiukeRecord.BunpitsuNo)}\" ");
            query.Append($"  ,T1.統計単収         As \"{nameof(D105030HikiukeRecord.ToukeiTansyu)}\" ");
            query.Append($"  ,T1.実量基準単収     As \"{nameof(D105030HikiukeRecord.JituryoKijunTansyu)}\" ");
            query.Append($"  ,T1.参酌コード       As \"{nameof(D105030HikiukeRecord.Sanjaku)}\" ");
            query.Append($"  ,T1.受委託区分       As \"{nameof(D105030HikiukeRecord.JuitakushaKbn)}\" ");
            query.Append($"  ,T1.受委託者コード   As \"{nameof(D105030HikiukeRecord.Shoyusha)}\" ");
            query.Append($"  ,T1.備考             As \"{nameof(D105030HikiukeRecord.Bikou)}\" ");
            query.Append($"  ,T2.局都道府県コード As \"{nameof(D105030HikiukeRecord.GisKyokuTodofuken)}\" ");
            query.Append($"  ,T2.市区町村コード   As \"{nameof(D105030HikiukeRecord.GisShichoson)}\" ");
            query.Append($"  ,T2.大字コード       As \"{nameof(D105030HikiukeRecord.GisOoaza)}\" ");
            query.Append($"  ,T2.小字コード       As \"{nameof(D105030HikiukeRecord.GisKoaza)}\" ");
            query.Append($"  ,T2.地番             As \"{nameof(D105030HikiukeRecord.GisChiban)}\" ");
            query.Append($"  ,T2.枝番             As \"{nameof(D105030HikiukeRecord.GisEdaban)}\" ");
            query.Append($"  ,T2.子番             As \"{nameof(D105030HikiukeRecord.GisKoban)}\" ");
            query.Append($"  ,T2.孫番             As \"{nameof(D105030HikiukeRecord.GisMagoban)}\" ");
            query.Append($"  ,T2.RS区分           As \"{nameof(D105030HikiukeRecord.GisRsKbn)}\" ");
            query.Append($"  ,cast('' || T1.xmin as integer) As \"{nameof(D105030HikiukeRecord.KouchiXmin)}\" ");
            query.Append($"  ,cast('' || T2.xmin as integer) As \"{nameof(D105030HikiukeRecord.GisXmin)}\" ");
            query.Append(" FROM t_11090_引受耕地     T1 ");
            query.Append(" LEFT OUTER JOIN t_11100_引受gis    T2 ");
            query.Append(" ON    T1.組合等コード   = T2.組合等コード ");
            query.Append("  AND  T1.年産           = T2.年産 ");
            query.Append("  AND  T1.共済目的コード = T2.共済目的コード ");
            query.Append("  AND  T1.組合員等コード = T2.組合員等コード ");
            query.Append("  AND  T1.耕地番号       = T2.耕地番号 ");
            query.Append("  AND  T1.分筆番号       = T2.分筆番号 ");
            query.Append(" LEFT OUTER JOIN m_00110_品種係数              M1 ");
            query.Append(" ON    T1.組合等コード   = M1.組合等コード ");
            query.Append("  AND  T1.年産           = M1.年産 ");
            query.Append("  AND  T1.共済目的コード = M1.共済目的コード ");
            query.Append("  AND  T1.品種コード     = M1.品種コード ");
            query.Append(" LEFT OUTER JOIN m_00130_産地別銘柄名称設定    M2 ");
            query.Append(" ON    T1.組合等コード   = M2.組合等コード ");
            query.Append("  AND  T1.年産           = M2.年産 ");
            query.Append("  AND  T1.共済目的コード = M2.共済目的コード ");
            query.Append("  AND  T1.産地別銘柄コード = M2.産地別銘柄コード ");
            query.Append(" LEFT OUTER JOIN v_shichoson_nm                M3 ");
            query.Append(" ON    T1.組合等コード   = M3.kumiaito_cd ");
            query.Append("  AND  M3.todofuken_cd   = @都道府県コード ");
            query.Append("  AND  T1.統計市町村コード = M3.shichoson_cd ");

            query.Append(" WHERE T1.組合等コード   = @組合等コード ");
            query.Append("  AND  T1.年産           = @年産 ");
            query.Append("  AND  T1.共済目的コード = @共済目的コード ");
            query.Append("  AND  T1.組合員等コード = @組合員等コード ");

            // ※「画面：地名地番」の入力がある場合
            if (!string.IsNullOrEmpty(SearchCondition.Chiban))
            {
                query.Append("  AND  T1.地名地番       = @地名地番 ");
                queryParams.Add(new("地名地番", SearchCondition.Chiban));
            }

            // ※「画面：耕地番号（開始）」のみ入力がある場合
            if (!string.IsNullOrEmpty(SearchCondition.KouchiNoFrom) && string.IsNullOrEmpty(SearchCondition.KouchiNoTo))
            {
                query.Append("  AND  T1.耕地番号       = @耕地番号From ");
                queryParams.Add(new("耕地番号From", SearchCondition.KouchiNoFrom));
            }

            // ※「画面：耕地番号（終了）」のみ入力がある場合
            if (string.IsNullOrEmpty(SearchCondition.KouchiNoFrom) && !string.IsNullOrEmpty(SearchCondition.KouchiNoTo))
            {
                query.Append("  AND  T1.耕地番号       = @耕地番号To ");
                queryParams.Add(new("耕地番号To", SearchCondition.KouchiNoTo));
            }

            // ※「画面：耕地番号（開始）」および「画面：耕地番号（終了）」に入力がある場合
            if (!string.IsNullOrEmpty(SearchCondition.KouchiNoFrom) && !string.IsNullOrEmpty(SearchCondition.KouchiNoTo))
            {
                query.Append("  AND (T1.耕地番号     >= @耕地番号From ");
                query.Append("  AND      T1.耕地番号     <= @耕地番号To) ");
                queryParams.Add(new("耕地番号From", SearchCondition.KouchiNoFrom));
                queryParams.Add(new("耕地番号To", SearchCondition.KouchiNoTo));
            }

            if (SearchCondition.DisplaySort1.HasValue || SearchCondition.DisplaySort2.HasValue)
            {
                // ORDER BY
                query.Append(" ORDER BY ");

                bool isPutOrder = false;
                //  画面指定ソート順
                if (SearchCondition.DisplaySort1.HasValue)
                {
                    isPutOrder = true;
                    switch (SearchCondition.DisplaySort1)
                    {
                        case D105030SearchCondition.DisplaySortType.Chiban:
                            query.Append($" T1.{nameof(T11090引受耕地.地名地番)} {SearchCondition.DisplaySortOrder1} ");
                            break;
                        case D105030SearchCondition.DisplaySortType.KouchBango:
                            query.Append($" T1.{nameof(T11090引受耕地.耕地番号)} {SearchCondition.DisplaySortOrder1} ");
                            break;
                    }
                }
                if (SearchCondition.DisplaySort2.HasValue)
                {
                    if (isPutOrder)
                    {
                        // ソート条件1が出力されていた場合、カンマを付与する
                        query.Append(", ");
                    }
                    switch (SearchCondition.DisplaySort2)
                    {
                        case D105030SearchCondition.DisplaySortType.Chiban:
                            query.Append($" T1.{nameof(T11090引受耕地.地名地番)} {SearchCondition.DisplaySortOrder2} ");
                            break;
                        case D105030SearchCondition.DisplaySortType.KouchBango:
                            query.Append($" T1.{nameof(T11090引受耕地.耕地番号)} {SearchCondition.DisplaySortOrder2} ");
                            break;
                    }
                }
            }



            List<D105030HikiukeRecord> records = new();
            records.AddRange(dbContext.Database.SqlQueryRaw<D105030HikiukeRecord>(query.ToString(), queryParams.ToArray()));

            return records;
        }

        /// <summary>
        /// t_11090_引受耕地とt_11100_引受gisの対象レコードを削除する
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="sessionInfo"></param>
        /// <param name="hikiukeDelRecords"></param>
        /// <returns>削除件数</returns>
        public int DeleteHikiukeGis(ref NskAppContext dbContext, 
            D105030SessionInfo sessionInfo, 
            ref List<D105030HikiukeRecord> hikiukeDelRecords)
        {
            int delCount = 0;

            // (1) t_11090_引受耕地の対象レコードを削除する。
            StringBuilder delHikiukeKouchi = new();
            delHikiukeKouchi.Append("DELETE FROM t_11090_引受耕地 ");
            delHikiukeKouchi.Append("WHERE ");
            delHikiukeKouchi.Append("     組合等コード   = @組合等コード ");
            delHikiukeKouchi.Append(" AND 年産           = @年産 ");
            delHikiukeKouchi.Append(" AND 共済目的コード = @共済目的コード ");
            delHikiukeKouchi.Append(" AND 組合員等コード = @組合員等コード ");
            delHikiukeKouchi.Append(" AND 耕地番号       = @耕地番号 ");
            delHikiukeKouchi.Append(" AND 分筆番号       = @分筆番号 ");
            delHikiukeKouchi.Append(" AND xmin           = @xmin ");

            foreach (D105030HikiukeRecord target in hikiukeDelRecords)
            {
                if (!target.KouchiXmin.HasValue)
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
                    new("耕地番号", target.KouchiNo),
                    new("分筆番号", target.BunpitsuNo),
                ];
                NpgsqlParameter xminParam = new("xmin", NpgsqlTypes.NpgsqlDbType.Xid) { Value = target.KouchiXmin };
                delParams.Add(xminParam);
                int cnt = dbContext.Database.ExecuteSqlRaw(delHikiukeKouchi.ToString(), delParams);
                if (cnt == 0)
                {
                    throw new DBConcurrencyException();
                }
                delCount += cnt;
            }

            // (2) t_11100_引受gisの対象レコードを削除する。
            StringBuilder delHikiukeGis = new();
            delHikiukeGis.Append("DELETE FROM t_11100_引受gis ");
            delHikiukeGis.Append("WHERE ");
            delHikiukeGis.Append("     組合等コード   = @組合等コード ");
            delHikiukeGis.Append(" AND 年産           = @年産 ");
            delHikiukeGis.Append(" AND 共済目的コード = @共済目的コード ");
            delHikiukeGis.Append(" AND 組合員等コード = @組合員等コード ");
            delHikiukeGis.Append(" AND 耕地番号       = @耕地番号 ");
            delHikiukeGis.Append(" AND 分筆番号       = @分筆番号 ");
            delHikiukeGis.Append(" AND xmin           = @xmin ");
            foreach (D105030HikiukeRecord target in hikiukeDelRecords)
            {
                if (!target.GisXmin.HasValue)
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
                    new("耕地番号", target.KouchiNo),
                    new("分筆番号", target.BunpitsuNo),
                ];
                NpgsqlParameter xminParam = new("xmin", NpgsqlTypes.NpgsqlDbType.Xid) { Value = target.GisXmin };
                delParams.Add(xminParam);
                int cnt = dbContext.Database.ExecuteSqlRaw(delHikiukeGis.ToString(), delParams);
                if (cnt == 0)
                {
                    throw new DBConcurrencyException();
                }
                delCount += cnt;
            }

            return delCount;
        }

        /// <summary>
        /// 削除対象レコードの情報をt_12140_引受耕地削除データ保持テーブルに登録する
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="hikiukeDelRecords"></param>
        public void RegistHikiukeDelRec(ref NskAppContext dbContext,
            D105030SessionInfo sessionInfo,
            string userId,
            DateTime? sysDateTime,
            ref List<D105030HikiukeRecord> hikiukeDelRecords)
        {
            // t_12140_引受耕地削除データを登録する。
            StringBuilder query = new();
            query.Append("INSERT INTO t_12140_引受耕地削除データ保持 ");
            query.Append(" VALUES (");
            query.Append("  @組合等コード ");
            query.Append(" ,@年産 ");
            query.Append(" ,@共済目的コード ");
            query.Append(" ,@組合員等コード ");
            query.Append(" ,@耕地番号 ");
            query.Append(" ,@分筆番号 ");
            query.Append(" ,@地名地番 ");
            query.Append(" ,@削除日時 ");
            query.Append(" ,@登録日時 ");
            query.Append(" ,@登録ユーザid ");
            query.Append(" )");

            foreach(D105030HikiukeRecord target in hikiukeDelRecords)
            {
                NpgsqlParameter[] addParams =
                [
                    new("組合等コード", sessionInfo.KumiaitoCd),
                    new("年産", sessionInfo.Nensan),
                    new("共済目的コード", sessionInfo.KyosaiMokutekiCd),
                    new("組合員等コード", sessionInfo.KumiaiintoCd),
                    new("耕地番号", target.KouchiNo),
                    new("分筆番号", target.BunpitsuNo),
                    new("地名地番", string.IsNullOrEmpty(target.ChimeiChiban) ? DBNull.Value : target.ChimeiChiban),
                    new("削除日時", sysDateTime),
                    new("登録日時", sysDateTime),
                    new("登録ユーザid", userId),
                ];
                dbContext.Database.ExecuteSqlRaw(query.ToString(), addParams);
            }
        }

        /// <summary>
        /// t_11090_引受耕地とt_11100_引受gisの対象レコードを更新する
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="hikiukeUpdRecords"></param>
        /// <returns>更新件数</returns>
        public int UpdateHikiukeGis(ref NskAppContext dbContext,
            D105030SessionInfo sessionInfo,
            string userId,
            DateTime? sysDateTime,
            ref List<D105030HikiukeRecord> hikiukeUpdRecords)
        {
            int updCount = 0;

            // (1) t_11090_引受耕地の対象レコードを更新する。
            StringBuilder updHikiuke = new();
            updHikiuke.Append("UPDATE t_11090_引受耕地 SET ");
            updHikiuke.Append("  組合等コード      = @組合等コード ");
            updHikiuke.Append(" ,年産              = @年産 ");
            updHikiuke.Append(" ,共済目的コード    = @共済目的コード ");
            updHikiuke.Append(" ,組合員等コード    = @組合員等コード ");
            updHikiuke.Append(" ,耕地番号          = @耕地番号 ");
            updHikiuke.Append(" ,分筆番号          = @分筆番号 ");
            updHikiuke.Append(" ,地名地番          = @地名地番 ");
            updHikiuke.Append(" ,耕地面積          = @耕地面積 ");
            updHikiuke.Append(" ,引受面積          = @引受面積 ");
            updHikiuke.Append(" ,転作等面積        = @転作等面積 ");
            updHikiuke.Append(" ,種類コード        = @種類コード ");
            updHikiuke.Append(" ,区分コード        = @区分コード ");
            updHikiuke.Append(" ,統計市町村コード  = @統計市町村コード ");
            updHikiuke.Append(" ,品種コード        = @品種コード ");
            updHikiuke.Append(" ,産地別銘柄コード  = @産地別銘柄コード ");
            updHikiuke.Append(" ,田畑区分          = @田畑区分 ");
            updHikiuke.Append(" ,収量等級コード    = @収量等級コード ");
            updHikiuke.Append(" ,統計単収          = @統計単収 ");
            updHikiuke.Append(" ,実量基準単収      = @実量基準単収 ");
            updHikiuke.Append(" ,参酌コード        = @参酌コード ");
            updHikiuke.Append(" ,受委託区分        = @受委託区分 ");
            updHikiuke.Append(" ,受委託者コード    = @受委託者コード ");
            updHikiuke.Append(" ,備考              = @備考 ");
            updHikiuke.Append(" ,更新日時          = @システム日時 ");
            updHikiuke.Append(" ,更新ユーザid      = @ユーザID ");
            updHikiuke.Append("WHERE ");
            updHikiuke.Append("     組合等コード   = @組合等コード ");
            updHikiuke.Append(" AND 年産           = @年産 ");
            updHikiuke.Append(" AND 共済目的コード = @共済目的コード ");
            updHikiuke.Append(" AND 組合員等コード = @組合員等コード ");
            updHikiuke.Append(" AND 耕地番号       = @耕地番号 ");
            updHikiuke.Append(" AND 分筆番号       = @分筆番号 ");
            updHikiuke.Append(" AND xmin           = @xmin ");

            foreach (D105030HikiukeRecord target in hikiukeUpdRecords)
            {
                if (!target.KouchiXmin.HasValue)
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
                    new("耕地番号", target.KouchiNo),
                    new("分筆番号", target.BunpitsuNo),
                    new("地名地番", string.IsNullOrEmpty(target.ChimeiChiban) ? DBNull.Value : target.ChimeiChiban),
                    new("耕地面積", target.HonchiMenseki.HasValue ? target.HonchiMenseki : DBNull.Value),
                    new("引受面積", target.HikukeMenseki.HasValue ? target.HikukeMenseki : DBNull.Value),
                    new("転作等面積", target.TensakutoMenseki.HasValue ? target.TensakutoMenseki : DBNull.Value),
                    new("種類コード", string.IsNullOrEmpty(target.Syurui) ? DBNull.Value : target.Syurui),
                    new("区分コード", string.IsNullOrEmpty(target.Kbn) ? DBNull.Value : target.Kbn),
                    new("統計市町村コード", string.IsNullOrEmpty(target.Shichoson) ? DBNull.Value : target.Shichoson),
                    new("品種コード", string.IsNullOrEmpty(target.Hinsyu) ? DBNull.Value : target.Hinsyu),
                    new("産地別銘柄コード", string.IsNullOrEmpty(target.SanchiMeigara) ? DBNull.Value : target.SanchiMeigara),
                    new("田畑区分", string.IsNullOrEmpty(target.Tahata) ? DBNull.Value : target.Tahata),
                    new("収量等級コード", string.IsNullOrEmpty(target.SyuryoTokyu) ? DBNull.Value : target.SyuryoTokyu),
                    new("統計単収	", target.ToukeiTansyu.HasValue ? target.ToukeiTansyu : DBNull.Value),
                    new("実量基準単収", target.JituryoKijunTansyu.HasValue ? target.JituryoKijunTansyu : DBNull.Value),
                    new("参酌コード", string.IsNullOrEmpty(target.Sanjaku) ? DBNull.Value : target.Sanjaku),
                    new("受委託区分", string.IsNullOrEmpty(target.JuitakushaKbn) ? DBNull.Value : target.JuitakushaKbn),
                    new("受委託者コード", string.IsNullOrEmpty(target.Shoyusha) ? DBNull.Value : target.Shoyusha),
                    new("備考", string.IsNullOrEmpty(target.Bikou) ? DBNull.Value : target.Bikou),
                    new("システム日時", sysDateTime),
                    new("ユーザID", userId),
                ];
                NpgsqlParameter xminParam = new("xmin", NpgsqlTypes.NpgsqlDbType.Xid) { Value = target.KouchiXmin };
                updParams.Add(xminParam);
                int cnt = dbContext.Database.ExecuteSqlRaw(updHikiuke.ToString(), updParams);
                if (cnt == 0)
                {
                    throw new DBConcurrencyException();
                }
                updCount += cnt;
            }

            // (2) t_11100_引受gisの対象レコードを更新する。
            StringBuilder updHikiukeGis = new();
            updHikiukeGis.Append("UPDATE t_11100_引受gis SET ");
            updHikiukeGis.Append("  組合等コード      = @組合等コード ");
            updHikiukeGis.Append(" ,年産              = @年産 ");
            updHikiukeGis.Append(" ,共済目的コード    = @共済目的コード ");
            updHikiukeGis.Append(" ,組合員等コード    = @組合員等コード ");
            updHikiukeGis.Append(" ,耕地番号          = @耕地番号 ");
            updHikiukeGis.Append(" ,分筆番号          = @分筆番号 ");
            updHikiukeGis.Append(" ,局都道府県コード  = @局都道府県コード ");
            updHikiukeGis.Append(" ,市区町村コード    = @市区町村コード ");
            updHikiukeGis.Append(" ,大字コード        = @大字コード ");
            updHikiukeGis.Append(" ,小字コード        = @小字コード ");
            updHikiukeGis.Append(" ,地番              = @地番 ");
            updHikiukeGis.Append(" ,枝番              = @枝番 ");
            updHikiukeGis.Append(" ,子番              = @子番 ");
            updHikiukeGis.Append(" ,孫番              = @孫番 ");
            updHikiukeGis.Append(" ,RS区分            = @RS区分 ");
            updHikiukeGis.Append(" ,更新日時          = @システム日時 ");
            updHikiukeGis.Append(" ,更新ユーザid      = @ユーザID ");
            updHikiukeGis.Append("WHERE ");
            updHikiukeGis.Append("     組合等コード   = @組合等コード ");
            updHikiukeGis.Append(" AND 年産           = @年産 ");
            updHikiukeGis.Append(" AND 共済目的コード = @共済目的コード ");
            updHikiukeGis.Append(" AND 組合員等コード = @組合員等コード ");
            updHikiukeGis.Append(" AND 耕地番号       = @耕地番号 ");
            updHikiukeGis.Append(" AND 分筆番号       = @分筆番号 ");
            updHikiukeGis.Append(" AND xmin           = @xmin ");

            foreach (D105030HikiukeRecord target in hikiukeUpdRecords)
            {
                if (dbContext.T11100引受giss.Any(x =>
                    (x.組合等コード == sessionInfo.KumiaitoCd) &&
                    (x.年産 == sessionInfo.Nensan) &&
                    (x.共済目的コード == sessionInfo.KyosaiMokutekiCd) &&
                    (x.組合員等コード == sessionInfo.KumiaiintoCd) &&
                    (x.耕地番号 == target.KouchiNo) &&
                    (x.分筆番号 == target.BunpitsuNo)
                    ))
                {
                    if (!target.GisXmin.HasValue)
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
                        new("耕地番号", target.KouchiNo),
                        new("分筆番号", target.BunpitsuNo),
                        new("局都道府県コード", string.IsNullOrEmpty(target.GisKyokuTodofuken) ? DBNull.Value : target.GisKyokuTodofuken),
                        new("市区町村コード", string.IsNullOrEmpty(target.GisShichoson) ? DBNull.Value : target.GisShichoson),
                        new("大字コード", string.IsNullOrEmpty(target.GisOoaza) ? DBNull.Value : target.GisOoaza),
                        new("小字コード", string.IsNullOrEmpty(target.GisKoaza) ? DBNull.Value : target.GisKoaza),
                        new("地番", string.IsNullOrEmpty(target.GisChiban) ? DBNull.Value : target.GisChiban),
                        new("枝番", string.IsNullOrEmpty(target.GisEdaban) ? DBNull.Value : target.GisEdaban),
                        new("子番", string.IsNullOrEmpty(target.GisKoban) ? DBNull.Value : target.GisKoban),
                        new("孫番", string.IsNullOrEmpty(target.GisMagoban) ? DBNull.Value : target.GisMagoban),
                        new("RS区分", string.IsNullOrEmpty(target.GisRsKbn) ? DBNull.Value : target.GisRsKbn),
                        new("システム日時", sysDateTime),
                        new("ユーザID", userId),
                    ];
                    NpgsqlParameter xminParam = new("xmin", NpgsqlTypes.NpgsqlDbType.Xid) { Value = target.GisXmin };
                    updParams.Add(xminParam);
                    int cnt = dbContext.Database.ExecuteSqlRaw(updHikiukeGis.ToString(), updParams);
                    if (cnt == 0)
                    {
                        throw new DBConcurrencyException();
                    }
                    updCount += cnt;
                }
                else
                {
                    dbContext.T11100引受giss.Add(new()
                    {
                        組合等コード = sessionInfo.KumiaitoCd,
                        年産 = (short)sessionInfo.Nensan,
                        共済目的コード = sessionInfo.KyosaiMokutekiCd,
                        組合員等コード = sessionInfo.KumiaiintoCd,
                        耕地番号 = target.KouchiNo,
                        分筆番号 = target.BunpitsuNo,
                        局都道府県コード = target.GisKyokuTodofuken,
                        市区町村コード = target.GisShichoson,
                        大字コード = target.GisOoaza,
                        小字コード = target.GisKoaza,
                        地番 = target.GisChiban,
                        枝番 = target.GisEdaban,
                        子番 = target.GisKoban,
                        孫番 = target.GisMagoban,
                        rs区分 = target.GisRsKbn,
                        登録日時 = sysDateTime,
                        登録ユーザid = userId,
                        更新日時 = sysDateTime,
                        更新ユーザid = userId
                    });
                    updCount += dbContext.SaveChanges();
                }

            }

            return updCount;
        }

        /// <summary>
        /// t_11090_引受耕地とt_11100_引受gisの対象レコードを登録する
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="transaction"></param>
        /// <param name="hikiukeAddRecords"></param>
        /// <returns>登録件数</returns>
        public int AppendHikiukeGis(ref NskAppContext dbContext,
            D105030SessionInfo sessionInfo,
            string userId,
            DateTime? sysDateTime,
             ref List<D105030HikiukeRecord> hikiukeAddRecords)
        {
            int addCount = 0;

            // t_11090_引受耕地の対象レコードを登録する。
            List<T11090引受耕地> addHikiukeRecs = new();
            foreach (D105030HikiukeRecord target in hikiukeAddRecords)
            {
                addHikiukeRecs.Add(new()
                {
                    組合等コード = sessionInfo.KumiaitoCd,
                    年産 = (short)sessionInfo.Nensan,
                    共済目的コード = sessionInfo.KyosaiMokutekiCd,
                    組合員等コード = sessionInfo.KumiaiintoCd,
                    耕地番号 = target.KouchiNo,
                    分筆番号 = target.BunpitsuNo,
                    類区分 = null,
                    地名地番 = target.ChimeiChiban,
                    耕地面積 = target.HonchiMenseki,
                    引受面積 = target.HikukeMenseki,
                    転作等面積 = target.TensakutoMenseki,
                    受委託区分 = target.JuitakushaKbn,
                    受委託者コード = target.Shoyusha,
                    田畑区分 = target.Tahata,
                    区分コード = target.Kbn,
                    種類コード = target.Syurui,
                    用途区分 = "0",
                    品種コード = target.Hinsyu,
                    産地別銘柄コード = target.SanchiMeigara,
                    収量等級コード = target.SyuryoTokyu,
                    参酌コード = target.Sanjaku,
                    実量基準単収 = target.JituryoKijunTansyu,
                    統計市町村コード = target.Shichoson,
                    統計単位地域コード = null,
                    統計単収 = target.ToukeiTansyu,
                    共済金額選択区分 = null,
                    調整係数 = null,
                    備考 = target.Bikou,
                    登録日時 = sysDateTime,
                    登録ユーザid = userId,
                    更新日時 = sysDateTime,
                    更新ユーザid = userId,
                });
            }
            dbContext.T11090引受耕地s.AddRange(addHikiukeRecs);
            dbContext.SaveChanges();
            addCount += addHikiukeRecs.Count;


            // t_11100_引受gisの対象レコードを登録する。
            List<T11100引受gis> addHikiukeGisRecs = new();
            foreach (D105030HikiukeRecord target in hikiukeAddRecords)
            {
                addHikiukeGisRecs.Add(new()
                {
                    組合等コード = sessionInfo.KumiaitoCd,
                    年産 = (short)sessionInfo.Nensan,
                    共済目的コード = sessionInfo.KyosaiMokutekiCd,
                    組合員等コード = sessionInfo.KumiaiintoCd,
                    耕地番号 = target.KouchiNo,
                    分筆番号 = target.BunpitsuNo,
                    rs区分 = target.GisRsKbn,
                    局都道府県コード = target.GisKyokuTodofuken,
                    市区町村コード = target.GisShichoson,
                    大字コード = target.GisOoaza,
                    小字コード = target.GisKoaza,
                    地番 = target.GisChiban,
                    枝番 = target.GisEdaban,
                    子番 = target.GisKoban,
                    孫番 = target.GisMagoban,
                    登録日時 = sysDateTime,
                    登録ユーザid = userId,
                    更新日時 = sysDateTime,
                    更新ユーザid = userId
                });
            }
            dbContext.T11100引受giss.AddRange(addHikiukeGisRecs);
            addCount += dbContext.SaveChanges();

            return addCount;
        }


        /// <summary>
        /// 画面入力値をこのこのクラスに反映する
        /// </summary>
        /// <param name="src"></param>
        public void ApplyInput(D105030HikiukeSearchResult src)
        {
            this.DispRecords = src.DispRecords;
            this.AllRecCount = src.AllRecCount;
        }

        /// <summary>
        /// 更新対象レコード取得
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="session"></param>
        /// <returns></returns>
        public override List<D105030HikiukeRecord> GetUpdateRecs(ref NskAppContext dbContext, BaseSessionInfo session)
        {
            D105030SessionInfo sessionInfo = (D105030SessionInfo)session;
            List<D105030HikiukeRecord> updRecs = new();

            // 検索結果取得
            List<D105030HikiukeRecord> dbResults = GetResult(dbContext, sessionInfo);

            // 検索結果と画面入力値を比較
            foreach (D105030HikiukeRecord dispRec in DispRecords)
            {
                // 追加行、削除行以外を対象とする
                if (dispRec is BasePagerRecord pagerRec && !pagerRec.IsNewRec && !pagerRec.IsDelRec)
                {
                    D105030HikiukeRecord dbRec = dbResults.SingleOrDefault(x =>
                        (x.KouchiNo == dispRec.KouchiNo) &&
                        (x.BunpitsuNo == dispRec.BunpitsuNo)
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
    }
}
