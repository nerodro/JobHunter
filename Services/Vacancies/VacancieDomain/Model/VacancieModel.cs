namespace VacancieDomain.Model
{
    public class VacancieModel
    {
        public int Id { get; set; }
        public string WorkName { get; set; } = null!;
        public string AboutWork { get; set; } = null!;
        public int CompanyId { get; set; }
        public int CityId { get; set; }
        public int CountryId { get; set; }
    }
}
