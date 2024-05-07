using AutoMapper;
using EProject_Sem_3.Exceptions;
using EProject_Sem_3.Models;
using EProject_Sem_3.Models.OrderDetails;
using EProject_Sem_3.Models.Orders;
using Microsoft.EntityFrameworkCore;


namespace EProject_Sem_3.Repositories.OrdersDetail;

public class OrderDetailRepo : IOrderDetailRepo
{

    private readonly AppDbContext _context;

    private readonly IMapper _mapper;

    public OrderDetailRepo(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    
    
    public async Task<List<OrderDetailRes>> GetAllByOrder(int orderId)
    {
        var orderDetails = await _context.OrderDetails
            .Where(od => od.OrderId == orderId)
            .ToListAsync();

        foreach (var orderDetail in orderDetails)
        {
            var book = await _context.Books.FindAsync(orderDetail.BookId) ?? throw new NotFoundException("Book Not Found");
            var order = await _context.Orders.FindAsync(orderDetail.OrderId) ?? throw new NotFoundException("Order Not Found");
            
            orderDetail.Book = book;
            orderDetail.Order = order;
        }
        var orderDetailResList = _mapper.Map<List<OrderDetailRes>>(orderDetails);

        return orderDetailResList;
    }

    public async void CreateOrderDetail(int orderId, OrderDto dto)
    {
        // list OrderDetailDto -> list OrderDetail
        var orderDetails = _mapper.Map<List<OrderDetail>>(dto.OrderDetails);
        
        // save orderDetail to db
        foreach (var orderDetail in orderDetails)
        {
            // Check if dto.BookId is not null and doesn't exist in the database
            var book = await _context.Books.FindAsync(orderDetail.BookId) ?? throw new NotFoundException("Book Not Found");

            orderDetail.Book = book;
            orderDetail.OrderId = orderId;
            
            _context.OrderDetails.Add(orderDetail);
            await _context.SaveChangesAsync();
        }
    }

    // public async void UpdateOrderDetail(int orderId, UpdateOrderDto dto)
    // {
    //     // delete All OrderDetail
    //     DeleteAll(orderId);
    //     
    //     // list OrderDetailDto -> list OrderDetail
    //     var orderDetails = _mapper.Map<List<OrderDetail>>(dto.OrderDetails);
    //     
    //     // save orderDetail to db
    //     foreach (var orderDetail in orderDetails)
    //     {
    //         // Check if dto.BookId is not null and doesn't exist in the database
    //         var book = await _context.Books.FindAsync(orderDetail.BookId) ?? throw new NotFoundException("Book Not Found");
    //
    //         orderDetail.Book = book;
    //         orderDetail.OrderId = orderId;
    //         
    //         _context.OrderDetails.Add(orderDetail);
    //         await _context.SaveChangesAsync();
    //     }
    // }
    
}