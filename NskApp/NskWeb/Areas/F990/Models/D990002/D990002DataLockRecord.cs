using System.ComponentModel.DataAnnotations;

namespace NskWeb.Areas.F990.Models.D990002
{
    /// <summary>
    /// データロック詳細
    /// </summary>
    [Serializable]
    public class D990002DataLockRecord
    {
        /// <summary>
        /// システム区分
        /// </summary>
        [Display(Name = "システム区分")]
        public string SystemKbn { get; set; }

        /// <summary>
        /// システム区分名称
        /// </summary>
        [Display(Name = "システム区分名称")]
        public string SystemKbnNm { get; set; }

        /// <summary>
        /// 都道府県コード
        /// </summary>
        [Display(Name = "都道府県コード")]
        public string TodofukenCd { get; set; }

        /// <summary>
        /// 都道府県名称
        /// </summary>
        [Display(Name = "都道府県名称")]
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
        /// ロック開始日時
        /// </summary>
        [Display(Name = "ロック開始日時")]
        public DateTime? LockDate { get; set; }

        /// <summary>
        /// ロック終了日時
        /// </summary>
        [Display(Name = "ロック終了日時")]
        public DateTime? LockEndDate { get; set; }

        /// <summary>
        /// ロック実行ユーザ
        /// </summary>
        [Display(Name = "ロック実行ユーザ")]
        public string LockUserId { get; set; }

        /// <summary>
        /// ロック実行処理
        /// </summary>
        [Display(Name = "ロック実行処理")]
        public string LockShori { get; set; }
    }
}
