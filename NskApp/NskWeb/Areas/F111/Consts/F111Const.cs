using System.ComponentModel;

namespace NskWeb.Areas.F111.Consts
{
    /// <summary>
    /// F11機能共通定数クラス
    /// </summary>
    public class F111Const
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
        /// 交付金計算処理画面ID(D111010)
        /// </summary>
        public const string SCREEN_ID_D111010 = "D111010";

        /// <summary>
        /// 交付金計算掛金徴収額入力画面ID(D111050)
        /// </summary>
        public const string SCREEN_ID_D111050 = "D111050";

        /// <summary>
        /// 交付金計算処理バッチID(NSK_111011B)
        /// </summary>
        public const string BATCH_ID_B111011 = "B111011";

        /// <summary>
        /// 交付金計算処理バッチID（正式ID）(NSK_111011B)
        /// </summary>
        public const string SCREEN_ID_NSK_111010D = "NSK_111010D";

        /// <summary>
        /// 交付金計算処理バッチID（正式ID）(NSK_111011B)
        /// </summary>
        public const string BATCH_ID_NSK_111011B = "NSK_111011B";

        /// <summary>
        /// セッションキー(D111010)
        /// </summary>
        public const string SESS_D111010 = $"{SCREEN_ID_D111010}_SCREEN";

        /// <summary>
        /// セッションキー(D111050)
        /// </summary>
        public const string SESS_D111050 = $"{SCREEN_ID_D111050}_SCREEN";

        /// <summary>
        /// 画面間パラメタ「D111010 -> D111050
        /// （セッション）
        /// </summary>
        public const string SESS_D111010_PARAMS = $"{SCREEN_ID_D111010}_PARAMS";

        /// <summary>
        /// 加入状況
        /// </summary>
        public enum KanyuStateType
        {
            /// <summary>加入</summary>
            [Description("加入")]
            MEMBER,
            /// <summary>未加入</summary>
            [Description("未加入")]
            NO_MEMBER,
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
            NSK,
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
    }
}
