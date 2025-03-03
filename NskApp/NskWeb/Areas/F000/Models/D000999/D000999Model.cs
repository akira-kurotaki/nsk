using CoreLibrary.Core.Base;
using CoreLibrary.Core.Consts;
using CoreLibrary.Core.Validator;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace NskWeb.Areas.F000.Models.D000999
{
    [Serializable]
    public class D000999Model : CoreViewModel
    {
        public D000999Model()
        {
        }

        [DisplayName("ユーザＩＤ")]
        public string UserId { get; set; }
        [DisplayName("パスワード")]
        public string Password { get; set; }
        [DisplayName("メッセージエリア")]
        public string MessageArea { get; set; }

        /// <summary>
        /// ユーザ管理番号
        /// </summary>
        [Display(Name = "ユーザ管理番号")]
        [Numeric]
        public string UserKanriNo { get; set; }

        /// <summary>
        /// 都道府県コード
        /// </summary>
        [Display(Name = "都道府県コード")]
        [WithinStringLength(2)]
        public string TodofukenCd { get; set; }

        /// <summary>
        /// 都道府県名
        /// </summary>
        public string TodofukenNm { get; set; }

        /// <summary>
        /// 組合等コード
        /// </summary>
        [Display(Name = "組合等コード")]
        [WithinStringLength(3)]
        public string KumiaitoCd { get; set; }

        /// <summary>
        /// 組合等名
        /// </summary>
        public string KumiaitoNm { get; set; }

        /// <summary>
        /// 支所コード
        /// </summary>
        [Display(Name = "支所コード")]
        [WithinStringLength(2)]
        public string ShishoCd { get; set; }

        /// <summary>
        /// 支所名
        /// </summary>
        public string ShishoNm { get; set; }

        /// <summary>
        /// ユーザ管理権限
        /// </summary>
        public CoreConst.UserKanriKengen UserKanriKengen { get; set; }

        /// <summary>
        /// 氏名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 氏名カナ
        /// </summary>
        public string NameKana { get; set; }

        /// <summary>
        /// 電話番号
        /// </summary>
        public string Tel { get; set; }

        /// <summary>
        /// メールアドレス
        /// </summary>
        public string Mail { get; set; }

        /// <summary>
        /// システム利用者区分
        /// </summary>
        public CoreConst.SystemRiyoKbn SystemRiyoKbn { get; set; }

        /// <summary>
        /// 画面操作権限ID
        /// </summary>
        [Display(Name = "画面操作権限ID")]
        public int? ScreenSosaKengenId { get; set; }

        /// <summary>
        /// 支所グループID
        /// </summary>
        [Display(Name = "支所グループID")]
        public int? ShishoGroupId { get; set; }

        /// <summary>
        /// 画面表示モード
        /// </summary>
        public string ScreenMode { get; set; }
    }
}