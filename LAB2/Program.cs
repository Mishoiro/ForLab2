using System;
using System.Collections.Generic;

namespace TableReservation
{
    public class Restaurant
    {
        public string name;
        public RestaurantTable[] table;
    }

    public class ReservationManager
    {
        public List<Restaurant> res;

        public ReservationManager()
        {
            res = new List<Restaurant>();
        }

        public void AddRestaurant(string name, int table)
        {
            if (name == string.Empty)
            {
                throw new ArgumentException("Error - empty name");
            }

            try
            {
                Restaurant restoraunt = new()
                {
                    name = name,
                    table = new RestaurantTable[table]
                };

                for (int i = 0; i < table; i++)
                {
                    restoraunt.table[i] = new RestaurantTable();
                }
                res.Add(restoraunt);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: ", ex);
            }
        }

        public bool BookTable(string restorauntName, DateTime date, int tableNumber)
        {
            foreach (var restoraunt in res)
            {
                if (restoraunt.name == restorauntName)
                {
                    if (tableNumber < 0 || tableNumber >= restoraunt.table.Length)
                    {
                        throw new Exception(null); //Invalid table number
                    }

                    return restoraunt.table[tableNumber].Book(date);
                }
            }

            throw new Exception(null); //Restaurant not found
        }
    }

    public class TablesManager : ReservationManager
    {
        public List<string> FindAllFreeTables(DateTime dateTime)
        {
            try
            {
                List<string> free = new();
                foreach (var restoraunt in res)
                {
                    for (int i = 0; i < restoraunt.table.Length; i++)
                    {
                        if (!restoraunt.table[i].IsBooked(dateTime))
                        {
                            free.Add($"{restoraunt.name} - Table {i + 1}");
                        }
                    }
                }
                return free;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: ", ex);
                return new List<string>();
            }
        }


        public void SortByAvailability(DateTime dateTime)
        {
            try
            {
                bool swapped;
                do
                {
                    swapped = false;
                    for (int i = 0; i < res.Count - 1; i++)
                    {
                        int currentAvialableTables = CountAvailableTables(res[i], dateTime);
                        int nextAvailableTables = CountAvailableTables(res[i + 1], dateTime);

                        if (currentAvialableTables < nextAvailableTables)
                        {
                            // Swap restaurants
                            var temp = res[i];
                            res[i] = res[i + 1];
                            res[i + 1] = temp;
                            swapped = true;
                        }
                    }
                } while (swapped);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: ", ex);
            }
        }

        public int CountAvailableTables(Restaurant restaurant, DateTime dateTime)
        {
            try
            {
                int count = 0;
                foreach (var table in restaurant.table)
                {
                    if (!table.IsBooked(dateTime))
                    {
                        count++;
                    }
                }
                return count;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: ", ex);
                return 0;
            }
        }
    }



    public class RestaurantTable
    {
        private List<DateTime> bookedDates;

        public RestaurantTable()
        {
            bookedDates = new List<DateTime>();
        }

        public bool IsBooked(DateTime date)
        {
            return bookedDates.Contains(date);
        }

        public bool Book(DateTime date)
        {
            try
            {
                if (IsBooked(date))
                {
                    return false;
                }

                bookedDates.Add(date);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: ", ex);
                return false;
            }
        }

    }
    public class TableReservationApp
    {
        static void Main(string[] args)
        {
            ReservationManager manager = new();
            manager.AddRestaurant("A", 10);
            manager.AddRestaurant("B", 5);

            bool result1 = manager.BookTable("A", new DateTime(2023, 12, 25), 3);
            bool result2 = manager.BookTable("A", new DateTime(2023, 12, 25), 3);

            Console.WriteLine(result1);
            Console.WriteLine(result2);
        }
    }
}