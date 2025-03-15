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
        var json = "{\"ip\":\"190.158.88.2\",\"hostname\":\"dynamic-ip-190158882.cable.net.co\",\"city\":\"Medellín\",\"region\":\"Antioquia\",\"country\":\"CO\",\"loc\":\"6.2518,-75.5636\",\"org\":\"AS10620 Telmex Colombia S.A.\",\"postal\":\"050001\",\"timezone\":\"America/Bogota\"}";

        var obj = JsonSerializer.Deserialize<IpInfo>(json);
    }
}
