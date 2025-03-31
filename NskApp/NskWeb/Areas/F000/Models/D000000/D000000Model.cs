using CoreLibrary.Core.Base;
using CoreLibrary.Core.Dto;
using ModelLibrary.Models;
using static NskWeb.Areas.F02.Models.D0201.D0201SearchCondition;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel;

namespace NskWeb.Areas.F000.Models.D000000
{
    /// <summary>
    /// ポータル
    /// </summary>
    /// <remarks>
    /// 作成日：2018/03/07
    /// 作成者：Gon Etuun
    /// </remarks>
    [Serializable]
    public class D000000Model : CoreViewModel
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public D000000Model()
        {
            SKyosaiMokutekiCd = "";
            SNensanHikiuke = "";
            SNensanHikiuke_dmy = "";
            SNensanHyoka = "";
            SNensanHyoka_dmy = "";
            SHikiukeJikkoTanniKbnHikiuke = "";
            SHikiukeJikkoTanniKbnHyoka = "";
            MessageArea1 = "";
            KumiaitoCd = "";
            ShishoCd = "";
            ShishoList = new ();
            ShishoGroupId = null;

            HikiukeNensanList = new List<SelectListItem>();
            HikiukeNensanModelList = new List<D000000Nensan> ();
            HyokaNensanList = new List<SelectListItem>();
            HyokaNensanModelList = new List<D000000Nensan>();
            SagyoJokyoList = new List<D000000SagyoJokyo> ();
            Torimatome = new D000000Torimatome();
            HikiukeHyokaJokyoList_Hiki = new List<D000000HikiukeHyokaJokyo> ();
            HikiukeHyokaJokyoList_Hyo = new List<D000000HikiukeHyokaJokyo> ();
            //CanUpdate = true;
            CanUpdateNensan = true;
        }

        /// <summary>
        /// 共済目的コード
        /// </summary>
        [DisplayName("共済目的")]
        //[Required]
        public string SKyosaiMokutekiCd { get; set; }
        /// <summary>
        /// 引受年産（リストボックス選択）
        /// </summary>
        //[Required]
        [DisplayName("引受年産")]
        public string SNensanHikiuke { get; set; }
        /// <summary>
        /// 引受年産（変更不可時の表示用）
        /// </summary>
        public string SNensanHikiuke_dmy { get; set; }
        /// <summary>
        /// 評価年産（リストボックス選択）
        /// </summary>
        //[Required]
        [DisplayName("評価年産")]
        public string SNensanHyoka { get; set; }
        /// <summary>
        /// 評価年産（変更不可時の表示用）
        /// </summary>
        public string SNensanHyoka_dmy { get; set; }
        /// <summary>
        /// 引受計算支所実行単位区分（引受年）
        /// </summary>
        [DisplayName("引受計算支所実行単位区分_引受")]
        public string SHikiukeJikkoTanniKbnHikiuke { get; set; }
        /// <summary>
        /// 引受計算支所実行単位区分（評価年）
        /// </summary>
        [DisplayName("引受計算支所実行単位区分_評価")]
        public string SHikiukeJikkoTanniKbnHyoka { get; set; }

        /// <summary>
        /// メッセージエリア1
        /// </summary>
        [Display(Name = "メッセージエリア1")]
        public string MessageArea1 { get; set; }

        /// <summary>
        /// 作業状況リスト
        /// </summary>
        public List<D000000SagyoJokyo> SagyoJokyoList { get; set; }

        /// <summary>
        /// とりまとめ状況
        /// </summary>
        public D000000Torimatome Torimatome { get; set; }

        /// <summary>
        /// 引受状況、評価状況リスト（引受年産）
        /// </summary>
        public List<D000000HikiukeHyokaJokyo> HikiukeHyokaJokyoList_Hiki { get; set; }

        /// <summary>
        /// 引受状況、評価状況リスト（評価年産）
        /// </summary>
        public List<D000000HikiukeHyokaJokyo> HikiukeHyokaJokyoList_Hyo { get; set; }

        /// <summary>
        /// 都道府県コード
        /// </summary>
        public string TodofukenCd { get; set; }

        /// <summary>
        /// ログインユーザの組合等コード
        /// </summary>
        public string KumiaitoCd {  get; set; }

        /// <summary>
        /// ログインユーザの支所コード
        /// </summary>
        public string ShishoCd { get; set; }

        /// <summary>
        /// 利用可能支所リスト
        /// </summary>
        public List<Shisho> ShishoList { get; set; }

        /// <summary>
        /// 支所グループID
        /// </summary>
        public int? ShishoGroupId { get; set; }

        /// <summary>
        /// 共済目的リスト
        /// </summary>
        public List<SelectListItem> KyosaiMokutekiList { get; set; }

        /// <summary>
        /// 引受年産リスト
        /// </summary>
        public List<SelectListItem> HikiukeNensanList { get; set; }
        /// <summary>
        /// 引受年産モデルリスト
        /// </summary>
        public List<D000000Nensan> HikiukeNensanModelList { get; set; }

        /// <summary>
        /// 評価年産リスト
        /// </summary>
        public List<SelectListItem> HyokaNensanList { get; set; }
        /// <summary>
        /// 評価年産モデルリスト
        /// </summary>
        public List<D000000Nensan> HyokaNensanModelList { get; set; }

        /// <summary>
        /// 画面に対するユーザの更新可否
        /// </summary>
        //public bool CanUpdate { get; set; }

        /// <summary>
        /// NSKポータル年産変更権限
        /// </summary>
        public bool CanUpdateNensan { get; set; }

    }
}