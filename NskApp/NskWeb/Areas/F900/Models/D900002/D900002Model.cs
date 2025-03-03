using CoreLibrary.Core.Base;
using System;

namespace SynMain.Areas.F900.Models.D900002
{
    /// <summary>
    /// 業務エラー
    /// </summary>
    /// <remarks>
    /// 作成日：2018/01/15
    /// 作成者：MATSUBAYASHI Atsushi
    /// </remarks>
    public class D900002Model : CoreViewModel
    {
        /// <summary>
        /// エラー発生日時
        /// </summary>
        public string ErrorTime { get; set; }

        /// <summary>
        /// エラー発生理由
        /// </summary>
        public string ErrorReason { get; set; }

        /// <summary>
        /// ヘッダーパターンID
        /// </summary>
        public string HeaderPatternId { get; set; }

        /// <summary>
        /// 申請者向けメッセージ
        /// </summary>
        public string ShinseiMessage { get; set; }
    }
}