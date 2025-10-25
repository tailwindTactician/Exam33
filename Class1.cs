using System;
using System.Data;
using Npgsql;

 ApartmentService service = new ApartmentService();

        Apartment a1 = new Apartment();
        a1.Area = 55;
        a1.Floor = 3;
        a1.Bedrooms = 2;
        a1.Price = 50000;
        a1.NumberOfApartment = 12;

        service.Add(a1);

        Console.WriteLine("=== Все квартиры ===");
        service.ShowAll();

        Console.WriteLine("\n=== Квартиры с площадью 48-60 ===");
        service.ShowByArea("48-60");

        Console.WriteLine("\n=== Обновление цены ===");
        service.UpdateByPrice(1, 60000);

        Console.WriteLine("\n=== После обновления ===");
        service.ShowAll();
public class Apartment
{
    public int Id { get; set; }
    public int Area { get; set; }
    public int Floor { get; set; }
    public int Bedrooms { get; set; }
    public decimal Price { get; set; }
    public int NumberOfApartment { get; set; }
}

public class ApartmentService
{
    string connString = "Host=localhost;Port=5432;Database=Exam3;Username=postgres;Password=1234";

    public void Add(Apartment a)
    {
        using (var conn = new NpgsqlConnection(connString))
        {
            conn.Open();
            string sql = "INSERT INTO Apartments (Area, Floor, Bedrooms, Price, NumberOfApartment) VALUES (@Area, @Floor, @Bedrooms, @Price, @NumberOfApartment)";
            using (var cmd = new NpgsqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("Area", a.Area);
                cmd.Parameters.AddWithValue("Floor", a.Floor);
                cmd.Parameters.AddWithValue("Bedrooms", a.Bedrooms);
                cmd.Parameters.AddWithValue("Price", a.Price);
                cmd.Parameters.AddWithValue("NumberOfApartment", a.NumberOfApartment);
                cmd.ExecuteNonQuery();
            }
        }
    }

    public void ShowByArea(string areaCondition)
    {
        string query = "";

        if (areaCondition == "<48")
        {
            query = "SELECT * FROM Apartments WHERE Area < 48";
        }
        else if (areaCondition == "48-60")
        {
            query = "SELECT * FROM Apartments WHERE Area >= 48 AND Area <= 60";
        }
        else if (areaCondition == ">60")
        {
            query = "SELECT * FROM Apartments WHERE Area > 60";
        }
        else
        {
            query = "SELECT * FROM Apartments";
        }

        using (var conn = new NpgsqlConnection(connString))
        {
            conn.Open();
            using (var cmd = new NpgsqlCommand(query, conn))
            {
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Console.WriteLine(
                            "ID: " + reader["id"] +
                            ", Area: " + reader["area"] +
                            ", Floor: " + reader["floor"] +
                            ", Bedrooms: " + reader["bedrooms"] +
                            ", Price: " + reader["price"] +
                            ", Number: " + reader["numberofapartment"]);
                    }
                }
            }
        }
    }

    public void UpdateByPrice(int id, decimal newPrice)
    {
        using (var conn = new NpgsqlConnection(connString))
        {
            conn.Open();
            string sql = "UPDATE Apartments SET Price=@Price WHERE Id=@Id";
            using (var cmd = new NpgsqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("Id", id);
                cmd.Parameters.AddWithValue("Price", newPrice);
                cmd.ExecuteNonQuery();
            }
        }
    }

    public void DeleteByNumberOfApartment(int numberOfApartment)
    {
        using (var conn = new NpgsqlConnection(connString))
        {
            conn.Open();
            string sql = "DELETE FROM Apartments WHERE NumberOfApartment=@NumberOfApartment";
            using (var cmd = new NpgsqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("NumberOfApartment", numberOfApartment);
                cmd.ExecuteNonQuery();
            }
        }
    }

    public void ShowAll()
    {
        using (var conn = new NpgsqlConnection(connString))
        {
            conn.Open();
            string sql = "SELECT * FROM Apartments";
            using (var cmd = new NpgsqlCommand(sql, conn))
            {
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Console.WriteLine(
                            "ID: " + reader["id"] +
                            ", Area: " + reader["area"] +
                            ", Floor: " + reader["floor"] +
                            ", Bedrooms: " + reader["bedrooms"] +
                            ", Price: " + reader["price"] +
                            ", Number: " + reader["numberofapartment"]);
                    }
                }
            }
        }
    }
}

       
