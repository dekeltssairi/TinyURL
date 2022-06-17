using TinyUrl.Models;
using TinyUrl.Services;

namespace TinyUrl.Dal
{
    public class TinyUrlDal : ITinyUrlDal
    {
        private readonly IBooksService _booksService;

        public TinyUrlDal(IBooksService bookService)
        {
            _booksService = bookService ?? throw new ArgumentNullException(nameof(bookService));
        }

        public async Task InsertTinyUrl(Uri originalUrl, Uri tinyUrl)
        {
            Book book = new Book()
            {
                Id = "my-firt-id-book",
                BookName = "my-book-name",
                Price = 10,
                Category = "economy",
                Author = "Dekel"
            };

            await _booksService.CreateAsync(book);
        }
    }
}
