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
    public class D000000ChangeNensanModel 
    {
        public D000000ChangeNensanModel() 
        {
            req = "";
            SKyosaiMokutekiCd = "";
        }
        public string req {  get; set; }
        public string SKyosaiMokutekiCd { get; set; }
    }
}