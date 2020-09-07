using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DomainEntities;

namespace DataAccessLayer
{
    public class SeatRepository : IRepository<Seat>
    {
        private List<Seat> _seats;

        private string _connectionString = @"Data Source =.\SQLEXPRESS;Initial Catalog = TicketManagement; Integrated Security = true";

        public SeatRepository()
        {
            _seats = new List<Seat>();
            string command = $"SELECT * FROM [Seat]";
            SqlCommand cmd = new SqlCommand(command);
            SqlConnection connection = new SqlConnection(_connectionString);
            connection.Open();
            cmd.Connection = connection;
            SqlDataReader dbreader = cmd.ExecuteReader();
            while (dbreader.Read())
            {
                Seat elem = new Seat(dbreader.GetInt32(0), dbreader.GetInt32(1), dbreader.GetInt32(2), dbreader.GetInt32(3));
                _seats.Add(elem);
            }

            dbreader.Close();
            connection.Close();
        }

        public void Create(Seat item)
        {
            _seats.Add(item);
            SaveChanges();
        }

        public Seat FindById(int id)
        {
            return _seats.Select(elem => elem).Where(elem => elem.Id == id).Single();
        }

        public List<Seat> GetAll()
        {
            return _seats;
        }

        public void Remove(Seat item)
        {
            _seats.Remove(item);
            SaveChanges();
        }

        public void Update(Seat item)
        {
            foreach (var elem in _seats)
            {
                if (elem.Id == item.Id)
                {
                    elem.AreaId = item.AreaId;
                    elem.Row = item.Row;
                    elem.Number = item.Number;
                    SaveChanges();
                    break;
                }
            }
        }

        public void Dispose()
        {
            _seats.Clear();
        }

        public void SaveChanges()
        {
            string command = $"DELETE FROM [Seat]";
            SqlCommand cmd = new SqlCommand(command);
            SqlConnection connection = new SqlConnection(_connectionString);
            connection.Open();
            cmd.Connection = connection;
            cmd.ExecuteNonQuery();
            command = $"INSERT INTO [Seat] (Id, AreaId, Row, Number) VALUES (@Id, @Area, @Row, @Numb)";
            foreach (var elem in _seats)
            {
                cmd = new SqlCommand(command);
                cmd.Connection = connection;
                cmd.Parameters.AddWithValue("@Id", elem.Id);
                cmd.Parameters.AddWithValue("@Area", elem.AreaId);
                cmd.Parameters.AddWithValue("@Row", elem.Row);
                cmd.Parameters.AddWithValue("@Numb", elem.Number);
                cmd.ExecuteNonQuery();
            }

            connection.Close();
        }
    }
}
