import Picker from 'vanilla-picker'
import './VanillaPicker.razor.scss'

function $(selector) { return document.querySelector(selector); }

export class VanillaPicker extends Picker {
    static factory(id, dotNetObjectReference, color, editorFormat, popup, alpha, cancelButton, editor) {
        const parentCustom = $('#' + id),
            popupCustom = new VanillaPicker({
                parent: parentCustom,
                popup: popup,
                color: color,
                alpha: alpha,
                cancelButton: cancelButton,
                editor: editor,
                editorFormat: editorFormat,
                onDone: function (color) {
                    parentCustom.style.backgroundColor = color.rgbaString;
                    dotNetObjectReference.invokeMethodAsync("OnColorChanged", color.rgba[0], color.rgba[1], color.rgba[2], color.rgba[3]);
                },
            });

        parentCustom.style.backgroundColor = color;
        return popupCustom;
    }

    setColorIncludeBg(color, id) {
        this.setColor(color, true);
        $('#' + id).style.backgroundColor = color;
    }
}