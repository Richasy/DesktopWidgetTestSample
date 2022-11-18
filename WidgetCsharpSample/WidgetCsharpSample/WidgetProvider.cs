using Microsoft.Windows.Widgets.Providers;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Runtime.InteropServices;
using Windows.Storage;

namespace WidgetCsharpSample
{
    /// <summary>
    /// Widget provider.
    /// </summary>
    [ComVisible(true)]
    [Guid("4FA8C8F5-F8A8-423F-ADA2-9B30D94DB722")]
    public sealed class WidgetProvider : IWidgetProvider
    {
        private const string TemplatePath = "ms-appx:///Assets/CountingWidgetTemplate.json";
        private int _count;
        private string _widgetId;
        private string _templateJson;

        /// <summary>
        /// Initializes a new instance of the <see cref="WidgetProvider"/> class.
        /// </summary>
        public WidgetProvider()
        {
            _count = 0;
            var manager = WidgetManager.GetDefault();
            var info = manager.GetWidgetInfos().FirstOrDefault();
            if (info != null)
            {
                _widgetId = info.WidgetContext.Id;
            }

            UpdateWidget();
        }

        /// <inheritdoc/>
        public void CreateWidget(WidgetContext widgetContext)
        {
            _widgetId = widgetContext.Id;
            var widgetName = widgetContext.DefinitionId;
            UpdateWidget();
        }

        /// <inheritdoc/>
        public void Activate(WidgetContext widgetContext)
        {
            UpdateWidget();
        }

        /// <inheritdoc/>
        public void Deactivate(string widgetId)
        {
        }

        /// <inheritdoc/>
        public void DeleteWidget(string widgetId, string customState)
        {
            _count = 0;
        }

        /// <inheritdoc/>
        public void OnActionInvoked(WidgetActionInvokedArgs actionInvokedArgs)
        {
            var verb = actionInvokedArgs.Verb;
            if (verb == "inc")
            {
                _count++;
                UpdateWidget();
            }
        }

        /// <inheritdoc/>
        public void OnWidgetContextChanged(WidgetContextChangedArgs contextChangedArgs)
        {
            _widgetId = contextChangedArgs.WidgetContext.Id;
            UpdateWidget();
        }

        private void UpdateWidget()
        {
            var options = new WidgetUpdateRequestOptions(_widgetId);
            if (string.IsNullOrEmpty(_templateJson))
            {
                var storageFile = StorageFile.GetFileFromApplicationUriAsync(new Uri(TemplatePath)).AsTask().Result;
                _templateJson = FileIO.ReadTextAsync(storageFile).AsTask().Result;
            }

            var dataJson = JsonConvert.SerializeObject(new
            {
                count = _count,
            });

            options.Template = _templateJson;
            options.Data = dataJson;
            options.CustomState = _count.ToString();
            WidgetManager.GetDefault().UpdateWidget(options);
        }
    }
}
