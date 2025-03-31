using System.Text;
using CoreLibrary.Core.Exceptions;
using CoreLibrary.Core.Utility;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using NskAppModelLibrary.Context;
using NskAppModelLibrary.Models;
using NskCommonLibrary.Core.Consts;

namespace NSK_B111041.Models
{
    /// <summary>
    /// 条件バッチ
    /// </summary>
    public class BatchJoken
    {
        /// <summary>
        /// 組合等コード
        /// </summary>
        public string JokenKumiaitoCd { get; set; } = string.Empty;

        /// <summary>
        /// 年産
        /// </summary>
        public string JokenNensan { get; set; } = string.Empty;

        /// <summary>
        /// 負担金交付区分
        /// </summary>
        public string JokenFutankinKofuKbn { get; set; } = string.Empty;

        /// <summary>
        /// 交付回
        /// </summary>
        public string JokenKoufuKai { get; set; } = string.Empty;

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
        /// 「支所コード存在情報」を取得する。
        /// </summary>
        /// <param name="dbContext">DBコンテキスト</param>
        /// <param name="todofukenCd">都道府県コード</param>
        /// <param name="kumiaitoCd">組合等コード</param>
        /// <param name="shishoCd">支所コード</param>
        /// <returns></returns>
        public static int GetShishoCdSonzaiJoho(NskAppContext dbContext, string todofukenCd, string kumiaitoCd, string shishoCd)
        {
            int shisho = dbContext.VShishoNms
                 .Where(x => x.TodofukenCd == todofukenCd && x.KumiaitoCd == kumiaitoCd & x.ShishoCd == shishoCd)
                 .Count();

            return shisho;
        }

        /// <summary>
        /// 交付金申請書データの取得
        /// </summary>
        /// <param name="dbContext">DBコンテキスト</param>
        /// <param name="todofukenCd">都道府県コード</param>
        /// <param name="kumiaitoCd">組合等コードコード</param>
        /// <param name="batchJoken">バッチ条件</param>
        /// <returns></returns>
        public static List<KoufuKinShinseiRecord> GetKoufuKinShinsei(NskAppContext dbContext, string todofukenCd, string kumiaitoCd, BatchJoken batchJoken)
        {
            StringBuilder sql = new();
            sql.Append($@"SELECT ");
            sql.Append($@"    T3.kumiaito_rnm AS 連合会略称 ");;
            sql.Append($@"    , T1.負担金交付区分 ");
            sql.Append($@"    , T1.組合等コード ");
            sql.Append($@"    , T1.組合等コード AS 組合等合併時識別番号 ");
            sql.Append($@"    , T1.交付回 ");
            sql.Append($@"    , T1.組合等別国庫負担金 ");
            sql.Append($@"    , T1.保険料総額 AS 組合等別連合会保険料 ");
            sql.Append($@"    , COALESCE(T1.保険料総額, 0) - COALESCE(T1.組合等別国庫負担金, 0) AS 組合等別連合会負担金交付対象額 ");
            sql.Append($@"    , T1.組合員等負担共済掛金 ");
            sql.Append($@"    , T1.組合員等負担共済掛金徴収済額 ");
            sql.Append($@"    , T1.共済掛金徴収割合 ");
            sql.Append($@"    , T1.組合等交付金の金額 ");
            sql.Append($@"    , T1.年産 ");
            sql.Append($@"FROM ");
            sql.Append($@"    t_15020_組合等交付 T1 ");
            sql.Append($@"    INNER JOIN v_kumiaito T3 ");
            sql.Append($@"        ON T3.todofuken_cd = @都道府県コード ");
            sql.Append($@"        AND T3.kumiaito_cd = T1.組合等コード ");
            sql.Append($@"WHERE ");
            sql.Append($@"    T1.組合等コード = @条件_組合等コード ");
            sql.Append($@"    AND T1.年産 = @条件_年産 ");
            sql.Append($@"    AND T1.負担金交付区分 = @条件_負担金交付区分 ");
            sql.Append($@"    AND T1.交付回 = @条件_交付回 ");

            // パラメータに値を付与する
            List<NpgsqlParameter> parameters =
            [
                new("条件_組合等コード", batchJoken.JokenKumiaitoCd),
                new("条件_年産", int.Parse(batchJoken.JokenNensan)),
                new("条件_負担金交付区分", batchJoken.JokenFutankinKofuKbn),
                new("条件_交付回", int.Parse(batchJoken.JokenKoufuKai)),
                new("都道府県コード", todofukenCd),
            ];

            // SQLのクエリ結果をListに格納する
            List<KoufuKinShinseiRecord> KoufuKin = dbContext.Database.SqlQueryRaw<KoufuKinShinseiRecord>(sql.ToString(), parameters.ToArray()).ToList();

            return KoufuKin;
        }

        /// <summary>
        /// バッチ条件を取得
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="jid"></param>
        /// <exception cref="AppException"></exception>
        public void GetBatchJoken(NskAppContext dbContext, string jid)
        {

            // ５．２．１．条件名定数から以下の項目を取得し、設定値をList<string>に格納する。
            List<string> jokenNames =
            [
                JoukenNameConst.JOUKEN_KUMIAITO_CD,           // 組合等コード
                JoukenNameConst.JOUKEN_NENSAN,                // 年産
                JoukenNameConst.JOUKEN_FUTANKIN_KOFU_KBN,     // 負担金交付区分
                JoukenNameConst.JOUKEN_KOUFU_KAI,             // 交付回
                JoukenNameConst.JOUKEN_MOJI_CD                // 文字コード
            ];

            // ５．２．２．[変数：バッチ条件のキー情報]とListをキーにバッチ条件テーブルから「バッチ条件情報」を取得する。
            // バッチ条件プロパティモデルは作成しない
            List<T01050バッチ条件> batchJokens = GetbatchJokens(dbContext, jid, jokenNames);

            // ５．２．３．「バッチ条件情報」が0件の場合
            if (batchJokens.Count == 0)
            {
                // 以下のエラーメッセージを設定し、「１０．」へ進む。
                throw new AppException("ME01645", MessageUtil.Get("ME01645", "パラメータの取得"));
            }

            // ５．３．バッチ条件情報のチェック
            // ５．３．１．取得した「バッチ条件情報」のうち条件名称が下記と一致するデータのを条件値を変数に設定する。
            foreach (T01050バッチ条件 joken in batchJokens)
            {
                switch (joken.条件名称)
                {
                    case JoukenNameConst.JOUKEN_KUMIAITO_CD:          // 組合等コード　※必須
                        JokenKumiaitoCd = joken.条件値;
                        break;
                    case JoukenNameConst.JOUKEN_NENSAN:               // 年産　※必須
                        JokenNensan = joken.条件値;
                        break;
                    case JoukenNameConst.JOUKEN_FUTANKIN_KOFU_KBN:    // 共済目的　※必須
                        JokenFutankinKofuKbn = joken.条件値;
                        break;
                    case JoukenNameConst.JOUKEN_KOUFU_KAI:            // 支所　※必須
                        JokenKoufuKai = joken.条件値;
                        break;
                    case JoukenNameConst.JOUKEN_MOJI_CD:              // 文字コード　※必須
                        JokenMojiCd = joken.条件値;
                        break;
                }

            }

        }

    }

}