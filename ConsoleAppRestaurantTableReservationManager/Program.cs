using System;
using System.Collections.Generic;
using System.IO;

// Main Application Class
public class ReservationApp
{
    static void Main(string[] args)
    {
        ReservationManager reservationManager = new ReservationManager();
        reservationManager.AddVenue("A", 10);
        reservationManager.AddVenue("B", 5);

        Console.WriteLine(reservationManager.BookTable("A", new DateTime(2023, 12, 25), 3)); // True
        Console.WriteLine(reservationManager.BookTable("A", new DateTime(2023, 12, 25), 3)); // False
    }
}

// Reservation Manager Class
public class ReservationManager
{
    private List<Venue> venues;

    public ReservationManager()
    {
        venues = new List<Venue>();
    }

    // Add Venue Method
    public void AddVenue(string name, int tableCount)
    {
        try
        {
            Venue venue = new Venue();
            venue.Name = name;
            venue.Tables = new VenueTable[tableCount];
            for (int i = 0; i < tableCount; i++)
            {
                venue.Tables[i] = new VenueTable();
            }
            venues.Add(venue);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error");
        }
    }

    // Load Venues From File
    private void LoadVenuesFromFile(string filePath)
    {
        try
        {
            string[] lines = File.ReadAllLines(filePath);
            foreach (string line in lines)
            {
                var parts = line.Split(',');
                if (parts.Length == 2 && int.TryParse(parts[1], out int tableCount))
                {
                    AddVenue(parts[0], tableCount);
                }
                else
                {
                    Console.WriteLine(line);
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error");
        }
    }

    // Find All Free Tables
    public List<string> FindAllFreeTables(DateTime date)
    {
        try
        {
            List<string> freeTables = new List<string>();
            foreach (var venue in venues)
            {
                for (int i = 0; i < venue.Tables.Length; i++)
                {
                    if (!venue.Tables[i].IsBooked(date))
                    {
                        freeTables.Add($"{venue.Name} - Table {i + 1}");
                    }
                }
            }
            return freeTables;
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error");
            return new List<string>();
        }
    }

    public bool BookTable(string venueName, DateTime date, int tableNumber)
    {
        foreach (var venue in venues)
        {
            if (venue.Name == venueName)
            {
                if (tableNumber < 0 || tableNumber >= venue.Tables.Length)
                {
                    throw new Exception("Invalid table number");
                }

                return venue.Tables[tableNumber].Book(date);
            }
        }

        throw new Exception("Venue not found");
    }

    public void SortVenuesByAvailabilityForUsers(DateTime date)
    {
        try
        {
            bool swapped;
            do
            {
                swapped = false;
                for (int i = 0; i < venues.Count - 1; i++)
                {
                    int avTc = CountAvailableTablesForVenue(venues[i], date);
                    int avTn = CountAvailableTablesForVenue(venues[i + 1], date);

                    if (avTc < avTn)
                    {
                        // Swap venues
                        var temp = venues[i];
                        venues[i] = venues[i + 1];
                        venues[i + 1] = temp;
                        swapped = true;
                    }
                }
            } while (swapped);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error");
        }
    }

    // Count available tables in a venue
    public int CountAvailableTablesForVenue(Venue venue, DateTime date)
    {
        try
        {
            int count = 0;
            foreach (var table in venue.Tables)
            {
                if (!table.IsBooked(date))
                {
                    count++;
                }
            }
            return count;
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error");
            return 0;
        }
    }
}

// Venue Class
public class Venue
{
    public string Name { get; set; } // Name
    public VenueTable[] Tables { get; set; } // Tables
}

// Table Class
public class VenueTable
{
    private List<DateTime> bookedDates; // Booked dates

    public VenueTable()
    {
        bookedDates = new List<DateTime>();
    }

    // Book
    public bool Book(DateTime date)
    {
        try
        {
            if (bookedDates.Contains(date))
            {
                return false;
            }
            // Add to booked dates
            bookedDates.Add(date);
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error");
            return false;
        }
    }

    // Is booked
    public bool IsBooked(DateTime date)
    {
        return bookedDates.Contains(date);
    }
}
