using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConceptArchitect.BookManagement
{
   public interface IBookRepository
    {
        Book AddBook(Book book);

        List<Book> GetAllBooks();

        Book GetBookById(string id);

        Book GetBookReviews(string id);

        void UpdateBook(string id, Book newBook);

        void RemoveBook(string id);

    }
}
