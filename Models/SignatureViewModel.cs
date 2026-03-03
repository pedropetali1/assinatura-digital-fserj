namespace EmailSignatureApp.Models
{
    public class SignatureViewModel
    {
        public string DisplayName { get; set; } = string.Empty;
        public string Mail { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Office { get; set; } = string.Empty;
        public string SignatureHtml { get; set; } = string.Empty;
    }
}