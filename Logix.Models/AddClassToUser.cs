using System.ComponentModel.DataAnnotations;

namespace Logix.Models
{
    public class AddClassToUser
    {
        [Required]
        public int ClassId { get; set; }
        [Required]
        public int UserId { get; set; }
    }
}
