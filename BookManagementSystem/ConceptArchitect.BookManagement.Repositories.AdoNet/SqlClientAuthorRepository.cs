using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ConceptArchitect.BookManagement.Repositories.AdoNet
{
    public class SqlClientAuthorRepository : IAuthorRepository
    {
        public  string ConnectionString { get; set; } = @"Data Source = (localdb)\ProjectModels; Initial Catalog = EcolabBooks;"
                                            + @" Integrated Security = True; Connect Timeout = 30; Encrypt = False; TrustServerCertificate = False; "
                                            + @"ApplicationIntent = ReadWrite; MultiSubnetFailover = False;";

        
        public Author AddAuthor(Author author)
        {
            DbConnection connection = new SqlConnection()
            {
                ConnectionString=ConnectionString
            };
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
            catch (SqlException ex)
            {
                if (ex.Message.Contains("Violation of PRIMARY KEY constraint"))
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

            DbConnection connection = new SqlConnection()
            {
                ConnectionString = ConnectionString
            };

            try
            {
                //connection = ???;

                connection.ConnectionString = ConnectionString;
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
            finally
            {
                connection.Close();
            }

            return null;
        }

        public Author GetAuthorById(string id)
        {
            DbConnection connection = null;

            try
            {
                connection = new SqlConnection();
                connection.ConnectionString = ConnectionString;
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
            DbConnection connection = null;
            try
            {
                connection = new SqlConnection();
                connection.ConnectionString = ConnectionString;
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
