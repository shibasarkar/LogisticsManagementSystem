using MediatR;
using Microsoft.EntityFrameworkCore;
using Logistics.Infrastructure.Data;

namespace Logistics.Application.Orders.Queries;

// 1. Response DTO: A flat structure optimized for front-end consumption
public record OrderResponse(Guid Id, string CustomerId, decimal TotalAmount, string Status, DateTime CreatedAtUtc);

// 2. The Query: Contains the filter criteria (the identifier)
public record GetOrderByIdQuery(Guid Id) : IRequest<OrderResponse?>;

// 3. The Handler: Uses EF Core performance optimizations for fast reads
public class GetOrderByIdQueryHandler : IRequestHandler<GetOrderByIdQuery, OrderResponse?>
{
    private readonly ApplicationDbContext _context;

    public GetOrderByIdQueryHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<OrderResponse?> Handle(GetOrderByIdQuery request, CancellationToken cancellationToken)
    {
        return await _context.Orders
            .Where(o => o.Id == request.Id)
            // CRITICAL OPTIMIZATION: Disables tracking overhead because this query is read-only
            .AsNoTracking() 
            .Select(o => new OrderResponse(
                o.Id,
                o.CustomerId,
                o.TotalAmount,
                o.Status.ToString(),
                o.CreatedAtUtc))
            .FirstOrDefaultAsync(cancellationToken);
    }
}
