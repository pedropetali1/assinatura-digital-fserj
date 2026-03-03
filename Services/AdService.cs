using System.DirectoryServices.Protocols;
using System.Net;
using EmailSignatureApp.Models;
using Microsoft.Extensions.Options;

namespace EmailSignatureApp.Services
{
    public class AdService
    {
        private readonly LdapSettings _settings;
        private readonly ILogger<AdService> _logger;

        public AdService(IOptions<LdapSettings> settings, ILogger<AdService> logger)
        {
            _settings = settings.Value;
            _logger = logger;
        }

        public UserAdModel? GetUserByUsername(string username)
        {
            var samAccountName = username.Contains('\\')
                ? username.Split('\\')[1]
                : username;

            try
            {
                var credential = new NetworkCredential(
                    _settings.ServiceAccountDN,
                    _settings.ServiceAccountPassword
                );

                using var connection = new LdapConnection(
                    new LdapDirectoryIdentifier(_settings.Server, _settings.Port)
                );

                connection.Credential = credential;
                connection.AuthType = AuthType.Basic;
                connection.SessionOptions.ProtocolVersion = 3;
                connection.Bind();

                var filter = $"(&(objectClass=user)(sAMAccountName={samAccountName}))";
                var attributes = new[] { "displayName", "mail", "description", "physicalDeliveryOfficeName" };

                var request = new SearchRequest(
                    _settings.BaseDN,
                    filter,
                    SearchScope.Subtree,
                    attributes
                );

                var response = (SearchResponse)connection.SendRequest(request);

                if (response.Entries.Count == 0)
                {
                    _logger.LogWarning("Usuário '{Username}' não encontrado no AD.", samAccountName);
                    return null;
                }

                var entry = response.Entries[0];

                return new UserAdModel
                {
                    DisplayName = GetAttribute(entry, "displayName"),
                    Mail       = GetAttribute(entry, "mail"),
                    Description = GetAttribute(entry, "description"),
                    Office     = GetAttribute(entry, "physicalDeliveryOfficeName")
                };
            }
            catch (LdapException ex)
            {
                _logger.LogError(ex, "Erro ao conectar ao AD para o usuário '{Username}'.", samAccountName);
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro inesperado ao buscar usuário '{Username}' no AD.", samAccountName);
                return null;
            }
        }

        public Dictionary<string, string> GetAllAttributes(string username)
        {
            var samAccountName = username.Contains('\\')
                ? username.Split('\\')[1]
                : username;

            var result = new Dictionary<string, string>();

            try
            {
                var credential = new NetworkCredential(
                    _settings.ServiceAccountDN,
                    _settings.ServiceAccountPassword
                );

                using var connection = new LdapConnection(
                    new LdapDirectoryIdentifier(_settings.Server, _settings.Port)
                );

                connection.Credential = credential;
                connection.AuthType = AuthType.Basic;
                connection.SessionOptions.ProtocolVersion = 3;
                connection.Bind();

                var filter = $"(&(objectClass=user)(sAMAccountName={samAccountName}))";

                var request = new SearchRequest(
                    _settings.BaseDN,
                    filter,
                    SearchScope.Subtree
                    // sem especificar atributos = retorna TODOS
                );

                var response = (SearchResponse)connection.SendRequest(request);

                if (response.Entries.Count == 0)
                    return result;

                var entry = response.Entries[0];

                foreach (DirectoryAttribute attr in entry.Attributes.Values)
                {
                    try
                    {
                        var value = attr[0]?.ToString() ?? string.Empty;
                        result[attr.Name] = value;
                    }
                    catch
                    {
                        result[attr.Name] = "(binário/não legível)";
                    }
                }
            }
            catch (Exception ex)
            {
                result["ERRO"] = ex.Message;
            }

            return result;
        }

        private static string GetAttribute(SearchResultEntry entry, string attribute)
        {
            if (entry.Attributes.Contains(attribute) && entry.Attributes[attribute].Count > 0)
                return entry.Attributes[attribute][0]?.ToString() ?? string.Empty;

            return string.Empty;
        }
    }
}