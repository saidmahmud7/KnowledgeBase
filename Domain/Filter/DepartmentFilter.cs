namespace Domain.Filter;

public class DepartmentFilter : BaseFilter  
{
   public int? Id { get; set; } // ← вот это поле мы обсуждаем
   public string? Name { get; set; } 

}