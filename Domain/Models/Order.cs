namespace Domain.Models
{
    public class Order
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; }
        public DateTimeOffset OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
