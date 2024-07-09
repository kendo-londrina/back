using System;
using Bogus;
using KenLo.Infra.Data.EF;
using Microsoft.EntityFrameworkCore;

namespace KenLo.EndToEndTests.Base;

public class BaseFixture
{
    public Faker Faker { get; set; }

    protected BaseFixture() 
        => Faker = new Faker("pt_BR");

    public bool GetRandomBoolean()
        => new Random().NextDouble() < 0.5;

    public KendoLondrinaDbContext CreateDbContext()
    {
        var context = new KendoLondrinaDbContext(
            new DbContextOptionsBuilder<KendoLondrinaDbContext>()
            .UseInMemoryDatabase($"end2end-tests-db")
            .Options
        );
        return context;
    }
}