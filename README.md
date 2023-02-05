# RetireSimple

A financial modeling framework focused on retirement, designed around extensibility and user choice.

This project is currently under construction.

## Roadmap

We have a general release plan with some of the rationale in the following excel sheet. Items in the sheet will include links to the corresponding GitHub Issues to validate progress. Items that were completed before this plan was created my not have a linked issue.

[Release Plan](https://1drv.ms/x/s!ApAyK07lZKjs5aVw3Fn2t7cW0NeymQ?e=aZJfgg)

## Toolchains/Building

To compile/develop RetireSimple, the following tools are required:

- .NET 6 (`dotnet` CLI)
- `dotnet-ef` tools [(link)](https://learn.microsoft.com/en-us/ef/core/get-started/overview/install#get-the-entity-framework-core-tools)
- Node.JS 16 (minimum compatible version, required for frontend build)

The project can be opened in most modern IDEs and built using CLI tools, but was specifically constructed in Visual Studio which should provide the fullest compatibility with the project configuration.

Windows machines are preferred for development as the `.esproj` format used by `dotnet` requires the use of .NET Framework 4.7 during build despite being a general wrapper over an Node project for Visual Studio. Development on other platforms is still viable, the use of the `--no-dependencies` flag with `dotnet build` will help in most cases.

## Documentation

Most of the documentation is still under construction and is subject to change until the initial release. This can be found on the [Wiki](https://github.com/rhit-westeraj/RetireSimple/wiki)
