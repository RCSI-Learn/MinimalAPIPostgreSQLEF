using Microsoft.EntityFrameworkCore;
using MinimalAPIPostgreSQLEF.Models;

namespace MinimalAPIPostgreSQLEF.Data {
    public class OfficeDB : DbContext {
        public OfficeDB(DbContextOptions<OfficeDB> options) : base(options) {
        }
        public DbSet<Employee> Employees => Set<Employee>();
    }
}
