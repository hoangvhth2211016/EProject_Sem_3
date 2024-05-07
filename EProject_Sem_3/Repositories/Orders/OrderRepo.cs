using AutoMapper;
using EProject_Sem_3.Exceptions;
using EProject_Sem_3.Models;
using EProject_Sem_3.Models.Books;
using EProject_Sem_3.Models.OrderDetails;
using EProject_Sem_3.Models.Orders;
using EProject_Sem_3.Repositories.OrdersDetail;
using Microsoft.EntityFrameworkCore;

namespace EProject_Sem_3.Repositories.Orders;

public class OrderRepo : IOrderRepo
{

    private readonly AppDbContext _context;

    private readonly IMapper _mapper;

    private readonly IOrderDetailRepo _orderDetailRepo;

    public OrderRepo(AppDbContext context, IMapper mapper, IOrderDetailRepo orderDetailRepo)
    {
        _context = context;
        _mapper = mapper;
        _orderDetailRepo = orderDetailRepo;
    }

    public async Task<List<OrderRes>> GetAllOrders()
    { 
        var orders = await _context.Orders.ToListAsync();
        
        // add list OrdersDetail for orders
        foreach (var order in orders)
        {
            var orderDetails = _context.OrderDetails
                .Where(od => od.OrderId == order.Id)
                .ToList();
            order.OrderDetails = orderDetails;
        }

        return _mapper.Map<List<OrderRes>>(orders);
    }
    
    public async Task<OrderRes> GetOrder(int orderId)
    { 
       var order = await _context.Orders.FindAsync(orderId)
               ??
               throw new NotFoundException("Order not found");
       
       // add OrderDetail for order
       var orderDetails = _context.OrderDetails
           .Where(od => od.OrderId == order.Id)
           .ToList();
       
       return _mapper.Map<OrderRes>(order);
    }


    
    public async Task<OrderRes> CreateOrder(OrderDto dto)
    {
        // Check if dto.UserId is not null and doesn't exist in the database
        if (dto.UserId != null && !_context.Users.Any(u => u.Id == dto.UserId))
        {
            throw new NotFoundException("UserId not found in the database.");
        }
        
        // OrderDto -> Order
        var order = _mapper.Map<Order>(dto);
        
        order.Status = OrderStatus.Process;
        
        // save order to db
        _context.Orders.Add(order);
        await _context.SaveChangesAsync();
        
        // save orderDetail to db
        _orderDetailRepo.CreateOrderDetail(order.Id, dto);
        
        // order -> orderRes
        var orderRes = _mapper.Map<OrderRes>(order);
        return orderRes;
    }

    public async Task<OrderRes> UpdateOrder(int orderId, UpdateOrderDto dto)
    {
        var orderCurrent =  await _context.Orders.FindAsync(orderId) ?? throw new NotFoundException("Order Not Found");
        
        if (orderCurrent.Status == OrderStatus.Process && 
            dto.Status is OrderStatus.Completed 
                        or OrderStatus.Return 
                        or OrderStatus.Returned)
        {
            throw new BadRequestException("Cannot change status " + orderCurrent.Status + " to " + dto.Status);
        }
        
        if (orderCurrent.Status == OrderStatus.Shipping &&
                 dto.Status is OrderStatus.Process 
                            or OrderStatus.Returned 
                            or OrderStatus.Cancel)
        {
            throw new BadRequestException("Cannot change status " + orderCurrent.Status + " to " + dto.Status);
        }
        
        if (orderCurrent.Status == OrderStatus.Completed &&
                 dto.Status is not OrderStatus.Completed )
        {
            throw new BadRequestException("Cannot change status " + orderCurrent.Status + " to " + dto.Status);
        }
        
        if (orderCurrent.Status == OrderStatus.Return &&
            dto.Status is not OrderStatus.Returned )
        {
            throw new BadRequestException("Cannot change status " + orderCurrent.Status + " to " + dto.Status);
        }
        
        if (orderCurrent.Status == OrderStatus.Returned &&
            dto.Status is not OrderStatus.Returned )
        {
            throw new BadRequestException("Cannot change status " + orderCurrent.Status + " to " + dto.Status);
        }
        
        if (orderCurrent.Status == OrderStatus.Cancel &&
            dto.Status is OrderStatus.Completed 
                        or OrderStatus.Returned 
                        or OrderStatus.Return)
        {
            throw new BadRequestException("Cannot change status " + orderCurrent.Status + " to " + dto.Status);
        }
        
        // Check if dto.UserId is not null and doesn't exist in the database
        if (dto.UserId != null && !_context.Users.Any(u => u.Id == dto.UserId))
        {
            throw new NotFoundException("UserId not found in the database.");
        }
        
      
        // OrderDto -> Order
        _mapper.Map(dto, orderCurrent);
        
        // update order to db
        _context.Entry(orderCurrent).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        
        // add OrderDetail for order
        var orderDetails = _context.OrderDetails
            .Where(od => od.OrderId == orderCurrent.Id)
            .ToList();
        orderCurrent.OrderDetails = orderDetails;
        
        // order -> orderRes
        var orderRes = _mapper.Map<OrderRes>(orderCurrent);
        return orderRes;
        
    }

    public async Task<string> DeleteOrder(int orderId)
    {
        var order = await _context.Orders.FindAsync(orderId) ?? throw new NotFoundException("Order Not Found");
        
        _context.Orders.Remove(order);
        await _context.SaveChangesAsync();

        return "The order has been deleted";
    }
}