using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Domain.Exceptions
{
    public class EntityNotFoundException : ApplicationException
    {
        // Nếu không tìm thấy đơn hàng thay vì trả về Exception thì sẽ trả về Entity not found Exception.
        public EntityNotFoundException(string entity, object key) :
            base($"Entity \"{entity}\" ({key}) was not found.")
        {

        }
    }
}
