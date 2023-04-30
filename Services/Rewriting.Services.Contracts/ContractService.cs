using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Rewriting.Common.Exceptions;
using Rewriting.Context;
using Rewriting.Context.Entities;
using Rewriting.Services.Cache;

namespace Rewriting.Services.Contracts;

internal class ContractService : IContractService, IContractObservable
{
    private readonly IContractRepository _contractRepository;
    private readonly IResultRepository _resultRepository;
    private readonly IMapper _mapper;

    public event Action<ContractDetailsModel> OnResultAdd;
    public event Action<ContractDetailsModel> OnResultAccept;
    public event Action<ContractDetailsModel> OnResultDecline;
    public event Action<ContractDetailsModel> OnContractorDecline;

    public ContractService(IContractRepository contractRepository, IResultRepository resultRepository, IMapper mapper)
    {
        _contractRepository = contractRepository;
        _resultRepository = resultRepository;
        _mapper = mapper;
    }

    public async Task<ClientAuthModel> GetClientAuthAsync(Guid contractUid)
    {
        var contract = await _contractRepository.GetContractAsync(contractUid);

        return _mapper.Map<ClientAuthModel>(contract);
    }

    public async Task<ContractorAuthModel> GetContractorAuthAsync(Guid contractUid)
    {
        var contract = await _contractRepository.GetContractAsync(contractUid);

        return _mapper.Map<ContractorAuthModel>(contract);
    }

    public async Task<ContractDetailsModel> GetContractDetailsAsync(Guid contractUid)
    {
        return await _contractRepository.GetContractDetailsAsync(contractUid);
    }

    public async Task<IEnumerable<ContractModel>> GetContractsByUserAsync(Guid userUid, int page = 0, int pageSize = 10)
    {
        return await _contractRepository.GetContractsByUserAsync(userUid, page, pageSize);
    }

    public async Task<IEnumerable<ResultModel>> GetResultsAsync(Guid contractUid, int page = 0, int pageSize = 10)
    {
        return await _resultRepository.GetResultsByOrderAsync(contractUid, page, pageSize);
    }

    public async Task AddResultAsync(AddResultModel model)
    {
        var contract = await _contractRepository.GetContractAsync(model.ContractUid);

        var status = contract.Order.Status;
        if (status != OrderStatus.Evaluation &&
            status != OrderStatus.InProgress)
            throw new ProcessException($"Unable to add result to order in status {status}");

        contract.Order.Status = OrderStatus.Evaluation;

        var result = _mapper.Map<Result>(model);
        result.PublishDate = DateTime.UtcNow;

        await _resultRepository.AddResult(result);

        var contractDetailsModel = await _contractRepository.GetContractDetailsAsync(model.ContractUid);
        OnResultAdd.Invoke(contractDetailsModel);
    }

    public async Task AcceptResultAsync(Guid contractUid)
    {
        var contract = await _contractRepository.GetContractAsync(contractUid);

        if (contract.Result is null)
            throw new ProcessException($"Results for order {contractUid} not found");
        if (contract.Order.Status != OrderStatus.Evaluation)
            throw new ProcessException($"Unable to accept result for order in status {contract.Order.Status}");

        contract.Order.Status = OrderStatus.Done;
        
        await _contractRepository.UpdateContractAsync(contract);

        var contractDetailsModel = await _contractRepository.GetContractDetailsAsync(contractUid);
        OnResultAccept.Invoke(contractDetailsModel);
    }

    public async Task DeclineResultAsync(Guid contractUid)
    {
        var contract = await _contractRepository.GetContractAsync(contractUid);
        var order = contract.Order;
       
        if (order.Status != OrderStatus.Evaluation)
            throw new ProcessException($"You can't decline result for order in status {order.Status}");

        order.Status = OrderStatus.InProgress;
        
        await _contractRepository.UpdateContractAsync(contract);

        var contractDetailsModel = await _contractRepository.GetContractDetailsAsync(contractUid);
        OnResultDecline.Invoke(contractDetailsModel);
    }

    public async Task DeclineContractorAsync(Guid contractUid)
    {
        var contract = await _contractRepository.GetContractAsync(contractUid);

        var order = contract.Order;

        if (order.Status != OrderStatus.InProgress)
            throw new ProcessException($"Unable to decline contractor for order in status {order.Status}");

        order.Status = OrderStatus.New;

        var contractDetailsModel = await _contractRepository.GetContractDetailsAsync(contractUid);

        await _contractRepository.DeleteContractAsync(contractUid);

        OnContractorDecline.Invoke(contractDetailsModel);
    }
}
