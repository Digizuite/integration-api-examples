namespace DigizuiteClientExample.Digizuite
{
    public class DigizuiteEnums
    {
        public enum AssetTypeEnum
        {
            Default = 0,
            Video = 1,
            Audio = 2,
            File = 3,
            Image = 4,
            PowerPoint = 5,
            HTML = 6,
            Text = 7,
            Word = 8,
            Excel = 9,
            InDesign = 10,
            Zip = 11,
            META = 12,
            PDF = 14,
            Archive = 15,
            Photoshop = 16,
            Illustrator = 17,
            Visio = 18,
            Cad = 19,
            Folder = 20,
            CatalogFolder = 21,
            User = 30,
            Group = 31,
            Profile = 32,
            Destination = 33,
            Catalog = 34,
            Metagroup = 35,
            Metafield = 36,
            DigizuiteConfig = 37,
            MediaFormat = 38,
            TranscodeSetting = 39,
            License = 40,
            ValidationFunction = 41,
            MediaFormatType = 42,
            MediaFormatTypeExtension = 43,
            DigizuiteConfig_AssetType = 44,
            IdentifyRule = 45,
            IdentifyRulePlugin = 46,
            LanguageLabels = 47,
            StorageManager = 48,
            StreamingAccessRule = 65,
            StreamingAccessIPMask = 66,
            ClientLicense = 67,
            MemberSetting_Preference = 50,
            MemberSetting_Information = 51,
            MemberSetting_SystemInformation = 52,
            Application = 53,
            Setting_Custom = 54,
            Application_media_transcode_proxy = 56,
            PortalWorkfolderItem = 60,
            PortalTemplate = 61,
            PortalLayoutFolder = 62,
            PortalPage = 63,
            PortalQualityDelete = 64,
            AssetCopy = 70,
            AssetClear = 71,
            Application_Reload = 80,
            ODT = 100,
            OTT = 101,
            ODS = 102,
            OTS = 103,
            ODP = 105,
            OTP = 106,
            ODG = 107,
            OTG = 108,
            ODB = 109,
            ODF = 110,
            ODM = 111,
            OTH = 112,
            WebPage = 1001,
            EDL_Index = 2001,
            Metafield_label = 150,
            ImageMagickGeneric = 200,
            EDLText = 201,
            Live = 1000,
            Search = 10000,
        };

        public enum FilterAssetType
        {
            Video = 1,
            Image = 4,
            PowerPoint = 5,
            InDesign = 10,
            PDF = 14,
            Photoshop = 16,
            Illustrator = 17
        };

        public enum VideoPlaybackType
        {
            Redirect = 1,
            Streaming = 2,
            Cdn = 3
        }

        public enum MetaFieldDataType
        {
            Int = 51,
            String = 60,
            Bit = 61,
            DateTime = 0x40,
            MultiComboValue = 67,
            ComboValue = 68,
            EditComboValue = 69,
            Note = 70,
            MasterItemReference = 80,
            SlaveItemReference = 81,
            Float = 82,
            EditMultiComboValue = 169,
            Tree = 300,
            Link = 350,
            MetaFieldGroup = 65,
            MetagroupRef = 100
        }
    }
}
