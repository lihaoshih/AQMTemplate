namespace AQMTemplate.Web;

public static class DependencyInjection
{
	public static IServiceCollection AddWebServices(this IServiceCollection services)
	{
		services.AddHttpClient("ServerAPI", client =>
		{
			client.BaseAddress = new Uri("https://localhost:7021");
		});

		services.AddScoped(sp =>
		{
			var httpClientFactory = sp.GetRequiredService<IHttpClientFactory>();
			return httpClientFactory.CreateClient("ServerAPI");
		});

		return services;
	}
}
