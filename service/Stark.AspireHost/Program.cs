var builder = DistributedApplication.CreateBuilder(args);

var apiService = builder.AddProject<Projects.Stark_Admin>("starkAdmin");


builder.Build().Run();