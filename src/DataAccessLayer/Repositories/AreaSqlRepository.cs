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
    // Repository for area type of data
    public class AreaSqlRepository : IRepository<Area>, ISqlRepository<Area>
    {
        // Repository filled with area data
        private List<Area> _areas;

        // Constructor that can get connection string
        public AreaSqlRepository(string connection)
        {
            ConnectionString = connection;
            _areas = new List<Area>();
        }

        // Creating repository without magor changes
        public AreaSqlRepository()
        {
            ConnectionString = @"Data Source =.\; Initial Catalog = TicketManagement; Integrated Security = true";
            _areas = new List<Area>();
        }

        public string ConnectionString { get; private set; }

        // Flag that used to check if data were taken from database
        public bool IsFilledWithDbData { get; private set; } = false;

        // Method that fills local repository with data from database
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

        // Method that add new area node to database
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

        // Method that search for area object with certain id
        public virtual Area FindById(int id)
        {
            return _areas.Find(elem => elem.Id == id);
        }

        // Method that returns repository to user
        public virtual List<Area> GetAll()
        {
            return _areas;
        }

        // Method that removes area object from repository with certain id
        public virtual void Remove(int id)
        {
            _areas.Remove(FindById(id));
            if (IsFilledWithDbData == true)
            {
                SaveChanges("Remove", new Area(id, 0, "", 0, 0));
            }
        }

        // Method that update object in repository
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

        // Method that brings changes from repository to database
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
