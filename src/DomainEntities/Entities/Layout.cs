using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainEntities
{
    public class Layout
    {
        public int Id { get; set; }
        public int VenueId { get; set; }
        public string Description { get; set; }
        public Layout(int id, int venueId, string description)
        {
            Id = id;
            VenueId = venueId;
            Description = description;
        }
    }
}
