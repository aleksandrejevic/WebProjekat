using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Models
{
    public class Spoj
    {

        [Key]
        public int ID { get; set; }

        [JsonIgnore]
        public Bioskop Bioskop { get; set; }
        public Film Film { get; set; }

        public PocetakPrikazivanja PocetakPrikazivanja { get; set; }
    }
}