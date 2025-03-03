using System.Text.RegularExpressions;

namespace CoreLibrary.Core.Utility
{
    /// <summary>
    /// 数値ユーティリティクラス
    /// </summary>
    /// <remarks>
    /// 作成日：2018/01/24
    /// 作成者：Rou I
    /// </remarks>
    public class NumberUtil
    {
        /// <summary>
        /// 数値型(符号無/小数無)の妥当性チェックメソッド。
        /// </summary>
        /// <param name="value">検証対象</param>
        /// <param name="intMaxLength">整数部の最大桁数</param>
        /// <returns>検証結果（true：成功 / false：失敗）</returns>
        public static bool IsNumberPositive(object value, int intMaxLength)
        {
            if ((value == null) || (string.IsNullOrEmpty(value.ToString())))
            {
                return true;
            }

            bool isSuccess = long.TryParse(value.ToString(), out long number);
            if (!isSuccess)
            {
                return false;
            }

            // 0の場合、OKとする
            if (number == 0)
            {
                return true;
            }
            else if (number < 0)
            {
                return false;
            }

            // 入力値を文字列に変換
            var numToString = value.ToString();

            // 桁数チェック
            if (intMaxLength < numToString.Length)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 数値型(符号無/小数有)の妥当性チェックメソッド。
        /// </summary>
        /// <param name="value">検証対象</param>
        /// <param name="intMaxLength">整数部の最大桁数</param>
        /// <param name="decMaxLength">>小数部の最大桁数</param>
        /// <returns>検証結果（true：成功 / false：失敗）</returns>
        public static bool IsNumberDec(object value, int intMaxLength, int decMaxLength)
        {
            if ((value == null) || (string.IsNullOrEmpty(value.ToString())))
            {
                return true;
            }

            // decimalに変換
            var isSuccess = decimal.TryParse(value.ToString(), out decimal number);
            if (!isSuccess)
            {
                return false;
            }

            // 0の場合、OKとする
            if (number == 0)
            {
                return true;
            }

            // decimal型の指数表記を回避するための実装
            var numToString = Regex.Replace(number.ToString("N13"), "([.]0+|0+)$", "");
            numToString = Regex.Replace(numToString, ",", "");

            // 符号がある場合、エラーとする
            if (numToString.Contains("-"))
            {
                return false;
            }

            // 小数がある場合
            if (numToString.Contains("."))
            {
                // 整数部、小数部取得
                var intDecValue = numToString.Split('.');

                // 整数部の桁数チェック
                if (intMaxLength < intDecValue[0].Length)
                {
                    return false;
                }

                // 小数部の桁数チェック
                if (decMaxLength < intDecValue[1].Length)
                {
                    return false;
                }
            }
            else
            {
                // 整数部の桁数チェック
                if (intMaxLength < numToString.Length)
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// 数値型(符号有/小数無)の妥当性チェックメソッド。
        /// </summary>
        /// <param name="value">検証対象</param>
        /// <param name="intMaxLength">整数部の最大桁数</param>
        /// <returns>検証結果（true：成功 / false：失敗）</returns>
        public static bool IsNumberSign(object value, int intMaxLength)
        {
            if ((value == null) || (string.IsNullOrEmpty(value.ToString())))
            {
                return true;
            }

            bool isSuccess = long.TryParse(value.ToString(), out long number);
            if (!isSuccess)
            {
                return false;
            }

            // 0の場合、OKとする
            if (number == 0)
            {
                return true;
            }

            // 入力値を文字列に変換
            var numToString = value.ToString();

            // 符号がある場合、符号を削除する
            if (numToString.Contains("-"))
            {
                numToString = numToString.Replace("-", string.Empty);
            }

            // 整数の桁数チェック
            if (intMaxLength < numToString.Length)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 数値型(符号有/小数有)の妥当性チェックメソッド。
        /// </summary>
        /// <param name="value">検証対象</param>
        /// <param name="intMaxLength">整数部の最大桁数</param>
        /// <param name="decMaxLength">小数部の最大桁数</param>
        /// <returns>検証結果（true：成功 / false：失敗）</returns>
        public static bool IsNumberSignDec(object value, int intMaxLength, int decMaxLength)
        {
            if ((value == null) || (string.IsNullOrEmpty(value.ToString())))
            {
                return true;
            }

            // decimalに変換
            var isSuccess = decimal.TryParse(value.ToString(), out decimal number);
            if (!isSuccess)
            {
                return false;
            }

            // 0の場合、OKとする
            if (number == 0)
            {
                return true;
            }

            // decimal型の指数表記を回避するための実装
            var numToString = Regex.Replace(number.ToString("N13"), "([.]0+|0+)$", "");
            numToString = Regex.Replace(numToString, ",", "");

            // 符号がある場合、符号を削除する
            if (numToString.Contains("-"))
            {
                numToString = numToString.Replace("-", string.Empty);
            }

            // 小数がある場合
            if (numToString.Contains("."))
            {
                // 整数部、小数部取得
                var intDecValue = numToString.Split('.');

                // 整数部の桁数チェック
                if (intMaxLength < intDecValue[0].Length)
                {
                    return false;
                }

                // 小数部の桁数チェック
                if (decMaxLength < intDecValue[1].Length)
                {
                    return false;
                }
            }
            else
            {
                // 整数部の桁数チェック
                if (intMaxLength < numToString.Length)
                {
                    return false;
                }
            }

            return true;
        }
    }
}