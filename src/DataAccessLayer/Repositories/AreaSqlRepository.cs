using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer.Repositories;
using DomainEntities;

namespace DataAccessLayer
{
    public class AreaSqlRepository : IRepository<Area>, ISqlRepository<Area>
    {
        private List<Area> _areas;

        public AreaSqlRepository(string connection)
        {
            ConnectionString = connection;
            _areas = new List<Area>();
        }

        public AreaSqlRepository()
        {
            ConnectionString = @"Data Source =.\SQLEXPRESS;Initial Catalog = TicketManagement; Integrated Security = true";
            _areas = new List<Area>();
        }

        public string ConnectionString { get; private set; }

        public bool IsFilledWithDbData { get; private set; } = false;

        public virtual void FillRepositoryWithSqlData()
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
            IsFilledWithDbData = true;
        }

        public virtual void Create(Area item)
        {
            if (item != null)
            {
                _areas.Add(item);
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

        public virtual Area FindById(int id)
        {
            return _areas.Find(elem => elem.Id == id);
        }

        public virtual List<Area> GetAll()
        {
            return _areas;
        }

        public virtual void Remove(int id)
        {
            _areas.Remove(FindById(id));
            if (IsFilledWithDbData == true)
            {
                SaveChanges("Remove", new Area(id, 0, "", 0, 0));
            }
        }

        public virtual void Update(Area item)
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

        public virtual void SaveChanges(string type, Area item)
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
