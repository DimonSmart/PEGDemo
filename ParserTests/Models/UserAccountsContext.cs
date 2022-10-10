using Microsoft.EntityFrameworkCore;

namespace ParserTests.Models
{
    public class UserAccountsContext : DbContext
    {
        public DbSet<UserAccountDAOModel> UserAccounts { get; set; }

        public UserAccountsContext(DbContextOptions options) : base(options)
        {
        }
    }
}
