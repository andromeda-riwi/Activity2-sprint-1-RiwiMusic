// Models/Concert.cs
using System;
public class Concert
{
    public int ConcertId { get; set; }
    public string EventName { get; set; }
    public string City { get; set; }
    public DateTime Date { get; set; }
    public int Capacity { get; set; }
    public decimal Price { get; set; }
}