// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using CleanArchitecture.Razor.Application.Common.Extensions;
using CleanArchitecture.Razor.Infrastructure.Constants.ClaimTypes;
using CleanArchitecture.Razor.Application.Constants.Permission;
using CleanArchitecture.Razor.Infrastructure.Constants.Role;
using CleanArchitecture.Razor.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using System.Reflection;

namespace CleanArchitecture.Razor.Infrastructure.Persistence;

public static class ApplicationDbContextSeed
{
    public static async Task SeedDefaultUserAsync(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager)
    {
        var administratorRole = new ApplicationRole(RoleConstants.AdministratorRole) { Description = "Admin Group" };
        var userRole = new ApplicationRole(RoleConstants.BasicRole) { Description = "Basic Group" };

        if (roleManager.Roles.All(r => r.Name != administratorRole.Name))
        {
            await roleManager.CreateAsync(administratorRole);
            await roleManager.CreateAsync(userRole);
            var Permissions = GetAllPermissions();
            foreach (var permission in Permissions)
            {
                await roleManager.AddClaimAsync(administratorRole, new System.Security.Claims.Claim(ApplicationClaimTypes.Permission, permission));
            }
        }

        var administrator = new ApplicationUser { UserName = "administrator", IsActive = true, Site = "Razor", DisplayName = "Administrator", Email = "new163@163.com", EmailConfirmed = true, ProfilePictureDataUrl = $"https://cn.gravatar.com/avatar/{"new163@163.com".ToMD5()}?s=120&d=retro" };
        var demo = new ApplicationUser { UserName = "Demo", IsActive = true, Site = "Razor", DisplayName = "Demo", Email = "neozhu@126.com", EmailConfirmed = true, ProfilePictureDataUrl = $"https://cn.gravatar.com/avatar/{"neozhu@126.com".ToMD5()}?s=120&d=retro" };

        if (userManager.Users.All(u => u.UserName != administrator.UserName))
        {
            await userManager.CreateAsync(administrator, RoleConstants.DefaultPassword);
            await userManager.AddToRolesAsync(administrator, new[] { administratorRole.Name });
            await userManager.CreateAsync(demo, RoleConstants.DefaultPassword);
            await userManager.AddToRolesAsync(demo, new[] { userRole.Name });


        }

    }
    private static IEnumerable<string> GetAllPermissions()
    {
        var allPermissions = new List<string>();
        var modules = typeof(Permissions).GetNestedTypes();

        foreach (var module in modules)
        {
            var moduleName = string.Empty;
            var moduleDescription = string.Empty;

            var fields = module.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);

            foreach (var fi in fields)
            {
                var propertyValue = fi.GetValue(null);

                if (propertyValue is not null)
                    allPermissions.Add(propertyValue.ToString());
            }
        }

        return allPermissions;
    }

    public static async Task SeedSampleDataAsync(ApplicationDbContext context)
    {
        //Seed, if necessary
        if (!context.MigrationProjects.Any())
        {
            context.MigrationProjects.Add(new Domain.Entities.MigrationProject() { Name = "MOVE-VT-PILOT", Status="Ongoing", Description = "MOVE-VT", BeginDateTime=DateTime.Now, Progress=0 });
            await context.SaveChangesAsync();

        }

        if (!context.MigrationObjects.Any())
        {
            //context.MigrationObjects.Add(new Domain.Entities.MigrationObject() { Name = "BOM", Description = "BOM" });
            //context.MigrationObjects.Add(new Domain.Entities.MigrationObject() { Name = "Customer", Description = "Customer" });
            //context.MigrationObjects.Add(new Domain.Entities.MigrationObject() { Name = "Supplier", Description = "Supplier" });
            //context.MigrationObjects.Add(new Domain.Entities.MigrationObject() { Name = "Production", Description = "Production" });
            //await context.SaveChangesAsync();

        }

        if (!context.DocumentTypes.Any())
        {
            context.DocumentTypes.Add(new Domain.Entities.DocumentType() { Name = "Document", Description = "Document" });
            context.DocumentTypes.Add(new Domain.Entities.DocumentType() { Name = "PDF", Description = "PDF" });
            context.DocumentTypes.Add(new Domain.Entities.DocumentType() { Name = "Image", Description = "Image" });
            context.DocumentTypes.Add(new Domain.Entities.DocumentType() { Name = "Other", Description = "Other" });
            await context.SaveChangesAsync();
      
        }
        if (!context.KeyValues.Any())
        {
            context.KeyValues.Add(new Domain.Entities.KeyValue() { Name = "LegacySystem", Value = "AP1", Text = "AP1", Description = "Legacy System" });
            context.KeyValues.Add(new Domain.Entities.KeyValue() { Name = "LegacySystem", Value = "NAVISION", Text = "NAVISION", Description = "Legacy System" });
            context.KeyValues.Add(new Domain.Entities.KeyValue() { Name = "Team", Value = "SD", Text = "SD", Description = "Team Name" });
            context.KeyValues.Add(new Domain.Entities.KeyValue() { Name = "Team", Value = "PP", Text = "PP", Description = "Team Name" });
            context.KeyValues.Add(new Domain.Entities.KeyValue() { Name = "Team", Value = "MM", Text = "MM", Description = "Team Name" });
            context.KeyValues.Add(new Domain.Entities.KeyValue() { Name = "Team", Value = "FI", Text = "FI", Description = "Team Name" });
            await context.SaveChangesAsync();

     
        }
        if (!context.Customers.Any())
        {
            context.Customers.Add(new Domain.Entities.Customer() { Name = "SmartAdmin", AddressOfEnglish = "https://wrapbootstrap.com/theme/smartadmin-responsive-webapp-WB0573SK0", GroupName = "SmartAdmin", Address = "https://wrapbootstrap.com/theme/smartadmin-responsive-webapp-WB0573SK0", Sales = "GotBootstrap", RegionSalesDirector = "GotBootstrap", Region = "CNC", NameOfEnglish = "SmartAdmin", PartnerType = Domain.Enums.PartnerType.TP, Contact = "GotBootstrap", Email = "drlantern@gotbootstrap.com" });
            await context.SaveChangesAsync();
            context.Loggers.Add(new Domain.Entities.Log.Logger() { Message = "Initial add customer", Level = "Information", UserName = "System", TimeStamp = System.DateTime.Now });
            context.Loggers.Add(new Domain.Entities.Log.Logger() { Message = "Debug", Level = "Debug", UserName = "System", TimeStamp = System.DateTime.Now.AddHours(-1) });
            context.Loggers.Add(new Domain.Entities.Log.Logger() { Message = "Error", Level = "Error", UserName = "System", TimeStamp = System.DateTime.Now.AddHours(-1) });
            context.Loggers.Add(new Domain.Entities.Log.Logger() { Message = "Warning", Level = "Warning", UserName = "System", TimeStamp = System.DateTime.Now.AddHours(-2) });
            context.Loggers.Add(new Domain.Entities.Log.Logger() { Message = "Trace", Level = "Trace", UserName = "System", TimeStamp = System.DateTime.Now.AddHours(-4) });
            context.Loggers.Add(new Domain.Entities.Log.Logger() { Message = "Fatal", Level = "Fatal", UserName = "System", TimeStamp = System.DateTime.Now.AddHours(-4) });
            await context.SaveChangesAsync();
        }
    }
}
