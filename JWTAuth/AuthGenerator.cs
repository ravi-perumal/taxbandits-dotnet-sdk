using System;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Configuration;

namespace JWTAuth
{
    public static class AuthGenerator
    {
        #region Generate JWS and get access token.   
        /// <summary>
        /// Generates the JWS and get access token.
        /// </summary>
        /// <param name="client">The client.</param>
        public static void GenerateJWSAndGetAccessToken(HttpClient client)
        {
            #region Get Credentials from TaxBandits Dev Console

            string userToken = GetAppSettings("UserToken");
            string clientId = GetAppSettings("ClientId");
            string clientSecret = GetAppSettings("ClientSecret");

            #endregion

            var clientSecretSymmetricKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(clientSecret));
            var signingCredentials = new SigningCredentials(clientSecretSymmetricKey, SecurityAlgorithms.HmacSha256);
            var claims = new[] {
                                    new Claim(JwtRegisteredClaimNames.Iss, clientId),
                                    new Claim(JwtRegisteredClaimNames.Sub, clientId),
                                    new Claim(JwtRegisteredClaimNames.Aud, userToken),
                                    new Claim(JwtRegisteredClaimNames.Iat, ConvertToUnixEpochTimestamp(DateTime.Now).ToString()),
                               };

            var token = new JwtSecurityToken(null, null, claims, expires: DateTime.Now.AddMinutes(5), signingCredentials: signingCredentials);
            var jws = new JwtSecurityTokenHandler().WriteToken(token);

            client.DefaultRequestHeaders.Clear();

            //Add the JWS constructed to the Authentication header
            client.DefaultRequestHeaders.Add("Authentication", jws);
        }
        #endregion

        #region Convert To Unix Epoch Timestamp        
        /// <summary>
        /// Converts to unix epoch timestamp.
        /// </summary>
        /// <param name="date">The date.</param>
        /// <returns></returns>
        public static double ConvertToUnixEpochTimestamp(DateTime date)
        {
            DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            TimeSpan diff = date.ToUniversalTime() - origin;
            return Math.Floor(diff.TotalSeconds);
        }
        #endregion

        #region Construct Headers With Access Token        
        /// <summary>
        /// Constructs the headers with access token.
        /// </summary>
        /// <param name="client">The client.</param>
        /// <param name="accessToken">The access token.</param>
        public static void ConstructHeadersWithAccessToken(HttpClient client, string accessToken)
        {
            client.DefaultRequestHeaders.Clear();
            var jwtAccessToken = $"Bearer {accessToken}";
            client.DefaultRequestHeaders.Add("Accept", "application/json");
            client.DefaultRequestHeaders.Add("Authorization", jwtAccessToken);
        }
        #endregion

        #region Get App Settings
        /// <summary>
        /// Get the appsetting value from the web.config
        /// </summary>
        /// <param name="appKey">appsettings key</param>
        /// <returns></returns>
        public static string GetAppSettings(string appKey)
        {
            object value = null;
            value = ConfigurationManager.AppSettings[appKey];
            if (value != null)
            {
                return value.ToString();
            }
            return string.Empty;
        }
        #endregion
    }
}
