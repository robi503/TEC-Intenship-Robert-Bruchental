using System.ComponentModel.DataAnnotations;

namespace ApiApp.ObjectModel
{
    public class PersonCreate
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Surname { get; set; }
        [Required]
        public int Age { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        public int PositionId { get; set; }
        [Required]
        public int SalaryId { get; set; }
    }
}
