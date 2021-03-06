﻿using System.Web.Optimization;
namespace NCD {
    public class BundleConfig {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles) {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                "~/Scripts/jquery.validate*",
                "~/Scripts/mvcfoolproof.unobtrusive.js"
                ));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                "~/Scripts/bootstrap.js",
                "~/Scripts/respond.js",
                "~/Scripts/bootstrap-table.js",
                "~/Scripts/bootstrap-table-filter.js",
                "~/Scripts/bootstrap-table-export.js",
                "~/JavaScript-Mvc-framework/byId.js",
                "~/JavaScript-Mvc-framework/app.js",
                "~/JavaScript-Mvc-framework/extensions/initialize.js",
                "~/JavaScript-Mvc-framework/controllers/controllers.js",
                "~/JavaScript-Mvc-framework/controllers/CriminalController.js",
                "~/JavaScript-Mvc-framework/controllers/controllers.initialize.js",
                "~/JavaScript-Mvc-framework/jQueryExtend.js",
                "~/JavaScript-Mvc-framework/app.js",
                "~/JavaScript-Mvc-framework/app.run.js"
                ));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                "~/Content/css/style.css",
                "~/Content/bootstrap.css",
                "~/Content/css/bootstrap-table.css",
                "~/Content/css/media.css",
                "~/Content/css/header.css",
                "~/Content/css/font-awesome.min.css",
                "~/Content/css/additional-styles.css", // additional css added which will be converted from .less files 
                "~/Content/css/animate.css",
                "~/Content/css/animate-refresh.css",
                "~/Content/css/color-fonts.css",
                "~/Content/css/utilities.css"
                )); 
        }
    }
}