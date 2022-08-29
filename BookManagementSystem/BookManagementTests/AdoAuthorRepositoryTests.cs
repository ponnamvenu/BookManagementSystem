using ConceptArchitect.BookManagement;
using ConceptArchitect.BookManagement.Repositories.AdoNet;
using ConceptArchitect.Utils;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using Xunit;

namespace BookManagementTests
{
    public class AdoAuthorRepositoryTests
    {
        const string connectionString = @$"Server=localhost;Database=ecolab_testdb;Uid=root;Pwd=@DM1n.;";
        AdoAuthorRepository authorRepository;

        Author[] authors=new Author[2];
        Func<DbConnection> connectionProvider;
        DbRepository db;
        

        public AdoAuthorRepositoryTests()
        {
            connectionProvider = () => new MySqlConnection()
            {
                ConnectionString = connectionString
            };

            db=new DbRepository(connectionProvider); 

            authorRepository = new AdoAuthorRepository(db) ;
            SetupTestDB();

        }

        private void SetupTestDB()
        {
            authors[0] = new Author() { Id = "jeffrey-archer", Name = "Jeffrey Archer", Biography = "Bestseller author in English fiction", Photo = "archer.jpg", Tags = new List<string>() { "english", "fiction" } };
            authors[1] = new Author() { Id = "john-grisham", Name = "John Grisham", Biography = "Bestseller author of legal fiction", Photo = "grisham.jpg", Tags = new List<string>() { "legalfiction", "fiction" } };


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
                    var tags = "";
                    author.Tags.ForEach(tag => tags += $"{tag},");
                    tags = tags.Substring(0, tags.Length - 1);
                    command.CommandText = $"insert into authors(Id,Name,Biography,Photo,Tags)" +
                                        $"values('{author.Id}','{author.Name}','{author.Biography}','{author.Photo}','{tags}')";
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

        [Fact]
        public void CanUpdateTheDetailsOfAuthorWithValidId()
        {
            var author = authors[0];
            author.Photo = "john-grisham.jpg";

            authorRepository.UpdateAuthor(author.Id, author);

            var result = authorRepository.GetAuthorById(author.Id);
            Assert.Equal(author.Photo, result.Photo);

        }


        [Fact]
        public void UpdateFailsForAuthorWithInvalidId()
        {
            var author = authors[0];
            author.Photo = "john-grisham.jpg";

            Assert.Throws<EntityNotFoundException<string>>(() =>
            {
                authorRepository.UpdateAuthor("invalid-id", author);
            });

            

        }

        [Fact]
        public void SearchCanReturnOnPartialAuthorName()
        {
            var result = authorRepository.Search("Grisham");

            Assert.Equal(1, result.Count);
            Assert.Equal("John Grisham", result[0].Name);
        }


        [Fact]
        public void SearchCanReturnOnPartialBiography()
        {
            var result = authorRepository.Search("Bestseller");

            Assert.Equal(2, result.Count);
        }

        [Fact]
        public void SearchCanReturnOnPartialTag()
        {
            var result = authorRepository.Search("legal");

            Assert.Equal(1, result.Count);
            Assert.Equal("John Grisham", result[0].Name);
        }

        [Fact]
        public void SearchReturnsEmptyListWhenNoMatchingAuthor()
        {
            var result = authorRepository.Search("MissingText");

            Assert.Equal(0, result.Count);
        }


        [Fact]
        public void DummyTest()
        {
            var builder = new ObjectRepository<Author>();
            builder.CreateTable();
        }

    }
}




