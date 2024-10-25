using System.ComponentModel.DataAnnotations;

namespace _3_Calculator.Models
{
    public class InputDto
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Expression must be not null/empty string")]
        public string Expression { get; set; }
    }
}
