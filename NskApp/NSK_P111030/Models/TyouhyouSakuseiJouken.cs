using CoreLibrary.Core.Exceptions;
using CoreLibrary.Core.Utility;
using NskAppModelLibrary.Context;
using NskAppModelLibrary.Models;
using NskCommonLibrary.Core.Consts;

namespace NSK_P111030.Models
{
    /// <summary>
    /// 帳票作成条件
    /// </summary>
    public class TyouhyouSakuseiJouken
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
        /// 条件_負担金交付区分
        /// </summary>
        public string JoukenFutankinKofuKbn { get; set; } = string.Empty;

        /// <summary>
        /// 条件_交付回
        /// </summary>
        public string JoukenKoufuKai { get; set; } = string.Empty;

        /// <summary>
        /// 条件_発行年月日
        /// </summary>
        public string JoukenHakkoDate { get; set; } = string.Empty;

        /// <summary>
        /// 条件_帳票名
        /// </summary>
        public string JoukenReportName { get; set; } = string.Empty;

        /// <summary>
        /// 帳票作成条件の取得
        /// </summary>
        /// <param name="dbContext">DBコンテキスト</param>
        /// <param name="jid">バッチ条件のキー情報</param>
        /// <exception cref="AppException"></exception>
        public void GetTyouhyouSakuseiJouken(NskAppContext dbContext, string jid)
        {
            // ２．１．１．[変数：バッチ条件のキー情報]をキーにバッチ条件テーブルから「帳票作成条件」を取得する。
            // 「帳票作成条件」を取得する。
            List<T01050バッチ条件> tyouhyouSakuseiJouken = dbContext.T01050バッチ条件s
                .Where(x => x.バッチ条件id == jid)
                .ToList();
            // ２．１．２．取得した件数が0件の場合
            if (tyouhyouSakuseiJouken.Count == 0)
            {
                // [変数：エラーメッセージ]を設定し、バッチ処理（NSK_111030B）の「８．バッチ実行状況更新」へ進む。
                // （"ME90010")
                throw new AppException("ME90010", MessageUtil.Get("ME90010"));
            }

            // ２．２．帳票作成条件のチェック
            // ２．２．１．取得した「帳票作成条件」から定数に設定された条件名称でデータを取り出し、条件値を変数に設定する。
            // 条件名称の取得
            // 条件名定数から以下の項目を取得し、設定値をList<string> に格納する。
            List<string> joukenNames =
            [
                    JoukenNameConst.JOUKEN_KUMIAITO,            // 組合等コード
                    JoukenNameConst.JOUKEN_NENSAN,              // 年産
                    JoukenNameConst.JOUKEN_KYOSAI_MOKUTEKI,     // 共済目的コード
                    JoukenNameConst.JOUKEN_FUTANKIN_KOFU_KBN,   // 負担金交付区分
                    JoukenNameConst.JOUKEN_KOUFU_KAI,           // 交付回
                    JoukenNameConst.JOUKEN_HAKKO_DATE,          // 発行年月日
                    JoukenNameConst.JOUKEN_REPORT_NAME          // 帳票名
            ];

            // 条件値のリストから帳票作成条件への値設定
            foreach (var joken in tyouhyouSakuseiJouken)
            {
                switch (joken.条件名称)
                {
                    case JoukenNameConst.JOUKEN_KUMIAITO:               // 組合等コード　　※必須
                        JoukenKumiaitoCd = joken.条件値;
                        break;
                    case JoukenNameConst.JOUKEN_NENSAN:                 // 年産　　　　　　※必須
                        JoukenNensan = joken.条件値;
                        break;
                    case JoukenNameConst.JOUKEN_KYOSAI_MOKUTEKI:        // 共済目的コード  ※必須
                        JoukenKyosaiMokutekiCd = joken.条件値;
                        break;
                    case JoukenNameConst.JOUKEN_FUTANKIN_KOFU_KBN:      // 負担金交付区分  ※必須
                        JoukenFutankinKofuKbn = joken.条件値;
                        break;
                    case JoukenNameConst.JOUKEN_KOUFU_KAI:              // 交付回          ※必須
                        JoukenKoufuKai = joken.条件値;
                        break;
                    case JoukenNameConst.JOUKEN_HAKKO_DATE:             // 発行年月日
                        JoukenHakkoDate = joken.条件値;
                        break;
                    case JoukenNameConst.JOUKEN_REPORT_NAME:            // 帳票名          ※必須
                        JoukenReportName = joken.条件値;
                        break;
                }
            }
        }

        /// <summary>
        /// 必須入力チェック
        /// </summary>
        public void IsRequired()
        {
            // ２．２．２．[変数：条件_組合等コード]がnullまたは空文字の場合
            if (string.IsNullOrEmpty(JoukenKumiaitoCd))
            {
                // 以下のエラーメッセージを設定し、ERRORログに出力して「６.」へ進む。
                // （"ME01054" 引数{0} ：条件_組合等コード)
                throw new AppException("ME01054", MessageUtil.Get("ME01054", "条件_組合等コード"));
            }

            // ２．２．３．[変数：条件_年産]がnullまたは空文字の場合
            if (string.IsNullOrEmpty(JoukenNensan))
            {
                // 以下のエラーメッセージを設定し、ERRORログに出力して「６.」へ進む。
                // （"ME01054"　引数{0} ：条件_年産)
                throw new AppException("ME01054", MessageUtil.Get("ME01054", "条件_年産"));
            }

            // ２．２．４．[変数：条件_共済目的コード]がnullまたは空文字の場合
            if (string.IsNullOrEmpty(JoukenKyosaiMokutekiCd))
            {
                // 以下のエラーメッセージを設定し、ERRORログに出力して「６.」へ進む。
                // （"ME01054"　引数{0} ：条件_共済目的コード)
                throw new AppException("ME01054", MessageUtil.Get("ME01054", "条件_共済目的コード"));
            }

            // ２．２．５．[変数：条件_負担金交付区分]がnullまたは空文字の場合
            if (string.IsNullOrEmpty(JoukenFutankinKofuKbn))
            {
                // 以下のエラーメッセージを設定し、ERRORログに出力して「６.」へ進む。
                // （"ME01054"　引数{0} ：条件_負担金交付区分)
                throw new AppException("ME01054", MessageUtil.Get("ME01054", "条件_負担金交付区分"));
            }

            // ２．２．６．[変数：条件_交付回]がnullまたは空文字の場合
            if (string.IsNullOrEmpty(JoukenKoufuKai))
            {
                // 以下のエラーメッセージを設定し、ERRORログに出力して「６.」へ進む。
                // （"ME01054"　引数{0} ：条件_交付回)
                throw new AppException("ME01054", MessageUtil.Get("ME01054", "条件_交付回"));
            }

            // ２．２．７．[変数：条件_帳票名]がnullまたは空文字の場合
            if (string.IsNullOrEmpty(JoukenReportName))
            {
                // 以下のエラーメッセージを設定し、ERRORログに出力して「６.」へ進む。
                // （"ME01054"　引数{0} ：条件_帳票名)
                throw new AppException("ME01054", MessageUtil.Get("ME01054", "条件_帳票名"));
            }
        }

        /// <summary>
        /// コードの整合性チェック
        /// </summary>
        /// <param name="dbContext">DBコンテキスト</param>
        /// <param name="todofukenCd">都道府県コード</param>
        /// <exception cref="AppException"></exception>
        public void IsConsistency(NskAppContext dbContext, string todofukenCd)
        {
            // ３．１．[配列：バッチ条件]から組合等コードが取得できた場合、「検索条件組合等コード存在情報」を取得する。
            // 検索条件組合等コード存在情報を取得する
            int kumiaitoCdCounter = dbContext.VKumiaitos
                .Where(x => x.TodofukenCd == todofukenCd && x.KumiaitoCd == JoukenKumiaitoCd)
                .Select(x => x.KumiaitoCd)
                .Count();
            // ３．２．データが取得できない場合（該当データがマスタデータに登録されていない場合）、
            if (kumiaitoCdCounter == 0)
            {
                // 以下のエラーメッセージを設定し、ERRORログに出力して「６.」へ進む。
                // （"ME10005"　引数{0} ：条件_組合等コード)
                throw new AppException("ME10005", MessageUtil.Get("ME10005", "条件_組合等コード"));
            }

            // ３．３．[配列：バッチ条件]から共済目的コードが取得できた場合、「共済目的コード存在情報」を取得する。
            // 共済目的コード存在情報を取得する
            int kyosaiMokutekiCdCounter = dbContext.M00010共済目的名称s
                .Where(x => x.共済目的コード == JoukenKyosaiMokutekiCd)
                .Select(x => x.共済目的コード)
                .Count();
            // ３．４．データが取得できない場合（該当データがマスタデータに登録されていない場合）、
            if (kyosaiMokutekiCdCounter == 0)
            {
                // 以下のエラーメッセージを設定し、ERRORログに出力して「６.」へ進む。
                // （"ME10005"、引数{0}：条件_共済目的コード)
                throw new AppException("ME10005", MessageUtil.Get("ME10005", "条件_共済目的コード"));
            }
        }
    }
}
