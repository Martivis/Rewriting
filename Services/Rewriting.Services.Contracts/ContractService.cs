using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Rewriting.Common.Exceptions;
using Rewriting.Context;
using Rewriting.Context.Entities;

namespace Rewriting.Services.Contracts;

internal class ContractService : IContractService, IContractObservable
{
    private readonly IDbContextFactory<AppDbContext> _contextFactory;
    private readonly IMapper _mapper;

    public event Action<ContractDetailsModel> OnResultAdd;
    public event Action<ContractDetailsModel> OnResultAccept;
    public event Action<ContractDetailsModel> OnResultDecline;
    public event Action<ContractDetailsModel> OnContractorDecline;

    public ContractService(IDbContextFactory<AppDbContext> contextFactory, IMapper mapper)
    {
        _contextFactory = contextFactory;
        _mapper = mapper;
    }

    public async Task<ClientAuthModel> GetClientAuthAsync(Guid contractUid)
    {
        using var context = await _contextFactory.CreateDbContextAsync();

        var contract = context.Set<Contract>().Find(contractUid)
            ?? throw new ProcessException($"Contract {contractUid} not found");

        return _mapper.Map<ClientAuthModel>(contract);
    }

    public async Task<ContractorAuthModel> GetContractorAuthAsync(Guid contractUid)
    {
        using var context = await _contextFactory.CreateDbContextAsync();

        var contract = context.Set<Contract>().Find(contractUid)
            ?? throw new ProcessException($"Contract {contractUid} not found");

        return _mapper.Map<ContractorAuthModel>(contract);
    }

    public async Task<ContractDetailsModel> GetContractDetailsAsync(Guid contractUid)
    {
        using var context = await _contextFactory.CreateDbContextAsync();

        var contract = context.Set<Contract>().Find(contractUid)
            ?? throw new ProcessException($"Contract {contractUid} not found");

        var order = contract.Order;

        return _mapper.Map<ContractDetailsModel>(order);
    }

    public async Task<IEnumerable<ContractModel>> GetContractsByUserAsync(Guid userUid, int page = 0, int pageSize = 10)
    {
        using var context = await _contextFactory.CreateDbContextAsync();

        var contracts = context.Set<Order>()
            .Where(order => order.Contract != null && order.Contract.ContractorUid == userUid)
            .Skip(pageSize * page)
            .Take(pageSize)
            .ToList();

        return _mapper.Map<IEnumerable<ContractModel>>(contracts);
    }

    public async Task AddResultAsync(AddResultModel model)
    {
        using var context = await _contextFactory.CreateDbContextAsync();

        var contract = context.Set<Contract>().Find(model.ContractUid)
            ?? throw new ProcessException($"Contract {model.ContractUid} not found");

        var status = contract.Order.Status;
        if (status != OrderStatus.Evaluation &&
            status != OrderStatus.InProgress)
            throw new ProcessException($"Unable to add result to order in status {status}");

        contract.Order.Status = OrderStatus.Evaluation;

        var result = _mapper.Map<Result>(model);
        result.PublishDate = DateTime.UtcNow;

        context.Add(result);
        context.SaveChanges();

        var refresedOrder = context.Set<Order>().Find(model.ContractUid);
        var contractDetailsModel = _mapper.Map<ContractDetailsModel>(refresedOrder);
        OnResultAdd.Invoke(contractDetailsModel);
    }

    public async Task AcceptResultAsync(Guid contractUid)
    {
        using var context = await _contextFactory.CreateDbContextAsync();

        var contract = context.Set<Contract>().Find(contractUid)
            ?? throw new ProcessException($"Contract {contractUid} not found");

        if (contract.Result is null)
            throw new ProcessException($"Results for order {contractUid} not found");
        if (contract.Order.Status != OrderStatus.Evaluation)
            throw new ProcessException($"Unable to accept result for order in status {contract.Order.Status}");

        contract.Order.Status = OrderStatus.Done;
        context.SaveChanges();

        var refresedOrder = context.Set<Order>().Find(contractUid);
        var contractDetailsModel = _mapper.Map<ContractDetailsModel>(refresedOrder);
        OnResultAccept.Invoke(contractDetailsModel);
    }

    public async Task DeclineResultAsync(Guid contractUid)
    {
        using var context = await _contextFactory.CreateDbContextAsync();

        var contract = context.Set<Contract>().Find(contractUid)
            ?? throw new ProcessException($"Contract {contractUid} not found");
        var order = contract.Order;
       
        if (order.Status != OrderStatus.Evaluation)
            throw new ProcessException($"You can't decline result for order in status {order.Status}");

        order.Status = OrderStatus.InProgress;
        
        context.SaveChanges();

        var refresedOrder = context.Set<Order>().Find(contractUid);
        var contractDetailsModel = _mapper.Map<ContractDetailsModel>(refresedOrder);
        OnResultDecline.Invoke(contractDetailsModel);
    }

    public async Task DeclineContractorAsync(Guid contractUid)
    {
        using var context = await _contextFactory.CreateDbContextAsync();

        var contract = context.Set<Contract>().Find(contractUid)
            ?? throw new ProcessException($"Contract {contractUid} not found");

        var order = contract.Order;

        if (order.Status != OrderStatus.InProgress)
            throw new ProcessException($"Unable to decline contractor for order in status {order.Status}");

        order.Status = OrderStatus.New;

        var refresedOrder = context.Set<Order>().Find(contractUid);
        var contractDetailsModel = _mapper.Map<ContractDetailsModel>(refresedOrder);

        context.Remove(contract);
        context.SaveChanges();

        OnContractorDecline.Invoke(contractDetailsModel);
    }
}
