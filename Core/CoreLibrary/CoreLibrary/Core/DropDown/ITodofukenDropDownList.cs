using CoreLibrary.Core.Dto;

namespace CoreLibrary.Core.DropDown
{
    /// <summary>
    /// 都道府県ドロップダウンリスト用モデルのインタフェース
    /// </summary>
    /// <remarks>
    /// 作成日：2017/12/19
    /// 作成者：Rou I
    /// </remarks>
    public interface ITodofukenDropDownList
    {
        /// <summary>
        /// モデルプロパティ名
        /// </summary>
        string ModelPropertyName { get; }

        /// <summary>
        /// 都道府県
        /// </summary>
        string TodofukenCd { get; }

        /// <summary>
        /// 組合等
        /// </summary>
        string KumiaitoCd { get; }

        /// <summary>
        /// 支所
        /// </summary>
        string ShishoCd { get; }

        /// <summary>
        /// 市町村
        /// </summary>
        string ShichosonCd { get; }

        /// <summary>
        /// 大地区
        /// </summary>
        string DaichikuCd { get; }

        /// <summary>
        /// 小地区
        /// </summary>
        string ShochikuCd { get; }

        /// <summary>
        /// 小地区(From)
        /// </summary>
        string ShochikuCdFrom { get; }

        /// <summary>
        /// 小地区(To)
        /// </summary>
        string ShochikuCdTo { get; }

        /// <summary>
        /// 都道府県コード指定有無フラグ
        /// </summary>
        bool IsTodofuken { get; }

        /// <summary>
        /// 組合等コード指定有無フラグ
        /// </summary>
        bool IsKumiaito { get; }

        /// <summary>
        /// 支所コード指定有無フラグ
        /// </summary>
        bool IsShisho { get; }

        /// <summary>
        /// 支所情報リスト（セッションから取得する利用可能な支所一覧）
        /// </summary>
        List<Shisho> ShishoList { get; set; }
    }
}