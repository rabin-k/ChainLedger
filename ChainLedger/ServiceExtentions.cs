using ChainLedger.Abstractions;
using ChainLedger.Models;
using ChainLedger.Security;
using ChainLedger.Services;
using Microsoft.Extensions.DependencyInjection;

namespace ChainLedger
{
    public static class ServiceExtentions
    {
        public static IServiceCollection AddChainLedger<T>(this IServiceCollection services)
        {
            services.AddTransient<IBlock<T>, Block<T>>();
            services.AddSingleton<IBlockChain<T>, BlockChain<T>>();
            services.AddScoped<IConsensusManager, ConsensusManager>();
            services.AddScoped<ISecurityManager, SecurityManager>();
            services.AddSingleton<IBlockChainService<T>, BlockchainService<T>>();

            return services;
        }
    }
}
