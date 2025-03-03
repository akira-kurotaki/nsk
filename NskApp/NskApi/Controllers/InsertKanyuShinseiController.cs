using NskApi.Base;
using NskApi.Models;
using NskAppModelLibrary.Context;
using NskAppModelLibrary.Models;
using CoreLibrary.Core.Attributes;
using CoreLibrary.Core.Utility;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace NskApi.Controllers
{
    /// <summary>
    /// 加入者情報登録
    /// </summary>
    [Route("[controller]")]
    [ApiController]
    public class InsertKanyuShinseiController : ApiControllerBase
    {
        /// <summary>
        /// 加入者情報登録
        /// </summary>
        /// <param name="request">リクエスト</param>
        /// <returns>加入者情報</returns>
        [HttpPost]
        public ActionResult<IApiResponse> InsertKanyuShinsei(InsertKanyuShinseiRequest request)
        {
            //debug 例外処理の外側で強制的に例外を発生
            //throw new Exception("強制的に例外を発生");
            
            try
            {

                var messages = new List<Message>();

                //リクエストの形式が不正な場合、以下のHTTPステータスを設定し、呼び出し元に返却する。
                // （HTTPステータス：400（BAD REQUEST））
                if (!ModelState.IsValid)
                {
                    logger.Error(MessageUtil.Get("ME01646", "リクエスト"));
                    messages = AddErrorMsgList(messages, "ME01646", "リクエスト");
                    return CreateBadRequestResponse<ApiResponseBase>(messages);
                }

                // 共通部分のチェック処理
                messages = CheckCommonRequest(request);
                if (0 < messages.Count)
                {
                    // [変数：エラー内容]に値が設定されている場合
                    // エラー用のレスポンスに[変数：エラー内容]と以下のHTTPステータスを設定し、呼び出し元に返却する。
                    // （HTTPステータス：400（BAD REQUEST））
                    return CreateBadRequestResponse<ApiResponseBase>(messages);
                }

                // 連携データ部のチェック処理
                messages = CheckRenkeiRequest(request);
                if (0 < messages.Count)
                {
                    // [変数：エラー内容]に値が設定されている場合
                    // エラー用のレスポンスに[変数：エラー内容]と以下のHTTPステータスを設定し、呼び出し元に返却する。
                    // （HTTPステータス：400（BAD REQUEST））
                    return CreateBadRequestResponse<ApiResponseBase>(messages);
                }

                // APIパスフレーズチェック
                if (!ApiPassCheck(request))
                {
                    messages = AddErrorMsgList(messages, "ME01646", "リクエスト");
                    logger.Error(MessageUtil.Get("ME01646", "リクエスト")); 
                    return CreateUnauthorizedResponse<ApiResponseBase>(messages); 
                }

                // 組織情報ログ出力
                var userId = CreateUserId(request);
                logger.Info("都道府県・組合等・支所: {UserId}", userId);

                // システム日時
                var sysDateTime = DateUtil.GetSysDateTime();

                // システムロック確認
                var result = ValidateSystemLock(request.SystemKbn, request.TodofukenCd, sysDateTime);
                if (result != null)
                {
                    return result;
                }

                // データロック確認
                result = ValidateDataLock(request.SystemKbn, request.TodofukenCd, request.KumiaitoCd, request.ShishoCd, sysDateTime);
                if (result != null)
                {
                    return result;
                }

                //DBコンテキスト取得
                var jigyoDb = getJigyoDb<NskAppContext>(request);

                // リクエストパラメータの農業者IDをint型へ変換。
                // int型の範囲を超える場合には、0を設定。
                int nogyoshaId;
                int.TryParse(request.NogyoshaId, out nogyoshaId);

                // 農業者情報取得の条件を設定
                var query = jigyoDb.VNogyoshas.Where(v => v.NogyoshaId == nogyoshaId && v.TodofukenCd == request.TodofukenCd);

                if (!string.IsNullOrEmpty(request.KumiaitoCd))
                {
                    query = query.Where(v => v.KumiaitoCd == request.KumiaitoCd);
                }

                if (!string.IsNullOrEmpty(request.ShishoCd))
                {
                    query = query.Where(v => v.ShishoCd == request.ShishoCd);
                }

                // 農業者情報を取得
                var nogyosha = query.ToList();

                // 農業者情報が取得できなかった場合、呼び出し元に以下を返却する。
                // ア） メッセージ （メッセージID：ME01644 、引数１："農業者ID："＋リクエストパラメータ[農業者ID]）
                // イ） HTTPステータス 400（BADREQUEST）
                if (nogyosha.Count == 0)
                {
                    logger.Error(MessageUtil.Get("ME01644", "該当する農業者情報"));
                    return CreateErrorResponse<ApiResponseBase>("ME01644", "該当する農業者情報");
                }

                // リクエストパラメータ対象年度の型変換
                var nendo = short.Parse(request.Nendo);
                //var kouchiMenseki = Decimal.Parse(request.KouchiMenseki);
                //var kanyuShinseiYmd = DateTime.ParseExact(request.KanyuShinseiYmd, "yyyy/MM/dd",null);

                decimal? kouchiMenseki = null;
                DateTime? kanyuShinseiYmd = null;

                // リクエストパラメータ耕地面積の変換
                if (!string.IsNullOrEmpty(request.KouchiMenseki) && 
                    decimal.TryParse(request.KouchiMenseki, out decimal parseKouchiMenseki))
                {
                    kouchiMenseki = parseKouchiMenseki;
                }

                // リクエストパラメータ加入申請年月日の変換
                if (!string.IsNullOrEmpty(request.KanyuShinseiYmd) && 
                    DateTime.TryParseExact(request.KanyuShinseiYmd, "yyyy/MM/dd", null, DateTimeStyles.None, out DateTime parseKanyuShinseiYmd))
                {
                    kanyuShinseiYmd = parseKanyuShinseiYmd;
                }

                // トランザクション開始
                using (var tx = jigyoDb.Database.BeginTransaction())
                {
                    try
                    {
                        // 削除対象レコードを取得
                        var kanyusha = jigyoDb.TKanyushas
                            .FirstOrDefault(t => t.NogyoshaId == nogyoshaId && t.Nendo == nendo);

                        if (kanyusha != null)
                        {
                            // レコードを削除  
                            jigyoDb.TKanyushas.Remove(kanyusha);
                            // 変更を保存  
                            jigyoDb.SaveChanges();

                            //debug用
                            //int affectedRows = jigyoDb.SaveChanges();
                            //Console.WriteLine($"変更された行数：{affectedRows} ");
                        }
                       
                        // 登録対象レコードを作成
                        var newKanyusha = new TKanyusha
                            {
                            NogyoshaId = nogyoshaId,
                            KanyushaCd = request.KanyushaCd,
                            Nendo = nendo,
                            KouchiPostalCd = request.KouchiPostalCd,
                            KouchiAddressKana = request.KouchiAddressKana,
                            KouchiAddress = request.KouchiAddress,
                            KouchiMenseki = kouchiMenseki,
                            KouchiKeitaiCd = request.KouchiKeitaiCd,
                            KojinjohoToriatsukaiFlg = request.KojinjohoToriatsukaiFlg,
                            KanyuShinseiYmd = kanyuShinseiYmd,
                            Biko = request.Biko,
                            InsertUserId = userId,
                            InsertDate = sysDateTime,
                            UpdateUserId = userId,
                            UpdateDate = sysDateTime
                        };

                        // レコードを登録
                        jigyoDb.TKanyushas.Add(newKanyusha);
                        jigyoDb.SaveChanges();

                        tx.Commit();

                        // HTTPステータス　200（OK）を返却
                        return Ok();

                    }
                    catch (Exception ex)
                    {
                        tx.Rollback();
                        logger.Error(ex);
                        logger.Error(MessageUtil.Get("ME01645", "加入者情報の登録"));
                        return CreateErrorResponse<ApiResponseBase>("ME01645", "加入者情報の登録");

                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                logger.Error(MessageUtil.Get("ME01645", "加入者情報の登録"));
                return CreateErrorResponse<ApiResponseBase>("ME01645", "加入者情報の登録");
            }
        }

        #region 連携データ部のチェック処理

        /// <summary>
        /// 連携データ部のチェック処理
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        protected List<Message> CheckRenkeiRequest(InsertKanyuShinseiRequest request)
        {
            var errorMsgList = new List<Message>();


            // 必須チェック
            // [renkei_todofuken_cd]が空白の場合、以下を[変数：エラー内容]に追加する。
            // （メッセージID：ME00001、引数{0}："農業者ID"、引数{1}：""）
            errorMsgList = ValidateRequired(request.NogyoshaId, errorMsgList, "農業者ID");

            // [nendo]が空白の場合、以下を[変数：エラー内容]に追加する。
            // （メッセージID：ME00001、引数{0}："対象年度"、引数{1}：""）
            errorMsgList = ValidateRequired(request.Nendo, errorMsgList, "対象年度");

            // [kouchi_keitai_cd]が空白の場合、以下を[変数：エラー内容]に追加する。
            // （メッセージID：ME00001、引数{0}："耕地形態"、引数{1}：""）
            errorMsgList = ValidateRequired(request.KouchiKeitaiCd, errorMsgList, "耕地形態");

            // [kojinjoho_toriatsukai_flg]が空白の場合、以下を[変数：エラー内容]に追加する。
            // （メッセージID：ME00001、引数{0}："個人情報取扱フラグ"、引数{1}：""）
            errorMsgList = ValidateRequired(request.KojinjohoToriatsukaiFlg, errorMsgList, "個人情報取扱フラグ");


            // 属性チェック
            // [nogyosha_id]が数字のみではない場合、以下を[変数：エラー内容]に追加する。
            // （メッセージID：ME00003、引数{0}："農業者ID"、引数{1}："農業者ID："）
            errorMsgList = ValidateNumeric(request.NogyoshaId, errorMsgList, "農業者ID", "農業者ID：");

            // [kanyusha_cd]が数字のみではない場合、以下を[変数：エラー内容]に追加する。
            // （メッセージID：ME00003、引数{0}："加入者管理コード"、引数{1}："加入者管理コード："）
            if (!string.IsNullOrEmpty(request.KanyushaCd))
            {
                errorMsgList = ValidateNumeric(request.KanyushaCd, errorMsgList, "加入者管理コード");
            }

            // [nendo]が数字のみではない場合、以下を[変数：エラー内容]に追加する。
            // （メッセージID：ME00003、引数{0}："対象年度"、引数{1}："対象年度："）
            errorMsgList = ValidateNumeric(request.Nendo, errorMsgList, "対象年度", "対象年度：");

            // [kouchi_postal_cd]が数字のみではない場合、以下を[変数：エラー内容]に追加する。
            // （メッセージID：ME00003、引数{0}："耕地郵便番号"、引数{1}："耕地郵便番号："）
            if (!string.IsNullOrEmpty(request.KouchiPostalCd))
            {
                errorMsgList = ValidateNumeric(request.KouchiPostalCd, errorMsgList, "耕地郵便番号", "耕地郵便番号：");
            }

            // [kouchi_address_kana]が半角カタカナのみではない場合、以下を[変数：エラー内容]に追加する。
            // （メッセージID：ME00011、引数{0}："耕地住所フリガナ"、引数{1}："耕地住所フリガナ："）
            if (!string.IsNullOrEmpty(request.KouchiAddressKana))
            {
                errorMsgList = ValidateHalfWidthKatakana(request.KouchiAddressKana, errorMsgList, "耕地住所フリガナ", "耕地住所フリガナ：");
            }

            // [kouchi_address]に外字が含まれている場合、以下を[変数：エラー内容]に追加する。
            // （メッセージID：ME00013、引数{0}："耕地住所"、引数{1}："耕地住所："）
            errorMsgList = ValidateGaiji(request.KouchiAddress, errorMsgList, "耕地住所");

            // [kouchi_keitai_cd]が数字のみではない場合、以下を[変数：エラー内容]に追加する。
            // （メッセージID：ME00003、引数{0}："耕地形態"、引数{1}："耕地形態："）
            errorMsgList = ValidateNumeric(request.KouchiKeitaiCd, errorMsgList, "耕地形態", "耕地形態：");

            // [kojinjoho_toriatsukai_flg]が数字のみではない場合、以下を[変数：エラー内容]に追加する。
            // （メッセージID：ME00003、引数{0}："個人情報取扱フラグ"、引数{1}："個人情報取扱フラグ："）
            errorMsgList = ValidateNumeric(request.KojinjohoToriatsukaiFlg, errorMsgList, "個人情報取扱フラグ", "個人情報取扱フラグ：");

            // [kanyu_shinsei_ymd]が日付として正しくない場合、以下を[変数：エラー内容]に追加する。
            // （メッセージID：ME00019、引数{0}："加入申請年月日"、引数{1}："加入申請年月日："）
            errorMsgList = ValidateDate(request.KanyuShinseiYmd, "yyyy/MM/dd", errorMsgList, "加入申請年月日", "加入申請年月日：");

            // [biko]に外字が含まれている場合、以下を[変数：エラー内容]に追加する。
            // （メッセージID：ME00013、引数{0}："備考"、引数{1}："備考："）
            errorMsgList = ValidateGaiji(request.Biko, errorMsgList, "備考");


            //桁数チェック
            // [kanyusha_cd]の桁数が13文字以外の場合、以下を[変数：エラー内容]に追加する。
            // （メッセージID：ME00015、引数{0}："加入者管理コード"、引数{1}：13、引数{2}：""）
            errorMsgList = ValidateFixLength(request.KanyushaCd, 13, errorMsgList, "加入者管理コード");

            // [nendo]の桁数が4文字以外の場合、以下を[変数：エラー内容]に追加する。
            // （メッセージID：ME00015、引数{0}："対象年度"、引数{1}：4、引数{2}：""）
            errorMsgList = ValidateFixLength(request.Nendo, 4, errorMsgList, "対象年度");

            // [kouchi_postal_cd]の桁数が7文字以外の場合、以下を[変数：エラー内容]に追加する。
            // （メッセージID：ME00015、引数{0}："耕地郵便番号"、引数{1}：7、引数{2}：""）
            errorMsgList = ValidateFixLength(request.KouchiPostalCd, 7, errorMsgList, "耕地郵便番号");

            // [kouchi_address_kana]の桁数が60文字より大きい場合、以下を[変数：エラー内容]に追加する。
            // （メッセージID：ME00014、引数{0}："耕地住所フリガナ"、引数{1}：60、引数{2}：""）
            errorMsgList = ValidateMaxLength(request.KouchiAddressKana, 60, errorMsgList, "耕地住所フリガナ");

            // [kouchi_address]の桁数が40文字より大きい場合、以下を[変数：エラー内容]に追加する。
            // （メッセージID：ME00014、引数{0}："耕地住所"、引数{1}：40、引数{2}：""）
            errorMsgList = ValidateMaxLength(request.KouchiAddress, 40, errorMsgList, "耕地住所");

            // [kouchi_menseki]の桁数が整数部が10文字より大きい、または小数部が1文字より大きい場合、以下を[変数：エラー内容]に追加する。
            // （メッセージID：ME00004、引数{0}："耕地面積"、引数{1}：11、引数{2}：""）
            errorMsgList = ValidateNumberDec(request.KouchiMenseki, 9, 1, errorMsgList, "耕地面積");

            // [biko]の桁数が300文字より大きい場合、以下を[変数：エラー内容]に追加する。
            // （メッセージID：ME00014、引数{0}："備考"、引数{1}：300、引数{2}：""）
            errorMsgList = ValidateMaxLength(request.Biko, 300, errorMsgList, "備考");


            //コード値チェック
            // 耕地形態
            if(!string.IsNullOrEmpty(request.KouchiKeitaiCd))
            { 
                var kouchiKeitaiKbnNm = HanyokubunUtil.GetKbnNm("kouchi_keitai", request.KouchiKeitaiCd);
                if (string.IsNullOrEmpty(kouchiKeitaiKbnNm))
                {
                    errorMsgList = AddErrorMsgList(errorMsgList, "ME01644", "耕地形態："+ request.KouchiKeitaiCd);
                }
            }

            // 個人情報取扱フラグ
            if (!string.IsNullOrEmpty(request.KojinjohoToriatsukaiFlg))
            {
                var kojinjohoKbnNm = HanyokubunUtil.GetKbnNm("flg", request.KojinjohoToriatsukaiFlg);
                if (string.IsNullOrEmpty(kojinjohoKbnNm))
                {
                    errorMsgList = AddErrorMsgList(errorMsgList, "ME01644", "個人情報取扱フラグ：" + request.KojinjohoToriatsukaiFlg);
                }
            }

            return errorMsgList;
        }
        #endregion

    }

}