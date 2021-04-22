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
    [Parallel, Method("workspace/didGrantWorkspaceTrust")]
    internal interface IDidGrantWorkspaceTrustHandler : IJsonRpcNotificationHandler<DidGrantWorkspaceTrustParams> { }

    internal class DidGrantWorkspaceTrustParams : IRequest { }

    internal class DidGrantWorkspaceTrustHandler : IDidGrantWorkspaceTrustHandler
    {
        private readonly ILogger _logger;
        private readonly AnalysisService _analysisService;
        private readonly WorkspaceService _workspaceService;

        public DidGrantWorkspaceTrustHandler(
            ILoggerFactory factory,
            AnalysisService analysisService,
            WorkspaceService workspaceService)
        {
            _logger = factory.CreateLogger<DidGrantWorkspaceTrustHandler>();
            _analysisService = analysisService;
            _workspaceService = workspaceService;
        }

        public Task<Unit> Handle(DidGrantWorkspaceTrustParams request, CancellationToken cancellationToken)
        {
            _workspaceService.IsTrusted = true;
            _logger.LogInformation("This workspace has been trusted.");
            _analysisService.RunScriptDiagnostics(_workspaceService.GetOpenedFiles());
            return Unit.Task;
        }
    }
}
