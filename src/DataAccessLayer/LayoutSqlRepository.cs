using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DomainEntities;

namespace DataAccessLayer
{
    public class LayoutSqlRepository : IRepository<Layout>
    {
        public LayoutSqlRepository(string connection)
        {
            ConnectionString = connection;
        }

        public LayoutSqlRepository()
        {
            ConnectionString = @"Data Source =.\SQLEXPRESS;Initial Catalog = TicketManagement; Integrated Security = true";
        }

        public string ConnectionString { get; private set; }

        public void Create(Layout item)
        {
            if (item != null)
            {
                string command = $"INSERT INTO [Layout] (Id, Name, VenueId, Description) VALUES (@Id, @Name, @Venue, @Descr)";
                SqlCommand cmd = new SqlCommand(command);
                SqlConnection connection = new SqlConnection(ConnectionString);
                connection.Open();
                cmd.Connection = connection;
                cmd.Parameters.AddWithValue("@Id", item.Id);
                cmd.Parameters.AddWithValue("@Name", item.Name);
                cmd.Parameters.AddWithValue("@Venue", item.VenueId);
                cmd.Parameters.AddWithValue("@Descr", item.Description);
                cmd.ExecuteNonQuery();
                connection.Close();
            }
            else
            {
                throw new ArgumentNullException(nameof(item));
            }
        }

        public Layout FindById(int id)
        {
            Layout layout = null;
            string command = $"SELECT * FROM [Layout] WHERE [Id] = @Id";
            SqlCommand cmd = new SqlCommand(command);
            SqlConnection connection = new SqlConnection(ConnectionString);
            connection.Open();
            cmd.Connection = connection;
            cmd.Parameters.AddWithValue("@Id", id);
            SqlDataReader dbreader = cmd.ExecuteReader();
            if (dbreader.Read())
            {
                layout = new Layout(dbreader.GetInt32(0), dbreader.GetString(1), dbreader.GetInt32(2), dbreader.GetString(3));
            }

            dbreader.Close();
            connection.Close();
            return layout;
        }

        public List<Layout> GetAll()
        {
            List<Layout> layouts = new List<Layout>();
            string command = $"SELECT * FROM [Layout]";
            SqlCommand cmd = new SqlCommand(command);
            SqlConnection connection = new SqlConnection(ConnectionString);
            connection.Open();
            cmd.Connection = connection;
            SqlDataReader dbreader = cmd.ExecuteReader();
            while (dbreader.Read())
            {
                Layout layout = new Layout(dbreader.GetInt32(0), dbreader.GetString(1), dbreader.GetInt32(2), dbreader.GetString(3));
                layouts.Add(layout);
            }

            dbreader.Close();
            connection.Close();
            return layouts;
        }

        public void Remove(Layout item)
        {
            if (item != null)
            {
                string command = $"DELETE FROM [Layout] WHERE [Id] = @Id";
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

        public void Update(Layout item)
        {
            if (item != null)
            {
                string command = $"UPDATE [Layout] SET Name = @Name, VenueId = @Venue, Description = @Descrt WHERE Id = @Id";
                SqlCommand cmd = new SqlCommand(command);
                SqlConnection connection = new SqlConnection(ConnectionString);
                connection.Open();
                cmd.Connection = connection;
                cmd.Parameters.AddWithValue("@Id", item.Id);
                cmd.Parameters.AddWithValue("@Name", item.Name);
                cmd.Parameters.AddWithValue("@Venue", item.VenueId);
                cmd.Parameters.AddWithValue("@Descr", item.Description);
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
