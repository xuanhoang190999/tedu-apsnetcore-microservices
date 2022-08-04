using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.SeedWork
{
    // Bắn ra những lỗi không thể xác định được
    public class ApiErrorResult<T> : ApiResult<T>
    {
        public ApiErrorResult() : this("Something wrong happened. Please try again later")
        {
        }

        public ApiErrorResult(string message)
            : base(false, message)
        {
        }

        public ApiErrorResult(List<string> errors)
            : base(false)
        {
            Errors = errors;
        }

        public List<string> Errors { set; get; }
    }
}
