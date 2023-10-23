using MultifactorAPI.DTOModels;
using MultifactorAPI.Domain;

namespace MultifactorAPI.Services
{
    public interface IRequestManager
    {
        Task<RequestCredential> GetCredentialAsync(RequestModel request);
        void SetCredential(AccessModel request);
    }
}
