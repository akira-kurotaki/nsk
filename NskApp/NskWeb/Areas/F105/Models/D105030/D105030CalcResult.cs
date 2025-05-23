﻿using NskAppModelLibrary.Context;
using NskAppModelLibrary.Models;
using System.ComponentModel.DataAnnotations;

namespace NskWeb.Areas.F105.Models.D105030
{
    /// <summary>
    /// 引受情報計算結果
    /// </summary>
    [Serializable]
    public class D105030CalcResult
    {
        /// <summary>メッセージエリア３</summary>
        public string MessageArea3 { get; set; } = string.Empty;

        /// <summary>筆数の合計</summary>
        [Display(Name = "筆数の合計")]
        [DisplayFormat(DataFormatString = "{0:0}", ApplyFormatInEditMode = true)]
        public decimal? FudesuGokei { get; set; }
        /// <summary>引受筆数</summary>
        [Display(Name = "引受筆数")]
        [DisplayFormat(DataFormatString = "{0:0}", ApplyFormatInEditMode = true)]
        public decimal? HikiukeFudesu { get; set; }
        /// <summary>引受面積の合計</summary>
        [Display(Name = "筆数の合計")]
        [DisplayFormat(DataFormatString = "{0:0}", ApplyFormatInEditMode = true)]
        public decimal? HikiukeMensekiGokei { get; set; }
        /// <summary>転作等面積の合計</summary>
        [Display(Name = "引受面積の合計")]
        [DisplayFormat(DataFormatString = "{0:0}", ApplyFormatInEditMode = true)]
        public decimal? TensakutoMensekiGokei { get; set; }
        /// <summary>本地面積の合計</summary>
        [Display(Name = "本地面積の合計")]
        [DisplayFormat(DataFormatString = "{0:0}", ApplyFormatInEditMode = true)]
        public decimal? HonchiMensekiGokei { get; set; }
        /// <summary>引受回</summary>
        [Display(Name = "引受回")]
        [DisplayFormat(DataFormatString = "{0:0}", ApplyFormatInEditMode = true)]
        public decimal? HikiukeKai { get; set; }

        /// <summary>
        /// 計算結果の算出
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="sessionInfo"></param>
        /// <param name="hikiukeSearchResult"></param>
        public void Calc(NskAppContext dbContext, D105030SessionInfo sessionInfo, D105030HikiukeSearchResult hikiukeSearchResult)
        {
            List<D105030HikiukeRecord> hikiukeRecords = hikiukeSearchResult.GetResult(dbContext, sessionInfo);

            // ３．１．１．[筆数の合計] の取得
            //   [筆数の合計]に[検索結果]の件数を設定する。
            FudesuGokei = hikiukeRecords.Count;

            // ３．１．２．[引受筆数] の取得
            //   [引受筆数]に[検索結果.引受面積]が0より大きいデータの件数を設定する。
            HikiukeFudesu = hikiukeRecords.Count(x => x.HikukeMenseki.GetValueOrDefault(0) > 0);

            // ３．１．３．[引受面積の合計] の取得
            //   [引受面積の合計]に[検索結果.引受面積]の合計を設定する。
            HikiukeMensekiGokei = hikiukeRecords.Sum(x => x.HikukeMenseki.GetValueOrDefault(0));

            // ３．１．４．[転作等面積の合計] の取得
            //   [転作等面積の合計]に[検索結果.転作等面積]の合計を設定する。
            TensakutoMensekiGokei = hikiukeRecords.Sum(x => x.TensakutoMenseki.GetValueOrDefault(0));

            // ３．１．５．[本地面積の合計] の取得
            //   [本地面積の合計]に[検索結果.耕地面積]の合計を設定する。
            HonchiMensekiGokei = hikiukeRecords.Sum(x => x.HonchiMenseki.GetValueOrDefault(0));

            // ３．１．６．[引受回] の取得
            // (1) t_00010_引受回テーブルから引受回を取得する。
            IQueryable<T00010引受回> hikiukeKais = dbContext.T00010引受回s.Where(m =>
                (m.組合等コード == sessionInfo.KumiaitoCd) &&
                (m.年産 == sessionInfo.Nensan) &&
                (m.共済目的コード == sessionInfo.KyosaiMokutekiCd));
            short? maxHikiukeKai = null;
            if (hikiukeKais.Count() > 0)
            {
                maxHikiukeKai = hikiukeKais.Max(m => m.引受回);
            }

            // (2)[引受回]に[取得した結果.引受回]の最大値を設定する。
            HikiukeKai = maxHikiukeKai;
        }
    }
}
