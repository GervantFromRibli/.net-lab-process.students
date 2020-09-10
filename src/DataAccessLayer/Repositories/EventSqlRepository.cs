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
        private List<Event> _events;

        public EventSqlRepository(string connection)
        {
            ConnectionString = connection;
            _events = new List<Event>();
            FillRepository();
        }

        public EventSqlRepository()
        {
            ConnectionString = @"Data Source =.\SQLEXPRESS;Initial Catalog = TicketManagement; Integrated Security = true";
            _events = new List<Event>();
            FillRepository();
        }

        public string ConnectionString { get; private set; }

        private void FillRepository()
        {
            string command = $"SELECT * FROM [Event]";
            SqlCommand cmd = new SqlCommand(command);
            SqlConnection connection = new SqlConnection(ConnectionString);
            connection.Open();
            cmd.Connection = connection;
            SqlDataReader dbreader = cmd.ExecuteReader();
            while (dbreader.Read())
            {
                Event @event = new Event(dbreader.GetInt32(0), dbreader.GetString(1), dbreader.GetString(2), dbreader.GetInt32(3), dbreader.GetDateTime(4), dbreader.GetDateTime(5));
                _events.Add(@event);
            }

            dbreader.Close();
            connection.Close();
        }

        public void Create(Event item)
        {
            if (item != null)
            {
                _events.Add(item);
                SaveChanges("Add", item);
            }
            else
            {
                throw new ArgumentNullException(nameof(item));
            }
        }

        public Event FindById(int id)
        {
            return _events.Find(elem => elem.Id == id);
        }

        public List<Event> GetAll()
        {
            return _events;
        }

        public void Remove(Event item)
        {
            if (item != null)
            {
                _events.Remove(FindById(item.Id));
                SaveChanges("Remove", item);
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
                for (int i = 0; i < _events.Count; i++)
                {
                    if (_events[i].Id == item.Id)
                    {
                        _events[i] = item;
                        break;
                    }
                }

                SaveChanges("Update", item);
            }
            else
            {
                throw new ArgumentNullException(nameof(item));
            }
        }

        public void SaveChanges(string type, Event item)
        {
            switch (type)
            {
                case "Add":
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
                        break;
                    }

                case "Remove":
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
                        break;
                    }

                case "Update":
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
                        break;
                    }
            }
        }
    }
}
