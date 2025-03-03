namespace CoreLibrary.Core.DropDown
{
    /// <summary>
    /// 種類パラメータクラス
    /// </summary>
    /// <remarks>
    /// 作成日：2018/05/16
    /// 作成者：Rou I
    /// </remarks>
    [Serializable]
    public class ShuruiParam
    {
        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public ShuruiParam()
        {
        }

        /// <summary>
        /// コンストラクタ
        /// <param name="detailModelName">明細モデル変数名</param>
        /// <param name="todofukenCd">都道府県コード</param>
        /// <param name="kumiaitoCd">組合等コード</param>
        /// <param name="shishoCd">支所コード</param>
        /// </summary>
        public ShuruiParam(string detailModelName, string todofukenCd, string kumiaitoCd, string shishoCd)
        {
            DetailModelName = detailModelName;
            TodofukenCd = todofukenCd;
            KumiaitoCd = kumiaitoCd;
            ShishoCd = shishoCd;
        }

        /// <summary>
        /// コンストラクタ
        /// <param name="detailModelName">明細モデル変数名</param>
        /// <param name="todofukenCd">都道府県コード</param>
        /// <param name="kumiaitoCd">組合等コード</param>
        /// <param name="shishoCd">支所コード</param>
        /// <param name="shuruiOnchangeFunc">種類onchangeイベントファンクション名</param>
        /// <param name="himmokuOnchangeFunc">品目onchangeイベントファンクション名</param>
        /// </summary>
        public ShuruiParam(string detailModelName, string todofukenCd, string kumiaitoCd, string shishoCd,
                            string shuruiOnchangeFunc, string himmokuOnchangeFunc)
        {
            DetailModelName = detailModelName;
            TodofukenCd = todofukenCd;
            KumiaitoCd = kumiaitoCd;
            ShishoCd = shishoCd;
            HimmokuOnchangeFunc = himmokuOnchangeFunc;
            ShuruiOnchangeFunc = shuruiOnchangeFunc;
        }
        #endregion

        #region プロパティ
        /// <summary>
        /// 明細モデル変数名
        /// </summary>
        public string DetailModelName { get; set; }

        /// <summary>
        /// 都道府県
        /// </summary>
        public string TodofukenCd { get; set; }

        /// <summary>
        /// 組合等
        /// </summary>
        public string KumiaitoCd { get; set; }

        /// <summary>
        /// 支所
        /// </summary>
        public string ShishoCd { get; set; }

        /// <summary>
        /// 種類onchangeイベントファンクション名
        /// </summary>
        public string ShuruiOnchangeFunc { get; set; }

        /// <summary>
        /// 品目onchangeイベントファンクション名
        /// </summary>
        public string HimmokuOnchangeFunc { get; set; }

        #endregion

    }
}