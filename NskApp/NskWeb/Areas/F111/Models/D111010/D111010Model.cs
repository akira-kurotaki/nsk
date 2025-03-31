using CoreLibrary.Core.Base;
using NskWeb.Areas.F111.Consts;

namespace NskWeb.Areas.F111.Models.D111010
{
    [Serializable]
    public class D111010Model : CoreViewModel
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

        #endregion

        #region "交付金計算処理"
        /// <summary>
        /// 交付金計算処理レコード
        /// </summary>
        public D111010KoufukinKeisan KoufukinKeisan { get; set; } = new();
        #endregion

        /// <summary>
        /// 初回交付回フラグ
        /// </summary>
        public bool syokaiKoufukaiFlg { get; set; } = false;

        /// <summary>
        /// 最新交付回掛金徴収額入力済みフラグ
        /// </summary>
        public bool saishinChoshuGakuNyuryokuzumiFlg { get; set; } = false;

        /// <summary>
        /// バッチ予約済みフラグ
        /// </summary>
        public bool batchYoyakuFlg { get; set; } = false;

        /// <summary>
        /// 交付回なしフラグ
        /// </summary>
        public bool noneKoufukaiFlg { get; set; } = false;

        /// <summary>
        /// 画面権限
        /// </summary>
        public F111Const.Authority DispKengen { get; set; } = F111Const.Authority.None;
    }
}
