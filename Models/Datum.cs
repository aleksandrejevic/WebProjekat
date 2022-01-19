using System;
using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class Datum
    {
        [Key]
        public int ID { get; set; }

        [DataType(DataType.Date)]
        [CustomDateAttribute]
        [Required]
        public DateTime DatumPrikazivanja { get; set; }
    }

    public class CustomDateAttribute : RangeAttribute
    {
    public CustomDateAttribute()
        : base(typeof(DateTime), 
                DateTime.Now.ToShortDateString(),
                DateTime.Now.AddDays(7).ToShortDateString()) 
    { } 
    }
}