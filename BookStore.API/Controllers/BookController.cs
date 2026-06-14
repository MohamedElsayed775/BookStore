using BookStore.Model;
using BookStore.Repository;
using BookStore.UnitOfWork;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly IRepository<Book> BookRepo;
        private readonly UnitOfWorks UnitOfWork;

        public BookController(IRepository<Book> _bookRepo , UnitOfWorks _unitOfWork)
        {
            BookRepo = _bookRepo;
            UnitOfWork = _unitOfWork;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllBooks()
        {
            List<Book> books = (await BookRepo.GetAllAsync()).ToList();
            return Ok(books);
        }

        [HttpPost]
        public async Task<IActionResult> add(Book book)
        {
            if (ModelState.IsValid)
            {
                await BookRepo.Add(book);
                UnitOfWork.SaveAndCommit();
                return Ok("Book Created Successfully");
            }
            return BadRequest(ModelState);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (ModelState.IsValid)
            {
                await BookRepo.Delete(id);
                UnitOfWork.SaveAndCommit();
                return Ok("Book Deleted Successfully");
            }
            return BadRequest(ModelState);
        }
    }
}