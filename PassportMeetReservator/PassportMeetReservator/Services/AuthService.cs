using System.Net;
using System.Threading.Tasks;

using Autofac;
using RestSharp;
using Newtonsoft.Json;

using PassportMeetReservator.Data;

namespace PassportMeetReservator.Services
{
    public class AuthService
    {
        private static RestClient Client = new RestClient("http://passportmeetreserver.somee.com/api/");

        private static SaltService SaltService = DependencyHolder.ServiceDependencies.Resolve<SaltService>();
        private static HashingService HashingService = DependencyHolder.ServiceDependencies.Resolve<HashingService>();

        private const string LOG_IN_ENDPOINT = "auth/login";
        public async Task<bool> LogIn(LogInForm logInForm)
        {
            LogInDto dto = new LogInDto();

            dto.Login = logInForm.Login;
            dto.Salt = SaltService.GetRandomSalt();

            string hashedPwd = HashingService.GetHash(logInForm.Password);
            dto.PasswordSalted = SaltService.GetSaltedHash(hashedPwd, dto.Salt);

            RestRequest request = new RestRequest(LOG_IN_ENDPOINT, Method.POST);
            request.AddJsonBody(JsonConvert.SerializeObject(dto));

            IRestResponse response = await Client.ExecuteAsync(request);
            return response.StatusCode == HttpStatusCode.OK;
        }
    }
}
