using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.ActionConstraints;

namespace CoreLibrary.Core.Attributes
{
    /// <summary>
    /// Button属性
    /// </summary>
    /// <remarks>
    /// 作成日：2017/12/1
    /// 作成者：MATSUBAYSHI Atsushi
    /// </remarks>
    public class ButtonAttribute : ActionMethodSelectorAttribute
    {
        // アクションメソッド付加時に設定したボタン名を保存
        public string ButtonName { get; set; }

        public override bool IsValidForRequest(RouteContext routeContext, ActionDescriptor action)
        {
            return routeContext.HttpContext.Request.HasFormContentType &&
                routeContext.HttpContext.Request.Form.Keys.Contains(ButtonName);
        }
    }
}