using System;
using TableReservation;
using Xunit;

namespace xUnixProj
{
    public class Tests
    {
        ReservationManager manager = new();

        [Fact]
        public void IsResultCorrect()
        {
            manager.AddRestaurant("A", 10);

            Assert.True(manager.BookTable("A", new DateTime(2023, 12, 25), 3));
            Assert.False(manager.BookTable("A", new DateTime(2023, 12, 25), 3));
        }

        [Fact]
        public void IsAdded()
        {
            string name = "A";
            int table = 10;
            manager.AddRestaurant(name, table);

            Assert.NotEmpty(manager.res);
            Assert.Equal(name, manager.res[0].name);
            Assert.Equal(table, manager.res[0].table.Length);
        }

        [Fact]
        public void EmptyNameEx() 
        {
            Assert.Throws<ArgumentException>(() => manager.AddRestaurant("", 10));
        }

        [Fact]
        public void InvalidTableNumberEx()
        {
            manager.AddRestaurant("A", 0);
            manager.AddRestaurant("B", 2);
            manager.AddRestaurant("C", 5);

            Assert.Throws<Exception>(() => manager.BookTable("A", new DateTime(2023, 12, 25), 3));
            Assert.Throws<Exception>(() => manager.BookTable("B", new DateTime(2023, 12, 25), 3));
            Assert.Throws<Exception>(() => manager.BookTable("C", new DateTime(2023, 12, 25), -1));
        }

        [Fact]
        public void ResNotFoundEx()
        {
            manager.AddRestaurant("A", 5);

            Assert.Throws<Exception>(() => manager.BookTable("B", new DateTime(2023, 12, 25), 3));
        }
    }
    
}

