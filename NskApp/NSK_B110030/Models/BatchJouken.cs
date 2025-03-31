using NskAppModelLibrary.Context;
using System.Reflection;
using NskCommonLibrary.Core.Consts;

namespace NSK_B110030.Models
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
        /// 条件_文字コード
        /// </summary>
        public string JoukenMojiCd { get; set; } = string.Empty;

        public void GetBatchJoukens(NskAppContext dbContext, string jid, BatchJouken batchJouken)
        {
            // 条件名称の取得
            // 条件名定数から以下の項目を取得し、設定値をList<string> に格納する。
            List<string> jokenNames =
            [
                    JoukenNameConst.JOUKEN_KUMIAITO,            // 組合等コード
                    JoukenNameConst.JOUKEN_NENSAN,              // 年産
                    JoukenNameConst.JOUKEN_KYOSAI_MOKUTEKI,     // 共済目的コード
                    JoukenNameConst.JOUKEN_MOJI_CD              // 文字コード
            ];

            // ３．１．３．１．t_01050_バッチ条件から[引数：バッチ条件のキー情報]および下記「条件名称」に一致する「バッチ条件情報」を取得する。
            var batchJokens = dbContext.T01050バッチ条件s
                .Where(x => x.バッチ条件id == jid && jokenNames.Contains(x.条件名称))
                .ToList();

            // 条件値のリストからバッチ条件情報への値設定
            foreach (var joken in batchJokens)
            {
                switch (joken.条件名称)
                {
                    case JoukenNameConst.JOUKEN_KUMIAITO:                   // 組合等コード
                        batchJouken.JoukenKumiaitoCd = joken.条件値;
                        break;
                    case JoukenNameConst.JOUKEN_NENSAN:                     // 年産
                        batchJouken.JoukenNensan = joken.条件値;
                        break;
                    case JoukenNameConst.JOUKEN_KYOSAI_MOKUTEKI:            // 共済目的コード
                        batchJouken.JoukenKyosaiMokutekiCd = joken.条件値;
                        break;
                    case JoukenNameConst.JOUKEN_MOJI_CD:                    // 文字コード
                        batchJouken.JoukenMojiCd = joken.条件値;
                        break;
                }
            }
        }

        public bool IsRequired()
        {
            // ３．１．３．２．データが取得できない場合（[引数：バッチ条件のキー情報]が登録されていない場合）、[変数：エラーメッセージ]を設定し、「７.」へ進む。
            PropertyInfo[] properties = GetType().GetProperties();
            foreach (PropertyInfo property in properties)
            {
                object? value = property.GetValue(this);
                if (value == null || value is string strValue && string.IsNullOrEmpty(strValue))
                {
                    return false;
                }
            }
            return true;
        }

        public bool IsConsistency(NskAppContext dbContext)
        {
            // ４．１．整合性チェックでエラーの場合、[変数：エラーメッセージ]を設定し、「７.」へ進む。
            // ※検索結果0件の場合にエラーとする。
            // ４．２．整合性チェックで取得した値を変数に設定する。
            // 共済目的コードマスタチェック
            int kyosaiMokutekiCdCounter = dbContext.M00010共済目的名称s
                .Where(x => x.共済目的コード == JoukenKyosaiMokutekiCd)
                .Select(x => x.共済目的コード)
                .Count();
            if (kyosaiMokutekiCdCounter == 0)
            {
                return false;
            }
            return true;
        }
    }
}