using Grpc.Net.Client;
using VacancieAPI.ViewModel;

namespace VacancieAPI.VacancieRpc
{
    public class CvRpc
    {
        private readonly CvServiceGrpc.CvServiceGrpcClient _rpc;
        public CvRpc()
        {
            var channel = GrpcChannel.ForAddress("https://localhost:7028");
            _rpc = new CvServiceGrpc.CvServiceGrpcClient(channel);
        }
        public async Task<CvViewModel> GetCv(int CvId)
        {
            var request = new CvRequestGrpc
            {
                CvId = CvId
            };

            var response = _rpc.GetCvById(request);
            CvViewModel Cv = new CvViewModel
            {
                Id = response.Cv.CvId,
                CvName = response.Cv.CvName
            };
            return Cv;
        }
    }
}
