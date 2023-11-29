﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VacancieDomain.Model;

namespace VacancieService.VacancieService
{
    public interface IVacancieService
    {
        IEnumerable<VacancieModel> GetAll();
        Task<VacancieModel> GetVacancie(int id);
        Task CreateVacancie(VacancieModel Vacancie);
        Task UpdateVacancie(VacancieModel Vacancie);
        Task DeleteVacancie(int id);
    }
}
