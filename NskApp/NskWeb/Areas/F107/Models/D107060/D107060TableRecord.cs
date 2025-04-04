using CoreLibrary.Core.Validator;
using NpgsqlTypes;
using System.ComponentModel.DataAnnotations;
using NskWeb.Common.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations.Schema;
using CoreLibrary.Core.DropDown;

namespace NskWeb.Areas.F107.Models.D107060
{
    /// <summary>
    /// 消込処理(手動)検索結果明細行
    /// </summary>
    public class D107060TableRecord : BasePagerRecord
    {

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public D107060TableRecord()
        {
            // 徴収区分ドロップダウンリスト用
            ChoshuKbnList = new List<SelectListItem>();
            ChoshuKbnModelList = new List<D107060ChoshuKbnList>();

            // 徴収理由ドロップダウンリスト用
            ChoshuRiyuList = new List<SelectListItem>();
            ChoshuRiyuModelList = new List<D107060ChoshuRiyuList>();
        }


        /// <summary>組合等コード</summary>
        [Display(Name = "組合員等コード")]
        [WithinStringLength(13)]
        public string KumiaiintoCd { get; set; } = string.Empty;

        /// <summary>氏名</summary>
        [Display(Name = "氏名")]
        public string NogyosyaNm { get; set; } = string.Empty;

        /// <summary>調定額</summary>
        [Display(Name = "調定額")]
        [Numeric]
        [DisplayFormat(DataFormatString = "{0:#,##0}", ApplyFormatInEditMode = true)]
        public int? ChoteiGaku { get; set; }

        /// <summary>共済掛金</summary>
        [Display(Name = "共済掛金")]
        [Numeric]
        [DisplayFormat(DataFormatString = "{0:#,##0}", ApplyFormatInEditMode = true)]
        public int? KyosaiKakekin { get; set; }

        /// <summary>賦課金</summary>
        [Display(Name = "賦課金")]
        [Numeric]
        [DisplayFormat(DataFormatString = "{0:#,##0}", ApplyFormatInEditMode = true)]
        public int? Hukakin { get; set; }

        /// <summary>徴収済額</summary>
        [Display(Name = "徴収済額")]
        [Numeric]
        [DisplayFormat(DataFormatString = "{0:#,##0}", ApplyFormatInEditMode = true)]
        public int? ChoshuzumiGaku { get; set; }

        /// <summary>前回徴収年月日</summary>
        [Display(Name = "前回徴収年月日")]
        [DateTime]
        public DateTime? ZenkaiChoshuYmd { get; set; }

        /// <summary>請求額</summary>
        [Display(Name = "請求額")]
        [Numeric]
        [DisplayFormat(DataFormatString = "{0:#,##0}", ApplyFormatInEditMode = true)]
        public int? SeikyuGaku { get; set; }

        /// <summary>今回徴収額</summary>
        [Display(Name = "今回徴収額")]
        [Numeric]
        [DisplayFormat(DataFormatString = "{0:#,##0}", ApplyFormatInEditMode = true)]
        public int? ChoshuGakuNow { get; set; }

        /// <summary>徴収区分</summary>
        [Display(Name = "徴収区分")]
        public decimal? ChoshuKbn { get; set; }
        /// <summary>
        /// 徴収区分ドロップダウンリスト
        /// </summary>
        [NotMapped]
        public List<SelectListItem> ChoshuKbnList { get; set; }
        /// <summary>
        /// 徴収区分モデルリスト
        /// </summary>
        [NotMapped]
        public List<D107060ChoshuKbnList> ChoshuKbnModelList { get; set; }

        /// <summary>徴収年月日</summary>
        [Display(Name = "徴収年月日")]
        [DateTime]
        public DateTime? ChoshuYmd { get; set; }

        /// <summary>徴収理由</summary>
        [Display(Name = "徴収理由")]
        public decimal? Riyu { get; set; }
        /// <summary>
        /// 徴収理由ドロップダウンリスト
        /// </summary>
        [NotMapped]
        public List<SelectListItem> ChoshuRiyuList { get; set; }
        /// <summary>
        /// 徴収理由モデルリスト
        /// </summary>
        [NotMapped]
        public List<D107060ChoshuRiyuList> ChoshuRiyuModelList { get; set; }

        /// <summary>徴収者</summary>
        [Display(Name = "徴収者")]
        [WithinStringLength(40)]
        public string Choshusya { get; set; } = string.Empty;

        /// <summary>自動振替フラグ</summary>
        [Display(Name = "自動振替フラグ")]
        public string JidoFurikaeFlg { get; set; }

        /// <summary>ChoshuJohoxmin</summary>
        public uint? ChoshuJohoXmin { get; set; }

        /// <summary>明細行活性非活性フラグ</summary>
        public int? ActivityFlg { get; set; } = 0;

        /// <summary>今回消込み実施フラグ</summary>
        public int? KeshikomiJissiFlg { get; set; } = 0;

        /// <summary>
        /// srcオブジェクトとの比較
        /// </summary>
        /// <param name="src"></param>
        /// <returns></returns>
        public bool Compare(D107060TableRecord src)
        {
            return (
                ($"{this.KumiaiintoCd}" == $"{src.KumiaiintoCd}") &&
                ($"{this.NogyosyaNm}" == $"{src.NogyosyaNm}") &&
                ($"{this.ChoteiGaku}" == $"{src.ChoteiGaku}") &&
                ($"{this.KyosaiKakekin}" == $"{src.KyosaiKakekin}") &&
                ($"{this.Hukakin}" == $"{src.Hukakin}") &&
                ($"{this.ChoshuzumiGaku}" == $"{src.ChoshuzumiGaku}") &&
                ($"{this.ZenkaiChoshuYmd}" == $"{src.ZenkaiChoshuYmd}") &&
                ($"{this.SeikyuGaku}" == $"{src.SeikyuGaku}") &&
                ($"{this.ChoshuGakuNow}" == $"{src.ChoshuGakuNow}") &&
                ($"{this.ChoshuYmd}" == $"{src.ChoshuYmd}") &&
                ($"{this.ChoshuKbn}" == $"{src.ChoshuKbn}") &&
                ($"{this.Riyu}" == $"{src.Riyu}") &&
                ($"{this.Choshusya}" == $"{src.Choshusya}")
            );
        }
    }
}