using Microsoft.AspNetCore.Mvc;

namespace UserAPI.ViewModel
{
    public class CategoryViewModel
    {
        [HiddenInput]
        public Int64 Id { get; set; }
        public string CategoryName { get; set; } = null!;
    }
}
