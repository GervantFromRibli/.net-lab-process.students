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
    public class SeatSqlRepository : IRepository<Seat>, ISqlRepository<Seat>
    {
        private List<Seat> _seats;

        public SeatSqlRepository(string connection)
        {
            ConnectionString = connection;
            _seats = new List<Seat>();
        }

        public SeatSqlRepository()
        {
            ConnectionString = @"Data Source =.\SQLEXPRESS;Initial Catalog = TicketManagement; Integrated Security = true";
            _seats = new List<Seat>();
        }

        public string ConnectionString { get; private set; }

        public bool IsFilledWithDbData { get; private set; } = false;

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

        public virtual Seat FindById(int id)
        {
            return _seats.Find(elem => elem.Id == id);
        }

        public virtual List<Seat> GetAll()
        {
            return _seats;
        }

        public virtual void Remove(int id)
        {
            _seats.Remove(FindById(id));
            if (IsFilledWithDbData == true)
            {
                SaveChanges("Remove", new Seat(id, 0, 0, 0));
            }
        }

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
