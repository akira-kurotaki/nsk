using NskApi.Base;
using System.Runtime.Serialization;

namespace NskApi.Models
{
    /// <summary>
    /// 加入者情報取得リクエスト
    /// </summary>
    public class InsertKanyuShinseiRequest : ApiRequestBase
    {

        /// <summary>
        /// 農業者ID
        /// </summary>
        [DataMember(Name = "nogyosha_id")]
        public string NogyoshaId { get; set; }

        /// <summary>
        /// 加入者管理コード
        /// </summary>
        [DataMember(Name = "kanyusha_cd")]
        public string KanyushaCd { get; set; }

        /// <summary>
        /// 対象年度
        /// </summary>
        [DataMember(Name = "nendo")]
        public string Nendo { get; set; }

        /// <summary>
        /// 耕地郵便番号
        /// </summary>
        [DataMember(Name = "kouchi_postal_cd")]
        public string KouchiPostalCd { get; set; }

        /// <summary>
        /// 耕地住所フリガナ
        /// </summary>
        [DataMember(Name = "kouchi_address_kana")]
        public string KouchiAddressKana { get; set; }

        /// <summary>
        /// 耕地住所
        /// </summary>
        [DataMember(Name = "kouchi_address")]
        public string KouchiAddress { get; set; }

        /// <summary>
        /// 耕地面積
        /// </summary>
        [DataMember(Name = "kouchi_menseki")]
        public string KouchiMenseki { get; set; }

        /// <summary>
        /// 耕地形態
        /// </summary>
        [DataMember(Name = "kouchi_keitai_cd")]
        public string KouchiKeitaiCd { get; set; }

        /// <summary>
        /// 個人情報取扱フラグ
        /// </summary>
        [DataMember(Name = "kojinjoho_toriatsukai_flg")]
        public string KojinjohoToriatsukaiFlg { get; set; }

        /// <summary>
        /// 加入申請年月日
        /// </summary>
        [DataMember(Name = "kanyu_shinsei_ymd")]
        public string KanyuShinseiYmd { get; set; }

        /// <summary>
        /// 備考
        /// </summary>
        [DataMember(Name = "biko")]
        public string Biko { get; set; }

    }
}
