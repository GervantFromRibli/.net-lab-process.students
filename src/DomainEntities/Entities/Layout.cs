using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainEntities
{
    public class Layout
    {
        public Layout(int id, string name, int venueId, string description)
        {
            Id = id;
            Name = name;
            VenueId = venueId;
            Description = description;
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public int VenueId { get; set; }

        public string Description { get; set; }
    }
}
