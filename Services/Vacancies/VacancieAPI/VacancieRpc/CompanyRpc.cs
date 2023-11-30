using Grpc.Net.Client;
using VacancieAPI.ViewModel;

namespace VacancieAPI.VacancieRpc
{
    public class CompanyRpc
    {
        private readonly CompanyServiceGrpc.CompanyServiceGrpcClient _rpc;
        public CompanyRpc()
        {
            var channel = GrpcChannel.ForAddress("https://localhost:7039");
            _rpc = new CompanyServiceGrpc.CompanyServiceGrpcClient(channel);
        }
        public async Task<CompanyViewModel> GetCompany(int CompanyId)
        {
            var request = new CompanyRequestGrpc
            {
                CompanyId = CompanyId
            };

            var response = _rpc.GetCompanyById(request);
            CompanyViewModel Company = new CompanyViewModel
            {
                Id = response.Company.CompanyId,
                CompanyName = response.Company.CompanyName
            };
            return Company;
        }

    }
}
