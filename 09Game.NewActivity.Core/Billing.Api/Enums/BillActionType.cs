using System;
using System.Collections.Generic;
using System.Text;

namespace Billing.Api.Enums
{
    internal enum BillActionType
    {
        Query = 11,
        Pay = 12,
        Sub = 13,
        RollBack = 16
    }
}
