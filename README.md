# Sphere 10 Framework

This general-purpose framework was developed by [Sphere 10 Software](https://www.sphere10.com) suitable for full-stack development. These libraries have been used in mobile, desktop, web and enterprise application and for open-source and commercial software.

This framework also forms core dependency for PascalCoin Layer-2 smart-contracts (WIP).

## Libraries

| Library                                  | Description                                                | Portability      | 
| :--------------------------------------- | :--------------------------------------------------------- | :--------------- |
| [Sphere10.Framework](https://github.com/Sphere10/Framework/tree/master/src/Sphere10.Framework)                  | General-purpose library usable across all tiers. Aspects include collections, caching, comparers, conversion, memory, mathm, loggin, networking, scoping, configuration and many other aspects | .NET Standard 2  |
| [Sphere10.Framework.Application](https://github.com/Sphere10/Framework/tree/master/src/Sphere10.Framework.Application)      | Application-layer framework including lifetime management, IoC, DRM | .NET Standard 2  |
| [Sphere10.Framework.Data](https://github.com/Sphere10/Framework/tree/master/src/Sphere10.Framework.Data)             | Data-oriented components, ADO.NET extensions, Data Access Components, SQL Builder, CSV support, etc   | .NET Standard 2  |
| [Sphere10.Framework.Data.Firebird](https://github.com/Sphere10/Framework/tree/master/src/Sphere10.Framework.Data.Firebird)    | Firebird implementation of Data framework.                 | .NET Standard 2  |
| [Sphere10.Framework.Data.MSSQL](https://github.com/Sphere10/Framework/tree/master/src/Sphere10.Framework.Data.MSSQL)       | SQL Server implementation of Data framework.               | .NET Standard 2  |
| [Sphere10.Framework.Data.Sqlite](https://github.com/Sphere10/Framework/tree/master/src/Sphere10.Framework.Data.Sqlite)      | Sqlite implementation for Data framework.                  | .NET Standard 2  |
| [Sphere10.Framework.Drawing](https://github.com/Sphere10/Framework/tree/master/src/Sphere10.Framework.Drawing)          | Drawing, Bitcoin and graphics support.                     | .NET Standard 2  |
| [Sphere10.Framework.NUnit](https://github.com/Sphere10/Framework/tree/master/src/Sphere10.Framework.NUnit)            | Unit test support.                                         | .NET Standard 2  |
| [Sphere10.Framework.Services](https://github.com/Sphere10/Framework/tree/master/src/Sphere10.Framework.Services)         | WCF Client support                                         | .NET Standard 2  |
| [Sphere10.Framework.DotNet](https://github.com/Sphere10/Framework/tree/master/src/Sphere10.Framework.DotNet)      | Framework components that could not be standardized            | .NET Framework 4.6.1 |
| [Sphere10.Framework.Web.AspNet](https://github.com/Sphere10/Framework/tree/master/src/Sphere10.Framework.Web.AspNet)      | ASP.NET support and Application Framework support| .NET Framework 4.6.1 |
| [Sphere10.Framework.Web.AspNetCore]((https://github.com/Sphere10/Framework/tree/master/src/Sphere10.Framework.Web.AspNetCore))  | ASP.NET Core support and Application Framework| .NET Core 2.2 |
| [Sphere10.Framework.Windows](https://github.com/Sphere10/Framework/tree/master/src/Sphere10.Framework.Windows)                   | Windows specific components including WIN32 API wrapper, hook support, Local NTFS support, etc | .NET Framwork 4.6.1  |
| [Sphere10.Framework.Windows.LevelDB](https://github.com/Sphere10/Framework/tree/master/src/Sphere10.Framework.Windows.LevelDB)           | Level-DB wrapper | .NET Framework 4.6.1  |
| [Sphere10.Framework.Windows.Forms](https://github.com/Sphere10/Framework/tree/master/src/Sphere10.Framework.Windows.Forms)          | Extensive WinForms controls, panels and helpers as well as Sphere 10's Application framework support for WinForms | .NET Framework 4.6.1  |
| [Sphere10.Framework.Windows.Forms.Firebird](https://github.com/Sphere10/Framework/tree/master/src/Sphere10.Framework.Windows.Forms.Firebird) | Graphical components for Firebird in WinForms | .NET Framework 4.6.1  |
| [Sphere10.Framework.Windows.Forms.MSSQL](https://github.com/Sphere10/Framework/tree/master/src/Sphere10.Framework.Windows.Forms.MSSQL)    | Graphical components for SQL Server in WinForms | .NET Framework 4.6.1  |
| [Sphere10.Framework.Windows.Forms.Sqlite](https://github.com/Sphere10/Framework/tree/master/src/Sphere10.Framework.Windows.Forms.Sqlite)   | Graphical components for Sqlite in WinForms | .NET Framework 4.6.1  |
| [Sphere10.Framework.Android](https://github.com/Sphere10/Framework/tree/master/src/Sphere10.Framework.Android)                   | Android specific components                                | .NET Standard 2  |
| [Sphere10.Framework.iOS](https://github.com/Sphere10/Framework/tree/master/src/Sphere10.Framework.iOS)                       | iOS specific components                                    | Xamarin.iOS      |
| [Sphere10.Framework.macOS](https://github.com/Sphere10/Framework/tree/master/src/Sphere10.Framework.macOS)                     | macOS specific components                                  | Xamarin.Mac, MonoMac|

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