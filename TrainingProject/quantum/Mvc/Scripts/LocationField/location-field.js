jQuery(document).ready(function ($) {
    if (typeof FormRulesSettings !== "undefined") {
        FormRulesSettings.addFieldSelector("location-field-container", "[data-sf-role='location-field-input']", ":checked");
        var container = $('[data-sf-role="location-field-container"]');
        var attachHandlers = function (input) {
            input.on('change', function (e) {
                if (typeof $.fn.processFormRules == 'function')
                    $(e.target).processFormRules();
            });
        };

        var inputs = container.find('[data-sf-role="location-field-input"]');
        attachHandlers(inputs);
    }
});