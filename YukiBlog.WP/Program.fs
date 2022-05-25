open System
open Microsoft.AspNetCore.Builder
open Microsoft.Extensions.Hosting
open PeachPied.WordPress.AspNetCore
open Plugins
open Microsoft.Extensions.DependencyInjection

let configEnvVariable (config:WordPressConfig) =
    let dbHost = Environment.GetEnvironmentVariable "dbHost"
    let dbName = Environment.GetEnvironmentVariable "dbName"
    let dbUser = Environment.GetEnvironmentVariable "dbUser"
    let dbPassword = Environment.GetEnvironmentVariable "dbPass"
    let homeUrl =  Environment.GetEnvironmentVariable "homeUrl"
    let siteUrl =  Environment.GetEnvironmentVariable "siteUrl"

    if not <| String.IsNullOrEmpty dbHost then config.DbHost <- dbHost
    if not <| String.IsNullOrEmpty dbName then config.DbName <- dbName
    if not <| String.IsNullOrEmpty dbUser then config.DbUser <- dbUser
    if not <| String.IsNullOrEmpty dbPassword then config.DbPassword <- dbPassword
    if not <| String.IsNullOrEmpty homeUrl then config.HomeUrl <- homeUrl
    if not <| String.IsNullOrEmpty siteUrl then config.SiteUrl <- siteUrl

let configPlugins (config:WordPressConfig) =
    for plugin in PluginProvider.getPlugins() do
        config.PluginContainer.Add(plugin) |> ignore

let configWordpress (config:WordPressConfig) =
    configEnvVariable config
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
       .UseHttpsRedirection()
       .UseHsts()
    |> ignore
    app.Run()

    0 // Exit code
