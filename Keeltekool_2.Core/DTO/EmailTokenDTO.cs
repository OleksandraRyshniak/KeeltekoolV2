namespace Keeltekool_2.Core.DTO
{
    public class EmailTokenDTO
    {
        public string Token { get; set; } = string.Empty;
        public string To { get; set; } = string.Empty;
        public string Subject { get; set; } = string.Empty;
        public string Body { get; set; } = string.Empty;
    }
}