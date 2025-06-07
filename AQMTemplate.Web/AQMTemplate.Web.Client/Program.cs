using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("Mzg1Nzc5OUAzMjM5MmUzMDJlMzAzYjMyMzkzYlUyK3YzNmtvK3NzMmRDVzFDeVovQkJlU0tqZEpCSHcwMVdXYWt6ZnJONTQ9");

await builder.Build().RunAsync();
