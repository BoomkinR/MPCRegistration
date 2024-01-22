using MpcRen.Register.Client;
using MpcRen.Register.Client.Network;
using MpcRen.Register.Infrastructure.Extensions;
using MpcRen.Register.Infrastructure.Sharing;

// var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

// builder.Services.AddSwaggerGen();


HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSecretSharingServices();
builder.Services.AddHostedService<TestService>();
builder.Services.AddScoped<IHttpRenRegClient, HttpRenRegClient>();
using IHost host = builder.Build();

await host.RunAsync();


class TestService : IHostedService
{
    private readonly ISecretShareService _secretShareService;
    private readonly IHttpRenRegClient _httpRenRegClient;

    public TestService(ISecretShareService secretShareService, IHttpRenRegClient httpRenRegClient)
    {
        this._secretShareService = secretShareService;
        _httpRenRegClient = httpRenRegClient;
    }


    public Task StartAsync(CancellationToken cancellationToken)
    {
        Console.WriteLine("=======IT IS CLIENT=======");
        Console.Write("Lets register you ...\n Login: ");
        var login = Console.ReadLine();
        Console.Write("Password: ");
        var pass = Console.ReadLine();
        Console.WriteLine();

        var (x1, x2, x3, x4) = _secretShareService.GenerateShares(pass!, 11441180254372124519);

        var shares = new[] { (x2, x3, x4), (x1, x3, x4), (x1, x2, x4), (x1, x2, x3) };
        Console.WriteLine("Waiting for server connecting ...");
        Task.Delay(TimeSpan.FromSeconds(5)).GetAwaiter().GetResult();
        Console.WriteLine("Server is not connected. Shut down ...");
        for (int i = 0; i < 4; i++)
        {
            _httpRenRegClient.SendMessageToServer(login!, shares[i], i);
        }

        

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}