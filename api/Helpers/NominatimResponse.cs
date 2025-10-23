using Newtonsoft.Json;

namespace api.Helpers
{
  /**
    * @class NominatimResponse
    * @brief Represents the response structure from the Nominatim API.
    */
  public class NominatimResponse
    {
        [JsonProperty("lat")]
        public required string Lat { get; set; }
        [JsonProperty("lon")]
        public required string Lon { get; set; }
        [JsonProperty("display_name")]
        public string Display_name { get; set; }
    }
}