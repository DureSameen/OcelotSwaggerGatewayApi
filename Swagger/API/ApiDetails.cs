namespace Swagger.API
{
    public class ApiDetails
    {
        public ApiDetails()
        {
        }

        public ApiDetails(string name, string scope, string title, string version, string authorizationFLow, string clientId)
        {
            Name = name;
            Scope = scope;
            Title = title;
            Version = version;
            AuthorizationFLow = authorizationFLow;
            ClientId = clientId;
        }

        /// <summary>
        /// Name 
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Scope { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Version { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string AuthorizationFLow { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string ClientId { get; set; }

    }
}
