using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using MovieChatacterAPI.Models.Domain;

namespace MovieChatacterAPI.Models
{
    public class FranchiseEditDTO
    {
        public int Id { get; set; }

        [MaxLength(50)]
        public string Name { get; set; }

        [MaxLength(50)]
        public string Description { get; set; }
        public ICollection<Movie> Movies { get; set; }
    }
}
