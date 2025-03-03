using CoreLibrary.Core.Base;
using CoreLibrary.Core.DropDown;
using CoreLibrary.Core.Utility;
using System.ComponentModel.DataAnnotations;

namespace NskWeb.Areas.F01.Models.D0102
{
    /// <summary>
    /// 加入者情報
    /// </summary>
    [Serializable]
    public class D0102Model : CoreViewModel
    {
        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public D0102Model()
        {
            EntryContent = new D0102EntryContent();
            TodofukenDropDownList = new TodofukenDropDownList("Kanyushajoho");
            DisplayFlg = HelpMessageUtil.IsDisplay(DateTime.Parse("2024/4/1"), "D0102", 1);
            Message = HelpMessageUtil.Get(DateTime.Parse("2024/4/1"), "D0102", 1);
            CanUpdate = true;
            CanReportOutput = true;
        }
        #endregion

        #region プロパティ
        /// <summary>
        /// 農業者ID
        /// </summary>
        public int? NogyoshaId { get; set; }

        /// <summary>
        /// 都道府県マルチドロップダウンリスト
        /// </summary>
        [Display(Name = "都道府県マルチドロップダウンリスト")]
        public TodofukenDropDownList TodofukenDropDownList { get; set; }

        /// <summary>
        /// 氏名又は法人名（フリガナ）
        /// </summary>
        [Display(Name = "氏名又は法人名（フリガナ）")]
        public string HojinFullKana { get; set; }

        /// <summary>
        /// 氏名又は法人名
        /// </summary>
        [Display(Name = "氏名又は法人名")]
        public string HojinFullNm { get; set; }

        /// <summary>
        /// 耕地形態（？ボタン）メッセージ
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 耕地形態（？ボタン）表示フラグ
        /// </summary>
        public bool DisplayFlg { get; set; }

        /// <summary>
        /// メッセージエリア1
        /// </summary>
        [Display(Name = "メッセージエリア1")]
        public string MessageArea1 { get; set; }

        /// <summary>
        /// メッセージエリア２
        /// </summary>
        [Display(Name = "メッセージエリア２")]
        public string MessageArea2 { get; set; }

        /// <summary>
        /// 登録内容
        /// </summary>
        public D0102EntryContent EntryContent { get; set; }

        #endregion

        #region コントロール
        /// <summary>
        /// 画面に対するユーザの更新可否
        /// </summary>
        public bool CanUpdate { get; set; }

        /// <summary>
        /// [画面：申請書類等作成]ボタン
        /// 活性かどうか
        /// </summary>
        public bool CanReportOutput { get; set; }
        #endregion
    }
}
