using CoreLibrary.Core.Exceptions;
using CoreLibrary.Core.Utility;
using NskAppModelLibrary.Context;
using NskAppModelLibrary.Models;
using NskCommonLibrary.Core.Consts;

namespace NSK_B105012.Models
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
        /// 共済目的コード
        /// </summary>
        public string JokenKyosaiMokutekitoCd { get; set; } = string.Empty;

        /// <summary>
        /// 支所
        /// </summary>
        public string JokenShisho { get; set; } = string.Empty;

        /// <summary>
        /// 市町村
        /// </summary>
        public string JokenShichoson { get; set; } = string.Empty;

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
        /// 組合員等コードFrom
        /// </summary>
        public string JokenKumiaiintoCdStart { get; set; } = string.Empty;

        /// <summary>
        /// 組合員等コードTo
        /// </summary>
        public string JokenKumiaiintoCdEnd { get; set; } = string.Empty;

        /// <summary>
        /// 出力順1
        /// </summary>
        public string JokenShuturyokujun1 { get; set; } = string.Empty;

        /// <summary>
        /// 昇順・降順1
        /// </summary>
        public string JokenShojunKojun1 { get; set; } = string.Empty;

        /// <summary>
        /// 出力順2
        /// </summary>
        public string JokenShuturyokujun2 { get; set; } = string.Empty;

        /// <summary>
        /// 昇順・降順2
        /// </summary>
        public string JokenShojunKojun2 { get; set; } = string.Empty;

        /// <summary>
        /// 出力順3
        /// </summary>
        public string JokenShuturyokujun3 { get; set; } = string.Empty;

        /// <summary>
        /// 昇順・降順3
        /// </summary>
        public string JokenShojunKojun3 { get; set; } = string.Empty;

        /// <summary>
        /// ファイル名
        /// </summary>
        public string JokenFileName { get; set; } = string.Empty;

        /// <summary>
        /// 文字コード
        /// </summary>
        public string JokenMojiCd { get; set; } = string.Empty;

        /// <summary>
        /// バッチ条件を取得する
        /// </summary>
        /// <param name="dbContext">DBコンテキスト</param>
        /// <param name="jid">バッチ条件ID</param>
        public void GetBatchJoukens(NskAppContext dbContext, string jid)
        {
            //５．１．バッチ条件情報の取得
            //５．１．１．条件名定数から以下の項目を取得し、設定値をList<string> に格納する。
            List<string> joukenNames =
            [
                    JoukenNameConst.JOUKEN_KUMIAITO,              // 組合等
                    JoukenNameConst.JOUKEN_NENSAN,                // 年産
                    JoukenNameConst.JOUKEN_KYOSAI_MOKUTEKI,       // 共済目的
                    JoukenNameConst.JOUKEN_SHISHO,                // 支所
                    JoukenNameConst.JOUKEN_SHICHOSON,             // 市町村
                    JoukenNameConst.JOUKEN_DAICHIKU,              // 大地区
                    JoukenNameConst.JOUKEN_SHOCHIKU_START,        // 小地区（開始）
                    JoukenNameConst.JOUKEN_SHOCHIKU_END,          // 小地区（終了）
                    JoukenNameConst.JOUKEN_KUMIAIINTO_CD_START,   // 組合員等コードFrom
                    JoukenNameConst.JOUKEN_KUMIAIINTO_CD_END,     // 組合員等コードTo
                    JoukenNameConst.JOUKEN_ORDER_BY_KEY1,         // 出力順1
                    JoukenNameConst.JOUKEN_ORDER_BY1,             // 昇順・降順1
                    JoukenNameConst.JOUKEN_ORDER_BY_KEY2,         // 出力順2
                    JoukenNameConst.JOUKEN_ORDER_BY2,             // 昇順・降順2
                    JoukenNameConst.JOUKEN_ORDER_BY_KEY3,         // 出力順3
                    JoukenNameConst.JOUKEN_ORDER_BY3,             // 昇順・降順3
                    JoukenNameConst.JOUKEN_FILE_NAME,             // ファイル名
                    JoukenNameConst.JOUKEN_MOJI_CD                // 文字コード
            ];

            // ５．１．２．[変数：バッチ条件のキー情報] とListをキーにバッチ条件テーブルから「バッチ条件情報」を取得する。
            // バッチ条件プロパティモデルは作成しない
            List<T01050バッチ条件> batchJokens = dbContext.T01050バッチ条件s
                .Where(x => x.バッチ条件id == jid && joukenNames.Contains(x.条件名称))
                .ToList();

            // ５．１．３．「バッチ条件情報」が0件の場合
            if (batchJokens.Count == 0)
            {
                // 以下のエラーメッセージを設定し、ERRORログに出力して「１０.」へ進む。
                // （"ME01645" 引数{ 0} ：パラメータの取得)
                throw new AppException("ME01645", MessageUtil.Get("ME01645", "パラメータの取得"));
            }

            // 条件値のリストからバッチ条件情報への値設定
            foreach (T01050バッチ条件 joken in batchJokens)
            {
                switch (joken.条件名称)
                {
                    case JoukenNameConst.JOUKEN_KUMIAITO:               // 組合等　※必須
                        JokenKumiaitoCd = joken.条件値;
                        break;
                    case JoukenNameConst.JOUKEN_NENSAN:                 // 年産　※必須
                        JokenNensan = joken.条件値;
                        break;
                    case JoukenNameConst.JOUKEN_KYOSAI_MOKUTEKI:        // 共済目的　※必須
                        JokenKyosaiMokutekitoCd = joken.条件値;
                        break;
                    case JoukenNameConst.JOUKEN_SHISHO:                 // 支所　※必須
                        JokenShisho = joken.条件値;
                        break;
                    case JoukenNameConst.JOUKEN_SHICHOSON:              // 市町村
                        JokenShichoson = joken.条件値;
                        break;
                    case JoukenNameConst.JOUKEN_DAICHIKU:               // 大地区
                        JokenDaichiku = joken.条件値;
                        break;
                    case JoukenNameConst.JOUKEN_SHOCHIKU_START:         // 小地区（開始）
                        JokenShochikuStart = joken.条件値;
                        break;
                    case JoukenNameConst.JOUKEN_SHOCHIKU_END:           // 小地区（終了）
                        JokenShochikuEnd = joken.条件値;
                        break;
                    case JoukenNameConst.JOUKEN_KUMIAIINTO_CD_START:    // 組合員等コードFrom
                        JokenKumiaiintoCdStart = joken.条件値;
                        break;
                    case JoukenNameConst.JOUKEN_KUMIAIINTO_CD_END:      // 組合員等コードTo
                        JokenKumiaiintoCdEnd = joken.条件値;
                        break;
                    case JoukenNameConst.JOUKEN_ORDER_BY_KEY1:          // 出力順1
                        JokenShuturyokujun1 = joken.条件値;
                        break;
                    case JoukenNameConst.JOUKEN_ORDER_BY1:              // 昇順・降順1
                        JokenShojunKojun1 = joken.条件値;
                        break;
                    case JoukenNameConst.JOUKEN_ORDER_BY_KEY2:          // 出力順2
                        JokenShuturyokujun2 = joken.条件値;
                        break;
                    case JoukenNameConst.JOUKEN_ORDER_BY2:              // 昇順・降順2
                        JokenShojunKojun2 = joken.条件値;
                        break;
                    case JoukenNameConst.JOUKEN_ORDER_BY_KEY3:          // 出力順3
                        JokenShuturyokujun3 = joken.条件値;
                        break;
                    case JoukenNameConst.JOUKEN_ORDER_BY3:              // 昇順・降順3
                        JokenShojunKojun3 = joken.条件値;
                        break;
                    case JoukenNameConst.JOUKEN_FILE_NAME:              // ファイル名　※必須
                        JokenFileName = joken.条件値;
                        break;
                    case JoukenNameConst.JOUKEN_MOJI_CD:                // 文字コード　※必須
                        JokenMojiCd = joken.条件値;
                        break;
                }
            }
        }

        /// <summary>
        /// 必須入力チェック
        /// </summary>
        /// <returns></returns>
        /// <exception cref="AppException"></exception>
        public void IsRequired()
        {
            // ５．２．２．[変数：条件_組合等コード]がnullまたは空文字の場合
            if (string.IsNullOrEmpty(JokenKumiaitoCd))
            {
                // 以下のエラーメッセージを設定し、ERRORログに出力して「１０.」へ進む。
                // （"ME01054" 引数{0} ：条件_組合等コード)
                throw new AppException("ME01054", MessageUtil.Get("ME01054", "条件_組合等コード"));
            }

            // ５．２．３．[変数：条件_年産]がnullまたは空文字の場合
            if (string.IsNullOrEmpty(JokenNensan))
            {
                // 以下のエラーメッセージを設定し、ERRORログに出力して「１０.」へ進む。
                // （"ME01054"　引数{0} ：条件_年産)
                throw new AppException("ME01054", MessageUtil.Get("ME01054", "条件_年産"));
            }

            // ５．２．４．[変数：条件_共済目的コード]がnullまたは空文字の場合
            if (string.IsNullOrEmpty(JokenKyosaiMokutekitoCd))
            {
                // 以下のエラーメッセージを設定し、ERRORログに出力して「１０.」へ進む。
                // （"ME01054"　引数{0} ：条件_共済目的コード)
                throw new AppException("ME01054", MessageUtil.Get("ME01054", "条件_共済目的コード"));
            }

            // ５．２．５．[変数：条件_支所コード]がnullまたは空文字の場合
            if (string.IsNullOrEmpty(JokenShisho))
            {
                // 以下のエラーメッセージを設定し、ERRORログに出力して「１０.」へ進む。
                // （"ME01054"　引数{0} ：条件_支所コード)
                throw new AppException("ME01054", MessageUtil.Get("ME01054", "条件_支所コード"));
            }

            // ５．２．６．[変数：条件_ファイル名]がnullまたは空文字の場合
            if (string.IsNullOrEmpty(JokenFileName))
            {
                // 以下のエラーメッセージを設定し、ERRORログに出力して「１０.」へ進む。
                // （"ME01054"　引数{0} ：条件_ファイル名)
                throw new AppException("ME01054", MessageUtil.Get("ME01054", "条件_ファイル名"));
            }

            // ５．２．７．[変数：条件_文字コード]がnullまたは空文字の場合
            if (string.IsNullOrEmpty(JokenMojiCd))
            {
                // 以下のエラーメッセージを設定し、ERRORログに出力して「１０.」へ進む。
                // （"ME01054"　引数{0} ：条件_文字コード)
                throw new AppException("ME01054", MessageUtil.Get("ME01054", "条件_文字コード"));
            }
        }

        /// <summary>
        /// 整合性チェック
        /// </summary>
        /// <param name="dbContext">DBコンテキスト</param>
        /// <param name="todofukenCd">都道府県コード</param>
        /// <param name="kumiaitoCd">組合等コード</param>
        /// <param name="shishoCd">支所コード</param>
        /// <returns></returns>
        /// <exception cref="AppException"></exception>
        public void IsConsistency(NskAppContext dbContext, string todofukenCd, string kumiaitoCd, string shishoCd)
        {
            // ６．１．「都道府県コード存在情報」を取得する。
            int todofuken = dbContext.VTodofukens
                .Where(x => x.TodofukenCd == todofukenCd)
                .Count();
            // ６．２．データが取得できない場合（該当データがマスタデータに登録されていない場合）
            if (todofuken == 0)
            {
                // 以下のエラーメッセージを設定し、ERRORログに出力して「１０.」へ進む。
                // （"ME10005"　引数{0} ：都道府県コード)
                throw new AppException("ME10005", MessageUtil.Get("ME10005", "都道府県コード"));
            }

            // ６．３．[変数：組合等コード] が入力されている場合
            if (!string.IsNullOrEmpty(kumiaitoCd))
            {
                // 「組合等コード存在情報」を取得する。
                int kumiaito = dbContext.VKumiaitos
                     .Where(x => x.TodofukenCd == todofukenCd && x.KumiaitoCd == kumiaitoCd)
                     .Count();
                // ６．４．データが取得できない場合（該当データがマスタデータに登録されていない場合）
                if (kumiaito == 0)
                {
                    // 以下のエラーメッセージを設定し、ERRORログに出力して「１０.」へ進む。
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
                    .Count();
                // ６．６．データが取得できない場合（該当データがマスタデータに登録されていない場合）
                if (shisho == 0)
                {
                    // 以下のエラーメッセージを設定し、ERRORログに出力して「１０.」へ進む。
                    // （"ME10005"　引数{0} ：支所コード)
                    throw new AppException("ME10005", MessageUtil.Get("ME10005", "支所コード"));
                }
            }

            // ６．７．[配列：バッチ条件] から組合等コードが取得できた場合
            if (!string.IsNullOrEmpty(JokenKumiaitoCd))
            {
                // 「検索条件組合等コード存在情報」を取得する。
                int kumiaito = dbContext.VKumiaitos
                     .Where(x => x.TodofukenCd == todofukenCd && x.KumiaitoCd == JokenKumiaitoCd)
                     .Count();
                // ６．８．データが取得できない場合（該当データがマスタデータに登録されていない場合）
                if (kumiaito == 0)
                {
                    // 以下のエラーメッセージを設定し、ERRORログに出力して「１０.」へ進む。
                    // （"ME10005"　引数{0} ：条件_組合等コード)
                    throw new AppException("ME10005", MessageUtil.Get("ME10005", "条件_組合等コード"));
                }
            }

            // ６．９．[配列：バッチ条件]から共済目的コードが取得できた場合
            if (!string.IsNullOrEmpty(JokenKyosaiMokutekitoCd))
            {
                // 「共済目的コード存在情報」を取得する。
                int kyosaiMokuteki = dbContext.M00010共済目的名称s
                    .Where(x => x.共済目的コード == JokenKyosaiMokutekitoCd)
                    .Count();
                // ６．１０．データが取得できない場合（該当データがマスタデータに登録されていない場合）
                if (kyosaiMokuteki == 0)
                {
                    // 以下のエラーメッセージを設定し、ERRORログに出力して「１０.」へ進む。
                    // （"ME10005"　引数{0} ：条件_共済目的コード)
                    throw new AppException("ME10005", MessageUtil.Get("ME10005", "条件_共済目的コード"));
                }
            }

            // ６．１１．[配列：バッチ条件] から支所コードが取得できた場合
            if (!string.IsNullOrEmpty(JokenShisho))
            {
                // 「検索条件支所コード存在情報」を取得する。
                int shisho = 0;

                if (!JokenShisho.Equals("00"))
                {
                    shisho = dbContext.VShishoNms
                        .Where(x => x.TodofukenCd == todofukenCd && x.KumiaitoCd == kumiaitoCd && x.ShishoCd == JokenShisho)
                        .Count();
                }
                else
                {
                    shisho = dbContext.VShishoNms
                        .Where(x => x.TodofukenCd == todofukenCd && x.KumiaitoCd == kumiaitoCd
                            && dbContext.VShishoNms
                                .Where(y => y.TodofukenCd == todofukenCd && y.KumiaitoCd == kumiaitoCd && y.ShishoCd != "00")
                                .Select(y => y.ShishoCd)
                                .ToList()
                                .Contains(x.ShishoCd))
                        .Count();
                }
                // ６．１２．データが取得できない場合（該当データがマスタデータに登録されていない場合）
                if (shisho == 0)
                {
                    // 以下のエラーメッセージを設定し、ERRORログに出力して「１０.」へ進む。
                    // （"ME10005"　引数{0} ：条件_支所コード)
                    throw new AppException("ME10005", MessageUtil.Get("ME10005", "条件_支所コード"));
                }
            }

            // ６．１３．[配列：バッチ条件] から市町村コードが取得できた場合
            if (!string.IsNullOrEmpty(JokenShichoson))
            {
                // 「市町村コード存在情報」を取得する。
                int shichoson = dbContext.VShichosonNms
                    .Where(x => x.TodofukenCd == todofukenCd && x.KumiaitoCd == kumiaitoCd && x.ShichosonCd == JokenShichoson)
                    .Count();
                // ６．１４．データが取得できない場合（該当データがマスタデータに登録されていない場合）
                if (shichoson == 0)
                {
                    // 以下のエラーメッセージを設定し、ERRORログに出力して「１０.」へ進む。
                    // （"ME10005"　引数{0} ：条件_市町村コード)
                    throw new AppException("ME10005", MessageUtil.Get("ME10005", "条件_市町村コード"));
                }
            }

            // ６．１５．[配列：バッチ条件]から大地区コードが取得できた場合
            if (!string.IsNullOrEmpty(JokenDaichiku))
            {
                // 「大地区コード存在情報」を取得する。
                int daichiku = dbContext.VDaichikuNms
                    .Where(x => x.TodofukenCd == todofukenCd && x.KumiaitoCd == kumiaitoCd && x.DaichikuCd == JokenDaichiku)
                    .Count();
                // ６．１４．データが取得できない場合（該当データがマスタデータに登録されていない場合）
                if (daichiku == 0)
                {
                    // 以下のエラーメッセージを設定し、ERRORログに出力して「１０.」へ進む。
                    // （"ME10005　引数{0} ：条件_大地区コード)
                    throw new AppException("ME10005", MessageUtil.Get("ME10005", "条件_大地区コード"));
                }
            }
        }
    }
}