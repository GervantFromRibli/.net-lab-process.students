using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DomainEntities;

namespace DataAccessLayer
{
    public class VenueRepository : IRepository<Venue>
    {
        private List<Venue> _venues;

        private string _connectionString = @"Data Source =.\SQLEXPRESS;Initial Catalog = TicketManagement; Integrated Security = true";

        public VenueRepository()
        {
            _venues = new List<Venue>();
            string command = $"SELECT * FROM [Venue]";
            SqlCommand cmd = new SqlCommand(command);
            SqlConnection connection = new SqlConnection(_connectionString);
            connection.Open();
            cmd.Connection = connection;
            SqlDataReader dbreader = cmd.ExecuteReader();
            string phone = "";
            while (dbreader.Read())
            {
                if (!dbreader.IsDBNull(3))
                {
                    phone = dbreader.GetString(3);
                }
                else
                {
                    phone = "";
                }

                Venue elem = new Venue(dbreader.GetInt32(0), dbreader.GetString(1), dbreader.GetString(2), phone);
                _venues.Add(elem);
            }

            dbreader.Close();
            connection.Close();
        }

        public void Create(Venue item)
        {
            _venues.Add(item);
            SaveChanges();
        }

        public Venue FindById(int id)
        {
            return _venues.Select(elem => elem).Where(elem => elem.Id == id).Single();
        }

        public List<Venue> GetAll()
        {
            return _venues;
        }

        public void Remove(Venue item)
        {
            _venues.Remove(item);
            SaveChanges();
        }

        public void Update(Venue item)
        {
            foreach (var elem in _venues)
            {
                if (elem.Id == item.Id)
                {
                    elem.Description = item.Description;
                    elem.Address = item.Address;
                    elem.Phone = item.Phone;
                    SaveChanges();
                    break;
                }
            }
        }

        public void Dispose()
        {
            _venues.Clear();
        }

        public void SaveChanges()
        {
            string command = $"DELETE FROM [Venue]";
            SqlCommand cmd = new SqlCommand(command);
            SqlConnection connection = new SqlConnection(_connectionString);
            connection.Open();
            cmd.Connection = connection;
            cmd.ExecuteNonQuery();
            command = $"INSERT INTO [Venue] (Id, Description, Address, Phone) VALUES (@Id, @Descr, @Address, @Phone)";
            foreach (var elem in _venues)
            {
                cmd = new SqlCommand(command);
                cmd.Connection = connection;
                cmd.Parameters.AddWithValue("@Id", elem.Id);
                cmd.Parameters.AddWithValue("@Descr", elem.Description);
                cmd.Parameters.AddWithValue("@Address", elem.Address);
                cmd.Parameters.AddWithValue("@Phone", elem.Phone);
                cmd.ExecuteNonQuery();
            }

            connection.Close();
        }
    }
}
