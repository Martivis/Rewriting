using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Rewriting.Common.Helpers;
using Rewriting.Common.Security;
using Rewriting.Services.Contracts;
using Rewriting.Services.Orders;

namespace Rewriting.API.Controllers.Contracts;

[Route("api/v{version:apiVersion}/[controller]/[action]")]
[ApiController]
public class ContractsController : ControllerBase
{
    private readonly IAuthorizationService _authorizationService;
    private readonly IContractService _contractService;
    private readonly IMapper _mapper;

    public ContractsController(
        IAuthorizationService authorizationService,
        IContractService contractService,
        IMapper mapper)
    {
        _authorizationService = authorizationService;
        _contractService = contractService;
        _mapper = mapper;
    }

    /// <summary>
    /// Get contracts relating to calling user
    /// </summary>
    /// <param name="page">Page number (starting with 0)</param>
    /// <param name="pageSize">Contracts number per page</param>
    /// <returns>IEnumerable of ContractResponse</returns>
    [HttpGet]
    [Authorize]
    public async Task<IEnumerable<ContractResponse>> GetContractsByUser([FromQuery] int page = 0,
                                                                        [FromQuery] int pageSize = 10)
    {
        var userUid = User.GetUid();
        var contracts = await _contractService.GetContractsByUserAsync(userUid, page, pageSize);
        return _mapper.Map<IEnumerable<ContractResponse>>(contracts);
    }

    /// <summary>
    /// Get detailed information about specified contract
    /// </summary>
    /// <param name="contractUid">Uid of target contract</param>
    /// <returns>ContractDetailsResponse</returns>
    [HttpGet]
    [Authorize]
    public async Task<ContractDetailsResponse> GetContractDetails([FromQuery] Guid contractUid)
    {
        var contractorAuth = await _contractService.GetContractorAuthAsync(contractUid);
        var clientAuth = await _contractService.GetClientAuthAsync(contractUid);

        var contractorAuthResult = await _authorizationService.AuthorizeAsync(User, contractorAuth, AppScopes.ContractsEdit);
        var clientAuchResult = await _authorizationService.AuthorizeAsync(User, clientAuth, AppScopes.ContractsEdit);

        if (!contractorAuthResult.Succeeded && !clientAuchResult.Succeeded)
        {
            HttpContext.Response.StatusCode = 403;
            return new ContractDetailsResponse();
        }
            
        var contractsDetails = await _contractService.GetContractDetailsAsync(contractUid);
        return _mapper.Map<ContractDetailsResponse>(contractsDetails);
    }

    /// <summary>
    /// Get results for specified contract
    /// </summary>
    /// <param name="contractUid">Uid of target contract</param>
    /// <returns>IEnumerable of ResultResponse</returns>
    [HttpGet]
    [Authorize]
    public async Task<IEnumerable<ResultResponse>> GetResults([FromQuery] Guid contractUid,
                                                              [FromQuery] int page = 0, 
                                                              [FromQuery] int pageSize = 10)
    {
        var clientAuth = await _contractService.GetClientAuthAsync(contractUid);
        var contractorAuth = await _contractService.GetContractorAuthAsync(contractUid);
    
        var clientAuthResult = await _authorizationService.AuthorizeAsync(User, clientAuth, AppScopes.ContractsEdit);
        var contractorAuthResult = await _authorizationService.AuthorizeAsync(User, contractorAuth, AppScopes.ContractsEdit);
    
        if (!clientAuthResult.Succeeded && !contractorAuthResult.Succeeded)
        {
            HttpContext.Response.StatusCode = 403;
            return new List<ResultResponse>();
        }
    
        var results = await _contractService.GetResultsAsync(contractUid, page, pageSize);
        return _mapper.Map<IEnumerable<ResultResponse>>(results);
    }

    /// <summary>
    /// Add new result
    /// </summary>
    /// <param name="request">AddResultRequest</param>
    /// <returns>IActionResult</returns>
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> AddResult([FromBody] AddResultRequest request)
    {
        var contractAuth = await _contractService.GetContractorAuthAsync(request.ContractUid);

        var authorizationResult = await _authorizationService.AuthorizeAsync(User, contractAuth, AppScopes.ContractsEdit);
        if (!authorizationResult.Succeeded)
            return Forbid();

        var addResultModel = _mapper.Map<AddResultModel>(request);
        await _contractService.AddResultAsync(addResultModel);
        return Ok();
    }

    /// <summary>
    /// Accept result in specified contract
    /// </summary>
    /// <param name="request">Uid of the target contract</param>
    /// <returns>IActionResult</returns>
    [HttpPatch]
    [Authorize]
    public async Task<IActionResult> AcceptResult([FromBody] ContractUidRequest request)
    {
        var contractAuth = await _contractService.GetClientAuthAsync(request.ContractUid);

        var authorizationResult = await _authorizationService.AuthorizeAsync(User, contractAuth, AppScopes.ContractsEdit);
        if (!authorizationResult.Succeeded)
            return Forbid();

        await _contractService.AcceptResultAsync(request.ContractUid);
        return Ok();
    }

    /// <summary>
    /// Decline result in specified contract
    /// </summary>
    /// <param name="request">Uid of the target contract</param>
    /// <returns>IActionResult</returns>
    [HttpPatch]
    [Authorize]
    public async Task<IActionResult> DeclineResult([FromBody] ContractUidRequest request)
    {
        var contractAuth = await _contractService.GetClientAuthAsync(request.ContractUid);

        var authorizationResult = await _authorizationService.AuthorizeAsync(User, contractAuth, AppScopes.ContractsEdit);
        if (!authorizationResult.Succeeded)
            return Forbid();

        await _contractService.DeclineResultAsync(request.ContractUid);
        return Ok();
    }

    /// <summary>
    /// Decline contractor in specified contract.
    /// </summary>
    /// <param name="request">Uid of the target contract</param>
    /// <returns>IActionResult</returns>
    [HttpPatch]
    [Authorize]
    public async Task<IActionResult> DeclineContractor([FromBody] ContractUidRequest request)
    {
        var contractAuth = await _contractService.GetClientAuthAsync(request.ContractUid);

        var authorizationResult = await _authorizationService.AuthorizeAsync(User, contractAuth, AppScopes.ContractsEdit);
        if (!authorizationResult.Succeeded)
            return Forbid();

        await _contractService.DeclineContractorAsync(request.ContractUid);
        return Ok();
    }
}
