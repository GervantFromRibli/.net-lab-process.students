using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace DomainEntities
{
    public class EventArea
    {
        public EventArea(int id, int eventId, string description, int coordX, int coordY, decimal price)
        {
            Id = id;
            EventId = eventId;
            Description = description;
            CoordX = coordX;
            CoordY = coordY;
            Price = price;
        }

        public int Id { get; set; }

        public int EventId { get; set; }

        public string Description { get; set; }

        public int CoordX { get; set; }

        public int CoordY { get; set; }

        public decimal Price { get; set; }
    }
}
