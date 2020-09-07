using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DomainEntities;

namespace DataAccessLayer
{
    public class EventSeatRepository : IRepository<EventSeat>
    {
        private List<EventSeat> _eventSeats;

        private string _connectionString = @"Data Source =.\SQLEXPRESS;Initial Catalog = TicketManagement; Integrated Security = true";

        public EventSeatRepository()
        {
            _eventSeats = new List<EventSeat>();
            string command = $"SELECT * FROM [EventSeat]";
            SqlCommand cmd = new SqlCommand(command);
            SqlConnection connection = new SqlConnection(_connectionString);
            connection.Open();
            cmd.Connection = connection;
            SqlDataReader dbreader = cmd.ExecuteReader();
            while (dbreader.Read())
            {
                EventSeat elem = new EventSeat(dbreader.GetInt32(0), dbreader.GetInt32(1), dbreader.GetInt32(2), dbreader.GetInt32(3), dbreader.GetInt32(4));
                _eventSeats.Add(elem);
            }

            dbreader.Close();
            connection.Close();
        }

        public void Create(EventSeat item)
        {
            _eventSeats.Add(item);
            SaveChanges();
        }

        public EventSeat FindById(int id)
        {
            return _eventSeats.Select(elem => elem).Where(elem => elem.Id == id).Single();
        }

        public List<EventSeat> GetAll()
        {
            return _eventSeats;
        }

        public void Remove(EventSeat item)
        {
            _eventSeats.Remove(item);
            SaveChanges();
        }

        public void Update(EventSeat item)
        {
            foreach (var elem in _eventSeats)
            {
                if (elem.Id == item.Id)
                {
                    elem.EventAreaId = item.EventAreaId;
                    elem.Row = item.Row;
                    elem.Number = item.Number;
                    elem.State = item.State;
                    SaveChanges();
                    break;
                }
            }
        }

        public void Dispose()
        {
            _eventSeats.Clear();
        }

        public void SaveChanges()
        {
            string command = $"DELETE FROM [EventSeat]";
            SqlCommand cmd = new SqlCommand(command);
            SqlConnection connection = new SqlConnection(_connectionString);
            connection.Open();
            cmd.Connection = connection;
            cmd.ExecuteNonQuery();
            command = $"INSERT INTO [EventSeat] (Id, EventAreaId, Row, Number, State) VALUES (@Id, @Event, @Row, @Numb, @State)";
            foreach (var elem in _eventSeats)
            {
                cmd = new SqlCommand(command);
                cmd.Connection = connection;
                cmd.Parameters.AddWithValue("@Id", elem.Id);
                cmd.Parameters.AddWithValue("@Event", elem.EventAreaId);
                cmd.Parameters.AddWithValue("@Row", elem.Row);
                cmd.Parameters.AddWithValue("@Numb", elem.Number);
                cmd.Parameters.AddWithValue("@State", elem.State);
                cmd.ExecuteNonQuery();
            }

            connection.Close();
        }
    }
}
