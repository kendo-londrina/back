using System;
using Bogus;
using KenLo.Infra.Data.EF;
using Microsoft.EntityFrameworkCore;

namespace KenLo.IntegrationTests;

public class BaseFixture
{
    public Faker Faker { get; set; }

    protected BaseFixture() 
        => Faker = new Faker("pt_BR");

    public bool GetRandomBoolean()
        => new Random().NextDouble() < 0.5;

    public KendoLondrinaDbContext CreateDbContext(bool preserveData = false, string dbId = "")
    {
        var context = new KendoLondrinaDbContext(
            new DbContextOptionsBuilder<KendoLondrinaDbContext>()
            .UseInMemoryDatabase($"integration-tests-db{dbId}")
            .Options
        );
        if (preserveData == false)
            context.Database.EnsureDeleted();
        return context;
    }
}