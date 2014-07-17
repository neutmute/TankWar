using System.Reflection;
using Kraken.Core;

[assembly: AssemblyTitle("TankWar.Web")]
[assembly: AssemblyDescription("TankWar Web")]
[assembly: AssemblyConfiguration("")]

#if DEBUG
[assembly: AssemblyProduct("TankWar.Web (Debug)")]
[assembly: AssemblyCompilation(BuildConfiguration.Debug)]
#else
[assembly: AssemblyProduct("TankWar.Web (Release)")]
[assembly:  AssemblyCompilation(BuildConfiguration.Release)]
#endif

