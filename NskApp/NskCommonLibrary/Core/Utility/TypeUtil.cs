using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;
using System.Text;
using CoreLibrary.Core.Consts;

namespace NskCommonLibrary.Core.Utility
{
    /// <summary>
    /// タイプユーティリティクラス
    /// 
    /// 作成日：2025/02/14
    /// </summary>
    public static class TypeUtil
    {

        /// <summary>
        /// 数値 文字 変換
        /// </summary>
        public class NullableIntConverter : ITypeConverter
        {
            /// <summary>
            /// 文字→数値("NULL"→NULL)
            /// </summary>
            /// <param name="text"></param>
            /// <param name="row"></param>
            /// <param name="memberMapData"></param>
            /// <returns></returns>
            public object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData)
            {
                return text == "NULL" ?
                    null :
                    (int?)int.Parse(text);
            }

            /// <summary>
            /// 数値→文字(数値項目がNULLだったら空欄にする)
            /// </summary>
            /// <param name="value"></param>
            /// <param name="row"></param>
            /// <param name="memberMapData"></param>
            /// <returns></returns>
            public string ConvertToString(object value, IWriterRow row, MemberMapData memberMapData)
            {
                string ret = string.Empty;
                try
                {
                    if(value == null)
                    {
                        ret = "";
                    }
                    else
                    {
                        ret = value.ToString();
                    }
                }
                catch
                {
                    ret = "";
                }
                return ret;
            }
        }

        /// <summary>
        /// 文字コード設定 
        /// </summary>
        /// <param name="strEncode"></param>
        /// <returns></returns>
        // 
        public static Encoding SetEncoding(string strEncode)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            Encoding encoding = Encoding.Default;
            if (CoreConst.CharacterCode.UTF8.Equals(strEncode))
            {
                encoding = Encoding.UTF8;
            }
            else if (CoreConst.CharacterCode.SJIS.Equals(strEncode))
            {
                encoding = Encoding.GetEncoding("Shift_JIS");
            }
            return encoding;
        }

        #region 文字・日付項目をダブルクォーテーションで囲む
        /// <summary>
        /// 文字・日付項目をダブルクォーテーションで囲む
        /// </summary>
        /// <param name="args">項目の配列</param>
        /// <returns>bool true:ダブルクォーテーションで囲む / false:何もしない</returns>
        public static bool SetShouldQuote(ShouldQuoteArgs args)
        {
            var config = args.Row.Configuration;
            var shouldQuote = false;
            if (args.FieldType.Name.ToLower() == "string" || args.FieldType.Name.ToLower() == "datetime")
            {
                shouldQuote = true;
            }

            return shouldQuote;
        }
        #endregion

        #region すべてをダブルクォーテーションで囲む
        /// <summary>
        /// すべてをダブルクォーテーションで囲む
        /// </summary>
        /// <param name="args">項目の配列</param>
        /// <returns>bool true:ダブルクォーテーションで囲む / false:何もしない</returns>
        public static bool SetShouldQuoteAll(ShouldQuoteArgs args)
        {
            var config = args.Row.Configuration;
            return true;
        }
        #endregion

    }
}
