using AutoMapper;
using EProject_Sem_3.Exceptions;
using EProject_Sem_3.Mapper;
using EProject_Sem_3.Models;
using EProject_Sem_3.Models.BookImages;
using EProject_Sem_3.Models.Books;
using EProject_Sem_3.Models.Orders;
using EProject_Sem_3.Repositories.Books;
using EProject_Sem_3.Repositories.Orders;
using EProject_Sem_3.Repositories.OrdersDetail;
using EProject_Sem_3.Services.VnpayService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EProject_Sem_3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly IBookRepo _bookRepo;
        private readonly IOrderRepo _orderRepo;
        private readonly IOrderDetailRepo _orderDetailRepo;
        private readonly IVnPayService _vnPayService;
        private readonly AppDbContext _dbContext;


        public BookController(IBookRepo bookRepo,
                                IOrderRepo orderRepo,
                                IOrderDetailRepo orderDetailRepo,
                                IVnPayService vnPayService,
                                AppDbContext dbContext)
        {
            _bookRepo = bookRepo;
            _orderRepo = orderRepo;
            _orderDetailRepo = orderDetailRepo;
            _vnPayService = vnPayService;
            _dbContext = dbContext;
        }


        //get all books
        [HttpGet]
        public async Task<IActionResult> GetAllBooks()
        {
            return Ok(await _bookRepo.GetAllBooks());
        }
        
        // get book
        [HttpGet("{id}")]
        public async Task<IActionResult> GetBook(int id)
        {
            return Ok(await _bookRepo.GetBook(id));
        }

        //create book
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> CreateBook([FromBody] BookDto bookDto)
        {
            
            return Ok(await _bookRepo.CreateBook(bookDto));
        }
        
        //delete book
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook(int id) {
            return Ok(await _bookRepo.DeleteBook(id));
        }
        
        // update book
        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBook(int id,[FromBody] BookDto bookDto)
        {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }
            
            return Ok(await _bookRepo.UpdateBook(id,bookDto));
        }

        // upload image book
        [Authorize(Roles = "Admin")]
        [HttpPost("{id}/UploadImageBook")]
        public async Task<IActionResult> UploadImageBook(int id,string imageUrl)
        {
            var existingBook = await _dbContext.Books.FirstOrDefaultAsync(b => b.Id == id)
                ?? throw new NotFoundException("Book not found");
            
            // create image book object
            var bookImage = new BookImage
            {
                Image = imageUrl,
                BookId = id,
                Book = existingBook,
            };

            // add to db
            _dbContext.BookImages.Add(bookImage);

            // save db
            await _dbContext.SaveChangesAsync();

            
            return Ok("Image uploaded successfully.");
        }

        
        // delete Image Book
        [HttpDelete("{id}/DeleteImage")]
        public async Task<IActionResult> DeleteImageBook(int id, string imageUrl)
        {
            var existingBook = await _dbContext.Books.FirstOrDefaultAsync(b => b.Id == id)
                               ?? throw new NotFoundException("Book not found");
            
            var existingImage = await _dbContext.BookImages.FirstOrDefaultAsync(i => i.Image == imageUrl)
                               ?? throw new NotFoundException("Image Book not found");


            _dbContext.BookImages.Remove(existingImage);
            await _dbContext.SaveChangesAsync();

            return Ok("The image book has been deleted");
        }
        
        
        
        [HttpPost("checkout")]
        public async Task<IActionResult> Checkout(OrderDto dto)
        {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            // save order to db
            var order = await _orderRepo.CreateOrder(dto);
        
            // save orderDetail to db
            await _orderDetailRepo.CreateOrderDetail(order.Id, dto);
        
        
            // create payment url
            var vnPayModel = new VnPaymentOrderRequestModel()
            {
                TotalAmount = dto.TotalAmount,
                OrderId = order.Id,
                Phone = order.Phone
            };
        
            // return url
            return Ok(_vnPayService.CreatePaymentUrlForOrder(vnPayModel));
        
        }
    }
}

