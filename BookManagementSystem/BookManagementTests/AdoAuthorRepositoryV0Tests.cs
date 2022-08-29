using ConceptArchitect.BookManagement;
using ConceptArchitect.BookManagement.Repositories.AdoNet;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using Xunit;

namespace BookManagementTests
{
    public class AdoAuthorRepositoryV0Tests
    {
        const string connectionString = @$"Server=localhost;Database=ecolab_testdb;Uid=root;Pwd=@DM1n.;";
        AdoAuthorRepositoryV0 authorRepository;

        Author[] authors=new Author[2];
        Func<DbConnection> connectionProvider;
        

        public AdoAuthorRepositoryV0Tests()
        {
            connectionProvider = () => new MySqlConnection()
            {
                ConnectionString = connectionString
            };

            authorRepository = new AdoAuthorRepositoryV0(connectionProvider) ;
            SetupTestDB();

        }

        private void SetupTestDB()
        {
            authors[0] = new Author() { Id = "author1", Name = "Author 1", Biography = "Biography of Author1", Photo = "author1.jpg", Tags = new List<string>() { "tag1", "tag2" } };
            authors[1] = new Author() { Id = "author2", Name = "Author 2", Biography = "Biography of Author2", Photo = "author2.jpg", Tags = new List<string>() { "tag1", "tag2" } };

            var connection = connectionProvider();


            try
            {
                connection.Open();
                var command = connection.CreateCommand();


                // 1. First drop authors table if already exists
                command.CommandText = "DROP TABLE IF EXISTS authors;";
                command.ExecuteNonQuery();
                // 2. create a new authors table
                command.CommandText = "CREATE TABLE authors"
                                      +"   (                                             "
                                      +"                                                 "
                                      +"         Id VARCHAR(50) NOT NULL PRIMARY KEY,    "
                                      +"         Name VARCHAR(50) NOT NULL,              "
                                      +"         Biography VARCHAR(2000) NOT NULL,       "
                                      +"         Tags VARCHAR(200) NOT NULL,             "
                                      +"         Photo VARCHAR(255) NOT NULL             "
                                      +"  );                                             ";

                command.ExecuteNonQuery();

                // 3. insert the known records
                foreach (var author in authors)
                {
                    command.CommandText = $"insert into authors(Id,Name,Biography,Photo,Tags)" +
                                        $"values('{author.Id}','{author.Name}','{author.Biography}','{author.Photo}','Tag1,Tag2')";
                    command.ExecuteNonQuery();
                }
            }
            finally
            {
                connection.Close();
            }
            
            
        }

       [Fact]
        public void GetAllAuthorsReturnsAllAuthors()
        {
            
            var authors= authorRepository.GetAllAuthors();
            Assert.Equal(2, authors.Count);
            Assert.Equal(authors[0].Name, authors[0].Name);
        }

        [Fact]
        public void AddAuthorCanAddAuthorWithUniqueId()
        {
           
            string id = "unique-id";
            var result=authorRepository.AddAuthor(new Author()
            {
                Id=id,
                Name="Mr Author",
                Biography="Biography of Mr Author",
                Photo="MrAuthor.Jpg",
                Tags=new List<string>() { "test","author"}
            });



            Assert.NotNull(result);
            Assert.Equal(id, result.Id);


        }

        [Fact]
        public void AddAuthorShouldFailForDuplicateId()
        {
            string id = authors[0].Id; //duplicate id

            MySqlException ex;

            var e= Assert.Throws<DuplicateEntitityException<string>>(() =>
            {
                var result = authorRepository.AddAuthor(new Author()
                {
                    Id = id,
                    Name = "Mr Author",
                    Biography = "Biography of Mr Author",
                    Photo = "MrAuthor.Jpg",
                    Tags = new List<string>() { "test", "author" }
                });

            });

            Assert.Equal(id, e.Id);
            Assert.True(e.InnerException is DbException);
            Assert.Contains("PRIMARY", e.InnerException.Message);         


        }

        [Fact]
        public void GetAuthorByIdReturnsCorrectAuthorWithValidId()
        {
            var author = authorRepository.GetAuthorById(authors[0].Id);

            Assert.NotNull(author);
            Assert.Equal(authors[0].Name, author.Name);
            Assert.Equal(authors[0].Biography, author.Biography);

        }

        [Fact]
        public void GetAuthorByIdThrowsEntityNotFoundExceptionForInvalidId()
        {
            var invalidId = "invalid-id";
            var ex = Assert.Throws<EntityNotFoundException<string>>(() =>
            {
                var author = authorRepository.GetAuthorById(invalidId);
            });

            Assert.Equal(invalidId, ex.Id);
        }

        [Fact]
        public void RemoveAuthorCanRemoveAuthorWithValidId()
        {
            authorRepository.RemoveAuthor(authors[0].Id);
            

            //Now this author will not be present in my database
            Assert.Throws<EntityNotFoundException<string>>(() => authorRepository.GetAuthorById(authors[0].Id));
        }

        [Fact]
        public void RemoveAuthorCanRemoveAuthorWithInvalidId()
        {
            var invalidId = "invalid-id";
            Assert.Throws<EntityNotFoundException<string>>(()=> authorRepository.RemoveAuthor(invalidId));



        }
    }
}




