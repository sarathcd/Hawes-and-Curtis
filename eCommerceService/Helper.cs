using System.Collections.Generic;
using System.Linq;

using Common.Enum;
using Dal;


namespace eCommerceService
{
    public static class Helper
    {
        public static int GetAvailableProductQuantity(this IEnumerable<ProductTransactionQuantity> productQuantities)
        {
            return productQuantities.Sum(pq =>
            {
                if (pq.TransactionTypeId == (int)TransactionType.Added || pq.TransactionTypeId == (int)TransactionType.CancelledRequest)
                    return pq.Quantity;

                if (pq.TransactionTypeId == (int)TransactionType.Purchased || pq.TransactionTypeId == (int)TransactionType.Requested)
                    return -1 * pq.Quantity;
                return 0;
            });
        }
    }
}
