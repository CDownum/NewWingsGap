var builder = DistributedApplication.CreateBuilder(args);

var cache = builder.AddRedis("cache");

var sql = builder.AddSqlServer("sql", port: 52105)
                 .WithDataVolume()
                 .WithLifetime(ContainerLifetime.Persistent);

var db = sql.AddDatabase("database");

var apiService = builder.AddProject<Projects.NewWingsGap_ApiService>("gapApi")
        .WithReference(db)
        .WaitFor(db);

builder.AddProject<Projects.NewWingsGap_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithReference(cache)
    .WaitFor(cache)
    .WithReference(apiService)
    .WaitFor(apiService);

builder.AddProject<Projects.NewWingsGap_MigrationService>("wingsgap-migrationservice")
    .WithReference(sql)
    .WaitFor(db);

builder.Build().Run();
