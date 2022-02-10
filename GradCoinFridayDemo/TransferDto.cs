namespace GradCoinFridayDemo
{
    public class TransferDto
    {
        public Guid PaymentWalletID { get; set; }
        public List<Debtors> Debtors { get; set; }
    }

    public class Debtors
    {
        public Guid WalletID { get; set; }
        public Decimal Amount { get; set; }
    }
}
