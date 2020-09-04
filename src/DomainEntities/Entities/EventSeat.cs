using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainEntities
{
    public class EventSeat
    {
        public int Id { get; set; }
        public int EventAreaId { get; set; }
        public int Row { get; set; }
        public int Number { get; set; }
        public int State { get; set; }
        public EventSeat(int id, int eventAreaId, int row, int number, int state)
        {
            Id = id;
            EventAreaId = eventAreaId;
            Row = row;
            Number = number;
            State = state;
        }
    }
}
