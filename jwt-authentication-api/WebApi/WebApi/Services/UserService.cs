using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;
using WebApi.Helpers;
using WebApi.Models;


namespace WebApi.Services
{
    public class UserService : IUserService
    {
        private readonly IMongoCollection<User> _users;
        private readonly AppSettings _appSettings;

        public UserService(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
            var client = new MongoClient(_appSettings.ConnectionString);
            var database = client.GetDatabase(_appSettings.Database);
            _users = database.GetCollection<User>("Users");
        }

        public async Task<User> Authenticate(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                return null;

            var user = await _users.Find(x => x.Username == username)
               .FirstOrDefaultAsync();

            // return null if user not found
            if (user == null)
                return null;

            // check if password is correct
            if (!SecurePasswordHash.Verify(password, user.Password))
                return null;

            // authentication successful so generate jwt token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Id)
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            user.Token = tokenHandler.WriteToken(token);
            user.Password = null;

            return user;
        }

        public async Task<IEnumerable<User>> GetUsers()
        {
            // return users without passwords
            return await _users.Find(x => x.Password != null).ToListAsync();
        }

        public async Task<User> Create(User user, string password)
        {
            // validation
            if (string.IsNullOrWhiteSpace(password))
                throw new AppException("Password is required");

            if (_users.Find(x => x.Username == user.Username).Any())
                throw new AppException("Username \"" + user.Username + "\" is already taken");

            string passwordHash = SecurePasswordHash.Hash(password);
            user.Password = passwordHash;

            await _users.InsertOneAsync(user);
            return user;
        }

    }
}
