namespace Plugins

open System.IO
open PeachPied.WordPress.Standard
open Giraffe.ViewEngine

module DashboardInfo =
    let view =
        div [] [
            h3 [] [ str "YukiBlog Powered by Wordpress on .Net" ]
            span [] [ str "Enjoy :)" ]
        ]

    let renderDashboardInfo (app:WpApp) (writer:TextWriter) =
        RenderView.AsString.htmlNode view
        |> writer.WriteLine

    type DashboardInfo() =
        interface IWpPlugin with
            member x.Configure(app) =
                app.DashboardWidget ("YukiBlog.WP.Widge.Info", "Dashboard Info", fun writer -> renderDashboardInfo app writer)
