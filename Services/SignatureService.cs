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
            var logoUrl = _settings.LogoUrl;

            return $"""
            <table cellpadding="0" cellspacing="0" border="0"
                  style="font-family: Arial, sans-serif; font-size: 12px;
                          color: #333333; border-collapse: collapse;">
              <tr>
                <!-- Coluna do Logo -->
                <td style="padding-right: 18px;
                          border-right: 3px solid #1F4E79;
                          vertical-align: middle;
                          text-align: center;">
                  <img src="{logoUrl}"
                      width="130" alt="Logo Institucional"
                      style="display: block; border: 0;" />
                </td>

                <!-- Coluna dos dados -->
                <td style="padding-left: 18px; vertical-align: middle;">

                  <p style="margin: 0 0 2px 0; font-size: 15px;
                            font-weight: bold; color: #1F4E79;">
                    {user.DisplayName}
                  </p>

                  {(!string.IsNullOrEmpty(user.Description) ? $"""
                  <p style="margin: 0 0 2px 0; font-size: 12px; color: #2E75B6; font-weight: bold;">
                    {user.Description}
                  </p>
                  """ : "")}

                  {(!string.IsNullOrEmpty(user.Office) ? $"""
                  <p style="margin: 0; font-size: 12px; color: #555555;">
                    {user.Office}
                  </p>
                  """ : "")}

                </td>
              </tr>
            </table>
            """;
        }
    }
}