using CoreLibrary.Core.Base;
using CoreLibrary.Core.Consts;
using CoreLibrary.Core.Dto;
using CoreLibrary.Core.Utility;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Storage;
using ModelLibrary.Models;
using NskAppModelLibrary.Context;
using NskAppModelLibrary.Models;
using NskWeb.Areas.F110.Consts;
using NskWeb.Common.Consts;
using System.Text;

namespace NskWeb.Areas.F110.Models.D110020
{
    /// <summary>
    /// 引受確定処理モデル
    /// </summary>
    [Serializable]
    public class D110020Model : CoreViewModel
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
        public D110020HikiukeKai HikiukeKai { get; set; } = new();
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
        public D110020Model()
        {
        }

        /// <summary>
        /// ドロップダウンリスト初期化
        /// </summary>
        /// <param name="dbContext"></param>
        public void InitializeDropdonwList(NskAppContext dbContext, D110020SessionInfo sessionInfo)
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
                //{
                //    // 「利用可能支所一覧」を取得する。
                //    List<string> shishoCds = new();
                //    shishoCds.AddRange(sessionInfo.RiyokanoShishoList.Select(x => x.ShishoCd));
                //    shishoNms.AddRange(dbContext.VShishoNms.Where(x =>
                //        (x.TodofukenCd == sessionInfo.TodofukenCd) &&
                //        (x.KumiaitoCd == sessionInfo.KumiaitoCd) &&
                //        (shishoCds.Contains(x.ShishoCd))
                //    )?.
                //    OrderBy(x => x.ShishoCd)
                //    );

                //}
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
        /// バッチ予約登録
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="sessionInfo"></param>
        /// <param name="resultMessage"></param>
        /// <returns></returns>
        public int RegistBatchYoyaku(NskAppContext dbContext, D110020SessionInfo sessionInfo, ref string resultMessage)
        {
            // １．１．バッチ条件を登録する。
            // （１）バッチ条件IDを取得する。
            // Guid.NewGuid().ToString("D")の結果をバッチ条件IDとする。
            string batchJoukenId = Guid.NewGuid().ToString("D");
            DateTime sysDateTime = DateUtil.GetSysDateTime();

            // （２）バッチ条件値を生成する。
            List<T01050バッチ条件> batchJoukens = new();
            StringBuilder dispBatchJokens = new();
            batchJoukens.Add(new()
            {
                バッチ条件id = batchJoukenId,
                連番 = 1,
                条件名称 = NskCommonLibrary.Core.Consts.JoukenNameConst.JOUKEN_NENSAN,
                条件値 = $"{sessionInfo.Nensan}",
                登録日時 = sysDateTime,
                登録ユーザid = sessionInfo.UserId,
                更新日時 = sysDateTime,
                更新ユーザid = sessionInfo.UserId
            });
            dispBatchJokens.AppendLine($"{NskCommonLibrary.Core.Consts.JoukenNameConst.JOUKEN_NENSAN}={sessionInfo.Nensan}");

            batchJoukens.Add(new()
            {
                バッチ条件id = batchJoukenId,
                連番 = 2,
                条件名称 = NskCommonLibrary.Core.Consts.JoukenNameConst.JOUKEN_KYOSAI_MOKUTEKI,
                条件値 = $"{sessionInfo.KyosaiMokutekiCd}",
                登録日時 = sysDateTime,
                登録ユーザid = sessionInfo.UserId,
                更新日時 = sysDateTime,
                更新ユーザid = sessionInfo.UserId
            });
            dispBatchJokens.AppendLine($"{NskCommonLibrary.Core.Consts.JoukenNameConst.JOUKEN_KYOSAI_MOKUTEKI}={sessionInfo.KyosaiMokutekiCd}");

            batchJoukens.Add(new()
            {
                バッチ条件id = batchJoukenId,
                連番 = 3,
                条件名称 = JoukenNameConst.JOUKEN_SHISHO,
                条件値 = $"{sessionInfo.ShishoCd}",
                登録日時 = sysDateTime,
                登録ユーザid = sessionInfo.UserId,
                更新日時 = sysDateTime,
                更新ユーザid = sessionInfo.UserId
            });
            dispBatchJokens.AppendLine($"{JoukenNameConst.JOUKEN_SHISHO}={sessionInfo.ShishoCd}");

            // （３）バッチ条件を登録する。
            IDbContextTransaction? transaction = null;
            try
            {
                transaction = dbContext.Database.BeginTransaction();
                dbContext.T01050バッチ条件s.AddRange(batchJoukens);
                dbContext.SaveChanges();
                transaction.Commit();

            }
            catch (Exception ex)
            {
                transaction?.Rollback();
                resultMessage = MessageUtil.Get("MI01645", "バッチの予約登録");

                return BatchUtil.RET_FAIL;
            }


            // ２．２．バッチ予約を登録する。
            string message = string.Empty;
            long batchId = 0;

            // バッチ予約登録（BatchUtil.InsertBatchYoyaku()）を呼び出し、バッチ登録を行う。
            int result = BatchUtil.InsertBatchYoyaku(
                AppConst.BatchBunrui.BATCH_BUNRUI_90_OTHER,
                CoreConst.SystemKbn.Nsk,
                sessionInfo.TodofukenCd,
                sessionInfo.KumiaitoCd,
                HonshoShishoCd,
                sysDateTime,
                sessionInfo.UserId,
                "NSK_111020D",
                "NSK_110020B",
                batchJoukenId,
                dispBatchJokens.ToString(),
                "0",
                BatchUtil.BATCH_TYPE_PATROL,
                null,
                "0",
                ref message,
                ref batchId);

            if (result == BatchUtil.RET_SUCCESS)
            {
                resultMessage = MessageUtil.Get("MI00004", "バッチの予約登録");
            }
            else
            {
                resultMessage = MessageUtil.Get("ME01645", "バッチの予約登録");
            }

            return result;
        }


        /// <summary>
        /// 検索結果（引受回情報）」を比較する。
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="sessionInfo"></param>
        /// <returns></returns>
        public bool IsExistsDiff(NskAppContext dbContext, D110020SessionInfo sessionInfo)
        {
            // １．１．引受回取得メソッドの呼び出し
            List<D110020HikiukeKaiRecord> dbRecs = HikiukeKai.GetResult(dbContext, sessionInfo, HonshoShishoCd);

            // １．２．引受回取得メソッドで取得した結果とセッション「検索結果（引受回情報）」を比較する。
            bool existsDiff = false;
            if (HikiukeKai.DispRecords.Count == dbRecs.Count)
            {
                foreach (D110020HikiukeKaiRecord dispRec in HikiukeKai.DispRecords)
                {
                    // 画面表示中引受回とDB引受回を比較
                    D110020HikiukeKaiRecord? dbRec = dbRecs.FirstOrDefault(x =>
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

        /// <summary>
        /// バッチ予約状況有無をチェックする
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="sessionInfo"></param>
        /// <returns></returns>
        public bool IsExistsBatchYoyaku(NskAppContext dbContext, D110020SessionInfo sessionInfo)
        {
            bool existsYoyaku = false;

            // バッチ予約状況取得
            // バッチ予約状況取得引数の設定
            BatchUtil.GetBatchYoyakuListParam batchYoyakuListparam = new()
            {
                SystemKbn = CoreConst.SystemKbn.Nsk,
                TodofukenCd = sessionInfo.TodofukenCd,
                KumiaitoCd = sessionInfo.KumiaitoCd,
                ShishoCd = sessionInfo.ShishoCd,
                BatchNm = "NSK_110020B"
            };
            // 総件数取得フラグ
            bool boolAllCntFlg = false;
            // 件数（出力パラメータ）
            int intAllCnt = 0;
            // エラーメッセージ（出力パラメータ）
            string message = string.Empty;
            // バッチ予約状況取得登録（BatchUtil.GetBatchYoyakuList()）を呼び出し、バッチ予約状況を取得する。
            List<BatchYoyaku> batchYoyakuList = BatchUtil.GetBatchYoyakuList(batchYoyakuListparam, boolAllCntFlg, ref intAllCnt, ref message);

            // ２．１．１．取得したバッチ予約状況に画面指定の支所コードに対するバッチ予約（バッチ条件）があるか確認する。
            foreach (BatchYoyaku yoyaku in batchYoyakuList)
            {
                // 処理待ち
                if (yoyaku.BatchStatus == BatchUtil.BATCH_STATUS_WAITING)
                {
                    if (dbContext.T01050バッチ条件s.Any(x =>
                        (x.バッチ条件id == yoyaku.BatchJoken) &&
                        (x.条件名称 == JoukenNameConst.JOUKEN_SHISHO) &&
                        ((x.条件値 == HonshoShishoCd) || (x.条件値 == AppConst.HONSHO_CD))))
                    {
                        // 指定した支所コードまたは本所に対する処理待ちの予約あり
                        existsYoyaku = true;
                        break;
                    }
                }
            }

            return existsYoyaku;
        }

        /// <summary>
        /// 引受確定処理対象取得
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="sessionInfo"></param>
        /// <returns></returns>
        public F110Const.HikiukeKakuteiShoriTaisho? GetShoriTaisho(NskAppContext dbContext, D110020SessionInfo sessionInfo)
        {
            F110Const.HikiukeKakuteiShoriTaisho? returnVal = null;

            List<D110020HikiukeKaiRecord> targetRecs = new();
            // ３．１．セッション「検索結果（引受回情報）」に格納されているレコード分以下の処理を繰り返す
            // ３．１．１．セッション「検索結果（引受回情報）」からデータを１件取得
            foreach (D110020HikiukeKaiRecord dispRec in HikiukeKai.DispRecords)
            {
                // ３．１．２．取得した検索結果（引受回情報）の引受計算実行日がNULL以外の場合、セッション「処理対象（引受回情報）に追加する
                if (dispRec.HikiukeKeisanJikkobi.HasValue)
                {
                    targetRecs.Add(dispRec);
                }
            }

            // ３．２．「２．１．２．」でセッション「処理対象（引受回情報）」に追加したレコードの判定をチェックし、結果を返す
            // 引受確定の件数
            int hikiukeKakuteiCnt = targetRecs.Where(x => x.HikiukeKakuteiNensan.HasValue).Count();
            if (hikiukeKakuteiCnt == 0)
            {
                // 0:全件NULL
                returnVal = F110Const.HikiukeKakuteiShoriTaisho.AllHikiukeMikakutei;
            }
            else if (hikiukeKakuteiCnt == targetRecs.Count)
            {
                // 引受回と引受確定の数が一致した場合

                // 1:全件セッション[年産]と一致
                returnVal = F110Const.HikiukeKakuteiShoriTaisho.AllHikiukeKakuteiZumi;
            }
            else
            {
                // 混在

                // 3:全件セッション[年産]と一致
                returnVal = F110Const.HikiukeKakuteiShoriTaisho.IchibuHikiukeKakuteiZumi;
            }

            return returnVal;
        }
    }
}
