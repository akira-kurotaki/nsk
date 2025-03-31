using System.ComponentModel.DataAnnotations.Schema;

namespace NskWeb.Common.Models
{
    /// <summary>
    /// ページャー対応レコード用抽象クラス
    /// </summary>
    public abstract class BasePagerRecord
    {
        private string _guid = Guid.NewGuid().ToString();
        /// <summary>ユニークキー</summary>
        [NotMapped]
        public string GUID { get { return _guid; } set { _guid = value; } }

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
