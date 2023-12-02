namespace AppView.Models.DetailsBillViewModel
{
	public class OrderStatusViewModel
	{
        public int Status { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime ConfirmationDate { get; set; }
        public DateTime DeliveryDate { get; set; }
        public DateTime SuccessDate { get; set; }
        public DateTime CancelDate { get; set; }
        public DateTime UpdateDate { get; set; }
    }
}
