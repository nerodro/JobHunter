using Microsoft.AspNetCore.Mvc;

namespace VacancieAPI.ViewModel
{
    public class ResponseViewModel
    {
        [HiddenInput]
        public Int64 Id { get; set; }
        public int CvId { get; set; }
        public int VacancieId { get; set; }
        public string Message { get; set; } = null!;
    }
}
