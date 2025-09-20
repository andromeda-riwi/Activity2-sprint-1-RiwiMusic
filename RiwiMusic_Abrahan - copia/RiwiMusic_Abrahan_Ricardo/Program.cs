// Program.cs
// VERSIÓN FINAL Y COMPLETA
// Capa de Presentación (Interfaz de Usuario de la Consola).

using System;
using System.Linq;

// Si usas carpetas, necesitarás estas líneas:
// using TuProyecto.Models;
// using TuProyecto.Services;
// using TuProyecto.Data;

public class Program
{
    private static readonly MusicManagementService managementService = new MusicManagementService();

    public static void Main(string[] args)
    {
        Console.WriteLine("¡Bienvenido al Sistema de Gestión RiwiMusic!");
        bool exit = false;
        while (!exit)
        {
            ShowMainMenu();
            string option = Console.ReadLine();
            switch (option)
            {
                case "1": ConcertManagementUI(); break;
                case "2": CustomerManagementUI(); break;
                case "3": TicketManagementUI(); break;
                case "4": AdvancedQueriesUI(); break;
                case "5":
                    exit = true;
                    Console.WriteLine("\nGracias por usar RiwiMusic. ¡Hasta pronto!");
                    break;
                default:
                    Console.WriteLine("\n[ERROR] Opción no válida.");
                    break;
            }
        }
    }

    #region MENÚS PRINCIPALES Y DE GESTIÓN

    private static void ShowMainMenu()
    {
        Console.WriteLine("\n╔═══════════════════════════════════════╗");
        Console.WriteLine("║             MENÚ PRINCIPAL            ║");
        Console.WriteLine("╠═══════════════════════════════════════╣");
        Console.WriteLine("║ 1. Gestión de Conciertos              ║");
        Console.WriteLine("║ 2. Gestión de Clientes                ║");
        Console.WriteLine("║ 3. Gestión de Tiquetes (Compras)      ║");
        Console.WriteLine("║ 4. Consultas Avanzadas                ║");
        Console.WriteLine("║ 5. Salir                              ║");
        Console.WriteLine("╚═══════════════════════════════════════╝");
        Console.Write("Seleccione una opción: ");
    }

    private static void ConcertManagementUI()
    {
        Console.WriteLine("\n--- Gestión de Conciertos ---");
        Console.WriteLine("1. Registrar Concierto");
        Console.WriteLine("2. Listar Conciertos");
        Console.WriteLine("3. Editar Concierto");
        Console.WriteLine("4. Eliminar Concierto");
        Console.Write("Opción: ");
        string option = Console.ReadLine();
        
        switch (option)
        {
            case "1":
                try
                {
                    Console.WriteLine("\n--- Registrar Nuevo Concierto ---");
                    Console.Write("Nombre del Evento: ");
                    string eventName = Console.ReadLine();
                    Console.Write("Ciudad: ");
                    string city = Console.ReadLine();
                    Console.Write("Fecha (YYYY-MM-DD): ");
                    DateTime date = DateTime.Parse(Console.ReadLine());
                    Console.Write("Capacidad Total: ");
                    int capacity = int.Parse(Console.ReadLine());
                    Console.Write("Precio del Tiquete: ");
                    decimal price = decimal.Parse(Console.ReadLine());
                    managementService.AddConcert(eventName, city, date, capacity, price);
                    Console.WriteLine("\n[ÉXITO] Concierto registrado.");
                }
                catch (FormatException) { Console.WriteLine("\n[ERROR] Formato de datos incorrecto."); }
                break;
            case "2":
                ListAvailableConcerts();
                break;
            case "3":
                Console.WriteLine("\n--- Editar Concierto ---");
                if (!managementService.GetAllConcerts().Any()) { Console.WriteLine("[AVISO] No hay conciertos para editar."); break; }
                ListAvailableConcerts();
                Console.Write("Ingrese el ID del concierto a editar: ");
                if (int.TryParse(Console.ReadLine(), out int idToEdit))
                {
                    var concert = managementService.GetAllConcerts().FirstOrDefault(c => c.ConcertId == idToEdit);
                    if (concert == null) { Console.WriteLine("[ERROR] ID no encontrado."); break; }

                    try
                    {
                        Console.Write($"Nuevo nombre ({concert.EventName}): ");
                        string newName = Console.ReadLine();
                        Console.Write($"Nueva ciudad ({concert.City}): ");
                        string newCity = Console.ReadLine();
                        Console.Write($"Nueva fecha ({concert.Date.ToShortDateString()}): ");
                        string newDateStr = Console.ReadLine();
                        Console.Write($"Nueva capacidad ({concert.Capacity}): ");
                        string newCapacityStr = Console.ReadLine();
                        Console.Write($"Nuevo precio ({concert.Price}): ");
                        string newPriceStr = Console.ReadLine();

                        // Actualizar solo si el usuario ingresó un nuevo valor
                        string finalName = string.IsNullOrWhiteSpace(newName) ? concert.EventName : newName;
                        string finalCity = string.IsNullOrWhiteSpace(newCity) ? concert.City : newCity;
                        DateTime finalDate = string.IsNullOrWhiteSpace(newDateStr) ? concert.Date : DateTime.Parse(newDateStr);
                        int finalCapacity = string.IsNullOrWhiteSpace(newCapacityStr) ? concert.Capacity : int.Parse(newCapacityStr);
                        decimal finalPrice = string.IsNullOrWhiteSpace(newPriceStr) ? concert.Price : decimal.Parse(newPriceStr);

                        if (managementService.UpdateConcert(idToEdit, finalName, finalCity, finalDate, finalCapacity, finalPrice))
                            Console.WriteLine("\n[ÉXITO] Concierto actualizado.");
                        else
                            Console.WriteLine("\n[ERROR] No se pudo actualizar el concierto.");
                    }
                    catch (FormatException) { Console.WriteLine("\n[ERROR] Formato de datos incorrecto."); }
                }
                else { Console.WriteLine("[ERROR] ID no válido."); }
                break;
            case "4":
                Console.WriteLine("\n--- Eliminar Concierto ---");
                if (!managementService.GetAllConcerts().Any()) { Console.WriteLine("[AVISO] No hay conciertos para eliminar."); break; }
                ListAvailableConcerts();
                Console.Write("Ingrese el ID del concierto a eliminar: ");
                if (int.TryParse(Console.ReadLine(), out int idToDelete))
                {
                    if (managementService.DeleteConcert(idToDelete)) Console.WriteLine("[ÉXITO] Concierto eliminado.");
                    else Console.WriteLine("[ERROR] No se pudo eliminar (ID no encontrado).");
                }
                else { Console.WriteLine("[ERROR] ID no válido."); }
                break;
            default: Console.WriteLine("[ERROR] Opción no válida."); break;
        }
    }

    private static void CustomerManagementUI()
    {
        Console.WriteLine("\n--- Gestión de Clientes ---");
        Console.WriteLine("1. Registrar Cliente");
        Console.WriteLine("2. Listar Clientes");
        Console.WriteLine("3. Editar Cliente");
        Console.WriteLine("4. Eliminar Cliente");
        Console.Write("Opción: ");
        string option = Console.ReadLine();

        switch (option)
        {
            case "1":
                Console.WriteLine("\n--- Registrar Nuevo Cliente ---");
                Console.Write("Nombre completo: ");
                string name = Console.ReadLine();
                Console.Write("Email: ");
                string email = Console.ReadLine();
                managementService.AddCustomer(name, email);
                Console.WriteLine("\n[ÉXITO] Cliente registrado.");
                break;
            case "2":
                ListAvailableCustomers();
                break;
            case "3":
                Console.WriteLine("\n--- Editar Cliente ---");
                if (!managementService.GetAllCustomers().Any()) { Console.WriteLine("[AVISO] No hay clientes para editar."); break; }
                ListAvailableCustomers();
                Console.Write("Ingrese el ID del cliente a editar: ");
                if (int.TryParse(Console.ReadLine(), out int idToEdit))
                {
                    var customer = managementService.GetAllCustomers().FirstOrDefault(c => c.CustomerId == idToEdit);
                    if (customer == null) { Console.WriteLine("[ERROR] ID no encontrado."); break; }

                    Console.Write($"Nuevo nombre ({customer.Name}): ");
                    string newName = Console.ReadLine();
                    Console.Write($"Nuevo email ({customer.Email}): ");
                    string newEmail = Console.ReadLine();

                    string finalName = string.IsNullOrWhiteSpace(newName) ? customer.Name : newName;
                    string finalEmail = string.IsNullOrWhiteSpace(newEmail) ? customer.Email : newEmail;

                    if (managementService.UpdateCustomer(idToEdit, finalName, finalEmail))
                        Console.WriteLine("\n[ÉXITO] Cliente actualizado.");
                    else
                        Console.WriteLine("\n[ERROR] No se pudo actualizar el cliente.");
                }
                else { Console.WriteLine("[ERROR] ID no válido."); }
                break;
            case "4":
                Console.WriteLine("\n--- Eliminar Cliente ---");
                if (!managementService.GetAllCustomers().Any()) { Console.WriteLine("[AVISO] No hay clientes para eliminar."); break; }
                ListAvailableCustomers();
                Console.Write("Ingrese el ID del cliente a eliminar: ");
                if (int.TryParse(Console.ReadLine(), out int idToDelete))
                {
                    if (managementService.DeleteCustomer(idToDelete)) Console.WriteLine("[ÉXITO] Cliente eliminado.");
                    else Console.WriteLine("[ERROR] No se pudo eliminar (ID no encontrado).");
                }
                else { Console.WriteLine("[ERROR] ID no válido."); }
                break;
            default: Console.WriteLine("[ERROR] Opción no válida."); break;
        }
    }

    private static void TicketManagementUI()
    {
        Console.WriteLine("\n--- Gestión de Tiquetes ---");
        Console.WriteLine("1. Registrar Compra");
        Console.WriteLine("2. Listar Tiquetes Vendidos");
        Console.WriteLine("3. Eliminar Compra");
        Console.Write("Opción: ");
        string option = Console.ReadLine();

        switch (option)
        {
            case "1":
                Console.WriteLine("\n--- Registrar Nueva Compra de Tiquete ---");
                if (!managementService.GetAllCustomers().Any() || !managementService.GetAllConcerts().Any())
                {
                    Console.WriteLine("\n[AVISO] Debe registrar al menos un cliente y un concierto.");
                    return;
                }
                Console.WriteLine("\nPASO 1: Seleccione el cliente");
                ListAvailableCustomers();
                Console.Write("Ingrese el ID del Cliente: ");
                if (!int.TryParse(Console.ReadLine(), out int customerId)) { Console.WriteLine("[ERROR] ID no válido."); return; }
                Console.WriteLine("\nPASO 2: Seleccione el concierto");
                ListAvailableConcerts();
                Console.Write("Ingrese el ID del Concierto: ");
                if (!int.TryParse(Console.ReadLine(), out int concertId)) { Console.WriteLine("[ERROR] ID no válido."); return; }
                string result = managementService.RegisterPurchase(customerId, concertId);
                Console.WriteLine($"\n>> RESULTADO: {result}");
                break;
            case "2":
                Console.WriteLine("\n--- Tiquetes Vendidos ---");
                var allPurchases = managementService.GetAllPurchases();
                if (!allPurchases.Any()) { Console.WriteLine("No se han vendido tiquetes."); return; }
                foreach (var p in allPurchases)
                {
                    var customer = managementService.GetAllCustomers().FirstOrDefault(c => c.CustomerId == p.CustomerId);
                    var concert = managementService.GetAllConcerts().FirstOrDefault(c => c.ConcertId == p.ConcertId);
                    Console.WriteLine($"ID: {p.PurchaseId} | Cliente: {customer?.Name ?? "N/A"} | Concierto: {concert?.EventName ?? "N/A"}");
                }
                break;
            case "3":
                Console.WriteLine("\n--- Eliminar Compra ---");
                if (!managementService.GetAllPurchases().Any()) { Console.WriteLine("[AVISO] No hay compras para eliminar."); break; }
                // Sería ideal listar las compras aquí también...
                Console.Write("ID de la compra a eliminar: ");
                if (int.TryParse(Console.ReadLine(), out int idToDelete))
                {
                    if(managementService.DeletePurchase(idToDelete)) Console.WriteLine("[ÉXITO] Compra eliminada.");
                    else Console.WriteLine("[ERROR] No se pudo eliminar la compra (ID no encontrado).");
                } else { Console.WriteLine("[ERROR] ID no válido.");}
                break;
            default: Console.WriteLine("[ERROR] Opción no válida."); break;
        }
    }
    
    #endregion

    #region MÉTODOS DE AYUDA (UI HELPERS)
    private static void ListAvailableCustomers()
    {
        Console.WriteLine("---------------------------------");
        var customers = managementService.GetAllCustomers();
        if (!customers.Any()) Console.WriteLine("No hay clientes registrados.");
        else foreach (var c in customers) Console.WriteLine($"ID: {c.CustomerId} | Nombre: {c.Name}");
        Console.WriteLine("---------------------------------");
    }

    private static void ListAvailableConcerts()
    {
        Console.WriteLine("---------------------------------");
        var concerts = managementService.GetAllConcerts();
        if (!concerts.Any()) Console.WriteLine("No hay conciertos registrados.");
        else foreach (var c in concerts) Console.WriteLine($"ID: {c.ConcertId} | Evento: {c.EventName}");
        Console.WriteLine("---------------------------------");
    }
    #endregion

    #region CONSULTAS AVANZADAS (Completas)
    private static void AdvancedQueriesUI()
    {
        Console.WriteLine("\n--- Consultas Avanzadas ---");
        Console.WriteLine("1. Historial de compras de un usuario");
        Console.WriteLine("2. Consultar conciertos por ciudad");
        Console.WriteLine("3. Consultar conciertos por rango de fechas");
        Console.WriteLine("4. Consultar ingresos totales de un concierto");
        Console.WriteLine("5. Reporte: Concierto más vendido");
        Console.WriteLine("6. Reporte: Cliente con más compras");
        Console.Write("Opción: ");
        string option = Console.ReadLine();
        
        switch(option)
        {
            case "1":
                if (!managementService.GetAllCustomers().Any()) { Console.WriteLine("\n[AVISO] No hay clientes para consultar."); break; }
                Console.WriteLine("\nSeleccione un cliente:");
                ListAvailableCustomers();
                Console.Write("Ingrese el ID del cliente: ");
                if (int.TryParse(Console.ReadLine(), out int customerId))
                {
                    var customer = managementService.GetAllCustomers().FirstOrDefault(c => c.CustomerId == customerId);
                    if (customer == null) { Console.WriteLine("[ERROR] ID de cliente no encontrado."); break; }
                    var purchases = managementService.GetPurchasesByCustomer(customerId);
                    Console.WriteLine($"\n--- Historial de compras de {customer.Name} ---");
                    if (!purchases.Any()) Console.WriteLine("Este usuario no ha realizado compras.");
                    else foreach (var p in purchases)
                    {
                        var concert = managementService.GetAllConcerts().First(c => c.ConcertId == p.ConcertId);
                        Console.WriteLine($"- Compra para '{concert.EventName}' el {p.PurchaseDate.ToShortDateString()}");
                    }
                }
                else { Console.WriteLine("[ERROR] ID no válido."); }
                break;
            case "2":
                Console.Write("\nIngrese la ciudad: ");
                string city = Console.ReadLine();
                var concertsByCity = managementService.GetConcertsByCity(city);
                Console.WriteLine($"\n--- Conciertos en {city} ---");
                if (!concertsByCity.Any()) Console.WriteLine("No se encontraron conciertos.");
                else foreach (var c in concertsByCity) Console.WriteLine($"- {c.EventName} el {c.Date.ToShortDateString()}");
                break;
            case "3":
                try
                {
                    Console.Write("Ingrese la fecha de inicio (YYYY-MM-DD): ");
                    DateTime startDate = DateTime.Parse(Console.ReadLine());
                    Console.Write("Ingrese la fecha de fin (YYYY-MM-DD): ");
                    DateTime endDate = DateTime.Parse(Console.ReadLine());
                    var concertsInRange = managementService.GetConcertsByDateRange(startDate, endDate);
                    Console.WriteLine($"\n--- Conciertos entre {startDate.ToShortDateString()} y {endDate.ToShortDateString()} ---");
                    if (!concertsInRange.Any()) Console.WriteLine("No se encontraron conciertos.");
                    else foreach (var c in concertsInRange) Console.WriteLine($"- {c.EventName} el {c.Date.ToShortDateString()}");
                }
                catch (FormatException) { Console.WriteLine("[ERROR] Formato de fecha no válido."); }
                break;
            case "4":
                if (!managementService.GetAllConcerts().Any()) { Console.WriteLine("\n[AVISO] No hay conciertos para consultar."); break; }
                Console.WriteLine("\nSeleccione un concierto:");
                ListAvailableConcerts();
                Console.Write("Ingrese el ID del concierto: ");
                if (int.TryParse(Console.ReadLine(), out int concertId))
                {
                    var concertInfo = managementService.GetAllConcerts().FirstOrDefault(c => c.ConcertId == concertId);
                    if(concertInfo != null)
                    {
                        decimal revenue = managementService.GetTotalRevenueForConcert(concertId);
                        Console.WriteLine($"\nLos ingresos para '{concertInfo.EventName}' son: ${revenue}");
                    }
                    else { Console.WriteLine($"\n[ERROR] ID de concierto no encontrado."); }
                }
                else { Console.WriteLine("[ERROR] ID no válido."); }
                break;
            case "5":
                var bestSeller = managementService.GetBestSellingConcert();
                if(bestSeller != null) Console.WriteLine($"\n>> Reporte: El concierto más vendido es '{bestSeller.EventName}'");
                else Console.WriteLine("\nNo hay datos de ventas para este reporte.");
                break;
            case "6":
                var topCustomer = managementService.GetTopCustomer();
                if(topCustomer != null) Console.WriteLine($"\n>> Reporte: El cliente con más compras es '{topCustomer.Name}'");
                else Console.WriteLine("\nNo hay datos de ventas para este reporte.");
                break;
            default:
                Console.WriteLine("[ERROR] Opción no válida.");
                break;
        }
    }
    #endregion
}