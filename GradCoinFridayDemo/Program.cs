using GradCoinFridayDemo;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
IConfigurationRoot configuration = new ConfigurationBuilder()
.SetBasePath(Directory.GetCurrentDirectory())
.AddJsonFile("appsettings.json")
.Build();
var connectionString = configuration.GetConnectionString("Db_Connection");
builder.Services.AddDbContext<GradCoinFridayDemoDbContext>(x => x.UseSqlServer(connectionString));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();



app.MapPost("/wallet/transfer", async (TransferDto transerDto, GradCoinFridayDemoDbContext _dbContext) =>
{

})
.WithName("TranserCoin");

app.MapGet("/wallet/{id}", async(Guid id, GradCoinFridayDemoDbContext _dbContext) =>
{
    var ReturnWallet = await _dbContext.wallets.Where(x => x.WalletID == id).FirstOrDefaultAsync();
    return ReturnWallet;    

   
})
.WithName("GetWalletByID");

app.MapGet("/ledger", async (GradCoinFridayDemoDbContext _dbContext) =>
{
    var ledgerEntries = await _dbContext.ledgers.ToListAsync();
    return ledgerEntries;
}).WithName("GetLedger");

app.Run();





//---------------------
//Classes
//---------------------


public class User
{
    [Key]
    public Guid UserID { get; set; }
    public String Email { get; set; }
}

public class Wallet
{
    [Key]
    public Guid WalletID { get; set; }
    public int Balance { get; set; }
    public String WalletType { get; set; }
    
}

public class Ledger
{
    [Key]
    public int LedgerID { get; set; }
    public Guid WalletID { get; set; }
    public String TransactionAction { get; set; }
    public int Amount { get; set; }
    public String ProcessStatus { get; set; }
    public int ParentID { get; set; }
    public Wallet Wallet { get; set; }
}

public class Transaction
{
    [Key]
    public Guid TransactionID { get; set; }
    public DateTime DateTime { get; set; }
    public Guid SenderWalletID { get; set; }
    public Guid ReceiverWalletID { get; set; }
    public Decimal Amount { get; set; }
}
    

