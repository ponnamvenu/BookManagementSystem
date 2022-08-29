using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConceptArchitect.BookManagement
{
    public interface IUserService
    {
        User AddUser(User user);// void 
        List<User> GetAll();

       User GetUserByid(string id);
        void RemoveUser(string id); // delete
        void SaveUser(User user); // update

    }
}
