using MultifactorAPI.DTOModels;
using MultifactorAPI.Domain;
using System.Collections.Concurrent;

namespace MultifactorAPI.Services
{
    public class RequestManager : IRequestManager
    {
        private const int REQUEST_TIMEOUT = 20000;
        private readonly ConcurrentDictionary<string, TaskCompletionSource<RequestCredential>> _requestMap;
        public int Timeout { get; init; } = REQUEST_TIMEOUT;

        public RequestManager()
        {
            _requestMap = new ConcurrentDictionary<string, TaskCompletionSource<RequestCredential>>();
        }

        public Task<RequestCredential> GetCredentialAsync(RequestModel request)
        {
            var tcs = new TaskCompletionSource<RequestCredential>();
            _requestMap.TryAdd(request.Resource, tcs);

            Task.Delay(Timeout)
                .GetAwaiter()
                .OnCompleted(delegate
                {
                    if (!_requestMap.TryRemove(request.Resource, out var tcsValue)) return;

                    tcsValue.TrySetResult(
                        new RequestCredential
                        {
                            Resource = request.Resource,
                            Status = CredentialStatus.Denied,
                            Reason = "Timeout expired"
                        });
                });

            return tcs.Task;
        }

        public void SetCredential(AccessModel request)
        {
            if (!_requestMap.TryRemove(request.Resource, out var tcs)) return;

            var newCred = new RequestCredential { Resource = request.Resource };
            if (request.Action == AccessAction.Grant)
            {
                newCred.Status = CredentialStatus.Granted;
            }
            else if (request.Action == AccessAction.Deny)
            {
                newCred.Status = CredentialStatus.Denied;
                newCred.Reason = "Denied by user";
            }

            tcs.TrySetResult(newCred);
        }
    }
}
