// using KenLo.Application;
// using FC.Codeflix.Catalog.Application.EventHandlers;
using KenLo.Application.Interfaces;
using KenLo.Application.UseCases.Graduacao;
// using FC.Codeflix.Catalog.Domain.Events;
using KenLo.Domain.Repository;
// using KenLo.Domain.SeedWork;
using KenLo.Infra.Data.EF;
using KenLo.Infra.Data.EF.Repositories;
// using FC.Codeflix.Catalog.Infra.Messaging.Configuration;
// using FC.Codeflix.Catalog.Infra.Messaging.Producer;
// using MediatR;
// using Microsoft.Extensions.Options;
// using RabbitMQ.Client;

namespace KenLo.Api.Configurations;

public static class UseCasesConfiguration
{
    public static IServiceCollection AddUseCases(
        this IServiceCollection services
    )
    {
        // services.AddMediatR(typeof(CreateGraduacao));
        services.AddRepositories();
        services.AddUsecases();
        // services.AddDomainEvents();
        return services;
    }

    private static IServiceCollection AddUsecases(this IServiceCollection services)
    {
        services.AddScoped<ICreateGraduacao, CreateGraduacao>();
        services.AddScoped<IReadGraduacao, ReadGraduacao>();
        services.AddScoped<IDeleteGraduacao, DeleteGraduacao>();
        services.AddScoped<IUpdateGraduacao, UpdateGraduacao>();
        return services;
    }

    private static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddTransient<IGraduacaoRepository, GraduacaoRepository>();
        // services.AddTransient<IGenreRepository, GenreRepository>();
        // services.AddTransient<ICastMemberRepository, CastMemberRepository>();
        // services.AddTransient<IVideoRepository, VideoRepository>();
        services.AddTransient<IUnitOfWork, UnitOfWork>();
        return services;
    }

    // private static IServiceCollection AddDomainEvents(
    //     this IServiceCollection services)
    // {
    //     services.AddTransient<IDomainEventPublisher, DomainEventPublisher>();
    //     services.AddTransient<IDomainEventHandler<VideoUploadedEvent>,
    //         SendToEncoderEventHandler>();

    //     return services;
    // }


}