using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserDomain.Models;

namespace UserRepository.Login
{
    public interface ILogin<T> where T : UserModel
    {
        T Get(long id);
    }
}
