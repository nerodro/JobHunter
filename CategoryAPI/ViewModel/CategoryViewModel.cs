using Microsoft.AspNetCore.Mvc;

namespace CategoryAPI.ViewModel
{
    public class CategoryViewModel
    {
        [HiddenInput]
        public Int64 Id { get; set; }
        public string CategoryName { get; set; } = null!;
    }
}
