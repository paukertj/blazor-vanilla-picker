using System;
using System.Drawing;
using System.Threading.Tasks;
using Blazor.VanillaPicker.Settings;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Blazor.VanillaPicker
{
    public partial class VanillaPicker : IAsyncDisposable
    {
        #region Color

        private Color _value;

        /// <summary>
        /// Selected color.
        /// </summary>
        [Parameter]
        public Color Value
        {
            get => _value;
            set => _value = value;
        }

        /// <summary>
        /// Selected color callback.
        /// </summary>
        [Parameter]
        public EventCallback<Color> ValueChanged { get; set; }

        #endregion

        /// <summary>
        /// How to display the selected color in the text field (the text field still supports input in any format).
        /// </summary>
        [Parameter] 
        public EditorFormatSettings EditorFormat { get; set; } = EditorFormatSettings.Hex;

        /// <summary>
        /// If the picker is used as a popup, where to place it relative to the parent. false to add the picker as a normal child element of the parent.
        /// </summary>
        [Parameter] 
        public PopupSettings Popup { get; set; } = PopupSettings.Right;

        /// <summary>
        /// Whether to enable adjusting the alpha channel.
        /// </summary>
        [Parameter] 
        public bool Alpha { get; set; } = true;

        /// <summary>
        /// Whether to have a "Cancel" button which closes the popup.
        /// </summary>
        [Parameter]
        public bool CancelButton { get; set; } = false;

        /// <summary>
        /// Whether to show a text field for color value editing.
        /// </summary>
        [Parameter]
        public bool Editor { get; set; } = true;

        /// <summary>
        /// Container class.
        /// </summary>
        [Parameter]
        public string Class { get; set; }

        private IJSObjectReference _instance;

        private DotNetObjectReference<VanillaPicker> _dotNetObjectReference;

        private readonly string _id = "id-" + Guid.NewGuid();

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (_instance == null)
            {
                _dotNetObjectReference = DotNetObjectReference.Create(this);

                await JSRuntime.InvokeAsync<IJSObjectReference>("import", "./_content/Blazor.VanillaPicker/VanillaPicker.js");

                _instance = await JSRuntime.InvokeAsync<IJSObjectReference>("VanillaPicker.VanillaPicker.factory", 
                    _id, 
                    _dotNetObjectReference, 
                    _value.ToRgba(), 
                    EditorFormat.ToString().ToLower(),
                    Popup.ToString().ToLower(),
                    Alpha,
                    CancelButton,
                    Editor);
            }

            await base.OnAfterRenderAsync(firstRender);
        }

        protected override async Task OnParametersSetAsync()
        {
            await SetColorAsync(Value);

            await base.OnParametersSetAsync();
        }

        private async Task SetColorAsync(Color color)
        {
            if (_instance == null)
            {
                return;
            }

            await _instance.InvokeVoidAsync("setColorIncludeBg", color.ToRgba(), _id);
        }

        [JSInvokable]
        public async Task OnColorChanged(byte r, byte g, byte b, double a)
        {
            _value = Color.FromArgb(a.ToAlpha(), r, g, b);

            await ValueChanged.InvokeAsync(_value);
        }

        public async ValueTask DisposeAsync()
        {
            if (_instance != null)
            {
                await _instance.DisposeAsync();
            }

            _dotNetObjectReference?.Dispose();
        }
    }
}
