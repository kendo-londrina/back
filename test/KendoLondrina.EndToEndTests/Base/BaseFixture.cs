using System;
using System.Net.Http;
using Bogus;
using KenLo.Infra.Data.EF;
using Microsoft.EntityFrameworkCore;

namespace KenLo.EndToEndTests.Base;

public class BaseFixture
{
    protected Faker Faker { get; set; }
    public CustomWebApplicationFactory<Program> WebAppFactory { get; set; }
    public HttpClient HttpClient { get; set; }
    public ApiClient ApiClient { get; set; }
    protected BaseFixture()
    {
        Faker = new Faker("pt_BR");
        WebAppFactory = new CustomWebApplicationFactory<Program>();
        HttpClient = WebAppFactory.CreateClient();
        ApiClient = new ApiClient(HttpClient);
    }

    public static bool GetRandomBoolean()
        => new Random().NextDouble() < 0.5;

    public static KendoLondrinaDbContext CreateDbContext()
    {
        var context = new KendoLondrinaDbContext(
            new DbContextOptionsBuilder<KendoLondrinaDbContext>()
            .UseInMemoryDatabase($"end2end-tests-db")
            .Options
        );
        return context;
    }
}