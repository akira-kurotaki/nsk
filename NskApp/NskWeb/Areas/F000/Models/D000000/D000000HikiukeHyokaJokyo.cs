using CoreLibrary.Core.Base;
using CoreLibrary.Core.Consts;
using CoreLibrary.Core.Validator;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NskWeb.Areas.F000.Models.D000000
{
    [Serializable]
    public class D000000HikiukeHyokaJokyo
    {
        public D000000HikiukeHyokaJokyo()
        {
        }
        /// <summary>
        /// データタイプ
        /// </summary>
        [Column("data_type")]
        public string data_type { get; set; }

        /// <summary>
        /// 支所コード
        /// </summary>
        [Column("支所コード")]
        public string 支所コード { get; set; }

        /// <summary>
        /// 支所
        /// </summary>
        [Column("支所")]
        public string 支所 { get; set; }

        /// <summary>
        /// 引受回
        /// </summary>
        [Column("引受回")]
        public short 引受回 { get; set; }

        /// <summary>
        /// 引受方式名称
        /// </summary>
        [Column("引受方式短縮名")]
        public string 引受方式名称 { get; set; }

        /// <summary>
        /// 組合等計引受戸数
        /// </summary>
        [Column("組合等計引受戸数")]
        public Decimal? 組合等計引受戸数 { get; set; }

        /// <summary>
        /// 組合等計引受面積
        /// </summary>
        [Column("組合等計引受面積")]
        public Decimal? 組合等計引受面積 { get; set; }

        /// <summary>
        /// 組合等計共済金額
        /// </summary>
        [Column("組合等計共済金額")]
        public Decimal? 組合等計共済金額 { get; set; }

        /// <summary>
        /// 組合等計組合員等負担共済掛金
        /// </summary>
        [Column("組合等計組合員等負担共済掛金")]
        public Decimal? 組合等計組合員等負担共済掛金 { get; set; }

        /// <summary>
        /// 組合等計賦課金
        /// </summary>
        [Column("組合等計賦課金")]
        public Decimal? 組合等計賦課金 { get; set; }

        /// <summary>
        /// 支払対象被害戸数
        /// </summary>
        [Column("支払対象被害戸数")]
        public Decimal? 支払対象被害戸数 { get; set; }

        /// <summary>
        /// 支払対象被害面積
        /// </summary>
        [Column("支払対象被害面積")]
        public Decimal? 支払対象被害面積 { get; set; }

        /// <summary>
        /// 支払対象支払共済金見込額
        /// </summary>
        [Column("支払対象支払共済金見込額")]
        public Decimal? 支払対象支払共済金見込額 { get; set; }

    }
}