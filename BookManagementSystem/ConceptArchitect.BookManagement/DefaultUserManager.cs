using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConceptArchitect.BookManagement
{
    public class DefaultUserManager : IUserService
    {
        IUserRepository userRepository;
        public DefaultUserManager(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        public User AddUser(User user)
        {
            return userRepository.AddUser(user);
        }

        public List<User> GetAll()
        {
            return userRepository.GetAllUsers();
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
    }
}
