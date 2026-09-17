using System.ComponentModel.DataAnnotations;
namespace ResearchFlow.ViewModels;
public class StudyFormViewModel : IValidatableObject 
{ 
   
    [Required(ErrorMessage = "عنوان الدراسة مطلوب"), StringLength(150, MinimumLength = 3)] 
    public string Title { get; set; } = string.Empty;
   
    [StringLength(2000)]
    public string? Description { get; set; } 
   
    [DataType(DataType.Date)] 
    public DateTime StartDate { get; set; } = DateTime.Today; 
   
   
    [DataType(DataType.Date)] 
    public DateTime ExpectedEndDate { get; set; } = DateTime.Today.AddMonths(3); 
    public IEnumerable<ValidationResult> Validate(ValidationContext c) 
    { 
        if (ExpectedEndDate < StartDate)
            yield return new("تاريخ النهاية لا يسبق البداية", [nameof(ExpectedEndDate)]);
    }
}