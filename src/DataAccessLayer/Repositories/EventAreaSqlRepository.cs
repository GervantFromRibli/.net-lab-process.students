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
    public class EventAreaSqlRepository : IRepository<EventArea>, ISqlRepository<EventArea>
    {
        private List<EventArea> _eventAreas;

        public EventAreaSqlRepository(string connection)
        {
            ConnectionString = connection;
            _eventAreas = new List<EventArea>();
        }

        public EventAreaSqlRepository()
        {
            ConnectionString = @"Data Source =.\SQLEXPRESS;Initial Catalog = TicketManagement; Integrated Security = true";
            _eventAreas = new List<EventArea>();
        }

        public string ConnectionString { get; private set; }

        public bool IsFilledWithDbData { get; private set; } = false;

        public virtual void FillRepositoryWithSqlData()
        {
            string command = $"SELECT * FROM [EventArea]";
            SqlCommand cmd = new SqlCommand(command);
            SqlConnection connection = new SqlConnection(ConnectionString);
            connection.Open();
            cmd.Connection = connection;
            SqlDataReader dbreader = cmd.ExecuteReader();
            while (dbreader.Read())
            {
                EventArea eventArea = new EventArea(dbreader.GetInt32(0), dbreader.GetInt32(1), dbreader.GetString(2), dbreader.GetInt32(3), dbreader.GetInt32(4), dbreader.GetDecimal(5));
                _eventAreas.Add(eventArea);
            }

            dbreader.Close();
            connection.Close();
            IsFilledWithDbData = true;
        }

        public virtual void Create(EventArea item)
        {
            if (item != null)
            {
                _eventAreas.Add(item);
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

        public virtual EventArea FindById(int id)
        {
            return _eventAreas.Find(elem => elem.Id == id);
        }

        public virtual List<EventArea> GetAll()
        {
            return _eventAreas;
        }

        public virtual void Remove(int id)
        {
            _eventAreas.Remove(FindById(id));
            if (IsFilledWithDbData == true)
            {
                SaveChanges("Remove", new EventArea(id, 0, "", 0, 0, 0));
            }
        }

        public virtual void Update(EventArea item)
        {
            if (item != null)
            {
                for (int i = 0; i < _eventAreas.Count; i++)
                {
                    if (_eventAreas[i].Id == item.Id)
                    {
                        _eventAreas[i] = item;
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

        public virtual void SaveChanges(string type, EventArea item)
        {
            switch (type)
            {
                case "Add":
                    {
                        string command = $"INSERT INTO [EventArea] (Id, EventId, Description, CoordX, CoordY, Price) VALUES (@Id, @Event, @Descr, @X, @Y, @Price)";
                        SqlCommand cmd = new SqlCommand(command);
                        SqlConnection connection = new SqlConnection(ConnectionString);
                        connection.Open();
                        cmd.Connection = connection;
                        cmd.Parameters.AddWithValue("@Id", item.Id);
                        cmd.Parameters.AddWithValue("@Event", item.EventId);
                        cmd.Parameters.AddWithValue("@Descr", item.Description);
                        cmd.Parameters.AddWithValue("@X", item.CoordX);
                        cmd.Parameters.AddWithValue("@Y", item.CoordY);
                        cmd.Parameters.AddWithValue("@Price", item.Price);
                        cmd.ExecuteNonQuery();
                        connection.Close();
                        break;
                    }

                case "Remove":
                    {
                        string command = $"DELETE FROM [EventArea] WHERE [Id] = @Id";
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
                        string command = $"UPDATE [EventArea] SET EventId = @Event, Description = @Descr, CoordX = @X, CoordY = @Y, Price = @Price WHERE Id = @Id";
                        SqlCommand cmd = new SqlCommand(command);
                        SqlConnection connection = new SqlConnection(ConnectionString);
                        connection.Open();
                        cmd.Connection = connection;
                        cmd.Parameters.AddWithValue("@Id", item.Id);
                        cmd.Parameters.AddWithValue("@Event", item.EventId);
                        cmd.Parameters.AddWithValue("@Descr", item.Description);
                        cmd.Parameters.AddWithValue("@X", item.CoordX);
                        cmd.Parameters.AddWithValue("@Y", item.CoordY);
                        cmd.Parameters.AddWithValue("@Price", item.Price);
                        cmd.ExecuteNonQuery();
                        connection.Close();
                        break;
                    }
            }
        }
    }
}
