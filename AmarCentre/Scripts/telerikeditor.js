(function (global, undefined) {
    global.TelerikDemo = {
        OnClientCommandExecuting: function (editor, args) {
            var name = args.get_name(); //The command name
            var val = args.get_value(); //The tool that initiated the command
 
            if (name == "Emoticons" || name == "Emoticons2") {
                //Set the background image to the head of the tool depending on the selected toolstrip item
                var tool = args.get_tool();
                tool.get_element().style.backgroundImage = "url(" + val + ")";
 
                //Paste the selected in the dropdown emoticon    
                editor.pasteHtml("<img src='" + val + "'>");
 
                //Cancel the further execution of the command
                args.set_cancel(true);
            }
 
            var elem = editor.getSelectedElement(); //Get a reference to the selected element                
            if (elem && (name == "OrderedListType" || name == "UnorderedListType")) {
                if (elem.tagName != "OL" && elem.tagName != "UL") {
                    while (elem != null) {
                        if (elem && elem.tagName == "OL" || elem.tagName == "UL") break;
                        elem = elem.parentNode;
                    }
 
                    if (elem) elem.style.listStyleType = val; //apply the selected item shape
                    else alert("No ordered list selected! Please select a list to modify");
                }
                args.set_cancel(true);
            }
 
            if (name == "DynamicDropdown" || name == "DynamicSplitButton") {
                editor.pasteHtml("<span style='width:200px;'>" + val + "</span>");
                //Cancel the further execution of the command
                args.set_cancel(true);
            }

            if (name == "ResetContent") {
                editor.set_html("");
                args.set_cancel(true);
            }
        }
    };
})(window);