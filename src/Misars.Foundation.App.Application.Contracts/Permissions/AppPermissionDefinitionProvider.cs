using Misars.Foundation.App.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;
using Volo.Abp.MultiTenancy;

namespace Misars.Foundation.App.Permissions;

public class AppPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var myGroup = context.AddGroup(AppPermissions.GroupName);

        myGroup.AddPermission(AppPermissions.Dashboard.Host, L("Permission:Dashboard"), MultiTenancySides.Host);
        myGroup.AddPermission(AppPermissions.Dashboard.Tenant, L("Permission:Dashboard"), MultiTenancySides.Tenant);

        //Define your own permissions here. Example:
        //myGroup.AddPermission(AppPermissions.MyPermission1, L("Permission:MyPermission1"));

        var patientPermission = myGroup.AddPermission(AppPermissions.Patients.Default, L("Permission:Patients"));
        patientPermission.AddChild(AppPermissions.Patients.Create, L("Permission:Create"));
        patientPermission.AddChild(AppPermissions.Patients.Edit, L("Permission:Edit"));
        patientPermission.AddChild(AppPermissions.Patients.Delete, L("Permission:Delete"));

        var doctorPermission = myGroup.AddPermission(AppPermissions.Doctors.Default, L("Permission:Doctors"));
        doctorPermission.AddChild(AppPermissions.Doctors.Create, L("Permission:Create"));
        doctorPermission.AddChild(AppPermissions.Doctors.Edit, L("Permission:Edit"));
        doctorPermission.AddChild(AppPermissions.Doctors.Delete, L("Permission:Delete"));

        var surgeryTimetablePermission = myGroup.AddPermission(AppPermissions.SurgeryTimetables.Default, L("Permission:SurgeryTimetables"));
        surgeryTimetablePermission.AddChild(AppPermissions.SurgeryTimetables.Create, L("Permission:Create"));
        surgeryTimetablePermission.AddChild(AppPermissions.SurgeryTimetables.Edit, L("Permission:Edit"));
        surgeryTimetablePermission.AddChild(AppPermissions.SurgeryTimetables.Delete, L("Permission:Delete"));

        var filemanagementPermission = myGroup.AddPermission(AppPermissions.Filemanagements.Default, L("Permission:Filemanagements"));
        filemanagementPermission.AddChild(AppPermissions.Filemanagements.Create, L("Permission:Create"));
        filemanagementPermission.AddChild(AppPermissions.Filemanagements.Edit, L("Permission:Edit"));
        filemanagementPermission.AddChild(AppPermissions.Filemanagements.Delete, L("Permission:Delete"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<AppResource>(name);
    }
}