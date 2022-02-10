var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


app.MapGet("/weatherforecast", () =>
{
    
})
.WithName("GetWeatherForecast");

app.Run();





//---------------------
//Classes
//---------------------

public class UserWallet
{
    public Guid UserID { get; set; }
    public Guid WalletID { get; set; }

    public User User { get; set; }
    public Wallet Wallet { get; set; }
}

public class User69
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}

public class Wallet
{
    public Guid WalletID { get; set; }
    public int Balance { get; set; }
    public String WalletType { get; set; }
    
}

public class Ledger
{
    public int LedgerID { get; set; }
    public Guid WalletID { get; set; }
    public String TransactionAction { get; set; }
    public int Amount { get; set; }
    public String ProcessStatus { get; set; }
    public int ParentID { get; set; }
    public Wallet Wallet { get; set; }
}
    public Guid UserID { get; set; }
    public String Email { get; set; }
}

