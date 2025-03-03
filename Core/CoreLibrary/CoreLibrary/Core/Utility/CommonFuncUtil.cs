using CoreLibrary.Core.Consts;
using ModelLibrary.Context;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace CoreLibrary.Core.Utility
{
    /// <summary>
    /// 共通機能ユーティリティ
    /// </summary>
    public static class CommonFuncUtil
    {
        /// <summary>
        /// 必須チェック
        /// 対象の文字列が空白(nullまたは空文字列)の場合、
        /// 出力パラメータのエラーメッセージにエラーメッセージ（メッセージID：ME80004）を設定し、falseを返却する。
        /// 上記以外の場合、trueを返却する。
        /// </summary>
        /// <param name="value">検査する文字列</param>
        /// <param name="paramName">項目名</param>
        /// <param name="message">エラーメッセージ（出力パラメータ）</param>
        /// <returns>チェック結果</returns>
        public static bool ValidateRequired(string value, string paramName, ref string message)
        {
            if (string.IsNullOrEmpty(value))
            {
                message = SystemMessageUtil.Get("ME80004", paramName);
                return false;
            }
            return true;
        }

        /// <summary>
        /// 必須チェック
        /// 対象の日時がnullの場合、
        /// 出力パラメータのエラーメッセージにエラーメッセージ（メッセージID：ME80004）を設定し、falseを返却する。
        /// 上記以外の場合、trueを返却する。
        /// </summary>
        /// <param name="value">検査する文字列</param>
        /// <param name="paramName">項目名</param>
        /// <param name="message">エラーメッセージ（出力パラメータ）</param>
        /// <returns>チェック結果</returns>
        public static bool ValidateRequired(DateTime? value, string paramName, ref string message)
        {
            if (value == null)
            {
                message = SystemMessageUtil.Get("ME80004", paramName);
                return false;
            }
            return true;
        }

        /// <summary>
        /// 桁数チェック
        /// 対象の文字列の桁数が最大桁数より大きい場合、
        /// 出力パラメータのエラーメッセージにエラーメッセージ（メッセージID：ME80005）を設定し、falseを返却する。
        /// 上記以外の場合、trueを返却する。
        /// </summary>
        /// <param name="value">検査する文字列</param>
        /// <param name="maxLength">最大桁数</param>
        /// <param name="paramName">項目名</param>
        /// <param name="message">エラーメッセージ（出力パラメータ）</param>
        /// <returns>チェック結果</returns>
        public static bool ValidateMaxLength(string value, int maxLength, string paramName, ref string message)
        {
            if (!string.IsNullOrEmpty(value) && maxLength < new StringInfo(value).LengthInTextElements)
            {
                message = SystemMessageUtil.Get("ME80005", paramName, maxLength.ToString());
                return false;
            }
            return true;
        }

        /// <summary>
        /// マスタ整合性チェック
        /// 汎用区分マスタに指定された値が存在しない場合、
        /// 出力パラメータのエラーメッセージにエラーメッセージ（メッセージID：ME80006）を設定し、falseを返却する。
        /// 上記以外の場合、trueを返却する。
        /// </summary>
        /// <param name="db">システム共通スキーマに接続するDBコンテキスト</param>
        /// <param name="kbnSbt">区分種別</param>
        /// <param name="kbnCd">区分コード</param>
        /// <param name="paramName">項目名</param>
        /// <param name="message">エラーメッセージ（出力パラメータ）</param>
        /// <returns>チェック結果</returns>
        public static bool ValidateHanyokubun(SystemCommonContext db, string kbnSbt, string kbnCd, string paramName, ref string message)
        {
            if (string.IsNullOrEmpty(kbnSbt) || string.IsNullOrEmpty(kbnCd))
            {
                message = SystemMessageUtil.Get("ME80006", paramName, "マスタ設定に含まれるコード");
                return false;
            }

            var mHanyokubun = db.MHanyokubuns.Where(m => m.KbnSbt == kbnSbt && m.KbnCd == kbnCd).SingleOrDefault();
            if (mHanyokubun == null)
            {
                message = SystemMessageUtil.Get("ME80006", paramName, "マスタ設定に含まれるコード");
                return false;
            }

            return true;
        }

        /// <summary>
        /// マスタ整合性チェック
        /// 都道府県マスタに指定された値が存在しない場合、
        /// 出力パラメータのエラーメッセージにエラーメッセージ（メッセージID：ME80006）を設定し、falseを返却する。
        /// 上記以外の場合、trueを返却する。
        /// </summary>
        /// <param name="db">システム共通スキーマに接続するDBコンテキスト</param>
        /// <param name="todofukenCd">都道府県コード</param>
        /// <param name="message">エラーメッセージ（出力パラメータ）</param>
        /// <returns>チェック結果</returns>
        public static bool ValidateTodofuken(SystemCommonContext db, string todofukenCd, ref string message)
        {
            if (string.IsNullOrEmpty(todofukenCd))
            {
                message = SystemMessageUtil.Get("ME80006", "都道府県コード", "マスタ設定に含まれるコード");
                return false;
            }

            // 都道府県コード：都道府県マスタ
            var mTodofuken = db.MTodofukens.Where(m => m.TodofukenCd == todofukenCd).SingleOrDefault();
            if (mTodofuken == null)
            {
                message = SystemMessageUtil.Get("ME80006", "都道府県コード", "マスタ設定に含まれるコード");
                return false;
            }

            return true;
        }

        /// <summary>
        /// マスタ整合性チェック
        /// 組合等マスタに指定された値が存在しない場合、
        /// 出力パラメータのエラーメッセージにエラーメッセージ（メッセージID：ME80006）を設定し、falseを返却する。
        /// 上記以外の場合、trueを返却する。
        /// </summary>
        /// <param name="db">システム共通スキーマに接続するDBコンテキスト</param>
        /// <param name="todofukenCd">都道府県コード</param>
        /// <param name="kumiaitoCd">組合等コード</param>
        /// <param name="message">エラーメッセージ（出力パラメータ）</param>
        /// <returns>チェック結果</returns>
        public static bool ValidateKumiaito(SystemCommonContext db, string todofukenCd, string kumiaitoCd, ref string message)
        {
            if (string.IsNullOrEmpty(todofukenCd) || string.IsNullOrEmpty(kumiaitoCd))
            {
                message = SystemMessageUtil.Get("ME80006", "組合等コード", "マスタ設定に含まれるコード");
                return false;
            }

            var mKumiaito = db.MKumiaitos.Where(m => m.TodofukenCd == todofukenCd && m.KumiaitoCd == kumiaitoCd).SingleOrDefault();
            if (mKumiaito == null)
            {
                message = SystemMessageUtil.Get("ME80006", "組合等コード", "マスタ設定に含まれるコード");
                return false;
            }

            return true;
        }

        /// <summary>
        /// マスタ整合性チェック
        /// 名称M支所に指定された値が存在しない場合、
        /// 出力パラメータのエラーメッセージにエラーメッセージ（メッセージID：ME80006）を設定し、falseを返却する。
        /// 上記以外の場合、trueを返却する。
        /// </summary>
        /// <param name="db">システム共通スキーマに接続するDBコンテキスト</param>
        /// <param name="todofukenCd">都道府県コード</param>
        /// <param name="kumiaitoCd">組合等コード</param>
        /// <param name="shishoCd">支所コード</param>
        /// <param name="message">エラーメッセージ（出力パラメータ）</param>
        /// <returns>チェック結果</returns>
        public static bool ValidateShisho(SystemCommonContext db, string todofukenCd, string kumiaitoCd, string shishoCd, ref string message)
        {
            if (string.IsNullOrEmpty(todofukenCd) || string.IsNullOrEmpty(kumiaitoCd) || string.IsNullOrEmpty(shishoCd))
            {
                message = SystemMessageUtil.Get("ME80006", "支所コード", "マスタ設定に含まれるコード");
                return false;
            }

            var mShishoNm = db.MShishoNms.Where(m => m.TodofukenCd == todofukenCd &&
                                                m.KumiaitoCd == kumiaitoCd &&
                                                m.ShishoCd == shishoCd).SingleOrDefault();
            if (mShishoNm == null)
            {
                message = SystemMessageUtil.Get("ME80006", "支所コード", "マスタ設定に含まれるコード");
                return false;
            }

            return true;
        }

        /// <summary>
        /// 指定されたタイプ、プロパティのStringLength属性のMaximumLength値を取得する
        /// StringLength属性が設定されていないプロパティの場合、-1を返却する
        /// </summary>
        /// <param name="t">対処クラスのType</param>
        /// <param name="name">プロパティ名</param>
        /// <returns>プロパティのMaximumLength値、StringLength属性が設定されていない場合、-1</returns>
        public static int GetMaximumLength(Type t, string name)
        {
            return ((StringLengthAttribute)Attribute.GetCustomAttribute(t.GetProperty(name), typeof(StringLengthAttribute)))?.MaximumLength ?? -1;
        }

        /// <summary>
        /// 指定されたタイプ、プロパティのDisplayName属性のMDisplayName値を取得する
        /// DisplayName属性が設定されていないプロパティの場合、nullを返却する
        /// </summary>
        /// <param name="t">対処クラスのType</param>
        /// <param name="name">プロパティ名</param>
        /// <returns>プロパティのDisplayName値、DisplayName属性が設定されていない場合、null</returns>
        public static string GetDisplayName(Type t, string name)
        {
            return ((DisplayNameAttribute)Attribute.GetCustomAttribute(t.GetProperty(name), typeof(DisplayNameAttribute)))?.DisplayName;
        }

        /// <summary>
        /// フラグの文字列表現を取得する。
        /// </summary>
        /// <param name="flag">フラグ</param>
        /// <returns>true："1"、false："0"</returns>
        public static string GetFlagStr(bool flag)
        {
            return flag ? CoreConst.FLG_ON : CoreConst.FLG_OFF;
        }

        /// <summary>
        /// システム共通スキーマの汎用区分マスタから区分名称を取得する。
        /// </summary>
        /// <param name="kbnSbt">区分種別</param>
        /// <param name="kbnCd">区分コード</param>
        /// <returns>区分名称</returns>
        public static string GetHanyokubunKbnNm(string kbnSbt, string kbnCd)
        {
            using (var db = new SystemCommonContext())
            {
                var mHanyokubun = db.MHanyokubuns.Where(m => m.KbnSbt == kbnSbt && m.KbnCd == kbnCd).SingleOrDefault();
                if (mHanyokubun != null)
                {
                    return mHanyokubun.KbnNm;
                }
                return null;
            }
        }

        /// <summary>
        /// 引数で指定された文字列がnullの場合、空文字列を返す
        /// null以外の場合は、引数で指定された文字列を返す
        /// </summary>
        /// <param name="str">検査対象文字列</param>
        /// <returns></returns>
        public static string GetString(string str)
        {
            if (str == null)
            {
                return string.Empty;
            }
            return str;
        }
    }
}
