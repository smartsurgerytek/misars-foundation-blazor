using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Account;
using Volo.Abp.AuditLogging;
using Volo.Abp.FeatureManagement;
using Volo.Abp.Identity;
using Volo.Abp.OpenIddict;
using Volo.Abp.LanguageManagement;
using Volo.Abp.Modularity;
using Volo.Abp.PermissionManagement;
using Volo.Abp.TextTemplateManagement;
using Volo.Abp.SettingManagement;
using Volo.Saas.Host;
using Volo.Abp.VirtualFileSystem;
using Volo.Abp.Gdpr;
using Volo.FileManagement;
using Volo.CmsKit;

namespace Misars.Foundation.App;

[DependsOn(
    typeof(AppApplicationContractsModule),
    typeof(AbpIdentityHttpApiClientModule),
    typeof(AbpPermissionManagementHttpApiClientModule),
    typeof(AbpFeatureManagementHttpApiClientModule),
    typeof(AbpSettingManagementHttpApiClientModule),
    typeof(SaasHostHttpApiClientModule),
    typeof(AbpAuditLoggingHttpApiClientModule),
    typeof(AbpOpenIddictProHttpApiClientModule),
    typeof(AbpAccountAdminHttpApiClientModule),
    typeof(AbpAccountPublicHttpApiClientModule),
    typeof(LanguageManagementHttpApiClientModule),
    typeof(AbpGdprHttpApiClientModule),
    typeof(TextTemplateManagementHttpApiClientModule)
)]
[DependsOn(typeof(FileManagementHttpApiClientModule))]
    [DependsOn(typeof(CmsKitProAdminHttpApiClientModule))]
    [DependsOn(typeof(CmsKitProHttpApiClientModule))]
    public class AppHttpApiClientModule : AbpModule
{
    public const string RemoteServiceName = "Default";

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddHttpClientProxies(
            typeof(AppApplicationContractsModule).Assembly,
            RemoteServiceName
        );

        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<AppHttpApiClientModule>();
        });
    }
}
