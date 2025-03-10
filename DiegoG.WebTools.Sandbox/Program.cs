using DiegoG.WebTools.Data;
using System.Globalization;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;

namespace DiegoG.WebTools.Sandbox;

internal class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine(JsonSerializer.Serialize(CultureInfo.CurrentCulture.NumberFormat, new JsonSerializerOptions() { WriteIndented = true }));
    }
}
