using NskApi.Base;
using NskApi.Const;
using NskApi.Models;
using NskAppModelLibrary.Context;
using NskAppModelLibrary.Models;
using CoreLibrary.Core.Attributes;
using CoreLibrary.Core.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using System.Numerics;
using System.Text;

namespace NskApi.Controllers
{
    /// <summary>
    /// 加入者情報取得
    /// </summary>
    [Route("[controller]")]
    [ApiController]
    [ExcludeSystemLockCheck]
    public class GetKanyuShinseiController : ApiControllerBase
    {
        /// <summary>
        /// 加入者情報取得
        /// </summary>
        /// <param name="request">リクエスト</param>
        /// <returns>加入者情報</returns>
        [HttpPost]
        public ActionResult<GetKanyuShinseiResponse> GetKanyuShinsei(GetKanyuShinseiRequest request)
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

                //DBコンテキスト取得
                var jigyoDb = getJigyoDb<NskAppContext>(request);

                // トランザクション開始
                using (var tx = jigyoDb.Database.BeginTransaction())
                {
                    try
                    {
                        // トークン取得
                        var token = request.Token;

                        // トークンが空の場合
                        if (string.IsNullOrEmpty(token))
                        {
                            // 加入者者情報取得
                            var num = CountNogyosha(jigyoDb,request);

                            if (num == 0)
                            {
                                logger.Error(MessageUtil.Get("ME01644", "該当する加入者情報"));
                                return CreateErrorResponse<ApiResponseBase>("ME01644", "該当する加入者情報");
                            }

                            //処理キー取得
                            var shoriKey = CreateShoriKey(userId, sysDateTime);

                            // 一時連携加入者登録
                            token = SetRenkeiKanyushas(jigyoDb, request, shoriKey);
                            jigyoDb.SaveChanges();
                        }

                        // 一時連携加入者取得
                        var resKanyushas = GetRenkeiKanyushas(jigyoDb, token);
                        if (resKanyushas.Count() == 0)
                        {
                            tx.Rollback();
                            logger.Error(MessageUtil.Get("ME01644", "該当する加入者情報"));
                            return CreateErrorResponse<ApiResponseBase>("ME01644", "該当する加入者情報");

                        }

                        // 一時連携加入者更新
                        UpdateRenkeiKanyushas(jigyoDb, token);
                        jigyoDb.SaveChanges();

                        tx.Commit();

                        // 一時連携加入者残件取得
                        var kensu = jigyoDb.WRenkeiKanyushas.Where(p => p.ShoriKey == token && p.RenkeiStatus == NskApiConst.RENKEI_MI).Count();

                        //返却データ
                        return Ok(SetResponse(token, kensu, resKanyushas));

                    }
                    catch (Exception ex)
                    {
                        tx.Rollback();
                        logger.Error(ex);
                        logger.Error(MessageUtil.Get("ME01645", "加入者情報の取得"));
                        return CreateErrorResponse<ApiResponseBase>("ME01645", "加入者情報の取得");
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                logger.Error(MessageUtil.Get("ME01645", "加入者情報の取得"));
                return CreateErrorResponse<ApiResponseBase>("ME01645", "加入者情報の取得");
            }
        }

        #region 連携データ部のチェック処理

        /// <summary>
        /// 連携データ部のチェック処理
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        protected List<Message> CheckRenkeiRequest(GetKanyuShinseiRequest request)
        {
            var errorMsgList = new List<Message>();

            // 必須チェック
            // [nendo]が空白の場合、以下を[変数：エラー内容]に追加する。
            // （メッセージID：ME00001、引数{0}："対象年度"、引数{1}：""）
            errorMsgList = ValidateRequired(request.Nendo, errorMsgList, "対象年度");

            // 必須チェック
            // [renkei_todofuken_cd]が空白の場合、以下を[変数：エラー内容]に追加する。
            // （メッセージID：ME00001、引数{0}："連携都道府県コード"、引数{1}：""）
            errorMsgList = ValidateRequired(request.RenkeiTodofukenCd, errorMsgList, "連携都道府県コード");



            // 属性チェック
            // [nendo]が数字のみではない場合、以下を[変数：エラー内容]に追加する。
            // （メッセージID：ME00003、引数{0}："対象年度"、引数{1}："対象年度："）
            errorMsgList = ValidateNumeric(request.Nendo, errorMsgList, "対象年度", "対象年度：");

            // 属性チェック
            // [renkei_todofuken_cd]が数字のみではない場合、以下を[変数：エラー内容]に追加する。
            // （メッセージID：ME00003、引数{0}："連携都道府県コード"、引数{1}："連携都道府県コード："）
            errorMsgList = ValidateNumeric(request.RenkeiTodofukenCd, errorMsgList, "連携都道府県コード", "連携都道府県コード：");


            // 属性チェック
            // [renkei_kumiaito_cd]が数字のみではない場合、以下を[変数：エラー内容]に追加する。
            // （メッセージID：ME00003、引数{0}："連携組合等コード"、引数{1}："連携組合等コード："）
            if (!string.IsNullOrEmpty(request.RenkeiKumiaitoCd))
            {
                errorMsgList = ValidateNumeric(request.RenkeiKumiaitoCd, errorMsgList, "連携組合等コード", "連携組合等コード：");
            }

            // 属性チェック
            // [renkei_shisho_cd]が数字のみではない場合、以下を[変数：エラー内容]に追加する。
            // （メッセージID：ME00003、引数{0}："連携支所コード"、引数{1}："連携支所コード："）
            if (!string.IsNullOrEmpty(request.RenkeiShishoCd))
            {
                errorMsgList = ValidateNumeric(request.RenkeiShishoCd, errorMsgList, "連携支所コード", "連携支所コード：");
            }
            // 属性チェック
            // [kanyusha_cd_start]が数字のみではない場合、以下を[変数：エラー内容]に追加する。
            // （メッセージID：ME00003、引数{0}："加入者管理コード開始"、引数{1}："加入者管理コード開始："）
            if (!string.IsNullOrEmpty(request.KanyushaCdStart))
            { 
            errorMsgList = ValidateNumeric(request.KanyushaCdStart, errorMsgList, "加入者管理コード開始", "加入者管理コード開始：");
            }
            // 属性チェック
            // [kanyusha_cd_end]が数字のみではない場合、以下を[変数：エラー内容]に追加する。
            // （メッセージID：ME00003、引数{0}："加入者管理コード終了"、引数{1}："加入者管理コード終了："）
            if (!string.IsNullOrEmpty(request.KanyushaCdEnd))
            {
                errorMsgList = ValidateNumeric(request.KanyushaCdEnd, errorMsgList, "加入者管理コード終了", "加入者管理コード終了：");
            }

            return errorMsgList;
        }
        #endregion

        #region 加入者情報取得（件数）
        /// <summary>
        /// 加入者情報取得（件数）
        /// </summary>
        /// <param name="request">リクエストパラメータ</param>
        /// <returns>加入者件数</returns>
        private int CountNogyosha(NskAppContext jigyoDb, GetKanyuShinseiRequest request)
        {
            // sql用定数定義
            var sql = new StringBuilder();
            var parameters = new List<NpgsqlParameter>();

            // 件数取得
            sql.Append("SELECT COUNT(1) AS \"Value\" ");

            sql.Append("FROM ");
            sql.Append("  v_nogyosha ");
            sql.Append("LEFT JOIN t_kanyusha ");
            sql.Append("  ON v_nogyosha.nogyosha_id = t_kanyusha.nogyosha_id ");

            sql.Append("WHERE '1' = '1' ");

            // 共通部の都道府県
            sql.Append("AND v_nogyosha.todofuken_cd = @TodofukenCd_1 ");
            parameters.Add(new NpgsqlParameter("@TodofukenCd_1", request.TodofukenCd));

            // 共通部の組合等
            if (!string.IsNullOrEmpty(request.KumiaitoCd))
            {
                // ※共通部の[組合等コード]が空でない場合のみ実施
                sql.Append("AND v_nogyosha.kumiaito_cd = @KumiaitoCd_1 ");
                parameters.Add(new NpgsqlParameter("@KumiaitoCd_1", request.KumiaitoCd));
            }

            // 共通部の支所
            if (!string.IsNullOrEmpty(request.ShishoCd))
            {
                // ※共通部の[支所コード]が空でない場合のみ実施
                sql.Append("AND v_nogyosha.shisho_cd = @ShishoCd_1 ");
                parameters.Add(new NpgsqlParameter("@ShishoCd_1", request.ShishoCd));
            }

            // 連携データ部の対象年度
            sql.Append("AND t_kanyusha.nendo = @Nendo ");
            parameters.Add(new NpgsqlParameter("@Nendo", int.Parse(request.Nendo)));

            // 連携データ部の都道府県コード
            sql.Append("AND v_nogyosha.todofuken_cd = @TodofukenCd_2 ");
            parameters.Add(new NpgsqlParameter("@TodofukenCd_2", request.RenkeiTodofukenCd));

            // 連携データ部の組合等
            if (!string.IsNullOrEmpty(request.RenkeiKumiaitoCd))
            {
                // ※共通部の[組合等コード]が空でない場合のみ実施
                sql.Append("AND v_nogyosha.kumiaito_cd = @KumiaitoCd_2 ");
                parameters.Add(new NpgsqlParameter("@KumiaitoCd_2", request.RenkeiKumiaitoCd));
            }

            // 連携データ部の支所
            if (!string.IsNullOrEmpty(request.RenkeiShishoCd))
            {
                // ※共通部の[支所コード]が空でない場合のみ実施
                sql.Append("AND v_nogyosha.shisho_cd = @ShishoCd_2 ");
                parameters.Add(new NpgsqlParameter("@ShishoCd_2", request.RenkeiShishoCd));
            }

            // 連携データ部の加入者管理コード（開始）
            if (!string.IsNullOrEmpty(request.KanyushaCdStart))
            {
                sql.Append("AND t_kanyusha.kanyusha_cd >= @KanyushaCdFrom ");
                parameters.Add(new NpgsqlParameter("@KanyushaCdFrom", request.KanyushaCdStart));
            }

            // 連携データ部の加入者管理コード（終了）
            if (!string.IsNullOrEmpty(request.KanyushaCdEnd))
            {
                sql.Append("AND t_kanyusha.kanyusha_cd <= @KanyushaCdTo ");
                parameters.Add(new NpgsqlParameter("@KanyushaCdTo", request.KanyushaCdEnd));
            }


            // sql実行 
            return jigyoDb.Database.SqlQueryRaw<int>(sql.ToString(), parameters.ToArray()).Single();

        }
        #endregion

        #region 一時連携加入者登録
        /// <summary>
        /// 一時連携加入者登録
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        private string SetRenkeiKanyushas(NskAppContext jigyoDb, GetKanyuShinseiRequest request,string token)
        {
            // 都道府県コード
            var TodofukenCd = decimal.Parse(request.TodofukenCd);
            //var KumiaitoCd = decimal.Parse(request.KumiaitoCd);

            // トークン設定
            //var token = TodofukenCd.ToString("00")
            //            + BaseApiConst.UNDERBAR2
            //            + DateUtil.GetSysDateTime().ToString("yyyyMMddHHmmssfff");

            // SQL
            var sql = new StringBuilder();
            var parameters = new List<NpgsqlParameter>();

            sql.Append("INSERT INTO w_renkei_kanyusha ");
            sql.Append("SELECT ");
            sql.Append("  @Token");

            parameters.Add(new NpgsqlParameter("@Token", token));

            sql.Append(", '0'");
            sql.Append(", t_kanyusha.nogyosha_id");
            sql.Append(", t_kanyusha.kanyusha_cd");
            sql.Append(", t_kanyusha.nendo");
            sql.Append(", t_kanyusha.kouchi_postal_cd");
            sql.Append(", t_kanyusha.kouchi_address_kana");
            sql.Append(", t_kanyusha.kouchi_address");
            sql.Append(", t_kanyusha.kouchi_menseki");
            sql.Append(", t_kanyusha.kouchi_keitai_cd");
            sql.Append(", t_kanyusha.kojinjoho_toriatsukai_flg");
            sql.Append(", t_kanyusha.kanyu_shinsei_ymd");
            sql.Append(", t_kanyusha.biko");
            sql.Append(", t_kanyusha.insert_user_id");
            sql.Append(", t_kanyusha.insert_date");
            sql.Append(", t_kanyusha.update_user_id");
            sql.Append(", t_kanyusha.update_date");
            sql.Append(" ");
            sql.Append("FROM ");
            sql.Append("    v_nogyosha  ");
            sql.Append("    LEFT JOIN t_kanyusha  ");
            sql.Append("            on v_nogyosha.nogyosha_id = t_kanyusha.nogyosha_id ");
            sql.Append("WHERE ");

            // 共通部の都道府県
            sql.Append("     v_nogyosha.todofuken_cd = @TodofukenCd_1 ");
            parameters.Add(new NpgsqlParameter("@TodofukenCd_1", request.TodofukenCd));

            // 共通部の組合等
            if (!string.IsNullOrEmpty(request.KumiaitoCd))
            {
                sql.Append(" AND v_nogyosha.kumiaito_cd = @KumiaitoCd_1 ");
                parameters.Add(new NpgsqlParameter("@KumiaitoCd_1", request.KumiaitoCd));
            }

            // 共通部の支所
            if (!string.IsNullOrEmpty(request.ShishoCd))
            {
                // ※共通部の[支所コード]が空でない場合のみ実施
                sql.Append("AND v_nogyosha.shisho_cd = @ShishoCd_1 ");
                parameters.Add(new NpgsqlParameter("@ShishoCd_1", request.ShishoCd));
            }


            // 連携データ部の対象年度
            sql.Append("AND t_kanyusha.nendo = @Nendo ");
            parameters.Add(new NpgsqlParameter("@Nendo", int.Parse(request.Nendo)));

            // 連携データ部の都道府県コード
            sql.Append("AND v_nogyosha.todofuken_cd = @TodofukenCd_2 ");
            parameters.Add(new NpgsqlParameter("@TodofukenCd_2", request.RenkeiTodofukenCd));

            // 連携データ部の組合等
            if (!string.IsNullOrEmpty(request.RenkeiKumiaitoCd))
            {
                // ※共通部の[組合等コード]が空でない場合のみ実施
                sql.Append("AND v_nogyosha.kumiaito_cd = @KumiaitoCd_2 ");
                parameters.Add(new NpgsqlParameter("@KumiaitoCd_2", request.RenkeiKumiaitoCd));
            }

            // 連携データ部の支所
            if (!string.IsNullOrEmpty(request.RenkeiShishoCd))
            {
                // ※共通部の[支所コード]が空でない場合のみ実施
                sql.Append("AND v_nogyosha.shisho_cd = @ShishoCd_2 ");
                parameters.Add(new NpgsqlParameter("@ShishoCd_2", request.RenkeiShishoCd));
            }

            // 連携データ部の加入者管理コード（開始）
            if (!string.IsNullOrEmpty(request.KanyushaCdStart))
            {
                sql.Append("AND t_kanyusha.kanyusha_cd >= @KanyushaCdFrom ");
                parameters.Add(new NpgsqlParameter("@KanyushaCdFrom", request.KanyushaCdStart));
            }

            // 連携データ部の加入者管理コード（終了）
            if (!string.IsNullOrEmpty(request.KanyushaCdEnd))
            {
                sql.Append("AND t_kanyusha.kanyusha_cd <= @KanyushaCdTo ");
                parameters.Add(new NpgsqlParameter("@KanyushaCdTo", request.KanyushaCdEnd));
            }

            sql.Append("ORDER BY ");
            sql.Append("    v_nogyosha.nogyosha_id ASC");
            

            // sql実行 
            var rownum = jigyoDb.Database.ExecuteSqlRaw(sql.ToString(), parameters.ToArray());

            return token;
        }
        #endregion

        #region 一時連携加入者取得
        /// <summary>
        /// 一時連携加入者取得
        /// </summary>
        /// <param name="token">トークン</param>
        /// <returns>一時連携加入者</returns>
        private List<WRenkeiKanyusha> GetRenkeiKanyushas(NskAppContext jigyoDb, string token)
        {

            BigInteger limit = BigInteger.Parse(ConfigUtil.Get(NskApiConst.LIMIT_NUM));
            var RenkeiStatus = NskApiConst.RENKEI_MI;

            // SQL
            var sql = new StringBuilder();
            var parameters = new List<NpgsqlParameter>();

            // 一時連携加入者取得
            //sql.Append("SELECT * ");
            sql.Append("SELECT shori_key,renkei_status,nogyosha_id,kanyusha_cd,nendo,kouchi_postal_cd,kouchi_address_kana,kouchi_address,kouchi_menseki,kouchi_keitai_cd,kojinjoho_toriatsukai_flg,kanyu_shinsei_ymd,biko,insert_user_id,insert_date,update_user_id,update_date,w_renkei_kanyusha.xmin ");


            sql.Append("FROM ");
            sql.Append("  w_renkei_kanyusha ");
            sql.Append("WHERE shori_key = @ShoriKey ");
            sql.Append("AND renkei_status = @RenkeiStatus " );
            sql.Append("ORDER BY nogyosha_id ASC " );
            sql.Append("LIMIT @Limit ");
                
            parameters.Add(new NpgsqlParameter("@ShoriKey", token));
            parameters.Add(new NpgsqlParameter("@RenkeiStatus", RenkeiStatus));
            parameters.Add(new NpgsqlParameter("@Limit", limit));

            // sql実行 
            return jigyoDb.Database.SqlQueryRaw<WRenkeiKanyusha>(sql.ToString(), parameters.ToArray()).ToList();

        }
        #endregion


        #region 一時加入者更新
        /// <summary>
        /// 一時加入者更新
        /// </summary>
        /// <param name="token">トークン</param>
        private void UpdateRenkeiKanyushas(NskAppContext jigyoDb, string token)
        {
            BigInteger limit = BigInteger.Parse(ConfigUtil.Get(NskApiConst.LIMIT_NUM));
            var RenkeiStatus = NskApiConst.RENKEI_ZUMI;

            var sql = new StringBuilder();
            var parameters = new List<NpgsqlParameter>();

            sql.Append("UPDATE ");
            sql.Append("  w_renkei_kanyusha ");
            sql.Append("SET ");
            sql.Append("  renkei_status = @RenkeiStatus ");
            sql.Append("FROM ( ");
            sql.Append("    SELECT nogyosha_id ");
            sql.Append("          ,shori_key ");
            sql.Append("    FROM   w_renkei_kanyusha ");
            sql.Append("    WHERE  renkei_status = 0 ");
            sql.Append("      and  shori_key = @ShoriKey ");
            sql.Append(" ORDER BY  nogyosha_id ");
            sql.Append("    LIMIT  @Limit ");
            sql.Append(") tmp ");
            sql.Append("WHERE w_renkei_kanyusha.shori_key = tmp.shori_key");
            sql.Append("  and w_renkei_kanyusha.nogyosha_id = tmp.nogyosha_id;");

            parameters.Add(new NpgsqlParameter("@RenkeiStatus", RenkeiStatus));
            parameters.Add(new NpgsqlParameter("@ShoriKey", token));
            parameters.Add(new NpgsqlParameter("@Limit", limit));

            //var runSql = string.Format(sql.ToString(), KtkConst.RENKEI_ZUMI, token, limit);
            //logger.Info(runSql);

            //nkkdb.Database.ExecuteSqlCommand(runSql);

            // sql実行 
            jigyoDb.Database.ExecuteSqlRaw(sql.ToString(), parameters.ToArray());

        }
        #endregion

        #region 返却データ作成
        /// <summary>
        /// 返却データ作成
        /// </summary>
        /// <param name="token">トークン</param>
        /// <param name="kensu">加入者残件</param>
        /// <param name="nogyoshas">一時連携加入者情報</param>
        /// <returns>レスポンスデータ</returns>
        private GetKanyuShinseiResponse SetResponse(string token, int kensu, List<WRenkeiKanyusha> WRenkeiKanyushas)
        {
            GetKanyuShinseiResponse response = new GetKanyuShinseiResponse();
            var records = new List<GetKanyuShinseiRecord>();

            foreach (var item in WRenkeiKanyushas)
            {
                records.Add(new GetKanyuShinseiRecord
                {
                    NogyoshaId = item.NogyoshaId.ToString(),
                    KanyushaCd = item.KanyushaCd,
                    Nendo = item.Nendo.ToString(),
                    KouchiPostalCd = item.KouchiPostalCd,
                    KouchiAddressKana = item.KouchiAddressKana,
                    KouchiAddress = item.KouchiAddress,
                    KouchiMenseki = item.KouchiMenseki.ToString(),
                    KouchiKeitaiCd = item.KouchiKeitaiCd,
                    KojinjohoToriatsukaiFlg = item.KojinjohoToriatsukaiFlg,
                    KanyuShinseiYmd = item.KanyuShinseiYmd.ToString(),
                    Biko = item.Biko,
                });
            };

            response.token = token;
            response.kensu = kensu;
            response.records = records;

            return response;

        }
        #endregion

    }

}