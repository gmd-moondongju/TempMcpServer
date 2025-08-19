using System.Collections.Generic;

namespace TempMcpServer.Models
{
    public record InitializationData(string ServerName, string ServerVersion, int Port, IEnumerable<ToolDefinition> Tools);
}