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
    // Repository for seat type of data
    public class SeatSqlRepository : IRepository<Seat>, ISqlRepository<Seat>
    {
        // Repository filled with seat data
        private List<Seat> _seats;

        // Constructor that can get connection string
        public SeatSqlRepository(string connection)
        {
            ConnectionString = connection;
            _seats = new List<Seat>();
        }

        // Creating repository without magor changes
        public SeatSqlRepository()
        {
            ConnectionString = @"Data Source =.\;Initial Catalog = TicketManagement; Integrated Security = true";
            _seats = new List<Seat>();
        }

        public string ConnectionString { get; private set; }

        // Flag that used to check if data were taken from database
        public bool IsFilledWithDbData { get; private set; } = false;

        // Method that fills local repository with data from database
        public virtual void FillRepositoryWithSqlData()
        {
            string command = $"SELECT * FROM [Seat]";
            SqlCommand cmd = new SqlCommand(command);
            SqlConnection connection = new SqlConnection(ConnectionString);
            connection.Open();
            cmd.Connection = connection;
            SqlDataReader dbreader = cmd.ExecuteReader();
            while (dbreader.Read())
            {
                Seat seat = new Seat(dbreader.GetInt32(0), dbreader.GetInt32(1), dbreader.GetInt32(2), dbreader.GetInt32(3));
                _seats.Add(seat);
            }

            dbreader.Close();
            connection.Close();
            IsFilledWithDbData = true;
        }

        // Method that add new seat node to database
        public virtual void Create(Seat item)
        {
            if (item != null)
            {
                _seats.Add(item);
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
        public virtual Seat FindById(int id)
        {
            return _seats.Find(elem => elem.Id == id);
        }

        // Method that returns repository to user
        public virtual List<Seat> GetAll()
        {
            return _seats;
        }

        // Method that removes layout object from repository with certain id
        public virtual void Remove(int id)
        {
            _seats.Remove(FindById(id));
            if (IsFilledWithDbData == true)
            {
                SaveChanges("Remove", new Seat(id, 0, 0, 0));
            }
        }

        // Method that update object in repository
        public virtual void Update(Seat item)
        {
            if (item != null)
            {
                for (int i = 0; i < _seats.Count; i++)
                {
                    if (_seats[i].Id == item.Id)
                    {
                        _seats[i] = item;
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
        public virtual void SaveChanges(string type, Seat item)
        {
            switch (type)
            {
                case "Add":
                    {
                        string command = $"INSERT INTO [Seat] (Id, AreaId, Row, Number) VALUES (@Id, @Area, @Row, @Numb)";
                        SqlCommand cmd = new SqlCommand(command);
                        SqlConnection connection = new SqlConnection(ConnectionString);
                        connection.Open();
                        cmd.Connection = connection;
                        cmd.Parameters.AddWithValue("@Id", item.Id);
                        cmd.Parameters.AddWithValue("@Area", item.AreaId);
                        cmd.Parameters.AddWithValue("@Row", item.Row);
                        cmd.Parameters.AddWithValue("@Numb", item.Number);
                        cmd.ExecuteNonQuery();
                        connection.Close();
                        break;
                    }

                case "Remove":
                    {
                        string command = $"DELETE FROM [Seat] WHERE [Id] = @Id";
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
                        string command = $"UPDATE [Seat] SET AreaId = @Area, Row = @Row, Number = @Numb WHERE Id = @Id";
                        SqlCommand cmd = new SqlCommand(command);
                        SqlConnection connection = new SqlConnection(ConnectionString);
                        connection.Open();
                        cmd.Connection = connection;
                        cmd.Parameters.AddWithValue("@Id", item.Id);
                        cmd.Parameters.AddWithValue("@Area", item.AreaId);
                        cmd.Parameters.AddWithValue("@Row", item.Row);
                        cmd.Parameters.AddWithValue("@Numb", item.Number);
                        cmd.ExecuteNonQuery();
                        connection.Close();
                        break;
                    }
            }
        }
    }
}
