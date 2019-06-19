using System.Text;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using MongoDB.Driver;
using Newtonsoft.Json;
using WebApi;
using WebApi.Models;
using System.Collections.Generic;

namespace TestingWebApi
{
    public class UsersControllerTest : IClassFixture<CustomAPIFactory<Startup>>, IDisposable
    {
        private readonly HttpClient _client;

        public UsersControllerTest(CustomAPIFactory<Startup> factory)
        {
            _client = factory.CreateClient();
        }

        public void Dispose()
        {
            // TODO: need to update, shouldn't be hard code
            var client = new MongoClient("mongodb://root:example@localhost:27017");
            var database = client.GetDatabase("Test_DB");
            database.DropCollection("Users");
        }

        private async Task RegisterUser(User user)
        {
            var content = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PostAsync("/api/users/register", content);

            // Must be successful.
            response.EnsureSuccessStatusCode();
        }

        private async Task<string> GetToken(User registereduser)
        {
            // authenticate
            var content = new StringContent(JsonConvert.SerializeObject(registereduser), Encoding.UTF8, "application/json");
            var httpResponse = await _client.PostAsync("/api/users/authenticate", content);

            // Must be successful.
            httpResponse.EnsureSuccessStatusCode();
            var stringResponse = await httpResponse.Content.ReadAsStringAsync();
            var user = JsonConvert.DeserializeObject<User>(stringResponse);

            return user.Token;
        }

        [Fact]
        public async Task TestRegisterUser()
        {
            var userModel = new User
            {
                Username = "nhatthai",
                FirstName = "Nhat",
                LastName = "Thai",
                Password = "nhatthai@123"
            };
            await RegisterUser(userModel);
        }

        [Fact]
        public async Task TestAuthenticationUser()
        {
            // Register user
            var registeredUser = new User
            {
                Username = "testing",
                FirstName = "Test",
                LastName = "Test",
                Password = "testing@123"
            };
            await RegisterUser(registeredUser);

            // Get Token
            string token = await GetToken(registeredUser);
            Assert.NotNull(token);
        }

        [Fact]
        public async Task TestGetListUsers()
        {

            // Register user
            var registeredUser = new User
            {
                Username = "testing1",
                FirstName = "Test1",
                LastName = "Test",
                Password = "testing1@123"
            };
            await RegisterUser(registeredUser);

            // Get Token
            string token = await GetToken(registeredUser);

            // add token into Header
            _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
            var resUsers = await _client.GetAsync("/api/users");

            // Must be successful.
            resUsers.EnsureSuccessStatusCode();

            // Deserialize and examine results.
            var stringResUsers = await resUsers.Content.ReadAsStringAsync();
            var users = JsonConvert.DeserializeObject<IEnumerable<User>>(stringResUsers);
            Assert.NotNull(users);
        }
    }
}
