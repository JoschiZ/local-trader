
using LocalTrader.Shared.Aspire;

var builder = DistributedApplication.CreateBuilder(args);

var postgres = builder
    .AddPostgres(Services.PostgresServer)
    .WithDataVolume()
    .WithLifetime(ContainerLifetime.Persistent);

var db = postgres
    .AddDatabase(Services.LocalTraderDb);

var app = builder
    .AddProject<Projects.LocalTrader>(Services.LocalTraderApp)
    .WaitFor(postgres)
    .WithReference(db);

builder.Build().Run();
