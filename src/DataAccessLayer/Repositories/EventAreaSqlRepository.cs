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
        private List<EventArea> _eventAreas;

        public EventAreaSqlRepository(string connection)
        {
            ConnectionString = connection;
            _eventAreas = new List<EventArea>();
            FillRepository();
        }

        public EventAreaSqlRepository()
        {
            ConnectionString = @"Data Source =.\SQLEXPRESS;Initial Catalog = TicketManagement; Integrated Security = true";
            _eventAreas = new List<EventArea>();
            FillRepository();
        }

        public string ConnectionString { get; private set; }

        private void FillRepository()
        {
            string command = $"SELECT * FROM [EventArea]";
            SqlCommand cmd = new SqlCommand(command);
            SqlConnection connection = new SqlConnection(ConnectionString);
            connection.Open();
            cmd.Connection = connection;
            SqlDataReader dbreader = cmd.ExecuteReader();
            while (dbreader.Read())
            {
                EventArea eventArea = new EventArea(dbreader.GetInt32(0), dbreader.GetInt32(1), dbreader.GetString(2), dbreader.GetInt32(3), dbreader.GetInt32(4), dbreader.GetDecimal(5));
                _eventAreas.Add(eventArea);
            }

            dbreader.Close();
            connection.Close();
        }

        public void Create(EventArea item)
        {
            if (item != null)
            {
                _eventAreas.Add(item);
                SaveChanges("Add", item);
            }
            else
            {
                throw new ArgumentNullException(nameof(item));
            }
        }

        public EventArea FindById(int id)
        {
            return _eventAreas.Find(elem => elem.Id == id);
        }

        public List<EventArea> GetAll()
        {
            return _eventAreas;
        }

        public void Remove(EventArea item)
        {
            if (item != null)
            {
                _eventAreas.Remove(FindById(item.Id));
                SaveChanges("Remove", item);
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
                for (int i = 0; i < _eventAreas.Count; i++)
                {
                    if (_eventAreas[i].Id == item.Id)
                    {
                        _eventAreas[i] = item;
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

        public void SaveChanges(string type, EventArea item)
        {
            switch (type)
            {
                case "Add":
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
                        break;
                    }

                case "Remove":
                    {
                        string command = $"DELETE FROM [EventArea] WHERE [Id] = @Id";
                        SqlCommand cmd = new SqlCommand(command);
                        SqlConnection connection = new SqlConnection(ConnectionString);
                        connection.Open();
                        cmd.Connection = connection;
                        cmd.Parameters.AddWithValue("@Id", item.Id);
                        cmd.ExecuteNonQuery();
                        connection.Close();
                        break;
                    }

                case "Update":
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
                        break;
                    }
            }
        }
    }
}
