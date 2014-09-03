namespace HidLibrary
{
    public class HidDeviceAttributes
    {
        internal HidDeviceAttributes(NativeMethods.HIDD_ATTRIBUTES attributes)
        {
            VendorId = attributes.VendorID;
            ProductId = attributes.ProductID;
            Version = attributes.VersionNumber;
        }

        public int VendorId { get; private set; }
        public int ProductId { get; private set; }
        public int Version { get; private set; }
    }
}
