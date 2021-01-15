using JWTAuth.Models;
using System;
using System.Net.Http;

namespace JWTAuth
{
    public class Program
    {
        /// <summary>
        /// Dependencies: System.IdentityModel.Tokens.Jwt & System.Net.Http.Formatting.Extension
        /// Steps to hit Business/List endpoint:
        /// 1. Get credentials/keys from Dev console and add it to App config
        /// 2. Construct the JWS using the credentials and hit OAuth API 
        /// 3. Access token returned from OAuth API will be valid for one hour
        /// 4. Call Business/List endpoint using Access token (JWT) in the header
        /// 5. Read the response from Business/List endpoint
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {
            //Get URLs from App.Config
            string oAuthApiUrl = AuthGenerator.GetAppSettings("OAuthApiUrl");
            string apiUrl = AuthGenerator.GetAppSettings("TBSApiUrl");

            //Call OAuth API 
            using (var oAuthClient = new HttpClient())
            {
                string oAuthRequestUri = AuthGenerator.GetAppSettings("OAuthApiMethodRoute");
                oAuthClient.BaseAddress = new Uri(oAuthApiUrl);

                //Generate JWS and get access token (JWT)
                AuthGenerator.GenerateJWSAndGetAccessToken(oAuthClient);

                //Read OAuth API response
                var response = oAuthClient.GetAsync(oAuthRequestUri).Result;
                if (response != null && response.IsSuccessStatusCode)
                {
                   var oauthApiResponse = response.Content.ReadAsAsync<AccessTokenResponse>().Result;
                    if (oauthApiResponse!=null && oauthApiResponse.StatusCode==200)
                    {
                        //Get Access token from OAuth API response
                        string accessToken = oauthApiResponse.AccessToken;
                        //Access token is valid for one hour. After that call OAuth API again & get new Access token.

                        if (!string.IsNullOrWhiteSpace(accessToken))
                        {
                            //Call TaxBandits API using the Access token
                            //Access token is valid for one hour. After that call OAuth API again & get new Access token.
                            using (var apiClient = new HttpClient())
                            {
                                string requestUri = "business/list?Page=1&FromDate=null&ToDate=null&PageSize=50";
                                apiClient.BaseAddress = new Uri(apiUrl);

                                //Construct HTTP headers
                                //If Access token got expired, call OAuth API again & get new Access token.
                                AuthGenerator.ConstructHeadersWithAccessToken(apiClient, accessToken);

                                var apiResponse = apiClient.GetAsync(requestUri).Result;
                                if (apiResponse != null && apiResponse.IsSuccessStatusCode)
                                {
                                    //Read response from TaxBandits API
                                    var listResponse = apiResponse.Content.ReadAsAsync<BusinessListResponse>().Result;
                                    if (listResponse != null)
                                    {
                                        BusinessListResponse fullList = new BusinessListResponse();
                                        fullList = listResponse;
                                        Console.WriteLine("No of Businesses: "+ fullList.TotalRecords.ToString());
                                    }
                                    else
                                    {
                                        Console.WriteLine("Error occurred");
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
