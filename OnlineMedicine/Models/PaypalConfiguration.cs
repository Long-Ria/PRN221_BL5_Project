using PayPal.Api;

namespace OnlineMedicine.Models
{
    public static class PaypalConfiguration
    {
        //Variables for storing the clientID and clientSecret key  
        public readonly static string ClientId;
        public readonly static string ClientSecret;
        //Constructor  
        static PaypalConfiguration()
        {
            var config = GetConfig();
            ClientId = "AU1BFWXfRbw6-wliKXWloJ0SuJVVaZmzjKl0pK5ULNjlVYHscpJpN1U86xUGlbvOWY_gJqXA22yjvzrW";
            ClientSecret = "EF8wQv_qMIh1yGPyDtsEnlNeC58CCopeqs4KpZEOWx74vFx6k7z7tNln80S8ZKzxRINBKeEjjCzLk1lp";
        }
        // getting properties from the web.config  
        public static Dictionary<string, string> GetConfig()
        {
            return PayPal.Api.ConfigManager.Instance.GetProperties();
        }
        private static string GetAccessToken()
        {
            // getting accesstocken from paypal  
            string accessToken = new OAuthTokenCredential(ClientId, ClientSecret, GetConfig()).GetAccessToken();
            return accessToken;
        }
        public static APIContext GetAPIContext()
        {
            // return apicontext object by invoking it with the accesstoken  
            APIContext apiContext = new APIContext(GetAccessToken());
            apiContext.Config = GetConfig();
            return apiContext;
        }
    }

}
