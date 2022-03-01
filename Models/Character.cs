

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MovieChatacterAPI.Models
{
    public class Character
    {
        public int Id { get; set; }
        
        [MaxLength(50)]
        public string FullName { get; set; }
        
        [MaxLength(50)]
        public string Alias { get; set; }

        [MaxLength(50)]
        public string Gender { get; set; }

        [MaxLength(100)]
        public string Picture { get; set; }
        public ICollection<Movie> Movies { get; set; }

    }
}
