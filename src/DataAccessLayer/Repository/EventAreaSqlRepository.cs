using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DomainEntities;

namespace DataAccessLayer
{
    public class EventAreaSqlRepository : IRepository<EventArea>
    {
        public EventAreaSqlRepository(string connection)
        {
            ConnectionString = connection;
        }

        public EventAreaSqlRepository()
        {
            ConnectionString = @"Data Source =.\SQLEXPRESS;Initial Catalog = TicketManagement; Integrated Security = true";
        }

        public string ConnectionString { get; private set; }

        public void Create(EventArea item)
        {
            if (item != null)
            {
                string command = $"INSERT INTO [EventArea] (Id, EventId, Description, CoordX, CoordY, Price) VALUES (@Id, @Event, @Descr, @X, @Y, @Price)";
                SqlCommand cmd = new SqlCommand(command);
                SqlConnection connection = new SqlConnection(ConnectionString);
                connection.Open();
                cmd.Connection = connection;
                cmd.Parameters.AddWithValue("@Id", item.Id);
                cmd.Parameters.AddWithValue("@Event", item.EventId);
                cmd.Parameters.AddWithValue("@Descr", item.Description);
                cmd.Parameters.AddWithValue("@X", item.CoordX);
                cmd.Parameters.AddWithValue("@Y", item.CoordY);
                cmd.Parameters.AddWithValue("@Price", item.Price);
                cmd.ExecuteNonQuery();
                connection.Close();
            }
            else
            {
                throw new ArgumentNullException(nameof(item));
            }
        }

        public EventArea FindById(int id)
        {
            EventArea eventArea = null;
            string command = $"SELECT * FROM [EventArea] WHERE [Id] = @Id";
            SqlCommand cmd = new SqlCommand(command);
            SqlConnection connection = new SqlConnection(ConnectionString);
            connection.Open();
            cmd.Connection = connection;
            cmd.Parameters.AddWithValue("@Id", id);
            SqlDataReader dbreader = cmd.ExecuteReader();
            if (dbreader.Read())
            {
                eventArea = new EventArea(dbreader.GetInt32(0), dbreader.GetInt32(1), dbreader.GetString(2), dbreader.GetInt32(3), dbreader.GetInt32(4), dbreader.GetDecimal(5));
            }

            dbreader.Close();
            connection.Close();
            return eventArea;
        }

        public List<EventArea> GetAll()
        {
            List<EventArea> eventAreas = new List<EventArea>();
            string command = $"SELECT * FROM [EventArea]";
            SqlCommand cmd = new SqlCommand(command);
            SqlConnection connection = new SqlConnection(ConnectionString);
            connection.Open();
            cmd.Connection = connection;
            SqlDataReader dbreader = cmd.ExecuteReader();
            while (dbreader.Read())
            {
                EventArea eventArea = new EventArea(dbreader.GetInt32(0), dbreader.GetInt32(1), dbreader.GetString(2), dbreader.GetInt32(3), dbreader.GetInt32(4), dbreader.GetDecimal(5));
                eventAreas.Add(eventArea);
            }

            dbreader.Close();
            connection.Close();
            return eventAreas;
        }

        public void Remove(EventArea item)
        {
            if (item != null)
            {
                string command = $"DELETE FROM [EventArea] WHERE [Id] = @Id";
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

        public void Update(EventArea item)
        {
            if (item != null)
            {
                string command = $"UPDATE [EventArea] SET EventId = @Event, Description = @Descr, CoordX = @X, CoordY = @Y, Price = @Price WHERE Id = @Id";
                SqlCommand cmd = new SqlCommand(command);
                SqlConnection connection = new SqlConnection(ConnectionString);
                connection.Open();
                cmd.Connection = connection;
                cmd.Parameters.AddWithValue("@Id", item.Id);
                cmd.Parameters.AddWithValue("@Event", item.EventId);
                cmd.Parameters.AddWithValue("@Descr", item.Description);
                cmd.Parameters.AddWithValue("@X", item.CoordX);
                cmd.Parameters.AddWithValue("@Y", item.CoordY);
                cmd.Parameters.AddWithValue("@Price", item.Price);
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
