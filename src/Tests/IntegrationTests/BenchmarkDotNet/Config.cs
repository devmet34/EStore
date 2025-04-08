using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Environments;
using BenchmarkDotNet.Jobs;

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
