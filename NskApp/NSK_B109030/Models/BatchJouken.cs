using CoreLibrary.Core.Exceptions;
using CoreLibrary.Core.Utility;
using NskAppModelLibrary.Context;
using NskAppModelLibrary.Models;
using NskCommonLibrary.Core.Consts;

namespace NSK_B109030.Models
{
    /// <summary>
    /// バッチ条件情報
    /// </summary>
    class BatchJouken
    {
        /// <summary>
        /// 条件_組合等コード
        /// </summary>
        public string JoukenKumiaitoCd { get; set; } = string.Empty;

        /// <summary>
        /// 条件_年産
        /// </summary>
        public string JoukenNensan { get; set; } = string.Empty;

        /// <summary>
        /// 条件_共済目的コード
        /// </summary>
        public string JoukenKyosaiMokutekiCd { get; set; } = string.Empty;

        /// <summary>
        /// 条件_類区分
        /// </summary>
        public string JoukenRuiKbn {  get; set; } = string.Empty;

        /// <summary>
        /// 条件_出力順キー１
        /// </summary>
        public string JoukenOrderByKey1 { get; set; } = string.Empty;

        /// <summary>
        /// 条件_出力順１
        /// </summary>
        public string JoukenOrderBy1 { get; set; } = string.Empty;

        /// <summary>
        /// 条件_文字コード
        /// </summary>
        public string JoukenMojiCd { get; set; } = string.Empty;

        /// <summary>
        /// バッチ条件情報の取得
        /// </summary>
        /// <param name="dbContext">DBコンテキスト</param>
        /// <param name="jid">バッチ条件のキー情報</param>
        /// <exception cref="AppException"></exception>
        public void GetBatchJoukens(NskAppContext dbContext, string jid)
        {
            // ５．１．１．条件名定数から以下の項目を取得し、設定値をList<string> に格納する。
            // 条件名称の取得
            // 条件名定数から以下の項目を取得し、設定値をList<string> に格納する。
            List<string> jokenNames =
            [
                    JoukenNameConst.JOUKEN_KUMIAITO,            // 組合等コード
                    JoukenNameConst.JOUKEN_NENSAN,              // 年産
                    JoukenNameConst.JOUKEN_KYOSAI_MOKUTEKI,     // 共済目的コード
                    JoukenNameConst.JOUKEN_RUI_KBN,             // 類区分
                    JoukenNameConst.JOUKEN_ORDER_BY_KEY1,       // 表示順キー１
                    JoukenNameConst.JOUKEN_ORDER_BY1,           // 表示順１
                    JoukenNameConst.JOUKEN_MOJI_CD              // 文字コード
            ];

            // ５．１．２．[変数：バッチ条件のキー情報]とListをキーにバッチ条件テーブルから「バッチ条件情報」を取得する。
            List<T01050バッチ条件> batchJokens = dbContext.T01050バッチ条件s
                .Where(x => x.バッチ条件id == jid && jokenNames.Contains(x.条件名称))
                .ToList();

            // ５．１．３．「バッチ条件情報」が0件の場合、
            if (batchJokens.Count == 0)
            {
                // [変数：エラーメッセージ]に、以下のエラーメッセージを設定し、「９.」へ進む。
                // （"ME01645"、引数{0}：パラメータの取得)
                throw new AppException("ME01645", MessageUtil.Get("ME01645", "パラメータの取得"));
            }

            // 条件値のリストからバッチ条件情報への値設定
            foreach (T01050バッチ条件 joken in batchJokens)
            {
                switch (joken.条件名称)
                {
                    case JoukenNameConst.JOUKEN_KUMIAITO:           // 組合等コード　  ※必須
                        JoukenKumiaitoCd = joken.条件値;
                        break;
                    case JoukenNameConst.JOUKEN_NENSAN:             // 年産　          ※必須
                        JoukenNensan = joken.条件値;
                        break;
                    case JoukenNameConst.JOUKEN_KYOSAI_MOKUTEKI:    // 共済目的コード　※必須
                        JoukenKyosaiMokutekiCd = joken.条件値;
                        break;
                    case JoukenNameConst.JOUKEN_RUI_KBN:            // 類区分
                        JoukenRuiKbn = joken.条件値;
                        break;
                    case JoukenNameConst.JOUKEN_ORDER_BY_KEY1:      // 表示順キー１
                        JoukenOrderByKey1 = joken.条件値;
                        break;
                    case JoukenNameConst.JOUKEN_ORDER_BY1:          // 表示順１
                        JoukenOrderBy1 = joken.条件値;
                        break;
                    case JoukenNameConst.JOUKEN_MOJI_CD:            // 文字コード
                        JoukenMojiCd = joken.条件値;
                        break;
                }
            }

            // ５．２．２．[変数：条件_組合等コード]がnullまたは空文字の場合
            if (string.IsNullOrEmpty(JoukenKumiaitoCd))
            {
                // 以下のエラーメッセージを設定し、ERRORログに出力して「９.」へ進む。
                // （"ME01054" 引数{0} ：条件_組合等コード)
                throw new AppException("ME01054", MessageUtil.Get("ME01054", "条件_組合等コード"));
            }

            // ５．２．３．[変数：条件_年産]がnullまたは空文字の場合
            if (string.IsNullOrEmpty(JoukenNensan))
            {
                // 以下のエラーメッセージを設定し、ERRORログに出力して「９.」へ進む。
                // （"ME01054"　引数{0} ：条件_年産)
                throw new AppException("ME01054", MessageUtil.Get("ME01054", "条件_年産"));
            }

            // ５．２．４．[変数：条件_共済目的コード]がnullまたは空文字の場合
            if (string.IsNullOrEmpty(JoukenKyosaiMokutekiCd))
            {
                // 以下のエラーメッセージを設定し、ERRORログに出力して「９.」へ進む。
                // （"ME01054"　引数{0} ：条件_共済目的コード)
                throw new AppException("ME01054", MessageUtil.Get("ME01054", "条件_共済目的コード"));
            }
        }

        /// <summary>
        /// コードの整合性チェック
        /// </summary>
        /// <param name="dbContext">DBコンテキスト</param>
        /// <param name="todofukenCd">都道府県コード</param>
        /// <param name="kumiaitoCd">組合等コード</param>
        /// <param name="shishoCd">支所コード</param>
        /// <exception cref="AppException"></exception>
        public void IsConsistency(NskAppContext dbContext, string todofukenCd, string kumiaitoCd, string shishoCd)
        {
            // ６．１．「都道府県コード存在情報」を取得する。
            int todofuken = dbContext.VTodofukens
                .Where(x => x.TodofukenCd == todofukenCd)
                .Select(x => x.TodofukenCd)
                .Count();
            // ６．２．データが取得できない場合（該当データがマスタデータに登録されていない場合）
            if (todofuken == 0)
            {
                // 以下のエラーメッセージを設定し、ERRORログに出力して「９.」へ進む。
                // （"ME10005"　引数{0} ：都道府県コード)
                throw new AppException("ME10005", MessageUtil.Get("ME10005", "都道府県コード"));
            }

            // ６．３．[変数：組合等コード] が入力されている場合
            if (!string.IsNullOrEmpty(kumiaitoCd))
            {
                // 「組合等コード存在情報」を取得する。
                int kumiaito = dbContext.VKumiaitos
                     .Where(x => x.TodofukenCd == todofukenCd && x.KumiaitoCd == kumiaitoCd)
                     .Select(x => x.KumiaitoCd)
                     .Count();
                // ６．４．データが取得できない場合（該当データがマスタデータに登録されていない場合）
                if (kumiaito == 0)
                {
                    // 以下のエラーメッセージを設定し、ERRORログに出力して「９.」へ進む。
                    // （"ME10005"　引数{0} ：組合等コード)
                    throw new AppException("ME10005", MessageUtil.Get("ME10005", "組合等コード"));
                }
            }

            // ６．５．[変数：支所コード]が入力されている場合
            if (!string.IsNullOrEmpty(shishoCd))
            {
                // 「支所コード存在情報」を取得する。
                int shisho = dbContext.VShishoNms
                    .Where(x => x.TodofukenCd == todofukenCd && x.KumiaitoCd == kumiaitoCd && x.ShishoCd == shishoCd)
                    .Select(x => x.ShishoCd)
                    .Count();
                // ６．６．データが取得できない場合（該当データがマスタデータに登録されていない場合）
                if (shisho == 0)
                {
                    // 以下のエラーメッセージを設定し、ERRORログに出力して「９.」へ進む。
                    // （"ME10005"　引数{0} ：支所コード)
                    throw new AppException("ME10005", MessageUtil.Get("ME10005", "支所コード"));
                }
            }

            // ６．７．[配列：バッチ条件] から組合等コードが取得できた場合
            if (!string.IsNullOrEmpty(JoukenKumiaitoCd))
            {
                // 「検索条件組合等コード存在情報」を取得する。
                int kumiaito = dbContext.VKumiaitos
                     .Where(x => x.TodofukenCd == todofukenCd && x.KumiaitoCd == JoukenKumiaitoCd)
                     .Select(x => x.KumiaitoCd)
                     .Count();
                // ６．８．データが取得できない場合（該当データがマスタデータに登録されていない場合）
                if (kumiaito == 0)
                {
                    // 以下のエラーメッセージを設定し、ERRORログに出力して「９.」へ進む。
                    // （"ME10005"　引数{0} ：条件_組合等コード)
                    throw new AppException("ME10005", MessageUtil.Get("ME10005", "条件_組合等コード"));
                }
            }

            // ６．９．[配列：バッチ条件]から共済目的コードが取得できた場合
            if (!string.IsNullOrEmpty(JoukenKyosaiMokutekiCd))
            {
                // 「共済目的コード存在情報」を取得する。
                int kyosaiMokuteki = dbContext.M00010共済目的名称s
                    .Where(x => x.共済目的コード == JoukenKyosaiMokutekiCd)
                    .Select(x => x.共済目的コード)
                    .Count();
                // ６．１０．データが取得できない場合（該当データがマスタデータに登録されていない場合）
                if (kyosaiMokuteki == 0)
                {
                    // 以下のエラーメッセージを設定し、ERRORログに出力して「９.」へ進む。
                    // （"ME10005"　引数{0} ：条件_共済目的コード)
                    throw new AppException("ME10005", MessageUtil.Get("ME10005", "条件_共済目的コード"));
                }
            }

            // ６．１１．[配列：バッチ条件]から類区分が取得できた場合
            if (!string.IsNullOrEmpty(JoukenRuiKbn))
            {
                // 「条件_類区分存在情報」を取得する。
                int kyosaiMokuteki = dbContext.M00020類名称s
                    .Where(x => x.共済目的コード == JoukenKyosaiMokutekiCd && x.類区分 == JoukenRuiKbn)
                    .Select(x => x.共済目的コード)
                    .Count();
                // ６．１２．データが取得できない場合（該当データがマスタデータに登録されていない場合）
                if (kyosaiMokuteki == 0)
                {
                    // 以下のエラーメッセージを設定し、ERRORログに出力して「９.」へ進む。
                    // （"ME10005"　引数{0} ：条件_類区分)
                    throw new AppException("ME10005", MessageUtil.Get("ME10005", "条件_類区分"));
                }
            }
        }
    }
}