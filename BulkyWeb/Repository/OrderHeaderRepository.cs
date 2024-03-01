using BulkyWeb.Data;
using BulkyWeb.Models;
using BulkyWeb.Repository.IRepository;

namespace BulkyWeb.Repository
{
    public class OrderHeaderRepository : Repository<OrderHeader>, IOrderHeaderRepository
    {
        private ApplicationDbContext _db;
        public OrderHeaderRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(OrderHeader obj)
        {
            _db.OrderHeaders.Update(obj);
        }
        //adding implementation for Updatestatus & UpdateStripePaymntId methods here
        public void UpdateStatus(int id,string orderStatus, string? paymentStatus = null)
        {
            var OrderFromDb = _db.OrderHeaders.FirstOrDefault(u => u.Id == id);
            if (OrderFromDb != null)
            {
                OrderFromDb.OrderStatus = orderStatus;
                if (!string.IsNullOrEmpty(paymentStatus))
                {
                    OrderFromDb.PaymentStatus = paymentStatus;
                }
            }
        }
        public void UpdateStripePaymentId(int id, string sessionId, string paymentIntentId)
        {
            var OrderFromDb = _db.OrderHeaders.FirstOrDefault(u => u.Id == id);
                if (!string.IsNullOrEmpty(sessionId))
                {
                    OrderFromDb.SessionId = sessionId;
                }
                if (!string.IsNullOrEmpty(paymentIntentId))
                {
                    OrderFromDb.PaymentIntentId = paymentIntentId;
                    OrderFromDb.PaymentDate = DateTime.Now;
                }
        }

    }
}
