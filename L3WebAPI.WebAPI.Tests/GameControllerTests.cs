using System.Net;
using System.Text;
using System.Text.Json;
using L3WebAPI.Common.Dto;
using L3WebAPI.Common.Request;
using Microsoft.AspNetCore.Mvc.Testing;

namespace L3WebAPI.WebAPI.Tests;

public class GameControllerTests
{
    private readonly HttpClient client;
    public GameControllerTests()
    {
        var webApplicationFactory = new WebApplicationFactory<Program>();
        client = webApplicationFactory.CreateClient();
    }

    [Fact]
    public async Task Should_GetAllGames_ArrayOfGames_OK()
    {
        var res = await client.GetAsync("/api/games");
        Assert.Equal(HttpStatusCode.OK, res.StatusCode);

        var data = JsonSerializer.Deserialize<IEnumerable<GameDTO>>(await res.Content.ReadAsStringAsync());
        
        Assert.NotNull(data);
        Assert.True(data.Count() > 0);
        
    }

    
    
    [Theory]
    [InlineData("a", 2)]
    [InlineData("A", 2)]
    [InlineData("alf", 1)]
    [InlineData("z", 0)]
    public async Task Should_SearchName_ArrayOfGames_Ok(string name, int expectedId)
    {
        var res = await client.GetAsync("/api/games");
        Assert.Equal(HttpStatusCode.OK, res.StatusCode);

        var data = JsonSerializer.Deserialize<IEnumerable<GameDTO>>(await res.Content.ReadAsStringAsync());
        
        Assert.NotNull(data);
        Assert.True(data.Count() > 0);
    }

    [Fact]
    public async Task Should_CreationGame_OK()
    {
        var game = new CreateGameRequest()
        {
            Name = "Minecraft",
            Prices =
            [
                new()
                {
                    Valeur = 19.99M,
                    Currency = Common.Currency.USD
                }
            ]
        };
        
        var content = new StringContent(JsonSerializer.Serialize(game),Encoding.UTF8,"application/json");
        
        var res = await client.PostAsync("/api/games", content);
        Assert.Equal(HttpStatusCode.OK, res.StatusCode);
    }
    
    [Fact]
    public async Task Should_CreationGame_KO()
    {
        var game = new CreateGameRequest()
        {
            Name = "",
            Prices =
            [
                new()
                {
                    Valeur = 19.99M,
                    Currency = Common.Currency.USD
                }
            ]
        };
        
        var content = new StringContent(JsonSerializer.Serialize(game),Encoding.UTF8,"application/json");
        
        var res = await client.PostAsync("/api/games", content);
        Assert.Equal(HttpStatusCode.BadRequest, res.StatusCode);
        Assert.Contains("Le nom doit être defini", await res.Content.ReadAsStringAsync());
    }
}




