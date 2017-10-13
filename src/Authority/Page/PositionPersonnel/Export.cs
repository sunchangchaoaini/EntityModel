﻿/*此标记表明此文件可被设计器更新,如果不允许此操作,请删除此行代码.design by:agebull designer date:2017/6/27 9:01:57*/
using Gboxt.Common.DataModel;
using Gboxt.Common.DataModel.MySql;
using Gboxt.Common.WebUI;

using Agebull.SystemAuthority.Organizations.DataAccess;

namespace Agebull.SystemAuthority.Organizations.PositionPersonnelPage
{
    /// <summary>
    /// 人员职位设置
    /// </summary>
    public class ExportAction : ExportPageBase<PositionPersonnelData, PositionPersonnelDataAccess>
    {
        /// <summary>
        /// 导出表名称
        /// </summary>
        protected override string Name => "人员职位设置";

        /// <summary>
        /// 当前数据筛选器
        /// </summary>
        /// <returns></returns>
        protected override LambdaItem<PositionPersonnelData> GetFilter()
        {
            var filter = new LambdaItem<PositionPersonnelData>
            {
                Root = p => p.DataState <= DataStateType.Discard
            };
            var keyWord = GetArg("keyWord");
            if (!string.IsNullOrEmpty(keyWord))
            {
                filter.AddAnd(p => p.Personnel.Contains(keyWord) || p.Appellation.Contains(keyWord) || p.Position.Contains(keyWord) || p.Role.Contains(keyWord) || p.Tel.Contains(keyWord) || p.Mobile.Contains(keyWord) || p.Organization.Contains(keyWord) || p.Department.Contains(keyWord) || p.Memo.Contains(keyWord));
            }
            return filter;
        }
    }
}