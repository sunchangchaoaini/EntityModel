﻿
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

using Gboxt.Common.DataModel;
using Gboxt.Common.DataModel.BusinessLogic;
using Gboxt.Common.DataModel.SqlServer;
using Gboxt.Common.SystemModel.DataAccess;

namespace Gboxt.Common.SystemModel.BusinessLogic
{
    /// <summary>
    /// 组织机构
    /// </summary>
    public sealed partial class OrganizationBusinessLogic : BusinessLogicBase<OrganizationData,OrganizationDataAccess>
    {
        /// <summary>
        ///     保存前的操作
        /// </summary>
        /// <param name="data">数据</param>
        /// <param name="isAdd">是否为新增</param>
        /// <returns>如果为否将阻止后续操作</returns>
        protected override bool OnSaving(OrganizationData data, bool isAdd)
        {
            return true;
        }

        /// <summary>
        ///     保存完成后的操作
        /// </summary>
        /// <param name="data">数据</param>
        /// <param name="isAdd">是否为新增</param>
        /// <returns>如果为否将阻止后续操作</returns>
        protected override bool OnSaved(OrganizationData data, bool isAdd)
        {
             return true;
        }
    }
}
