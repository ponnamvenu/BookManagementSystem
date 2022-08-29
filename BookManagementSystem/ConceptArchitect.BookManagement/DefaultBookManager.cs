using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConceptArchitect.BookManagement
{
    public class DefaultBookManager: IBookService
    {
        IBookRepository bookRepository;

        public DefaultBookManager(IBookRepository bookRepository)
        {
            this.bookRepository = bookRepository;
        }

        public void AddBook(Book book)
        {
            bookRepository.AddBook(book);
        }

        public List<Book> GetAll()
        {
           return bookRepository.GetAllBooks();
        }

        public Book GetBookByid(string bookId)
        {
            return bookRepository.GetBookById(bookId);
        }

        public List<Book> GetBooksAboveRating(double rating)
        {
            return (from book in GetAll()
                    where book.Rating > rating
                    select book).ToList();
        }
        public List<Book> GetBooksAboverating(double rating)
        {
            throw new NotImplementedException();
        }

        public void RemoveBook(string bookId)
        {
            bookRepository.RemoveBook(bookId);
        }

        public void SaveBook(Book book)
        {
            bookRepository.UpdateBook(book.Id, book);
        }
    }
}
