using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DomainEntities;

namespace DataAccessLayer
{
    public class LayoutRepository : IRepository<Layout>
    {
        private List<Layout> _layouts;

        private string _connectionString = @"Data Source =.\SQLEXPRESS;Initial Catalog = TicketManagement; Integrated Security = true";

        public LayoutRepository()
        {
            _layouts = new List<Layout>();
            string command = $"SELECT * FROM [Layout]";
            SqlCommand cmd = new SqlCommand(command);
            SqlConnection connection = new SqlConnection(_connectionString);
            connection.Open();
            cmd.Connection = connection;
            SqlDataReader dbreader = cmd.ExecuteReader();
            while (dbreader.Read())
            {
                Layout elem = new Layout(dbreader.GetInt32(0), dbreader.GetInt32(1), dbreader.GetString(2));
                _layouts.Add(elem);
            }

            dbreader.Close();
            connection.Close();
        }

        public void Create(Layout item)
        {
            _layouts.Add(item);
            SaveChanges();
        }

        public Layout FindById(int id)
        {
            return _layouts.Select(elem => elem).Where(elem => elem.Id == id).Single();
        }

        public List<Layout> GetAll()
        {
            return _layouts;
        }

        public void Remove(Layout item)
        {
            _layouts.Remove(item);
            SaveChanges();
        }

        public void Update(Layout item)
        {
            foreach (var elem in _layouts)
            {
                if (elem.Id == item.Id)
                {
                    elem.VenueId = item.VenueId;
                    elem.Description = item.Description;
                    SaveChanges();
                    break;
                }
            }
        }

        public void Dispose()
        {
            _layouts.Clear();
        }

        public void SaveChanges()
        {
            string command = $"DELETE FROM [Layout]";
            SqlCommand cmd = new SqlCommand(command);
            SqlConnection connection = new SqlConnection(_connectionString);
            connection.Open();
            cmd.Connection = connection;
            cmd.ExecuteNonQuery();
            command = $"INSERT INTO [Layout] (Id, VenueId, Description) VALUES (@Id, @Venue, @Descr)";
            foreach (var elem in _layouts)
            {
                cmd = new SqlCommand(command);
                cmd.Connection = connection;
                cmd.Parameters.AddWithValue("@Id", elem.Id);
                cmd.Parameters.AddWithValue("@Venue", elem.VenueId);
                cmd.Parameters.AddWithValue("@Descr", elem.Description);
                cmd.ExecuteNonQuery();
            }

            connection.Close();
        }
    }
}
