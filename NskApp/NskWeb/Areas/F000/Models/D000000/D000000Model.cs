using CoreLibrary.Core.Base;
using CoreLibrary.Core.Dto;
using ModelLibrary.Models;

namespace NskWeb.Areas.F000.Models.D000000
{
    /// <summary>
    /// ポータル
    /// </summary>
    /// <remarks>
    /// 作成日：2018/03/07
    /// 作成者：Gon Etuun
    /// </remarks>
    [Serializable]
    public class D000000Model : CoreViewModel
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public D000000Model()
        {
            this.D000000Info = new NSKPortalInfoModel();  // $$$$$$$$$$$$$$$$$$$
            this.D000000Info2 = new NSKPortalInfoModel();  // $$$$$$$$$$$$$$$$$$$
            this.ShishoList = new List<Shisho>();
            this.ShishoCd = "";
        }


        // $$$$$$$$$$$$$$$$$$$$$
        public NSKPortalInfoModel D000000Info { get; set; }
        public NSKPortalInfoModel D000000Info2 { get; set; }
        // $$$$$$$$$$$$$$$$$$$$$
        public string wtest { get; set; }
        public List<Shisho> ShishoList {  get; set; }

        public string ShishoCd { get; set; }

        /// <summary>
        /// 支所グループID
        /// </summary>
        public int? ShishoGroupId { get; set; }

    }
}