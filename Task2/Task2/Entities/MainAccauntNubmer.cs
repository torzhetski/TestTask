namespace Task2.Entities
{
    public class MainAccauntNubmer
    {
        public int AccountNumber { get; set; }

        public override bool Equals(object? obj)
        {
            return ((MainAccauntNubmer)obj).AccountNumber == this.AccountNumber;
        }

        public override int GetHashCode()
        {
            return AccountNumber.GetHashCode();
        }
    }
}
