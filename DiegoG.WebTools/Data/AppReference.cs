using DiegoG.WebTools.Services;
using Microsoft.AspNetCore.Components.WebAssembly.Http;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http.Json;

namespace DiegoG.WebTools.Data;

public sealed class AppReference : ICatalogItem
{
    public bool Enabled { get; init; }
    public string DisplayName { get; init; }
    public string Alt { get; set; }
    public string Uri { get; init; }
    public string Id { get; init; }
    public Dictionary<string, string>? LocalizedNames { get; init; }
    public string? Styles { get; init; }
}
