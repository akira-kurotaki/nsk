using System.ComponentModel;

namespace NskWeb.Areas.F109.Consts
{
    public class F109Const
    {
        /// <summary>
        /// 規模別分布状況データ作成設定 画面ID(D109020)
        /// </summary>
        public const string SCREEN_ID_D109020 = "D109020";


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
    }
}