using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using DomainEntities;

namespace DataAccessLayer
{
    public class AreaSqlRepository : IRepository<Area>
    {
        public AreaSqlRepository(string connection)
        {
            ConnectionString = connection;
        }

        public AreaSqlRepository()
        {
            ConnectionString = @"Data Source =.\SQLEXPRESS;Initial Catalog = TicketManagement; Integrated Security = true";
        }

        public string ConnectionString { get; private set; }

        public void Create(Area item)
        {
            if (item != null)
            {
                string command = $"INSERT INTO [Area] (Id, LayoutId, Description, CoordX, CoordY) VALUES (@Id, @Layout, @Descr, @X, @Y)";
                SqlCommand cmd = new SqlCommand(command);
                SqlConnection connection = new SqlConnection(ConnectionString);
                connection.Open();
                cmd.Connection = connection;
                cmd.Parameters.AddWithValue("@Id", item.Id);
                cmd.Parameters.AddWithValue("@Layout", item.LayoutId);
                cmd.Parameters.AddWithValue("@Descr", item.Description);
                cmd.Parameters.AddWithValue("@X", item.CoordX);
                cmd.Parameters.AddWithValue("@Y", item.CoordY);
                cmd.ExecuteNonQuery();
                connection.Close();
            }
            else
            {
                throw new ArgumentNullException(nameof(item));
            }
        }

        public Area FindById(int id)
        {
            Area area = null;
            string command = $"SELECT * FROM [Area] WHERE [Id] = @Id";
            SqlCommand cmd = new SqlCommand(command);
            SqlConnection connection = new SqlConnection(ConnectionString);
            connection.Open();
            cmd.Connection = connection;
            cmd.Parameters.AddWithValue("@Id", id);
            SqlDataReader dbreader = cmd.ExecuteReader();
            if (dbreader.Read())
            {
                area = new Area(dbreader.GetInt32(0), dbreader.GetInt32(1), dbreader.GetString(2), dbreader.GetInt32(3), dbreader.GetInt32(4));
            }

            dbreader.Close();
            connection.Close();
            return area;
        }

        public List<Area> GetAll()
        {
            List<Area> areas = new List<Area>();
            string command = $"SELECT * FROM [Area]";
            SqlCommand cmd = new SqlCommand(command);
            SqlConnection connection = new SqlConnection(ConnectionString);
            connection.Open();
            cmd.Connection = connection;
            SqlDataReader dbreader = cmd.ExecuteReader();
            while (dbreader.Read())
            {
                Area elem = new Area(dbreader.GetInt32(0), dbreader.GetInt32(1), dbreader.GetString(2), dbreader.GetInt32(3), dbreader.GetInt32(4));
                areas.Add(elem);
            }

            dbreader.Close();
            connection.Close();
            return areas;
        }

        public void Remove(Area item)
        {
            if (item != null)
            {
                string command = $"DELETE FROM [Area] WHERE [Id] = @Id";
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

        public void Update(Area item)
        {
            if (item != null)
            {
                string command = $"UPDATE [Area] SET LayoutId = @Layout, Description = @Descr, CoordX = @X, CoordY = @Y WHERE Id = @Id";
                SqlCommand cmd = new SqlCommand(command);
                SqlConnection connection = new SqlConnection(ConnectionString);
                connection.Open();
                cmd.Connection = connection;
                cmd.Parameters.AddWithValue("@Id", item.Id);
                cmd.Parameters.AddWithValue("@Layout", item.LayoutId);
                cmd.Parameters.AddWithValue("@Descr", item.Description);
                cmd.Parameters.AddWithValue("@X", item.CoordX);
                cmd.Parameters.AddWithValue("@Y", item.CoordY);
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
