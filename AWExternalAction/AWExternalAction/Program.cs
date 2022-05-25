using Digizuite.AutomationWorkflows.Models;
using Digizuite.Extensions.Setup;
using Digizuite.Models;
using Digizuite.SDK.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDigizuite(new DigizuiteConfiguration()
{
    BaseUrl = new Uri("{*My-DAM-URL*}"),
    SystemUsername = "superadministrator",
    SystemPassword = "test",
})
    .UseConsoleLogger()
    .AutomaticallyDeclareCustomAutomationSteps()
    .DeclareAutomationStep(s => s.WithName("Is Lucky")
        .WithInvocationUri("https://{*My-API-Application-Domain*}/api/aw/my-wrapper/is-lucky")
        .WithRuleType(CustomRuleType.Filter)
        .WithDescription("Check if a number is lucky")
        .WithInputParameter(p => p.WithKey("number")
            .WithType(CustomStepParameterType.Int)
            .WithDescription("The number to check")));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
