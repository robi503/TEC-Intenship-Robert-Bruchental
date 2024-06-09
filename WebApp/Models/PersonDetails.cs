using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;

namespace WebApp.Models
{
    public class PersonDetails
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
