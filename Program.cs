using MediaTrackerYoutubeService.Data;
using MediaTrackerYoutubeService.GraphQL;
using MediaTrackerYoutubeService.Services.AuthTokenExchangeService;
using MediaTrackerYoutubeService.Services.FetchYoutubeDataService;
using MediaTrackerYoutubeService.Services.ProcessYoutubeDataService;
using MediaTrackerYoutubeService.Services.StoreYoutubeDataService;
using MediaTrackerYoutubeService.Services.UserVideoService;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// builder.Services.AddCors();

// Add services to the container.
builder.Services.AddPooledDbContextFactory<AppDbContext>(
    options => options.UseSqlServer(builder.Configuration.GetConnectionString("DBConnectionString"))
);

builder.Services
    .AddGraphQLServer()
    .ModifyRequestOptions(opt => opt.IncludeExceptionDetails = true)
    // .AddQueryType<Query>()
    // .AddMutationType<Mutation>()
    // .AddSubscriptionType<Subscription>()
    .AddProjections()
    .AddFiltering()
    .AddSorting();

// .AddRedisSubscriptions((sp) => ConnectionMultiplexer.Connect("localhost:5000")); /TODO: LOOK INTO REDIS FOR THIS
// .AddInMemorySubscriptions();

// builder.Services.AddSingleton(builder.Configuration.GetSection("GoogleOauth"));

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
// builder.Services.AddCors(options =>
//     {
//         options.AddPolicy("AllowSpecificOrigins",
//             builder =>
//             {
//                 builder
//                     .AllowAnyOrigin()
//                     .AllowAnyHeader()
//                     .AllowAnyMethod();
//             });
//     });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(typeof(Program).Assembly);
builder.Services.AddHttpClient();
builder.Services.AddScoped<IUserVideoService, UserVideoService>();
builder.Services.AddScoped<IAuthTokenExchangeService, AuthTokenExchangeService>();
builder.Services.AddScoped<IFetchYoutubeDataService, FetchYoutubeDataService>();
builder.Services.AddScoped<IProcessYoutubeDataService, ProcessYoutubeDataService>();
builder.Services.AddScoped<IStoreYoutubeDataService, StoreYoutubeDataService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();

    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseWebSockets();
app.UseRouting();

app.UseCors("AllowSpecificOrigins");
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapGraphQL();
app.MapGraphQLVoyager("graphql-voyager");
app.Run();
