using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConceptArchitect.BookManagement
{
    public interface IAuthorService
    {
        void AddAuthor(Author author);
        List<Author> GetAll();
        List<Author> GetAuthorsAboveRating(double rating);
        List<Author> Search(string term);

        List<Book> GetBooksByAuthor(string authorId);

        Author GetAuthorById(string authorId);

        void SaveAuthor(Author author);

        void RemoveAuthor(string authorId);
    }
}
