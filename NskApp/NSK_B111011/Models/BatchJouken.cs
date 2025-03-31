using CoreLibrary.Core.Exceptions;
using CoreLibrary.Core.Utility;
using NskAppModelLibrary.Context;
using NskAppModelLibrary.Models;
using NskCommonLibrary.Core.Consts;

namespace NSK_B111011.Models
{
    /// <summary>
    /// バッチ条件情報
    /// </summary>
    class BatchJouken
    {
        /// <summary>
        /// 条件_負担金交付区分
        /// </summary>
        public string JoukenFutankinKofuKbn { get; set; } = string.Empty;

        /// <summary>
        /// 条件_交付回
        /// </summary>
        public string JoukenKoufuKai { get; set; } = string.Empty;

        /// <summary>
        /// 条件_年産
        /// </summary>
        public string JoukenNensan { get; set; } = string.Empty;

        /// <summary>
        /// バッチ条件を取得する
        /// </summary>
        /// <param name="dbContext">DBコンテキスト</param>
        /// <param name="jid">バッチ条件ID</param>
        public void GetBatchJokens(NskAppContext dbContext, string jid)
        {
            // ６．１．条件リストを取得
            // ６．１．１．条件名定数から以下の項目を取得し、設定値をList<string>に格納する。
            // ※例：[年産,共済目的コード,抽出区分...]
            // 条件名称リストの取得
            List<string> joukenNames =
            [
                JoukenNameConst.JOUKEN_FUTANKIN_KOFU_KBN,  // 負担金交付区分
                JoukenNameConst.JOUKEN_KOUFU_KAI,          // 交付回
                JoukenNameConst.JOUKEN_NENSAN,             // 年産
            ];

            // ６．１．２．「バッチ条件リスト」を取得する。
            // t_01050_バッチ条件から[引数：バッチ条件のキー情報]および「条件名称」に一致する「バッチ条件リスト」を取得する。
            List<T01050バッチ条件> batchJoukens = dbContext.T01050バッチ条件s
                .Where(x => x.バッチ条件id == jid && joukenNames.Contains(x.条件名称))
                .ToList();

            // ６．２．「バッチ条件リスト」の取得した件数が0件の場合
            if (batchJoukens.Count == 0)
            {
                // [変数：エラーメッセージ]を設定し、「１４．」へ進む。
                // （"ME01645" 引数{ 0} ：パラメータの取得)
                throw new AppException("ME01645", MessageUtil.Get("ME01645", "パラメータの取得"));
            }

            // ６．３．「バッチ条件リスト」の取得した件数が0件以外の場合
            // ６．３．１．取得したデータのうち条件名称が下記と一致するデータのを条件値を変数に設定する。
            // 条件値のリストからバッチ条件情報への値設定
            foreach (T01050バッチ条件 jouken in batchJoukens)
            {
                foreach (string a in joukenNames)
                {
                    switch (jouken.条件名称)
                    {
                        case JoukenNameConst.JOUKEN_FUTANKIN_KOFU_KBN: // 負担金交付区分
                            JoukenFutankinKofuKbn = jouken.条件値;
                            break;
                        case JoukenNameConst.JOUKEN_KOUFU_KAI:         // 交付回
                            JoukenKoufuKai = jouken.条件値;
                            break;
                        case JoukenNameConst.JOUKEN_NENSAN:            // 年産
                            JoukenNensan = jouken.条件値;
                            break;
                    }
                }
            }
        }

        public void IsConsistency(NskAppContext dbContext, string todofukenCd, string kumiaitoCd, string shishoCd)
        {
            // ５．１．「都道府県コード存在情報」を取得する。
            // ※検索結果0件の場合にエラーとする。
            int todofukenCdCounter = dbContext.VTodofukens
                .Where(x => x.TodofukenCd == todofukenCd)
                .Select(x => x.TodofukenCd)
                .Count();
            // ５．２．データが取得できない場合（該当データがマスタデータに登録されていない場合）、
            if (todofukenCdCounter == 0)
            {
                // 以下のエラーメッセージを設定し、ERRORログに出力して「１４．」へ進む。
                // （"ME10005"　引数{0} ：都道府県コード)
                throw new AppException("ME10005", MessageUtil.Get("ME10005", "都道府県コード"));
            }

            // ５．３．[変数：組合等コード]が入力されている場合、「組合等コード存在情報」を取得する。
            int kumiaitoCdCounter = dbContext.VKumiaitos
                .Where(x => x.TodofukenCd == todofukenCd && x.KumiaitoCd == kumiaitoCd)
                .Select(x => x.KumiaitoCd)
                .Count();
            // ５．４．データが取得できない場合（該当データがマスタデータに登録されていない場合）、
            if (kumiaitoCdCounter == 0)
            {
                // 以下のエラーメッセージを設定し、ERRORログに出力して「１４．」へ進む。
                // （"ME10005"　引数{0} ：組合等コード)
                throw new AppException("ME10005", MessageUtil.Get("ME10005", "組合等コード"));
            }

            // ５．５．[変数：支所コード]が入力されている場合、「支所コード存在情報」を取得する。
            int shishoCdCounter = dbContext.VShishoNms
                .Where(x => x.TodofukenCd == todofukenCd && x.KumiaitoCd == kumiaitoCd && x.ShishoCd == shishoCd)
                .Select(x => x.ShishoCd)
                .Count();
            // ５．６．データが取得できない場合（該当データがマスタデータに登録されていない場合）、
            if (shishoCdCounter == 0)
            {
                // 以下のエラーメッセージを設定し、ERRORログに出力して「１４．」へ進む。
                // （"ME10005"　引数{0} ：支所コード)
                throw new AppException("ME10005", MessageUtil.Get("ME10005", "支所コード"));
            }
        }
    }
}