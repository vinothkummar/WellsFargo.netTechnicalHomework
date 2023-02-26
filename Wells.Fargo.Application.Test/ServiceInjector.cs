using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using Wells.Fargo.Application;
using Wells.Fargo.Application.Interface;
using Wells.Fargo.Application.RuleEngine.Interface;
using Wells.Fargo.Application.RuleEngine.Rules;

namespace Wells.Fargo.Application.Test
{
    public static class ServiceInjector
    {
        public static IServiceProvider ConfigureServices()
        {
            IServiceCollection services = new ServiceCollection();

            services.AddSingleton<IOMSRuleEngine, OMSRuleEngine>();

            services.AddSingleton<IOMSRuleEngineProcessor, OMSTypeAAA>();
            
            //services.AddSingleton<IOMSRuleEngineProcessor, OMSTypeBBB>();


            return services.BuildServiceProvider();
        }
    }
}