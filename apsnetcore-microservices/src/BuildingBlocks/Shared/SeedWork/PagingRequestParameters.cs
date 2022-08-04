using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.SeedWork
{
    // Có 1 số thông số mặc định cho việc phân trang, ở params có thể không cần viết lại
    public class PagingRequestParameters
    {
        private const int maxPageSize = 50;

        private int _pageNumber = 1;

        private int _pageSize = 10;

        public int PageNumber
        {
            get => _pageNumber;
            set => _pageNumber = value < 1 ? 1 : value;
        }

        public int PageSize
        {
            get => _pageSize;
            set
            {
                if (value > 0)
                    _pageSize = value > maxPageSize ? maxPageSize : value;
            }
        }

        public string OrderBy { get; set; }
    }
}
