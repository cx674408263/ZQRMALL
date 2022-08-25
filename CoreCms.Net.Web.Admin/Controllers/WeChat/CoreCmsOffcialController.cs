using CoreCms.Net.Filter;
using CoreCms.Net.Model.ViewModels.UI;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace CoreCms.Net.Web.Admin.Controllers.WeChat
{

    /// <summary>
    ///微信公众号配置表
    /// </summary>
    [Description("微信公众号配置表")]
    [Route("api/[controller]/[action]")]
    [ApiController]
    [RequiredErrorForAdmin]
    [Authorize]
    public class CoreCmsOffcialController : ControllerBase
    {
        #region 公众号底部菜单============================================================

        // POST: Api/CoreCmsOffcial/GetIndex
        /// <summary>
        /// 菜单首页数据
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Description("菜单首页数据")]
        public JsonResult GetIndex()
        {
            //返回数据
            var jm = new AdminUiCallBack { code = 0 };
            return new JsonResult(jm);
        }

        #endregion
    }
}
