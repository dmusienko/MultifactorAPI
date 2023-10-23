namespace MultifactorAPI.Domain
{
    public class RequestCredential
    {
        public string? Resource { get; set; }
        public CredentialStatus Status { get; set; }
        public string Reason { get; set; } = string.Empty;
    }
}