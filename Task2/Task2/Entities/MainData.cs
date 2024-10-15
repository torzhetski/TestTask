namespace Task2.Entities
{
    public class MainData
    {
        public int SubAccountNumber {  get; set; }
        public double InSaldoActive { get; set; }
        public double InSaldoPassive { get; set; }
        public double OutSaldoActive { get; set; }
        public double OutSaldoPassive { get; set; }
        public double TurnoverDebit { get; set; }
        public double TurnoverCredit { get; set; }

        public int MainAccauntNuberId { get; set; }

        public int ClassOfAccountId { get; set; }
        public ClassOfAccount? ClassOfAccount { get; set; }
    }
}
