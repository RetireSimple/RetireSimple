# RetireSimple

A financial modeling framework focused on retirement, designed around extensibility and user choice.

This project is currently under construction.

## Roadmap

We have a general release plan with some of the rationale in the following excel sheet. Items in the sheet will include links to the corresponding GitHub Issues to validate progress. Items that were completed before this plan was created my not have a linked issue.

[Release Plan](https://1drv.ms/x/s!ApAyK07lZKjs5aVw3Fn2t7cW0NeymQ?e=aZJfgg)

## Supported Platforms

### RetireSimple Executable

We provide the following versions main RetireSimple Executable (which launches a web server and launches your browser to our Frontend):

| OS | Release Suffix | Supported |
| --- | --- | --- |
| Windows 32-bit | `win-x86` | :white_check_mark: |
| Windows 64-bit | `win-x64` | :white_check_mark: |
| Windows ARM64 | `win-arm64` | :white_check_mark: |
| Linux 64-bit | `linux-x64` | :white_check_mark: |
| Linux ARM64 | `linux-arm64` | :white_check_mark: |
| MacOS 11+ Intel | `osx.11-x64` | :x: |
| MacOS 11+ Apple Silicon | `osx.11-arm64` | :x: |

> For more information about MacOS Release Limitations, checkout the respective wiki page: TBD

### RetireSimple Engine (Library)

Because the RetireSimple Engine uses .NET 6, it is able to be compiled for other platforms. This means any project using .NET 6 will be able to use the RetireSimple Engine. We provide a NuGet package via GitHub packages.

Happy Hacking!

## Toolchains/Building

To compile/develop RetireSimple, the following tools are required:

- .NET 6 (`dotnet` CLI)
- `dotnet-ef` tools [(link)](https://learn.microsoft.com/en-us/ef/core/get-started/overview/install#get-the-entity-framework-core-tools)
- Node.JS 16 (minimum compatible version, required for frontend build)
- **IMPORTANT**`pnpm` v7 (install to global tools via `npm install -g pnpm`)

The project can be opened in most modern IDEs and built using CLI tools, but was specifically constructed in Visual Studio which should provide the fullest compatibility with the project configuration.

The `RetireSimple.Frontend` project uses pnpm over npm for dependency management and managing builds. There is no difference in how you would normally build/run the frontend, except for using `pnpm` (i.e. `pnpm run build`, `pnpm test`, `pnpm install`, etc.). For more information about pnpm, [visit their website](https://pnpm.io/).

A local release of RetireSimple can be built with `dotnet publish` on the `RetireSimple.Backend` project. We provide a publish profile with settings to optimize the build, so the recommend way to execute this build at the root of the repository is:

```shell
dotnet publish RetireSimple.Backend/Retiresimple.Backend.csproj -p:PublishProfile=RetireSimple.Backend/Properties/PublishProfiles/FolderProfiles.pubxml
```

## Documentation

Most of the documentation is still under construction and is subject to change until the initial release. This can be found on the [Wiki](https://github.com/rhit-westeraj/RetireSimple/wiki)
