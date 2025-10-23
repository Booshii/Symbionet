using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace api.Helpers
{
    /**
     * @class GeocodingHelper
     * @brief Provides utility methods for geocoding operations.
     */
    public static class GeocodingHelper
    {
        /**
         * @brief Fetches geographic coordinates for a given address using the Nominatim API.
         * 
         * @param address The full address to fetch coordinates for.
         * @return A tuple containing latitude and longitude, or null if coordinates cannot be determined.
         */
        public static async Task<(string Latitude, string Longitude)?> GetCoordinatesFromAddress(string address)
        {
            var encodedAddress = Uri.EscapeDataString(address);
            var apiUrl = $"https://nominatim.openstreetmap.org/search?format=json&addressdetails=1&limit=1&q={encodedAddress}";

            using (var httpClient = new HttpClient())
            {
                try
                {
                    // Set Accept-Language and User-Agent Headers
                    httpClient.DefaultRequestHeaders.Add("Accept-Language", "de");
                    httpClient.DefaultRequestHeaders.Add("User-Agent", "YourAppName/1.0 (leon.boddin+gitlab@student.htw-berlin.de)");

                    var response = await httpClient.GetAsync(apiUrl);
                    if (!response.IsSuccessStatusCode)
                    {
                        Console.Error.WriteLine($"Nominatim API Error: {response.StatusCode}");
                        return null;
                    }

                    var json = await response.Content.ReadAsStringAsync();
                    var data = JsonConvert.DeserializeObject<List<NominatimResponse>>(json);

                    if (data == null || !data.Any())
                    {
                        return null;
                    }

                    var firstResult = data.First();
                    return (firstResult.Lat, firstResult.Lon);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error fetching coordinates: {ex.Message}");
                    return null;
                }
            }
        }
    }

}