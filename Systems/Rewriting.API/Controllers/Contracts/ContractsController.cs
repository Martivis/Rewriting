using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Rewriting.Common.Helpers;
using Rewriting.Common.Security;
using Rewriting.Services.Contracts;
using Rewriting.Services.Orders;

namespace Rewriting.API.Controllers.Contracts;

[Route("api/[controller]/[action]")]
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
    /// <returns>IEnumerable of ContractResponse</returns>
    [HttpGet]
    [Authorize]
    public async Task<IEnumerable<ContractResponse>> GetContractsByUser()
    {
        var userUid = User.GetUid();
        var contracts = await _contractService.GetContractsByUserAsync(userUid);
        return _mapper.Map<IEnumerable<ContractResponse>>(contracts);
    }

    /// <summary>
    /// Get detailed information about specified contract
    /// </summary>
    /// <param name="contractUid">Uid of target contract</param>
    /// <returns>ContractDetailsResponse</returns>
    [HttpGet]
    [Authorize]
    public async Task<ContractDetailsResponse> GetContractDetails(Guid contractUid)
    {
        var contract = await _contractService.GetContractorAuthAsync(contractUid);

        var authorizationResult = await _authorizationService.AuthorizeAsync(User, contract, AppScopes.ContractsEdit);
        if (!authorizationResult.Succeeded)
        {
            HttpContext.Response.StatusCode = 403;
            return new ContractDetailsResponse();
        }
            
        var contractsDetails = await _contractService.GetContractDetailsAsync(contractUid);
        return _mapper.Map<ContractDetailsResponse>(contractsDetails);
    }

    /// <summary>
    /// Add new result
    /// </summary>
    /// <param name="request">AddResultRequest</param>
    /// <returns>IActionResult</returns>
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> AddResult(AddResultRequest request)
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
    /// <param name="contractUid">Uid of the target contract</param>
    /// <returns>IActionResult</returns>
    [HttpPatch]
    [Authorize]
    public async Task<IActionResult> AcceptResult(Guid contractUid)
    {
        var contractAuth = await _contractService.GetClientAuthAsync(contractUid);

        var authorizationResult = await _authorizationService.AuthorizeAsync(User, contractAuth, AppScopes.ContractsEdit);
        if (!authorizationResult.Succeeded)
            return Forbid();

        await _contractService.AcceptResultAsync(contractUid);
        return Ok();
    }

    /// <summary>
    /// Decline result in specified contract
    /// </summary>
    /// <param name="contractUid">Uid of the target contract</param>
    /// <returns>IActionResult</returns>
    [HttpPatch]
    [Authorize]
    public async Task<IActionResult> DeclineResult(Guid contractUid)
    {
        var contractAuth = await _contractService.GetClientAuthAsync(contractUid);

        var authorizationResult = await _authorizationService.AuthorizeAsync(User, contractAuth, AppScopes.ContractsEdit);
        if (!authorizationResult.Succeeded)
            return Forbid();

        await _contractService.DeclineResultAsync(contractUid);
        return Ok();
    }

    /// <summary>
    /// Decline contractor in specified contract.
    /// </summary>
    /// <param name="contractUid">Uid of the target contract</param>
    /// <returns>IActionResult</returns>
    [HttpPatch]
    [Authorize]
    public async Task<IActionResult> DeclineContractor(Guid contractUid)
    {
        var contractAuth = await _contractService.GetClientAuthAsync(contractUid);

        var authorizationResult = await _authorizationService.AuthorizeAsync(User, contractAuth, AppScopes.ContractsEdit);
        if (!authorizationResult.Succeeded)
            return Forbid();

        await _contractService.DeclineContractorAsync(contractUid);
        return Ok();
    }
}
