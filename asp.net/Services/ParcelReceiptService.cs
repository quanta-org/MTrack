using asp.net.Models;
using System;
using System.Data;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;

namespace asp.net.Services;

public static class ParcelReceiptService
{
    public static string user = Environment.GetEnvironmentVariable("DBUSER") ?? "";
    public static string pwd = Environment.GetEnvironmentVariable("DBUSERPASS") ?? "";
    public static string db = Environment.GetEnvironmentVariable("DB") ?? "";
    public static string conStringUser = "User Id=" + user + ";Password=" + pwd + ";Data Source=" + db + ";";
    static List<ParcelReceipt> Parcels { get; set; } = new List<ParcelReceipt>();

    public static List<ParcelReceipt>? GetAll()
    {
        // Open DB connection
        OracleConnection con;
        try
        {
            con = new OracleConnection(conStringUser);
            con.Open();
        }
        catch (Exception ex){
            System.Console.WriteLine("Could not connect to DB: " + ex.Message);
            throw new Exception("Database connection error.");
        }

        // Write SQL query
        OracleCommand cmd = new OracleCommand("SELECT id, INTIME, RECEIVER_ID, RECEIPT_LOCATION, CARRIER, TRACKING_NUMBER, ROUTING_LOCATION FROM ParcelReceipt", con);

        // Read from db
        OracleDataReader reader;
        try
        {
            reader = cmd.ExecuteReader();
        }
        catch (Exception ex)
        {
            System.Console.WriteLine("Could not write to DB: " + ex.Message);
            cmd.Dispose();
            con.Dispose();
            throw new Exception("Database error.");
        }

        Parcels = new List<ParcelReceipt>();
        while (reader.Read())
        {
            Parcels.Add(new ParcelReceipt {
                id = reader.GetInt16(0), 
                Time = reader.GetDateTime(1), 
                ReceiverID = reader.GetString(2), 
                ReceiptLocation = reader.GetString(3), 
                Carrier = reader.GetString(4), 
                TrackingNumber = reader.GetString(5), 
                RoutingLocation = reader.GetString(6) 
            });
        }

        Console.WriteLine("Successful read from db!");

        // Clean up
        reader.Dispose();
        cmd.Dispose();
        con.Dispose();

        return Parcels;
    }

    public static void Add(ParcelReceipt receipt)
    {
        // Open DB connection
        OracleConnection con;
        try
        {
            con = new OracleConnection(conStringUser);
            con.Open();
        }
        catch (Exception ex)
        {
            System.Console.WriteLine("Could not connect to DB: " + ex.Message);
            throw new Exception("Database connection error.");
        }

        // Write SQL query
        OracleCommand cmd = new OracleCommand("INSERT INTO ParcelReceipt (RECEIVER_ID, RECEIPT_LOCATION, CARRIER, TRACKING_NUMBER, ROUTING_LOCATION) VALUES (:1, :2, :3, :4, :5)", con);
        cmd.Parameters.Add(new OracleParameter("1", receipt.ReceiverID));
        cmd.Parameters.Add(new OracleParameter("2", receipt.ReceiptLocation));
        cmd.Parameters.Add(new OracleParameter("3", receipt.Carrier));
        cmd.Parameters.Add(new OracleParameter("4", receipt.TrackingNumber));
        cmd.Parameters.Add(new OracleParameter("5", receipt.RoutingLocation));

        // Write to db
        OracleDataReader reader;
        try
        {
            reader = cmd.ExecuteReader();
        }
        catch (Exception ex)
        {
            System.Console.WriteLine("Could not write to DB: " + ex.Message);
            cmd.Dispose();
            con.Dispose();
            throw new Exception("Database error.");
        }

        Console.WriteLine("Successful write to db!");

        // Clean up
        reader.Dispose();
        cmd.Dispose();
        con.Dispose();
    }
}