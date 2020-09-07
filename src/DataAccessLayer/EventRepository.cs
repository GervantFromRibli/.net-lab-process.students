using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DomainEntities;

namespace DataAccessLayer
{
    public class EventRepository : IRepository<Event>
    {
        private List<Event> _events;

        private string _connectionString = @"Data Source =.\SQLEXPRESS;Initial Catalog = TicketManagement; Integrated Security = true";

        public EventRepository()
        {
            _events = new List<Event>();
            string command = $"SELECT * FROM [Event]";
            SqlCommand cmd = new SqlCommand(command);
            SqlConnection connection = new SqlConnection(_connectionString);
            connection.Open();
            cmd.Connection = connection;
            SqlDataReader dbreader = cmd.ExecuteReader();
            while (dbreader.Read())
            {
                Event elem = new Event(dbreader.GetInt32(0), dbreader.GetString(1), dbreader.GetString(2), dbreader.GetInt32(3));
                _events.Add(elem);
            }

            dbreader.Close();
            connection.Close();
        }

        public void Create(Event item)
        {
            _events.Add(item);
            SaveChanges();
        }

        public Event FindById(int id)
        {
            return _events.Select(elem => elem).Where(elem => elem.Id == id).Single();
        }

        public List<Event> GetAll()
        {
            return _events;
        }

        public void Remove(Event item)
        {
            _events.Remove(item);
            SaveChanges();
        }

        public void Update(Event item)
        {
            foreach (var elem in _events)
            {
                if (elem.Id == item.Id)
                {
                    elem.LayoutId = item.LayoutId;
                    elem.Description = item.Description;
                    elem.Name = item.Name;
                    SaveChanges();
                    break;
                }
            }
        }

        public void Dispose()
        {
            _events.Clear();
        }

        public void SaveChanges()
        {
            string command = $"DELETE FROM [Event]";
            SqlCommand cmd = new SqlCommand(command);
            SqlConnection connection = new SqlConnection(_connectionString);
            connection.Open();
            cmd.Connection = connection;
            cmd.ExecuteNonQuery();
            command = $"INSERT INTO [Event] (Id, Name, Description, LayoutId) VALUES (@Id, @Name, @Descr, @Layout)";
            foreach (var elem in _events)
            {
                cmd = new SqlCommand(command);
                cmd.Connection = connection;
                cmd.Parameters.AddWithValue("@Id", elem.Id);
                cmd.Parameters.AddWithValue("@Layout", elem.LayoutId);
                cmd.Parameters.AddWithValue("@Descr", elem.Description);
                cmd.Parameters.AddWithValue("@Name", elem.Name);
                cmd.ExecuteNonQuery();
            }

            connection.Close();
        }
    }
}
