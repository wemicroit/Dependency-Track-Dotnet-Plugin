using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Kiota.Abstractions;
using Microsoft.Kiota.Abstractions.Authentication;
using Microsoft.Kiota.Http.HttpClientLibrary;
using WeMicroIt.DependencyTrack.Dotnet.Models;

namespace WeMicroIt.DependencyTrack.Dotnet.Plugin;

internal class Program
{
    static async Task Main(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .AddCommandLine(args)
            .Build();

        var projectOptions = configuration.Get<ProjectOptions>();
        // API requires no authentication, so use the anonymous
        // authentication provider
        if(projectOptions == null){
            return;
        }
        var authProvider = new ApiKeyAuthenticationProvider(projectOptions.ApiKey, "X-Api-Key", ApiKeyAuthenticationProvider.KeyLocation.Header);
        // Create request adapter using the HttpClient-based implementation
        var adapter = new HttpClientRequestAdapter(authProvider){
            BaseUrl = projectOptions.BaseUrl
        };
        // Create the API client
        var client = new BomClient(adapter);

        try
        {
            var body = new MultipartBody();
            if (!string.IsNullOrEmpty(projectOptions.ProjectUUID)){
                body.AddOrReplacePart("project", "multipart/form-data", projectOptions.ProjectUUID);
            }
            else if(!string.IsNullOrEmpty(projectOptions.ProjectName) && !string.IsNullOrEmpty(projectOptions.ProjectVersion)){
                body.AddOrReplacePart("projectName", "multipart/form-data", projectOptions.ProjectName);
                body.AddOrReplacePart("projectVersion", "multipart/form-data", projectOptions.ProjectVersion);
            }
            else{

            }
            if (!string.IsNullOrEmpty(projectOptions.ProjectUUID)){
                body.AddOrReplacePart("parentUUID", "multipart/form-data", projectOptions.ParentUUID);
            }
            else if(!string.IsNullOrEmpty(projectOptions.ProjectName) && !string.IsNullOrEmpty(projectOptions.ProjectVersion)){
                body.AddOrReplacePart("parentName", "multipart/form-data", projectOptions.ParentName);
                body.AddOrReplacePart("parentVersion", "multipart/form-data", projectOptions.ParentVersion);
            }
            body.AddOrReplacePart("autoCreate", "multipart/form-data", projectOptions.AutoCreate);
            //body.AddOrReplacePart("bom", "multipart/form-data", );

            var uploadedBom = await client.V1.Bom.PostAsync(body);
            Console.WriteLine($"Uploaded BOM with token: {uploadedBom?.Token}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"ERROR: {ex.Message}");
            Console.WriteLine(ex.StackTrace);
        }
    }
}