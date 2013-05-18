using System.Reflection;
using Kraken.Framework.Core;

[assembly: AssemblyTitle("TankWar.Engine")]
[assembly: AssemblyDescription("TankWar Engine")]
[assembly: AssemblyConfiguration("")]

#if DEBUG
[assembly: AssemblyProduct("TankWar.Engine (Debug)")]
[assembly: AssemblyCompilation(BuildConfiguration.Debug)]
#else
[assembly: AssemblyProduct("TankWar.Engine (Release)")]
[assembly:  AssemblyCompilation(BuildConfiguration.Release)]
#endif

