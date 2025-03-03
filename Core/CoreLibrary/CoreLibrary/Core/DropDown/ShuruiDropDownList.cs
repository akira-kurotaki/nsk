using System.ComponentModel.DataAnnotations;

namespace CoreLibrary.Core.DropDown
{
    /// <summary>
    /// 種類・品目・用途ドロップダウンリスト用モデル
    /// </summary>
    /// <remarks>
    /// 作成日：2018/05/16
    /// 作成者：Rou I
    /// </remarks>
    [Serializable]
    public class ShuruiDropDownList : IShuruiDropDownList
    {
        /// <summary>
        /// 都道府県
        /// </summary>
        [Display(Name = "都道府県")]
        public string TodofukenCd { get; set; }

        /// <summary>
        /// 組合等
        /// </summary>
        [Display(Name = "組合等")]
        public string KumiaitoCd { get; set; }

        /// <summary>
        /// 支所
        /// </summary>
        [Display(Name = "支所")]
        public string ShishoCd { get; set; }

        /// <summary>
        /// 種類
        /// </summary>
        [Display(Name = "種類")]
        public string ShuruiCd { get; set; }

        /// <summary>
        /// 品目
        /// </summary>
        [Display(Name = "品目")]
        public string HimmokuCd { get; set; }

        /// <summary>
        /// 用途
        /// </summary>
        [Display(Name = "用途")]
        public string YotoCd { get; set; }

        /// <summary>
        /// プログラムモード
        /// </summary>
        [Display(Name = "プログラムモード")]
        //public ShuruiDropDownListUtil.ProgramMode? ProgramMode { get; set; }
        public string ProgramMode { get; set; }
    }
}