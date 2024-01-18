using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VacancieDomain.Model
{
    public class FavoriteVacancie
    {
        public int Id { get; set; }
        public int VacancieId { get; set; }
        public virtual VacancieModel Vacancie { get; set; }
        public int UserId { get; set; }
    }
}
