using System;
using System.Web;
using System.Web.Mvc;
using NCD.Constants;

namespace NCD.Helpers {
    public static class HtmlHelpers {
        public static readonly int TruncateLength = 35;
        
        #region Base Urls
        /// <summary>
        /// Returns BaseUrl and slash.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static string GetBaseUrl(this HttpRequestBase request) {
            if (request.Url == null) {
                return "";
            }
            return request.Url.Scheme + "://" + request.Url.Authority + VirtualPathUtility.ToAbsolute("~/");
        }

        /// <summary>
        /// Returns BaseUrl and slash.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static string GetBaseUrl(this HttpContext context) {
            if (context != null) {
                var request = context.Request;
                return request.Url.Scheme + "://" + request.Url.Authority + VirtualPathUtility.ToAbsolute("~/");
            }
            return "";
        }

        /// <summary>
        /// Returns BaseUrl and slash.
        /// </summary>
        /// <param name="htmlhelper"></param>
        /// <returns></returns>
        public static string GetBaseUrl(this HtmlHelper htmlhelper) {
            return GetBaseUrl(HttpContext.Current);
        }
        /// <summary>
        /// Returns url without the host name. 
        /// Slash is included
        /// </summary>
        /// <param name="helper"></param>
        /// <returns>Returns url without the host name.</returns>
        public static string GetCurrentUrlString(this HtmlHelper helper) {
            return HttpContext.Current.Request.RawUrl;
        }
        /// <summary>
        /// Returns url whole page url with the host name. 
        /// </summary>
        /// <param name="helper"></param>
        /// <returns>Returns url whole page url with the host name. </returns>
        public static string GetCurrentUrlWithHostName(this HtmlHelper helper) {
            return GetBaseUrl(helper) + HttpContext.Current.Request.RawUrl;
        }
        #endregion

        #region Icons generate : badge

        public static HtmlString GetBadge(this HtmlHelper helper, long number) {
            var markup = string.Format(@"<span class='badge'>{0}</span>", number);

            return new HtmlString(markup);
        }

        #endregion

        #region Confirming Buttons
        /// <summary>
        /// Confirms before submit.
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="buttonName"></param>
        /// <param name="alertMessage"></param>
        /// <returns></returns>
        public static HtmlString ConfirmingSubmitButton(this HtmlHelper helper, string buttonName = "Save",
            string alertMessage = "Are you sure about this action?") {
            var sendbtn = String.Format(
                "<input type=\"submit\" value=\"{0}\" onClick=\"return confirm('{1}');\" />",
                buttonName, alertMessage);
            return new HtmlString(sendbtn);
        }

        /// <summary>
        /// Renders a submit button with icon.
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="buttonName"></param>
        /// <param name="alertMessage"></param>
        /// <param name="iconClass"></param>
        /// <param name="btnType"></param>
        /// <param name="placeIconLeft"></param>
        /// <param name="additionalClasses"></param>
        /// <returns></returns>
        public static HtmlString SubmitButton(this HtmlHelper helper, string buttonName = "Submit",
            string iconClass = "fa fa-floppy-o",
            string tooltip = "",
            string additionalClasses = "btn btn-success",
            bool placeIconLeft = true,
            string btnType = "Submit"
            ) {
            const string iconFormt = "<i class=\"{0}\"></i>";
            string leftIcon = "",
                   rightIcon = "";
            if (placeIconLeft) {
                leftIcon = string.Format(iconFormt, iconClass);
            } else {
                rightIcon = string.Format(iconFormt, iconClass);
            }
            var buttonHtml = String.Format(
                "<button type=\"{0}\"  title=\"{1}\" class=\"{2}\">{3} {4} {5}</button>",
                btnType, tooltip, additionalClasses, leftIcon, buttonName, rightIcon);
            return new HtmlString(buttonHtml);
        }

        public static HtmlString SubmitButtonIconRight(this HtmlHelper helper, string buttonName = "Submit",
           string iconClass = "fa fa-floppy-o",
           string tooltip = "",
           string additionalClasses = "btn btn-success",
           bool placeIconLeft = false,
           string btnType = "Submit"
           ) {
            return SubmitButton(helper, buttonName,
                iconClass,
                tooltip,
                additionalClasses,
                placeIconLeft,
                btnType);
        }

        public static HtmlString EmailButtonIconRight(this HtmlHelper helper, string buttonName = "Send",
          string iconClass = Icons.EmailO,
          string tooltip = "",
          string additionalClasses = "btn btn-success",
          bool placeIconLeft = false,
          string btnType = "Submit"
          ) {
            return SubmitButton(helper, buttonName,
                iconClass,
                tooltip,
                additionalClasses,
                placeIconLeft,
                btnType);
        }
        /// <summary>
        /// Renders a remove button with icon.
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="buttonName"></param>
        /// <param name="alertMessage"></param>
        /// <param name="iconClass"></param>
        /// <param name="btnType"></param>
        /// <param name="placeIconLeft"></param>
        /// <param name="additionalClasses"></param>
        /// <returns></returns>
        public static HtmlString RemoveButton(this HtmlHelper helper, string buttonName = "Delete",
            string iconClass = "fa fa-times",
            string tooltip = "",
            string btnType = "Submit",
            bool placeIconLeft = true,
            string additionalClasses = "btn btn-danger") {
            return SubmitButton(helper, buttonName, iconClass, tooltip, btnType, placeIconLeft, additionalClasses);
        }
        #endregion

        #region Truncates

        public static string Truncate(this HtmlHelper helper, string input, int? length, bool isShowElipseDot = true) {
            if (string.IsNullOrEmpty(input))
                return "";
            if (length == null) {
                length = TruncateLength;
            }
            if (input.Length <= length) {
                return input;
            }
            if (isShowElipseDot) {
                return input.Substring(0, (int)length) + "...";
            }
            return input.Substring(0, (int)length);
        }

        public static string Truncate(this HtmlHelper helper, string input, int starting, int length) {
            if (string.IsNullOrEmpty(input))
                return "";
            if (length == -1) {
                length = input.Length;
            }
            if (input.Length <= length) {
                length = input.Length;
            }
            length = length - starting;
            if (input.Length < starting) {
                return "";
            }
            return input.Substring(starting, length);
        }

        public static bool IsTruncateNeeded(this HtmlHelper helper, string input, int mid) {
            if (string.IsNullOrEmpty(input))
                return false;
            if (input.Length > mid) {
                return false;
            }
            return true;
        }

        #endregion

        #region Link Generates

        public static HtmlString SamePageLink(this HtmlHelper helper, string linkName, string title, bool h1 = true,
            string addClass = "") {
            //var area = HttpContext.Current.Request.RequestContext.RouteData.DataTokens["area"];
            //var controller = HttpContext.Current.Request.RequestContext.RouteData.Values["controller"];
            //var action = HttpContext.Current.Request.RequestContext.RouteData.Values["action"];
            return SamePageLinkWithIcon(helper, linkName, title, null, h1, addClass);
        }

        /// <summary>
        /// Generates same page url anchor with an icon left or right
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="linkName">Link display name</param>
        /// <param name="title">Link tooltip title</param>
        /// <param name="iconClass">Icon classes: Font-awesome icons or bootstrap icon classes</param>
        /// <param name="h1">Is nested inside a H1 element (W3c valid).</param>
        /// <param name="addClass">Anchor class</param>
        /// <param name="isLeft"></param>
        /// <returns></returns>
        public static HtmlString SamePageLinkWithIcon(this HtmlHelper helper,
            string linkName,
            string title,
            string iconClass,
            bool h1 = true,
            string addClass = "",
            bool isLeft = true) {
            var markup = "";
            var uri = HttpContext.Current.Request.RawUrl;
            uri = GetBaseUrl(helper) + uri;

            var icon = "";
            if (!string.IsNullOrEmpty(iconClass)) {
                icon = string.Format("<i class='{0}'></i>", iconClass);
            }
            if (isLeft) {
                //left icon
                markup = string.Format("<div class='header-margin-space-type-1'><a href='{0}' class='{1}' title='{2}'>{4} {3}</a></div>", uri, addClass, title, linkName, icon);
            } else {
                //right icon
                markup = string.Format("<div class='header-margin-space-type-1'><a href='{0}' class='{1}' title='{2}'>{3} {4}</a></div>", uri, addClass, title, linkName, icon);
            }
            if (h1) {
                markup = string.Format("<h1 title='{0}' class='h3'>{1}</h1>", title, markup);
            }
            return new HtmlString(markup);
        }

        public static HtmlString HeaderWithIcon(this HtmlHelper helper,
           string linkName,
           string title,
           string iconClass,
           bool h1 = true,
           string addClass = "",
           bool isLeft = true) {
            var markup = "";
            var icon = "";
            if (!string.IsNullOrEmpty(iconClass)) {
                icon = string.Format("<i class='{0}'></i>", iconClass);
            }
            if (isLeft) {
                //left icon
                markup = string.Format("<div class='header-margin-space-type-1'><a class='{1}' title='{2}'>{4} {3}</a></div>", "", addClass, title, linkName, icon);
            } else {
                //right icon
                markup = string.Format("<div class='header-margin-space-type-1'><a class='{1}' title='{2}'>{3} {4}</a></div>", "", addClass, title, linkName, icon);
            }
            if (h1) {
                markup = string.Format("<h1 title='{0}' class='h3'>{1}</h1>", title, markup);
            }
            return new HtmlString(markup);
        }

        /// <summary>
        /// Generates same page url anchor with an icon left or right
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="linkDisplayName">Link display name</param>
        /// <param name="routeValues"></param>
        /// <param name="title">Link tooltip title</param>
        /// <param name="iconClass">Icon classes: Font-awesome icons or bootstrap icon classes</param>
        /// <param name="h1">Is nested inside a H1 element (W3c valid).</param>
        /// <param name="addClass">Anchor class</param>
        /// <param name="isLeft"></param>
        /// <param name="actionName"></param>
        /// <param name="controllerName"></param>
        /// <returns></returns>
        public static HtmlString ActionLinkWithIcon(this HtmlHelper helper,
            string linkDisplayName,
            string actionName,
            string controllerName,
            object routeValues,
            string title,
            string iconClass,
            string addClass = "",
            bool h1 = false,
            bool isLeft = true) {
            var markup = "";
            string uri = "";
            var urlHelper = new UrlHelper(HttpContext.Current.Request.RequestContext);
            if (!string.IsNullOrWhiteSpace(controllerName)) {
                uri = urlHelper.Action(actionName, controllerName, routeValues);
            } else if (routeValues != null) {
                uri = urlHelper.Action(actionName, routeValues);
            } else {
                uri = urlHelper.Action(actionName);
            }
            uri = GetBaseUrl(HttpContext.Current) + uri;

            var icon = "";
            if (!string.IsNullOrEmpty(iconClass)) {
                icon = string.Format("<i class='{0}'></i>", iconClass);
            }
            if (isLeft) {
                //left icon
                markup = string.Format("<a href='{0}' class='{1}' title='{2}'>{4} {3}</a>", uri, addClass, title, linkDisplayName, icon);
            } else {
                //right icon
                markup = string.Format("<a href='{0}' class='{1}' title='{2}'>{3} {4}</a>", uri, addClass, title, linkDisplayName, icon);
            }
            if (h1) {
                markup = string.Format("<h1 title='{0}' class='h3'>{1}</h1>", title, markup);
            }
            return new HtmlString(markup);
        }


        /// <summary>
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="linkName">null gives the number on the display</param>
        /// <param name="title"></param>
        /// <param name="number"></param>
        /// <param name="h1"></param>
        /// <param name="addClass"></param>
        /// <returns></returns>
        public static HtmlString PhoneNumberLink(this HtmlHelper helper, long phonenumber, string title, bool h1 = false,
            string addClass = "") {
            var phone = "+" + phonenumber;

            var markup = string.Format("<a href='tel:{0}' class='{1}' title='{2}'>{3}</a>", phone, addClass, title,
                phone);

            if (h1) {
                markup = string.Format("<h1 title='{0}'>{1}</h1>", title, markup);
            }
            return new HtmlString(markup);
        }

        /// <summary>
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="linkName">null gives the number on the display</param>
        /// <param name="title"></param>
        /// <param name="number"></param>
        /// <param name="h1"></param>
        /// <param name="addClass"></param>
        /// <returns></returns>
        public static HtmlString PhoneNumberLink(this HtmlHelper helper, string phonenumber, string title,
            bool h1 = false, string addClass = "") {
            var phone = "+" + phonenumber;

            var markup = string.Format("<a href='tel:{0}' class='{1}' title='{2}'>{3}</a>", phone, addClass, title,
                phone);

            if (h1) {
                markup = string.Format("<h1 title='{0}'>{1}</h1>", title, markup);
            }
            return new HtmlString(markup);
        }

        public static HtmlString EmailLink(this HtmlHelper helper, string email, string title, bool h1 = false,
            string addClass = "") {
            var markup = string.Format("<a href='mailto:{0}' class='{1}' title='{2}'>{3}</a>", email, addClass, title,
                email);

            if (h1) {
                markup = string.Format("<h1 title='{0}'><strong title='" + title + "'>{1}</strong></h1>", title, markup);
            }
            return new HtmlString(markup);
        }

        #endregion

        #region Image Generates
        /// <summary>
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="src">use absolute http url for image src.</param>
        /// <param name="alt"></param>
        /// <returns></returns>
        public static HtmlString ImageFromAbsolutePath(this HtmlHelper helper, string src, string alt) {
            var markup = string.Format("<img src='{0}' alt='{1}'/>", src, alt);
            return new HtmlString(markup);
            //return (markup);
        }

        /// <summary>
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="src">relative url.</param>
        /// <param name="alt"></param>
        /// <returns></returns>
        public static HtmlString Image(this HtmlHelper helper, string src, string alt) {
            var markup = string.Format("<img src='{0}' alt='{1}'/>", VirtualPathUtility.ToAbsolute(src), alt);
            return new HtmlString(markup);
            //return (markup);
        }

        /// <summary>
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="folder"></param>
        /// <param name="src">relative from folder</param>
        /// <param name="ext"></param>
        /// <param name="alt"></param>
        /// <returns></returns>
        public static HtmlString Image(this HtmlHelper helper, string folder, string src, string ext, string alt) {
            var markup = string.Format("<img src='{0}{1}.{2}' alt='{3}'/>", VirtualPathUtility.ToAbsolute(folder), src,
                ext, alt);
            //return  new HtmlString(markup);
            return new HtmlString(markup);
        }

        #endregion

    }
}