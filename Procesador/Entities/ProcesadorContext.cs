using Microsoft.EntityFrameworkCore;

namespace Procesador.Entities
{
    public class ProcesadorContext : DbContext
    {
        public ProcesadorContext()
        {

        }

        public ProcesadorContext(DbContextOptions<ProcesadorContext> dbContextOptions) : base(dbContextOptions)
        {

        }

        public DbSet<Blob> Blobs { get; set; }

        public DbSet<Process> Processes { get; set; }
    }
}
