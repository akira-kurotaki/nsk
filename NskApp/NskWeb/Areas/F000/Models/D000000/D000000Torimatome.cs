using CoreLibrary.Core.Base;
using CoreLibrary.Core.Consts;
using CoreLibrary.Core.Validator;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NskWeb.Areas.F000.Models.D000000
{
    [Serializable]
    public class D000000Torimatome
    {
        public D000000Torimatome()
        {
        }
        /// <summary>
        /// 報告回
        /// </summary>
        [Column("報告回")]
        public short? 報告回 { get; set; }

        /// <summary>
        /// 報告実施日
        /// </summary>
        [Column("報告実施日")]
        public string 報告実施日 { get; set; }

        /// <summary>
        /// 交付回
        /// </summary>
        [Column("交付回")]
        public short? 交付回 { get; set; }

        /// <summary>
        /// 交付計算実施日
        /// </summary>
        [Column("交付計算実施日")]
        public string 交付計算実施日 { get; set; }

        /// <summary>
        /// 登録日時
        /// </summary>
        [Column("当初評価高登録日時")]
        public string 当初評価高とりまとめ計算 { get; set; }

        /// <summary>
        /// 登録日時
        /// </summary>
        [Column("保険金計算登録日時")]
        public string 保険金計算 { get; set; }

    }
}