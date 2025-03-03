using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NskReportMain.Models.H002
{
    /// <summary>
    /// H002_加入者情報（単票）集約単位取得用
    /// </summary>
    public class H002GroupInfoModel
    {
        /// <summary>
        /// 年度
        /// </summary>
        public short Nendo { get; set; }

        /// <summary>
        /// 都道府県コード
        /// </summary>
        public string TodofukenCd { get; set; }

        /// <summary>
        /// 組合等コード
        /// </summary>
        public string KumiaitoCd { get; set; }

        /// <summary>
        /// 支所コード
        /// </summary>
        public string ShishoCd { get; set; }
    }
}
