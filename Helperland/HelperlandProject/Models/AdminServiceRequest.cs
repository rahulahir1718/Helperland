namespace HelperlandProject.Models
{
    public class AdminServiceRequest
    {
        public int serviceRequestId { get; set; }

        public string serviceDate { get; set; }

        public string serviceTime   { get; set; }

        //public string serviceStartTime { get; set; }

        //public string serviceEndTime   { get; set; }

        public string customerName { get; set; }

        public string customerAddress { get; set; }

        public string? spName { get; set; }

        public string? spAvtar { get; set; }

        public decimal? spRating  { get; set; }

        public string totalAmount { get; set; }

        public string status { get; set; }
    }
}
