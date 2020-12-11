using System;
using System.ComponentModel.DataAnnotations;

namespace MulitpleDb.Sample.Models
{
    public class RocketQueryModel
    {
        [Required]
        public FuelTypeEnum FuelType { get; set; }

        [Required]
        public String Planet { get; set; }
    }
}
