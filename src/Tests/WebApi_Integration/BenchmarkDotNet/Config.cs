using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Environments;
using BenchmarkDotNet.Jobs;
using NuGet.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApi_Integration.BenchmarkDotNet;
public class Config
{

  //mc; config for benchmarkdotnet
  public static ManualConfig GetConfig() 
  {
    var config = ManualConfig.Create(DefaultConfig.Instance)
        .WithOptions(ConfigOptions.DisableOptimizationsValidator)
        .AddJob(Job.ShortRun.WithPowerPlan(PowerPlan.UserPowerPlan));

    return config;
  }
}
