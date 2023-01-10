using asp.net.Models;
using System;
using System.Data;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;

namespace asp.net.Services;

public static class PizzaService
{
    public static string user = Environment.GetEnvironmentVariable("DBUSER") ?? "";
    public static string pwd = Environment.GetEnvironmentVariable("DBUSERPASS") ?? "";
    public static string db = Environment.GetEnvironmentVariable("DB") ?? "";
    public static string conStringUser = "User Id=" + user + ";Password=" + pwd + ";Data Source=" + db + ";";
    static List<Pizza> Pizzas { get; set; } = new List<Pizza>();

    public static List<Pizza> GetAll()
    {
        // Open DB connection
        OracleConnection con = new OracleConnection(conStringUser);
        con.Open();

        // Write SQL query
        OracleCommand cmd = new OracleCommand("SELECT ID, NAME, ISGLUTENFREE FROM PIZZATABLE", con);

        // Read from db
        OracleDataReader reader = cmd.ExecuteReader();
        Pizzas = new List<Pizza>();
        while (reader.Read())
        {
            if (reader.GetInt16(2) == 0)
            {
                Pizzas.Add(new Pizza { Id = reader.GetInt16(0), Name = reader.GetString(1), IsGlutenFree = false });
            }
            else
            {
                Pizzas.Add(new Pizza { Id = reader.GetInt16(0), Name = reader.GetString(1), IsGlutenFree = true });
            }
        }

        // Clean up
        reader.Dispose();
        cmd.Dispose();
        con.Dispose();

        return Pizzas;
    }

    public static Pizza? Get(int id)
    {
        return Pizzas.FirstOrDefault(p => p.Id == id);
    }

    public static void Add(String Name, Boolean IsGlutenFree)
    {
        // Open DB connection
        OracleConnection con;
        try
        {
            con = new OracleConnection(conStringUser);
        }
        catch (Exception ex)
        {
            System.Console.WriteLine(ex.Message);
            return;
        }
        con.Open();

        // Write SQL query
        OracleCommand cmd = new OracleCommand("INSERT INTO PIZZATABLE (NAME, ISGLUTENFREE) VALUES (:1, :2)", con);
        cmd.Parameters.Add(new OracleParameter("1", Name));
        if (IsGlutenFree)
        {
            cmd.Parameters.Add(new OracleParameter("2", OracleDbType.Int16, 1, ParameterDirection.Input));
        }
        else
        {
            cmd.Parameters.Add(new OracleParameter("2", OracleDbType.Int16, 0, ParameterDirection.Input));
        }

        // Read from db
        OracleDataReader reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            Console.WriteLine(reader);
        }

        // Clean up
        reader.Dispose();
        cmd.Dispose();
        con.Dispose();
    }

    public static void Delete(int id)
    {
        var pizza = Get(id);
        if (pizza is null)
        {
            return;
        }

        Pizzas.Remove(pizza);
    }

    public static void Update(Pizza pizza)
    {
        var index = Pizzas.FindIndex(p => p.Id == pizza.Id);
        if (index == -1)
        {
            return;
        }

        Pizzas[index] = pizza;
    }
}