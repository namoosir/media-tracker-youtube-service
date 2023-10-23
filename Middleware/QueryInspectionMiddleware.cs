using System;
using System.Text;
using System.Threading.Tasks;
using HotChocolate;
using HotChocolate.Execution;
using Microsoft.AspNetCore.Http;
using GraphQLParser;
using GraphQLParser.AST;
using Newtonsoft.Json.Linq;
using HotChocolate.Types;
using GraphQLParser.Visitors;
using HotChocolate.Types.Descriptors.Definitions;
using Microsoft.AspNetCore.Http.Extensions;

namespace MediaTrackerYoutubeService.Middleware;

public class QueryInspectionMiddleware
{
    private readonly Microsoft.AspNetCore.Http.RequestDelegate _next;

    public QueryInspectionMiddleware(Microsoft.AspNetCore.Http.RequestDelegate next)
    {
        _next = next;
    }

    private string? ExtractUserId(List<ASTNode> definitions)
    {
        foreach (var definition in definitions)
        {
            if (definition is GraphQLOperationDefinition operation)
            {
                var userField = operation.SelectionSet.Selections
                    .OfType<GraphQLField>()
                    .FirstOrDefault(f => f.Name.StringValue == "user");

                if (userField != null && userField.Arguments != null)
                {
                    var whereArgument = userField.Arguments.FirstOrDefault(
                        arg => arg.Name.StringValue == "where"
                    );

                    if (
                        whereArgument != null
                        && whereArgument.Value is GraphQLObjectValue whereInputObject
                        && whereInputObject.Fields != null
                    )
                    {
                        var userIdField = whereInputObject.Fields.FirstOrDefault(
                            f => f.Name.StringValue == "userId"
                        );

                        if (
                            userIdField != null
                            && userIdField.Value is GraphQLObjectValue eqValue
                            && eqValue.Fields != null
                        )
                        {
                            var eqField = eqValue.Fields.FirstOrDefault(
                                f => f.Name.StringValue == "eq"
                            );

                            if (eqField != null && eqField.Value is GraphQLIntValue intValue)
                            {
                                return intValue.Value.ToString();
                            }
                        }
                    }
                }
            }
        }
        return null;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var uri = context.Request.GetEncodedPathAndQuery();

        if (uri != "/graphql")
        {
            await _next(context);
            return;
        }

        var requestBodyStream = new MemoryStream();
        await context.Request.Body.CopyToAsync(requestBodyStream);
        requestBodyStream.Seek(0, SeekOrigin.Begin);

        using (var reader = new StreamReader(requestBodyStream, Encoding.UTF8, true, 1024, true))
        {
            string queryJson = await reader.ReadToEndAsync();

            var json = JObject.Parse(queryJson);

            var queryObject = Parser.Parse(json["query"]?.Value<string>());
            var userId = ExtractUserId(queryObject.Definitions);

            if (userId != null)
            {
                //call sync service on the userid
            }
            else
            {
                //some error handling??
            }
        }
        requestBodyStream.Seek(0, SeekOrigin.Begin);
        context.Request.Body = requestBodyStream;

        // Continue processing the request.
        await _next(context);
    }
}
