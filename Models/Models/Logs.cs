namespace Models.Models
{
    public class Logs
    {
        public int Id { get; set; }

        public string Table { get; set; }

        public string Operation { get; set; }

        public DateTime Time { get; set; }

        public string User { get; set; }

        public string Changes { get; set; }

    }
}
