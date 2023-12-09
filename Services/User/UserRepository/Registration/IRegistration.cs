using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserDomain.Models;

namespace UserRepository.Registration
{
    public interface IRegistration<T> where T : UserModel
    {
        Task Create(T entity);
    }
}
