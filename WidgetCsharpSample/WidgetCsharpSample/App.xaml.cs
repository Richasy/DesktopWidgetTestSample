// Copyright (c) Microsoft Corporation and Contributors.
// Licensed under the MIT License.

using Microsoft.UI.Xaml;
using System;
using System.Runtime.InteropServices;

namespace WidgetCsharpSample
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    public partial class App : Application
    {
        private uint _comObjectId;
        private Window _window;

        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            var factory = new SingletonFactory<WidgetProvider>();
            _ = NativeMethods.CoRegisterClassObject(
               typeof(WidgetProvider).GUID,
               factory,
               4,
               1,
               out _comObjectId);
            this.InitializeComponent();
        }

        /// <summary>
        /// Invoked when the application is launched.
        /// </summary>
        /// <param name="args">Details about the launch request and process.</param>
        protected override void OnLaunched(LaunchActivatedEventArgs args)
        {
            _window = new MainWindow();
            _window.Closed += OnWindowClosed;
            _window.Activate();
        }

        private void OnWindowClosed(object sender, WindowEventArgs args)
        {
            if (_comObjectId != 0)
            {
                _ = NativeMethods.CoRevokeClassObject(_comObjectId);
            }
        }

        internal static class NativeMethods
        {
            [DllImport("ole32.dll")]
            public static extern int CoRegisterClassObject(
                [MarshalAs(UnmanagedType.LPStruct)] Guid rclsid,
                [MarshalAs(UnmanagedType.IUnknown)] object pUnk,
                uint dwClsContext,
                uint flags,
                out uint lpdwRegister);

            [DllImport("ole32.dll")]
            public static extern int CoRevokeClassObject(uint dwRegister);
        }
    }
}
