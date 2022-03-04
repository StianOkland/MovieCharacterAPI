using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MovieChatacterAPI.Models.Domain
{
    public class Franchise
    {
        public int Id { get; set; }

        [MaxLength(50)]
        public string Name { get; set; }
        
        [MaxLength(50)]
        public string Description { get; set; }
        public ICollection<Movie> Movies { get; set; }
    }
}
