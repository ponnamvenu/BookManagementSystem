using ConceptArchitect.Utils;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConceptArchitect.BookManagement.Repositories.AdoNet
{
    public class AdoUserRepository : IUserRepository

    {
        DbRepository UserDb;
        public Func<DbConnection> ConnectionProvider { get; set; }

        public AdoUserRepository(DbRepository db)
        {
            this.UserDb = db;

        }
        public User AddUser(User user)
        {
            return UserDb.ExecuteCommand(command =>
            {
                try
                {
                    command.CommandText = $"insert into users (Email,Name,Password,PhotoUrl)" +
                    $" values('{user.Email}' , '{user.Name}' , '{user.Password}' , '{user.PhotoUrl}')";
                    command.ExecuteNonQuery();
                    return user;
                }
                catch (DbException ex)
                {
                    if (ex.Message.Contains("PRIMARY"))
                        throw new DuplicateEntitityException<string>(user.Email, $"Duplicate User Email {user.Email}", ex);

                    else
                        throw;  //throw the same exception you caught
                }

            });
        }

       
        public User GetUserByid(string id)
        {
            throw new NotImplementedException();
        }

        public void RemoveUser(string id)
        {
            throw new NotImplementedException();
        }

        public void SaveUser(User user)
        {
            throw new NotImplementedException();
        }

        public List<User> GetAllUsers()
        {
            return UserDb.Query("select * from users",
                 ObjectMapper.To<User>((user, reader) => { }));

        }

        public User GetUserById(string id)
        {
            throw new NotImplementedException();
        }

        public void UpdateUser(string id, User newUser)
        {
            throw new NotImplementedException();
        }
    }
}
