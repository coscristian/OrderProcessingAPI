using OrderProcessing.Application.Services.OrderService.Dto;
using OrderProcessing.Application.Services.OrderService.Interfaces;
using OrderProcessing.Application.Services.ProductService.Interfaces;
using OrderProcessing.Application.Services.CustomerService.Interfaces;
using OrderProcessing.Application.Services.Discounts;
using OrderProcessing.Domain.Aggregates.OrderAggregate;

namespace OrderProcessing.Application.Services.OrderService;

public class OrderService : IOrderService
{
    private readonly IOrderRepository _orderRepository;
    private readonly IProductRepository _productRepository;
    private readonly ICustomerRepository _customerRepository;
    private readonly DiscountService _discountService;

    public OrderService(
        IOrderRepository orderRepository,
        IProductRepository productRepository,
        ICustomerRepository customerRepository,
        DiscountService discountService)
    {
        _orderRepository = orderRepository;
        _productRepository = productRepository;
        _customerRepository = customerRepository;
        _discountService = discountService;
    }

    public async Task<OrderResponse> CreateAsync(CreateOrderRequest request, CancellationToken cancellationToken = default)
    {
        var customer = await _customerRepository.GetByIdAsync(request.CustomerId, cancellationToken);

        if (customer is null)
            throw new NotFoundException($"Customer {request.CustomerId} not found.");

        decimal subtotal = 0m;
        var products = new Dictionary<int, OrderProcessing.Domain.Aggregates.ProductAggregate.Product>();

        foreach (var item in request.Items)
        {
            var product = await _productRepository.GetByIdAsync(item.ProductId);

            if (product is null)
                throw new NotFoundException($"Product {item.ProductId} not found.");

            if (item.Quantity > product.StockQuantity)
                throw new ConflictException($"Product {item.ProductId} does not have enough stock.");

            products[item.ProductId] = product;

            subtotal += product.Price * item.Quantity;
        }

        var order = Order.Create(request.CustomerId);

        foreach (var item in request.Items)
        {
            var orderItem = OrderItem.Create(item.ProductId, item.Quantity);
            order.AddItem(orderItem);
        }

        foreach (var kv in products)
        {
            var p = kv.Value;
            var requested = request.Items.First(i => i.ProductId == kv.Key).Quantity;
            p.DecreaseStock(requested);
        }

        var discount = _discountService.CalculateDiscount(customer.Tier, subtotal);
        var total = subtotal - discount;

        order.SetTotal(total);

        await _orderRepository.AddAsync(order, cancellationToken);

        foreach (var p in products.Values)
        {
            _productRepository.Update(p);
        }

        await _orderRepository.SaveChangesAsync(cancellationToken);

        return MapToResponse(order);
    }

    public async Task<OrderResponse?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var order = await _orderRepository.GetByIdAsync(id, cancellationToken);

        if (order is null)
            return null;

        return MapToResponse(order);
    }

    public async Task<PagedOrdersResponse> GetPagedAsync(int page, int pageSize, int? customerId, DateTime? from, DateTime? to, CancellationToken cancellationToken = default)
    {
        var items = await _orderRepository.GetPagedAsync(page, pageSize, customerId, from, to, cancellationToken);
        var totalCount = await _orderRepository.CountAsync(customerId, from, to, cancellationToken);

        var responses = items.Select(o => MapToResponse(o)).ToList();

        var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

        return new PagedOrdersResponse(responses, page, pageSize, totalCount, totalPages);
    }

    private static OrderResponse MapToResponse(Order order)
    {
        var items = order.Items.Select(i => new OrderItemResponse(i.ProductId, i.Quantity)).ToList();

        return new OrderResponse(order.Id, order.CustomerId, order.CreatedAt, order.Total, items);
    }
}