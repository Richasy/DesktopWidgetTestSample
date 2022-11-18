using System;
using System.Runtime.InteropServices;

namespace WidgetCsharpSample
{
    [ComVisible(true)]
    [ClassInterface(ClassInterfaceType.None)]
    internal class SingletonFactory<T> : IClassFactory
    {
        public int CreateInstance([In] IntPtr pUnkOuter, [In] ref Guid riid, [Out] out IntPtr ppvObject)
        {
            object instance = Activator.CreateInstance(typeof(T));
            if (pUnkOuter != IntPtr.Zero)
            {
                ppvObject = IntPtr.Zero;
                return unchecked((int)0x80040110); // CLASS_E_NOAGGREGATION
            }

            ppvObject = Marshal.GetIUnknownForObject(instance);
            return 0;
        }

        public int LockServer([In, MarshalAs(UnmanagedType.VariantBool)] bool fLock)
        {
            return 0;
        }
    }

    [ComImport]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [Guid("00000001-0000-0000-C000-000000000046")]
    internal interface IClassFactory
    {
        [PreserveSig]
        int CreateInstance(IntPtr pUnkOuter, ref Guid riid, out IntPtr ppvObject);

        [PreserveSig]
        int LockServer(bool fLock);
    }
}
