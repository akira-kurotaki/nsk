using CoreLibrary.Core.Base;
using NskWeb.Areas.F111.Consts;

namespace NskWeb.Areas.F111.Models.D111050
{
    [Serializable]
    public class D111050Model : CoreViewModel
    {
        /// <summary>
        /// メッセージエリア1
        /// </summary>
        public string MessageArea1 { get; set; } = string.Empty;


        #region "ヘッダ部"
        /// <summary>
        /// 負担金交付区分コード
        /// </summary>
        public string FutankinKofuKbnCd { get; set; } = string.Empty;

        /// <summary>
        /// 負担金交付区分
        /// </summary>
        public string FutankinKofuKbn { get; set; } = string.Empty;

        /// <summary>
        /// 年産
        /// </summary>
        public string Nensan { get; set; } = string.Empty;

        /// <summary>
        /// 適用交付回
        /// </summary>
        public string Koufukai { get; set; } = string.Empty;

        #endregion

        #region "交付金計算掛金徴収額入力"
        public D111050KakekinChoshugaku KakekinChoshugaku { get; set; } = new();
        #endregion

        #region "徴収済み額"
        public D111050ChoshuzumiGaku ChoshuzumiGaku { get; set; } = new();
        #endregion

        /// <summary>
        /// 画面権限
        /// </summary>
        public F111Const.Authority DispKengen { get; set; } = F111Const.Authority.None;
    }
}
