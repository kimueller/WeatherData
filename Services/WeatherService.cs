using Microsoft.EntityFrameworkCore;
using WeatherApp.Data;
using WeatherApp.Models;
using WeatherApp.Models.DTOs;
using WeatherApp.Pages.Weather;

namespace WeatherApp.Services
{
    public class WeatherService : IWeatherService
    {
        private readonly ApplicationDbContext dbc;
        private readonly HttpClient httpClient;

        //constructor 
        public WeatherService(ApplicationDbContext dbc, HttpClient httpClient)
        {
            this.dbc = dbc;//creates an instance of the dbContext
            this.httpClient = httpClient;//creates an instance of httpClient
        }

        /// <summary>
        /// Gets the latitude of city
        /// </summary>
        /// <param name="city">city to find latidue</param>
        /// <returns>latitude (deicmal)</returns>
        private decimal GetLatidute(string city)
        {
            var latitude = dbc.Cities
                               .Where(c => c.CityName == city)
                               .Select(c => c.Latitude)
                               .FirstOrDefault();
            return latitude;
        }

        /// <summary>
        /// Gets the longitude of city
        /// </summary>
        /// <param name="city">city to find longitude</param>
        /// <returns>longitude (deicmal)</returns>
        private decimal GetLongitude(string city)
        {
            var longitude = dbc.Cities
                               .Where(c => c.CityName == city)
                               .Select(c => c.Longitude)
                               .FirstOrDefault();
            return longitude;
        }

        /// <summary>
        /// Gets the countryname of the countrycode
        /// </summary>
        /// <param name="city">city of the country</param>
        /// <returns>name of ocuntry</returns>
        private string GetCountryName(string city)
        {
            var country = dbc.Cities
                            .Include(c => c.Country)
                            .Where(c => c.CityName == city)
                            .Select(c => c.Country.CountryName)
                            .FirstOrDefault();
            return country;
        }

        /// <summary>
        /// makes an api-call to openweathermap to get the specific weather data of the city
        /// </summary>
        /// <param name="city">city to get weatherforecast</param>
        /// <returns>object of weatherdata</returns>
        public async Task<WeatherData> GetWeatherForecastAsync(string city)
        {
            //sends an http get request to openweathermap
            var response = await httpClient.GetAsync($"https://api.openweathermap.org/data/2.5/weather?lat={GetLatidute(city)}&lon={GetLongitude(city)}&APPID=72bf310c823f2e48cb85ef2294b583cc&lang=de&units=metric");
            response.EnsureSuccessStatusCode();//checks if the result is correct, if not it throws an exception
            var weatherData = await response.Content.ReadFromJsonAsync<WeatherData>();//reads from the generated json string and deserializes it to WeatherData

            weatherData.Sys.Country = GetCountryName(city);//gets the country of the city from db

            // Create a new WeatherData object with the desired properties
            return new WeatherData
            {
                Name = city,
                Weather = weatherData.Weather,
                Main = weatherData.Main,
                Sys = weatherData.Sys
            };
        }

        /// <summary>
        /// Gets all cities in db
        /// </summary>
        /// <returns>List of cities</returns>
        public List<City> GetCities()
        {
            var cities = dbc.Cities
                           .OrderBy(c => c.CountryCode)
                           .Select(c => new City
                           {
                               CityName = c.CityName,
                               CountryCode = c.CountryCode,
                               Latitude = c.Latitude,
                               Longitude = c.Longitude,
                               Country = c.Country,
                           });
            return cities.ToList();
        }

        /// <summary>
        /// Gets cities from specific country
        /// </summary>
        /// <param name="countryName">CountryCode of country to filter</param>
        /// <returns>List of cities that are in the country</returns>
        public List<City> GetCities(string countryName)
        {
            var cities = dbc.Cities
                        //when the countryName parameter is empty it searches for all
                        .Where(c => countryName == string.Empty || c.CountryCode == countryName)
                        .ToList();
            return cities;
        }

        /// <summary>
        /// Gets all countries in db
        /// </summary>
        /// <returns>List of countries</returns>
        public List<Country> GetCountries()
        {
            var countries = dbc.Countries
                            .OrderBy(c => c.CountryCode)
                            .Select(c => new Country
                            {
                                CountryName = c.CountryName,
                                CountryCode = c.CountryCode,
                            });
            return countries.ToList();
        }

        /// <summary>
        /// Gets a list of countries that have a city tracked
        /// </summary>
        /// <returns>List of countries</returns>
        public List<Country> GetRegisterdCountries()
        {
            //get all countrycodes
            var cities = dbc.Cities
                        .Select(c => c.CountryCode).ToList();

            //queries all countrycodes that have been used in the cities table
            var countries = dbc.Countries
                .OrderBy(c => c.CountryCode)
                .Where(c => cities.Contains(c.CountryCode));
            return countries.ToList();
        }

        /// <summary>
        /// Adds a new city to db
        /// </summary>
        /// <param name="city">city to add</param>
        public void AddCity(City city)
        {
            dbc.Add(city);//adds the city to the db
            dbc.SaveChanges();
        }
    }
}
