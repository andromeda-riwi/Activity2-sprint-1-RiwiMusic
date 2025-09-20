// Models/TicketPurchase.cs
using System;
public class TicketPurchase
{
    public int PurchaseId { get; set; }
    public int ConcertId { get; set; }
    public int CustomerId { get; set; }
    public DateTime PurchaseDate { get; set; }
}