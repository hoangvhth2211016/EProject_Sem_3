using AutoMapper;
using EProject_Sem_3.Exceptions;
using EProject_Sem_3.Mapper;
using EProject_Sem_3.Models.Books;
using EProject_Sem_3.Repositories.Books;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EProject_Sem_3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly IBookRepo _bookRepo;


        public BookController(IBookRepo bookRepo)
        {
            _bookRepo = bookRepo;
        }


        //get all books
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllBooks()
        {
            return Ok(await _bookRepo.GetAllBooks());
        }
        
        // get book
        [Authorize(Roles = "Admin")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(int id)
        {
            return Ok(await _bookRepo.GetBook(id));
        }

        //create book
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> CreateBook([FromBody] BookDto bookDto)
        {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }
            
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
        public async Task<IActionResult> UpdateBook(int id,BookDto bookDto)
        {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }
            
            return Ok(await _bookRepo.UpdateBook(id,bookDto));
        }
    }
}

