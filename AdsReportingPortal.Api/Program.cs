using AdsReportingPortal.Api.Service.Implementation;
using AdsReportingPortal.Api.Service.Interface;
using AdsReportingPortal.Data.Context;
using AdsReportingPortal.Data.Repository.Implementation;
using AdsReportingPortal.Data.Repository.Interface;
using AdsReportingPortal.Data.Seeders;
using AdsReportingPortal.Model.Entities;
using AdsReportingPortal.Service.Implementation;
using AdsReportingPortal.Service.Interface;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

using Quartz;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<ProjectDbContext>().AddDefaultTokenProviders();
builder.Services.AddDbContext<ProjectDbContext>(dbContextOptions => dbContextOptions.UseNpgsql(builder.Configuration["ConnectionStrings:ProdDb"]));
builder.Services.AddScoped<IAccountRepo, AccountRepo>();
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.AllowAnyMethod();
        builder.AllowAnyOrigin();
        builder.AllowAnyHeader();
    });

});

builder.Services.AddScoped(typeof(IAdsPortalRepo<>), typeof(AdsPortalRepo<>));
builder.Services.AddScoped<IEncryptionService, EncryptionService>();
builder.Services.AddScoped<ICampaignsService, CampaignsService>();
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IAdsStatService, AdsStatService>();
builder.Services.AddScoped<IGenerateJwt, GenerateJwt>();
builder.Services.AddScoped<IEmailServices, EmailService>();
builder.Services.AddScoped<IAccessTokenService, AccessTokenService>();
builder.Services.AddHostedService<AdsReportingPortal.Api.Service.Implementation.QuartzHostedService>();
builder.Services.AddQuartz(q =>
{
    // Use a scoped DI container to create jobs
    q.UseMicrosoftDependencyInjectionJobFactory();

    // Register the job with the scheduler
    var jobKey = new JobKey("MyJob");
    q.AddJob<JobService>(opts => opts.WithIdentity(jobKey));
    q.AddTrigger(opts => opts
        .ForJob(jobKey)
        .WithIdentity("MyJob-trigger")
        .WithSimpleSchedule(x => x
            .WithInterval(TimeSpan.FromHours(1))
            .RepeatForever()));
});

// Add Quartz.NET hosted service
builder.Services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);

var app = builder.Build();
Seeder.SeedData(app).Wait();
// Configure the HTTP request pipeline.
/*if (app.Environment.IsDevelopment())
{*/
    app.UseSwagger();
    app.UseSwaggerUI();
/*}*/
app.UseCors();
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
