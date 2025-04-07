using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public class WeatherRecord : IEntity
    {
        public int Id { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public decimal Temperature { get; set; }
        public decimal MinTemperature { get; set; }
        public decimal MaxTemperature { get; set; }
        public DateTime LastUpdate { get; set; }
    }
}
