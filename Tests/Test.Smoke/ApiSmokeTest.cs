namespace Test.Smoke;

[TestClass]
public class ApiSmokeTest
{
    private const string DEFAULT_BASE_URL = "http://localhost:5000";
    private static HttpClient _client = null!;

    [ClassInitialize]
    public static void Setup(TestContext _)
    {
        var baseUrl = Environment.GetEnvironmentVariable("SMOKE_API_URL") ?? DEFAULT_BASE_URL;
        _client = new HttpClient { BaseAddress = new Uri(baseUrl), Timeout = TimeSpan.FromSeconds(30) };
    }

    [ClassCleanup]
    public static void Cleanup() => _client?.Dispose();

    [TestMethod]
    public async Task Given_ApiDesplegada_When_SeConsultaBuild_Then_RespondeOkConVersion()
    {
        var response = await _client.GetAsync("/Deployment/build");

        Assert.IsTrue(response.IsSuccessStatusCode,
            $"La api no respondio OK al endpoint build. Status: {response.StatusCode}");
        var body = await response.Content.ReadAsStringAsync();
        StringAssert.Contains(body, "Api Liga Libre",
            $"La respuesta no contiene el identificador esperado. Body: {body}");
    }

    [TestMethod]
    public async Task Given_ApiDesplegada_When_SeConsultaBuild_Then_ContentTypeEsTexto()
    {
        var response = await _client.GetAsync("/Deployment/build");

        Assert.IsNotNull(response.Content.Headers.ContentType,
            "La respuesta deberia tener un Content-Type definido");
    }

    [TestMethod]
    public async Task Given_ApiConectadaALaDb_When_SeConsultaMigrations_Then_RespondeOk()
    {
        var response = await _client.GetAsync("/Deployment/migrations");

        Assert.IsTrue(response.IsSuccessStatusCode,
            $"La api no pudo comunicarse con la db al consultar migrations. Status: {response.StatusCode}");
    }

    [TestMethod]
    public async Task Given_ApiConectadaALaDb_When_SeConsultaMigrations_Then_ListaEncabezadoDeMigracionesAplicadas()
    {
        var response = await _client.GetAsync("/Deployment/migrations");

        var body = await response.Content.ReadAsStringAsync();
        StringAssert.Contains(body, "Migraciones aplicadas",
            $"Se esperaba el encabezado de migraciones aplicadas. Body: {body}");
    }

    [TestMethod]
    public async Task Given_RutaInexistente_When_SeConsulta_Then_ApiRespondeNotFound()
    {
        var response = await _client.GetAsync("/Deployment/ruta-que-no-existe");

        Assert.AreEqual(System.Net.HttpStatusCode.NotFound, response.StatusCode,
            "Una ruta inexistente deberia devolver 404, confirmando que la api rutea correctamente");
    }
}