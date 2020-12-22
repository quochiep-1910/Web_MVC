using Models.EF;

namespace Models.DAO
{
    public class OrderDAO
    {
        private Web_MVC db = null;

        public OrderDAO()
        {
            db = new Web_MVC();
        }

        public long Insert(Order order)
        {
            db.Orders.Add(order);
            db.SaveChanges();
            return order.ID;
        }
    }
}