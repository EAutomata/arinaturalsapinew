using System;
using System.Collections.Generic;
using AriNaturals.Entity;

namespace AriNaturals.Entity
{
    public class Customer
    {
        public Guid CustomerId { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? Phone { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // Navigation
        public ICollection<Address> Addresses { get; set; } = new List<Address>();
        public ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}
