using NskAppModelLibrary.Models;
using NskWeb.Common.Consts;
using CoreLibrary.Core.Validator;
using System.ComponentModel.DataAnnotations;

namespace NskWeb.Areas.F01.Models.D0102
{
    /// <summary>
    /// 加入者情報：登録内容
    /// </summary>
    [Serializable]
    public class D0102EntryContent
    {
        public D0102EntryContent() { }

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="kanyusha">加入者情報</param>
        public D0102EntryContent(TKanyusha kanyusha)
        {
            // 加入者管理コード
            KanyushaCd = kanyusha.KanyushaCd;
            // 耕地郵便番号1
            KouchiPostalCd1 = string.IsNullOrEmpty(kanyusha.KouchiPostalCd) ? string.Empty : kanyusha.KouchiPostalCd.Substring(0, 3);
            // 耕地郵便番号2
            KouchiPostalCd2 = string.IsNullOrEmpty(kanyusha.KouchiPostalCd) ? string.Empty : kanyusha.KouchiPostalCd.Substring(3, 4);
            // 耕地住所（フリガナ）
            KouchiAddressKana = kanyusha.KouchiAddressKana;
            // 耕地住所
            KouchiAddress = kanyusha.KouchiAddress;
            // 耕地面積
            KouchiMenseki = kanyusha.KouchiMenseki.HasValue ? kanyusha.KouchiMenseki.Value.ToString() : "";
            // 耕地形態
            KouchiKeitaiCd = kanyusha.KouchiKeitaiCd;
            // 個人情報の取扱いについて
            KojinjohoToriatsukaiFlg = AppConst.FLG_ON.Equals(kanyusha.KojinjohoToriatsukaiFlg);
            // 申請年月日
            KanyuShinseiYmd = kanyusha.KanyuShinseiYmd;
            // 備考
            Biko = kanyusha.Biko;
        }
        #endregion

        #region プロパティ

        /// <summary>
        /// 対象年度
        /// </summary>
        [Display(Name = "対象年度")]
        [Required]
        public short? Nendo { get; set; }

        /// <summary>
        /// 加入者管理コード
        /// </summary>
        [Display(Name = "加入者管理コード")]
        [Numeric]
        [FullDigitLength(13)]
        public string KanyushaCd { get; set; }

        /// <summary>
        /// 耕地郵便番号1
        /// </summary>
        [Display(Name = "耕地郵便番号1")]
        [Numeric]
        [FullDigitLength(3)]
        public string KouchiPostalCd1 { get; set; }

        /// <summary>
        /// 耕地郵便番号2
        /// </summary>
        [Display(Name = "耕地郵便番号2")]
        [Numeric]
        [FullDigitLength(4)]
        public string KouchiPostalCd2 { get; set; }

        /// <summary>
        /// 耕地住所（フリガナ）
        /// </summary>
        [Display(Name = "耕地住所（フリガナ）")]
        [HalfWidthKatakana]
        [WithinStringLength(30)]
        public string KouchiAddressKana { get; set; }

        /// <summary>
        /// 耕地住所
        /// </summary>
        [Display(Name = "耕地住所")]
        [ExceptGaiji]
        [WithinStringLength(40)]
        public string KouchiAddress { get; set; }

        /// <summary>
        /// 耕地面積
        /// </summary>
        [Display(Name = "耕地面積")]
        [NumberDec(10, 1)]
        public string KouchiMenseki { get; set; }

        /// <summary>
        /// 耕地形態
        /// </summary>
        [Display(Name = "耕地形態")]
        [Required]
        public string KouchiKeitaiCd { get; set; }

        /// <summary>
        /// 個人情報の取扱いについて
        /// </summary>
        [Display(Name = "個人情報の取扱いについて")]
        public bool KojinjohoToriatsukaiFlg { get; set; }

        /// <summary>
        /// 申請年月日
        /// </summary>
        [Display(Name = "申請年月日")]
        [DateGYMD]
        public DateTime? KanyuShinseiYmd { get; set; }

        /// <summary>
        /// 備考
        /// </summary>
        [Display(Name = "備考")]
        [ExceptGaiji]
        [WithinStringLength(300)]
        public string Biko { get; set; }

        #endregion

    }
}