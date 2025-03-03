using CoreLibrary.Core.Base;
using CoreLibrary.Core.DropDown;
using CoreLibrary.Core.Dto;
using CoreLibrary.Core.Validator;
using System.ComponentModel.DataAnnotations;

namespace NskWeb.Areas.F990.Models.D990002
{
    /// <summary>
    /// システムロック
    /// </summary>
    [Serializable]
    public class D990002Model : CoreViewModel
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public D990002Model()
        {
            // 都道府県マルチドロップダウンリスト
            TodofukenDropDownList = new TodofukenDropDownList("");
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="syokuin"></param>
        /// <param name="shishoList"></param>
        public D990002Model(Syokuin syokuin, List<Shisho> shishoList)
        {
            // 都道府県マルチドロップダウンリスト
            TodofukenDropDownList = new TodofukenDropDownList("", syokuin, shishoList);

            // データロック
            DataLocks = new D990002DataLocks();

            // システムロック
            SysLocks = new D990002SysLocks();
        }

        /// <summary>
        /// メッセージエリア1
        /// </summary>
        [Display(Name = "メッセージエリア1")]
        public string MessageArea1 { get; set; }

        /// <summary>
        /// メッセージエリア2
        /// </summary>
        [Display(Name = "メッセージエリア2")]
        public string MessageArea2 { get; set; }

        /// <summary>
        /// メッセージエリア3
        /// </summary>
        [Display(Name = "メッセージエリア3")]
        public string MessageArea3 { get; set; }

        /// <summary>
        /// システム区分
        /// </summary>
        [Display(Name = "システム区分")]
        public string SystemKbn { get; set; }

        /// <summary>
        /// 都道府県マルチドロップダウンリスト
        /// </summary>
        [Display(Name = "都道府県マルチドロップダウンリスト")]
        public TodofukenDropDownList TodofukenDropDownList { get; set; }

        /// <summary>
        /// ロック開始日時
        /// </summary>
        [Display(Name = "ロック開始日時")]
        [DateTime("yyyy/MM/dd HH:mm")]
        public DateTime? LockStart { get; set; }

        /// <summary>
        /// ロック終了日時
        /// </summary>
        [Display(Name = "ロック終了日時")]
        [DateTime("yyyy/MM/dd HH:mm")]
        public DateTime? LockEnd { get; set; }

        /// <summary>
        /// ロック実行処理
        /// </summary>
        [Display(Name = "ロック実行処理")]
        public string LockShori { get; set; }

        /// <summary>
        /// ロック実行ユーザID
        /// </summary>
        [Display(Name = "ロック実行ユーザID")]
        public string UserId { get; set; }

        /// <summary>
        /// データロック
        /// </summary>
        public D990002DataLocks DataLocks { get; set; }

        /// <summary>
        /// システムロック
        /// </summary>
        public D990002SysLocks SysLocks { get; set; }
    }
}
