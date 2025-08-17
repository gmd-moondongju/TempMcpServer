namespace TempMcpServer.Test
{
    [DoNotParallelize]
    [TestClass]
    public sealed class SseServerTest
    {
        [TestMethod]
        public async Task RunAsync_StartsAndCancelsGracefully()
        {
            using var cts = new CancellationTokenSource();
            var runTask = SseServer.RunAsync(null, cts.Token);
            cts.CancelAfter(100);
            await Assert.ThrowsExceptionAsync<TaskCanceledException>(async () => await runTask);
        }

        [TestMethod]
        public async Task RunAsync_CancellationBeforeStart_ThrowsImmediately()
        {
            using var cts = new CancellationTokenSource();
            cts.Cancel();
            await Assert.ThrowsExceptionAsync<TaskCanceledException>(async () => await SseServer.RunAsync(null, cts.Token));
        }

        [TestMethod]
        public async Task RunAsync_NullLoggerFactory_DoesNotThrow()
        {
            using var cts = new CancellationTokenSource();
            cts.CancelAfter(100);
            var runTask = SseServer.RunAsync(null, cts.Token);
            await Assert.ThrowsExceptionAsync<TaskCanceledException>(async () => await runTask);
        }
    }
}