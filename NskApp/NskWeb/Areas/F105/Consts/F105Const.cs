using System.ComponentModel;

namespace NskWeb.Areas.F105.Consts
{
    /// <summary>
    /// F105機能共通定数クラス
    /// </summary>
    public class F105Const
    {
        /// <summary>
        /// ページ0
        /// </summary>
        public const string PAGE_0 = "0";

        /// <summary>
        /// ページ1
        /// </summary>
        public const int PAGE_1 = 1;

        /// <summary>
        /// 加入申込書入力（水稲）画面ID(D105030)
        /// </summary>
        public const string SCREEN_ID_D105030 = "D105030";

        /// <summary>
        /// 加入申込書入力組合員等検索（共通）画面ID(D105036)
        /// </summary>
        public const string SCREEN_ID_D105036 = "D105036";

        /// <summary>
        /// 加入申込書入力（麦）画面ID(D105070)
        /// </summary>
        public const string SCREEN_ID_D105070 = "D105070";

        /// <summary>
        /// 加入申込書入力（麦）統計地域危険段階区分設定画面ID(D105073)
        /// </summary>
        public const string SCREEN_ID_D105073 = "D105073";
        /// <summary>
        /// 加入申込書入力（麦）用途別単価設定画面ID(D105074)
        /// </summary>
        public const string SCREEN_ID_D105074 = "D105074";
        /// <summary>
        /// 基準収穫量設定（災害収入、品質）画面ID(D105150)
        /// </summary>
        public const string SCREEN_ID_D105150 = "D105150";

        /// <summary>
        /// 共済金額設定画面ID(D105190)
        /// </summary>
        public const string SCREEN_ID_D105190 = "D105190";



        /// <summary>
        /// 画面間パラメタ「D105036 -> D105030、D105070」
        /// （セッション）
        /// </summary>
        public const string SESS_D105036_PARAMS = $"{SCREEN_ID_D105036}_PARAMS";

        /// <summary>
        /// 画面間パラメタ「D105070 -> D105073、D105074」
        /// （セッション）
        /// </summary>
        public const string SESS_D105070_PARAMS = $"{SCREEN_ID_D105070}_PARAMS";

        /// <summary>
        /// 名称グループコード　作付時期（麦）
        /// </summary>
        public const string MEISHO_GRP_SAKUTUKEJIKI_MUGI = "41";

        /// <summary>
        /// 加入状況
        /// </summary>
        public enum KanyuStateType
        {
            /// <summary>加入</summary>
            [Description("加入")]
            Member,
            /// <summary>未加入</summary>
            [Description("未加入")]
            NoMember,
        }


        /// <summary>
        /// 加入形態ドロップダウンリスト要素
        /// </summary>
        public enum KanyuKeitaiType
        {
            /// <summary>個人</summary>
            [Description("個人")]
            Kojin = 1,
            /// <summary>農作物共済資格団体</summary>
            [Description("農作物共済資格団体")]
            Nsk,
            /// <summary>法人</summary>
            [Description("法人")]
            Hojin = 10,
        }

        /// <summary>
        /// 受委託者区分ドロップダウンリスト要素
        /// </summary>
        public enum JuitakusyaKbn
        {
            /// <summary>無し</summary>
            [Description("無し")]
            None,
            /// <summary>受託</summary>
            [Description("受託")]
            Jutaku,
            /// <summary>委託</summary>
            [Description("委託")]
            Itaku,
        }

        /// <summary>
        /// 共済金額改定時コードドロップダウンリスト要素
        /// </summary>
        public enum KyosaiKingakuKaiteiKbn
        {
            /// <summary>a：変更後の共済限度額と同額</summary>
            [Description("a：変更後の共済限度額と同額")]
            Dogaku,
            /// <summary>b：変更しない</summary>
            [Description("b：変更しない")]
            None,
            /// <summary>c：改定時に申請する。</summary>
            [Description("c：改定時に申請する。")]
            Shinsei,

        }

        /// <summary>
        /// 引受方式
        /// </summary>
        public enum HikiukeHoushikiCd
        {
            /// <summary>半相殺</summary>
            [Description("半相殺")]
            Hansousai = 2,
            /// <summary>全相殺</summary>
            [Description("全相殺")]
            Zensousai,
            /// <summary>災害収入</summary>
            [Description("災害収入")]
            SaigaiSyuunyuu,
            /// <summary>品質</summary>
            [Description("品質")]
            Hinshitsu,
            /// <summary>地域インデックス</summary>
            [Description("地域インデックス")]
            ChiikiIndex
        }

        /// <summary>
        /// 権限
        /// </summary>
        public enum Authority
        {
            /// <summary>権限なし</summary>
            [Description("権限なし")]
            None,
            /// <summary>参照権限</summary>
            [Description("参照権限")]
            ReadOnly,
            /// <summary>一部権限</summary>
            [Description("一部権限")]
            Part,
            /// <summary>更新権限</summary>
            [Description("更新権限")]
            Update
        }

        /// <summary>
        /// 担手農家区分
        /// </summary>
        public enum NinaiteNoukaKbn
        {
            /// <summary>課税</summary>
            [Description("課税")]
            Kazei = 1,
            /// <summary>免税</summary>
            [Description("免税")]
            Menzei,
            /// <summary>その他</summary>
            [Description("その他")]
            Other
        }
    }
}
