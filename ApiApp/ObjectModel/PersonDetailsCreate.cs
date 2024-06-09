using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiApp.ObjectModel
{
    public class PersonDetailsCreate
    {
        public int Id { get; set; }
        [Required]
        public DateTime BirthDay { get; set; }
        [Required]
        public string PersonCity { get; set; }

        [ForeignKey("Person")]
        public int PersonId { get; set; }
    }
}
