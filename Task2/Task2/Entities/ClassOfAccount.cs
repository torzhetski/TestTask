namespace Task2.Entities
{
    public class ClassOfAccount
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public override bool Equals(object? obj)
        {
            return ((ClassOfAccount)obj).Id == this.Id ;
        }
        public override int GetHashCode() 
        {
            return this.Id.GetHashCode() ;
        }
    }
}
