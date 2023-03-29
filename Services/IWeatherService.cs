using WeatherApp.Models;
using WeatherApp.Models.DTOs;

namespace WeatherApp.Services
{
    public interface IWeatherService
    {
        List<City> GetCities();
        List<City> GetCities(string countryName);

        Task<WeatherData> GetWeatherForecastAsync(string city);

        List<Country> GetCountries();
        List<Country> GetRegisterdCountries();

        void AddCity(City city);

    }
}
