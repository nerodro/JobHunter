using Grpc.Core;
using UserDomain.Models;
using UserService.CvService;
using static CvServiceGrpc;

namespace UserAPI.ServiceGrpc
{
    public class CvRpc : CvServiceGrpcBase
    {
        private readonly ICvService _CvService;
        public CvRpc(ICvService CvService)
        {
            _CvService = CvService;
        }
        public async override Task<CvResponseGrpc> GetCvById(CvRequestGrpc request, ServerCallContext context)
        {
            int CvId = request.CvId;
            CvModel model = await _CvService.GetCv(CvId);

            if (model == null)
            {
                throw new RpcException(new Status(StatusCode.Cancelled, "Запрашиваемое резюме не найдено"));
            }

            CvGrpc Cv = new CvGrpc
            {
                CvId = model.Id,
                CvName = model.JobNmae
            };
            var response = new CvResponseGrpc
            {
                Cv = Cv
            };

            return await Task.FromResult(response);
        }
    }
}
