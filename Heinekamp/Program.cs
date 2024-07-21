using Heinekamp;
using Microsoft.AspNetCore;

var host = WebHost.CreateDefaultBuilder(args)
    .UseStartup<Startup>()
    .Build();

host.Run();