using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Models
{
    public class PocetakPrikazivanja
    {
        [Key]
        public int ID { get; set; }

        [Required]
        [RegularExpression("^([0-1]?[0-9]|2[0-3]):[0-5][0-9]$")]
        public string Sati { get; set; }

        public Datum Datum { get; set; }

        [JsonIgnore]
        public List<Spoj> BioskopiFilmovi { get; set; }
    }
}