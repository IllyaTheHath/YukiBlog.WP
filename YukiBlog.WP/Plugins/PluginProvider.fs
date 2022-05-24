namespace Plugins

open PeachPied.WordPress.Standard

module PluginProvider =
    let getPlugins() :IWpPlugin seq =
        seq {
            yield DashboardInfo.DashboardInfo()
        }
