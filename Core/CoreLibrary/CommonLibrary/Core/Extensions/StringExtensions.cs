using CoreLibrary.Core.Utility;

namespace CoreLibrary.Core.Extensions
{
    /// <summary>
    /// String拡張メソッド用クラス
    /// </summary>
    /// <remarks>
    /// 作成日：2017/11/07
    /// 作成者：Hiratake Minori
    /// </remarks>
    public static class StringExtensions
    {
        /// <summary>
        /// 対象の文字列が数字のみかどうかを検査
        /// </summary>
        /// <param name="value">検査する文字列</param>
        /// <returns>検査結果(true / false)</returns>
        public static bool IsNumeric(this string value)
        {
            return StringUtil.IsNumeric(value);
        }

        /// <summary>
        /// 対象の文字列が半角英数のみかどうかを検査
        /// </summary>
        /// <param name="value"></param>
        /// <returns>検査結果(true / false)</returns>
        public static bool IsHalfWidthAlphaNum(this string value)
        {
            return StringUtil.IsHalfWidthAlphaNum(value);
        }

        /// <summary>
        /// 対象の文字列が半角英数記号のみかどうかを検査
        /// </summary>
        /// <param name="value">検査する文字列</param>
        /// <returns>検査結果(true / false)</returns>
        public static bool IsHalfWidthAlphaNumSymbol(this string value)
        {
            return StringUtil.IsHalfWidthAlphaNumSymbol(value);
        }

        /// <summary>
        /// 対象の文字列がひらがなのみかどうかを検査
        /// </summary>
        /// <param name="value">検査する文字列</param>
        /// <returns>検査結果(true / false)</returns>
        public static bool IsHiragana(this string value)
        {
            return StringUtil.IsHiragana(value);
        }

        /// <summary>
        /// 対象の文字列が全角カタカナのみかどうかを検査
        /// </summary>
        /// <param name="value">検査する文字列</param>
        /// <returns>検査結果(true / false)</returns>
        public static bool IsFullWidthKatakana(this string value)
        {
            return StringUtil.IsFullWidthKatakana(value);
        }

        /// <summary>
        /// 対象の文字列が半角カタカナのみかどうかを検査
        /// </summary>
        /// <param name="value">検査する文字列</param>
        /// <returns>検査結果(true / false)</returns>
        public static bool IsHalfWidthKatakana(this string value)
        {
            return StringUtil.IsHalfWidthKatakana(value);
        }

        /// <summary>
        /// 対象の文字列から全角カタカナを半角カタカナに変換
        /// </summary>
        /// <param name="value">変換する文字列</param>
        /// <returns>変換結果</returns>
        public static string ToHalfWidthKatakana(this string value)
        {
            return StringUtil.ToHalfWidthKatakana(value);
        }

        /// <summary>
        /// 対象の文字列がnullの場合、空文字列を返す
        /// null以外の場合は、引数で指定された文字列を返す
        /// </summary>
        /// <param name="value">検査する文字列</param>
        /// <returns>nullの場合：空文字</returns>
        public static string GetEmptyIfNull(this string value)
        {
            return StringUtil.GetEmptyIfNull(value);
        }
    }
}