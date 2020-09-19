using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer.Repositories;
using DomainEntities;

namespace DataAccessLayer
{
    // Repository for layout type of data
    public class LayoutSqlRepository : IRepository<Layout>, ISqlRepository<Layout>
    {
        // Repository filled with layout data
        private List<Layout> _layouts;

        // Constructor that can get connection string
        public LayoutSqlRepository(string connection)
        {
            ConnectionString = connection;
            _layouts = new List<Layout>();
        }

        // Creating repository without magor changes
        public LayoutSqlRepository()
        {
            ConnectionString = @"Data Source =.\;Initial Catalog = TicketManagement; Integrated Security = true";
            _layouts = new List<Layout>();
        }

        public string ConnectionString { get; private set; }

        // Flag that used to check if data were taken from database
        public bool IsFilledWithDbData { get; private set; } = false;

        // Method that fills local repository with data from database
        public virtual void FillRepositoryWithSqlData()
        {
            string command = $"SELECT * FROM [Layout]";
            SqlCommand cmd = new SqlCommand(command);
            SqlConnection connection = new SqlConnection(ConnectionString);
            connection.Open();
            cmd.Connection = connection;
            SqlDataReader dbreader = cmd.ExecuteReader();
            while (dbreader.Read())
            {
                Layout layout = new Layout(dbreader.GetInt32(0), dbreader.GetString(1), dbreader.GetInt32(2), dbreader.GetString(3));
                _layouts.Add(layout);
            }

            dbreader.Close();
            connection.Close();
            IsFilledWithDbData = true;
        }

        // Method that add new layout node to database
        public virtual void Create(Layout item)
        {
            if (item != null)
            {
                _layouts.Add(item);
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

        // Method that search for layout object with certain id
        public virtual Layout FindById(int id)
        {
            return _layouts.Find(elem => elem.Id == id);
        }

        // Method that returns repository to user
        public virtual List<Layout> GetAll()
        {
            return _layouts;
        }

        // Method that removes layout object from repository with certain id
        public virtual void Remove(int id)
        {
            _layouts.Remove(FindById(id));
            if (IsFilledWithDbData == true)
            {
                SaveChanges("Remove", new Layout(id, "", 0, ""));
            }
        }

        // Method that update object in repository
        public virtual void Update(Layout item)
        {
            if (item != null)
            {
                for (int i = 0; i < _layouts.Count; i++)
                {
                    if (_layouts[i].Id == item.Id)
                    {
                        _layouts[i] = item;
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
        public virtual void SaveChanges(string type, Layout item)
        {
            switch (type)
            {
                case "Add":
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
                        break;
                    }

                case "Remove":
                    {
                        string command = $"DELETE FROM [Layout] WHERE [Id] = @Id";
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
                        break;
                    }
            }
        }
    }
}
