using api.Entities;
using Microsoft.EntityFrameworkCore;

namespace api.Context
{
    public class DBContext : DbContext
    {
        public DBContext(DbContextOptions options) : base(options)
        {
            
        }
        
        public DbSet<User> Users { get; set; }
        
        public DbSet<Province> Provinces { get; set; }
        
        public DbSet<District> Districts { get; set; }
        
        public DbSet<Ward> Wards { get; set; }
        
        public DbSet<PostOffice> PostOffices { get; set; }
        
        public DbSet<DeliveryAddress> ConsigneeAddresses { get; set; }
        
        public DbSet<ShippingAddress> ShipperAddresses { get; set; }
        
        public DbSet<Role> Roles { get; set; }
        
        public DbSet<Permission> Permissions { get; set; }
        
        public DbSet<Employee> Employees { get; set; }
        
        public DbSet<Bill> Bills { get; set; }
        
        public DbSet<BillDetail> BillDetails { get; set; }
        
        public DbSet<UnitPrice> UnitPrices { get; set; }
        
        public DbSet<Status> Status { get; set; }
    }
}



