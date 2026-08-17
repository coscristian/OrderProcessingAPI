using Moq;
using OrderProcessing.Application.Services.OrderService;
using OrderProcessing.Application.Services.OrderService.Interfaces;
using OrderProcessing.Application.Services.ProductService.Interfaces;
using OrderProcessing.Application.Services.CustomerService.Interfaces;
using OrderProcessing.Application.Services.Discounts;
using OrderProcessing.Domain.Aggregates.ProductAggregate;
using OrderProcessing.Domain.Aggregates.CustomerAggregate;
using OrderProcessing.Application.Services.OrderService.Dto;

namespace OrderProcessing.Tests.Services;

public class OrderServiceTests
{
    [Fact]
    public async Task CreateAsync_HappyPath_CreatesOrderAndDecreasesStock()
    {
        var product = Product.Create("Prod", 100m, 10);
        var customer = Customer.Create("Cust", CustomerTier.Premium);

        var mockOrderRepo = new Mock<IOrderRepository>();
        var mockProductRepo = new Mock<IProductRepository>();
        var mockCustomerRepo = new Mock<ICustomerRepository>();

        mockProductRepo.Setup(p => p.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(product);

        mockCustomerRepo.Setup(c => c.GetByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(customer);

        mockOrderRepo.Setup(r => r.AddAsync(It.IsAny<Domain.Aggregates.OrderAggregate.Order>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        mockOrderRepo.Setup(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var discountService = new DiscountService(new IDiscountStrategy[] { new RegularDiscountStrategy(), new PremiumDiscountStrategy(), new VipDiscountStrategy() });

        var service = new OrderService(
            mockOrderRepo.Object,
            mockProductRepo.Object,
            mockCustomerRepo.Object,
            discountService);

        var request = new CreateOrderRequest(1, new List<OrderItemRequest> { new OrderItemRequest(1, 2) });

        var result = await service.CreateAsync(request);

        Assert.Equal(180m, result.Total);
        Assert.Equal(8, product.StockQuantity);

        mockOrderRepo.Verify(r => r.AddAsync(It.IsAny<Domain.Aggregates.OrderAggregate.Order>(), It.IsAny<CancellationToken>()), Times.Once);
        mockOrderRepo.Verify(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task CreateAsync_InsufficientStock_ThrowsAndDoesNotPersist()
    {
        var product = Product.Create("Prod", 50m, 2);
        var customer = Customer.Create("Cust", CustomerTier.Regular);

        var mockOrderRepo = new Mock<IOrderRepository>();
        var mockProductRepo = new Mock<IProductRepository>();
        var mockCustomerRepo = new Mock<ICustomerRepository>();

        mockProductRepo
            .Setup(p => p.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(product);

        mockCustomerRepo
            .Setup(c => c.GetByIdAsync(
                It.IsAny<int>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(customer);

        var discountService = new DiscountService(
            new IDiscountStrategy[]
            {
                new RegularDiscountStrategy(),
                new PremiumDiscountStrategy(),
                new VipDiscountStrategy()
            });

        var service = new OrderService(
            mockOrderRepo.Object,
            mockProductRepo.Object,
            mockCustomerRepo.Object,
            discountService);

        var request = new CreateOrderRequest(
            1,
            new List<OrderItemRequest>
            {
                new OrderItemRequest(1, 5)
            });

        var exception = await Assert.ThrowsAsync<ConflictException>(
            () => service.CreateAsync(request));

        Assert.Equal(
            "Product 1 does not have enough stock.",
            exception.Message);

        mockOrderRepo.Verify(
            r => r.AddAsync(
                It.IsAny<Domain.Aggregates.OrderAggregate.Order>(),
                It.IsAny<CancellationToken>()),
            Times.Never);

        mockOrderRepo.Verify(
            r => r.SaveChangesAsync(
                It.IsAny<CancellationToken>()),
            Times.Never);

        Assert.Equal(2, product.StockQuantity);
    }
    
    [Fact]
    public async Task CreateAsync_CustomerNotFound_ThrowsAndDoesNotPersist()
    {
        var mockOrderRepo = new Mock<IOrderRepository>();
        var mockProductRepo = new Mock<IProductRepository>();
        var mockCustomerRepo = new Mock<ICustomerRepository>();

        mockCustomerRepo
            .Setup(c => c.GetByIdAsync(
                It.IsAny<int>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync((Customer?)null);

        var discountService = new DiscountService(
            new IDiscountStrategy[]
            {
                new RegularDiscountStrategy(),
                new PremiumDiscountStrategy(),
                new VipDiscountStrategy()
            });

        var service = new OrderService(
            mockOrderRepo.Object,
            mockProductRepo.Object,
            mockCustomerRepo.Object,
            discountService);

        var request = new CreateOrderRequest(
            999,
            new List<OrderItemRequest>
            {
                new OrderItemRequest(1, 2)
            });

        var exception = await Assert.ThrowsAsync<NotFoundException>(
            () => service.CreateAsync(request));

        Assert.Equal("Customer 999 not found.", exception.Message);

        mockProductRepo.Verify(
            r => r.GetByIdAsync(It.IsAny<int>()),
            Times.Never);

        mockOrderRepo.Verify(
            r => r.AddAsync(
                It.IsAny<Domain.Aggregates.OrderAggregate.Order>(),
                It.IsAny<CancellationToken>()),
            Times.Never);

        mockOrderRepo.Verify(
            r => r.SaveChangesAsync(It.IsAny<CancellationToken>()),
            Times.Never);
    }
    
    [Fact]
    public async Task CreateAsync_ProductNotFound_ThrowsAndDoesNotPersist()
    {
        var customer = Customer.Create("Cust", CustomerTier.Regular);

        var mockOrderRepo = new Mock<IOrderRepository>();
        var mockProductRepo = new Mock<IProductRepository>();
        var mockCustomerRepo = new Mock<ICustomerRepository>();

        mockCustomerRepo
            .Setup(c => c.GetByIdAsync(
                It.IsAny<int>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(customer);

        mockProductRepo
            .Setup(p => p.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync((Product?)null);

        var discountService = new DiscountService(
            new IDiscountStrategy[]
            {
                new RegularDiscountStrategy(),
                new PremiumDiscountStrategy(),
                new VipDiscountStrategy()
            });

        var service = new OrderService(
            mockOrderRepo.Object,
            mockProductRepo.Object,
            mockCustomerRepo.Object,
            discountService);

        var request = new CreateOrderRequest(
            1,
            new List<OrderItemRequest>
            {
                new OrderItemRequest(999, 2)
            });

        var exception = await Assert.ThrowsAsync<NotFoundException>(
            () => service.CreateAsync(request));

        Assert.Equal(
            "Product 999 not found.",
            exception.Message);

        mockOrderRepo.Verify(
            r => r.AddAsync(
                It.IsAny<Domain.Aggregates.OrderAggregate.Order>(),
                It.IsAny<CancellationToken>()),
            Times.Never);

        mockOrderRepo.Verify(
            r => r.SaveChangesAsync(
                It.IsAny<CancellationToken>()),
            Times.Never);
    }
    
    [Fact]
    public async Task CreateAsync_InvalidQuantity_ThrowsAndDoesNotPersist()
    {
        var product = Product.Create("Prod", 100m, 10);
        var customer = Customer.Create("Cust", CustomerTier.Regular);

        var mockOrderRepo = new Mock<IOrderRepository>();
        var mockProductRepo = new Mock<IProductRepository>();
        var mockCustomerRepo = new Mock<ICustomerRepository>();

        mockCustomerRepo
            .Setup(c => c.GetByIdAsync(
                It.IsAny<int>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(customer);

        mockProductRepo
            .Setup(p => p.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(product);

        var discountService = new DiscountService(
            new IDiscountStrategy[]
            {
                new RegularDiscountStrategy(),
                new PremiumDiscountStrategy(),
                new VipDiscountStrategy()
            });

        var service = new OrderService(
            mockOrderRepo.Object,
            mockProductRepo.Object,
            mockCustomerRepo.Object,
            discountService);

        var request = new CreateOrderRequest(
            1,
            new List<OrderItemRequest>
            {
                new OrderItemRequest(1, 0)
            });

        var exception = await Assert.ThrowsAsync<ArgumentException>(
            () => service.CreateAsync(request));

        Assert.Equal(
            "Quantity must be greater than zero. (Parameter 'quantity')",
            exception.Message);

        mockOrderRepo.Verify(
            r => r.AddAsync(
                It.IsAny<Domain.Aggregates.OrderAggregate.Order>(),
                It.IsAny<CancellationToken>()),
            Times.Never);

        mockOrderRepo.Verify(
            r => r.SaveChangesAsync(
                It.IsAny<CancellationToken>()),
            Times.Never);

        Assert.Equal(10, product.StockQuantity);
    }
}