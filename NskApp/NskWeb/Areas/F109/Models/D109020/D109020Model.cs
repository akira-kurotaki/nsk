using CoreLibrary.Core.Base;
using ModelLibrary.Models;
using NskAppModelLibrary.Context;
using NskWeb.Areas.F109.Consts;
using NskWeb.Common.Consts;

namespace NskWeb.Areas.F109.Models.D109020
{
    /// <summary>
    /// 規模別分布状況データ作成設定モデル
    /// </summary>
    [Serializable]
    public class D109020Model : CoreViewModel
    {
        /// <summary>
        /// メッセージエリア1
        /// </summary>
        public string MessageArea1 { get; set; } = string.Empty;

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
        /// 検索条件
        /// </summary>
        public D109020SearchCondition SearchCondition { get; set; } = new();
        #endregion


        #region "規模別分布状況データ"
        /// <summary>
        /// 規模別分布状況データ
        /// </summary>
        public D109020KibobetsuBunpu KibobetsuBunpu { get; set; } = new();
        #endregion


        /// <summary>
        /// 画面権限
        /// </summary>
        public F109Const.DispAuthority DispKengen { get; set; } = F109Const.DispAuthority.None;

        /// <summary>
        /// 支所ドロップダウン　非活性フラグ
        /// </summary>
        public bool IsShishoDropdownListDisabled { get; set; } = false;
        /// <summary>
        /// 登録ボタン　非活性属性
        /// </summary>
        public string InsertButtonDisableAttr { get; set; } = string.Empty;

        /// <summary>
        /// 非活性属性文字
        /// </summary>
        private const string DISABLED_ATTRIBUTE = "disabled";


        /// <summary>
        /// コンストラクタ
        /// </summary>
        public D109020Model()
        {
        }

        /// <summary>
        /// ドロップダウンリスト初期化
        /// </summary>
        /// <param name="dbContext"></param>
        public void InitializeDropdonwList(NskAppContext dbContext, D109020SessionInfo sessionInfo)
        {

            SearchCondition.ShishoList = new();

            List<VShishoNm> shishoNms = new();
            if (sessionInfo.HikiukeJikkoTanniKbnHikiuke == "1")
            {
                // 本所のみ
                shishoNms.AddRange(dbContext.VShishoNms.Where(x =>
                    (x.TodofukenCd == sessionInfo.TodofukenCd) &&
                    (x.KumiaitoCd == sessionInfo.KumiaitoCd) &&
                    (x.ShishoCd == AppConst.HONSHO_CD)
                )?.
                OrderBy(x => x.ShishoCd)
                );
            }
            else if (sessionInfo.HikiukeJikkoTanniKbnHikiuke == "2")
            {
                // ２．４．[組合等設定.引受計算支所実行単位区分]＝「2：本所支所」の場合
                // 「利用可能支所一覧」に一致する支所情報を取得する。（支所コード、支所名）

                List<string> shishoCds = new();
                shishoCds.AddRange(sessionInfo.RiyokanoShishoList.Select(x => x.ShishoCd));
                shishoNms.AddRange(dbContext.VShishoNms.Where(x =>
                    (x.TodofukenCd == sessionInfo.TodofukenCd) &&
                    (x.KumiaitoCd == sessionInfo.KumiaitoCd) &&
                    (shishoCds.Contains(x.ShishoCd))
                )?.
                OrderBy(x => x.ShishoCd)
                );
            }
            else if (sessionInfo.HikiukeJikkoTanniKbnHikiuke == "3")
            {
                // [セッション：支所コード]に一致する支所情報を取得する。（支所コード、支所名）

                // 「本所配下の支所一覧」を取得する。
                shishoNms.AddRange(dbContext.VShishoNms.Where(x =>
                    (x.TodofukenCd == sessionInfo.TodofukenCd) &&
                    (x.KumiaitoCd == sessionInfo.KumiaitoCd) &&
                    (x.ShishoCd == sessionInfo.ShishoCd)
                )?.
                OrderBy(x => x.ShishoCd)
                );

            }

            for (int i = 0; i < shishoNms.Count; i++)
            {
                VShishoNm shisho = shishoNms[i];
                SearchCondition.ShishoList.Add(new($"{shisho.ShishoCd} {shisho.ShishoNm}", $"{shisho.ShishoCd}"));
            }
        }

        /// <summary>
        /// 画面コントロール表示制御
        /// </summary>
        public void DispControl(D109020SessionInfo sessionInfo)
        {
            // 画面権限（画面機能コード：D109020）
            if (DispKengen != F109Const.DispAuthority.Update)
            {
                // 登録ボタンを非活性にする
                InsertButtonDisableAttr = DISABLED_ATTRIBUTE;
            }

            // 「引受計算支所実行単位区分」
            if ((sessionInfo.HikiukeJikkoTanniKbnHikiuke == "1") ||
                (sessionInfo.HikiukeJikkoTanniKbnHikiuke == "3"))
            {
                IsShishoDropdownListDisabled = true;
            }
        }
    }
}
