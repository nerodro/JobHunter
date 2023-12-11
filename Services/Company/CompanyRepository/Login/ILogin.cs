using CompanyDomain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserDomain.Models;

namespace CompanyRepository.Login
{
    public interface ILogin<T> where T : CompanyModel
    {
        Task<T> Get(long id);
    }
}
