using System.ComponentModel.DataAnnotations;

namespace _3_Calculator.Entities
{
    public class CalculationResult
    {
        [Key]
        public int ID { get; set; }
        
        public string Expression { get; set; }

        public double Result { get; set; }
    }
}
