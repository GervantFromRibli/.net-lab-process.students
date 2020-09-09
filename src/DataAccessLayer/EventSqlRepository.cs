using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DomainEntities;

namespace DataAccessLayer
{
    public class EventSqlRepository : IRepository<Event>
    {
        public EventSqlRepository(string connection)
        {
            ConnectionString = connection;
        }

        public EventSqlRepository()
        {
            ConnectionString = @"Data Source =.\SQLEXPRESS;Initial Catalog = TicketManagement; Integrated Security = true";
        }

        public string ConnectionString { get; private set; }

        public void Create(Event item)
        {
            if (item != null)
            {
                SqlConnection connection = new SqlConnection(ConnectionString);
                connection.Open();
                SqlCommand cmd = new SqlCommand("AddEvent", connection)
                {
                    CommandType = CommandType.StoredProcedure,
                };
                cmd.Parameters.AddWithValue("@Id", item.Id);
                cmd.Parameters.AddWithValue("@Name", item.Name);
                cmd.Parameters.AddWithValue("@Descr", item.Description);
                cmd.Parameters.AddWithValue("@LayoutId", item.LayoutId);
                cmd.Parameters.AddWithValue("@StartDate", item.StartDate);
                cmd.Parameters.AddWithValue("@EndDate", item.EndDate);
                cmd.ExecuteNonQuery();
                connection.Close();
            }
            else
            {
                throw new ArgumentNullException(nameof(item));
            }
        }

        public Event FindById(int id)
        {
            Event @event = null;
            string command = $"SELECT * FROM [Event] WHERE [Id] = @Id";
            SqlCommand cmd = new SqlCommand(command);
            SqlConnection connection = new SqlConnection(ConnectionString);
            connection.Open();
            cmd.Connection = connection;
            cmd.Parameters.AddWithValue("@Id", id);
            SqlDataReader dbreader = cmd.ExecuteReader();
            if (dbreader.Read())
            {
                @event = new Event(dbreader.GetInt32(0), dbreader.GetString(1), dbreader.GetString(2), dbreader.GetInt32(3), dbreader.GetDateTime(4), dbreader.GetDateTime(5));
            }

            dbreader.Close();
            connection.Close();
            return @event;
        }

        public List<Event> GetAll()
        {
            List<Event> events = new List<Event>();
            string command = $"SELECT * FROM [Event]";
            SqlCommand cmd = new SqlCommand(command);
            SqlConnection connection = new SqlConnection(ConnectionString);
            connection.Open();
            cmd.Connection = connection;
            SqlDataReader dbreader = cmd.ExecuteReader();
            while (dbreader.Read())
            {
                Event @event = new Event(dbreader.GetInt32(0), dbreader.GetString(1), dbreader.GetString(2), dbreader.GetInt32(3), dbreader.GetDateTime(4), dbreader.GetDateTime(5));
                events.Add(@event);
            }

            dbreader.Close();
            connection.Close();
            return events;
        }

        public void Remove(Event item)
        {
            if (item != null)
            {
                SqlConnection connection = new SqlConnection(ConnectionString);
                connection.Open();
                SqlCommand cmd = new SqlCommand("DeleteEvent", connection)
                {
                    CommandType = CommandType.StoredProcedure,
                };
                cmd.Parameters.AddWithValue("@Id", item.Id);
                cmd.ExecuteNonQuery();
                connection.Close();
            }
            else
            {
                throw new ArgumentNullException(nameof(item));
            }
        }

        public void Update(Event item)
        {
            if (item != null)
            {
                SqlConnection connection = new SqlConnection(ConnectionString);
                connection.Open();
                SqlCommand cmd = new SqlCommand("UpdateEvent", connection)
                {
                    CommandType = CommandType.StoredProcedure,
                };
                cmd.Parameters.AddWithValue("@Id", item.Id);
                cmd.Parameters.AddWithValue("@Name", item.Name);
                cmd.Parameters.AddWithValue("@Descr", item.Description);
                cmd.Parameters.AddWithValue("@LayoutId", item.LayoutId);
                cmd.Parameters.AddWithValue("@StartDate", item.StartDate);
                cmd.Parameters.AddWithValue("@EndDate", item.EndDate);
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
