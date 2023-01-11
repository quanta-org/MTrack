namespace asp.net.Models;

public class ParcelReceipt {
    public int id { get; set; }

    public DateTime Time { get; set; }

    public string ReceiverID { get; set; } = String.Empty;

    public string ReceiptLocation { get; set; } = String.Empty;

    public string Carrier { get; set; } = String.Empty;

    public string TrackingNumber { get; set; } = String.Empty;

    public string RoutingLocation { get; set; } = String.Empty;
}