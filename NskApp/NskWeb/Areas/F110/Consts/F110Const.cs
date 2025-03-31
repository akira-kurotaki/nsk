using System.ComponentModel;

namespace NskWeb.Areas.F110.Consts
{
    public class F110Const
    {
        /// <summary>
        /// 再引受前処理 画面ID(D110010)
        /// </summary>
        public const string SCREEN_ID_D110010 = "D110010";

        /// <summary>
        /// 引受確定処理 画面ID(D110020)
        /// </summary>
        public const string SCREEN_ID_D110020 = "D110020";

        /// <summary>
        /// ユーザー権限
        /// </summary>
        public enum UserAuthority
        {
            /// <summary>権限なし</summary>
            [Description("権限なし")]
            None,
            /// <summary>支所権限</summary>
            [Description("支所権限")]
            Shisho,
            /// <summary>本所権限</summary>
            [Description("本所権限")]
            Honsho,
        }

        /// <summary>
        /// 画面権限
        /// </summary>
        public enum DispAuthority
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
        /// 引受確定処理対象
        /// </summary>
        public enum HikiukeKakuteiShoriTaisho
        {
            /// <summary>0:全件引受未確定</summary>
            [Description("全件引受未確定")]
            AllHikiukeMikakutei = 0,
            /// <summary>1:全件引受確定済み</summary>
            [Description("全件引受確定済み")]
            AllHikiukeKakuteiZumi,
            /// <summary>2:全件損害評価未確定</summary>
            [Description("全件損害評価未確定")]
            AllSongaiMikakutei,
            /// <summary>3:一部引受確定済み</summary>
            [Description("一部引受確定済み")]
            IchibuHikiukeKakuteiZumi,
            /// <summary>4:一部損害評価未確定</summary>
            [Description("一部損害評価未確定")]
            IchibuSongaiMikakutei,
            /// <summary>5:一部引受確定済みかつ一部損害評価未確定</summary>
            [Description("一部引受確定済みかつ一部損害評価未確定")]
            IchibuHikiukeKakuteiZumiAndIchibuSongaiMikakutei
        }
    }
}