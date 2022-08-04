using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.SeedWork
{
    // Lấy tất cả các response của api về đúng 1 mẫu
    public class ApiSuccessResult<T> : ApiResult<T>
    {
        public ApiSuccessResult(T data) : base(true, data, "Success")
        {
        }

        public ApiSuccessResult(T data, string message) : base(true, data, message)
        {
        }
    }
}
