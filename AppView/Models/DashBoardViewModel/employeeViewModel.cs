namespace AppView.Models.DashBoardViewModel
{
	public class employeeViewModel
	{
		public Guid EmployeeId { get; set; }
		public string Image { get; set; }
		public string UserName { get; set; }
		public string Email { get; set; }
		public string Fullname { get; set; }
		public DateTime CreateDate { get; set; }
		public int Status { get; set; }
	}
}
