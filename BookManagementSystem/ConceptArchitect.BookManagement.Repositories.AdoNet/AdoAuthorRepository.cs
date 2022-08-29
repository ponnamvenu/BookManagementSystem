using ConceptArchitect.Utils;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ConceptArchitect.BookManagement.Repositories.AdoNet
{

    //public delegate DbConnection ConnectionProvider();

    public class AdoAuthorRepository : IAuthorRepository
    {
        DbRepository db;

        public Func<DbConnection> ConnectionProvider { get; set; }

        public AdoAuthorRepository(DbRepository db)
        {
            this.db = db;
        }

        public Author AddAuthor(Author author)
        {

            return db.ExecuteCommand(command =>
            {
                try
                {
                    var tags = "";
                    author.Tags.ForEach(tag => tags += $" {tag},");
                    tags = tags.Substring(0, tags.Length - 1);

                    command.CommandText = $"insert into authors(Id,Name,Biography,Photo,Tags)" +
                                         $"values('{author.Id}','{author.Name}','{author.Biography}'," +
                                         $"'{author.Photo}','{tags}')";

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
            });

            
        }

       
        

        public List<Author> GetAllAuthors()
        {
            return db.Query("select * from authors", 
                ObjectMapper.To<Author>((author, reader) => {

                    author.Tags = reader["Tags"].ToString().Split(',').ToList() ;

                }));
        }

        private  Author GetAuthor(DbDataReader reader)
        {
            var author = reader.To<Author>(); //create object and assign matching properties 

            //assign other properties that need manual operation
            author.Tags = reader["Tags"].ToString().Split(',').ToList();


            return author;
        }

        public Author GetAuthorById(string id)
        {
            //var author = db.QueryOne($"select * from authors where id='{id}'", ObjectMapper.To<Author>);

            var author = db.QueryOne($"select * from authors where id='{id}'", GetAuthor);
            return author ??  throw new EntityNotFoundException<string>(id, $"Not Author with id {id} found");
           
        }

        public void RemoveAuthor(string id)
        {
            db.ExecuteCommand(command =>
            {

                command.CommandText = $"delete from  authors where id='{id}'";

                var rows = command.ExecuteNonQuery();
                if (rows == 0)
                    throw new EntityNotFoundException<string>(id, $"Invalid Author Id {id}");

                return rows;

            });
        }

        public List<Author> Search(string term)
        {
            var qry= $"select * from authors where " +
                                      $"Name like '%{term}%'  OR Biography like '%{term}%' OR Tags like '%{term}%'  ";


            return db.Query(qry, GetAuthor);


            
        }

        public void UpdateAuthor(string id, Author newAuthor)
        {
            db.ExecuteCommand(command =>
            {
                string tags = "";
                foreach (var tag in newAuthor.Tags)
                    tags += $"{tag},";
                tags = tags.Substring(0, tags.Length - 1);
                command.CommandText = $"update authors " +
                                    $"set Name='{newAuthor.Name}'," +
                                    $"    Biography='{newAuthor.Biography}'," +
                                    $"    Photo='{newAuthor.Photo}',  " +
                                    $"    Tags='{tags}'  " +
                                    $"where Id='{id}'";

                var rows= command.ExecuteNonQuery();
                if (rows == 0)
                    throw new EntityNotFoundException<string>(id, $"No author with id {id}");
                else
                    return rows;
            });
        }
    }
}
