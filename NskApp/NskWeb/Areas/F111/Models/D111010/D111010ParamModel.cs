using CoreLibrary.Core.Base;
using NskWeb.Areas.F111.Consts;

namespace NskWeb.Areas.F111.Models.D111010
{
    [Serializable]
    public class D111010ParamModel : CoreViewModel
    {

        /// <summary>
        /// 負担金交付区分コード
        /// </summary>
        public string FutankinKofuKbnCd { get; set; } = string.Empty;

        /// <summary>
        /// 交付回
        /// </summary>
        public int Koufukai { get; set; }

    }
}
