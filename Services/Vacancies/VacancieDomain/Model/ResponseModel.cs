namespace VacancieDomain.Model
{
    public class ResponseModel
    {
        public int Id { get; set; }
        public int CvId { get; set; }
        public int VacancieId { get; set; }
        public string Message { get; set; } = null!;
    }
}
