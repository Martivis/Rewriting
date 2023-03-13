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

    [HttpGet]
    [Authorize]
    public async Task<IEnumerable<ContractResponse>> GetContracts()
    {
        var userUid = User.GetUid();
        var contracts = await _contractService.GetContractsByUser(userUid);
        return _mapper.Map<IEnumerable<ContractResponse>>(contracts);
    }

    [HttpGet]
    [Authorize]
    public async Task<ContractDetailsResponse> GetContractDetails(Guid contractUid)
    {
        var contract = await _contractService.GetContractorAuth(contractUid);

        var authorizationResult = await _authorizationService.AuthorizeAsync(User, contract, AppScopes.ContractsEdit);
        if (!authorizationResult.Succeeded)
        {
            HttpContext.Response.StatusCode = 403;
            return new ContractDetailsResponse();
        }
            
        var contractsDetails = await _contractService.GetContractDetails(contractUid);
        return _mapper.Map<ContractDetailsResponse>(contractsDetails);
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> AddResult(AddResultRequest request)
    {
        var contractAuth = await _contractService.GetContractorAuth(request.ContractUid);

        var authorizationResult = await _authorizationService.AuthorizeAsync(User, contractAuth, AppScopes.ContractsEdit);
        if (!authorizationResult.Succeeded)
            return Forbid();

        var addResultModel = _mapper.Map<AddResultModel>(request);
        await _contractService.AddResult(addResultModel);
        return Ok();
    }

    [HttpPatch]
    [Authorize]
    public async Task<IActionResult> AcceptResult(Guid contractUid)
    {
        var contractAuth = await _contractService.GetClientAuth(contractUid);

        var authorizationResult = await _authorizationService.AuthorizeAsync(User, contractAuth, AppScopes.ContractsEdit);
        if (!authorizationResult.Succeeded)
            return Forbid();

        await _contractService.AcceptResult(contractUid);
        return Ok();
    }

    [HttpPatch]
    [Authorize]
    public async Task<IActionResult> DeclineResult(Guid contractUid)
    {
        var contractAuth = await _contractService.GetClientAuth(contractUid);

        var authorizationResult = await _authorizationService.AuthorizeAsync(User, contractAuth, AppScopes.ContractsEdit);
        if (!authorizationResult.Succeeded)
            return Forbid();

        await _contractService.DeclineResult(contractUid);
        return Ok();
    }

    [HttpPatch]
    [Authorize]
    public async Task<IActionResult> DeclineContractor(Guid contractUid)
    {
        var contractAuth = await _contractService.GetClientAuth(contractUid);

        var authorizationResult = await _authorizationService.AuthorizeAsync(User, contractAuth, AppScopes.ContractsEdit);
        if (!authorizationResult.Succeeded)
            return Forbid();

        await _contractService.DeclineContractor(contractUid);
        return Ok();
    }
}
