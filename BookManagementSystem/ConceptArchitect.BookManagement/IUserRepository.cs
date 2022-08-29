using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConceptArchitect.BookManagement
{
    public interface IUserRepository
    {
        User AddUser(User user);
        List<User> GetAllUsers();
        User GetUserById(string id);
        void UpdateUser(string id,User newUser);
        void RemoveUser(string id);
    }
}
