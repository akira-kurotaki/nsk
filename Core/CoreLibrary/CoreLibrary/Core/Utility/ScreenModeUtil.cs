using CoreLibrary.Core.Consts;

namespace CoreLibrary.Core.Utility
{
    /// <summary>
    /// 画面表示モードユーティリティ
    /// </summary>
    /// <remarks>
    /// 作成日：2018/03/07
    /// 作成者：MATSUBAYASHI Atsushi
    /// </remarks>
    public class ScreenModeUtil
    {
        /// <summary>
        /// 画面表示モードを返す
        /// 
        /// </summary>
        /// <returns></returns>
        //public static string GetScreenMode()
        public static string GetScreenMode(HttpContext context)
        {
            string sm = String.Empty;
            if (SessionUtil.Get<CoreConst.ScreenMode?>(CoreConst.SESS_SCREEN_MODE, context) != null)
            {
                if ((SessionUtil.Get<CoreConst.ScreenMode>(CoreConst.SESS_SCREEN_MODE, context)) == CoreConst.ScreenMode.PC)
                {
                    sm = CoreConst.SCREEN_MODE_PC_HASH;
                }
                else if ((SessionUtil.Get<CoreConst.ScreenMode>(CoreConst.SESS_SCREEN_MODE, context)) == CoreConst.ScreenMode.Tablet)
                {
                    sm = CoreConst.SCREEN_MODE_TABLET_HASH;
                }
            }
            return sm;
        }

        /// <summary>
        /// ファイル一時保管システムの画面表示モードを返す
        /// 
        /// </summary>
        /// <returns></returns>
        //public static string GetExcelUploadScreenMode()
        public static string GetExcelUploadScreenMode(HttpContext context)
        {
            string sm = String.Empty;
            if (SessionUtil.Get<CoreConst.ScreenMode?>(CoreConst.SESS_SCREEN_MODE, context) != null)
            {
                if ((SessionUtil.Get<CoreConst.ScreenMode>(CoreConst.SESS_SCREEN_MODE, context)) == CoreConst.ScreenMode.PC)
                {
                    sm = "pc";
                }
                else if ((SessionUtil.Get<CoreConst.ScreenMode>(CoreConst.SESS_SCREEN_MODE, context)) == CoreConst.ScreenMode.Tablet)
                {
                    sm = "tb";
                }
            }
            return sm;
        }
    }
}