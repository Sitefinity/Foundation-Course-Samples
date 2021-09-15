import { Injectable, ClassProvider } from "@angular/core";
import { ToolBarItem, EditorConfigProvider, EDITOR_CONFIG_TOKEN } from "@progress/sitefinity-adminapp-sdk/app/api/v1";
import { InsertSymbolGenerator, DATA_ATTRIBUTE_NAME } from "./symbol-list/insert-symbol-generator";
import { ToolBuilder, ToolConfig } from "../../helpers/tool-builder";

const TOOLBAR_BUTTON_DATA = {
    DEFAULT: {
        name: "insertbootstrapicon",
        tooltip: "Insert Bootstrap icon"
    }
};

declare var kendo;

require("!style-loader!css-loader!./insert-bootstrap-icon.provider.css");
import symbolList from "./symbol-list/symbol-list.json";

@Injectable()
class InsertBootstrapIconProvider implements EditorConfigProvider {
    /**
     * If you want to remove some toolbar items return their names as strings in the array. Order is insignificant.
     * Otherwise return an empty array.
     * Example: return [ "embed" ];
     * The above code will remove the embed toolbar item from the editor.
     * Documentation where you can find all tools' names: https://docs.telerik.com/kendo-ui/api/javascript/ui/editor/configuration/tools
     *
     * @returns {string[]}
     * @memberof InsertBootstrapIconProvider
     */
    getToolBarItemsNamesToRemove(): string[] {
        return [];
    }

    /**
     * This gives access to the Kendo UI Editor configuration object
     * that is used to initialize the editor upon creation
     * Kendo UI Editor configuration overview documentation -> https://docs.telerik.com/kendo-ui/controls/editors/editor/overview#configuration
     *
     * @param {*} configuration
     * @returns The modified configuration.
     * @memberof InsertBootstrapIconProvider
     */
    configureEditor(configuration: any) {
        // configuration.stylesheets[0] = [
        //     "../ResourcePackages/Bootstrap4/assets/dist/bootstrap-icons/bootstrap-icons.css"
        // ]
        return configuration;
    }

    /**
     * The method that gets invoked when the editor constructs the toolbar actions.
     *
     * @param {*} editorHost The Kendo's editor object.
     * @returns {ToolBarItem[]} The custom toolbar items that will be added to the Kendo's toolbar.
     * @memberof InsertBootstrapIconProvider
     */
    getToolBarItems(editorHost: any): ToolBarItem[] {
        /**
         * The custom tool.
         */
        const DEFAULT_TOOL: ToolBarItem = {
            name: TOOLBAR_BUTTON_DATA.DEFAULT.name,
            tooltip: TOOLBAR_BUTTON_DATA.DEFAULT.tooltip,
            ordinal: 31,
            exec: () => { return; }
        };

        this.configureInsertBootstrapIconTool();
        return [DEFAULT_TOOL];
    }

    private configureInsertBootstrapIconTool() {
        const buttonTemplate = kendo.ui.editor.EditorUtils.buttonTemplate;
        const NS = "kendoEditor";

        const popupTemplateGenerator = function () {
            const symbolGenerator = new InsertSymbolGenerator(Object.keys(symbolList).map(k => symbolList[k]));
            const generatedHtml = symbolGenerator.generateHtml();
            return `<div class='k-ct-popup symbol-popup'><div class='k-status symbol-title'>INSERT BOOTSTRAP ICON</div>${generatedHtml}</div>`;
        };

        const config: ToolConfig = {
            _activate: function () {
                let that = this;
                const element = that.popup().element;

                element.autoApplyNS(NS).on("down", function (e) {
                    e.preventDefault();
                    const symbolCell = e.target.closest(".symbol-cell");
                    if(symbolCell){
                        const symbolValue = symbolCell.getAttribute(DATA_ATTRIBUTE_NAME);

                        var spanString = "<span class='bi " + symbolValue + "'>&nbsp;</span>&nbsp;";
                    
                        that._exec(spanString);
                    }
                });
            },
            _exec: function (sym) {
                // adds the chosen icon to the editor
                var options = {
                    skipCleaners: true
                };
                
                this._editor.paste(sym, options);
                this._editor.trigger("change");


                this.popup().close();
            },
            _close: function () {
                this.popup().element.off("." + NS);
            }
        };

        const insertSymbolTool = ToolBuilder.createPopupTool(kendo, TOOLBAR_BUTTON_DATA.DEFAULT.name, config, null, popupTemplateGenerator);

        ToolBuilder.registerTool(kendo, insertSymbolTool, TOOLBAR_BUTTON_DATA.DEFAULT.name, buttonTemplate, true);
    }
}

export const INSERT_BOOTSTRAP_ICON_PROVIDER: ClassProvider = {
    multi: true,
    provide: EDITOR_CONFIG_TOKEN,
    useClass: InsertBootstrapIconProvider
};
