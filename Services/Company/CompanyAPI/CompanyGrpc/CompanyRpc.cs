using CompanyAPI.ViewModel;
using CompanyDomain.Model;
using CompanyService.CompanyService;
using Grpc.Core;
using Grpc.Net.Client;
using static CompanyServiceGrpc;

namespace CompanyAPI.CompanyGrpc
{
    public class CompanyRpc : CompanyServiceGrpcBase
    {
        private readonly ICompanyService _CompanyService;
        public CompanyRpc(ICompanyService CompanyService)
        {
            _CompanyService = CompanyService;
        }
        public async override Task<CompanyResponseGrpc> GetCompanyById(CompanyRequestGrpc request, ServerCallContext context)
        {
            int CompanyId = request.CompanyId;
            CompanyModel model = await _CompanyService.GetCompany(CompanyId);

            if (model == null)
            {
                throw new RpcException(new Status(StatusCode.Cancelled, "Запрашиваемая компания не найдена"));
            }

            CompanyGrpcMode Company = new CompanyGrpcMode
            {
                CompanyId = model.Id,
                CompanyName = model.CompanyName
            };
            var response = new CompanyResponseGrpc
            {
                Company = Company
            };

            return await Task.FromResult(response);
        }
    }
}
