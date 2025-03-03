using System.Text;
using System.Text.RegularExpressions;

namespace CoreLibrary.Core.Utility
{
    /// <summary>
    /// 文字ユーティリティ
    /// </summary>
    /// <remarks>
    /// 作成日：2017/11/07
    /// 作成者：Hiratake Minori
    /// </remarks>
    public class StringUtil
    {
        private static readonly Dictionary<char, string> katakana =
            new Dictionary<char, string> {
                {'ア',"ｱ"}, {'イ',"ｲ"}, {'ウ',"ｳ"}, {'エ',"ｴ"}, {'オ',"ｵ"},
                {'カ',"ｶ"}, {'キ',"ｷ"}, {'ク',"ｸ"}, {'ケ',"ｹ"}, {'コ',"ｺ"},
                {'サ',"ｻ"}, {'シ',"ｼ"}, {'ス',"ｽ"}, {'セ',"ｾ"}, {'ソ',"ｿ"},
                {'タ',"ﾀ"}, {'チ',"ﾁ"}, {'ツ',"ﾂ"}, {'テ',"ﾃ"}, {'ト',"ﾄ"},
                {'ナ',"ﾅ"}, {'ニ',"ﾆ"}, {'ヌ',"ﾇ"}, {'ネ',"ﾈ"}, {'ノ',"ﾉ"},
                {'ハ',"ﾊ"}, {'ヒ',"ﾋ"}, {'フ',"ﾌ"}, {'ヘ',"ﾍ"}, {'ホ',"ﾎ"},
                {'マ',"ﾏ"}, {'ミ',"ﾐ"}, {'ム',"ﾑ"}, {'メ',"ﾒ"}, {'モ',"ﾓ"},
                {'ヤ',"ﾔ"}, {'ユ',"ﾕ"}, {'ヨ',"ﾖ"},
                {'ラ',"ﾗ"}, {'リ',"ﾘ"}, {'ル',"ﾙ"}, {'レ',"ﾚ"}, {'ロ',"ﾛ"},
                {'ワ',"ﾜ"}, {'ヲ',"ｦ"}, {'ン',"ﾝ"},
                {'ァ',"ｧ"}, {'ィ',"ｨ"}, {'ゥ',"ｩ"}, {'ェ',"ｪ"}, {'ォ',"ｫ"},
                {'ガ',"ｶﾞ"},{'ギ',"ｷﾞ"},{'グ',"ｸﾞ"},{'ゲ',"ｹﾞ"},{'ゴ',"ｺﾞ"},
                {'ザ',"ｻﾞ"},{'ジ',"ｼﾞ"},{'ズ',"ｽﾞ"},{'ゼ',"ｾﾞ"},{'ゾ',"ｿﾞ"},
                {'ダ',"ﾀﾞ"},{'ヂ',"ﾁﾞ"},{'ヅ',"ﾂﾞ"},{'デ',"ﾃﾞ"},{'ド',"ﾄﾞ"},
                {'バ',"ﾊﾞ"},{'ビ',"ﾋﾞ"},{'ブ',"ﾌﾞ"},{'ベ',"ﾍﾞ"},{'ボ',"ﾎﾞ"},
                {'パ',"ﾊﾟ"},{'ピ',"ﾋﾟ"},{'プ',"ﾌﾟ"},{'ペ',"ﾍﾟ"},{'ポ',"ﾎﾟ"},
                {'ャ',"ｬ"}, {'ュ',"ｭ"}, {'ョ',"ｮ"}, {'ッ',"ｯ"},
                {'ヴ',"ｳﾞ"},{'ヷ',"ﾜﾞ"},{'ヺ',"ｦﾞ"},
                {'ー',"ｰ"}, {'・',"･"}, {'゛',"ﾞ"}, {'゜',"ﾟ"}, {'　'," "},
                {'Ａ',"A"}, {'Ｂ',"B"}, {'Ｃ',"C"}, {'Ｄ',"D"}, {'Ｅ',"E"},
                {'Ｆ',"F"}, {'Ｇ',"G"}, {'Ｈ',"H"}, {'Ｉ',"I"}, {'Ｊ',"J"},
                {'Ｋ',"K"}, {'Ｌ',"L"}, {'Ｍ',"M"}, {'Ｎ',"N"}, {'Ｏ',"O"},
                {'Ｐ',"P"}, {'Ｑ',"Q"}, {'Ｒ',"R"}, {'Ｓ',"S"}, {'Ｔ',"T"},
                {'Ｕ',"U"}, {'Ｖ',"V"}, {'Ｗ',"W"}, {'Ｘ',"X"}, {'Ｙ',"Y"},
                {'Ｚ',"Z"},
                {'ａ',"a"}, {'ｂ',"b"}, {'ｃ',"c"}, {'ｄ',"d"}, {'ｅ',"e"},
                {'ｆ',"f"}, {'ｇ',"g"}, {'ｈ',"h"}, {'ｉ',"i"}, {'ｊ',"j"},
                {'ｋ',"k"}, {'ｌ',"l"}, {'ｍ',"m"}, {'ｎ',"n"}, {'ｏ',"o"},
                {'ｐ',"p"}, {'ｑ',"q"}, {'ｒ',"r"}, {'ｓ',"s"}, {'ｔ',"t"},
                {'ｕ',"u"}, {'ｖ',"v"}, {'ｗ',"w"}, {'ｘ',"x"}, {'ｙ',"y"},
                {'ｚ',"z"},
                {'０',"0"}, {'１',"1"}, {'２',"2"}, {'３',"3"}, {'４',"4"},
                {'５',"5"}, {'６',"6"}, {'７',"7"}, {'８',"8"}, {'９',"9"},
                {'あ',"ｱ"}, {'い',"ｲ"}, {'う',"ｳ"}, {'え',"ｴ"}, {'お',"ｵ"},
                {'か',"ｶ"}, {'き',"ｷ"}, {'く',"ｸ"}, {'け',"ｹ"}, {'こ',"ｺ"},
                {'さ',"ｻ"}, {'し',"ｼ"}, {'す',"ｽ"}, {'せ',"ｾ"}, {'そ',"ｿ"},
                {'た',"ﾀ"}, {'ち',"ﾁ"}, {'つ',"ﾂ"}, {'て',"ﾃ"}, {'と',"ﾄ"},
                {'な',"ﾅ"}, {'に',"ﾆ"}, {'ぬ',"ﾇ"}, {'ね',"ﾈ"}, {'の',"ﾉ"},
                {'は',"ﾊ"}, {'ひ',"ﾋ"}, {'ふ',"ﾌ"}, {'へ',"ﾍ"}, {'ほ',"ﾎ"},
                {'ま',"ﾏ"}, {'み',"ﾐ"}, {'む',"ﾑ"}, {'め',"ﾒ"}, {'も',"ﾓ"},
                {'や',"ﾔ"}, {'ゆ',"ﾕ"}, {'よ',"ﾖ"},
                {'ら',"ﾗ"}, {'り',"ﾘ"}, {'る',"ﾙ"}, {'れ',"ﾚ"}, {'ろ',"ﾛ"},
                {'わ',"ﾜ"}, {'を',"ｦ"}, {'ん',"ﾝ"},
                {'ぁ',"ｧ"}, {'ぃ',"ｨ"}, {'ぅ',"ｩ"}, {'ぇ',"ｪ"}, {'ぉ',"ｫ"},
                {'が',"ｶﾞ"},{'ぎ',"ｷﾞ"},{'ぐ',"ｸﾞ"},{'げ',"ｹﾞ"},{'ご',"ｺﾞ"},
                {'ざ',"ｻﾞ"},{'じ',"ｼﾞ"},{'ず',"ｽﾞ"},{'ぜ',"ｾﾞ"},{'ぞ',"ｿﾞ"},
                {'だ',"ﾀﾞ"},{'ぢ',"ﾁﾞ"},{'づ',"ﾂﾞ"},{'で',"ﾃﾞ"},{'ど',"ﾄﾞ"},
                {'ば',"ﾊﾞ"},{'び',"ﾋﾞ"},{'ぶ',"ﾌﾞ"},{'べ',"ﾍﾞ"},{'ぼ',"ﾎﾞ"},
                {'ぱ',"ﾊﾟ"},{'ぴ',"ﾋﾟ"},{'ぷ',"ﾌﾟ"},{'ぺ',"ﾍﾟ"},{'ぽ',"ﾎﾟ"},
                {'ゃ',"ｬ"}, {'ゅ',"ｭ"}, {'ょ',"ｮ"}, {'っ',"ｯ"},{'ゔ',"ｳﾞ"},
                {'！',"!"}, {'＂',"\""}, {'＃',"#"}, {'＄',"$"}, {'％',"%"},
                {'＆',"&"}, {'＇',"'"}, {'’',"'"}, {'（',"("}, {'）',")"},
                {'＊',"*"}, {'＋',"+"}, {'，',","}, {'－',"-"}, {'．',"."},
                {'／',"/"}, {'：',":"}, {'；',";"}, {'＜',"<"}, {'＝',"="},
                {'＞',">"}, {'？',"?"}, {'＠',"@"}, {'［',"["}, {'＼',"\\"},
                {'］',"]"}, {'＾',"^"}, {'＿',"_"}, {'｀',"`"}, {'｛',"{"},
                {'｜',"|"}, {'｝',"}"}, {'～',"~"}, {'￥',"\\"},
            };

        /// <summary>
        /// 対象の文字列が数字のみかどうかを検査
        /// </summary>
        /// <param name="value">検査する文字列</param>
        /// <returns>検査結果(true / false)</returns>
        public static bool IsNumeric(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return false;
            }

            // 「0-9」を数字とする
            return Regex.IsMatch(value, @"^[0-9]+$");
        }

        /// <summary>
        /// 対象の文字列が半角英数のみかどうかを検査
        /// </summary>
        /// <param name="value">検査対象の文字列</param>
        /// <returns>検査結果(true / false)</returns>
        public static bool IsHalfWidthAlphaNum(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return false;
            }

            // 「a-zA-Z0-9」を半角英数とする
            return Regex.IsMatch(value, @"^[a-zA-Z0-9]+$");
        }

        /// <summary>
        /// 対象の文字列が半角英数記号のみかどうかを検査
        /// </summary>
        /// <param name="value">検査対象の文字列</param>
        /// <returns>検査結果(true / false)</returns>
        public static bool IsHalfWidthAlphaNumSymbol(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return false;
            }

            // 「" "（半角スペース）」～「~」を半角英数記号とする
            return Regex.IsMatch(value, @"^[ -~]+$");
        }

        /// <summary>
        /// 対象の文字列がひらがなのみかどうかを検査
        /// </summary>
        /// <param name="value">検査対象の文字列</param>
        /// <returns>検査結果(true / false)</returns>
        public static bool IsHiragana(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return false;
            }

            // 「ぁ」～「ゟ(より)」までと、
            // 「ー」をひらがなとする
            return Regex.IsMatch(value, @"^[\u3041-\u309F\u30FC]+$");
        }

        /// <summary>
        /// 対象の文字列が全角カタカナのみかどうかを検査
        /// </summary>
        /// <param name="value">検査対象の文字列</param>
        /// <returns>検査結果(true / false)</returns>
        public static bool IsFullWidthKatakana(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return false;
            }

            // 「゠」～「ヿ(コト)」までを全角カタカナとする
            return Regex.IsMatch(value, @"^[\u30A0-\u30FF]+$");
        }

        /// <summary>
        /// 対象の文字列が半角カタカナのみかどうかを検査
        /// </summary>
        /// <param name="value">検査対象の文字列</param>
        /// <returns>検査結果(true / false)</returns>
        public static bool IsHalfWidthKatakana(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return false;
            }

            // 「･」～「ﾟ」までと半角英数記号とする
            return Regex.IsMatch(value, @"^[\uFF65-\uFF9F -~]+$");
        }

        /// <summary>
        /// 対象の文字列がMS932の範囲内で、外字を含まないかどうかを検査
        /// </summary>
        /// <param name="value">検査対象の文字列</param>
        /// <returns>検査結果(true / false)</returns>
        public static bool IsMS932ExceptGaiji(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return false;
            }

            // MS932チェック
            var beforeUnicodeByte = Encoding.Unicode.GetBytes(value);
            var afterUnicodeByte = ConvUniSjisUni(beforeUnicodeByte);

            // 変換前後のUTF-8を比較
            if (!beforeUnicodeByte.SequenceEqual(afterUnicodeByte))
            {
                return false;
            }
            // 外字チェック
            return Regex.IsMatch(value, @"^[^\uE000-\uF8FF]+$");
        }

        /// <summary>
        /// Unicode をSJISに変換後、再びUnicodeに変換する
        /// </summary>
        /// <param name="beforeUnicodeByte"></param>
        /// <returns>Unicodeのバイト配列</returns>
        private static byte[] ConvUniSjisUni(byte[] beforeUnicodeByte)
        {
            // SJISに変換
            var encSJIS = Encoding.GetEncoding("shift_jis");
            var sjisByte = Encoding.Convert(Encoding.Unicode, encSJIS, beforeUnicodeByte);

            // もう一度UTF-8に戻す
            // UTF-8に変換できなかったものは「?」u003F になる
            return Encoding.Convert(encSJIS, Encoding.Unicode, sjisByte);
        }

        /// <summary>
        /// 対象の文字列がMS932の範囲内で、外字を含まないかどうかを検査
        /// </summary>
        /// <param name="value">検査対象の文字列</param>
        /// <returns>チェックに引っかかった文字</returns>
        public static String CheckMS932ExceptGaiji(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return "";
            }

            // MS932チェック
            var beforeUnicodeByte = Encoding.Unicode.GetBytes(value);
            var afterUnicodeByte = ConvUniSjisUni(beforeUnicodeByte);

            // 変換前後のUTF-8を比較
            List<byte> ngMS932 = new List<byte>();
            for (int i = 0; i < beforeUnicodeByte.Length; i++)
            {
                if (beforeUnicodeByte[i] != afterUnicodeByte[i])
                {
                    ngMS932.Add(beforeUnicodeByte[i]);
                }
            }

            var ngMS932ToString = new string(Encoding.Unicode.GetChars(ngMS932.ToArray()));

            // 外字チェック
            StringBuilder ngGaiji = new StringBuilder();
            foreach (Match match in Regex.Matches(value, @"[\uE000-\uF8FF]"))
            {
                // MS932のチェックに引っかかった文字以外を追加する。
                if (ngMS932ToString.IndexOf(match.Value) == -1)
                {
                    ngGaiji.Append(match.Value);
                }
            }

            return ngMS932ToString + ngGaiji.ToString();
        }

        /// <summary>
        /// 対象の文字列から全角カタカナを半角カタカナに変換
        /// </summary>
        /// <param name="value">変換する文字列</param>
        /// <returns>変換結果</returns>
        public static string ToHalfWidthKatakana(string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                string retValue = string.Empty;
                foreach (var ch in value.ToCharArray())
                {
                    retValue += katakana.ContainsKey(ch) ? katakana[ch] : ch.ToString();
                }
                return retValue;
            }
            return value;
        }

        /// <summary>
        /// 改行コードを削除する
        /// </summary>
        /// <param name="value">変換する文字列</param>
        /// <returns>変換結果</returns>
        public static string RemoveIndention(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return value;
            }

            return value.Replace("\r", string.Empty).Replace("\n", string.Empty);
        }

        /// <summary>
        /// PostGreSqlのEscape文字を転化する
        /// </summary>
        /// <param name="value">変換する文字列</param>
        /// <returns>変換結果</returns>
        public static string RemoveEscapeChar(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return value;
            }

            return value.Replace("\\", "\\\\").Replace("_", "\\_").Replace("%", "\\%");
        }

        /// <summary>
        /// 引数で指定された文字列がnullの場合、空文字列を返す
        /// null以外の場合は、引数で指定された文字列を返す
        /// </summary>
        /// <param name="value">検査対象文字列</param>
        /// <returns>nullの場合：空文字</returns>
        public static string GetEmptyIfNull(string value)
        {
            return value ?? string.Empty;
        }
    }
}