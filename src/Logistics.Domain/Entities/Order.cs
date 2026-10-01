namespace Logistics.Domain.Entities;

public enum OrderStatus
{
    Pending,
    Processing,
    Shipped,
    Delivered,
    Cancelled
}

public class Order
{
    // Private setter ensures properties can only be altered via valid domain methods
    public Guid Id { get; private set; }
    public string CustomerId { get; private set; } = string.Empty;
    public decimal TotalAmount { get; private set; }
    public OrderStatus Status { get; private set; }
    public DateTime CreatedAtUtc { get; private set; }

    // Private constructor for EF Core materialization
    private Order() { }

    // Factory method ensures an Order can never be initialized in an invalid state
    public static Order Create(string customerId, decimal totalAmount)
    {
        if (string.IsNullOrWhiteSpace(customerId))
            throw new ArgumentException("Customer ID cannot be empty.", nameof(customerId));

        if (totalAmount <= 0)
            throw new ArgumentException("Total amount must be greater than zero.", nameof(totalAmount));

        return new Order
        {
            Id = Guid.NewGuid(),
            CustomerId = customerId,
            TotalAmount = totalAmount,
            Status = OrderStatus.Pending,
            CreatedAtUtc = DateTime.UtcNow
        };
    }

    // Business Logic: Rules governing state transitions
    public void UpdateStatus(OrderStatus newStatus)
    {
        if (Status == OrderStatus.Cancelled || Status == OrderStatus.Delivered)
            throw new InvalidOperationException($"Cannot transition status from {Status} to {newStatus}.");

        Status = newStatus;
    }
}
