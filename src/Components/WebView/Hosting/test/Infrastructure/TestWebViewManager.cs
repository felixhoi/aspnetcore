using System;
using System.Collections.Generic;

namespace Microsoft.AspNetCore.Components.WebView
{
    public class TestWebViewManager : WebViewManager
    {
        private const string AppBaseUrl = "app://testhost/";
        private List<string> _sentIpcMessages = new();

        public TestWebViewManager(IServiceProvider provider)
            : base(provider, Dispatcher.CreateDefault(), AppBaseUrl)
        {
        }

        public IReadOnlyList<string> SentIpcMessages => _sentIpcMessages;

        protected override void SendMessage(string message)
        {
            _sentIpcMessages.Add(message);
        }

        public override void Navigate(string absoluteUrl)
        {
            throw new NotImplementedException();
        }

        internal void ReceiveIpcMessage(IpcCommon.IncomingMessageType messageType, params object[] args)
        {
            // Same serialization convention as used by blazor.webview.js
            MessageReceived(AppBaseUrl + "page", IpcCommon.Serialize(messageType, args));
        }

        public void ReceiveAttachPageMessage()
        {
            ReceiveIpcMessage(IpcCommon.IncomingMessageType.AttachPage, "http://example/", "http://example/testStartUrl");
        }
    }
}
