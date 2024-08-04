using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DBtestApp.Models;
using System.Threading.Tasks;

namespace YourNamespace.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly MyDbContext _context;

        public BooksController(MyDbContext context)
        {
            _context = context;
        }

        // POST: api/Books
        [HttpPost("AddBook")]
        public async Task<ActionResult<Book>> PostBook(Book book)
        {
            if (book == null)
            {
                return BadRequest("Book object is null.");
            }

            _context.Books.Add(book);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetBook), new { id = book.Id }, book);
        }

        // GET: api/Books/5
        [HttpGet("GetBook")]
        public async Task<ActionResult<Book>> GetBook(int id)
        {
            var book = await _context.Books.FindAsync(id);

            if (book == null)
            {
                return NotFound();
            }

            return book;
        }

        // GET: api/Books/5
        [HttpGet("GetAllBooks")]
        public async Task<ActionResult<IEnumerable<Book>>> GetAllBooks()
        {
            var books = await _context.Books.ToListAsync();

            if (books == null)
            {
                return NotFound();
            }

            return books;
        }
        // POST: api/Books
        [HttpPost("DeleteBook")]
        public async Task<ActionResult<Book>> DeleteBook(int id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book == null)
            {
                return NotFound();
            }
            _context.Books.Remove(book);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetBook), new { id = book.Id }, book);
        }

        // POST: api/Books
        [HttpPut("EditeBook")]
        public async Task<ActionResult<Book>> EditeBook(int id, Book updatedBook)
        {
            if(id != updatedBook.Id)
            {
                return BadRequest("Book not found!");
            }

            var oldBook = await _context.Books.FindAsync(id);

            if (oldBook == null)
            {
                return NotFound(new { message = "Book not found" });
            }

            oldBook.Title = updatedBook.Title;
            oldBook.Author = updatedBook.Author;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BookExists(id))
                {
                    return NotFound(new { message = "Book not found during update" });
                }
                else
                {
                    throw;
                }
            }
            return NoContent();
        }
        private bool BookExists(int id)
        {
            return _context.Books.Any(e => e.Id == id);
        }
    }
}