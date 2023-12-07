using Microsoft.AspNetCore.Mvc;

namespace UserAPI.ViewModel
{
    public class ResponseViewModel
    {
        [HiddenInput]
        public Int64 Id { get; set; }
        public int CvId { get; set; }
        public string CvName { get; set; } = null!;
        public int VacancieId { get; set; }
        public string VacancieName { get; set; } = null!;
        public string Message { get; set; } = null!;
    }
}
