open System
open Microsoft.AspNetCore.Builder
open Microsoft.Extensions.Hosting
open PeachPied.WordPress.AspNetCore
open Plugins
open Microsoft.Extensions.DependencyInjection

let configDatabase (config:WordPressConfig) =
    let dbHost = Environment.GetEnvironmentVariable "dbHost"
    let dbName = Environment.GetEnvironmentVariable "dbName"
    let dbUser = Environment.GetEnvironmentVariable "dbUser"
    let dbPassword = Environment.GetEnvironmentVariable "dbPass"

    config.DbHost <- dbHost
    config.DbName <- dbName
    config.DbUser <- dbUser
    config.DbPassword <- dbPassword

let configPlugins (config:WordPressConfig) =
    for plugin in PluginProvider.getPlugins() do
        config.PluginContainer.Add(plugin) |> ignore

let configWordpress (config:WordPressConfig) =
    configDatabase config
    configPlugins config

[<EntryPoint>]
let main args =
    let builder = WebApplication.CreateBuilder(args)

    let services = builder.Services
    services.AddDistributedMemoryCache()
            .AddSession()
    |> ignore
    services.AddWordPress(fun config -> configWordpress config) |> ignore

    let app = builder.Build()

    app.UseSession()
       .UseWordPress()
    |> ignore
    app.Run()

    0 // Exit code
