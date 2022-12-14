/***********************************************************************
 *            Project: CoreCms
 *        ProjectName: 核心内容管理系统                                
 *                Web: https://www.corecms.net                      
 *             Author: 大灰灰                                          
 *              Email: jianweie@163.com                                
 *         CreateTime: 2021/1/31 21:45:10
 *        Description: 暂无
 ***********************************************************************/

using System.Threading.Tasks;
using CoreCms.Net.Model.Entities;
using CoreCms.Net.Model.FromBody;
using CoreCms.Net.Model.ViewModels.UI;

namespace CoreCms.Net.IServices
{
    /// <summary>
    ///     商品类型 服务工厂接口
    /// </summary>
    public interface ICoreCmsGoodsTypeServices : IBaseServices<CoreCmsGoodsType>
    {
        /// <summary>
        ///     更新参数
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<AdminUiCallBack> UpdateParametersAsync(FMUpdateArrayIntDataByIntId entity);


        /// <summary>
        ///     更新属性
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<AdminUiCallBack> UpdateTypesAsync(FMUpdateArrayIntDataByIntId entity);

        #region 重写增删改查操作===========================================================

        /// <summary>
        ///     事务重写异步插入方法
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<AdminUiCallBack> InsertAsync(FmGoodsTypeInsert entity);


        ///// <summary>
        ///// 重写异步更新方法方法
        ///// </summary>
        ///// <param name="entity"></param>
        ///// <returns></returns>
        //new Task<AdminUiCallBack> UpdateAsync(CoreCmsGoodsType entity);


        ///// <summary>
        ///// 重写异步更新方法方法
        ///// </summary>
        ///// <param name="entity"></param>
        ///// <returns></returns>
        //new Task<AdminUiCallBack> UpdateAsync(List<CoreCmsGoodsType> entity);


        /// <summary>
        ///     重写删除指定ID的数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<AdminUiCallBack> DeleteByIdAsync(int id);


        ///// <summary>
        ///// 重写删除指定ID集合的数据(批量删除)
        ///// </summary>
        ///// <param name="ids"></param>
        ///// <returns></returns>
        //new Task<AdminUiCallBack> DeleteByIdsAsync(int[] ids);

        #endregion
    }
}