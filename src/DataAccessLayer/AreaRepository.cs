using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DomainEntities;

namespace DataAccessLayer
{
    public class AreaRepository : IRepository<Area>
    {
        private List<Area> _areas;

        private string _connectionString = @"Data Source =.\SQLEXPRESS;Initial Catalog = TicketManagement; Integrated Security = true";

        public AreaRepository()
        {
            _areas = new List<Area>();
            string command = $"SELECT * FROM [Area]";
            SqlCommand cmd = new SqlCommand(command);
            SqlConnection connection = new SqlConnection(_connectionString);
            connection.Open();
            cmd.Connection = connection;
            SqlDataReader dbreader = cmd.ExecuteReader();
            while (dbreader.Read())
            {
                Area elem = new Area(dbreader.GetInt32(0), dbreader.GetInt32(1), dbreader.GetString(2), dbreader.GetInt32(3), dbreader.GetInt32(4));
                _areas.Add(elem);
            }

            dbreader.Close();
            connection.Close();
        }

        public void Create(Area item)
        {
            _areas.Add(item);
            SaveChanges();
        }

        public Area FindById(int id)
        {
            return _areas.Select(elem => elem).Where(elem => elem.Id == id).Single();
        }

        public List<Area> GetAll()
        {
            return _areas;
        }

        public void Remove(Area item)
        {
            _areas.Remove(item);
            SaveChanges();
        }

        public void Update(Area item)
        {
            foreach (var elem in _areas)
            {
                if (elem.Id == item.Id)
                {
                    elem.LayoutId = item.LayoutId;
                    elem.Description = item.Description;
                    elem.CoordX = item.CoordX;
                    elem.CoordY = item.CoordY;
                    SaveChanges();
                    break;
                }
            }
        }

        public void Dispose()
        {
            _areas.Clear();
        }

        public void SaveChanges()
        {
            string command = $"DELETE FROM [Area]";
            SqlCommand cmd = new SqlCommand(command);
            SqlConnection connection = new SqlConnection(_connectionString);
            connection.Open();
            cmd.Connection = connection;
            cmd.ExecuteNonQuery();
            command = $"INSERT INTO [Area] (Id, LayoutId, Description, CoordX, CoordY) VALUES (@Id, @Layout, @Descr, @X, @Y)";
            foreach (var elem in _areas)
            {
                cmd = new SqlCommand(command);
                cmd.Connection = connection;
                cmd.Parameters.AddWithValue("@Id", elem.Id);
                cmd.Parameters.AddWithValue("@Layout", elem.LayoutId);
                cmd.Parameters.AddWithValue("@Descr", elem.Description);
                cmd.Parameters.AddWithValue("@X", elem.CoordX);
                cmd.Parameters.AddWithValue("@Y", elem.CoordY);
                cmd.ExecuteNonQuery();
            }

            connection.Close();
        }
    }
}
