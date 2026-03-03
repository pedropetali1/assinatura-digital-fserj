namespace EmailSignatureApp.Models
{
    public class LdapSettings
    {
        public string Server { get; set; } = string.Empty;
        public int Port { get; set; } = 389;
        public string BaseDN { get; set; } = string.Empty;
        public string ServiceAccountDN { get; set; } = string.Empty;
        public string ServiceAccountPassword { get; set; } = string.Empty;
    }
}