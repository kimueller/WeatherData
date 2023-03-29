using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WeatherApp.Models
{
    public class City
    {
        [Key]
        public int CityID { get; set; }

        [Required(ErrorMessage = "Type in a city name!")]
        public string CityName { get; set; }

        [Required(ErrorMessage = "No country selected!")]
        public string CountryCode { get; set; }

        [ForeignKey("CountryCode")]
        public virtual Country Country { get; set; }

        [Required]
        [Column(TypeName = "decimal(3, 1)")]
        [Range(typeof(decimal), "-180", "180", ErrorMessage = "Latitude must be between -180 and 180 degrees.")]
        public decimal Latitude { get; set; }        
        [Required]
        [Column(TypeName = "decimal(3, 1)")]
        [Range(typeof(decimal), "-180", "180", ErrorMessage = "Longitude must be between -180 and 180 degrees.")]
        public decimal Longitude { get; set; }

    }
}
