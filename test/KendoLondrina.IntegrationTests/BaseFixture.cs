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

    public KendoLondrinaDbContext CreateDbContext(bool preserveData = false)
    {
        var context = new KendoLondrinaDbContext(
            new DbContextOptionsBuilder<KendoLondrinaDbContext>()
            .UseInMemoryDatabase("integration-tests-db")
            .Options
        );
        if (preserveData == false)
            context.Database.EnsureDeleted();
        return context;
    }
}