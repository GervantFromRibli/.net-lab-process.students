using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer.Repositories;
using DomainEntities;

namespace DataAccessLayer
{
    // Repository for event type of data
    public class EventSqlRepository : IRepository<Event>, ISqlRepository<Event>
    {
        // Repository filled with event data
        private List<Event> _events;

        // Constructor that can get connection string
        public EventSqlRepository(string connection)
        {
            ConnectionString = connection;
            _events = new List<Event>();
        }

        // Creating repository without magor changes
        public EventSqlRepository()
        {
            ConnectionString = @"Data Source = .\;Initial Catalog = TicketManagement; Integrated Security = true";
            _events = new List<Event>();
        }

        public string ConnectionString { get; private set; }

        // Flag that used to check if data were taken from database
        public bool IsFilledWithDbData { get; private set; } = false;

        // Method that fills local repository with data from database
        public virtual void FillRepositoryWithSqlData()
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

        // Method that add new event node to database
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

        // Method that search for event object with certain id
        public virtual Event FindById(int id)
        {
            return _events.Find(elem => elem.Id == id);
        }

        // Method that returns repository to user
        public virtual List<Event> GetAll()
        {
            return _events;
        }

        // Method that removes event object from repository with certain id
        public virtual void Remove(int id)
        {
            _events.Remove(FindById(id));
            if (IsFilledWithDbData == true)
            {
                SaveChanges("Remove", new Event(id, "", "", 0, DateTime.Now, DateTime.Now));
            }
        }

        // Method that update object in repository
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

        // Method that brings changes from repository to database
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
