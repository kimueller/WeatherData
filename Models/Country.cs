using System.ComponentModel.DataAnnotations;

namespace WeatherApp.Models
{
    public class Country
    {
        [Key]
        [MinLength(2), MaxLength(2)]
        public string CountryCode { get; set; }

        [Required]
        public string CountryName { get; set; }

        public virtual ICollection<City> City { get; set; }
    }
}
