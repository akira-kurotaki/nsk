using System.ComponentModel.DataAnnotations;

namespace NskWeb.Areas.F04.Models.D0401
{
    /// <summary>
    /// データ反映ボタン押下時のステータスごとの出力内容
    /// </summary>
    [Serializable]
    public class D0401DataReflection
    {
        /// <summary>
        /// 見出し
        /// </summary>
        [Display(Name = "見出し")]
        public string Caption { get; set; }

        /// <summary>
        /// 説明文
        /// </summary>
        [Display(Name = "説明文")]
        public string Description { get; set; }

        /// <summary>
        /// メッセージエリア3
        /// </summary>
        [Display(Name = "メッセージエリア3")]
        public string MessageArea3 { get; set; }

        /// <summary>
        /// バッチID
        /// </summary>
        [Display(Name = "バッチID")]
        public string BatchId { get; set; }

        /// <summary>
        /// 取込対象コード
        /// </summary>
        [Display(Name = "取込対象コード")]
        public string TorikomiCd { get; set; }

        /// <summary>
        /// 取込対象名
        /// </summary>
        [Display(Name = "取込対象名")]
        public string TorikomiNm { get; set; }

        /// <summary>
        /// 対象年度
        /// </summary>
        [Display(Name = "対象年度")]
        public short? Nendo { get; set; }

        /// <summary>
        /// 都道府県コード
        /// </summary>
        [Display(Name = "都道府県コード")]
        public string TodofukenCd { get; set; }

        /// <summary>
        /// 都道府県
        /// </summary>
        [Display(Name = "都道府県")]
        public string TodofukenNm { get; set; }

        /// <summary>
        /// 組合等コード
        /// </summary>
        [Display(Name = "組合等コード")]
        public string KumiaitoCd { get; set; }

        /// <summary>
        /// 組合等名称
        /// </summary>
        [Display(Name = "組合等名称")]
        public string KumiaitoNm { get; set; }

        /// <summary>
        /// 支所コード
        /// </summary>
        [Display(Name = "支所コード")]
        public string ShishoCd { get; set; }

        /// <summary>
        /// 支所名称
        /// </summary>
        [Display(Name = "支所名称")]
        public string ShishoNm { get; set; }

        /// <summary>
        /// 取込ファイルパス
        /// </summary>
        [Display(Name = "取込ファイルパス")]
        public string TorikomiFilePath { get; set; }

        /// <summary>
        /// 取込ファイル名
        /// </summary>
        [Display(Name = "取込ファイル名")]
        public string TorikomiFileNm { get; set; }

        /// <summary>
        /// ハッシュ値
        /// </summary>
        [Display(Name = "ハッシュ値")]
        public string Hash { get; set; }

        /// <summary>
        /// ステータス
        /// </summary>
        [Display(Name = "ステータス")]
        public string TorikomiSts { get; set; }

        /// <summary>
        /// 取込完了日時
        /// </summary>
        [Display(Name = "取込完了日時")]
        public DateTime? TorikomiDate { get; set; }

        /// <summary>
        /// ロック開始日時
        /// </summary>
        [Display(Name = "ロック開始日時")]
        public DateTime? LockDate { get; set; }

        /// <summary>
        /// データ反映日時
        /// </summary>
        [Display(Name = "データ反映日時")]
        public DateTime? HaneiDate { get; set; }

    }
}
