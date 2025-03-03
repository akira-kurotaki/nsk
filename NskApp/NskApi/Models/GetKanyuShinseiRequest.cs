using NskApi.Base;
using System.Runtime.Serialization;

namespace NskApi.Models
{
    /// <summary>
    /// 加入者情報取得リクエスト
    /// </summary>
    public class GetKanyuShinseiRequest : ApiRequestBase
    {
        
        
        /// <summary>
        /// 対象年度
        /// </summary>
        [DataMember(Name = "nendo")]
        public string Nendo { get; set; }

        /// <summary>
        /// 連携都道府県コード
        /// </summary>
        [DataMember(Name = "renkei_todofuken_cd")]
        public string RenkeiTodofukenCd { get; set; }

        /// <summary>
        /// 連携組合等コード
        /// </summary>
        [DataMember(Name = "renkei_kumiaito_cd")]
        public string RenkeiKumiaitoCd { get; set; }

        /// <summary>
        /// 連携支所コード
        /// </summary>
        [DataMember(Name = "renkei_shisho_cd")]
        public string RenkeiShishoCd { get; set; }

        /// <summary>
        /// 加入者管理コード開始
        /// </summary>
        [DataMember(Name = "kanyusha_cd_start")]
        public string KanyushaCdStart { get; set; }

        /// <summary>
        /// 加入者管理コード終了
        /// </summary>
        [DataMember(Name = "kanyusha_cd_end")]
        public string KanyushaCdEnd { get; set; }

        /// <summary>
        /// トークン
        /// </summary>
        [DataMember(Name = "token")]
        public string Token { get; set; }
    }
}
