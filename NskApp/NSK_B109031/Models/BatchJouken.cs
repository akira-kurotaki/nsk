using CoreLibrary.Core.Exceptions;
using CoreLibrary.Core.Utility;
using NskAppModelLibrary.Context;
using NskAppModelLibrary.Models;
using NskCommonLibrary.Core.Consts;

namespace NSK_B109031.Models
{
    /// <summary>
    /// バッチ条件情報
    /// </summary>
    class BatchJouken
    {
        /// <summary>
        /// 条件_年産
        /// </summary>
        public string JoukenNensan { get; set; } = string.Empty;

        /// <summary>
        /// 条件_共済目的コード
        /// </summary>
        public string JoukenKyosaiMokutekiCd { get; set; } = string.Empty;

        /// <summary>
        /// 条件_引受方式コード
        /// </summary>
        public string JoukenHikiukeHoushikiCd { get; set; } = string.Empty;

        /// <summary>
        /// バッチ条件情報の取得
        /// </summary>
        /// <param name="dbContext">DBコンテキスト</param>
        /// <param name="jid">バッチ条件のキー情報</param>
        /// <exception cref="AppException"></exception>
        public void GetBatchJoukens(NskAppContext dbContext, string jid)
        {
            // ５．１．バッチ条件情報の取得
            // ５．１．１．条件名定数から以下の項目を取得し、設定値をList<string> に格納する。
            // 条件名称の取得
            // 条件名定数から以下の項目を取得し、設定値をList<string> に格納する。
            List<string> jokenNames =
            [
                    JoukenNameConst.JOUKEN_NENSAN,                  // 年産
                    JoukenNameConst.JOUKEN_KYOSAI_MOKUTEKI,         // 共済目的コード
                    JoukenNameConst.JOUKEN_HIKIUKE_HOUSHIKI_CD      // 引受方式コード
            ];

            // ５．１．２．t_01050_バッチ条件から[引数：バッチ条件のキー情報]および下記「条件名称」に一致する「バッチ条件情報」を取得する。
            List<T01050バッチ条件> batchJokens = dbContext.T01050バッチ条件s
                .Where(x => x.バッチ条件id == jid && jokenNames.Contains(x.条件名称))
                .ToList();

            if (batchJokens.Count == 0)
            {
                // [変数：エラーメッセージ]に、以下のエラーメッセージを設定し、「９.」へ進む。
                // （"ME01645"、引数{0}：パラメータの取得)
                throw new AppException("ME01645", MessageUtil.Get("ME01645", "パラメータの取得"));
            }

            // ５．２．バッチ条件情報のチェック
            // ５．２．１．取得した「バッチ条件情報」のうち条件名称が下記と一致するデータを条件値を変数に設定する。
            foreach (T01050バッチ条件 joken in batchJokens)
            {
                switch (joken.条件名称)
                {
                    case JoukenNameConst.JOUKEN_NENSAN:                 // 年産　※必須
                        JoukenNensan = joken.条件値;
                        break;
                    case JoukenNameConst.JOUKEN_KYOSAI_MOKUTEKI:        // 共済目的コード　※必須
                        JoukenKyosaiMokutekiCd = joken.条件値;
                        break;
                    case JoukenNameConst.JOUKEN_HIKIUKE_HOUSHIKI_CD:    // 文字コード　※必須
                        JoukenHikiukeHoushikiCd = joken.条件値;
                        break;
                }
            }

            // ５．２．２．[変数：条件_年産]がnullまたは空文字の場合
            if (string.IsNullOrEmpty(JoukenNensan))
            {
                // 以下のエラーメッセージを設定し、ERRORログに出力して「９.」へ進む。
                // （"ME01054"　引数{0} ：条件_年産)
                throw new AppException("ME01054", MessageUtil.Get("ME01054", "条件_年産"));
            }

            // ５．２．３．[変数：条件_共済目的コード]がnullまたは空文字の場合
            if (string.IsNullOrEmpty(JoukenKyosaiMokutekiCd))
            {
                // 以下のエラーメッセージを設定し、ERRORログに出力して「９.」へ進む。
                // （"ME01054"　引数{0} ：条件_共済目的コード)
                throw new AppException("ME01054", MessageUtil.Get("ME01054", "条件_共済目的コード"));
            }

            // ５．２．４．[変数：条件_引受方式コード]がnullまたは空文字の場合
            if (string.IsNullOrEmpty(JoukenHikiukeHoushikiCd))
            {
                // 以下のエラーメッセージを設定し、ERRORログに出力して「９.」へ進む。
                // （"ME01054" 引数{0} ：条件_引受方式コード)
                throw new AppException("ME01054", MessageUtil.Get("ME01054", "条件_引受方式コード"));
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
                // （"ME91003"　引数{0} ：都道府県コード　{1} ：都道府県マスタ)
                throw new AppException("ME91003", MessageUtil.Get("ME91003", "都道府県コード", "都道府県マスタ"));
            }

            // ６．３．[配列：バッチ条件]から共済目的コードが取得できた場合
            if (!string.IsNullOrEmpty(JoukenKyosaiMokutekiCd))
            {
                // 「共済目的コード存在情報」を取得する。
                int kyosaiMokuteki = dbContext.M00010共済目的名称s
                    .Where(x => x.共済目的コード == JoukenKyosaiMokutekiCd)
                    .Select(x => x.共済目的コード)
                    .Count();
                // ６．４．データが取得できない場合（該当データがマスタデータに登録されていない場合）
                if (kyosaiMokuteki == 0)
                {
                    // 以下のエラーメッセージを設定し、ERRORログに出力して「９.」へ進む。
                    // （"ME91003"　引数{0} ：共済目的コード　{1} ：共済目的名称マスタ)
                    throw new AppException("ME91003", MessageUtil.Get("ME91003", "共済目的コード", "共済目的名称マスタ"));
                }
            }

            // ６．５．[変数：組合等コード]が入力されている場合、
            if (!string.IsNullOrEmpty(kumiaitoCd))
            {
                // 「組合等コード存在情報」を取得する。
                int kyosaiMokuteki = dbContext.VKumiaitos
                    .Where(x => x.TodofukenCd == todofukenCd && x.KumiaitoCd == kumiaitoCd)
                    .Select(x => x.KumiaitoCd)
                    .Count();
                // ６．６．データが取得できない場合（該当データがマスタデータに登録されていない場合）
                if (kyosaiMokuteki == 0)
                {
                    // 以下のエラーメッセージを設定し、ERRORログに出力して「９.」へ進む。
                    // （"ME91003"　引数{0} ：組合等コード　{1} ：組合等マスタ)
                    throw new AppException("ME91003", MessageUtil.Get("ME91003", "組合等コード", "組合等マスタ"));
                }
            }
        }
    }
}