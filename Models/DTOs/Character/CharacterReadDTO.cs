using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using MovieChatacterAPI.Models.Domain;

namespace MovieChatacterAPI.Models
{
    public class CharacterReadDTO
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

        public List<int> Movies { get; set; }
    }
}
