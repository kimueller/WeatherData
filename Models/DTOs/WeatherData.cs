using System.Text.Json.Serialization;

namespace WeatherApp.Models.DTOs
{
    public class WeatherData
    {
        public List<Weather> Weather { get; set; }

        public Main Main { get; set; }
        public string Name { get; set; }
        public Sys Sys { get; set; }
    }


    public class Main
    {
        [JsonPropertyName("temp")]
        public decimal Temperature { get; set; }

        [JsonPropertyName("temp_min")]
        public decimal MinTemperature { get; set; }

        [JsonPropertyName("temp_max")]
        public decimal MaxTemperature { get; set; }
    }

    public class Weather
    {
        [JsonPropertyName("main")]
        public string WeatherDescription{ get; set; }

    }


    public class Sys
    {
        public string Country { get; set; }

    }

}


