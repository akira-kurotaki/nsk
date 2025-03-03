using NskAppModelLibrary.Models;
using CoreLibrary.Core.Base;
using CoreLibrary.Core.DropDown;
using CoreLibrary.Core.Dto;
using CoreLibrary.Core.Utility;
using System.ComponentModel.DataAnnotations;

namespace NskWeb.Areas.F04.Models.D0401
{
    /// <summary>
    /// ファイル取込画面モデル
    /// </summary>
    [Serializable]
    public class D0401Model : CoreViewModel
    {
        public D0401Model(Syokuin syokuin, List<Shisho> shishoList)
        {
            // 対象年度
            Nendo = NendoUtil.GetStandardNendo();

            // 都道府県マルチドロップダウンリスト
            this.TodofukenDropDownList = new TodofukenDropDownList(syokuin, shishoList);

            this.SearchResult = new D0401SearchResult();
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
        /// エラーメッセージエリア2
        /// </summary>
        [Display(Name = "エラーメッセージエリア2")]
        public string ErrorMessageArea2 { get; set; }

        /// <summary>
        /// 検索結果表示フラグ
        /// </summary>
        [Display(Name = "検索結果表示フラグ")]
        public bool IsSearchResultDisplayFlag { get; set; }

        /// <summary>
        /// データ反映ボタン押下時のステータスごとの出力内容表示フラグ
        /// </summary>
        [Display(Name = "データ反映ボタン押下時のステータスごとの出力内容表示フラグ")]
        public bool IsDataReflectionDisplayFlag { get; set; }

        /// <summary>
        /// 取込対象コード
        /// </summary>
        [Display(Name = "取込対象")]
        public string TorikomiCd { get; set; }

        /// <summary>
        /// 取込対象
        /// </summary>
        [Display(Name = "取込対象")]
        [Required]
        public string TorikomiNm { get; set; }

        /// <summary>
        /// 取込対象リスト
        /// </summary>
        public List<MTorikomi> MTorikomiList { get; set; }

        /// <summary>
        /// 対象年度
        /// </summary>
        [Display(Name = "対象年度")]
        [Required]
        public string Nendo { get; set; }

        /// <summary>
        /// 都道府県マルチドロップダウンリスト
        /// </summary>
        [Display(Name = "都道府県マルチドロップダウンリスト")]
        public TodofukenDropDownList TodofukenDropDownList { get; set; }

        /// <summary>
        /// アップロードファイル
        /// </summary>
        [Display(Name = "アップロードファイル")]
        [Required]
        public string FilePath { get; set; }

        /// <summary>
        /// ファイルサイズ
        /// </summary>
        [Display(Name = "ファイルサイズ")]
        public int FileSize { get; set; }

        /// <summary>
        /// ファイルサイズ(単位を含む)
        /// </summary>
        [Display(Name = "ファイルサイズ")]
        public String FileSizeStr { get; set; }

        /// <summary>
        /// 検索結果
        /// </summary>
        [Display(Name = "検索結果")] 
        public D0401SearchResult SearchResult { get; set; }

        /// <summary>
        /// データ反映ボタン押下時のステータスごとの出力内容
        /// </summary>
        [Display(Name = "データ反映ボタン押下時のステータスごとの出力内容")]
        public D0401DataReflection DataReflection { get; set; }
    }
}
