using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CoreLibrary.Core.Attributes
{
    /// <summary>
    /// ファイル削除属性
    /// </summary>
    /// <remarks>
    /// 作成日：2018/05/09
    /// 作成者：You En
    /// </remarks>
    public class DeleteFileAttribute : ActionFilterAttribute
    {
        public override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            if ((filterContext.Result as PhysicalFileResult) != null)
            {
                string filePath = (filterContext.Result as PhysicalFileResult).FileName;
                filterContext.HttpContext.Response.Body.Flush();
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
            }
        }
    }
}