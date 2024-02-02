using Microsoft.AspNetCore.Mvc;
using UserAPI.ViewModel;

namespace UserAPI.RabbitMq
{
    public interface ICreateProducer
    {
        Task GeneratePdfCv(CvViewModel model);
    }
}
