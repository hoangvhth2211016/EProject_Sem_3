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

    public async Task<PageOrderRes> GetAllOrders(int page, int pageSize)
    { 
       
        var orders = await _context.Orders
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
        
        
        foreach (var order in orders)
        {
            var orderDetails = _context.OrderDetails
                .Where(od => od.OrderId == order.Id)
                .ToList();
            
            foreach (var orderDetail in orderDetails)
            {
               
                // add list book for order detail
                var book = await _context.Books.FindAsync(orderDetail.BookId)??
                            throw new NotFoundException("Book not found");
                orderDetail.Book = book;
                
                // add list bookImages for Book
                var bookImages = _context.BookImages
                    .Where(b =>  b.BookId== book.Id)
                    .ToList();
                book.BookImages = bookImages;

            }
            
            // add list OrdersDetail for orders
            order.OrderDetails = orderDetails;
            
        }

        // list Orders --> PageOrder
        var totalOrders = await _context.Orders.CountAsync();
        var totalPage = (int)Math.Ceiling((double)totalOrders / pageSize);
        
        var pageOrderRes = new PageOrderRes
        {
            Orders = _mapper.Map<List<OrderRes>>(orders),
            Page = page,
            TotalPage = totalPage,
            TotalOrders = totalOrders
        };

        return pageOrderRes;
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
        
       
        
        // order -> orderRes
        var orderRes = _mapper.Map<OrderRes>(order);
        return orderRes;
    }

    public async Task<string> UpdateOrderStatus(int orderId, OrderStatus newStatus)
    {
        // find order with orderId
        var orderCurrent =  await _context.Orders.FindAsync(orderId) ?? throw new NotFoundException("Order Not Found");
        
        // check Status valid
        if (orderCurrent.Status == OrderStatus.Process && 
            newStatus is OrderStatus.Completed 
                        or OrderStatus.Return 
                        or OrderStatus.Returned)
        {
            throw new BadRequestException("Cannot change status " + orderCurrent.Status + " to " + newStatus);
        }
        
        if (orderCurrent.Status == OrderStatus.Shipping &&
            newStatus is OrderStatus.Process 
                            or OrderStatus.Returned 
                            or OrderStatus.Cancel)
        {
            throw new BadRequestException("Cannot change status " + orderCurrent.Status + " to " + newStatus);
        }
        
        if (orderCurrent.Status == OrderStatus.Completed &&
            newStatus is not OrderStatus.Completed )
        {
            throw new BadRequestException("Cannot change status " + orderCurrent.Status + " to " + newStatus);
        }
        
        if (orderCurrent.Status == OrderStatus.Return &&
            newStatus is not OrderStatus.Returned )
        {
            throw new BadRequestException("Cannot change status " + orderCurrent.Status + " to " + newStatus);
        }
        
        if (orderCurrent.Status == OrderStatus.Returned &&
            newStatus is not OrderStatus.Returned )
        {
            throw new BadRequestException("Cannot change status " + orderCurrent.Status + " to " + newStatus);
        }
        
        if (orderCurrent.Status == OrderStatus.Cancel &&
            newStatus is OrderStatus.Completed 
                        or OrderStatus.Returned 
                        or OrderStatus.Return)
        {
            throw new BadRequestException("Cannot change status " + orderCurrent.Status + " to " + newStatus);
        }
        
        // if status valid -> save status
        orderCurrent.Status = newStatus;
        try
        {
            await _context.SaveChangesAsync();
            return "Order status updated successfully";
        }
        catch (Exception ex)
        {
            return "Error updating order status: " + ex.Message;
        }
        
    }

    public async Task<string> DeleteOrder(int orderId)
    {
        var order = await _context.Orders.FindAsync(orderId) ?? throw new NotFoundException("Order Not Found");
        
        _context.Orders.Remove(order);
        await _context.SaveChangesAsync();

        return "The order has been deleted";
    }
}