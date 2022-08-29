using ConceptArchitect.Utils;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConceptArchitect.BookManagement.Repositories.AdoNet
{
    public class AdoBookRepository : IBookRepository
    {
        DbRepository BookDb;
        public Func<DbConnection> ConnectionProvider { get; set; }

        public AdoBookRepository(DbRepository db)
        {
            this.BookDb = db;
           
        }

        public Book AddBook(Book book)
        {
            return BookDb.ExecuteCommand(command =>
            {
                try
                {
                    command.CommandText = $"insert into books(Id,Title,AuthorId,Price,Cover,Description)"+
                    $" values('{book.Id}' , '{book.Title}' , '{book.AuthorId}','{book.Price}' , '{book.Cover}' , '{book.Description}')";
                    command.ExecuteNonQuery();
                    return book;
                }
                catch (DbException ex)
                {
                    if (ex.Message.Contains("PRIMARY"))
                        throw new DuplicateEntitityException<string>(book.Id, $"Duplicate book Id {book.Id}", ex);

                    if (ex.Message.Contains("FOREIGN"))
                        throw new EntityNotFoundException<string>(book.AuthorId, $"No author with  id {book.AuthorId}");
                    else
                        throw;  //throw the same exception you caught
                }

            });
         


           
        }

        public List<Book> GetAllBooks()
        {
            return BookDb.Query("select * from books",
                 ObjectMapper.To<Book>((book, reader) => {

                    // author.Tags = reader["Tags"].ToString().Split(',').ToList();

                 }));
        }

        private Book GetBook(DbDataReader reader)
        {
            var book = reader.To<Book>();
            return book;
        }
        public Book GetBookById(string id)
        {
            var book = BookDb.QueryOne($"select * from books where id='{id}'", GetBook);
            return book ?? throw new EntityNotFoundException<string>(id, $"No Book with id {id} found");
        }

        public Book GetBookReviews(string id)
        {
            throw new NotImplementedException();
        }

        public void RemoveBook(string id)
        {
            BookDb.ExecuteCommand(command =>
            {

                command.CommandText = $"delete from  books where id='{id}'";

                var rows = command.ExecuteNonQuery();
                if (rows == 0)
                    throw new EntityNotFoundException<string>(id, $"Invalid book Id {id}");

                return rows;

            });
        }

        public void UpdateBook(string id, Book newBook)
        {
            try
            {
                BookDb.ExecuteCommand(command =>
                {

                    command.CommandText = $"update books " +
                                        $"set Title='{newBook.Title}'," +
                                        $"   AuthorId='{newBook.AuthorId}'," +
                                        $"    Price='{newBook.Price}',  " +
                                        $"    Cover='{newBook.Cover}',  " +
                                         $"    Description='{newBook.Description}'  " +

                                        $"where Id='{id}'";

                    var rows = command.ExecuteNonQuery();
                    if (rows == 0)
                        throw new EntityNotFoundException<string>(id, $"No book with id {id}");
                    else
                        return rows;
                });
            }
            catch (DbException ex)
            {
                if (ex.Message.Contains("FOREIGN"))
                    throw new EntityNotFoundException<string>(newBook.AuthorId, $"No author with  id {newBook.AuthorId}");
                else
                    throw;  //throw the same exception you caught
            }

        }
    }
}
