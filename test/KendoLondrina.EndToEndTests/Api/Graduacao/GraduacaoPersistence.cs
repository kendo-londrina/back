using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using KenLo.Infra.Data.EF;
using Microsoft.EntityFrameworkCore;
using entity = KenLo.Domain.Entity;

namespace KenLo.EndToEndTests.Api.Graduacao;
public class GraduacaoPersistence
{
    private readonly KendoLondrinaDbContext _context;

    public GraduacaoPersistence(KendoLondrinaDbContext context)
        => _context = context;

    public async Task<entity.Graduacao?> GetById(Guid id)
        => await _context
            .Graduacoes.AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id);

    public async Task InsertList(List<entity.Graduacao> graduacoes)
    {
        await _context.Graduacoes.AddRangeAsync(graduacoes);
        await _context.SaveChangesAsync();
    }
}