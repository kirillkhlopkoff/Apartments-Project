using ApiApartmentIdentity.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ApiApartmentIdentity.Context
{
    public class UsersDbContext :IdentityDbContext<User,Role, string>
    {
        private readonly IConfiguration _configuration;
        public UsersDbContext(DbContextOptions<UsersDbContext> options, IConfiguration configuration) : base(options)
        {
            _configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_configuration.GetConnectionString("DefaultConnection"));
        }
    }
}
