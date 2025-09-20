// Data/DataStore.cs
using System.Collections.Generic;
public static class DataStore
{
    public static List<Customer> Customers { get; set; } = new List<Customer>();
    public static List<Concert> Concerts { get; set; } = new List<Concert>();
    public static List<TicketPurchase> Purchases { get; set; } = new List<TicketPurchase>();

    public static int NextCustomerId { get; set; } = 1;
    public static int NextConcertId { get; set; } = 1;
    public static int NextPurchaseId { get; set; } = 1;
}