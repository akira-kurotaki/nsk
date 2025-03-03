using System.Runtime.Serialization;

namespace BaseDmpApi.Base
{
    /// <summary>
    /// データ連携REST APIサービスのリクエストの基底クラス
    /// </summary>
    public abstract class ApiRequestBase
    {
        /// <summary>
        /// パスフレーズ
        /// </summary>
        [DataMember(Name = "pass")]
        public string Pass { get; set; }

        /// <summary>
        /// システム区分
        /// </summary>
        [DataMember(Name = "system_kbn")]
        public string SystemKbn { get; set; }

        /// <summary>
        /// 都道府県コード
        /// </summary>
        [DataMember(Name = "todofuken_cd")]
        public string TodofukenCd { get; set; }

        /// <summary>
        /// 組合等コード
        /// </summary>
        [DataMember(Name = "kumiaito_cd")]
        public string KumiaitoCd { get; set; }

        /// <summary>
        /// 支所コード
        /// </summary>
        [DataMember(Name = "shisho_cd")]
        public string ShishoCd { get; set; }

    }
}
