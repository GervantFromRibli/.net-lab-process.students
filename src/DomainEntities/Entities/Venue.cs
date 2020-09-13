using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainEntities
{
    public class Venue
    {
        public Venue(int id, string name, string description, string address)
        {
            Id = id;
            Name = name;
            Description = description;
            Address = address;
            Phone = null;
        }

        public Venue(int id, string name, string description, string address, string phone)
        {
            Id = id;
            Name = name;
            Description = description;
            Address = address;
            Phone = phone;
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Address { get; set; }

        public string Phone { get; set; }
    }
}
