using NskAppModelLibrary.Context;
using CoreLibrary.Core.Base;
using System.ComponentModel.DataAnnotations;
using NskAppModelLibrary.Models;
using NskWeb.Areas.F105.Consts;

namespace NskWeb.Areas.F105.Models.D105030
{
    [Serializable]
    public class D105030Model : CoreViewModel
    {
        /// <summary>
        /// メッセージエリア1
        /// </summary>
        public string MessageArea1 { get; set; } = string.Empty;

        /// <summary>
        /// メッセージエリア2
        /// </summary>
        public string MessageArea2 { get; set; } = string.Empty;

        #region "ヘッダ部"
        /// <summary>
        /// 年産
        /// </summary>
        public string Nensan { get; set; } = string.Empty;
        /// <summary>
        /// 共済目的
        /// </summary>
        public string KyosaiMokuteki { get; set; } = string.Empty;
        /// <summary>
        /// 共済目的コード
        /// </summary>
        public string KyosaiMokutekiCd { get; set; } = string.Empty;
        /// <summary>
        /// 組合員等コード
        /// </summary>
        public string KumiaiinToCd { get; set; } = string.Empty;
        /// <summary>
        /// 氏名
        /// </summary>
        public string FullNm { get; set; } = string.Empty;
        /// <summary>
        /// 電話番号
        /// </summary>
        public string Tel { get; set; } = string.Empty;
        /// <summary>
        /// 支所
        /// </summary>
        public string ShishoNm { get; set; } = string.Empty;
        /// <summary>
        /// 支所コード
        /// </summary>
        public string ShishoCd { get; set; } = string.Empty;
        /// <summary>
        /// 市町村
        /// </summary>
        public string ShichosonNm { get; set; } = string.Empty;
        /// <summary>
        /// 市町村コード
        /// </summary>
        public string ShichosonCd { get; set; } = string.Empty;
        /// <summary>
        /// 大地区
        /// </summary>
        public string DaichikuNm { get; set; } = string.Empty;
        /// <summary>
        /// 大地区コード
        /// </summary>
        public string DaichikuCd { get; set; } = string.Empty;
        /// <summary>
        /// 小地区
        /// </summary>
        public string ShochikuNm { get; set; } = string.Empty;
        /// <summary>
        /// 小地区コード
        /// </summary>
        public string ShochikuCd { get; set; } = string.Empty;
        /// <summary>
        /// 合併時識別
        /// </summary>
        public string GappeijiShikibetu { get; set; } = string.Empty;
        #endregion

        #region "検索条件"
        /// <summary>
        /// 引受情報入力タブ：条件選択
        /// </summary>
        [Display(Name = "検索条件")]
        public D105030SearchCondition SearchCondition { get; set; } = new();
        #endregion

        #region "検索結果"
        /// <summary>
        /// 検索結果
        /// </summary>
        public D105030HikiukeSearchResult HikiukeSearchResult { get; set; } = new();
        #endregion


        #region "計算結果"
        /// <summary>
        /// 計算結果
        /// </summary>
        public D105030CalcResult CalcResult { get; set; } = new();
        #endregion


        #region "組合員等毎設定"
        public D105030KumiaiintoSettei KumiaiintoSettei { get; set; } = new();
        #endregion


        #region "類別設定"
        public D105030RuibetsuSettei RuibetsuSettei { get; set; } = new();
        #endregion

        #region "危険段階"
        public D105030KikenDankaiKbn KikenDankaiKbn { get; set; } = new();
        #endregion


        /// <summary>
        /// 画面権限
        /// </summary>
        public F105Const.Authority DispKengen { get; set; } = F105Const.Authority.None;

        /// <summary>
        /// 引受確定の有無
        /// </summary>
        public bool ExistsHikiukeKakutei { get; set; } = false;


        /// <summary>
        /// 引受確定の有無を取得する
        /// </summary>
        /// <param name="dbContext"></param>
        /// <returns></returns>
        public bool GetHikiukeKakutei(NskAppContext dbContext, D105030SessionInfo sessionInfo)
        {
            bool isHikiukeKakutei = false;
            // ２．１８．引受確定の有無を取得する。		
            // 	(1) t_00010_引受回テーブルから引受回を取得する。
            IQueryable<T00010引受回> hikiukeKais = dbContext.T00010引受回s.Where(m =>
                (m.組合等コード == sessionInfo.KumiaitoCd) &&
                (m.年産 == sessionInfo.Nensan) &&
                (m.共済目的コード == sessionInfo.KyosaiMokutekiCd));
            short? maxHikiukeKai = null;
            if (hikiukeKais.Count() > 0)
            {
                maxHikiukeKai = hikiukeKais.Max(m => m.引受回);
            }

            // 	(2) t_00020_引受確定テーブルから引受確定の有無を取得する。
            isHikiukeKakutei = dbContext.T00020引受確定s.Any(m =>
                (m.組合等コード == sessionInfo.KumiaitoCd) &&
                (m.年産 == sessionInfo.Nensan) &&
                (m.共済目的コード == sessionInfo.KyosaiMokutekiCd) &&
                (m.確定引受回 == maxHikiukeKai)
                );

            return isHikiukeKakutei;
        }

    }
}
