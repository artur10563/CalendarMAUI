namespace RougeTracker.Messages
{
    public class MonthGrossChangedMessage
    {
        public int Year { get; set; }
        public int Month { get; set; }
        public decimal Loss { get; set; }
        public decimal Income { get; set; }
    }
}
