using Microsoft.AspNetCore.Mvc;
using VacancieDomain.Model;

namespace VacancieAPI.ViewModel
{
    public class FavoriteViewModel
    {
        [HiddenInput]
        public Int64 Id { get; set; }
        public int VacancieId { get; set; }
        public int UserId { get; set; }
    }
}
