using CompanyDomain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserDomain.Models;

namespace CompanyRepository.Registration
{
    public interface IRegistration<T> where T : CompanyModel
    {
        Task Create(T entity);
    }
}
