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
              <tr
                  style="display: flex; align-items: center; gap: 18px;">
                <!-- Coluna do Logo -->
                <td style="
                          vertical-align: middle;
                          text-align: center;">
                  <img src="{logoUrl}"
                      width="136px" alt="Logo Institucional"
                      style="display: block; border: 0;" />
                </td>

                <!-- Coluna dos dados -->
                <td style="padding-left: 18px; vertical-align: top;">

                  <!-- Nome -->
                  <p style="margin-bottom: -6px; font-size: 20px;
                            font-weight: 900; color: #000000;">
                    {user.DisplayName}
                  </p>

                  <!-- Setor -->
                  {(!string.IsNullOrEmpty(user.Office) ? $"""
                  <p style="margin: 0 0 8px 0; font-size: 14px; color: #2E75B6; font-weight: bold;">
                    {user.Office}
                  </p>
                  """ : "")}

                  <!-- Cargo -->
                  {(!string.IsNullOrEmpty(user.Description) ? $"""
                  <p style="margin: 0 0 1px 0; font-size: 12px; color: #555555;">
                    {user.Description}
                  </p>
                  """ : "")}

                  <!-- Divisor -->
                  <table cellpadding="0" cellspacing="0" border="0"
                        style="border-top: 1px solid #cccccc; width: 100%; margin-bottom: 6px;">
                    <tr><td></td></tr>
                  </table>

                  <!-- Nome da Instituição -->
                  <p style="margin: 0 0 2px 0; font-size: 12px;
                            font-weight: 900; color: #1F4E79;">
                    {_settings.InstitutionName}
                  </p>

                  <!-- Endereço -->
                  <p style="margin: 0 0 2px 0; font-size: 11px; color: #000000;">
                    {_settings.Address}
                  </p>

                  <!-- Telefone -->
                  {(!string.IsNullOrEmpty(_settings.Phone) ? $"""
                  <p style="margin: 0 0 2px 0;
                            font-size: 14px;
                            color: #555555;
                            font-weight: 900;">
                    {_settings.Phone}
                  </p>
                  """ : "")}

                </td>
              </tr>
            </table>
            """;
        }
    }
}