using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace CleanArchitecture.Razor.Infrastructure.Constants.Permission
{
    public static class Permissions
    {

        [DisplayName("Products")]
        [Description("Products Permissions")]
        public static class Products
        {
            public const string View = "Permissions.Products.View";
            public const string Create = "Permissions.Products.Create";
            public const string Edit = "Permissions.Products.Edit";
            public const string Delete = "Permissions.Products.Delete";
            public const string Search = "Permissions.Products.Search";
            public const string Export = "Permissions.Products.Export";
            public const string Import = "Permissions.Products.Import";
        }
        [DisplayName("Customers")]
        [Description("Customers Permissions")]
        public static class Customers
        {
            public const string View = "Permissions.Customers.View";
            public const string Create = "Permissions.Customers.Create";
            public const string Edit = "Permissions.Customers.Edit";
            public const string Delete = "Permissions.Customers.Delete";
            public const string Search = "Permissions.Customers.Search";
            public const string Export = "Permissions.Customers.Export";
            public const string Import = "Permissions.Customers.Import";
        }

        [DisplayName("Categories")]
        [Description("Categories Permissions")]
        public static class Categories
        {
            public const string View = "Permissions.Categories.View";
            public const string Create = "Permissions.Categories.Create";
            public const string Edit = "Permissions.Categories.Edit";
            public const string Delete = "Permissions.Categories.Delete";
            public const string Search = "Permissions.Categories.Search";
            public const string Export = "Permissions.Categories.Export";
            public const string Import = "Permissions.Categories.Import";
        }

        [DisplayName("Documents")]
        [Description("Documents Permissions")]
        public static class Documents
        {
            public const string View = "Permissions.Documents.View";
            public const string Create = "Permissions.Documents.Create";
            public const string Edit = "Permissions.Documents.Edit";
            public const string Delete = "Permissions.Documents.Delete";
            public const string Search = "Permissions.Documents.Search";
        }
        [DisplayName("DocumentTypes")]
        [Description("DocumentTypes Permissions")]
        public static class DocumentTypes
        {
            public const string View = "Permissions.DocumentTypes.View";
            public const string Create = "Permissions.DocumentTypes.Create";
            public const string Edit = "Permissions.DocumentTypes.Edit";
            public const string Delete = "Permissions.DocumentTypes.Delete";
            public const string Search = "Permissions.DocumentTypes.Search";
        }


        [DisplayName("Users")]
        [Description("Users Permissions")]
        public static class Users
        {
            public const string View = "Permissions.Users.View";
            public const string Create = "Permissions.Users.Create";
            public const string Edit = "Permissions.Users.Edit";
            public const string Delete = "Permissions.Users.Delete";
            public const string Search = "Permissions.Users.Search";
            public const string Import = "Permissions.Users.Import";
            public const string ManageRoles = "Permissions.Users.ManageRoles";
        }

        [DisplayName("Roles")]
        [Description("Roles Permissions")]
        public static class Roles
        {
            public const string View = "Permissions.Roles.View";
            public const string Create = "Permissions.Roles.Create";
            public const string Edit = "Permissions.Roles.Edit";
            public const string Delete = "Permissions.Roles.Delete";
            public const string Search = "Permissions.Roles.Search";
            public const string Export = "Permissions.Roles.Export";
            public const string Import = "Permissions.Roles.Import";
            public const string ManagePermissions = "Permissions.Roles.Permissions";
        }

        [DisplayName("Role Claims")]
        [Description("Role Claims Permissions")]
        public static class RoleClaims
        {
            public const string View = "Permissions.RoleClaims.View";
            public const string Create = "Permissions.RoleClaims.Create";
            public const string Edit = "Permissions.RoleClaims.Edit";
            public const string Delete = "Permissions.RoleClaims.Delete";
            public const string Search = "Permissions.RoleClaims.Search";
        }



        [DisplayName("Dashboards")]
        [Description("Dashboards Permissions")]
        public static class Dashboards
        {
            public const string View = "Permissions.Dashboards.View";
        }

        [DisplayName("Hangfire")]
        [Description("Hangfire Permissions")]
        public static class Hangfire
        {
            public const string View = "Permissions.Hangfire.View";
        }


        /// <summary>
        /// Returns a list of Permissions.
        /// </summary>
        /// <returns></returns>
        public static List<string> GetRegisteredPermissions()
        {
            var permissions = new List<string>();
            foreach (var prop in typeof(Permissions).GetNestedTypes().SelectMany(c => c.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)))
            {
                var propertyValue = prop.GetValue(null);
                if (propertyValue is not null)
                    permissions.Add(propertyValue.ToString());
            }
            return permissions;
        }


    }
}
