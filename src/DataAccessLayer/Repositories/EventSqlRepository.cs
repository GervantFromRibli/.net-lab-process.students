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
        }

        public EventSqlRepository()
        {
            ConnectionString = @"Data Source =.\SQLEXPRESS;Initial Catalog = TicketManagement; Integrated Security = true";
            _events = new List<Event>();
        }

        public string ConnectionString { get; private set; }

        public bool IsFilledWithDbData { get; private set; } = false;

        public virtual void FillRepository()
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
            IsFilledWithDbData = true;
        }

        public virtual void Create(Event item)
        {
            if (item != null)
            {
                _events.Add(item);
                if (IsFilledWithDbData == true)
                {
                    SaveChanges("Add", item);
                }
            }
            else
            {
                throw new ArgumentNullException(nameof(item));
            }
        }

        public virtual Event FindById(int id)
        {
            return _events.Find(elem => elem.Id == id);
        }

        public virtual List<Event> GetAll()
        {
            return _events;
        }

        public virtual void Remove(int id)
        {
            _events.Remove(FindById(id));
            if (IsFilledWithDbData == true)
            {
                SaveChanges("Remove", new Event(id, "", "", 0, DateTime.Now, DateTime.Now));
            }
        }

        public virtual void Update(Event item)
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

                if (IsFilledWithDbData == true)
                {
                    SaveChanges("Update", item);
                }
            }
            else
            {
                throw new ArgumentNullException(nameof(item));
            }
        }

        public virtual void SaveChanges(string type, Event item)
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
