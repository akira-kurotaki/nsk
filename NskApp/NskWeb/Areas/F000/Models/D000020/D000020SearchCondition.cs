using CoreLibrary.Core.Base;
using CoreLibrary.Core.Consts;
using CoreLibrary.Core.DropDown;
using CoreLibrary.Core.Dto;
using CoreLibrary.Core.Utility;
using CoreLibrary.Core.Validator;
using ModelLibrary.Models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using static NskWeb.Areas.F000.Models.D000020.D000020SearchCondition;

namespace NskWeb.Areas.F000.Models.D000020
{
    /// <summary>
    /// 組合員等コード検索(子画面)画面項目モデル（検索条件部分）
    /// </summary>
    [Serializable]
    public class D000020SearchCondition
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public D000020SearchCondition()
        {
            // 都道府県マルチドロップダウンリスト
            TodofukenDropDownList = new TodofukenDropDownList("SearchCondition");
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="syokuin">職員マスタ</param>
        public D000020SearchCondition(Syokuin syokuin, List<Shisho> shishoList)
        {
            // 都道府県マルチドロップダウンリスト
            TodofukenDropDownList = new TodofukenDropDownList("SearchCondition", syokuin, shishoList);
        }

        /// <summary>
        /// 年産
        /// </summary>
        [Display(Name = "年産：")]
        public string Nensan { get; set; }

        /// <summary>
        /// 共済目的コード
        /// </summary>
        [Display(Name = "共済目的コード")]
        public string KyosaiMokutekiCd { get; set; }

        /// <summary>
        /// 共済目的名称
        /// </summary>
        [Display(Name = "共済目的：")]
        public string KyosaiMokutekiNm { get; set; }

        /// <summary>
        /// 都道府県マルチドロップダウンリスト
        /// </summary>
        [Display(Name = "都道府県マルチドロップダウンリスト")]
        public TodofukenDropDownList TodofukenDropDownList { get; set; }

        /// <summary>
        /// 組合員等氏名
        /// </summary>
        [Display(Name = "組合員等氏名")]
        [WithinStringLength(30)]
        public string KumiaiintoNm { get; set; }

        /// <summary>
        /// 組合員等コード
        /// </summary>
        [Display(Name = "組合員等コード")]
        public string KumiaiintoCd { get; set; }

        /// <summary>
        /// 組合員等コード(開始)
        /// </summary>
        [Display(Name = "組合員等コード(開始)")]
        [Numeric]
        [WithinDigitLength(13)]
        public string KumiaiintoCdFrom { get; set; }

        /// <summary>
        /// 組合員等コード(終了)
        /// </summary>
        [Display(Name = "組合員等コード(終了)")]
        [Numeric]
        [WithinDigitLength(13)]
        public string KumiaiintoCdTo { get; set; }

        /// <summary>
        /// 氏名
        /// </summary>
        [Display(Name = "氏名")]
        public string Name { get; set; }

        /// <summary>
        /// 住所
        /// </summary>
        [Display(Name = "住所")]
        public string Post { get; set; }

        /// <summary>
        /// 電話番号
        /// </summary>
        [Display(Name = "電話番号")]
        public string Tel { get; set; }

        /// <summary>
        /// 検索子画面からの戻り値をセットする項目のキー
        /// </summary>
        public string RetKey { get; set; }

    }
}
