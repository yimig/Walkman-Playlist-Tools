using System.Xml.Serialization;

namespace Test
{
    public class Identification
    {
        [XmlElement("class")]
        public string ProductGroup { get; set; }
        [XmlElement("model")]
        public string ProductModel { get; set; }
        [XmlElement("marketingname")]
        public string MarketName { get; set; }
        [XmlElement("vendor")]
        public string Vendor { get; set; }
        [XmlElement("firmwareversion")]
        public string FirmwareVersion { get; set; }
    }

    public class Storage
    {
        [XmlElement("type")]
        public string Type { get; set; }
        [XmlElement("exclusive-muti-storage")]
        public bool IsExclusiveMultiStorage { get; set; }
        [XmlElement("fw-update-supported")]
        public bool IsFirmwareUpdateSupported { get; set; }
        [XmlElement("max-full")]
        public int MaxFull { get; set; }
    }

    public class Display
    {
        [XmlElement("width")]
        public int Width { get; set; }
        [XmlElement("height")]
        public int Height { get; set; }
    }

    public class Format
    {
        [XmlElement("extension")]
        public string Extension { get; set; }
        [XmlElement("sample-rate")]
        public int SampleRate { get; set; }
        [XmlElement("bits-per-sample")]
        public int BPS { get; set; }
    }

    public class Audio
    {
        [XmlElement("format")]
        public  Format[] SupportFormats { get; set; }
    }

    public class AudioPlaylist
    {
        [XmlElement("format")]
        public Format[] SupportFormats { get; set; }
    }

    public class Path
    {
        [XmlElement("sound")]
        public string MusicStoragePath { get; set; }
    }

    public class FileSystem
    {
        [XmlElement("path")]
        public Path Path { get; set; }
    }

    public class Drm
    {
        [XmlElement("marlin")]
        public bool IsSupportMarlin { get; set; }
    }

    public class Lyrics
    {
        [XmlElement("format")]
        public Format[] SupportFormats { get; set; }
    }

    public class Device
    {
        [XmlElement("identification")]
        public Identification Identification { get; set; }

        [XmlElement("storage")]
        public Storage Storage { get; set; }

        [XmlElement("display")]
        public Display Display { get; set; }

        [XmlElement("audio")]
        public Audio AudioFormatSupports { get; set; }

        [XmlElement("audio-playlist")]
        public AudioPlaylist AudioPlaylistFormatSupports { get; set; }

        [XmlElement("filesystem")]
        public FileSystem FileSystem { get; set; }

        [XmlElement("drm")]
        public Drm Drm { get; set; }

        [XmlElement("lyrics")]
        public Lyrics LyricsFormatSupports { get; set; }
    }

    [XmlRoot("devices")]
    public class Devices
    {
        [XmlElement("version")]
        public string DocumentVersion { get; set; }

        [XmlElement("device")]
        public Device[] Device { get; set; }
    }
}
