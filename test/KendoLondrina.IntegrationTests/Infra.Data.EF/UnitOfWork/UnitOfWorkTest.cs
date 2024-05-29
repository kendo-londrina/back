using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;
using infraEF = KenLo.Infra.Data.EF;

namespace KenLo.IntegrationTests.Infra.Data.EF.UnitOfWork;

[Collection(nameof(UnitOfWorkFixture))]
public class UnitOfWorkTest
{
    private readonly UnitOfWorkFixture _fixture;

    public UnitOfWorkTest(UnitOfWorkFixture fixture) 
        => _fixture = fixture;

    [Fact(DisplayName = nameof(Commit))]
    public async Task Commit()
    {
        var dbId = "";
        // caso o paralelismo dos testes estejam conflitando, crie um
        // contexto separado passando um id aleatório: dbId = Guid.NewGuid().ToString();
        // ou configure o paralelismo em test/KendoLondrina.IntegrationTests/xunit.runner.json

        var dbContext = _fixture.CreateDbContext(dbId: dbId);
        var graduacoes = _fixture.GetGraduacaoList();
        await dbContext.AddRangeAsync(graduacoes);

        var unitOfWork = new infraEF.UnitOfWork(dbContext);

        await unitOfWork.Commit(CancellationToken.None);

        var assertDbContext = _fixture.CreateDbContext(true, dbId);
        var savedGraduacoes = assertDbContext.Graduacoes
            .AsNoTracking().ToList();
        savedGraduacoes.Should().HaveCount(graduacoes.Count);
    }

    [Fact(DisplayName = nameof(Rollback))]
    public async Task Rollback()
    {
        var dbContext = _fixture.CreateDbContext();
        var unitOfWork = new infraEF.UnitOfWork(dbContext);

        var task = async () => await unitOfWork.Rollback(CancellationToken.None);

        await task.Should().NotThrowAsync();
    }
}
