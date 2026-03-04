using EmailSignatureApp.Models;
using Microsoft.Extensions.Options;

namespace EmailSignatureApp.Services
{
    public class SignatureService
    {
        private readonly SignatureSettings _settings;

        public SignatureService(IOptions<SignatureSettings> settings)
        {
            _settings = settings.Value;
        }

        public string GenerateHtml(UserAdModel user)
        {
            var setor = !string.IsNullOrEmpty(user.Office)
                ? $"<p style=\"margin:0 0 6px 0;font-size:13px;color:#2E75B6;font-weight:bold\">{user.Office}</p>"
                : "";

            var cargo = !string.IsNullOrEmpty(user.Description)
                ? $"<p style=\"margin:0 0 4px 0;font-size:12px;color:#555\">{user.Description}</p>"
                : "";

            var telefone = !string.IsNullOrEmpty(_settings.Phone)
                ? $"<p style=\"margin:0;font-size:13px;color:#555;font-weight:bold\">{_settings.Phone}</p>"
                : "";

            return $"<table cellpadding=\"0\" cellspacing=\"0\" border=\"0\" style=\"font-family:Arial,sans-serif;font-size:12px;color:#333\">" +
                   $"<tr>" +
                   $"<td style=\"vertical-align:middle;text-align:center\">" +
                   $"<img src=\"{_settings.LogoUrl}\" width=\"120\" alt=\"Logo\" style=\"display:block;border:0\"/>" +
                   $"</td>" +
                   $"<td style=\"padding-left:22px;vertical-align:top;\">" +
                   $"<p style=\"margin:0 0 2px 0;font-size:18px;font-weight:900;color:#000\">{user.DisplayName}</p>" +
                   setor +
                   cargo +
                   $"<p style=\"margin:4px 0 2px 0;font-size:12px;font-weight:bold;color:#1F4E79\">{_settings.InstitutionName}</p>" +
                   $"<p style=\"margin:0 0 2px 0;font-size:11px;color:#000\">{_settings.Address}</p>" +
                   telefone +
                   $"</td>" +
                   $"</tr>" +
                   $"</table>";
        }
    }
}