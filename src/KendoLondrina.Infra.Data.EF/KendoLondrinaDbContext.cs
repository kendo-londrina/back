using KenLo.Domain.Entity;
using Microsoft.EntityFrameworkCore;

namespace KenLo.Infra.Data.EF;
public class KendoLondrinaDbContext : DbContext
{
    public DbSet<Graduacao> Graduacoes => Set<Graduacao>();

    public KendoLondrinaDbContext(DbContextOptions<KendoLondrinaDbContext> options)
        : base(options)
    {}

    // protected override void OnModelCreating(ModelBuilder builder)
    // {
    //     builder.ApplyConfiguration(new GraduacaoConfiguration());
    // }

}
