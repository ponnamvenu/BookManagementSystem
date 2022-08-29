// See https://aka.ms/new-console-template for more information


using ConceptArchitect.BookManagement.Repositories.AdoNet;
using ConceptArchitect.Utils;
using System.Data.Common;
using System.Data.SqlClient;

Func<DbConnection> connectionProvider = () => new SqlConnection()
{

    ConnectionString = @"Data Source = (localdb)\ProjectModels; Initial Catalog = EcolabBooks;"
                                                             + @" Integrated Security = True; Connect Timeout = 30; Encrypt = False; TrustServerCertificate = False; "
                                                             + @"ApplicationIntent = ReadWrite; MultiSubnetFailover = False;"
};


var db = new DbRepository(connectionProvider);

var repository = new AdoAuthorRepository(db);



var authors = repository.GetAllAuthors();

foreach(var author in authors)
    Console.WriteLine($"{author.Name}");
