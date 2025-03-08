using DiegoG.WebTools.Data;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace DiegoG.WebTools.Sandbox;

internal class Program
{
    static async Task Main(string[] args)
    {
        var http = new HttpClient();
        var x = await http.GetFromJsonAsync<List<MeansOfContact>>("https://raw.githubusercontent.com/DiegoG1019/DiegoG1019/refs/heads/main/ContactMeans.json");

    }
}
