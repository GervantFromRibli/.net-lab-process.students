using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainEntities
{
    public class Venue
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public string Address { get; set; }
        public int? Phone { get; set; }
        public Venue(int id, string description, string address)
        {
            Id = id;
            Description = description;
            Address = address;
            Phone = null;
        }
        public Venue(int id, string description, string address, int phone)
        {
            Id = id;
            Description = description;
            Address = address;
            Phone = phone;
        }
    }
}
