# Sphere 10 Framework

This general-purpose framework was developed by [Sphere 10 Software](https://www.sphere10.com) suitable for full-stack development. These libraries have been used in mobile, desktop, web and enterprise application and for open-source and commercial software.

This framework also forms core dependency for PascalCoin Layer-2 smart-contracts (WIP).

## Libraries

| Library                                  | Description                                                | Portability      | 
| :--------------------------------------- | :--------------------------------------------------------- | :--------------- |
| [Sphere10.Framework][1]                  | General-purpose library usable across all tiers. Aspects include collections, caching, comparers, conversion, memory, mathm, loggin, networking, scoping, configuration and many other aspects | .NET Standard 2  |
| [Sphere10.Framework.Data][2]             | Data-oriented components, ADO.NET extensions, Data Access Components, SQL Builder, CSV support, etc   | .NET Standard 2  |
| [Sphere10.Framework.Data.Firebird][3]    | Firebird implementation of Data framework.                 | .NET Standard 2  |
| [Sphere10.Framework.Data.MSSQL][4]       | SQL Server implementation of Data framework.               | .NET Standard 2  |
| [Sphere10.Framework.Data.Sqlite][5]      | Sqlite implementation for Data framework.                  | .NET Standard 2  |
| [Sphere10.Framework.Drawing][6]          | Drawing, Bitcoin and graphics support.                     | .NET Standard 2  |
| [Sphere10.Framework.NUnit][7]            | Unit test support.                                         | .NET Standard 2  |
| [Sphere10.Framework.Services][8]         | WCF Client support                                         | .NET Standard 2  |
| [Sphere10.Framework.Web][9]              | ASP NET Core support                                       | .NET Standard 2  |
| [Sphere10.Application][10]               | Application-layer framework including lifetime management, IoC, DRM | .NET Standard 2  |
| [Sphere10.Application.Web][11]           | Dependecy-injector support for ASP NET Core (WIP)          | .NET Core 2.2    |
| [Sphere10.Application.WinForms][12]      | Application-layer GUIs and controls                        | .NET Framework 4.6.1  |
| [Sphere10.Windows.LevelDB][13]           | Level-DB wrapper                                           | .NET Framework 4.6.1  |
| [Sphere10.Windows.WinForms][14]          | WinForms Controls, Dialogs, Forms, Extensions and helpers  | .NET Framework 4.6.1  |
| [Sphere10.Windows.WinForms.Firebird][15] | Firebird DB controls for WinForms/Data Framework           | .NET Framework 4.6.1  |
| [Sphere10.Windows.WinForms.MSSQL][16]    | SQL Server DB controls for WinForms/Data Framework         | .NET Framework 4.6.1  |
| [Sphere10.Windows.WinForms.Sqlite][17]   | SQL Server DB controls for WinForms/Data Framework         | .NET Framework 4.6.1  |
| [Sphere10.DotNet][18]                    | Framework components that could not be standardized        | .NET Framework 4.6.1  |
| [Sphere10.Android][19]                   | Android specific components                                | .NET Standard 2  |
| [Sphere10.iOS][20]                       | iOS specific components                                    | Xamarin.iOS      |
| [Sphere10.macOS][21]                     | macOS specific components                                  | Xamarin.Mac, MonoMac|
| [Sphere10.Windows][22]                   | Windows specific components including WIN32 API wrapper, hook support, Local NTFS support, etc | .NET Framwork 4.6.1  |

## Roadmap / Dev notes

- Merge Sphere10.Application* and Sphere10.Framework.Data into Sphere10.Framework* 
    + Simpler for Nuget management and no longer needs separation due to identical Net Standard dependencies (no benefit to separate)
- ComponentRegistry & Module Config should replace all singleton-based global 
    + Use ComponentRegistry and ModuleConfigurations to setup settings, loggers, sql server controls (no more type activation)    
- Fix/clean/refactory web-modules for ASP NET Core
- Cleanup mobile modules 
    + remove ios/android specific wcf client support (should work now with net standard)
- Deprecate/remove older classes
- Global cosmetic cleanup, ready for Smart-Contract development

### Credits

Written by Herman Schoenfeld and property of Sphere 10 Software (unless otherwise specified in the code-file itself).

[1]: https://github.com/Sphere10/Framework/tree/master/src/Sphere10.Framework
[2]: https://github.com/Sphere10/Framework/tree/master/src/Sphere10.Framework.Data
[3]: https://github.com/Sphere10/Framework/tree/master/src/Sphere10.Framework.Data.Firebird
[4]: https://github.com/Sphere10/Framework/tree/master/src/Sphere10.Framework.Data.MSSQL
[5]: https://github.com/Sphere10/Framework/tree/master/src/Sphere10.Framework.Data.Sqlite
[6]: https://github.com/Sphere10/Framework/tree/master/src/Sphere10.Framework.Drawing
[7]: https://github.com/Sphere10/Framework/tree/master/src/Sphere10.Framework.NUnit
[8]: https://github.com/Sphere10/Framework/tree/master/src/Sphere10.Framework.Services
[9]: https://github.com/Sphere10/Framework/tree/master/src/Sphere10.Framework.Web
[10]: https://github.com/Sphere10/Framework/tree/master/src/Sphere10.Application
[11]: https://github.com/Sphere10/Framework/tree/master/src/Sphere10.Application.Web
[12]: https://github.com/Sphere10/Framework/tree/master/src/Sphere10.Application.WinForms
[13]: https://github.com/Sphere10/Framework/tree/master/src/Sphere10.Windows.LevelDB
[14]: https://github.com/Sphere10/Framework/tree/master/src/Sphere10.Windows.WinForms
[15]: https://github.com/Sphere10/Framework/tree/master/src/Sphere10.Windows.WinForms.Firebird
[16]: https://github.com/Sphere10/Framework/tree/master/src/Sphere10.Windows.WinForms.MSSQL
[17]: https://github.com/Sphere10/Framework/tree/master/src/Sphere10.Windows.WinForms.Sqlite
[18]: https://github.com/Sphere10/Framework/tree/master/src/Sphere10.DotNet
[19]: https://github.com/Sphere10/Framework/tree/master/src/Sphere10.Android
[20]: https://github.com/Sphere10/Framework/tree/master/src/Sphere10.iOS
[21]: https://github.com/Sphere10/Framework/tree/master/src/Sphere10.macOS
[22]: https://github.com/Sphere10/Framework/tree/master/src/Sphere10.Windows
