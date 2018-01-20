using System;
using System.Collections.Generic;
using System.Text;

namespace Hunter.Models
{
    public enum Code
    {
        /// <summary> 成功
        /// </summary>
        Success = 0,

        /// <summary> 失败
        /// </summary>
        Fail = -1,

        /// <summary> 插入成功
        /// </summary>
        InsertSuccess = 101,

        /// <summary> 更新成功
        /// </summary>
        UpdateSuccess = 102,

        /// <summary> 删除成功
        /// </summary>
        DeleteSuccess = 103,

        /// <summary> 插入失败
        /// </summary>
        InsertFail = -101,

        /// <summary> 更新失败
        /// </summary>
        UpdateFail = -102,

        /// <summary> 删除失败
        /// </summary>
        DeleteFail = -103,

        /// <summary> 没找到
        /// </summary>
        NotFound = -2,

        /// <summary> 存在的
        /// </summary>
        Exist = -3,

        /// <summary> 空模型
        /// </summary>
        NullModel = -4,

        /// <summary> 空主键
        /// </summary>
        EmptyPrimaryKey = -5,

        /// <summary> 验证不通过
        /// </summary>
        Invalid = -6,
    }

}
