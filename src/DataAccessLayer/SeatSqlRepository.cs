using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DomainEntities;

namespace DataAccessLayer
{
    public class SeatSqlRepository : IRepository<Seat>
    {
        public SeatSqlRepository(string connection)
        {
            ConnectionString = connection;
        }

        public SeatSqlRepository()
        {
            ConnectionString = @"Data Source =.\SQLEXPRESS;Initial Catalog = TicketManagement; Integrated Security = true";
        }

        public string ConnectionString { get; private set; }

        public void Create(Seat item)
        {
            if (item != null)
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
            }
            else
            {
                throw new ArgumentNullException(nameof(item));
            }
        }

        public Seat FindById(int id)
        {
            Seat seat = null;
            string command = $"SELECT * FROM [Seat] WHERE [Id] = @Id";
            SqlCommand cmd = new SqlCommand(command);
            SqlConnection connection = new SqlConnection(ConnectionString);
            connection.Open();
            cmd.Connection = connection;
            cmd.Parameters.AddWithValue("@Id", id);
            SqlDataReader dbreader = cmd.ExecuteReader();
            if (dbreader.Read())
            {
                seat = new Seat(dbreader.GetInt32(0), dbreader.GetInt32(1), dbreader.GetInt32(2), dbreader.GetInt32(3));
            }

            dbreader.Close();
            connection.Close();
            return seat;
        }

        public List<Seat> GetAll()
        {
            List<Seat> seats = new List<Seat>();
            string command = $"SELECT * FROM [Seat]";
            SqlCommand cmd = new SqlCommand(command);
            SqlConnection connection = new SqlConnection(ConnectionString);
            connection.Open();
            cmd.Connection = connection;
            SqlDataReader dbreader = cmd.ExecuteReader();
            while (dbreader.Read())
            {
                Seat seat = new Seat(dbreader.GetInt32(0), dbreader.GetInt32(1), dbreader.GetInt32(2), dbreader.GetInt32(3));
                seats.Add(seat);
            }

            dbreader.Close();
            connection.Close();
            return seats;
        }

        public void Remove(Seat item)
        {
            if (item != null)
            {
                string command = $"DELETE FROM [Seat] WHERE [Id] = @Id";
                SqlCommand cmd = new SqlCommand(command);
                SqlConnection connection = new SqlConnection(ConnectionString);
                connection.Open();
                cmd.Connection = connection;
                cmd.Parameters.AddWithValue("@Id", item.Id);
                cmd.ExecuteNonQuery();
                connection.Close();
            }
            else
            {
                throw new ArgumentNullException(nameof(item));
            }
        }

        public void Update(Seat item)
        {
            if (item != null)
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
            }
            else
            {
                throw new ArgumentNullException(nameof(item));
            }
        }
    }
}
