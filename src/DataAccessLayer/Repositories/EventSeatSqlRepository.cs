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
    // Repository for event seat type of data
    public class EventSeatSqlRepository : IRepository<EventSeat>, ISqlRepository<EventSeat>
    {
        // Repository filled with event seat data
        private List<EventSeat> _eventSeats;

        // Constructor that can get connection string
        public EventSeatSqlRepository(string connection)
        {
            ConnectionString = connection;
            _eventSeats = new List<EventSeat>();
        }

        // Creating repository without magor changes
        public EventSeatSqlRepository()
        {
            ConnectionString = @"Data Source =.\;Initial Catalog = TicketManagement; Integrated Security = true";
            _eventSeats = new List<EventSeat>();
        }

        public string ConnectionString { get; private set; }

        // Flag that used to check if data were taken from database
        public bool IsFilledWithDbData { get; private set; } = false;

        // Method that fills local repository with data from database
        public virtual void FillRepositoryWithSqlData()
        {
            string command = $"SELECT * FROM [EventSeat]";
            SqlCommand cmd = new SqlCommand(command);
            SqlConnection connection = new SqlConnection(ConnectionString);
            connection.Open();
            cmd.Connection = connection;
            SqlDataReader dbreader = cmd.ExecuteReader();
            while (dbreader.Read())
            {
                EventSeat eventSeat = new EventSeat(dbreader.GetInt32(0), dbreader.GetInt32(1), dbreader.GetInt32(2), dbreader.GetInt32(3), dbreader.GetInt32(4));
                _eventSeats.Add(eventSeat);
            }

            dbreader.Close();
            connection.Close();
            IsFilledWithDbData = true;
        }

        // Method that add new event seat node to database
        public virtual void Create(EventSeat item)
        {
            if (item != null)
            {
                _eventSeats.Add(item);
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

        // Method that search for event seat object with certain id
        public virtual EventSeat FindById(int id)
        {
            return _eventSeats.Find(elem => elem.Id == id);
        }

        // Method that returns repository to user
        public virtual List<EventSeat> GetAll()
        {
            return _eventSeats;
        }

        // Method that removes event seat object from repository with certain id
        public virtual void Remove(int id)
        {
            _eventSeats.Remove(FindById(id));
            if (IsFilledWithDbData == true)
            {
                SaveChanges("Remove", new EventSeat(id, 0, 0, 0, 0));
            }
        }

        // Method that update object in repository
        public virtual void Update(EventSeat item)
        {
            if (item != null)
            {
                for (int i = 0; i < _eventSeats.Count; i++)
                {
                    if (_eventSeats[i].Id == item.Id)
                    {
                        _eventSeats[i] = item;
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
        public virtual void SaveChanges(string type, EventSeat item)
        {
            switch (type)
            {
                case "Add":
                    {
                        string command = $"INSERT INTO [EventSeat] (Id, EventAreaId, Row, Number, State) VALUES (@Id, @EventArea, @Row, @Numb, @State)";
                        SqlCommand cmd = new SqlCommand(command);
                        SqlConnection connection = new SqlConnection(ConnectionString);
                        connection.Open();
                        cmd.Connection = connection;
                        cmd.Parameters.AddWithValue("@Id", item.Id);
                        cmd.Parameters.AddWithValue("@EventArea", item.EventAreaId);
                        cmd.Parameters.AddWithValue("@Row", item.Row);
                        cmd.Parameters.AddWithValue("@Numb", item.Number);
                        cmd.Parameters.AddWithValue("@State", item.State);
                        cmd.ExecuteNonQuery();
                        connection.Close();
                        break;
                    }

                case "Remove":
                    {
                        string command = $"DELETE FROM [EventSeat] WHERE [Id] = @Id";
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
                        string command = $"UPDATE [EventSeat] SET EventAreaId = @EventArea, Row = @Row, Number = @Numb, State = @State WHERE Id = @Id";
                        SqlCommand cmd = new SqlCommand(command);
                        SqlConnection connection = new SqlConnection(ConnectionString);
                        connection.Open();
                        cmd.Connection = connection;
                        cmd.Parameters.AddWithValue("@Id", item.Id);
                        cmd.Parameters.AddWithValue("@EventArea", item.EventAreaId);
                        cmd.Parameters.AddWithValue("@Row", item.Row);
                        cmd.Parameters.AddWithValue("@Numb", item.Number);
                        cmd.Parameters.AddWithValue("@State", item.State);
                        cmd.ExecuteNonQuery();
                        connection.Close();
                        break;
                    }
            }
        }
    }
}
