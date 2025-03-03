using System.ComponentModel.DataAnnotations.Schema;

namespace NskWeb.Areas.F105.Models.D105030
{
    /// <summary>
    /// ページャー対応レコード用抽象クラス
    /// </summary>
    public abstract class D105030PagerRecord
    {
        /// <summary>追加行か否かの判定用フラグ</summary>
        [NotMapped]
        public bool IsNewRec { get; set; } = false;

        /// <summary>削除行か否かの判定用フラグ</summary>
        [NotMapped]
        public bool IsDelRec { get; set; } = false;

        /// <summary>選択</summary>
        [NotMapped]
        public bool CheckSelect { get; set; } = false;
    }
}
