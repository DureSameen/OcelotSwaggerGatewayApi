namespace Swagger.API
{
    public class APIClient
    {
        /// <summary>
        /// 
        /// </summary>
        public APIClient()
        {
        }
        public APIClient(string name, string secret, string idpUrl)
        {
            Name = name;
            Secret = secret;
            IdpUrl = idpUrl;
        }

        /// <summary>
        /// 
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Secret { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string IdpUrl { get; set; }

    }
}
