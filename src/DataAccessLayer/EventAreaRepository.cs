using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DomainEntities;

namespace DataAccessLayer
{
    public class EventAreaRepository : IRepository<EventArea>
    {
        private List<EventArea> _eventAreas;

        private string _connectionString = @"Data Source =.\SQLEXPRESS;Initial Catalog = TicketManagement; Integrated Security = true";

        public EventAreaRepository()
        {
            _eventAreas = new List<EventArea>();
            string command = $"SELECT * FROM [EventArea]";
            SqlCommand cmd = new SqlCommand(command);
            SqlConnection connection = new SqlConnection(_connectionString);
            connection.Open();
            cmd.Connection = connection;
            SqlDataReader dbreader = cmd.ExecuteReader();
            while (dbreader.Read())
            {
                EventArea elem = new EventArea(dbreader.GetInt32(0), dbreader.GetInt32(1), dbreader.GetString(2), dbreader.GetInt32(3), dbreader.GetInt32(4), dbreader.GetDecimal(5));
                _eventAreas.Add(elem);
            }

            dbreader.Close();
            connection.Close();
        }

        public void Create(EventArea item)
        {
            _eventAreas.Add(item);
            SaveChanges();
        }

        public EventArea FindById(int id)
        {
            return _eventAreas.Select(elem => elem).Where(elem => elem.Id == id).Single();
        }

        public List<EventArea> GetAll()
        {
            return _eventAreas;
        }

        public void Remove(EventArea item)
        {
            _eventAreas.Remove(item);
            SaveChanges();
        }

        public void Update(EventArea item)
        {
            foreach (var elem in _eventAreas)
            {
                if (elem.Id == item.Id)
                {
                    elem.EventId = item.EventId;
                    elem.Description = item.Description;
                    elem.CoordX = item.CoordX;
                    elem.CoordY = item.CoordY;
                    elem.Price = item.Price;
                    SaveChanges();
                    break;
                }
            }
        }

        public void Dispose()
        {
            _eventAreas.Clear();
        }

        public void SaveChanges()
        {
            string command = $"DELETE FROM [EventArea]";
            SqlCommand cmd = new SqlCommand(command);
            SqlConnection connection = new SqlConnection(_connectionString);
            connection.Open();
            cmd.Connection = connection;
            cmd.ExecuteNonQuery();
            command = $"INSERT INTO [EventArea] (Id, EventId, Description, CoordX, CoordY, Price) VALUES (@Id, @Event, @Descr, @X, @Y, @Price)";
            foreach (var elem in _eventAreas)
            {
                cmd = new SqlCommand(command);
                cmd.Connection = connection;
                cmd.Parameters.AddWithValue("@Id", elem.Id);
                cmd.Parameters.AddWithValue("@Event", elem.EventId);
                cmd.Parameters.AddWithValue("@Descr", elem.Description);
                cmd.Parameters.AddWithValue("@X", elem.CoordX);
                cmd.Parameters.AddWithValue("@Y", elem.CoordY);
                cmd.Parameters.AddWithValue("@Price", elem.Price);
                cmd.ExecuteNonQuery();
            }

            connection.Close();
        }
    }
}
