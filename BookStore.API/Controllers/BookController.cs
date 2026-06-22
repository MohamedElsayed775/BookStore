using System.Diagnostics.Eventing.Reader;
using BookStore.DTO.BookDTO;
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

        public BookController(IRepository<Book> _bookRepo, UnitOfWorks _unitOfWork)
        {
            BookRepo = _bookRepo;
            UnitOfWork = _unitOfWork;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllBooks()
        {

            List<Book> books = (await BookRepo.GetAllAsync()).ToList();
            List<GetBookDTO> getBookDTOs = new List<GetBookDTO>();
            foreach (var book in books)
            {
                getBookDTOs.Add(new GetBookDTO
                {
                    Title = book.Title,
                    AuthorName = book.AuthorName,
                    Price = book.Price,
                    Quantity = book.Quantity,
                    CreatedOn = book.CreatedOn
                });
            }
            return Ok(getBookDTOs);
        }



        [HttpGet("{id:int}")]
        public IActionResult GetById (int id)
        {
            Book book = BookRepo.Get(o=>o.Id == id).Result;
            if (book == null || book.IsDeleted==true)
            {
                return NotFound("Book Not Found");
            }
            
            else
            {
                GetBookDTO getBookDTO = new GetBookDTO
                {
                    Title = book.Title,
                    AuthorName = book.AuthorName,
                    Price = book.Price,
                    Quantity = book.Quantity,
                    CreatedOn = book.CreatedOn
                };
                return Ok(getBookDTO);
            }
        }

        [HttpPost]
        public async Task<IActionResult> add(CreateBookDTO createBookDTO)
        {
            if (ModelState.IsValid)
            {
                Book book = new Book
                {
                    Title = createBookDTO.Title,
                    AuthorName = createBookDTO.AuthorName,
                    Price = createBookDTO.Price,
                    Quantity = createBookDTO.Quantity
                };
                await BookRepo.Add(book);
                UnitOfWork.SaveAndCommit();
                return Ok("Book Created Successfully");
            }
            return BadRequest(ModelState);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, UpdateBookDTO updateBookDTO)
        {
            if (ModelState.IsValid)
            {
                Book book = await BookRepo.Get(o => o.Id == id);
                if (book == null)
                {
                    return NotFound("Book Not Found");
                }
                else
                {
                    book.Title = updateBookDTO.Title;
                    book.AuthorName = updateBookDTO.AuthorName;
                    book.Price = updateBookDTO.Price;
                    book.Quantity = updateBookDTO.Quantity;
                    BookRepo.Update(book);
                    UnitOfWork.SaveAndCommit();
                    return Ok("Book Updated Successfully");
                }
            }
            else
            {
                return BadRequest(ModelState);
            }

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