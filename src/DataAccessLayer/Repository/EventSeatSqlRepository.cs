using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DomainEntities;

namespace DataAccessLayer
{
    public class EventSeatSqlRepository : IRepository<EventSeat>
    {
        public EventSeatSqlRepository(string connection)
        {
            ConnectionString = connection;
        }

        public EventSeatSqlRepository()
        {
            ConnectionString = @"Data Source =.\SQLEXPRESS;Initial Catalog = TicketManagement; Integrated Security = true";
        }

        public string ConnectionString { get; private set; }

        public void Create(EventSeat item)
        {
            if (item != null)
            {
                string command = $"INSERT INTO [EventSeat] (Id, EventAreaId, Row, Number, State) VALUES (@Id, @EventArea, @Row, @Numb, @State)";
                SqlCommand cmd = new SqlCommand(command);
                SqlConnection connection = new SqlConnection(ConnectionString);
                connection.Open();
                cmd.Connection = connection;
                cmd.Parameters.AddWithValue("@Id", item.Id);
                cmd.Parameters.AddWithValue("@EventArea", item.EventAreaId);
                cmd.Parameters.AddWithValue("@Row", item.Row);
                cmd.Parameters.AddWithValue("@Numb", item.Number);
                cmd.Parameters.AddWithValue("@State", item.State);
                cmd.ExecuteNonQuery();
                connection.Close();
            }
            else
            {
                throw new ArgumentNullException(nameof(item));
            }
        }

        public EventSeat FindById(int id)
        {
            EventSeat eventSeat = null;
            string command = $"SELECT * FROM [EventSeat] WHERE [Id] = @Id";
            SqlCommand cmd = new SqlCommand(command);
            SqlConnection connection = new SqlConnection(ConnectionString);
            connection.Open();
            cmd.Connection = connection;
            cmd.Parameters.AddWithValue("@Id", id);
            SqlDataReader dbreader = cmd.ExecuteReader();
            if (dbreader.Read())
            {
                eventSeat = new EventSeat(dbreader.GetInt32(0), dbreader.GetInt32(1), dbreader.GetInt32(2), dbreader.GetInt32(3), dbreader.GetInt32(4));
            }

            dbreader.Close();
            connection.Close();
            return eventSeat;
        }

        public List<EventSeat> GetAll()
        {
            List<EventSeat> eventSeats = new List<EventSeat>();
            string command = $"SELECT * FROM [EventSeat]";
            SqlCommand cmd = new SqlCommand(command);
            SqlConnection connection = new SqlConnection(ConnectionString);
            connection.Open();
            cmd.Connection = connection;
            SqlDataReader dbreader = cmd.ExecuteReader();
            while (dbreader.Read())
            {
                EventSeat eventSeat = new EventSeat(dbreader.GetInt32(0), dbreader.GetInt32(1), dbreader.GetInt32(2), dbreader.GetInt32(3), dbreader.GetInt32(4));
                eventSeats.Add(eventSeat);
            }

            dbreader.Close();
            connection.Close();
            return eventSeats;
        }

        public void Remove(EventSeat item)
        {
            if (item != null)
            {
                string command = $"DELETE FROM [EventSeat] WHERE [Id] = @Id";
                SqlCommand cmd = new SqlCommand(command);
                SqlConnection connection = new SqlConnection(ConnectionString);
                connection.Open();
                cmd.Connection = connection;
                cmd.Parameters.AddWithValue("@Id", item.Id);
                cmd.ExecuteNonQuery();
                connection.Close();
            }
            else
            {
                throw new ArgumentNullException(nameof(item));
            }
        }

        public void Update(EventSeat item)
        {
            if (item != null)
            {
                string command = $"UPDATE [EventSeat] SET EventAreaId = @EventArea, Row = @Row, Number = @Numb, State = @State WHERE Id = @Id";
                SqlCommand cmd = new SqlCommand(command);
                SqlConnection connection = new SqlConnection(ConnectionString);
                connection.Open();
                cmd.Connection = connection;
                cmd.Parameters.AddWithValue("@Id", item.Id);
                cmd.Parameters.AddWithValue("@EventArea", item.EventAreaId);
                cmd.Parameters.AddWithValue("@Row", item.Row);
                cmd.Parameters.AddWithValue("@Numb", item.Number);
                cmd.Parameters.AddWithValue("@State", item.State);
                cmd.ExecuteNonQuery();
                connection.Close();
            }
            else
            {
                throw new ArgumentNullException(nameof(item));
            }
        }
    }
}
