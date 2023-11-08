using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserDomain.Models
{
    public class LanguageModel
    {
        public int Id { get; set; }
        public string Language { get; set; } = null!;
        public virtual List<CvModel> CvModels { get; set; }
        public LanguageModel() 
        { 
            CvModels = new List<CvModel>();
        }
    }
}
