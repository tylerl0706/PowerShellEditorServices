//
// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
//

using System.Management.Automation;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.PowerShell.EditorServices.Services;
using MediatR;
using OmniSharp.Extensions.JsonRpc;
using OmniSharp.Extensions.LanguageServer.Protocol.Models;

namespace Microsoft.PowerShell.EditorServices.Handlers
{
    [Parallel, Method("workspace/didTrustWorkspace")]
    internal interface IDidTrustWorkspaceHandler : IJsonRpcNotificationHandler<DidTrustWorkspaceParams> { }

    internal class DidTrustWorkspaceParams : IRequest { }

    internal class DidTrustWorkspaceHandler : IDidTrustWorkspaceHandler
    {
        private readonly ILogger _logger;
        private readonly WorkspaceService _workspaceService;

        public DidTrustWorkspaceHandler(ILoggerFactory factory, WorkspaceService workspaceService)
        {
            _logger = factory.CreateLogger<DidTrustWorkspaceHandler>();
            _workspaceService = workspaceService;
        }

        public Task<Unit> Handle(DidTrustWorkspaceParams request, CancellationToken cancellationToken)
        {
            _workspaceService.IsTrusted = true;
            _logger.LogInformation("This workspace has been trusted.");
            return Unit.Task;
        }
    }
}
