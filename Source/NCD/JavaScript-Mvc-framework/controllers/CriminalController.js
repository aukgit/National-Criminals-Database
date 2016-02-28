$.app.controllers = $.app.controllers || {};
$.app.controllers.helloController = {
    // any thing related to controllers.
    pageId: "criminal-controller",
    $pageElement: null,
    initialize: function () {
        var controllers = $.app.controllers,
            current = controllers.helloController;
        if (controllers.isCurrentPage(current)) {
            controllers.execute(current);
        }
    },
    isDebugging: true,
    actions: {
        /// <summary>
        /// Represents the collection of actions exist inside a controller.
        /// </summary>
        confirmation: function () {
            /// <summary>
            /// Represents Confirmation action page.
            /// Refers to the data-action attribute.
            /// </summary>
            /// <returns type=""></returns>
            var self = $.app.controllers.helloController,
                $page = self.$pageElement,
                url = $page.attr("data-request-url"),
                requestCacheToken = $page.attr("data-cache-token"),
                $antiToken = $.byId("anti-token"),
                antiTokenField = $antiToken.find("[name='__RequestVerificationToken']"),
                antiForgeryTokenValue = antiTokenField.val(),
                $headerTitle = $.byId("header-title"),
                $icon = $headerTitle.find(".icon"),
                $message = $headerTitle.find(".message"),
                $hidingLabel = $.byId("hiding-label"),
                $conditionalButtonToSearchAgain = $.byId("conditional-search");

            var jsonData = {
                token: requestCacheToken,
                __RequestVerificationToken: antiForgeryTokenValue
            };
            var isInTestingMode = false;
            jQuery.ajax({
                method: "POST", 
                url: url,
                data: jsonData,
                dataType: "JSON",
            }).done(function (response) {
                if (isInTestingMode) {
                    console.log(response);
                }
                var isFound = response.found,
                    message = response.message;
            
                $hidingLabel.addClass("animated")
                            .addClass("fadeOut")
                            .delay(500)
                            .addClass("hide");
                if (isFound) {
                    // email is sent successfully.
                    $conditionalButtonToSearchAgain
                        .removeClass("hide")
                        .addClass("animated")
                        .addClass("fadeIn");
                    $icon.replaceWith("<i class='fa fa-check green'></i>");
                    $message.addClass("green")
                            .html(message);
                } else {
                    $icon.replaceWith("<i class='fa fa-times red'></i>");
                    $message.addClass("red")
                            .html(message);
                }
            }).fail(function (jqXHR, textStatus, exceptionMessage) {
                console.log("Request failed: " + exceptionMessage);
                $icon.replaceWith("<i class='fa fa-times red'></i>");
                $message.addClass("red")
                        .addClass("f-em-d-7")
                        .html("Sorry! Your request is failed to send email. Try again.");
            }).always(function () {
                //console.log("complete");
            });
        },
    },
    bindEvents: {
        /// <summary>
        /// Events which needs to be bound at runtime or anytime.
        /// </summary>
    },
    elements: {
        /// <summary>
        /// Write functions to get elements from DOM.
        /// </summary>
    },


    render: {
        /// <summary>
        /// Write functions to render elements in the DOM.
        /// </summary>
    },
    // related to filtering and url retrieval
    urls: {
        /// <summary>
        /// Write methods to generate urls
        /// </summary>
    }
}