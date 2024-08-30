using System;
using System.Net.Http;
using Blazorise.Bootstrap5;
using Blazorise.Icons.FontAwesome;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Misars.Foundation.App.Blazor.Client.Navigation;
using OpenIddict.Abstractions;
using Volo.Abp.Account.Pro.Admin.Blazor.WebAssembly;
using Volo.Abp.AspNetCore.Components.Web.Theming.Routing;
using Volo.Abp.AspNetCore.Components.WebAssembly.LeptonXTheme;
using Volo.Abp.AuditLogging.Blazor.WebAssembly;
using Volo.Abp.Autofac.WebAssembly;
using Volo.Abp.AutoMapper;
using Volo.Abp.Gdpr.Blazor.Extensions;
using Volo.Abp.Gdpr.Blazor.WebAssembly;
using Volo.Abp.Identity.Pro.Blazor.Server.WebAssembly;
using Volo.Abp.Modularity;
using Volo.Abp.UI.Navigation;
using Volo.Abp.LanguageManagement.Blazor.WebAssembly;
using Volo.Abp.LeptonX.Shared;
using Volo.Abp.AspNetCore.Components.Web.LeptonXTheme;
using Volo.Abp.OpenIddict.Pro.Blazor.WebAssembly;
using Volo.Abp.SettingManagement.Blazor.WebAssembly;
using Volo.Abp.TextTemplateManagement.Blazor.WebAssembly;
using Volo.Saas.Host.Blazor.WebAssembly;
using Volo.FileManagement.Blazor.WebAssembly;


namespace Misars.Foundation.App.Blazor.Client;

[DependsOn(
    typeof(AbpAccountAdminBlazorWebAssemblyModule),
    typeof(AbpAspNetCoreComponentsWebAssemblyLeptonXThemeModule),
    typeof(AbpAuditLoggingBlazorWebAssemblyModule),
    typeof(AbpAutofacWebAssemblyModule),
    typeof(AbpGdprBlazorWebAssemblyModule),
    typeof(AbpIdentityProBlazorWebAssemblyModule),
    typeof(AbpOpenIddictProBlazorWebAssemblyModule),
    typeof(AbpSettingManagementBlazorWebAssemblyModule),
    typeof(LanguageManagementBlazorWebAssemblyModule),
    typeof(AppHttpApiClientModule),
    typeof(SaasHostBlazorWebAssemblyModule),
    typeof(TextTemplateManagementBlazorWebAssemblyModule)
)]
[DependsOn(typeof(FileManagementBlazorWebAssemblyModule))]
    public class AppBlazorClientModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        var environment = context.Services.GetSingletonInstance<IWebAssemblyHostEnvironment>();
        var builder = context.Services.GetSingletonInstance<WebAssemblyHostBuilder>();

        ConfigureAuthentication(builder);
        ConfigureHttpClient(context, environment);
        ConfigureBlazorise(context);
        ConfigureRouter(context);
        ConfigureMenu(context);
        ConfigureAutoMapper(context);
        ConfigureCookieConsent(context);
        ConfigureTheme();
    }
    
    private void ConfigureCookieConsent(ServiceConfigurationContext context)
    {
        context.Services.AddAbpCookieConsent(options =>
        {
            options.IsEnabled = true;
            options.CookiePolicyUrl = "/CookiePolicy";
            options.PrivacyPolicyUrl = "/PrivacyPolicy";
        });
    }

    private void ConfigureTheme()
    {
        Configure<LeptonXThemeOptions>(options =>
        {
            options.DefaultStyle = LeptonXStyleNames.System;
        });

        Configure<LeptonXThemeBlazorOptions>(options =>
        {
            // When this is changed, `AbpCli:Bundle:LeptonXTheme.Layout` parameter with value 'top-menu' should be added into appsettings.json,
            // Then `abp bundle` command should be executed to apply the changes.
            options.Layout = LeptonXBlazorLayouts.SideMenu;
        });
    }

    private void ConfigureRouter(ServiceConfigurationContext context)
    {
        Configure<AbpRouterOptions>(options =>
        {
            options.AppAssembly = typeof(AppBlazorClientModule).Assembly;
        });
    }

    private void ConfigureMenu(ServiceConfigurationContext context)
    {
        Configure<AbpNavigationOptions>(options =>
        {
            options.MenuContributors.Add(new AppMenuContributor(context.Services.GetConfiguration()));
        });
    }

    private void ConfigureBlazorise(ServiceConfigurationContext context)
    {
        context.Services
            .AddBootstrap5Providers()
            .AddFontAwesomeIcons();
    }

    private static void ConfigureAuthentication(WebAssemblyHostBuilder builder)
    {
        builder.Services.AddOidcAuthentication(options =>
        {
            builder.Configuration.Bind("AuthServer", options.ProviderOptions);
            options.UserOptions.NameClaim = OpenIddictConstants.Claims.Name;
            options.UserOptions.RoleClaim = OpenIddictConstants.Claims.Role;

            options.ProviderOptions.DefaultScopes.Add("App");
            options.ProviderOptions.DefaultScopes.Add("roles");
            options.ProviderOptions.DefaultScopes.Add("email");
            options.ProviderOptions.DefaultScopes.Add("phone");
            options.ProviderOptions.DefaultScopes.Add("offline_access");
        });
    }
    
    private static void ConfigureHttpClient(ServiceConfigurationContext context, IWebAssemblyHostEnvironment environment)
    {
        context.Services.AddTransient(sp => new HttpClient
        {
            BaseAddress = new Uri(environment.BaseAddress)
        });
    }

    private void ConfigureAutoMapper(ServiceConfigurationContext context)
    {
        Configure<AbpAutoMapperOptions>(options =>
        {
            options.AddMaps<AppBlazorClientModule>();
        });
    }
}
