using System;
using System.Net.Http.Headers;
using AWExternalActionAITagging.Controllers.Ximilar;
using Digizuite.AutomationWorkflows.Models;
using Digizuite.Extensions.Setup;
using Digizuite.Models;
using Digizuite.SDK.Extensions.Hosting;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<XimilarAIService>();
builder.Services.AddHttpClient("AI_HTTP_CLIENT", client =>
{
    client.DefaultRequestHeaders.Accept.Add(
        new MediaTypeWithQualityHeaderValue("application/json"));
});
// https://localhost:7130/api/aw/aiservices/ximilarfashiontagging
builder.Services.AddDigizuite(new DigizuiteConfiguration()
    {
        BaseUrl = new Uri("https://dev-dc.qa.digizuite.com/"),
        SystemUsername = "superadministrator",
        SystemPassword = "098f6bcd4621d373cade4e832627b4f6",
    })
    .UseConsoleLogger()
    .AutomaticallyDeclareCustomAutomationSteps()
    .DeclareAutomationStep(s =>
        s.WithName("Ximilar AI Fashion Tagging")
            .WithInvocationUri("https://879c-152-115-216-254.eu.ngrok.io/api/aw/aiservices/ximilarfashiontagging")
            .WithRuleType(CustomRuleType.Action)
            .WithDescription("Return AI tags from Ximilar Fashion Engine")
            .WithInputParameter(p => p.WithKey("asset_id")
                .WithType(CustomStepParameterType.String)
                .WithDescription("The asset id to use"))
            .WithOutputValue(p => p.WithKey("tagsstring")
                .WithType(CustomStepParameterType.String)
                .WithDescription("Output from Ximilar AI Fashion Tagging")
            ));

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
