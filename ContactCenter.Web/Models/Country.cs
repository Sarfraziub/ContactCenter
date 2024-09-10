using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ContactCenter.Data.Models
{
    public class Country
    {
        [Key]
        public string Id { get; set; }
        [Required]
        public string Name { get; set; }

        public virtual ICollection<EdrsmUser> EdrsmUsers { get; set; }
    }
}
