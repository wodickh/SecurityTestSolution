using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
//using Microsoft.IdentityModel.Protocols.WSTrust;
using Newtonsoft.Json;

namespace SecurityTestWPFApplication
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string authority = "https://auth.test.dbb.dk/adfs";
        string resourceURI = "http://buxsecuritytest/webapi";
        string clientID = "E1CF1107-FF90-4228-93BF-26052DD2C714";
        string clientReturnURI = "http://ligegyldiguri/";
        public MainWindow()
        {
            InitializeComponent();
        }

        private UserCredential GetCurrentUser()
        {
            
            UserCredential a = new UserCredential();
            return a;
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            AuthenticationContext ac = new AuthenticationContext(authority, false);
            
            AuthenticationResult ar = ac.AcquireToken(resourceURI, clientID, new Uri(clientReturnURI));
            
            string authHeader = ar.CreateAuthorizationHeader();

            HttpClient client = new HttpClient();
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, "http://localhost:59334/api/test");
            request.Headers.TryAddWithoutValidation("Authorization", authHeader);
            HttpResponseMessage response = await client.SendAsync(request);
            string responseString = await response.Content.ReadAsStringAsync();
            MessageBox.Show(responseString);
        }

        private async void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var ctx = new AuthenticationContext(authority + "/oauth2", false);
            string clientId = "f54e57e1-e4fc-487c-a0c3-1b9389456ab3"; // f54e57e1-e4fc-487c-a0c3-1b9389456ab3
            string redirectionUrl = "https://wsnota113.dbb.dk:44304/test";
            string resource = "https://localhost:44300";
            var authnResult = ctx.AcquireToken(resource, clientId, new Uri(redirectionUrl));
            //UserIdentifier user = new UserIdentifier("msw", );
            //var authnResult2 = ctx.AcquireTokenSilent(resourceURI, clientID, new UserIdentifier())
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authnResult.AccessToken);
            var response = await client.GetAsync("http://localhost:59334/api/test");
            string responseString = await response.Content.ReadAsStringAsync();
            MessageBox.Show(responseString);
        }

        private async void Button_OAuth2Flow(object sender, RoutedEventArgs eventArgs)
        {
//            var ctx = new AuthenticationContext(authority + "/oauth2", false);
           
//            var authResult = ctx.AcquireToken(@"https://localhost:44300/", "f54e57e1-e4fc-487c-a0c3-1b9389456ab3",
//                new Uri(@"http://wsnota113.dbb.dk:44300"));

            HttpClient client = new HttpClient();
            var builder = new UriBuilder(authority+"/oauth2/authorize");
            var query = HttpUtility.ParseQueryString(builder.Query);
            query["response_type"] = "code";
            query["client_id"] = "f54e57e1-e4fc-487c-a0c3-1b9389456ab3";
            query["resource"] = @"https://localhost:44300/";
            query["redirect_url"] = "http://wsnota113.dbb.dk:52901";
            builder.Query = query.ToString();
            string url = builder.ToString();

            var tokenResponse = await client.GetAsync(url);
            
            string token = await tokenResponse.Content.ReadAsStringAsync();
            var request = new TokenRequest()
            {
                grant_type = "code",
                client_id = "",
                code = "john",
                redirect_uri = @"https://wsnota113.dbb.dk:44300/oauth/token"
            };

            
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer","JOHN");
            var response = await client.PostAsync("http://wsnota113.dbb.dk:52901/oauth2/token", new StringContent(JsonConvert.SerializeObject(request).ToString(), Encoding.UTF8, "application/json"));
            string responseString = await response.Content.ReadAsStringAsync();
            MessageBox.Show(responseString);
        }
        /*
        private async void Buttion_SAMLFlow(object sender, RoutedEventArgs eventArgs)
        {
          
             var binding = new WS2007HttpBinding(SecurityMode.TransportWithMessageCredential); 
             binding.Security.Message.EstablishSecurityContext = false; 
             binding.Security.Transport.ClientCredentialType = HttpClientCredentialType.None; 
             binding.Security.Message.ClientCredentialType = MessageCredentialType.Windows; 
             var factory = new WSTrustChannelFactory( 
                 binding, 
                 new EndpointAddress(AdFsBaseUri + "services/trust/13/windowsmixed")) 
             { 
                 TrustVersion = TrustVersion.WSTrust13 
             }; 
             if (factory.Credentials == null) return null; 
             factory.Credentials.Windows.ClientCredential = CredentialCache.DefaultNetworkCredentials; 
             var rst = new RequestSecurityToken 
             { 
                 RequestType = WSTrust13Constants.RequestTypes.Issue, 
                 AppliesTo = new EndpointAddress(AcsBaseUri), 
                 KeyType = WSTrust13Constants.KeyTypes.Bearer 
             }; 
             var channel = factory.CreateChannel(); 
             var genericToken = channel.Issue(rst) as GenericXmlSecurityToken; 
             if (genericToken == null) return null; 
             return genericToken.TokenXml.OuterXml; 
         } 
        */
        }
      
    
       public class TokenRequest
        {
            public String grant_type { get; set; }
            public String code { get; set; }
            public String client_id { get; set; }
            public String redirect_uri { get; set; }
        }
}
