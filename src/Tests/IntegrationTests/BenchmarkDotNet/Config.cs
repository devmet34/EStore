using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Environments;
using BenchmarkDotNet.Jobs;
using NuGet.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegrationTests.BenchmarkDotNet;
public class Config
{

  //mc; config for benchmarkdotnet
  
  public static ManualConfig GetConfig() 
  {
    var config = ManualConfig.Create(DefaultConfig.Instance)
        .WithOptions(ConfigOptions.DisableOptimizationsValidator)
        //this prevents tool to maximize power settings and brightness 
        .AddJob(Job.ShortRun.WithPowerPlan(PowerPlan.UserPowerPlan)); 

    return config;
  }
}
