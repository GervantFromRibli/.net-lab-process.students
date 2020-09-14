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
    // Repository for venue type of data
    public class VenueSqlRepository : IRepository<Venue>, ISqlRepository<Venue>
    {
        // Repository filled with venue data
        private List<Venue> _venues;

        // Constructor that can get connection string
        public VenueSqlRepository(string connection)
        {
            ConnectionString = connection;
            _venues = new List<Venue>();
        }

        // Creating repository without magor changes
        public VenueSqlRepository()
        {
            ConnectionString = @"Data Source =.\;Initial Catalog = TicketManagement; Integrated Security = true";
            _venues = new List<Venue>();
        }

        public string ConnectionString { get; private set; }

        // Flag that used to check if data were taken from database
        public bool IsFilledWithDbData { get; private set; } = false;

        // Method that fills local repository with data from database
        public virtual void FillRepositoryWithSqlData()
        {
            string command = $"SELECT * FROM [Venue]";
            SqlCommand cmd = new SqlCommand(command);
            SqlConnection connection = new SqlConnection(ConnectionString);
            connection.Open();
            cmd.Connection = connection;
            SqlDataReader dbreader = cmd.ExecuteReader();
            while (dbreader.Read())
            {
                Venue venue = new Venue(dbreader.GetInt32(0), dbreader.GetString(1), dbreader.GetString(2), dbreader.GetString(3), dbreader.GetString(4));
                _venues.Add(venue);
            }

            dbreader.Close();
            connection.Close();
            IsFilledWithDbData = true;
        }

        // Method that add new venue node to database
        public virtual void Create(Venue item)
        {
            if (item != null)
            {
                _venues.Add(item);
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

        // Method that search for venue object with certain id
        public virtual Venue FindById(int id)
        {
            return _venues.Find(elem => elem.Id == id);
        }

        // Method that returns repository to user
        public virtual List<Venue> GetAll()
        {
            return _venues;
        }

        // Method that removes layout object from repository with certain id
        public virtual void Remove(int id)
        {
            _venues.Remove(FindById(id));
            if (IsFilledWithDbData == true)
            {
                SaveChanges("Remove", new Venue(id, "", "", ""));
            }
        }

        // Method that update object in repository
        public virtual void Update(Venue item)
        {
            if (item != null)
            {
                for (int i = 0; i < _venues.Count; i++)
                {
                    if (_venues[i].Id == item.Id)
                    {
                        _venues[i] = item;
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
        public virtual void SaveChanges(string type, Venue item)
        {
            switch (type)
            {
                case "Add":
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
                        break;
                    }

                case "Remove":
                    {
                        string command = $"DELETE FROM [Venue] WHERE [Id] = @Id";
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
                        break;
                    }
            }
        }
    }
}
