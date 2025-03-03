using CoreLibrary.Core.Consts;
using ModelLibrary.Models;
using NLog;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace CoreLibrary.Core.Utility
{
    /// <summary>
    /// 日付編集のユーティリティクラス
    /// </summary>
    /// <remarks>
    /// 作成日：2017/12/12
    /// 作成者：Rou I
    /// </remarks>
    public class DateUtil
    {
        /// <summary>
        /// システム時間(年月日)
        /// </summary>
        public static DateTime? SystemDate { private get; set; }

        /// <summary>
        /// システム時間取得メソッド。
        /// </summary>
        /// <returns>システム時間</returns>
        public static DateTime GetSysDateTime()
        {
            var retDateTime = DateTime.Now;
            if (bool.Parse(ConfigUtil.Get(CoreConst.SYS_DATE_TIME_FLAG)))
            {
                if (!SystemDate.HasValue)
                {
                    var filePath = ConfigUtil.Get(CoreConst.SYS_DATE_TIME_PATH);
                    if (File.Exists(filePath))
                    {
                        using (var file = new StreamReader(filePath))
                        {
                            SystemDate = DateTime.Parse(file.ReadLine());
                        }
                    }
                    else
                    {
                        Logger logger = LogManager.GetCurrentClassLogger();
                        // {0}が見つからなかったため、マシン時間をシステム時間へ設定します。
                        logger.Error(SystemMessageUtil.Get("MW00004", filePath));
                        SystemDate = retDateTime;
                    }
                }
                retDateTime = new DateTime(
                    SystemDate.Value.Year, SystemDate.Value.Month, SystemDate.Value.Day,
                    retDateTime.Hour, retDateTime.Minute, retDateTime.Second, retDateTime.Millisecond);
            }
            return retDateTime;
        }

        /// <summary>
        /// 和暦を西暦に変換メソッド。
        /// 
        /// 改元に伴い元年にあたる年は月日があっていなくてもＯＫとする。
        /// 昭和はS64/01/07まで、平成はH01/01/08からであるが、下記の変換を行う。
        /// S64/01/08 → 1989/01/08
        /// H01/01/07 → 1989/01/07
        /// </summary>
        /// <param name="dateStr">和暦日付</param>
        /// <returns>西暦日付(DateTime型)</returns>
        public static DateTime? GetGregorianDateTime(string dateStr)
        {
            if (string.IsNullOrEmpty(dateStr))
            {
                return null;
            }

            List<MGengo> mGengos = GengoUtil.GetGengoList() as List<MGengo>;
            if (mGengos.Count == 0)
            {
                return null;
            }

            JapaneseDate japaneseDate = GetJapaneseDateObject(dateStr);

            if (japaneseDate == null)
            {
                return null;
            }

            MGengo mGengo = mGengos.Find(x => x.RyakugoEn.Contains(japaneseDate.Gengo));
            if (mGengo == null || mGengo.TekiyoStartYmd == null)
            {
                return null;
            }

            // 改元の元年にあたる年は月日があっていなくてもOKとする
            int? year = GetGregorianYear(japaneseDate.Gengo + String.Format("{0:D2}", japaneseDate.Year));
            if (year == null)
            {
                return null;
            }

            // 実在日かをチェックする
            StringBuilder sb = new StringBuilder();
            sb.Append(year.ToString().PadLeft(4, '0'));
            sb.Append(japaneseDate.Month.ToString().PadLeft(2, '0'));
            sb.Append(japaneseDate.Day.ToString().PadLeft(2, '0'));

            DateTime parsedDate;
            if (!DateTime.TryParseExact(sb.ToString(), "yyyyMMdd", null, DateTimeStyles.None, out parsedDate))
            {
                return null;
            }
            else
            {
                return parsedDate;
            }
        }

        /// <summary>
        /// 和暦を西暦に変換メソッド(yyyy/MM/dd)。
        /// </summary>
        /// <param name="dateStr">和暦日付</param>
        /// <returns>西暦日付</returns>
        public static string GetGregorianDate(string dateStr)
        {
            return GetGregorianDate(dateStr, "yyyy/MM/dd");
        }

        /// <summary>
        /// 和暦を西暦に変換メソッド。
        /// </summary>
        /// <param name="dateStr">和暦日付</param>
        /// <param name="format">日付変換フォーマット</param>
        /// <returns>西暦日付</returns>
        public static string GetGregorianDate(string dateStr, string format)
        {
            if (string.IsNullOrEmpty(dateStr))
            {
                return string.Empty;
            }

            var parsedDate = GetGregorianDateTime(dateStr);

            if (parsedDate == null)
            {
                return string.Empty;
            }
            else
            {
                return ((DateTime)parsedDate).ToString(format);
            }
        }

        /// <summary>
        /// 和暦として認識できるかを試みる
        /// </summary>
        /// <param name="dateStr"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public static bool TryParseJapaneseDate(string dateStr, out DateTime result)
        {
            result = DateTime.MinValue;
            if (string.IsNullOrEmpty(dateStr))
            {
                return false;
            }

            DateTime? datetime = GetGregorianDateTime(dateStr);
            if (datetime != null)
            {
                result = datetime.Value;
                return true;
            }
            return false;
        }

        /// <summary>
        /// 和暦(年)を西暦(年)に変換メソッド。
        /// </summary>
        /// <param name="nendo">和暦(年)</param>
        /// <returns>西暦(年)</returns>
        public static int? GetGregorianYear(string nendo)
        {
            int? retValue = null;
            if (3 == nendo.Length)
            {
                var gengoStr = nendo.Substring(0, 1);
                var nensuStr = nendo.Substring(1, 2);

                List<MGengo> mGengos = GengoUtil.GetGengoList() as List<MGengo>;
                MGengo mGengo = mGengos.Find(x => gengoStr.ToUpper().Equals(x.RyakugoEn));

                if ((mGengo != null && mGengo.TekiyoStartYmd != null) && (int.TryParse(nensuStr, out int nensu)))
                {
                    if (0 < nensu)
                    {
                        retValue = mGengo.TekiyoStartYmd.Value.Year - 1 + nensu;

                        if (mGengo.TekiyoEndYmd != null)
                        {
                            return retValue <= mGengo.TekiyoEndYmd.Value.Year ? retValue : null;
                        }
                    }
                }
            }
            return retValue;
        }

        /// <summary>
        /// 和暦チェックメソッド。
        /// </summary>
        /// <param name="dateStr">和暦日付</param>
        /// <returns>検査結果(true / false)</returns>
        public static bool IsJapaneseDate(string dateStr)
        {
            if (string.IsNullOrEmpty(dateStr))
            {
                return true;
            }

            DateTime? parsedDate = GetGregorianDateTime(dateStr);
            if (parsedDate == null)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 和暦日付型取得メソッド。
        /// </summary>
        /// <param name="dateStr">西暦日付</param>
        /// <returns>和暦型</returns>
        public static string GetJapaneseDate(DateTime? date)
        {
            if (date == null)
            {
                return string.Empty;
            }

            List<MGengo> mGengos = GengoUtil.GetGengoList() as List<MGengo>;
            if (mGengos.Count == 0)
            {
                return string.Empty;
            }

            DateTime tmpDate = new DateTime(date.Value.Year, date.Value.Month, date.Value.Day, 0, 0, 0);
            MGengo mGengo = mGengos.Find(x => (x.TekiyoStartYmd <= tmpDate && x.TekiyoEndYmd >= tmpDate) ||
                                              (x.TekiyoStartYmd <= tmpDate && x.TekiyoEndYmd == null));
            if (mGengo == null)
            {
                return string.Empty;
            }

            DateTime tekiyoStartYmd = (DateTime)mGengo.TekiyoStartYmd;

            StringBuilder sb = new StringBuilder();
            sb.Append(mGengo.RyakugoEn);
            sb.Append((tmpDate.Year - tekiyoStartYmd.Year + 1).ToString().PadLeft(2, '0'));
            sb.Append("/");
            sb.Append(tmpDate.Month.ToString().PadLeft(2, '0'));
            sb.Append("/");
            sb.Append(tmpDate.Day.ToString().PadLeft(2, '0'));

            return sb.ToString();
        }

        /// <summary>
        /// 帳票用の和暦(年)取得メソッド。
        /// </summary>
        /// <param name="year">西暦年(YYYY形式)</param>
        /// <param name="keieiKeitaiCd">経営形態(0:未選択, 1:個人, 2:法人)</param>
        /// <param name="jigyonendo">日付データ</param>
        /// <returns>
        /// 和暦年(␣Y形式)
        /// ・経営形態が0:未選択の場合は空文字を返却する。
        /// ・西暦年と事業年度開始日が不正な場合は空文字を返却する。
        /// </returns>
        public static string GetReportJapaneseYY(int? year, string keieiKeitaiCd, DateTime? jigyonendo)
        {
            var retValue = string.Empty;

            if ("0".Equals(keieiKeitaiCd))
            {
                keieiKeitaiCd = "1";
            }

            if ("1".Equals(keieiKeitaiCd) || "2".Equals(keieiKeitaiCd))
            {
                // 経営形態が1:個人の場合、01月01日固定
                if ("1".Equals(keieiKeitaiCd))
                {
                    if (DateTime.TryParseExact(
                                   year.ToString() + "0101", "yyyyMMdd", null, DateTimeStyles.None, out DateTime date))
                    {
                        retValue = GetReportJapaneseYY(date);
                    }
                }
                else
                {
                    if (!jigyonendo.HasValue)
                    {
                        return string.Empty;
                    }

                    var month = jigyonendo.Value.ToString("MM");
                    var day = jigyonendo.Value.ToString("dd");

                    if (DateTime.TryParseExact(
                                   year.ToString() + month + day, "yyyyMMdd", null, DateTimeStyles.None, out DateTime date))
                    {
                        retValue = GetReportJapaneseYY(date);
                    }
                }
            }
            return retValue;
        }

        /// <summary>
        /// 帳票用の和暦(年)取得メソッド。
        /// </summary>
        /// <param name="year">西暦年(YYYY形式)</param>
        /// <param name="keieiKeitaiCd">経営形態(0:未選択, 1:個人, 2:法人)</param>
        /// <param name="jigyonendo">日付データ</param>
        /// <returns>
        /// 和暦年(ＺＺ␣Y形式)
        /// ・経営形態が0:未選択の場合は空文字を返却する。
        /// ・西暦年と事業年度開始日が不正な場合は空文字を返却する。
        /// </returns>
        public static string GetReportJapaneseYear(int? year)
        {
            var retValue = string.Empty;
            if (year == null)
            {
                return retValue;
            }

            if (DateTime.TryParseExact(
                                  year.ToString() + "0101", "yyyyMMdd", null, DateTimeStyles.None, out DateTime date))
            {
                retValue = GetReportJapaneseYear(date);
            }

            return retValue;
        }

        /// <summary>
        /// 帳票用の和暦(年)取得メソッド。
        /// </summary>
        /// <param name="date">日付データ</param>
        /// <returns>
        /// 和暦年(ＺＺ␣Y形式)
        /// </returns>
        public static string GetReportJapaneseYear(DateTime? date)
        {
            var retValue = string.Empty;
            if (date == null)
            {
                return retValue;
            }

            DateTime tmpDate = new DateTime(date.Value.Year, date.Value.Month, date.Value.Day, 0, 0, 0);
            List<MGengo> mGengos = GengoUtil.GetGengoList() as List<MGengo>;
            if (0 < mGengos.Count)
            {
                var mGengo = mGengos.Find(x => (
                    x.TekiyoStartYmd <= tmpDate && tmpDate <= x.TekiyoEndYmd) || (
                    x.TekiyoStartYmd <= tmpDate && null == x.TekiyoEndYmd));
                if (null != mGengo)
                {
                    DateTime startYmd = (DateTime)mGengo.TekiyoStartYmd;
                    var nendo = string.Format("{0, 2}", tmpDate.Year - startYmd.Year + 1);
                    retValue = mGengo.Gengo + (" 1".Equals(nendo) ? "元" : nendo);
                }
            }
            return retValue;
        }

        /// <summary>
        /// 帳票用の和暦(年)取得メソッド。
        /// </summary>
        /// <param name="date">日付データ</param>
        /// <returns>
        /// 和暦年(␣Y形式)
        /// </returns>
        public static string GetReportJapaneseYY(DateTime? date)
        {
            var retValue = string.Empty;
            if (date == null)
            {
                return retValue;
            }

            DateTime tmpDate = new DateTime(date.Value.Year, date.Value.Month, date.Value.Day, 0, 0, 0);
            List<MGengo> mGengos = GengoUtil.GetGengoList() as List<MGengo>;
            if (0 < mGengos.Count)
            {
                var mGengo = mGengos.Find(x => (
                    x.TekiyoStartYmd <= tmpDate && tmpDate <= x.TekiyoEndYmd) || (
                    x.TekiyoStartYmd <= tmpDate && null == x.TekiyoEndYmd));
                if (null != mGengo)
                {
                    DateTime startYmd = (DateTime)mGengo.TekiyoStartYmd;
                    var nendo = string.Format("{0, 2}", tmpDate.Year - startYmd.Year + 1);
                    retValue = (" 1".Equals(nendo) ? "元" : nendo);
                }
            }
            return retValue;
        }

        /// <summary>
        /// 帳票用の和暦日付型(ＮＮZ9年Z9月Z9日形式)取得メソッド。
        /// </summary>
        /// <param name="date">日付データ</param>
        /// <returns>
        /// 和暦日付型(ＮＮZ9年Z9月Z9日形式)
        /// </returns>
        public static string GetReportJapaneseDate(DateTime? date)
        {
            var retValue = string.Empty;
            if (date == null)
            {
                return retValue;
            }

            DateTime tmpDate = new DateTime(date.Value.Year, date.Value.Month, date.Value.Day, 0, 0, 0);
            List<MGengo> mGengos = GengoUtil.GetGengoList() as List<MGengo>;
            if (0 < mGengos.Count)
            {
                var mGengo = mGengos.Find(x => (
                    x.TekiyoStartYmd <= tmpDate && tmpDate <= x.TekiyoEndYmd) || (
                    x.TekiyoStartYmd <= tmpDate && null == x.TekiyoEndYmd));
                if (null != mGengo)
                {
                    DateTime startYmd = (DateTime)mGengo.TekiyoStartYmd;
                    var nendo = string.Format("{0, 2}", tmpDate.Year - startYmd.Year + 1);
                    retValue = mGengo.Gengo + (" 1".Equals(nendo) ? "元" : nendo) + "年" + FormatMonthByDateTime(date) + "月" + FormatDayByDateTime(date) + "日";
                }
            }
            return retValue;
        }

        /// <summary>
        /// 和暦日付型取得メソッド。
        /// </summary>
        /// <param name="dateStr">和暦日付</param>
        /// <returns>和暦型</returns>
        private static JapaneseDate GetJapaneseDateObject(string dateStr)
        {
            var vGengo = "";
            var vYear = "";
            var vMonth = "";
            var vDay = "";
            var value = dateStr.Split('/');

            if (value.Length == 3)
            {
                var vGengoYear = value[0];
                if (vGengoYear.Length != 2 && vGengoYear.Length != 3)
                {
                    return null;
                }
                if (vGengoYear.Length == 2)
                {
                    vGengo = vGengoYear.Substring(0, 1);
                    vYear = vGengoYear.Substring(1, 1);
                }
                else if (vGengoYear.Length == 3)
                {
                    vGengo = vGengoYear.Substring(0, 1);
                    vYear = vGengoYear.Substring(1, 2);
                }

                if (!Regex.IsMatch(vYear, @"^[0-9]+$") || int.Parse(vYear) == 0)
                {
                    return null;
                }
                vMonth = value[1];
                vDay = value[2];
                if (vMonth.Length == 0 || !Regex.IsMatch(vMonth, @"^[0-9]+$") || int.Parse(vMonth) == 0)
                {
                    return null;
                }
                if (vDay.Length == 0 || !Regex.IsMatch(vDay, @"^[0-9]+$") || int.Parse(vDay) == 0)
                {
                    return null;
                }

            }
            else
            {
                if (dateStr.Length != 7)
                {
                    return null;
                }
                else
                {
                    vGengo = dateStr.Substring(0, 1);
                    vYear = dateStr.Substring(1, 2);
                    vMonth = dateStr.Substring(3, 2);
                    vDay = dateStr.Substring(5, 2);

                    if (!Regex.IsMatch(vYear, @"^[0-9]+$") || int.Parse(vYear) == 0)
                    {
                        return null;
                    }
                    if (!Regex.IsMatch(vMonth, @"^[0-9]+$") || int.Parse(vMonth) == 0)
                    {
                        return null;
                    }
                    if (!Regex.IsMatch(vDay, @"^[0-9]+$") || int.Parse(vDay) == 0)
                    {
                        return null;
                    }
                }
            }

            JapaneseDate japaneseDate = new JapaneseDate()
            {
                Gengo = vGengo,
                Year = int.Parse(vYear),
                Month = int.Parse(vMonth),
                Day = int.Parse(vDay)
            };

            return japaneseDate;
        }

        /// <summary>
        /// 帳票の出力年月日を取得メソッド。
        /// </summary>
        /// <returns>yyyy年MM月dd日（月、日の前ゼロはブランク）</returns>
        public static string GetOutputYMD()
        {
            var date = GetSysDateTime();

            var year = date.Year;
            var month = FormatMonthByDateTime(date);
            var day = FormatDayByDateTime(date);

            return year + "年" + month + "月" + day + "日";
        }

        /// <summary>
        /// 帳票の出力時分を取得メソッド。
        /// </summary>
        /// <returns>HH:mm（HHの前ゼロはブランク、mmが0～9の場合、前ゼロ追加）</returns>
        public static string GetOutputHM()
        {
            var date = GetSysDateTime();

            var hour = FormatHourByDateTime(date);
            var minute = FormatMinuteByDateTime(date);

            return hour + ":" + minute;
        }

        /// <summary>
        /// 日付より、月を取得し、フォーマットするメソッド。
        /// </summary>
        /// <param name="date">日付</param>
        /// <returns>フォーマットした月（月の前ゼロはブランク）</returns>
        public static string FormatMonthByDateTime(DateTime? date)
        {
            if (date == null)
            {
                return string.Empty;
            }
            DateTime tmpDate = (DateTime)date;
            return tmpDate.Month.ToString().Length == 1 ? " " + tmpDate.Month.ToString() : tmpDate.Month.ToString();
        }

        /// <summary>
        /// 日付より、日を取得し、フォーマットするメソッド。
        /// </summary>
        /// <param name="date">日付</param>
        /// <returns>フォーマットした日（日の前ゼロはブランク）</returns>
        public static string FormatDayByDateTime(DateTime? date)
        {
            if (date == null)
            {
                return string.Empty;
            }
            DateTime tmpDate = (DateTime)date;
            return tmpDate.Day.ToString().Length == 1 ? " " + tmpDate.Day.ToString() : tmpDate.Day.ToString();
        }

        /// <summary>
        /// 日付より、時を取得し、フォーマットする
        /// </summary>
        /// <param name="date">日付</param>
        /// <returns>フォーマットした時（時の前ゼロはブランク）</returns>
        public static string FormatHourByDateTime(DateTime? date)
        {
            if (date == null)
            {
                return string.Empty;
            }
            DateTime tmpDate = (DateTime)date;
            return tmpDate.Hour.ToString().Length == 1 ? " " + tmpDate.Hour.ToString() : tmpDate.Hour.ToString();
        }

        /// <summary>
        /// 日付より、分を取得し、フォーマットする
        /// </summary>
        /// <param name="date">日付</param>
        /// <returns>フォーマットした分（分が0～9の場合、前ゼロ追加）</returns>
        public static string FormatMinuteByDateTime(DateTime? date)
        {
            if (date == null)
            {
                return string.Empty;
            }
            DateTime tmpDate = (DateTime)date;
            return tmpDate.Minute.ToString().Length == 1 ? "0" + tmpDate.Minute.ToString() : tmpDate.Minute.ToString();
        }


        /// <summary>
        /// 月をフォーマットするメソッド。
        /// </summary>
        /// <param name="month">月</param>
        /// <returns>
        /// フォーマットした月
        /// 引数：02 ⇒ 戻り値： 2
        /// 引数：12 ⇒ 戻り値：12
        /// </returns>
        public static string FormatMonthByStr(string month)
        {
            if (month == null)
            {
                return string.Empty;
            }

            if (month.Length == 2)
            {
                if (month.StartsWith("0"))
                {
                    return " " + month.Substring(1);
                }
                else
                {
                    return month;
                }
            }
            else
            {
                return " " + month;
            }
        }

        /// <summary>
        /// 日をフォーマットするメソッド。
        /// </summary>
        /// <param name="day">日</param>
        /// <returns>
        /// フォーマットした日
        /// 引数：02 ⇒ 戻り値： 2
        /// 引数：12 ⇒ 戻り値：12
        /// </returns>
        public static string FormatDayByStr(string day)
        {
            return FormatMonthByStr(day);
        }

        /// <summary>
        /// 日付型フォーマットメソッド(帳票用)。
        /// </summary>
        /// <param name="date">日付</param>
        /// <returns>yyyy/MM/dd（月、日の前ゼロはブランク）</returns>
        public static string FormatDate(string date)
        {
            DateTime parsedDate;
            if (!DateTime.TryParse(date, out parsedDate))
            {
                return string.Empty;
            }

            StringBuilder sb = new StringBuilder();
            sb.Append(parsedDate.Year.ToString());
            sb.Append("/");
            sb.Append(parsedDate.Month.ToString().PadLeft(2, ' '));
            sb.Append("/");
            sb.Append(parsedDate.Day.ToString().PadLeft(2, ' '));

            return sb.ToString();
        }
    }

    /// <summary>
    /// 和暦型モデルクラス
    /// 
    /// 作成日：2017/12/14
    /// 作成者：Rou I
    /// </summary>
    [Serializable]
    public class JapaneseDate
    {
        /// <summary>
        /// 年号
        /// </summary>
        public string Gengo { get; set; }

        /// <summary>
        /// 年
        /// </summary>
        public int Year { get; set; }

        /// <summary>
        /// 月
        /// </summary>
        public int Month { get; set; }

        /// <summary>
        /// 日
        /// </summary>
        public int Day { get; set; }
    }
}