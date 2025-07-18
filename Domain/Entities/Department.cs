using System.ComponentModel.DataAnnotations;
 
 namespace Domain.Entities;
 
 public class Department
 {
     public int Id { get; set; }
     [Required]
     public string Name { get; set; }
     public List<SubDepartment>? SubDepartments { get; set; }
     public List<User>? Users { get; set; }
 }