using BaseDmpApi.Const;
using CoreLibrary.Core.Consts;
using CoreLibrary.Core.Dto;
using CoreLibrary.Core.Exceptions;
using CoreLibrary.Core.Utility;
using Microsoft.AspNetCore.Mvc;
using ModelLibrary.Context;
using ModelLibrary.Models;
using NLog;
using System.Data;
using System.Data.Common;
using System.Globalization;

namespace BaseDmpApi.Base
{
    /// <summary>
    /// ベースアプリケーションREST APIのコントローラーの基底クラス
    /// </summary>
    [ApiController]
    public abstract class ApiControllerBase : ControllerBase, IDisposable
    {
        /// <summary>
        /// ロガー
        /// </summary>
        protected static Logger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// 事業共通DBに接続するDBコンテキストクラス
        /// </summary>
        private JigyoCommonContext jigyoCommonDb;

        /// <summary>
        /// システム共通DBに接続するDBコンテキストクラス
        /// </summary>
        private SystemCommonContext systemCommonDb;

        /// <summary>
        /// 農業者情報管理スキーマに接続するDBコンテキスト
        /// </summary>
        private FimContext fimDb;

        /// <summary>
        /// 都道府県別事業スキーマに接続するDBコンテキスト
        /// </summary>
        private JigyoContext jigyoDb;

        /// <summary>
        /// 事業共通DBに接続するDBコンテキストを取得する
        /// </summary>
        /// <returns>事業共通DBに接続するDBコンテキスト</returns>
        protected JigyoCommonContext getJigyoCommonDb()
        {
            if (jigyoCommonDb == null)
            {
                jigyoCommonDb = new JigyoCommonContext();
            }
            return jigyoCommonDb;
        }

        /// <summary>
        /// システム共通DBに接続するDBコンテキストを取得する
        /// </summary>
        /// <returns>システム共通DBに接続するDBコンテキスト</returns>
        protected SystemCommonContext getSystemCommonDb()
        {
            if (systemCommonDb == null)
            {
                systemCommonDb = new SystemCommonContext();
            }
            return systemCommonDb;
        }

        /// <summary>
        /// 都道府県別事業スキーマに接続するDBコンテキストを取得する
        /// 事業システム向け
        /// </summary>
        /// <typeparam name="T">JigyoContext継承クラス</typeparam>
        /// <returns>都道府県別事業スキーマに接続するDBコンテキスト</returns>
        protected T getJigyoDb<T>(ApiRequestBase request) where T : JigyoContext
        {
            if (jigyoDb == null)
            {
                // システム区分
                string systemKbn = ConfigUtil.Get(CoreConst.APP_ENV_SYSTEM_KBN);

                // システム共通スキーマからログインユーザの所属に応じた都道府県別事業スキーマのDB接続先を取得する
                DbConnectionInfo info = DBUtil.GetDbConnectionInfo(systemKbn, request.TodofukenCd, request.KumiaitoCd, request.ShishoCd);
                if (info == null)
                {
                    // TODO：取得できない場合のエラー処理
                    throw new AppException("MF00001", MessageUtil.Get("MF00001"), CoreConst.HEADER_PATTERN_ID_2);
                }

                // 取得したDB接続先からDBコンテキストを作成する
                jigyoDb = (JigyoContext)Activator.CreateInstance(typeof(T), info.ConnectionString, info.DefaultSchema, ConfigUtil.GetInt("CommandTimeout"));
            }

            return (T)jigyoDb;
        }

        /// <summary>
        /// 農業者情報管理スキーマに接続するDBコンテキストを取得する
        /// </summary>
        /// <returns>農業者情報管理スキーマに接続するDBコンテキスト</returns>
        protected FimContext getFimDb()
        {
            if (fimDb == null)
            {
                fimDb = new FimContext();
            }
            return fimDb;
        }

        #region IDisposable Support
        private bool disposedValue = false;

        ~ApiControllerBase()
        {
            Dispose(false);
        }

        /// <summary>
        /// Dispose
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    if (jigyoCommonDb != null)
                    {
                        jigyoCommonDb.Dispose();
                    }

                    if (systemCommonDb != null)
                    {
                        systemCommonDb.Dispose();
                    }

                    if (jigyoDb != null)
                    {
                        jigyoDb.Dispose();
                    }

                    if (fimDb != null)
                    {
                        fimDb.Dispose();
                    }
                }
                disposedValue = true;
            }
        }

        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
        }
        #endregion

        #region DB操作
        /// <summary>
        /// DBの接続が閉じている場合、接続を開く
        /// </summary>
        /// <param name="conn">DB接続</param>
        protected void OpenConnection(DbConnection conn)
        {
            var isMastOpen = conn.State != ConnectionState.Open;
            if (!ConnectionState.Open.Equals(conn.State))
            {
                // DB接続
                conn.Open();
            }
        }
        #endregion

        #region チェック処理

        /// <summary>
        /// 共通部分のチェック処理（必須チェック）
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        protected List<Message> CheckCommonRequest(ApiRequestBase request)
        {
            var errorMsgList = new List<Message>();

            // 必須チェック
            // [pass]が空白の場合、以下を[変数：エラー内容]に追加する。
            // （メッセージID：ME00001、引数{0}："パスフレーズ"、引数{1}：""）
            errorMsgList = ValidateRequired(request.Pass, errorMsgList, "パスフレーズ");

            // 必須チェック
            // [system_kbn]が空白の場合、以下を[変数：エラー内容]に追加する。
            // （メッセージID：ME00001、引数{0}："システム区分"、引数{1}：""）
            errorMsgList = ValidateRequired(request.SystemKbn, errorMsgList, "システム区分");

            // 必須チェック
            // [todofuken_cd]が空白の場合、以下を[変数：エラー内容]に追加する。
            // （メッセージID：ME00001、引数{0}："都道府県コード"、引数{1}：""）
            errorMsgList = ValidateRequired(request.TodofukenCd, errorMsgList, "都道府県コード");

            // 属性チェック
            // [system_kbn]が定数（設定ファイル）：システム区分と一致しない場合、以下を[変数：エラー内容]に追加する。
            // （メッセージID：ME00003、引数{0}："システム区分"、引数{1}："システム区分："）
            if (!string.IsNullOrEmpty(request.SystemKbn) && !request.SystemKbn.Equals(ConfigUtil.Get(CoreConst.APP_ENV_SYSTEM_KBN)))
            {
                errorMsgList = AddErrorMsgList(errorMsgList, "ME00003", "システム区分", "システム区分：" + request.SystemKbn);
            }

            // 属性チェック
            // [todofuken_cd]が数字のみではない場合、以下を[変数：エラー内容]に追加する。
            // （メッセージID：ME00003、引数{0}："都道府県コード"、引数{1}："都道府県コード："）
            errorMsgList = ValidateNumeric(request.TodofukenCd, errorMsgList, "都道府県コード", "都道府県コード：");


            // 属性チェック
            // [kumiaito_cd]が数字のみではない場合、以下を[変数：エラー内容]に追加する。
            // （メッセージID：ME00003、引数{0}："組合等コード"、引数{1}："組合等コード："）
            if (!string.IsNullOrEmpty(request.KumiaitoCd))
            {
                errorMsgList = ValidateNumeric(request.KumiaitoCd, errorMsgList, "組合等コード", "組合等コード：");
            }

            // 属性チェック
            // [shisho_cd]が数字のみではない場合、以下を[変数：エラー内容]に追加する。
            // （メッセージID：ME00003、引数{0}："支所コード"、引数{1}："支所コード："）
            if (!string.IsNullOrEmpty(request.ShishoCd))
            {
                errorMsgList = ValidateNumeric(request.ShishoCd, errorMsgList, "支所コード", "支所コード：");
            }

            return errorMsgList;
        }
        #endregion

        #region レスポンス生成
        /// <summary>
        /// 例外をエラーログに出力し、エラーメッセージおよびHTTPステータス500をレスポンスに設定する
        /// </summary>
        /// <typeparam name="T">エラーレスポンスの型</typeparam>
        /// <param name="e">例外</param>
        /// <param name="messageId">メッセージID</param>
        /// <param name="arg">置換用文字列の配列(可変長)</param>
        /// <returns>エラーレスポンス</returns>
        protected ActionResult CreateErrorResponse<T>(Exception e, string messageId, params string[] arg)
            where T : IApiResponse, new()
        {
            if (e != null)
            {
                logger.Error(e);
            }

            var errorMessages = new List<Message>();
            var errMessage = new Message()
            {
                message = MessageUtil.Get(messageId, arg)
            };
            errorMessages.Add(errMessage);

            Response.StatusCode = StatusCodes.Status500InternalServerError;

            return new JsonResult(new T()
            {
                messages = errorMessages
            });
        }

        /// <summary>
        /// エラーメッセージおよびHTTPステータス500をレスポンスに設定する
        /// </summary>
        /// <typeparam name="T">エラーレスポンスの型</typeparam>
        /// <param name="messageId">メッセージID</param>
        /// <param name="arg">置換用文字列の配列(可変長)</param>
        /// <returns>エラーレスポンス</returns>
        protected ActionResult CreateErrorResponse<T>(string messageId, params string[] arg)
            where T : IApiResponse, new()
        {
            return CreateErrorResponse<T>(null, messageId, arg);
        }

        /// <summary>
        /// エラーメッセージおよびHTTPステータス400をレスポンスに設定する
        /// </summary>
        /// <typeparam name="T">エラーレスポンスの型</typeparam>
        /// <param name="errMessages">エラーメッセージリスト</param>
        /// <returns>エラーレスポンス</returns>
        protected ActionResult CreateBadRequestResponse<T>(List<Message> errMessages)
            where T : IApiResponse, new()
        {
            return BadRequest(new T()
            {
                messages = errMessages,
            });
        }

        /// <summary>
        /// エラーメッセージおよびHTTPステータス401をレスポンスに設定する
        /// </summary>
        /// <typeparam name="T">エラーレスポンスの型</typeparam>
        /// <param name="errMessages">エラーメッセージリスト</param>
        /// <returns>エラーレスポンス</returns>
        protected ActionResult CreateUnauthorizedResponse<T>(List<Message> errMessages)
            where T : IApiResponse, new()
        {
            return Unauthorized(new T()
            {
                messages = errMessages,
            });
        }

        /// <summary>
        /// エラーメッセージおよびHTTPステータス404をレスポンスに設定する
        /// </summary>
        /// <typeparam name="T">エラーレスポンスの型</typeparam>
        /// <param name="messages">エラーメッセージリスト</param>
        /// <returns>エラーレスポンス</returns>
        protected ActionResult CreateNotFoundResponse<T>(List<Message> errMessages)
            where T : IApiResponse, new()
        {
            return NotFound(new T()
            {
                messages = errMessages,
            });
        }

        #endregion

        #region パラメータ値チェック
        /// <summary>
        /// 対象の文字列がnullまたは空文字列、数字のみではない場合、エラーメッセージリストにエラーメッセージ（メッセージID：ME00003）を追加し、
        /// エラーメッセージリスト返却する
        /// 対象件数が指定された(null以外)場合は、対象件数「（x件目）」をエラーメッセージに含める
        /// エラーメッセージはエラーログに出力する
        /// </summary>
        /// <param name="value">検査する文字列</param>
        /// <param name="errorMsgList">エラーメッセージリスト</param>
        /// <param name="paramName">項目名</param>
        /// <param name="rowNo">対象件数</param>
        /// <returns>エラーメッセージを追加したエラーメッセージリスト</returns>
        protected List<Message> ValidateNumeric(string value, List<Message> errorMsgList, string paramName, int? rowNo)
        {
            if (!StringUtil.IsNumeric(value))
            {
                var arg = "";
                if (rowNo != null)
                {
                    arg = "（" + rowNo + "件目）";
                }

                errorMsgList = AddErrorMsgList(errorMsgList, "ME00003", paramName, arg);
            }
            return errorMsgList;
        }

        /// <summary>
        /// 対象の文字列がnullまたは空文字列、数字のみではない場合、エラーメッセージリストにエラーメッセージ（メッセージID：ME00003）を追加し、
        /// エラーメッセージリスト返却する
        /// エラーメッセージはエラーログに出力する
        /// </summary>
        /// <param name="value">検査する文字列</param>
        /// <param name="errorMsgList">エラーメッセージリスト</param>
        /// <param name="paramName">項目名</param>
        /// <returns>エラーメッセージを追加したエラーメッセージリスト</returns>
        protected List<Message> ValidateNumeric(string value, List<Message> errorMsgList, string paramName)
        {
            return ValidateNumeric(value, errorMsgList, paramName, null as int?);
        }

        /// <summary>
        /// 対象の文字列がnullまたは空文字列、数字のみではない場合、エラーメッセージリストにエラーメッセージ（メッセージID：ME00003）を追加し、
        /// エラーメッセージリスト返却する
        /// 説明文字列が指定された(null以外)場合は、説明文字列および対象の文字列をエラーメッセージに含める
        /// エラーメッセージはエラーログに出力する
        /// </summary>
        /// <param name="value">検査する文字列</param>
        /// <param name="errorMsgList">エラーメッセージリスト</param>
        /// <param name="paramName">項目名</param>
        /// <param name="info">説明文字列</param>
        /// <returns>エラーメッセージを追加したエラーメッセージリスト</returns>
        protected List<Message> ValidateNumeric(string value, List<Message> errorMsgList, string paramName, string info)
        {
            if (!StringUtil.IsNumeric(value))
            {
                errorMsgList = AddErrorMsgList(errorMsgList, "ME00003", paramName, info + value);
            }
            return errorMsgList;
        }

        /// <summary>
        /// 対象の文字列の桁数が最大桁数より大きい場合、エラーメッセージリストにエラーメッセージ（メッセージID：ME00014）を追加し、
        /// エラーメッセージリスト返却する
        /// 対象件数が指定された(null以外)場合は、対象件数「（x件目）」をエラーメッセージに含める
        /// エラーメッセージはエラーログに出力する
        /// </summary>
        /// <param name="value">検査する文字列</param>
        /// <param name="maxLength">最大桁数</param>
        /// <param name="errorMsgList">エラーメッセージリスト</param>
        /// <param name="paramName">項目名</param>
        /// <param name="rowNo">対象件数</param>
        /// <returns>エラーメッセージを追加したエラーメッセージリスト</returns>
        protected List<Message> ValidateMaxLength(string value, int maxLength, List<Message> errorMsgList, string paramName, int? rowNo)
        {
            if (!string.IsNullOrEmpty(value) && maxLength < new StringInfo(value).LengthInTextElements)
            {
                var arg = "";
                if (rowNo != null)
                {
                    arg = "（" + rowNo + "件目）";
                }

                errorMsgList = AddErrorMsgList(errorMsgList, "ME00014", paramName, maxLength.ToString(), arg);
            }
            return errorMsgList;
        }

        /// <summary>
        /// 対象の文字列の桁数が最大桁数より大きい場合、エラーメッセージリストにエラーメッセージ（メッセージID：ME00014）を追加し、
        /// エラーメッセージリスト返却する
        /// エラーメッセージはエラーログに出力する
        /// </summary>
        /// <param name="value">検査する文字列</param>
        /// <param name="maxLength">最大桁数</param>
        /// <param name="errorMsgList">エラーメッセージリスト</param>
        /// <param name="paramName">項目名</param>
        /// <returns>エラーメッセージを追加したエラーメッセージリスト</returns>
        protected List<Message> ValidateMaxLength(string value, int maxLength, List<Message> errorMsgList, string paramName)
        {
            return ValidateMaxLength(value, maxLength, errorMsgList, paramName, null as int?);
        }

        /// <summary>
        /// 対象の文字列の桁数が指定された桁数ではない場合、エラーメッセージリストにエラーメッセージ（メッセージID：ME00015）を追加し、
        /// エラーメッセージリスト返却する
        /// エラーメッセージはエラーログに出力する
        /// </summary>
        /// <param name="value">検査する文字列</param>
        /// <param name="length">桁数</param>
        /// <param name="errorMsgList">エラーメッセージリスト</param>
        /// <param name="paramName">項目名</param>
        /// <returns>エラーメッセージを追加したエラーメッセージリスト</returns>
        protected List<Message> ValidateFixLength(string value, int length, List<Message> errorMsgList, string paramName)
        {
            if (!string.IsNullOrEmpty(value) && length != new StringInfo(value).LengthInTextElements)
            {
                errorMsgList = AddErrorMsgList(errorMsgList, "ME00015", paramName, length.ToString(), "");
            }
            return errorMsgList;
        }


        /// <summary>
        /// 対象の文字列の桁数（整数部、小数部）が指定された桁数ではない場合、エラーメッセージリストにエラーメッセージ（メッセージID：ME00004）を追加し、
        /// エラーメッセージリスト返却する
        /// エラーメッセージはエラーログに出力する
        /// </summary>
        /// <param name="value">検査する文字列</param>
        /// <param name="length">桁数（整数部）</param>
        /// <param name="length">桁数（小数部）</param>
        /// <param name="errorMsgList">エラーメッセージリスト</param>
        /// <param name="paramName">項目名</param>
        /// <returns>エラーメッセージを追加したエラーメッセージリスト</returns>
        protected List<Message> ValidateNumberDec(string value, int intMaxLength, int decMaxLength, List<Message> errorMsgList, string paramName)
        {
            if (!string.IsNullOrEmpty(value) && (!NumberUtil.IsNumberDec(value, intMaxLength, decMaxLength)))
            {
                errorMsgList = AddErrorMsgList(errorMsgList, "ME00004", paramName, intMaxLength.ToString(), decMaxLength.ToString(), "");
            }
            return errorMsgList;
        }



        /// <summary>
        /// 対象の文字列に外字が含まれる場合、エラーメッセージリストにエラーメッセージ（メッセージID：ME00013）を追加し、
        /// エラーメッセージリスト返却する
        /// 対象件数が指定された(null以外)場合は、対象件数「（x件目）」をエラーメッセージに含める
        /// エラーメッセージはエラーログに出力する
        /// </summary>
        /// <param name="value">検査する文字列</param>
        /// <param name="errorMsgList">エラーメッセージリスト</param>
        /// <param name="paramName">項目名</param>
        /// <param name="rowNo">対象件数</param>
        /// <returns>エラーメッセージを追加したエラーメッセージリスト</returns>
        protected List<Message> ValidateGaiji(string value, List<Message> errorMsgList, string paramName, int? rowNo)
        {
            var gaiji = StringUtil.CheckMS932ExceptGaiji(value);
            if (!string.IsNullOrEmpty(gaiji))
            {
                var arg = "";
                if (rowNo != null)
                {
                    arg = "（" + rowNo + "件目）";
                }

                errorMsgList = AddErrorMsgList(errorMsgList, "ME00013", paramName, gaiji, arg);
            }
            return errorMsgList;
        }

        /// <summary>
        /// 対象の文字列に外字が含まれる場合、エラーメッセージリストにエラーメッセージ（メッセージID：ME00013）を追加し、
        /// エラーメッセージリスト返却する
        /// エラーメッセージはエラーログに出力する
        /// </summary>
        /// <param name="value">検査する文字列</param>
        /// <param name="errorMsgList">エラーメッセージリスト</param>
        /// <param name="paramName">項目名</param>
        /// <returns>エラーメッセージを追加したエラーメッセージリスト</returns>
        protected List<Message> ValidateGaiji(string value, List<Message> errorMsgList, string paramName)
        {
            return ValidateGaiji(value, errorMsgList, paramName, null as int?);
        }

        /// <summary>
        /// 対象の文字列がnullまたは空文字列、半角カタカナのみではない場合、エラーメッセージリストにエラーメッセージ（メッセージID：ME00011）を追加し、
        /// エラーメッセージリスト返却する
        /// 説明文字列が指定された(null以外)場合は、説明文字列および対象の文字列をエラーメッセージに含める
        /// エラーメッセージはエラーログに出力する
        /// </summary>
        /// <param name="value">検査する文字列</param>
        /// <param name="errorMsgList">エラーメッセージリスト</param>
        /// <param name="paramName">項目名</param>
        /// <param name="info">説明文字列</param>
        /// <returns>エラーメッセージを追加したエラーメッセージリスト</returns>
        protected List<Message> ValidateHalfWidthKatakana(string value, List<Message> errorMsgList, string paramName, string info)
        {
            if (!StringUtil.IsHalfWidthKatakana(value))
            {
                errorMsgList = AddErrorMsgList(errorMsgList, "ME00011", paramName, info + value);
            }
            return errorMsgList;
        }

        /// <summary>
        /// 対象の文字列が日付として正しくない場合、エラーメッセージリストにエラーメッセージ（メッセージID：ME00019）を追加し、
        /// エラーメッセージリスト返却する
        /// エラーメッセージはエラーログに出力する
        /// </summary>
        /// <param name="value">検査する文字列</param>
        /// <param name="value">検査する文字列のフォーマット</param>
        /// <param name="errorMsgList">エラーメッセージリスト</param>
        /// <param name="paramName">項目名</param>
        /// <returns>エラーメッセージを追加したエラーメッセージリスト</returns>
        protected List<Message> ValidateDate(string value, string format, List<Message> errorMsgList, string paramName, string info)
        {
            if (!string.IsNullOrEmpty(value))
            {
                if (!DateTime.TryParseExact(value, format, null, DateTimeStyles.None, out _))//変数に代入せずに無視するためにアンダースコアを使用
                {
                    errorMsgList = AddErrorMsgList(errorMsgList, "ME00019", paramName, info + value);
                }
            }
            return errorMsgList;
        }

        /// <summary>
        /// 対象の文字列が空白(nullまたは空文字列)の場合、エラーメッセージリストにエラーメッセージ（メッセージID：ME00001）を追加し、
        /// エラーメッセージリスト返却する
        /// エラーメッセージはエラーログに出力する
        /// </summary>
        /// <param name="value">検査する文字列</param>
        /// <param name="errorMsgList">エラーメッセージリスト</param>
        /// <param name="paramName">項目名</param>
        /// <returns>エラーメッセージを追加したエラーメッセージリスト</returns>
        protected List<Message> ValidateRequired(string value, List<Message> errorMsgList, string paramName)
        {
            if (string.IsNullOrEmpty(value))
            {
                errorMsgList = AddErrorMsgList(errorMsgList, "ME00001", paramName, "");
            }
            return errorMsgList;
        }

        /// <summary>
        /// エラーメッセージリストにエラーメッセージを追加し、エラーメッセージリスト返却する
        /// エラーメッセージはエラーログに出力する
        /// </summary>
        /// <param name="errorMsgList">エラーメッセージリスト</param>
        /// <param name="messageId">メッセージID</param>
        /// <param name="arg">置換用文字列の配列(可変長)</param>
        /// <returns>エラーメッセージを追加したエラーメッセージリスト</returns>
        protected List<Message> AddErrorMsgList(List<Message> errorMsgList, string messageId, params string[] arg)
        {
            var messageNaiyo = MessageUtil.Get(messageId, arg);
            var errMessage = new Message()
            {
                message = messageNaiyo
            };
            errorMsgList.Add(errMessage);
            logger.Error(messageNaiyo);
            return errorMsgList;
        }
        #endregion

        #region ユーザーID生成
        /// <summary>
        /// 以下のルールでユーザーID作成する
        /// [変数：ユーザID]=[変数：都道府県コード]+[変数：組合等コード]+[変数：支所コード]
        /// ・[変数：都道府県コード]=「todofuken_cd」を2桁になるよう0埋めする
        /// ・[変数：組合等コード]=「kumiaito_cd」を3桁になるよう0埋めする。指定なしは「---」
        /// ・[変数：支所コード]=「shisho_cd」を2桁になるよう0埋めする。指定なしは「--」
        /// </summary>
        /// <param name="request">リクエストパラメータ</param>
        /// <returns>ユーザーID</returns>
        protected string CreateUserId(ApiRequestBase request)
        {
            string kumiaitoCd = string.IsNullOrEmpty(request.KumiaitoCd) ? "---" : request.KumiaitoCd.PadLeft(3, '0');
            string shishoCd = string.IsNullOrEmpty(request.ShishoCd) ? "--" : request.ShishoCd.PadLeft(2, '0');

            return request.TodofukenCd.PadLeft(2, '0') + kumiaitoCd + shishoCd;

        }
        #endregion

        #region トークン生成
        /// <summary>
        /// 以下のルールでトークンを作成する
        /// [変数：トークン]=[変数：ユーザID]＋[変数：現在日時]（yyyyMMddHHmmssfff）
        /// </summary>
        /// <param name="userId">ユーザID</param>
        /// <param name="sysDateTime">現在日時</param>
        /// <returns>トークン</returns>
        protected string CreateShoriKey(string userId, DateTime sysDateTime)
        {
            return userId + BaseDmpApiConst.UNDERBAR2 + sysDateTime.ToString("yyyyMMddHHmmssfff");
        }
        #endregion

        #region APIパスフレーズチェック
        /// <summary>
        /// APIパスフレーズチェック
        /// </summary>
        /// <param name="request">リクエスト</param>
        /// <returns>パスチェック結果</returns>
        protected bool ApiPassCheck(ApiRequestBase request)
        {
            bool result = false;

            // リクエストパスを取得  
            var path = HttpContext.Request.Path;
            // APIの名前（最後の部分）を取得  
            var apiName = path.Value.Split('/').Last();

            // システム共通スキーマに接続
            using (var nohoDb = new SystemCommonContext())
            {
                // APIパスフレーズマスタ取得
                var list = nohoDb.MApiPassphrases.Where(a => a.SystemKbn == request.SystemKbn &&
                                                                a.ShoriNm == apiName &&
                                                                a.TodofukenCd == request.TodofukenCd).ToList();

                if (list.Count == 1)
                {
                    // パスワード平文の場合のチェック
                    //    if(list[0].Passphrase == request.Pass)
                    //    {
                    //        result = true;
                    //    }

                    //debug用（パスワードハッシュの生成）
                    // var hashPwd = PasswordUtil.HashPassword<MApiPassphrase>(list[0], request.Pass);

                    if (PasswordUtil.VerifyHashedPassword(list[0], list[0].Passphrase, request.Pass))
                    {
                        // ログイン成功
                        result = true;
                    }
                }
            }

            return result;

        }
        #endregion
    }
}
