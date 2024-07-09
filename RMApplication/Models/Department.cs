using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RMApplication.Models
{
    public class Department
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = null!;
        public string? Logo { get; set; }
        public int? ParentDepartmentId { get; set; }
        [ForeignKey("ParentDepartmentId")]
        public Department ParentDepartment { get; set; }
        public ICollection<Department> SubDepartments { get; set; } = new List<Department>();
    }
}
