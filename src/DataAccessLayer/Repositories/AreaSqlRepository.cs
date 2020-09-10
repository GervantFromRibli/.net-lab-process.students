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
        private List<Area> _areas;

        public AreaSqlRepository(string connection)
        {
            ConnectionString = connection;
            _areas = new List<Area>();
            FillRepository();
        }

        public AreaSqlRepository()
        {
            ConnectionString = @"Data Source =.\SQLEXPRESS;Initial Catalog = TicketManagement; Integrated Security = true";
            _areas = new List<Area>();
            FillRepository();
        }

        public string ConnectionString { get; private set; }

        private void FillRepository()
        {
            string command = $"SELECT * FROM [Area]";
            SqlCommand cmd = new SqlCommand(command);
            SqlConnection connection = new SqlConnection(ConnectionString);
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
            if (item != null)
            {
                _areas.Add(item);
                SaveChanges("Add", item);
            }
            else
            {
                throw new ArgumentNullException(nameof(item));
            }
        }

        public Area FindById(int id)
        {
            return _areas.Find(elem => elem.Id == id);
        }

        public List<Area> GetAll()
        {
            return _areas;
        }

        public void Remove(Area item)
        {
            if (item != null)
            {
                _areas.Remove(FindById(item.Id));
                SaveChanges("Remove", item);
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
                for (int i = 0; i < _areas.Count; i++)
                {
                    if (_areas[i].Id == item.Id)
                    {
                        _areas[i] = item;
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

        public void SaveChanges(string type, Area item)
        {
            switch (type)
            {
                case "Add":
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
                        break;
                    }

                case "Remove":
                    {
                        string command = $"DELETE FROM [Area] WHERE [Id] = @Id";
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
                        break;
                    }
            }
        }
    }
}
