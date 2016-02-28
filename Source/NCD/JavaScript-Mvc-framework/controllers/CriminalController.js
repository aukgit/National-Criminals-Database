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
        index : function () {
            /// <summary>
            /// Represents index action page.
            /// Refers to the data-action attribute.
            /// </summary>
            /// <returns type=""></returns>
            var self = $.app.controllers.helloController;
            console.log($.getHiddenValue("Hello"));
            console.log("Index action ran.");
            self.$pageElement.append($.getHiddenValue("Hello"));
        },

        contact: function () {
            /// <summary>
            /// Represents contact action page.
            /// Refers to the data-action attribute.
            /// </summary>
            /// <returns type=""></returns>
            var self = $.app.controllers.helloController;
            console.log("Contact action ran.");
            console.log($.getHiddenValue("Hello"));
            console.log($.getHiddenField("Hello").length);
            self.$pageElement.append($.getHiddenValue("Hello"));
        }
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