namespace TempMcpServer.Demo
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            SseServer.RunAsync(Mockup.CreateTestInitData()).GetAwaiter().GetResult();
        }
    }
}