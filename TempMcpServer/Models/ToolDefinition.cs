using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ModelContextProtocol.Protocol.Types;

namespace TempMcpServer.Models
{
    public record ToolDefinition(
        string Name,
        string Description,
        IEnumerable<ParameterDefinition> Parameters,
        Func<IReadOnlyDictionary<string, object>, Task<CallToolResponse>> Tool);
}