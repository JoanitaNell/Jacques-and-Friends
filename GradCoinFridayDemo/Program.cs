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
    Decimal amount = 0; 
    foreach (var debtor in transerDto.Debtors)
    {
        amount+= debtor.Amount;
    }

    var ledgerCT = new Ledger
    {
        WalletID = transerDto.PaymentWalletID,
        TransactionAction = "CT",
        Amount = amount,
        ProcessStatus = "Captured",
        ParentID= -1,  
    };

    _dbContext.ledgers.Add(ledgerCT);
    _dbContext.SaveChanges();

    var parentId = ledgerCT.LedgerID;

    foreach(var debtor in transerDto.Debtors)
    {
        var ledgerDT = new Ledger
        {
            WalletID = debtor.WalletID,
            TransactionAction = "DT",
            Amount = debtor.Amount,
            ProcessStatus = "Captured",
            ParentID = -1,
        };

        _dbContext.ledgers.Add(ledgerDT);
        _dbContext.SaveChanges();
    }

    var ctWallet = await _dbContext.wallets.FindAsync(transerDto.PaymentWalletID);
    if (ctWallet != null)
    {
        foreach(var debtor in transerDto.Debtors)
        {
            var dtWallet = await _dbContext.wallets.FindAsync(debtor.WalletID);
            if (dtWallet != null)
            {
                Decimal amount2= debtor.Amount;
                var id = new Guid();
                var date = new DateTime();
                var transactionKT = new Transaction
                {
                    TransactionID = id,
                    TransactionDate = date,
                    SenderWalletID = transerDto.PaymentWalletID,
                    ReceiverWalletID = debtor.WalletID,
                    Amount = amount,
                };

                var transactionDT = new Transaction
                {
                    TransactionID = id,
                    TransactionDate = date,
                    SenderWalletID = transerDto.PaymentWalletID,
                    ReceiverWalletID = debtor.WalletID,
                    Amount = amount,
                };

                ctWallet.Transactions.Add(transactionKT);
                ctWallet.Balance -= amount2;

                dtWallet.Transactions.Add(transactionDT);
                dtWallet.Balance += amount2;

                await _dbContext.SaveChangesAsync();
            }
            
        }

        ledgerCT.ProcessStatus = "Processed";
        var ledgersDT = await _dbContext.ledgers.Where(x => x.ParentID == parentId).ToListAsync();
        foreach (var ledger in ledgersDT)
        {
            ledger.ProcessStatus = "Processed";
        }
        
        await _dbContext.SaveChangesAsync();
       
    }
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
})
.WithName("GetLedger");

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
    public Decimal Balance { get; set; }
    public String WalletType { get; set; }
    public List<Transaction> Transactions { get; set; }
    
}

public class Ledger
{
    [Key]
    public int LedgerID { get; set; }
    public Guid WalletID { get; set; }
    public String TransactionAction { get; set; }
    public Decimal Amount { get; set; }
    public String ProcessStatus { get; set; }
    public int ParentID { get; set; }
    public Wallet Wallet { get; set; }
}

public class Transaction
{
    [Key]
    public Guid TransactionID { get; set; }
    public DateTime TransactionDate { get; set; }
    public Guid SenderWalletID { get; set; }
    public Guid ReceiverWalletID { get; set; }
    public Decimal Amount { get; set; }
}
    

