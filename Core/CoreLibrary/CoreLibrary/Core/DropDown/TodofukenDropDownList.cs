using CoreLibrary.Core.Dto;
using CoreLibrary.Core.Extensions;
using System.ComponentModel.DataAnnotations;

namespace CoreLibrary.Core.DropDown
{
    /// <summary>
    /// 都道府県マルチドロップダウンリスト用モデル
    /// </summary>
    /// <remarks>
    /// 作成日：2017/11/21
    /// 作成者：Rou I
    /// </remarks>
    [Serializable]
    public class TodofukenDropDownList : ITodofukenDropDownList
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public TodofukenDropDownList()
        {
            this.Init();
        }

        /// <summary>
        /// コンストラクタ
        /// <param name="name">モデルプロパティ名</param>
        /// </summary>
        public TodofukenDropDownList(string name)
        {
            this.Init(name);
        }

        /// <summary>
        /// コンストラクタ
        /// <param name="syokuin">ユーザ情報モデル</param>
        /// <param name="shishoList">支所情報リスト</param>
        /// </summary>
        public TodofukenDropDownList(Syokuin syokuin, List<Shisho> shishoList)
        {
            this.Init(syokuin, shishoList);
        }

        /// <summary>
        /// コンストラクタ
        /// <param name="name">モデルプロパティ名</param>
        /// <param name="syokuin">ユーザ情報モデル</param>
        /// <param name="shishoList">支所情報リスト</param>
        /// </summary>
        public TodofukenDropDownList(string name, Syokuin syokuin, List<Shisho> shishoList)
        {
            this.Init(name, syokuin, shishoList);
        }

        /// <summary>
        /// 初期化
        /// </summary>
        public void Init()
        {
            this.IsTodofuken = false;
            this.IsKumiaito = false;
            this.IsShisho = false;
        }

        /// <summary>
        /// 初期化
        /// <param name="name">モデルプロパティ名</param>
        /// </summary>
        public void Init(string name)
        {
            this.Init();
            this.ModelPropertyName = name;
        }

        /// <summary>
        /// 初期化
        /// <param name="syokuin">ユーザ情報モデル</param>
        /// <param name="shishoList">支所情報リスト</param>
        /// </summary>
        public void Init(Syokuin syokuin, List<Shisho> shishoList)
        {
            // 支所情報リスト
            this.ShishoList = shishoList;
            this.Init();
            if (syokuin != null)
            {
                // 都道府県コード指定ある場合
                if (!string.IsNullOrWhiteSpace(syokuin.TodofukenCd))
                {
                    // 都道府県
                    this.IsTodofuken = true;
                    this.TodofukenCd = syokuin.TodofukenCd;
                }
                // 組合等コード指定ある場合
                if (!string.IsNullOrWhiteSpace(syokuin.KumiaitoCd))
                {
                    // 組合等
                    this.IsKumiaito = true;
                    this.KumiaitoCd = syokuin.KumiaitoCd;
                }
                // 支所グループ情報指定ある場合
                if (!shishoList.IsNullOrEmpty() && !string.IsNullOrWhiteSpace(syokuin.ShishoCd))
                {
                    // 支所コード指定ある場合 
                    this.ShishoCd = syokuin.ShishoCd;
                }
                else if(!string.IsNullOrWhiteSpace(syokuin.ShishoCd)) 
                {
                    // 支所コード指定ある場合 
                    this.IsShisho = true;
                    this.ShishoCd = syokuin.ShishoCd;
                }
            }
        }

        /// <summary>
        /// 初期化
        /// <param name="name">モデルプロパティ名</param>
        /// <param name="syokuin">ユーザ情報モデル</param>
        /// <param name="shishoList">支所情報リスト</param>
        /// </summary>
        public void Init(string name, Syokuin syokuin, List<Shisho> shishoList)
        {
            this.Init(syokuin, shishoList);
            this.ModelPropertyName = name;
        }

        /// <summary>
        /// モデルプロパティ名
        /// </summary>
        public string ModelPropertyName { get; set; }

        /// <summary>
        /// 都道府県
        /// </summary>
        [Display(Name = "都道府県")]
        public string TodofukenCd { get; set; }

        /// <summary>
        /// 組合等
        /// </summary>
        [Display(Name = "組合等")]
        public string KumiaitoCd { get; set; }

        /// <summary>
        /// 支所
        /// </summary>
        [Display(Name = "支所")]
        public string ShishoCd { get; set; }

        /// <summary>
        /// 市町村
        /// </summary>
        [Display(Name = "市町村")]
        public string ShichosonCd { get; set; }

        /// <summary>
        /// 大地区
        /// </summary>
        [Display(Name = "大地区")]
        public string DaichikuCd { get; set; }

        /// <summary>
        /// 小地区
        /// </summary>
        [Display(Name = "小地区")]
        public string ShochikuCd { get; set; }

        /// <summary>
        /// 小地区(From)
        /// </summary>
        [Display(Name = "小地区")]
        public string ShochikuCdFrom { get; set; }

        /// <summary>
        /// 小地区(To)
        /// </summary>
        [Display(Name = "～")]
        public string ShochikuCdTo { get; set; }

        /// <summary>
        /// 都道府県コード指定有無フラグ
        /// </summary>
        public bool IsTodofuken { get; set; }

        /// <summary>
        /// 組合等コード指定有無フラグ
        /// </summary>
        public bool IsKumiaito { get; set; }

        /// <summary>
        /// 支所コード指定有無フラグ
        /// </summary>
        public bool IsShisho { get; set; }

        /// <summary>
        /// 支所情報リスト（セッションから取得する利用可能な支所一覧）
        /// </summary>
        public List<Shisho> ShishoList { get; set; }
    }
}