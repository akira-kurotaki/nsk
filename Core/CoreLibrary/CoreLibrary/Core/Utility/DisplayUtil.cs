namespace CoreLibrary.Core.Utility
{
    /// <summary>
    /// 帳票項目表示のユーティリティクラス
    /// 
    /// 作成日：2018/04/19
    /// 作成者：ROU I
    /// </summary>
    public static class DisplayUtil
    {
        /// <summary>
        /// フォーマット区分
        /// </summary>
        public enum FormatKbn
        {
            COMMA_SEPARATED_DIGITS_ONE,                 // カンマ区切り　ZZZ,ZZZ,ZZZ,ZZZ,ZZ9、----,---,---,---,--9
            COMMA_SEPARATED_DIGITS_ONE_PLUS,            // カンマ区切り　+ZZ,ZZZ,ZZZ,ZZZ,ZZ9、++++,+++,+++,+++,++9
            COMMA_SEPARATED_DIGITS_BLANK,               // カンマ区切り　ZZZ,ZZZ,ZZZ,ZZZ,ZZZ、----,---,---,---,---
            DIGITS_ONE,                                 // 桁 プレースホルダ ZZZZZZZZZZZ9、------------9
            DIGITS_BLANK,                               // 桁 プレースホルダ ZZZZZZZZZZZZ、-------------
            ZERO_PLACEHOLDERS                           // ゼロ プレースホルダ 999999999999、-999999999999
        }

        /// <summary>
        /// フォーマット区分(小数点付き)
        /// </summary>
        public enum DecimalFormatKbn
        {
            COMMA_SEPARATED_DECIMAL_DIGITS_ONE,         // カンマ区切り小数付き　ZZZ,ZZZ,ZZZ,ZZZ,ZZ9.999、----,---,---,---,--9.999
            COMMA_SEPARATED_DECIMAL_DIGITS_BLANK        // カンマ区切り小数付き　ZZZ,ZZZ,ZZZ,ZZZ,ZZZ.ZZZ、----,---,---,---,--Z.ZZZ
        }

        /// <summary>
        /// 対象を指定した形式に変換
        /// </summary>
        /// <param name="value">変換する対象</param>
        /// <param name="intDig">整数部有効桁数</param>
        /// <param name="format">フォーマット区分</param>
        /// <returns>変換結果</returns>
        public static object Format(object value, int intDig, FormatKbn kbn)
        {
            if (value == null)
            {
                return string.Empty;
            }

            if (intDig <= 0)
            {
                return value;
            }

            Decimal tmpDecimal = new Decimal();
            switch (kbn)
            {
                // カンマ区切り　ZZZ,ZZZ,ZZZ,ZZZ,ZZ9、----,---,---,---,--9
                case FormatKbn.COMMA_SEPARATED_DIGITS_ONE:
                    if (Decimal.TryParse(value.ToString(), out tmpDecimal))
                    {
                        if (tmpDecimal == Decimal.Zero)
                        {
                            return Decimal.Zero;
                        }
                        return string.Format("{0:#,0}", GetEnabledDigits(tmpDecimal, intDig));
                    }
                    return value;
                // カンマ区切り　+ZZ,ZZZ,ZZZ,ZZZ,ZZ9、++++,+++,+++,+++,++9
                case FormatKbn.COMMA_SEPARATED_DIGITS_ONE_PLUS:
                    if (Decimal.TryParse(value.ToString(), out tmpDecimal))
                    {
                        if (tmpDecimal == Decimal.Zero)
                        {
                            return Decimal.Zero;
                        }
                        return string.Format(tmpDecimal > Decimal.Zero ? "+{0:#,0}" : "{0:#,0}", GetEnabledDigits(tmpDecimal, intDig));
                    }
                    return value;
                // カンマ区切り　ZZZ,ZZZ,ZZZ,ZZZ,ZZZ、----,---,---,---,---
                case FormatKbn.COMMA_SEPARATED_DIGITS_BLANK:
                    if (Decimal.TryParse(value.ToString(), out tmpDecimal))
                    {
                        if (tmpDecimal == Decimal.Zero)
                        {
                            return string.Empty;
                        }
                        return string.Format("{0:#,#}", GetEnabledDigits(tmpDecimal, intDig));
                    }
                    return value;
                // 桁 プレースホルダ ZZZZZZZZZZZ9、------------9
                case FormatKbn.DIGITS_ONE:
                    if (Decimal.TryParse(value.ToString(), out tmpDecimal))
                    {
                        if (tmpDecimal == Decimal.Zero)
                        {
                            return Decimal.Zero;
                        }
                        return string.Format("{0:" + GetSharpStr(intDig - 1) + "0}", GetEnabledDigits(tmpDecimal, intDig));
                    }
                    return value;
                // 桁 プレースホルダ ZZZZZZZZZZZZ、-------------
                case FormatKbn.DIGITS_BLANK:
                    if (Decimal.TryParse(value.ToString(), out tmpDecimal))
                    {
                        if (tmpDecimal == Decimal.Zero)
                        {
                            return string.Empty;
                        }
                        return string.Format("{0:" + GetSharpStr(intDig) + "}", GetEnabledDigits(tmpDecimal, intDig));
                    }
                    return value;
                // ゼロ プレースホルダ 999999999999、-999999999999
                case FormatKbn.ZERO_PLACEHOLDERS:
                    if (Decimal.TryParse(value.ToString(), out tmpDecimal))
                    {
                        if (tmpDecimal == Decimal.Zero)
                        {
                            return string.Format("{0:" + GetZeroStr(intDig) + "}", Decimal.Zero);
                        }
                        return string.Format("{0:" + GetZeroStr(intDig) + "}", GetEnabledDigits(tmpDecimal, intDig));
                    }
                    return value;
                default:
                    return value;
            }
        }

        /// <summary>
        /// 対象を指定した形式に変換
        /// </summary>
        /// <param name="value">変換する対象</param>
        /// <param name="intDig">整数部有効桁数</param>
        /// <param name="format">フォーマット区分</param>
        /// <param name="unit">単位</param>
        /// <returns>変換結果</returns>
        public static object Format(object value, int intDig, FormatKbn kbn, string unit)
        {
            object tmp = Format(value, intDig, kbn);
            if (tmp == null)
            {
                return string.Empty;
            }
            else
            {
                string str = tmp.ToString();
                if (string.IsNullOrEmpty(str) || string.IsNullOrWhiteSpace(str))
                {
                    return string.Empty;
                }
                else
                {
                    return string.Concat(tmp, unit);
                }
            }
        }

        /// <summary>
        /// 対象を指定した形式に変換(小数点付き)
        /// </summary>
        /// <param name="value">変換する対象</param>
        /// <param name="intDig">整数部有効桁数</param>
        /// <param name="fracDig">小数部桁数</param>
        /// <param name="format">フォーマット区分</param>
        /// <returns>変換結果</returns>
        public static object FormatDecimal(object value, int intDig, int fracDig, DecimalFormatKbn kbn)
        {
            if (value == null)
            {
                return string.Empty;
            }

            if (intDig <= 0 || fracDig <= 0)
            {
                return value;
            }

            Decimal tmpDecimal = new Decimal();
            switch (kbn)
            {
                // カンマ区切り小数付き　ZZZ,ZZZ,ZZZ,ZZZ,ZZ9.999、----,---,---,---,--9.999
                case DecimalFormatKbn.COMMA_SEPARATED_DECIMAL_DIGITS_ONE:
                    if (Decimal.TryParse(value.ToString(), out tmpDecimal))
                    {
                        if (tmpDecimal == Decimal.Zero)
                        {
                            return string.Format("{0:0,." + GetZeroStr(fracDig) + "}", tmpDecimal);
                        }
                        string tmp = string.Format("{0:#,0." + GetZeroStr(fracDig + 1) + "}", GetEnabledDigits(tmpDecimal, intDig, fracDig));
                        return tmp.Substring(0, tmp.Length - 1);
                    }
                    return value;
                // カンマ区切り小数付き　ZZZ,ZZZ,ZZZ,ZZZ,ZZZ.ZZZ、----,---,---,---,--Z.ZZZ
                case DecimalFormatKbn.COMMA_SEPARATED_DECIMAL_DIGITS_BLANK:
                    if (Decimal.TryParse(value.ToString(), out tmpDecimal))
                    {
                        if (tmpDecimal == Decimal.Zero)
                        {
                            return string.Empty;
                        }
                        return string.Format("{0:#,0." + GetSharpStr(fracDig) + "}", GetEnabledDigits(tmpDecimal, intDig, fracDig));
                    }
                    return value;
                default:
                    return value;
            }
        }

        /// <summary>
        /// 対象を指定した形式に変換(小数点付き)
        /// </summary>
        /// <param name="value">変換する対象</param>
        /// <param name="intDig">整数部有効桁数</param>
        /// <param name="fracDig">小数部桁数</param>
        /// <param name="format">フォーマット区分</param>
        /// <param name="unit">単位</param>
        /// <returns>変換結果</returns>
        public static object FormatDecimal(object value, int intDig, int fracDig, DecimalFormatKbn kbn, string unit)
        {
            object tmp = FormatDecimal(value, intDig, fracDig, kbn);
            if (tmp == null)
            {
                return string.Empty;
            }
            else
            {
                string str = tmp.ToString();
                if (string.IsNullOrEmpty(str) || string.IsNullOrWhiteSpace(str))
                {
                    return string.Empty;
                }
                else
                {
                    return string.Concat(tmp, unit);
                }
            }
        }

        /// <summary>
        /// 整数部有効桁数のデータを取得
        /// </summary>
        /// <param name="value">変換する対象</param>
        /// <param name="intDig">整数部有効桁数</param>
        /// <returns>変換結果</returns>
        private static Decimal GetEnabledDigits(Decimal value, int intDig)
        {
            Decimal tmpDecimal = new Decimal();
            if (value < 0)
            {
                string tmp = value.ToString().Substring(1);
                string[] tmps = tmp.Split('.');
                if (tmps.Length == 2)
                {
                    string intDigStr = tmps[0];
                    string fracDigStr = tmps[1];

                    if (intDigStr.Length >= intDig)
                    {
                        tmp = intDigStr.Substring(intDigStr.Length - intDig);
                        if (Decimal.TryParse(tmp, out tmpDecimal))
                        {
                            return (0 - tmpDecimal);
                        }
                    }
                } 
                else
                {
                    if (tmp.Length >= intDig)
                    {
                        tmp = tmp.Substring(tmp.Length - intDig);
                        if (Decimal.TryParse(tmp, out tmpDecimal))
                        {
                            return (0 - tmpDecimal);
                        }
                    }
                }
            }
            else
            {
                string tmp = value.ToString();
                string[] tmps = tmp.Split('.');
                if (tmps.Length == 2)
                {
                    string intDigStr = tmps[0];
                    string fracDigStr = tmps[1];

                    if (intDigStr.Length >= intDig)
                    {
                        tmp = intDigStr.Substring(intDigStr.Length - intDig);
                        if (Decimal.TryParse(tmp, out tmpDecimal))
                        {
                            return tmpDecimal;
                        }
                    }
                }
                else
                {
                    if (tmp.Length >= intDig)
                    {
                        tmp = tmp.Substring(tmp.Length - intDig);
                        if (Decimal.TryParse(tmp, out tmpDecimal))
                        {
                            return tmpDecimal;
                        }
                    }
                }
            }
            return value;
        }

        /// <summary>
        /// 整数部&小数部有効桁数のデータを取得
        /// </summary>
        /// <param name="value">変換する対象</param>
        /// <param name="intDig">整数部有効桁数</param>
        /// <param name="fracDig">小数部桁数</param>
        /// <returns>変換結果</returns>
        private static Decimal GetEnabledDigits(Decimal value, int intDig, int fracDig)
        {
            Decimal tmpDecimal = new Decimal();
            if (value < 0)
            {
                string tmp = value.ToString().Substring(1);
                string[] tmps = tmp.Split('.');
                if (tmps.Length == 2)
                {
                    string intDigStr = tmps[0];
                    string fracDigStr = tmps[1];

                    if (intDigStr.Length >= intDig && fracDigStr.Length < fracDig)
                    {
                        tmp = intDigStr.Substring(intDigStr.Length - intDig) + "." + fracDigStr;
                        if (Decimal.TryParse(tmp, out tmpDecimal))
                        {
                            return (0 - tmpDecimal);
                        }
                    }
                    else if (intDigStr.Length < intDig && fracDigStr.Length >= fracDig)
                    {
                        if (fracDigStr.Length == fracDig)
                        {
                            tmp = intDigStr + "." + fracDigStr;
                        }
                        else
                        {
                            tmp = intDigStr + "." + fracDigStr.Substring(0, fracDig);
                        }
                        if (Decimal.TryParse(tmp, out tmpDecimal))
                        {
                            return (0 - tmpDecimal);
                        }
                    }
                    else if (intDigStr.Length >= intDig && fracDigStr.Length >= fracDig)
                    {
                        if (fracDigStr.Length == fracDig)
                        {
                            tmp = intDigStr.Substring(intDigStr.Length - intDig) + "." + fracDigStr;
                        }
                        else
                        {
                            tmp = intDigStr.Substring(intDigStr.Length - intDig) + "." + fracDigStr.Substring(0, fracDig);
                        }
                        if (Decimal.TryParse(tmp, out tmpDecimal))
                        {
                            return (0 - tmpDecimal);
                        }
                    }
                }
            }
            else
            {
                string tmp = value.ToString();
                string[] tmps = tmp.Split('.');
                if (tmps.Length == 2)
                {
                    string intDigStr = tmps[0];
                    string fracDigStr = tmps[1];

                    if (intDigStr.Length >= intDig && fracDigStr.Length < fracDig)
                    {
                        tmp = intDigStr.Substring(intDigStr.Length - intDig) + "." + fracDigStr;
                        if (Decimal.TryParse(tmp, out tmpDecimal))
                        {
                            return tmpDecimal;
                        }
                    }
                    else if (intDigStr.Length < intDig && fracDigStr.Length >= fracDig)
                    {
                        if (fracDigStr.Length == fracDig)
                        {
                            tmp = intDigStr + "." + fracDigStr;
                        }
                        else
                        {
                            tmp = intDigStr + "." + fracDigStr.Substring(0, fracDig);
                        }
                        if (Decimal.TryParse(tmp, out tmpDecimal))
                        {
                            return tmpDecimal;
                        }
                    }
                    else if (intDigStr.Length >= intDig && fracDigStr.Length >= fracDig)
                    {
                        if (fracDigStr.Length == fracDig)
                        {
                            tmp = intDigStr.Substring(intDigStr.Length - intDig) + "." + fracDigStr;
                        }
                        else
                        {
                            tmp = intDigStr.Substring(intDigStr.Length - intDig) + "." + fracDigStr.Substring(0, fracDig);
                        }
                        if (Decimal.TryParse(tmp, out tmpDecimal))
                        {
                            return tmpDecimal;
                        }
                    }
                }
            }
            return value;
        }

        /// <summary>
        /// 文字列str1とstr2を連結した文字列を戻す
        /// </summary>
        /// <param name="str1">文字列1</param>
        /// <param name="str2">文字列2</param>
        /// <returns>連結後結果</returns>
        public static string Concat(string str1, string str2)
        {
            if (string.IsNullOrEmpty(str1))
            {
                return string.Empty;
            }
            else
            {
                return string.Concat(str1, str2);
            }
        }

        /// <summary>
        /// 対象桁数のゼロを取得
        /// </summary>
        /// <param name="num">小数点桁数</param>
        /// <returns>変換結果</returns>
        private static string GetZeroStr(int num)
        {
            return string.Empty.PadLeft(num, '0');
        }

        /// <summary>
        /// 対象桁数のシャープを取得
        /// </summary>
        /// <param name="num">小数点桁数</param>
        /// <returns>変換結果</returns>
        private static string GetSharpStr(int num)
        {
            return string.Empty.PadLeft(num, '#');
        }
    }
}
