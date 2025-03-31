using System.Text;
using CoreLibrary.Core.Exceptions;
using CoreLibrary.Core.Utility;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using NSK_B105200.Common;
using NskAppModelLibrary.Context;
using NskAppModelLibrary.Models;
using NskCommonLibrary.Core.Consts;

namespace NSK_B105200.Models
{
    /// <summary>
    /// 条件バッチ
    /// </summary>
    public class BatchJoken
    {
        /// <summary>
        /// 年産
        /// </summary>
        public string JokenNensan { get; set; } = string.Empty;

        /// <summary>
        /// 共済目的コード
        /// </summary>
        public string JokenKyosaiMokutekiCd { get; set; } = string.Empty;

        /// <summary>
        /// 大地区
        /// </summary>
        public string JokenDaichiku { get; set; } = string.Empty;

        /// <summary>
        /// 小地区（開始）
        /// </summary>
        public string JokenShochikuStart { get; set; } = string.Empty;

        /// <summary>
        /// 小地区（終了）
        /// </summary>
        public string JokenShochikuEnd { get; set; } = string.Empty;

        /// <summary>
        /// 組合員等コード（開始）
        /// </summary>
        public string JokenKumiaiintoCdStart { get; set; } = string.Empty;

        /// <summary>
        /// 組合員等コード（終了）
        /// </summary>
        public string JokenKumiaiintoCdEnd { get; set; } = string.Empty;

        /// <summary>
        /// 類区分
        /// </summary>
        public string JokenRuiKubun { get; set; } = string.Empty;

        /// <summary>
        /// 更新日時（開始）
        /// </summary>
        public string JokenUpdateDateStart { get; set; } = string.Empty;

        /// <summary>
        /// 更新日時（終了）
        /// </summary>
        public string JokenUpdateDateEnd { get; set; } = string.Empty;

        /// <summary>
        /// 表示順キー１
        /// </summary>
        public string JokenOrderByKey1 { get; set; } = string.Empty;

        /// <summary>
        /// 表示順１
        /// </summary>
        public string JokenOrderBy1 { get; set; } = string.Empty;

        /// <summary>
        /// 表示順キー２
        /// </summary>
        public string JokenOrderByKey2 { get; set; } = string.Empty;

        /// <summary>
        /// 表示順２
        /// </summary>
        public string JokenOrderBy2 { get; set; } = string.Empty;

        /// <summary>
        /// 表示順キー３
        /// </summary>
        public string JokenOrderByKey3 { get; set; } = string.Empty;

        /// <summary>
        /// 表示順３
        /// </summary>
        public string JokenOrderBy3 { get; set; } = string.Empty;

        /// <summary>
        /// ファイル名
        /// </summary>
        public string JokenFileName { get; set; } = string.Empty;

        /// <summary>
        /// 文字コード
        /// </summary>
        public string JokenMojiCd { get; set; } = string.Empty;

        /// <summary>
        /// 「バッチ条件情報」を取得する。
        /// </summary>
        /// <param name="dbContext">DBコンテキスト</param>
        /// <param name="jid">変数：バッチ条件ID</param>
        /// <param name="jokenNames">条件名のList</param>
        /// <returns></returns>
        public static List<T01050バッチ条件> GetbatchJokens(NskAppContext dbContext, string jid, List<string> jokenNames)
        {
            List<T01050バッチ条件> batchJokens = dbContext.T01050バッチ条件s
                .Where(x => x.バッチ条件id == jid && jokenNames.Contains(x.条件名称))
                .ToList();

            return batchJokens;
        }

        /// <summary>
        /// 「都道府県コード存在情報」を取得する。
        /// </summary>
        /// <param name="dbContext">DBコンテキスト</param>
        /// <param name="todofukenCd">都道府県コード</param>
        /// <returns></returns>
        public static int GetTodofukenCdSonzaiJoho(NskAppContext dbContext, string todofukenCd)
        {
            int todofuken = dbContext.VTodofukens
                 .Where(x => x.TodofukenCd == todofukenCd)
                 .Count();

            return todofuken;
        }

        /// <summary>
        /// 「共済目的コード存在情報」を取得する。
        /// </summary>
        /// <param name="dbContext">DBコンテキスト</param>
        /// <param name="kyosaiMokutekiCd">共済目的コード</param>
        /// <returns></returns>
        public static int GetKyosaiMokutekiCdSonzaiJoho(NskAppContext dbContext, string kyosaiMokutekiCd)
        {
            int kyosaiMokuteki = dbContext.M00010共済目的名称s
                 .Where(x => x.共済目的コード == kyosaiMokutekiCd)
                 .Count();

            return kyosaiMokuteki;
        }

        /// <summary>
        /// 「組合等コード存在情報」を取得する。
        /// </summary>
        /// <param name="dbContext">DBコンテキスト</param>
        /// <param name="todofukenCd">都道府県コード</param>
        /// <param name="kumiaitoCd">組合等コード</param>
        /// <returns></returns>
        public static int GetKumiaitoCdSonzaiJoho(NskAppContext dbContext, string todofukenCd, string kumiaitoCd)
        {
            int kumiaito = dbContext.VKumiaitos
                 .Where(x => x.TodofukenCd == todofukenCd && x.KumiaitoCd == kumiaitoCd)
                 .Count();

            return kumiaito;
        }

        /// <summary>
        /// 「大地区コード存在情報」を取得する。
        /// </summary>
        /// <param name="dbContext">DBコンテキスト</param>
        /// <param name="todofukenCd">都道府県コード</param>
        /// <param name="kumiaitoCd">組合等コード</param>
        /// <param name="daichikuCd">大地区コード</param>
        /// <returns></returns>
        public static int GetDaichikuSonzaiJoho(NskAppContext dbContext, string todofukenCd, string kumiaitoCd, string daichikuCd)
        {
            int daichiku = dbContext.VDaichikuNms
                 .Where(x => x.TodofukenCd == todofukenCd && x.KumiaitoCd == kumiaitoCd & x.DaichikuCd == daichikuCd)
                 .Count();

            return daichiku;
        }

        /// <summary>
        /// 共済金額一覧表データの取得
        /// </summary>
        /// <param name="dbContext">DBコンテキスト</param>
        /// <param name="todofukenCd">都道府県コード</param>
        /// <param name="kumiaitoCd">組合等コードコード</param>
        /// <param name="batchJoken">バッチ条件</param>
        /// <returns></returns>
        public static List<KyousaiKingakuRecord> GetKyousaiKingaku(NskAppContext dbContext, string todofukenCd, string kumiaitoCd, string shishoCd, BatchJoken batchJoken)
        {
            StringBuilder sql = new();

            #region 取得項目
            sql.Append($" SELECT ");
            sql.Append($" 	T1.組合等コード ");
            sql.Append($"  ,T2.kumiaito_nm AS 組合等名 ");
            sql.Append($"  ,T1.年産 ");
            sql.Append($"  ,T1.共済目的コード ");
            sql.Append($"  ,T7.共済目的名称 AS 共済目的名 ");
            sql.Append($"  ,T9.引受方式名称 ");
            sql.Append($"  ,T1.類区分 ");
            sql.Append($"  ,T8.類名称 AS 類区分名 ");
            sql.Append($"  ,T6.shisho_cd AS 支所コード ");
            sql.Append($"  ,T3.shisho_nm AS 支所名 ");
            sql.Append($"  ,T6.daichiku_cd AS 大地区コード ");
            sql.Append($"  ,T4.daichiku_nm AS 大地区名 ");
            sql.Append($"  ,T6.shochiku_cd AS 小地区コード ");
            sql.Append($"  ,T5.shochiku_nm AS 小地区名 ");
            sql.Append($"  ,T1.組合員等コード ");
            sql.Append($"  ,T6.hojin_full_nm AS 組合員等氏名 ");
            sql.Append($"  ,T1.共済金額 ");
            #endregion

            #region 検索テーブル
            sql.Append($" FROM	 ");
            sql.Append($"   t_12080_組合員等別共済金額設定 T1  ");
            sql.Append($"   INNER JOIN v_kumiaito T2  ");
            sql.Append($"     ON @都道府県コード = T2.todofuken_cd ");
            sql.Append($"     AND T1.組合等コード = T2.kumiaito_cd ");
            sql.Append($"   INNER JOIN v_shisho_nm T3  ");
            sql.Append($"     ON @都道府県コード = T3.todofuken_cd ");
            sql.Append($"     AND T2.kumiaito_cd = T3.kumiaito_cd ");
            sql.Append($"   INNER JOIN v_daichiku_nm T4  ");
            sql.Append($"     ON @都道府県コード = T4.todofuken_cd ");
            sql.Append($"     AND T3.kumiaito_cd = T4.kumiaito_cd ");
            sql.Append($"   INNER JOIN v_shochiku_nm T5  ");
            sql.Append($"     ON @都道府県コード = T5.todofuken_cd ");
            sql.Append($"     AND T4.kumiaito_cd = T5.kumiaito_cd ");
            sql.Append($"     AND T4.daichiku_cd = T5.daichiku_cd ");
            sql.Append($"   INNER JOIN v_nogyosha T6  ");
            sql.Append($"     ON @都道府県コード = T6.todofuken_cd ");
            sql.Append($"     AND T1.組合員等コード = T6.kumiaiinto_cd ");
            sql.Append($"     AND T2.kumiaito_cd = T6.kumiaito_cd ");
            sql.Append($"     AND T3.shisho_cd = T6.shisho_cd ");
            sql.Append($"     AND T5.daichiku_cd = T6.daichiku_cd ");
            sql.Append($"     AND T5.shochiku_cd = T6.shochiku_cd ");
            sql.Append($"   INNER JOIN m_00010_共済目的名称 T7  ");
            sql.Append($"     ON T1.共済目的コード = T7.共済目的コード ");
            sql.Append($"   INNER JOIN m_00020_類名称 T8  ");
            sql.Append($"     ON T1.共済目的コード = T8.共済目的コード ");
            sql.Append($"     AND T1.類区分 = T8.類区分 ");
            sql.Append($"   LEFT OUTER JOIN m_10080_引受方式名称 T9  ");
            sql.Append($"     ON T9.引受方式 = '4' ");
            #endregion


            // パラメータに値を付与する
            List<NpgsqlParameter> parameters =
            [
                new("条件_年産", int.Parse(batchJoken.JokenNensan)),
                new("条件_共済目的コード", batchJoken.JokenKyosaiMokutekiCd),
                new("組合等コード", kumiaitoCd),
                new("支所コード", shishoCd),
                new("都道府県コード", todofukenCd),
            ];

            #region 検索条件
            sql.Append($" WHERE ");
            sql.Append($"   T1.組合等コード = @組合等コード ");
            sql.Append($"   AND T1.年産 = @条件_年産 ");
            sql.Append($"   AND T1.共済目的コード = @条件_共済目的コード ");
            sql.Append($"   AND T6.shisho_cd = @支所コード ");
            
            // 大地区の指定がある場合
            if (!string.IsNullOrWhiteSpace(batchJoken.JokenDaichiku))
            {
                sql.Append($"   AND T6.daichiku_cd = @条件_大地区コード ");
                parameters.Add(new("条件_大地区コード", batchJoken.JokenDaichiku));
            }

            // 小地区（開始）の指定がある場合
            if (!string.IsNullOrWhiteSpace(batchJoken.JokenShochikuStart))
            {
                sql.Append($"   AND T6.shochiku_cd >= @条件_小地区From ");
                parameters.Add(new("条件_小地区From", batchJoken.JokenShochikuStart));
            }

            // 小地区（終了）の指定がある場合
            if (!string.IsNullOrWhiteSpace(batchJoken.JokenShochikuEnd))
            {
                sql.Append($"   AND T6.shochiku_cd <= @条件_小地区To ");
                parameters.Add(new("条件_小地区To", batchJoken.JokenShochikuEnd));
            }

            // 組合員等コード（開始）の指定がある場合
            if (!string.IsNullOrWhiteSpace(batchJoken.JokenKumiaiintoCdStart))
            {
                sql.Append($"   AND T1.組合員等コード >= @条件_組合員等コードFrom ");
                parameters.Add(new("条件_組合員等コードFrom", batchJoken.JokenKumiaiintoCdStart));
            }

            // 組合員等コード（終了）の指定がある場合
            if (!string.IsNullOrWhiteSpace(batchJoken.JokenKumiaiintoCdEnd))
            {
                sql.Append($"   AND T1.組合員等コード <= @条件_組合員等コードTo ");
                parameters.Add(new("条件_組合員等コードTo", batchJoken.JokenKumiaiintoCdEnd));
            }

            // 類区分の指定がある場合
            if (!string.IsNullOrWhiteSpace(batchJoken.JokenRuiKubun))
            {
                sql.Append($"   AND T1.類区分 = @条件_類区分 ");
                parameters.Add(new("条件_類区分", batchJoken.JokenRuiKubun));
            }

            // 更新日時（開始）の指定がある場合
            if (!string.IsNullOrWhiteSpace(batchJoken.JokenUpdateDateStart))
            {
                sql.Append($"   AND T1.更新日時 >= @条件_更新日時From ");
                parameters.Add(new("条件_更新日時From", DateTime.Parse(batchJoken.JokenUpdateDateStart)));
            }

            // 更新日時（終了）の指定がある場合
            if (!string.IsNullOrWhiteSpace(batchJoken.JokenUpdateDateEnd))
            {
                sql.Append($"   AND T1.更新日時 <= @条件_更新日時To ");
                parameters.Add(new("条件_更新日時To", DateTime.Parse(batchJoken.JokenUpdateDateEnd)));
            }
            #endregion

            #region ソート条件
            sql.Append(" ORDER BY ");

            bool ruikbnSort = false;
            bool daichikuSort = false;
            bool shochikuSort = false;
            bool kumiaiintoSort = false;
            bool updateDateSort = false;
            bool isPutOrder = false;

            // ※「画面：表示順キー１」「画面：表示順キー２」「画面：表示順キー３」のいずれかが選択されている場合
            if (!string.IsNullOrWhiteSpace(batchJoken.JokenOrderByKey1) || !string.IsNullOrWhiteSpace(batchJoken.JokenOrderByKey2) || !string.IsNullOrWhiteSpace(batchJoken.JokenOrderByKey3))
            {

                //  画面指定ソート順
                if (!string.IsNullOrWhiteSpace(batchJoken.JokenOrderByKey1))
                {
                    isPutOrder = true;
                    switch (batchJoken.JokenOrderByKey1)
                    {
                        case Constants.ruikbnSortName:
                            sql.Append($" T1.類区分 {batchJoken.JokenOrderBy1} ");
                            ruikbnSort = true;
                            break;
                        case Constants.daichikuSortName:
                            sql.Append($" T6.daichiku_cd {batchJoken.JokenOrderBy1} ");
                            daichikuSort = true;
                            break;
                        case Constants.shochikuSortName:
                            sql.Append($" T6.shochiku_cd {batchJoken.JokenOrderBy1} ");
                            shochikuSort = true;
                            break;
                        case Constants.kumiaiintoSortName:
                            sql.Append($" T1.組合員等コード {batchJoken.JokenOrderBy1} ");
                            kumiaiintoSort = true;
                            break;
                        case Constants.updateDateSortName:
                            sql.Append($" T1.更新日時 {batchJoken.JokenOrderBy1} ");
                            updateDateSort = true;
                            break;
                    }
                }

                if (!string.IsNullOrWhiteSpace(batchJoken.JokenOrderByKey2))
                {
                    // ソート条件1が出力されていた場合、カンマを付与する
                    if (isPutOrder)
                        sql.Append(", ");

                    isPutOrder = true;
                    switch (batchJoken.JokenOrderByKey2)
                    {
                        case Constants.ruikbnSortName:
                            sql.Append($" T1.類区分 {batchJoken.JokenOrderBy2} ");
                            ruikbnSort = true;
                            break;
                        case Constants.daichikuSortName:
                            sql.Append($" T6.daichiku_cd {batchJoken.JokenOrderBy2} ");
                            daichikuSort = true;
                            break;
                        case Constants.shochikuSortName:
                            sql.Append($" T6.shochiku_cd {batchJoken.JokenOrderBy2} ");
                            shochikuSort = true;
                            break;
                        case Constants.kumiaiintoSortName:
                            sql.Append($" T1.組合員等コード {batchJoken.JokenOrderBy2} ");
                            kumiaiintoSort = true;
                            break;
                        case Constants.updateDateSortName:
                            sql.Append($" T1.更新日時 {batchJoken.JokenOrderBy2} ");
                            updateDateSort = true;
                            break;
                    }
                }
                if (!string.IsNullOrWhiteSpace(batchJoken.JokenOrderByKey3))
                {
                    // ソート条件1or2が出力されていた場合、カンマを付与する
                    if (isPutOrder)
                        sql.Append(", ");

                    isPutOrder = true;
                    switch (batchJoken.JokenOrderByKey3)
                    {
                        case Constants.ruikbnSortName:
                            sql.Append($" T1.類区分 {batchJoken.JokenOrderBy3} ");
                            ruikbnSort = true;
                            break;
                        case Constants.daichikuSortName:
                            sql.Append($" T6.daichiku_cd {batchJoken.JokenOrderBy3} ");
                            daichikuSort = true;
                            break;
                        case Constants.shochikuSortName:
                            sql.Append($" T6.shochiku_cd {batchJoken.JokenOrderBy3} ");
                            shochikuSort = true;
                            break;
                        case Constants.kumiaiintoSortName:
                            sql.Append($" T1.組合員等コード {batchJoken.JokenOrderBy3} ");
                            kumiaiintoSort = true;
                            break;
                        case Constants.updateDateSortName:
                            sql.Append($" T1.更新日時 {batchJoken.JokenOrderBy3} ");
                            updateDateSort = true;
                            break;
                    }
                }
            }

            // ソート条件1or2or3が出力されていた場合、カンマを付与する
            if (isPutOrder)
                sql.Append(", ");

            sql.Append(" T1.組合等コード Asc ");
            sql.Append(" ,T1.年産 Asc ");
            sql.Append(" ,T1.共済目的コード Asc ");

            // 「表示順キー」で類区分の指定がない場合
            if (!ruikbnSort)
                sql.Append(" ,T1.類区分 Asc ");

            sql.Append(" ,T6.shisho_cd Asc ");

            // 「表示順キー」で大地区の指定がない場合
            if (!daichikuSort)
                sql.Append(" ,T6.daichiku_cd Asc ");

            // 「表示順キー」で小地区の指定がない場合
            if (!shochikuSort)
                sql.Append(" ,T6.shochiku_cd Asc ");

            // 「表示順キー」で組合員等コードの指定がない場合
            if (!kumiaiintoSort)
                sql.Append(" ,T1.組合員等コード Asc ");

            // 「表示順キー」で更新日時の指定がない場合
            if (!updateDateSort)
                sql.Append(" ,T1.更新日時 Asc ");


            #endregion


            // SQLのクエリ結果をListに格納する
            List<KyousaiKingakuRecord> KyousaiKingaku = dbContext.Database.SqlQueryRaw<KyousaiKingakuRecord>(sql.ToString(), parameters.ToArray()).ToList();

            return KyousaiKingaku;
        }

        /// <summary>
        /// バッチ条件を取得
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="jid"></param>
        /// <exception cref="AppException"></exception>
        public void GetBatchJoken(NskAppContext dbContext, string jid)
        {

            // ５．１．１．条件名定数から以下の項目を取得し、設計値をList<string>に格納する。
            List<string> jokenNames =
            [
                JoukenNameConst.JOUKEN_NENSAN,                     //年産
                JoukenNameConst.JOUKEN_KYOSAI_MOKUTEKI,            //共済目的コード
                JoukenNameConst.JOUKEN_DAICHIKU,                   //大地区コード
                JoukenNameConst.JOUKEN_SHOCHIKU_START,             //小地区コードFrom
                JoukenNameConst.JOUKEN_SHOCHIKU_END,               //小地区コードTo
                JoukenNameConst.JOUKEN_KUMIAIINTO_CD_START,        //組合員等コードFrom
                JoukenNameConst.JOUKEN_KUMIAIINTO_CD_END,          //組合員等コードTo
                JoukenNameConst.JOUKEN_RUI_KBN,                    //類区分
                JoukenNameConst.JOUKEN_UPDATE_DATE_START,          //更新日時From
                JoukenNameConst.JOUKEN_UPDATE_DATE_END,            //更新日時To
                JoukenNameConst.JOUKEN_ORDER_BY_KEY1,              //表示順キー１
                JoukenNameConst.JOUKEN_ORDER_BY1,                  //表示順１
                JoukenNameConst.JOUKEN_ORDER_BY_KEY2,              //表示順キー２
                JoukenNameConst.JOUKEN_ORDER_BY2,                  //表示順２
                JoukenNameConst.JOUKEN_ORDER_BY_KEY3,              //表示順キー３
                JoukenNameConst.JOUKEN_ORDER_BY3,                  //表示順３
                JoukenNameConst.JOUKEN_FILE_NAME,                  //ファイル名
                JoukenNameConst.JOUKEN_MOJI_CD                     //文字コード

            ];

            // ５．１．２．[変数：バッチ条件のキー情報]とListをキーにバッチ条件テーブルから「バッチ条件情報」を取得する。
            List<T01050バッチ条件> batchJokens = GetbatchJokens(dbContext, jid, jokenNames);

            // ５．１．３．「バッチ条件情報」が0件の場合、エラーメッセージを設定し、ERRORログに出力して「１０.」へ進む。
            if (batchJokens.Count == 0)
            {
                // 以下のエラーメッセージを設定し、「１０．」へ進む。
                throw new AppException("ME01645", MessageUtil.Get("ME01645", "パラメータの取得"));
            }

            // ５．２．バッチ条件情報のチェック
            // ５．２．１．取得した「バッチ条件情報」のうち条件名称が下記と一致するデータを条件値を変数に設定する。ｙ
            foreach (T01050バッチ条件 joken in batchJokens)
            {
                switch (joken.条件名称)
                {
                    case JoukenNameConst.JOUKEN_NENSAN:                 // 年産　※必須
                        JokenNensan = joken.条件値;
                        break;
                    case JoukenNameConst.JOUKEN_KYOSAI_MOKUTEKI:        // 共済目的コード　※必須
                        JokenKyosaiMokutekiCd = joken.条件値;
                        break;
                    case JoukenNameConst.JOUKEN_DAICHIKU:               // 大地区コード
                        JokenDaichiku = joken.条件値;
                        break;
                    case JoukenNameConst.JOUKEN_SHOCHIKU_START:         // 小地区コードFrom
                        JokenShochikuStart = joken.条件値;
                        break;
                    case JoukenNameConst.JOUKEN_SHOCHIKU_END:           // 小地区コードTo
                        JokenShochikuEnd = joken.条件値;
                        break;
                    case JoukenNameConst.JOUKEN_KUMIAIINTO_CD_START:    // 組合員等コードFrom
                        JokenKumiaiintoCdStart = joken.条件値;
                        break;
                    case JoukenNameConst.JOUKEN_KUMIAIINTO_CD_END:      // 組合員等コードTo
                        JokenKumiaiintoCdEnd = joken.条件値;
                        break;
                    case JoukenNameConst.JOUKEN_RUI_KBN:                // 類区分
                        JokenRuiKubun = joken.条件値;
                        break;
                    case JoukenNameConst.JOUKEN_UPDATE_DATE_START:      // 更新日時From
                        JokenUpdateDateStart = joken.条件値;
                        break;
                    case JoukenNameConst.JOUKEN_UPDATE_DATE_END:        // 更新日時To
                        JokenUpdateDateEnd = joken.条件値;
                        break;
                    case JoukenNameConst.JOUKEN_ORDER_BY_KEY1:          // 表示順キー１
                        JokenOrderByKey1 = joken.条件値;
                        break;
                    case JoukenNameConst.JOUKEN_ORDER_BY1:              // 表示順１
                        JokenOrderBy1 = joken.条件値;
                        break;
                    case JoukenNameConst.JOUKEN_ORDER_BY_KEY2:          // 表示順キー２
                        JokenOrderByKey2 = joken.条件値;
                        break;
                    case JoukenNameConst.JOUKEN_ORDER_BY2:              // 表示順２
                        JokenOrderBy2 = joken.条件値;
                        break;
                    case JoukenNameConst.JOUKEN_ORDER_BY_KEY3:          // 表示順キー３
                        JokenOrderByKey3 = joken.条件値;
                        break;
                    case JoukenNameConst.JOUKEN_ORDER_BY3:              // 表示順３
                        JokenOrderBy3 = joken.条件値;
                        break;
                    case JoukenNameConst.JOUKEN_FILE_NAME:              // ファイル名
                        JokenFileName = joken.条件値;
                        break;
                    case JoukenNameConst.JOUKEN_MOJI_CD:                // 文字コード
                        JokenMojiCd = joken.条件値;
                        break;

                }

            }

        }

    }

}