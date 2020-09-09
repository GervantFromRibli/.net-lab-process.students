using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DomainEntities;

namespace DataAccessLayer
{
    public class VenueSqlRepository : IRepository<Venue>
    {
        public VenueSqlRepository(string connection)
        {
            ConnectionString = connection;
        }

        public VenueSqlRepository()
        {
            ConnectionString = @"Data Source =.\SQLEXPRESS;Initial Catalog = TicketManagement; Integrated Security = true";
        }

        public string ConnectionString { get; private set; }

        public void Create(Venue item)
        {
            if (item != null)
            {
                string command = $"INSERT INTO [Venue] (Id, Name, Description, Address, Phone) VALUES (@Id, @Name, @Descr, @Address, @Phone)";
                SqlCommand cmd = new SqlCommand(command);
                SqlConnection connection = new SqlConnection(ConnectionString);
                connection.Open();
                cmd.Connection = connection;
                cmd.Parameters.AddWithValue("@Id", item.Id);
                cmd.Parameters.AddWithValue("@Name", item.Name);
                cmd.Parameters.AddWithValue("@Descr", item.Description);
                cmd.Parameters.AddWithValue("@Address", item.Address);
                cmd.Parameters.AddWithValue("@Phone", item.Phone);
                cmd.ExecuteNonQuery();
                connection.Close();
            }
            else
            {
                throw new ArgumentNullException(nameof(item));
            }
        }

        public Venue FindById(int id)
        {
            Venue venue = null;
            string command = $"SELECT * FROM [Venue] WHERE [Id] = @Id";
            SqlCommand cmd = new SqlCommand(command);
            SqlConnection connection = new SqlConnection(ConnectionString);
            connection.Open();
            cmd.Connection = connection;
            cmd.Parameters.AddWithValue("@Id", id);
            SqlDataReader dbreader = cmd.ExecuteReader();
            if (dbreader.Read())
            {
                venue = new Venue(dbreader.GetInt32(0), dbreader.GetString(1), dbreader.GetString(2), dbreader.GetString(3), dbreader.GetString(4));
            }

            dbreader.Close();
            connection.Close();
            return venue;
        }

        public List<Venue> GetAll()
        {
            List<Venue> venues = new List<Venue>();
            string command = $"SELECT * FROM [Venue]";
            SqlCommand cmd = new SqlCommand(command);
            SqlConnection connection = new SqlConnection(ConnectionString);
            connection.Open();
            cmd.Connection = connection;
            SqlDataReader dbreader = cmd.ExecuteReader();
            while (dbreader.Read())
            {
                Venue venue = new Venue(dbreader.GetInt32(0), dbreader.GetString(1), dbreader.GetString(2), dbreader.GetString(3), dbreader.GetString(4));
                venues.Add(venue);
            }

            dbreader.Close();
            connection.Close();
            return venues;
        }

        public void Remove(Venue item)
        {
            if (item != null)
            {
                string command = $"DELETE FROM [Venue] WHERE [Id] = @Id";
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

        public void Update(Venue item)
        {
            if (item != null)
            {
                string command = $"UPDATE [Venue] SET Name = @Name, Description = @Descr, Address = @Address, Phone = @Phone WHERE Id = @Id";
                SqlCommand cmd = new SqlCommand(command);
                SqlConnection connection = new SqlConnection(ConnectionString);
                connection.Open();
                cmd.Connection = connection;
                cmd.Parameters.AddWithValue("@Id", item.Id);
                cmd.Parameters.AddWithValue("@Name", item.Name);
                cmd.Parameters.AddWithValue("@Descr", item.Description);
                cmd.Parameters.AddWithValue("@Address", item.Address);
                cmd.Parameters.AddWithValue("@Phone", item.Phone);
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
