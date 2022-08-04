﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Domain.Enums
{
    public enum EOrderStatus
    {
        New = 1, // start with 1, 0 is used for filter All = 0
        Pending, // order os pending, not any activities for a period time.
        Paid, // order is paid
        Shipping, // order is on the shippinf
        Fulfilled // order is fulfilled
    }
}
