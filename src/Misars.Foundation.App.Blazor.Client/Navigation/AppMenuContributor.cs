using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Misars.Foundation.App.Localization;
using Misars.Foundation.App.Permissions;
using Volo.Abp.Account.Localization;
using Volo.Abp.AuditLogging.Blazor.Menus;
using Volo.Abp.Identity.Pro.Blazor.Navigation;
using Volo.Abp.LanguageManagement.Blazor.Menus;
using Volo.Abp.SettingManagement.Blazor.Menus;
using Volo.Abp.TextTemplateManagement.Blazor.Menus;
using Volo.Abp.UI.Navigation;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.OpenIddict.Pro.Blazor.Menus;
using Volo.Saas.Host.Blazor.Navigation;

namespace Misars.Foundation.App.Blazor.Client.Navigation;

public class AppMenuContributor : IMenuContributor
{
    private readonly IConfiguration _configuration;

    public AppMenuContributor(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task ConfigureMenuAsync(MenuConfigurationContext context)
    {
        if (context.Menu.Name == StandardMenus.Main)
        {
            await ConfigureMainMenuAsync(context);
        }
        else if (context.Menu.Name == StandardMenus.User)
        {
            await ConfigureUserMenuAsync(context);
        }
    }

    private static Task ConfigureMainMenuAsync(MenuConfigurationContext context)
    {
        var l = context.GetLocalizer<AppResource>();

        context.Menu.AddItem(new ApplicationMenuItem(
            AppMenus.Home,
            l["Menu:Home"],
            "/",
            icon: "fas fa-home",
            order: 1
        ));

        //HostDashboard
        context.Menu.AddItem(
            new ApplicationMenuItem(
                AppMenus.HostDashboard,
                l["Menu:Dashboard"],
                "/HostDashboard",
                icon: "fa fa-chart-line",
                order: 2
            ).RequirePermissions(AppPermissions.Dashboard.Host)
        );

        //TenantDashboard
        context.Menu.AddItem(
            new ApplicationMenuItem(
                AppMenus.TenantDashboard,
                l["Menu:Dashboard"],
                "/Dashboard",
                icon: "fa fa-chart-line",
                order: 2
            ).RequirePermissions(AppPermissions.Dashboard.Tenant)
        );

        /* Example nested menu definition:

        context.Menu.AddItem(
            new ApplicationMenuItem("Menu0", "Menu Level 0")
            .AddItem(new ApplicationMenuItem("Menu0.1", "Menu Level 0.1", url: "/test01"))
            .AddItem(
                new ApplicationMenuItem("Menu0.2", "Menu Level 0.2")
                    .AddItem(new ApplicationMenuItem("Menu0.2.1", "Menu Level 0.2.1", url: "/test021"))
                    .AddItem(new ApplicationMenuItem("Menu0.2.2", "Menu Level 0.2.2")
                        .AddItem(new ApplicationMenuItem("Menu0.2.2.1", "Menu Level 0.2.2.1", "/test0221"))
                        .AddItem(new ApplicationMenuItem("Menu0.2.2.2", "Menu Level 0.2.2.2", "/test0222"))
                    )
                    .AddItem(new ApplicationMenuItem("Menu0.2.3", "Menu Level 0.2.3", url: "/test023"))
                    .AddItem(new ApplicationMenuItem("Menu0.2.4", "Menu Level 0.2.4", url: "/test024")
                        .AddItem(new ApplicationMenuItem("Menu0.2.4.1", "Menu Level 0.2.4.1", "/test0241"))
                )
                .AddItem(new ApplicationMenuItem("Menu0.2.5", "Menu Level 0.2.5", url: "/test025"))
            )
            .AddItem(new ApplicationMenuItem("Menu0.2", "Menu Level 0.2", url: "/test02"))
        );

        */

        //Administration
        var administration = context.Menu.GetAdministration();
        administration.Order = 5;

        //Administration->Identity
        administration.SetSubItemOrder(IdentityProMenus.GroupName, 1);

        //Administration->Saas
        administration.SetSubItemOrder(SaasHostMenus.GroupName, 2);

        //Administration->OpenIddict
        administration.SetSubItemOrder(OpenIddictProMenus.GroupName, 3);

        //Administration->Language Management
        administration.SetSubItemOrder(LanguageManagementMenus.GroupName, 5);

        //Administration->Text Template Management
        administration.SetSubItemOrder(TextTemplateManagementMenus.GroupName, 6);

        //Administration->Audit Logs
        administration.SetSubItemOrder(AbpAuditLoggingMenus.GroupName, 7);

        //Administration->Settings
        administration.SetSubItemOrder(SettingManagementMenus.GroupName, 8);

        context.Menu.AddItem(
            new ApplicationMenuItem(
                AppMenus.Patients,
                l["Menu:Patients"],
                url: "/patients",
icon: "fa fa-file-alt",
                requiredPermissionName: AppPermissions.Patients.Default)
        );

        context.Menu.AddItem(
            new ApplicationMenuItem(
                AppMenus.Doctors,
                l["Menu:Doctors"],
                url: "/doctors",
icon: "fa fa-file-alt",
                requiredPermissionName: AppPermissions.Doctors.Default)
        );

        context.Menu.AddItem(
            new ApplicationMenuItem(
                AppMenus.SurgeryTimetables,
                l["Menu:SurgeryTimetables"],
                url: "/surgery-timetables",
icon: "fa fa-file-alt",
                requiredPermissionName: AppPermissions.SurgeryTimetables.Default)
        );

        context.Menu.AddItem(
            new ApplicationMenuItem(
                AppMenus.Filemanagements,
                l["Menu:Filemanagements"],
                url: "/filemanagements",
                icon: "fa fa-file-alt",
                requiredPermissionName: AppPermissions.Filemanagements.Default)
        );
        return Task.CompletedTask;
    }

    private async Task ConfigureUserMenuAsync(MenuConfigurationContext context)
    {
        var accountStringLocalizer = context.GetLocalizer<AccountResource>();
        var authServerUrl = _configuration["AuthServer:Authority"] ?? "";

        context.Menu.AddItem(new ApplicationMenuItem(
            "Account.Manage",
            accountStringLocalizer["MyAccount"],
            $"{authServerUrl.EnsureEndsWith('/')}Account/Manage",
            icon: "fa fa-cog",
            order: 1000,
            target: "_blank").RequireAuthenticated());

        context.Menu.AddItem(new ApplicationMenuItem(
            "Account.SecurityLogs",
            accountStringLocalizer["MySecurityLogs"],
            $"{authServerUrl.EnsureEndsWith('/')}Account/SecurityLogs",
            icon: "fa fa-user-shield",
            order: 1001,
            target: "_blank").RequireAuthenticated());

        context.Menu.AddItem(new ApplicationMenuItem(
            "Account.Sessions",
            accountStringLocalizer["Sessions"],
            url: $"{authServerUrl.EnsureEndsWith('/')}Account/Sessions",
            icon: "fa fa-clock",
            order: 1002,
            target: "_blank").RequireAuthenticated());

        await Task.CompletedTask;
    }
}