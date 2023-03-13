using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Internal;
using Rewriting.Common.Exceptions;
using Rewriting.Context;
using Rewriting.Context.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Rewriting.Services.Contracts;

internal class ContractService : IContractService
{
    private readonly IDbContextFactory<AppDbContext> _contextFactory;
    private readonly IMapper _mapper;

    public ContractService(IDbContextFactory<AppDbContext> contextFactory, IMapper mapper)
    {
        _contextFactory = contextFactory;
        _mapper = mapper;
    }

    public async Task<ClientAuthModel> GetClientAuth(Guid contractUid)
    {
        using var context = await _contextFactory.CreateDbContextAsync();

        var contract = await context.Set<Contract>().FindAsync(contractUid)
            ?? throw new ProcessException($"Contract {contractUid} not found");

        return _mapper.Map<ClientAuthModel>(contract);
    }

    public async Task<ContractorAuthModel> GetContractorAuth(Guid contractUid)
    {
        using var context = await _contextFactory.CreateDbContextAsync();

        var contract = await context.Set<Contract>().FindAsync(contractUid)
            ?? throw new ProcessException($"Contract {contractUid} not found");

        return _mapper.Map<ContractorAuthModel>(contract);
    }

    public async Task<ContractDetailsModel> GetContractDetails(Guid contractUid)
    {
        using var context = await _contextFactory.CreateDbContextAsync();

        var contract = await context.Set<Contract>().FindAsync(contractUid)
            ?? throw new ProcessException($"Contract {contractUid} not found");

        var order = contract.Order;

        return _mapper.Map<ContractDetailsModel>(order);
    }

    public async Task<IEnumerable<ContractModel>> GetContractsByUser(Guid userUid)
    {
        using var context = await _contextFactory.CreateDbContextAsync();

        var contracts = context.Set<Order>()
            .Where(order => order.Contract != null && order.Contract.ContractorUid == userUid)
            .ToList();

        return _mapper.Map<IEnumerable<ContractModel>>(contracts);
    }

    public async Task AddResult(AddResultModel model)
    {
        using var context = await _contextFactory.CreateDbContextAsync();

        var contract = context.Set<Contract>().Find()
            ?? throw new ProcessException($"Contract {model.ContractUid} not found");

        var status = contract.Order.Status;
        if (status != OrderStatus.Evaluation &&
            status != OrderStatus.InProgress)
            throw new ProcessException($"Unable to add result to order in status {status}");

        var result = _mapper.Map<Result>(model);

        context.Add(result);
        context.SaveChanges();
    }

    public async Task AcceptResult(Guid contractUid)
    {
        using var context = await _contextFactory.CreateDbContextAsync();

        var contract = await context.Set<Contract>().FindAsync(contractUid)
            ?? throw new ProcessException($"Contract {contractUid} not found");

        if (contract.Result is null)
            throw new ProcessException($"Results for order {contractUid} not found");
        if (contract.Order.Status != OrderStatus.InProgress)
            throw new ProcessException($"Unable to accept result for order {contractUid}");

        contract.Order.Status = OrderStatus.Done;
        context.SaveChanges();
    }

    public async Task DeclineResult(Guid contractUid)
    {
        using var context = await _contextFactory.CreateDbContextAsync();

        var contract = await context.Set<Contract>().FindAsync(contractUid)
            ?? throw new ProcessException($"Contract {contractUid} not found");
        var order = contract.Order;
       
        if (order.Status != OrderStatus.Evaluation)
            throw new ProcessException($"You can't decline result for order in status {order.Status}");

        order.Status = OrderStatus.InProgress;
        
        context.SaveChanges();
    }

    public async Task DeclineContractor(Guid contractUid)
    {
        using var context = await _contextFactory.CreateDbContextAsync();

        var contract = await context.Set<Contract>().FindAsync(contractUid)
            ?? throw new ProcessException($"Contract {contractUid} not found");

        if (contract.Order.Status != OrderStatus.InProgress)
            throw new ProcessException($"Unable to decline contractor for order in status {contract.Order.Status}");

        context.Remove(contract);
        context.SaveChanges();
    }
}
