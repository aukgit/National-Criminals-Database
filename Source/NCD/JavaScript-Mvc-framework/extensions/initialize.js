; $.app = $.app || {};
$.app.initialize = function () {
    /// <summary>
    /// Run all modules.
    /// </summary>
    
    var app = $.app;
    app.initHiddenContainer();
    // run controller module
    app.controllers.initialize(); // runs all controllers modules.

    var $tables = $(".bootstrap-table-convert");
    if ($tables.length > 0) {
        $tables.bootstrapTable();
    }
};
