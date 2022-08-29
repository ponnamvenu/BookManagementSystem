using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ConceptArchitect.BookManagement.Repositories.AdoNet
{

    

    public class AdoAuthorRepositoryV0 : IAuthorRepository
    {
      

        public Func<DbConnection> ConnectionProvider { get; set; }

        public AdoAuthorRepositoryV0(Func<DbConnection> connectionProvider)
        {
            ConnectionProvider = connectionProvider;   
        }

        public Author AddAuthor(Author author)
        {
            DbConnection connection = ConnectionProvider();
            try
            {
                connection.Open();
                var command=connection.CreateCommand();

                command.CommandText = $"insert into authors(Id,Name,Biography,Photo,Tags)" +
                                     $"values('{author.Id}','{author.Name}','{author.Biography}'," +
                                     $"'{author.Photo}','Tag1,Tag2')";

                command.ExecuteNonQuery();

                

                return author;
            }
            catch (DbException ex)
            {
                if (ex.Message.Contains("PRIMARY"))
                    throw new DuplicateEntitityException<string>(author.Id, $"Duplicate Author Id {author.Id}", ex);
                else
                    throw;  //throw the same exception you caught
            }
            finally
            {
                connection.Close();
            }
        }

       
        

        public List<Author> GetAllAuthors()
        {
            DbConnection connection = ConnectionProvider();

            try
            {
                connection.Open();
                DbCommand command= connection.CreateCommand();


                command.CommandText = "select * from authors";
                List<Author> authors=new List<Author>();
                DbDataReader reader= command.ExecuteReader();

                while(reader.Read())
                {
                    var author = new Author()
                    {
                        Id = reader["Id"].ToString(),
                        Name = reader["Name"].ToString(),
                        Biography = reader["Biography"].ToString(),
                        Tags = reader["Tags"].ToString().Split(",").ToList(),
                        Photo = reader["Photo"].ToString()
                    };

                    authors.Add(author);
                }
                return authors;
            }
            catch(DbException ex)
            {

                throw;
            }
            finally
            {
                connection.Close();
            }

            return null;
        }

        public Author GetAuthorById(string id)
        {
            DbConnection connection = ConnectionProvider();

            try
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = $"select * from authors where id='{id}'";
                
                var reader = command.ExecuteReader();

                if (reader.Read())
                {
                    var author = new Author()
                    {
                        Id = reader["Id"].ToString(),
                        Name = reader["Name"].ToString(),
                        Biography = reader["Biography"].ToString(),
                        Tags = reader["Tags"].ToString().Split(",").ToList(),
                        Photo = reader["Photo"].ToString()
                    };
                    return author;
                   
                }
                //return null;                
                throw new EntityNotFoundException<string>(id, $"Not Author with id {id} found");
            }
            
            finally
            {
                connection.Close();
            }

            return null;
        }

        public void RemoveAuthor(string id)
        {
            DbConnection connection = ConnectionProvider();
            try
            {
                connection.Open();
                var command = connection.CreateCommand();

                command.CommandText = $"delete from  authors where id='{id}'";

                var rows= command.ExecuteNonQuery();
                if (rows == 0)
                    throw new EntityNotFoundException<string>(id,$"Invalid Author Id {id}");



            }
            catch (SqlException ex)
            {  
               throw;  //throw the same exception you caught
            }
            finally
            {
                connection.Close();
            }
        }

        public List<Author> Search(string term)
        {
            throw new NotImplementedException();
        }

        public void UpdateAuthor(string id, Author newAuthor)
        {
            throw new NotImplementedException();
        }
    }
}
