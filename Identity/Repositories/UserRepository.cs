//using Domain.Entities;
//using Domain.Repositories;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.Configuration;
//using Microsoft.IdentityModel.Tokens;
//using System.IdentityModel.Tokens.Jwt;
//using System.Security.Claims;
//using System.Text;

//namespace Identity.Repositories
//{
//    public class UserRepository : IUserRepository
//    {
//        private readonly UsersDbContext usersDbContext;
//        private readonly IConfiguration configuration;

//        public UserRepository(UsersDbContext usersDbContext, IConfiguration configuration)
//        {
//            this.usersDbContext = usersDbContext;
//            this.configuration = configuration;
//        }

//        public async Task<string> Login(User user)
//        {
//            var existingUser = await usersDbContext.Users.FirstOrDefaultAsync(u => u.Email == user.Email);
//            Console.WriteLine(BCrypt.Net.BCrypt.EnhancedVerify("string", existingUser.PasswordHash));
//            if (existingUser == null || !BCrypt.Net.BCrypt.EnhancedVerify("string", existingUser.PasswordHash)) 
//            {
//                throw new UnauthorizedAccessException("Invalid credentials");
//            }

//            var tokenHandler = new JwtSecurityTokenHandler();
//            var key = Encoding.ASCII.GetBytes(configuration["Jwt:Key"]!);
//            var tokenDescriptor = new SecurityTokenDescriptor
//            {
//                Subject = new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, existingUser.Id.ToString()) }),
//                Expires = DateTime.UtcNow.AddMinutes(1),
//                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
//            };

//            var token = tokenHandler.CreateToken(tokenDescriptor);
//            return tokenHandler.WriteToken(token);
//        }


//        public async Task<Guid> Register(User user, CancellationToken cancellationToken)
//        {
//            var existingUser = await usersDbContext.Users.SingleOrDefaultAsync(u => u.Email == user.Email);
//            if (existingUser != null)
//            {
//                throw new InvalidOperationException("A user with this email already exists.");
//            }

//            user.PasswordHash = BCrypt.Net.BCrypt.EnhancedHashPassword(user.PasswordHash, 13);

//            usersDbContext.Users.Add(user);
//            await usersDbContext.SaveChangesAsync(cancellationToken);
//            return user.Id;
//        }

//        public async Task<List<User>> GetAllUsersAsync()
//        {
//            return await usersDbContext.Users.ToListAsync();
//        }
//    }
//}

using Domain.Common;
using Domain.Entities;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Identity.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly UsersDbContext usersDbContext;
        private readonly IConfiguration configuration;

        public UserRepository(UsersDbContext usersDbContext, IConfiguration configuration)
        {
            this.usersDbContext = usersDbContext;
            this.configuration = configuration;
        }

        public async Task<LoginResult> Login(User user)
        {
            var existingUser = await usersDbContext.Users.FirstOrDefaultAsync(u => u.Email == user.Email);
            if (existingUser == null || !BCrypt.Net.BCrypt.Verify(user.PasswordHash, existingUser.PasswordHash))
            {
                throw new UnauthorizedAccessException("Invalid credentials");
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(configuration["Jwt:Key"]!);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, existingUser.Id.ToString()) }),
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Issuer = configuration["Jwt:Issuer"],
                Audience = configuration["Jwt:Audience"]
            };


            var token = tokenHandler.CreateToken(tokenDescriptor);
            return new LoginResult
            {
                Token = tokenHandler.WriteToken(token),
                Admin = existingUser.Admin
            };
        }

        public async Task<Guid> Register(User user, CancellationToken cancellationToken)
        {
            var existingUser = await usersDbContext.Users.SingleOrDefaultAsync(u => u.Email == user.Email);
            if (existingUser != null)
            {
                throw new InvalidOperationException("A user with this email already exists.");
            }
            Console.WriteLine(user.PasswordHash);

            user.PasswordHash = user.PasswordHash;

            usersDbContext.Users.Add(user);
            await usersDbContext.SaveChangesAsync(cancellationToken);
            return user.Id;
        }

        public async Task<List<User>> GetAllUsersAsync()
        {
            return await usersDbContext.Users.ToListAsync();
        }
    }
}

