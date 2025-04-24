using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlantillaComponents.Services
{
    public class TableEventEmitter
    {
        private readonly IJSRuntime _jsRuntime;

        public TableEventEmitter(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }

        public async Task EmitAsync(string eventName)
        {
            await _jsRuntime.InvokeVoidAsync("emitTableEvent", eventName);
        }
    }
}
