using Newtonsoft.Json;
using Nop.Plugin.Api.DTO;
using Nop.Plugin.Api.DTO.Base;

namespace Nop.Front.Api.DTOs.Shipping
{
    [JsonObject(Title = "WarehouseForShipment")]
    public class WarehouseForShipmentDto : BaseDto
    {
        /// <summary>
        /// Gets or sets the warehouse name
        /// </summary>
        [JsonProperty("name")]
        public string WarehouseName { get; set; }

        /// <summary>
        /// Gets or sets the warehouse quantity
        /// </summary>
        [JsonProperty("quantity")]
        public int Quantity { get; set; }

        /// <summary>
        /// Gets or sets the address identifier of the warehouse
        /// </summary>
        [JsonProperty("address")]
        public AddressDto Address { get; set; }
    }
}
