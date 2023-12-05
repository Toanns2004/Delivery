using Microsoft.EntityFrameworkCore;

namespace api.Context
{
    public class DBContext : DbContext
    {
        public DBContext(DbContextOptions options) : base(options)
        {
            
        }
    }
}



