using Microsoft.EntityFrameworkCore;

namespace GradCoinFridayDemo
{
    public class GradCoinFridayDemoDbContext : DbContext
    {
        public GradCoinFridayDemoDbContext(DbContextOptions<GradCoinFridayDemoDbContext> options): base(options)
        {

        }

        public DbSet<Ledger> ledgers { get; set; }
        public DbSet<Transaction> transactions { get; set; }
        public DbSet<User> users { get; set; }
        public DbSet<Wallet> wallets { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();
                var connectionString = configuration.GetConnectionString("Db_Connection");
                optionsBuilder.UseSqlServer(connectionString);
            }
        }


    }
}
