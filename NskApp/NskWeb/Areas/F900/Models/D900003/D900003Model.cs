using CoreLibrary.Core.Base;
using System;

namespace SynMain.Areas.F900.Models.D900003
{
    /// <summary>
    /// システムエラー
    /// </summary>
    /// <remarks>
    /// 作成日：2018/01/15
    /// 作成者：MATSUBAYASHI Atsushi
    /// </remarks>
    public class D900003Model : CoreViewModel
    {
        // エラー発生日時
        public String ErrorTime { get; set; }

        // エラーコード
        public String ErrorCode { get; set; }

        // NoDB
        public bool NoDB { get; set; }

        // ヘッダーパターンID
        public String HeaderPatternId { get; set; }
    }
}