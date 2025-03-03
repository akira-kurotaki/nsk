using CoreLibrary.Core.Cache;
using CoreLibrary.Core.Consts;
using Microsoft.AspNetCore.Mvc.Rendering;
using ModelLibrary.Models;

namespace CoreLibrary.Core.Utility
{
    /// <summary>
    /// 年度テーブルのユーティリティクラス
    /// </summary>
    /// <remarks>
    /// 作成日：2018/02/19
    /// 作成者：Rou I
    /// </remarks>
    public static class NendoUtil
    {
        /// <summary>
        /// 年度ドロップダウンリストの取得メソッド。
        /// </summary>
        /// <param name="nendo">年度</param>
        /// <returns>年度選択リスト</returns>
        public static SelectList GetSelectList(string nendo)
        {

            List<SelectListItem> selectListItem = new List<SelectListItem>();

            IEnumerable<MNendo> nendoList = GetCandidateNendoList();

            foreach (var item in nendoList)
            {
                selectListItem.Add(new SelectListItem { Value = item.Nendo.ToString(), Text = item.NendoDisp1 });
            }

            if (string.IsNullOrEmpty(nendo))
            {
                return new SelectList(selectListItem, "Value", "Text");
            }
            else
            {
                return new SelectList(selectListItem, "Value", "Text", nendo);
            }
        }

        /// <summary>
        /// 年度選択リスト（表示範囲指定）を取得する。
        /// </summary>
        /// <param name="nendo">年度</param>
        /// <param name="rangeKbn">範囲指定区分値("0": 指定した年度までをリスト化する，"1":指定した年度以降をリスト化する)</param>
        /// <param name="rangeNendo">範囲指定年度</param>
        /// <returns>選択リスト（表示範囲指定）</returns>
        public static SelectList GetSelectListWithRange(string nendo, string rangeKbn, string rangeNendo)
        {

            List<SelectListItem> selectListItem = new List<SelectListItem>();

            IEnumerable<MNendo> nendoList = GetCandidateNendoListWithRange(rangeKbn, rangeNendo);

            foreach (var item in nendoList)
            {
                selectListItem.Add(new SelectListItem { Value = item.Nendo.ToString(), Text = item.NendoDisp1 });
            }

            if (string.IsNullOrEmpty(nendo))
            {
                return new SelectList(selectListItem, "Value", "Text");
            }
            else
            {
                return new SelectList(selectListItem, "Value", "Text", nendo);
            }
        }

        /// <summary>
        /// 候補年度のデータ取得メソッド。
        /// </summary>
        /// <returns>候補年度リスト</returns>
        public static IEnumerable<MNendo> GetCandidateNendoList()
        {
            // システム日付
            DateTime sysDateTime = DateUtil.GetSysDateTime();

            // システム日付.年
            int sysYear = sysDateTime.Year;
            // 候補年
            int candidateYear = sysYear;
            // 候補年/03/31 23:59:59
            DateTime candidateDateTime = new DateTime(sysYear, 3, 31, 23, 59, 59);

            // システム日付 > 候補年/03/31 23:59:59 の場合
            if (sysDateTime.CompareTo(candidateDateTime) > 0)
            {
                candidateYear = candidateYear + 1;
            }

            return GetNendoList().Where(a => a.Nendo <= candidateYear).OrderByDescending(a => a.Nendo).ToList();

        }

        /// <summary>
        /// 候補年度のデータ（表示範囲指定）を取得する
        /// </summary>
        /// <param name="rangeKbn">範囲指定区分値("0": 指定した年度までをリスト化する，"1":指定した年度以降をリスト化する)</param>
        /// <param name="rangeNendo">範囲指定年度</param>
        /// <returns>年度マスタデータリスト（表示範囲指定）</returns>
        public static IEnumerable<MNendo> GetCandidateNendoListWithRange(string rangeKbn, string rangeNendo)
        {
            // システム日付
            DateTime sysDateTime = DateUtil.GetSysDateTime();

            // システム日付.年
            int sysYear = sysDateTime.Year;
            // 検索年
            int candidateYear = sysYear;
            // 検索年/03/31 23:59:59
            DateTime candidateDateTime = new DateTime(sysYear, 3, 31, 23, 59, 59);

            // 本日日付が本日日付の年の3月31日 23時59分59秒より大きい場合
            if (sysDateTime.CompareTo(candidateDateTime) > 0)
            {
                candidateYear = candidateYear + 1;
            }

            var rangeNendoShort = short.Parse(rangeNendo);

            // 引数の範囲指定区分値は「"0"」の場合(指定した年度までをリスト化する場合)
            if ("0".Equals(rangeKbn))
            {
                return GetNendoList().Where(a => a.Nendo <= candidateYear && a.Nendo <= rangeNendoShort).OrderByDescending(a => a.Nendo).ToList();
            }

            // 引数の範囲指定区分値は「"1"」の場合(指定した年度以降をリスト化する場合)
            return GetNendoList().Where(a => a.Nendo <= candidateYear && a.Nendo >= rangeNendoShort).OrderByDescending(a => a.Nendo).ToList();
        }

        /// <summary>
        /// 基準年度の取得メソッド。
        /// </summary>
        /// <returns>基準年度</returns>
        public static string GetStandardNendo()
        {
            // システム日付
            DateTime sysDateTime = DateUtil.GetSysDateTime();

            // ３か月先の年度を返す
            DateTime addThreeMonthDateTime = sysDateTime.AddMonths(3);

            return addThreeMonthDateTime.Year.ToString();
        }

        /// <summary>
        /// 基準年度（表示範囲指定）を取得する
        /// </summary>
        /// <param name="rangeKbn">範囲指定区分値("0": 指定した年度までをリスト化する，"1":指定した年度以降をリスト化する)</param>
        /// <param name="rangeNendo">範囲指定年度</param>
        /// <returns>基準年度（表示範囲指定）</returns>
        public static string GetStandardNendoWithRange(string rangeKbn, string rangeNendo)
        {
            // 年度マスタデータリスト（表示範囲指定）」を取得する。
            IEnumerable<MNendo> nendoList = GetCandidateNendoListWithRange(rangeKbn, rangeNendo);

            // システム日付
            DateTime sysDateTime = DateUtil.GetSysDateTime();

            // 「システム日付+3か月」を取得する
            DateTime addThreeMonthDateTime = sysDateTime.AddMonths(3);

            // 「年度マスタデータリスト（表示範囲指定）」の件数>0の場合
            if (nendoList.Count() > 0)
            {
                // 取得した「システム日付 + 3か月」の年度が「年度マスタデータリスト（表示範囲指定）」に存在する場合
                if (nendoList.Where(t => t.Nendo == addThreeMonthDateTime.Year).Count() > 0)
                {
                    // 「システム日付+3か月」の年度を返却する
                    return addThreeMonthDateTime.Year.ToString();
                }
                // 取得した「システム日付+3か月」の年度が「年度マスタデータリスト（表示範囲指定）」に存在しない場合
                // 引数の「範囲指定年度」を返却する
                return rangeNendo;
            }
            // 取得した「年度マスタデータリスト（表示範囲指定）」の件数は0件の場合、空文字で返却する
            return "";

        }

        /// <summary>
        /// 基準年度(システム日付-1年)の取得メソッド。
        /// </summary>
        /// <returns>基準年度(システム日付-1年)</returns>
        public static string GetStandardNendoPreYear()
        {
            // システム日付
            DateTime sysDateTime = DateUtil.GetSysDateTime();

            // システム日付は2019以前の場合
            if (sysDateTime.Year <= 2019)
            {
                // 2019を返す
                return "2019";
            }
            // (システム日付-1年)を返す
            DateTime lastYearDateTime = sysDateTime.AddYears(-1);

            return lastYearDateTime.Year.ToString();
        }

        /// <summary>
        /// キャッシュから年度表示1を取得する。
        /// </summary>
        /// <param name="nendo">年度</param>
        /// <returns></returns>
        public static string GetNendoDisp1(int nendo)
        {
            var mNendo = GetNendo(nendo);
            return (null == mNendo || string.IsNullOrEmpty(mNendo.NendoDisp1)) ? string.Empty : mNendo.NendoDisp1;
        }

        /// <summary>
        /// キャッシュから年度表示1を取得する。
        /// </summary>
        /// <param name="nendo">年度</param>
        /// <returns></returns>
        public static string GetNendoDisp1(string nendo)
        {
            if (string.IsNullOrEmpty(nendo))
            {
                return string.Empty;
            }
            var mNendo = GetNendo(int.Parse(nendo));
            return (null == mNendo || string.IsNullOrEmpty(mNendo.NendoDisp1)) ? string.Empty : mNendo.NendoDisp1;
        }

        /// <summary>
        /// キャッシュから年度表示2を取得する。
        /// </summary>
        /// <param name="nendo">年度</param>
        /// <returns></returns>
        public static string GetNendoDisp2(int nendo)
        {
            var mNendo = GetNendo(nendo);
            return (null == mNendo || string.IsNullOrEmpty(mNendo.NendoDisp2)) ? string.Empty : mNendo.NendoDisp2;
        }

        /// <summary>
        /// キャッシュから年度表示2を取得する。
        /// </summary>
        /// <param name="nendo">年度</param>
        /// <returns></returns>
        public static string GetNendoDisp2(string nendo)
        {
            if (string.IsNullOrEmpty(nendo))
            {
                return string.Empty;
            }
            var mNendo = GetNendo(int.Parse(nendo));
            return (null == mNendo || string.IsNullOrEmpty(mNendo.NendoDisp2)) ? string.Empty : mNendo.NendoDisp2;
        }

        /// <summary>
        /// キャッシュから年度表示3を取得する。
        /// </summary>
        /// <param name="nendo">年度</param>
        /// <returns></returns>
        public static string GetNendoDisp3(int nendo)
        {
            var mNendo = GetNendo(nendo);
            return (null == mNendo || string.IsNullOrEmpty(mNendo.NendoDisp3)) ? string.Empty : mNendo.NendoDisp3;
        }

        /// <summary>
        /// キャッシュから年度表示3を取得する。
        /// </summary>
        /// <param name="nendo">年度</param>
        /// <returns></returns>
        public static string GetNendoDisp3(string nendo)
        {
            if (string.IsNullOrEmpty(nendo))
            {
                return string.Empty;
            }
            var mNendo = GetNendo(int.Parse(nendo));
            return (null == mNendo || string.IsNullOrEmpty(mNendo.NendoDisp3)) ? string.Empty : mNendo.NendoDisp3;
        }

        /// <summary>
        /// 年度マスタのデータ取得メソッド。
        /// </summary>
        /// <param name="nendo">年度</param>
        /// <returns>年度データ</returns>
        public static MNendo GetNendo(int nendo)
        {
            return GetNendoList().Where(a => a.Nendo == nendo).SingleOrDefault();
        }

        /// <summary>
        /// 年度マスタのデータ取得メソッド。
        /// </summary>
        /// <returns>年度リスト</returns>
        public static IEnumerable<MNendo> GetNendoList()
        {
            MNendoCache mNendoCache = new MNendoCache(CacheManager.GetInstance());
            return CacheUtil.Get<IEnumerable<MNendo>>(CacheManager.GetInstance(), CoreConst.M_NENDO_CACHE, () => (IEnumerable<MNendo>)mNendoCache.GetList());
        }

        /// <summary>
        /// 年度マスタのリフレッシュメソッド。
        /// </summary>
        /// <returns>なし</returns>
        public static void Refresh()
        {
            MNendoCache mNendoCache = new MNendoCache(CacheManager.GetInstance());
            CacheUtil.Refresh<IEnumerable<MNendo>>(CacheManager.GetInstance(), CoreConst.M_NENDO_CACHE, () => (IEnumerable<MNendo>)mNendoCache.GetList());
        }
    }
}