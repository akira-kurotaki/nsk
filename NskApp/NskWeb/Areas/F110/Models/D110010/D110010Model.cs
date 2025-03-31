using CoreLibrary.Core.Base;
using Microsoft.AspNetCore.Mvc.Rendering;
using ModelLibrary.Models;
using NskAppModelLibrary.Context;
using NskWeb.Areas.F110.Consts;
using NskWeb.Common.Consts;

namespace NskWeb.Areas.F110.Models.D110010
{
    /// <summary>
    /// 再引受前処理モデル
    /// </summary>
    [Serializable]
    public class D110010Model : CoreViewModel
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
        #endregion

        /// <summary>
        /// 本所・支所
        /// </summary>
        public string HonshoShishoCd { get; set; } = string.Empty;
        /// <summary>
        /// 本所・支所リスト
        /// </summary>
        public List<SelectListItem> HonshoShishoList { get; set; } = new();

        #region "引受回"
        public D110010HikiukeKai HikiukeKai { get; set; } = new();
        #endregion


        /// <summary>
        /// ユーザー権限
        /// </summary>
        public F110Const.UserAuthority UserKengen { get; set; } = F110Const.UserAuthority.None;
        /// <summary>
        /// 画面権限
        /// </summary>
        public F110Const.DispAuthority DispKengen { get; set; } = F110Const.DispAuthority.None;

        /// <summary>
        /// 本所・支所ドロップダウン　非活性フラグ
        /// </summary>
        public bool IsShishoDropdownListDisabled { get; set; } = false;
        /// <summary>
        /// 実行ボタン　非活性属性
        /// </summary>
        public string ExecButtonDisableAttr { get; set; } = string.Empty;

        /// <summary>
        /// 非活性属性文字
        /// </summary>
        private const string DISABLED_ATTRIBUTE = "disabled";


        /// <summary>
        /// コンストラクタ
        /// </summary>
        public D110010Model()
        {
        }

        /// <summary>
        /// ドロップダウンリスト初期化
        /// </summary>
        /// <param name="dbContext"></param>
        public void InitializeDropdonwList(NskAppContext dbContext, D110010SessionInfo sessionInfo)
        {
            HonshoShishoList = new();

            List<VShishoNm> shishoNms = new();
            // セッションから取得した「支所コード」
            if (sessionInfo.ShishoCd == AppConst.HONSHO_CD)
            {
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
                else if ((sessionInfo.HikiukeJikkoTanniKbnHikiuke == "2") ||
                         (sessionInfo.HikiukeJikkoTanniKbnHikiuke == "3"))
                {
                    // 「本所配下の支所一覧」を取得する。
                    shishoNms.AddRange(dbContext.VShishoNms.Where(x =>
                        (x.TodofukenCd == sessionInfo.TodofukenCd) &&
                        (x.KumiaitoCd == sessionInfo.KumiaitoCd)
                    )?.
                    OrderBy(x => x.ShishoCd)
                    );

                }
            }
            else
            {
                // 「支所」を取得する。
                // 自支所と利用可能支所
                // 自支所
                List<string> shishoCds = [sessionInfo.ShishoCd];
                shishoCds.AddRange(sessionInfo.RiyokanoShishoList.Select(x => x.ShishoCd));

                // 利用可能支所
                shishoNms.AddRange(dbContext.VShishoNms.Where(x =>
                    (x.TodofukenCd == sessionInfo.TodofukenCd) &&
                    (x.KumiaitoCd == sessionInfo.KumiaitoCd) &&
                    (shishoCds.Contains(x.ShishoCd))
                )?.
                OrderBy(x => x.ShishoCd)
                );

            }

            for (int i = 0; i < shishoNms.Count; i++)
            {
                VShishoNm shisho = shishoNms[i];
                HonshoShishoList.Add(new($"{shisho.ShishoCd} {shisho.ShishoNm}", $"{shisho.ShishoCd}"));
            }

            HonshoShishoCd = shishoNms[0]?.ShishoCd ?? string.Empty;
        }

        /// <summary>
        /// 画面コントロール表示制御
        /// </summary>
        public void DispControl()
        {
            bool isExistsHikiukeKeisanJikkobi = false;
            // 引受回情報ありの場合
            if (HikiukeKai.DispRecords.Count > 0)
            {
                // 引受計算実行日の有無
                isExistsHikiukeKeisanJikkobi = HikiukeKai.DispRecords.Any(x => x.HikiukeKeisanJikkobi.HasValue);
            }

            //// ユーザー権限が「支所」の場合
            //if (UserKengen == F110Const.UserAuthority.Shisho)
            //{
            //    // 本所・支所ドロップダウンリストを非活性にする
            //    IsShishoDropdownListDisabled = true;
            //}

            // 画面権限が「更新権限」 かつ 引受計算実行日の有無が「無し」の場合
            // または、画面権限が「参照権限」の場合
            ExecButtonDisableAttr = string.Empty;
            if (((DispKengen == F110Const.DispAuthority.Update) &&
                 (!isExistsHikiukeKeisanJikkobi)) ||
                (DispKengen == F110Const.DispAuthority.ReadOnly))
            {
                // 実行ボタンを非活性にする
                ExecButtonDisableAttr = DISABLED_ATTRIBUTE;

            }

        }

        /// <summary>
        /// 検索結果（引受回情報）」を比較する。
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="sessionInfo"></param>
        /// <returns></returns>
        public bool IsExistsDiff(NskAppContext dbContext, D110010SessionInfo sessionInfo)
        {
            // １．１．引受回取得メソッドの呼び出し
            List<D110010HikiukeKaiRecord> dbRecs = HikiukeKai.GetResult(dbContext, sessionInfo, HonshoShishoCd);

            // １．２．引受回取得メソッドで取得した結果とセッション「検索結果（引受回情報）」を比較する。
            bool existsDiff = false;
            if (HikiukeKai.DispRecords.Count == dbRecs.Count)
            {
                foreach (D110010HikiukeKaiRecord dispRec in HikiukeKai.DispRecords)
                {
                    // 画面表示中引受回とDB引受回を比較
                    D110010HikiukeKaiRecord? dbRec = dbRecs.FirstOrDefault(x =>
                        (x.ShishoCd == dispRec.ShishoCd) &&
                        (x.HikiukeKai == dispRec.HikiukeKai)
                        );

                    if (dbRec is null)
                    {
                        // 差分あり
                        existsDiff = true;
                        break;
                    }
                }
            }
            else
            {
                // 差分あり
                existsDiff = true;
            }

            return existsDiff;
        }
    }
}
