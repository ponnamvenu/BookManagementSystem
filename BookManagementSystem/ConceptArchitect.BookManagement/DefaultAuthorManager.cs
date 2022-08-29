using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConceptArchitect.BookManagement
{
    public class DefaultAuthorManager : IAuthorService
    {
        IAuthorRepository repository;
        public DefaultAuthorManager(IAuthorRepository repository)
        {
            this.repository = repository;
        }

        public void AddAuthor(Author author)
        {
            repository.AddAuthor(author);
        }

        public List<Author> GetAll()
        {
            return repository.GetAllAuthors();
        }

        public Author GetAuthorById(string authorId)
        {
            return repository.GetAuthorById(authorId);
        }

        public List<Author> GetAuthorsAboveRating(double rating)
        {
            return (from author in GetAll()
                    where author.Rating> rating
                    select author).ToList();
        }

        public List<Book> GetBooksByAuthor(string authorId)
        {
            return GetAuthorById(authorId).Books;
        }

        public void RemoveAuthor(string authorId)
        {
            repository.RemoveAuthor(authorId);
        }

        public void SaveAuthor(Author author)
        {
            repository.UpdateAuthor(author.Id, author);
        }

        public List<Author> Search(string term)
        {
            return repository.Search(term);
        }
    }
}
