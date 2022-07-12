﻿

namespace FirstTestWithHelaly.Models
{
    public class Genre
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public byte Id { get; set; }
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
    }
}
