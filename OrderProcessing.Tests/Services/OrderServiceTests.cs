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

        mockProductRepo.Setup(p => p.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(product);

        mockCustomerRepo.Setup(c => c.GetByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(customer);

        var discountService = new DiscountService(new IDiscountStrategy[] { new RegularDiscountStrategy(), new PremiumDiscountStrategy(), new VipDiscountStrategy() });

        var service = new OrderService(
            mockOrderRepo.Object,
            mockProductRepo.Object,
            mockCustomerRepo.Object,
            discountService);

        var request = new CreateOrderRequest(1, new List<OrderItemRequest> { new OrderItemRequest(1, 5) });

        await Assert.ThrowsAsync<InvalidOperationException>(() => service.CreateAsync(request));

        mockOrderRepo.Verify(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        Assert.Equal(2, product.StockQuantity);
    }
}