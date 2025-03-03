using CoreLibrary.Core.Base;
using CoreLibrary.Core.Consts;
using CoreLibrary.Core.Validator;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace NskWeb.Areas.F000.Models.D000000
{
    [Serializable]
    public class NSKPortalInfoModel : CoreViewModel
    {
        public NSKPortalInfoModel()
        {
            SKyosaiMokutekiCd = "";
            SNensanHikiuke = "";
            SNensanHyoka = "";
            SHikiukeJikkoTanniKbnHikiuke = "";
            SHikiukeJikkoTanniKbnHyoka = "";
        }

        [DisplayName("共済目的コード")]
        [FullDigitLength(2)]
        public string  SKyosaiMokutekiCd{ get; set; }
        [DisplayName("引受年産")]
        [Numeric]
        public string SNensanHikiuke { get; set; }
        [DisplayName("評価年産")]
        [Numeric]
        public string SNensanHyoka { get; set; }
        [DisplayName("引受計算支所実行単位区分_引受")]
        public string SHikiukeJikkoTanniKbnHikiuke { get; set; }
        [DisplayName("引受計算支所実行単位区分_評価")]
        public string SHikiukeJikkoTanniKbnHyoka { get; set; }
    }
}