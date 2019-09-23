using Microsoft.EntityFrameworkCore;

namespace MinhaPrimeiraApi.Model
{
    public class ApiDbContext : DbContext
    {
        public ApiDbContext(DbContextOptions options) : base(options) { }

        public DbSet<Fornecedor> Fornecedores { get; set; }
    }
}