using ModelContextProtocol.Protocol.Types;
using TempMcpServer.Models;

namespace TempMcpServer;

public static class Mockup
{
    public static InitializationData CreateTestInitData()
    {
        var tools = new List<ToolDefinition>
        {
            new(
                "test_tool",
                "A test tool",
                new List<ParameterDefinition>
                {
                    new("param1", "string", "First parameter"),
                    new("param2", "number", "Second parameter")
                },
                async _ =>
                {
                    await Task.Delay(10);
                    return new CallToolResponse
                    {
                        Content = [new Content { Type = "text", Text = "Test response" }]
                    };
                }
            ),
            new(
                "echo_tool",
                "Echoes the input",
                new List<ParameterDefinition>
                {
                    new("message", "string", "Message to echo")
                },
                async args =>
                {
                    await Task.Delay(10);
                    var message = args.TryGetValue("message", out var arg) ? arg.ToString() : "No message";
                    return new CallToolResponse
                    {
                        Content = [new Content { Type = "text", Text = $"Echo: {message}" }]
                    };
                }
            )
        };

        return new InitializationData("TestServer", "1.0.0", 3001, tools);
    }
}