var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.CleanArchitectureTemplate_WebAPI>("CleanArchitectureTemplate-webapi");

builder.Build().Run();
