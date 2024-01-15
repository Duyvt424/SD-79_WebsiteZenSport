namespace AppView.Models.DashBoardViewModel
{
	public class updateEmployeeViewModel
	{
        public Guid EmployeeId { get; set; }
        public string Image { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Fullname { get; set; }
        public string Password { get; set; }
        public string PhoneNumber { get; set; }
        public string RoleName { get; set; }
        public string Address { get; set; }
        public string Note { get; set; }
        public DateTime CreateDate { get; set; }
        public int Status { get; set; }
    }
}
