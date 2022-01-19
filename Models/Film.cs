using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Http;

namespace Models
{
    public class Film
    {
        [Key]
        public int ID { get; set; }

        [Required]
        [MaxLength(50)]
        public string Naziv { get; set; }

        public string Image { get; set; }

        [MaxLength(50)]
        public string OriginalniNaziv { get; set; }
        public int Trajanje { get; set; }
        public int Godina { get; set; }

        [MaxLength(50)]
        public string ZemljaPorekla { get; set; }

        [MaxLength(50)]
        public string Reziser { get; set; }

        [MaxLength(100)]
        public string Uloge { get; set; }

        [MaxLength(50)]
        public string Zanr { get; set; }

        [JsonIgnore]
        public List<Spoj> FilmBioskop { get; set; }
    }
}