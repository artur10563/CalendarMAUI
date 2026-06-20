namespace RougeTracker.Messages
{
    public class MonthGrossChangedMessage
    {
        public int Year { get; set; }
        public int Month { get; set; }
        public int Loss { get; set; }
        public int Income { get; set; }
    }
}
