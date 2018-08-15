using DineConnect.DomainObject;
using System.Data.Entity;

namespace DineConnect.DataAccess
{
    public class TicketDataContext : DbContext
    {
        public TicketDataContext() : base("TicketDataContext")
        {
        }

        public DbSet<Ticket> Tickets { get; set; }

        public DbSet<Transaction> Transactions { get; set; }

        public DbSet<Payment> Payments { get; set; }

        public DbSet<Order> Orders { get; set; }        
    }
}
