using System;
using System.Collections.Generic;
using System.Text;

namespace Hunter.Models
{
    public class EditResult : Result, IData<bool>
    {
        public static EditResult CreateInsertResult(int flag)
        {
            return CreateInsertResult(flag > 0);
        }

        public static EditResult CreateInsertResult(bool flag)
        {
            return new EditResult()
            {
                Code = flag ? Code.InsertSuccess : Code.InsertFail,
                Data = flag
            };
        }

        public static EditResult CreateUpdateResult(int flag)
        {
            return CreateUpdateResult(flag > 0);
        }

        public static EditResult CreateUpdateResult(bool flag)
        {
            return new EditResult()
            {
                Code = flag ? Code.UpdateSuccess : Code.UpdateFail,
                Data = flag
            };
        }

        public static EditResult CreateDeleteResult(int flag)
        {
            return CreateDeleteResult(flag > 0);
        }

        public static EditResult CreateDeleteResult(bool flag)
        {
            return new EditResult()
            {
                Code = flag ? Code.DeleteSuccess : Code.DeleteFail,
                Data = flag
            };
        }

        public bool Data { get; set; }


    }
}
