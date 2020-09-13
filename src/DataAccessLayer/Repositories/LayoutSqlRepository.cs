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
        private List<Layout> _layouts;

        public LayoutSqlRepository(string connection)
        {
            ConnectionString = connection;
            _layouts = new List<Layout>();
        }

        public LayoutSqlRepository()
        {
            ConnectionString = @"Data Source =.\SQLEXPRESS;Initial Catalog = TicketManagement; Integrated Security = true";
            _layouts = new List<Layout>();
        }

        public string ConnectionString { get; private set; }

        public bool IsFilledWithDbData { get; private set; } = false;

        public virtual void FillRepository()
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

        public virtual Layout FindById(int id)
        {
            return _layouts.Find(elem => elem.Id == id);
        }

        public virtual List<Layout> GetAll()
        {
            return _layouts;
        }

        public virtual void Remove(int id)
        {
            _layouts.Remove(FindById(id));
            if (IsFilledWithDbData == true)
            {
                SaveChanges("Remove", new Layout(id, "", 0, ""));
            }
        }

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
