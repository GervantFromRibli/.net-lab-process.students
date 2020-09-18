namespace DomainEntities
{
    // Class that represent event seat table in database
    public class EventSeat
    {
        public EventSeat(int id, int eventAreaId, int row, int number, int state)
        {
            Id = id;
            EventAreaId = eventAreaId;
            Row = row;
            Number = number;
            State = state;
        }

        public int Id { get; set; }

        public int EventAreaId { get; set; }

        public int Row { get; set; }

        public int Number { get; set; }

        public int State { get; set; }
    }
}
