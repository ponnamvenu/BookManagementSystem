using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConceptArchitect.BookManagement
{
    public interface IAuthorRepository
    {
        Author AddAuthor(Author author);
        List<Author> GetAllAuthors();

        Author GetAuthorById(string id);

        void UpdateAuthor(string id, Author newAuthor);

        void RemoveAuthor(string id);

        List<Author> Search(string term);
    }
}
