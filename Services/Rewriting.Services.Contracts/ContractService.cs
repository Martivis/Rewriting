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

    public async Task AddResult(AddResultModel model)
    {
        using var context = await _contextFactory.CreateDbContextAsync();

        var result = _mapper.Map<Result>(model);

        context.Add(result);
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

    public async Task DeclineResult(Guid contractUid)
    {
        using var context = await _contextFactory.CreateDbContextAsync();

        var contract = await context.Set<Contract>().FindAsync(contractUid)
            ?? throw new ProcessException($"Contract {contractUid} not found");
        var order = contract.Order;
       
        if (order.Status != OrderStatus.InProgress)
            throw new ProcessException($"You can't decline result for order in status {order.Status}");

        order.Status = OrderStatus.InProgress;
        
        context.SaveChanges();
    }

    public async Task<ContractModel> GetContract(Guid contractUid)
    {
        using var context = await _contextFactory.CreateDbContextAsync();

        var contract = await context.Set<Contract>().FindAsync(contractUid)
            ?? throw new ProcessException($"Contract {contractUid} not found");

        return _mapper.Map<ContractModel>(contract.Order);
    }

    public async Task<ContractDetailsModel> GetContractDetails(Guid contractUid)
    {
        using var context = await _contextFactory.CreateDbContextAsync();

        var contract = await context.Set<Contract>().FindAsync(contractUid)
            ?? throw new ProcessException($"Contract {contractUid} not found");

        return _mapper.Map<ContractDetailsModel>(contract.Order);
    }

    public async Task<IEnumerable<ContractModel>> GetContractsByUser(Guid userUid)
    {
        using var context = await _contextFactory.CreateDbContextAsync();

        var contracts = context.Set<Order>()
            .Where(order => order.Contract != null && order.Contract.ContractorUid == userUid)
            .ToList();

        return _mapper.Map<IEnumerable<ContractModel>>(contracts);
    }
}
