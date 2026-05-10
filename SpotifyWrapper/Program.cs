using SpotifyAPI.Web;

var builder = WebApplication.CreateBuilder(args);

// Services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Spotify client
builder.Services.AddSingleton(sp =>
{
    var config = sp.GetRequiredService<IConfiguration>();
    var clientId = config["Spotify:ClientId"];
    var clientSecret = config["Spotify:ClientSecret"];

    var spotifyConfig = SpotifyClientConfig.CreateDefault();
    var request = new ClientCredentialsRequest(clientId!, clientSecret!);
    var response = new OAuthClient(spotifyConfig).RequestToken(request).Result;

    return new SpotifyClient(spotifyConfig.WithToken(response.AccessToken));
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();
app.MapControllers();
app.Run();