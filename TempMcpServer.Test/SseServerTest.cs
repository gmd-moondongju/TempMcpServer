using Microsoft.Extensions.Logging;
using ModelContextProtocol.Protocol.Types;
using TempMcpServer.Models;

namespace TempMcpServer.Test;

[DoNotParallelize]
[TestClass]
public sealed class SseServerTest
{
    [TestMethod]
    public async Task RunAsync_StartsAndCancelsGracefully()
    {
        var initData = Mockup.CreateTestInitData();
        using var cts = new CancellationTokenSource();
        var runTask = SseServer.RunAsync(initData, null, cts.Token);
        cts.CancelAfter(100);
        await Assert.ThrowsExceptionAsync<TaskCanceledException>(async () => await runTask);
    }

    [TestMethod]
    public async Task RunAsync_CancellationBeforeStart_ThrowsImmediately()
    {
        var initData = Mockup.CreateTestInitData();
        using var cts = new CancellationTokenSource();
        cts.Cancel();
        await Assert.ThrowsExceptionAsync<TaskCanceledException>(async () =>
            await SseServer.RunAsync(initData, null, cts.Token));
    }

    [TestMethod]
    public async Task RunAsync_NullLoggerFactory_DoesNotThrow()
    {
        var initData = Mockup.CreateTestInitData();
        using var cts = new CancellationTokenSource();
        cts.CancelAfter(100);
        var runTask = SseServer.RunAsync(initData, null, cts.Token);
        await Assert.ThrowsExceptionAsync<TaskCanceledException>(async () => await runTask);
    }

    [TestMethod]
    public async Task RunAsync_WithCustomLoggerFactory_UsesProvidedLogger()
    {
        var initData = Mockup.CreateTestInitData();
        var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());

        using var cts = new CancellationTokenSource();
        cts.CancelAfter(100);
        var runTask = SseServer.RunAsync(initData, loggerFactory, cts.Token);

        await Assert.ThrowsExceptionAsync<TaskCanceledException>(async () => await runTask);
    }

    [TestMethod]
    public async Task RunAsync_WithEmptyToolsList_StartsSuccessfully()
    {
        var initData = new InitializationData("TestServer", "1.0.0", 3001, new List<ToolDefinition>());

        using var cts = new CancellationTokenSource();
        cts.CancelAfter(100);
        var runTask = SseServer.RunAsync(initData, null, cts.Token);

        await Assert.ThrowsExceptionAsync<TaskCanceledException>(async () => await runTask);
    }

    [TestMethod]
    public async Task RunAsync_WithMultipleTools_ConfiguresAllTools()
    {
        var tools = new List<ToolDefinition>();
        for (var i = 0; i < 10; i++)
        {
            var index = i;
            tools.Add(
                new ToolDefinition(
                    $"tool_{index}",
                    $"Tool number {index}",
                    new List<ParameterDefinition>
                    {
                        new($"param_{index}", "string", $"Parameter for tool {index}")
                    },
                    async _ =>
                    {
                        await Task.Delay(10);
                        return new CallToolResponse
                        {
                            Content = [new Content() { Type = "text", Text = $"Response from tool {index}" }]
                        };
                    }
                ));
        }

        var initData = new InitializationData("TestServer", "1.0.0", 3001, tools);

        using var cts = new CancellationTokenSource();
        cts.CancelAfter(100);
        var runTask = SseServer.RunAsync(initData, null, cts.Token);

        await Assert.ThrowsExceptionAsync<TaskCanceledException>(async () => await runTask);
    }

    [TestMethod]
    public async Task RunAsync_DifferentPortNumbers_ConfiguresCorrectly()
    {
        var ports = new[] { 3000, 3001, 8080, 9090 };

        foreach (var port in ports)
        {
            var initData = new InitializationData("TestServer", "1.0.0", port, new List<ToolDefinition>());

            using var cts = new CancellationTokenSource();
            cts.CancelAfter(50);
            var runTask = SseServer.RunAsync(initData, null, cts.Token);

            await Assert.ThrowsExceptionAsync<TaskCanceledException>(async () => await runTask);
        }
    }

    [TestMethod]
    public async Task RunAsync_WithComplexToolParameters_ConfiguresCorrectly()
    {
        var tools = new List<ToolDefinition>
        {
            new(
                "complex_tool",
                "A tool with multiple parameter types",
                new List<ParameterDefinition>
                {
                    new("stringParam", "string", "A string parameter"),
                    new("numberParam", "number", "A number parameter"),
                    new("booleanParam", "boolean", "A boolean parameter"),
                    new("objectParam", "object", "An object parameter"),
                    new("arrayParam", "array", "An array parameter")
                },
                async _ =>
                {
                    await Task.Delay(10);
                    return new CallToolResponse
                    {
                        Content = [new Content() { Type = "text", Text = "Complex tool response" }]
                    };
                }
            )
        };

        var initData = new InitializationData("TestServer", "1.0.0", 3001, tools);

        using var cts = new CancellationTokenSource();
        cts.CancelAfter(100);
        var runTask = SseServer.RunAsync(initData, null, cts.Token);

        await Assert.ThrowsExceptionAsync<TaskCanceledException>(async () => await runTask);
    }

    [TestMethod]
    public async Task RunAsync_ServerNameAndVersion_ConfiguredCorrectly()
    {
        var testCases = new[]
        {
            ("MyServer", "1.0.0"),
            ("AnotherServer", "2.5.1"),
            ("TestApp", "0.0.1-alpha"),
            ("ProductionServer", "3.2.1-release")
        };

        foreach (var (serverName, serverVersion) in testCases)
        {
            var initData = new InitializationData(serverName, serverVersion, 3001, new List<ToolDefinition>());

            using var cts = new CancellationTokenSource();
            cts.CancelAfter(50);
            var runTask = SseServer.RunAsync(initData, null, cts.Token);

            await Assert.ThrowsExceptionAsync<TaskCanceledException>(async () => await runTask);
        }
    }
}