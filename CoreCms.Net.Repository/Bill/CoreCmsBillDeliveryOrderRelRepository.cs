/***********************************************************************
 *            Project: CoreCms
 *        ProjectName: 核心内容管理系统                                
 *                Web: https://www.corecms.net                      
 *             Author: 大灰灰                                          
 *              Email: jianweie@163.com                                
 *         CreateTime: 2021/1/31 21:45:10
 *        Description: 暂无
 ***********************************************************************/

using CoreCms.Net.IRepository;
using CoreCms.Net.IRepository.UnitOfWork;
using CoreCms.Net.Model.Entities;

namespace CoreCms.Net.Repository
{
    /// <summary>
    ///     发货单订单关联表 接口实现
    /// </summary>
    public class CoreCmsBillDeliveryOrderRelRepository : BaseRepository<CoreCmsBillDeliveryOrderRel>,
        ICoreCmsBillDeliveryOrderRelRepository
    {
        public CoreCmsBillDeliveryOrderRelRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}