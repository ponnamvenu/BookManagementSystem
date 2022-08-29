using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConceptArchitect.BookManagement
{
    public interface IBookService
    {
        void AddBook(Book book);
        List<Book> GetAll();
        List<Book> GetBooksAboverating(double rating);
        //List<Book> Search(string term);

        Book GetBookByid(string bookId);
        void RemoveBook(string bookId);
        void SaveBook(Book book);
    }
}
