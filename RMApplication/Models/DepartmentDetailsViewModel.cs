namespace RMApplication.Models
{
    public class DepartmentDetailsViewModel
    {
        public Department Department { get; set; }
        public List<Department> ParentDepartments { get; set; }
    }
}
