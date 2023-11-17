using Microsoft.VisualStudio.TestTools.UnitTesting;
using GraphQL;
using GraphQL.Client.Http;
using GraphQL.Client.Serializer.Newtonsoft;
using System;
using System.Threading.Tasks;

[TestClass]
public class GraphQLTests
{
    private readonly GraphQLHttpClient _client;

    public GraphQLTests()
    {
        _client = new GraphQLHttpClient(
            new GraphQLHttpClientOptions
            {
                EndPoint = new Uri("http://localhost:5097/graphql"), // Your GraphQL endpoint
                HttpMessageHandler = new HttpClientHandler()
            },
            new NewtonsoftJsonSerializer()
        );
    }

    [TestMethod]
    public async Task Test_GetUser()
    {
        var query = new GraphQLRequest
        {
            Query =
                @"
                query {
                user(where: {userId: {eq: 'hbgfjbdvjgfh'}}){
                    nodes {
                    userId  createdAt updatedAt videoPlaylists{
                        youtubeId
                    }
                    }
                }
                }
            "
        };

        var response = await _client.SendQueryAsync<dynamic>(query);

        Assert.IsNotNull(response.Data);
        Assert.IsNull(response.Errors);
    }

    [TestMethod]
    public async Task Test_GetPost()
    {
        var query = new GraphQLRequest
        {
            Query =
                @"
                query {
                    post(id: 1) {
                        id
                        title
                        content
                    }
                }
            "
        };

        var response = await _client.SendQueryAsync<dynamic>(query);

        Assert.IsNotNull(response.Data);
        Assert.IsNull(response.Errors);
    }
}
