using MessagePack;
using System.ComponentModel;

namespace HSManager.Dto
{
    [MessagePackObject]
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class PluginData
    {
        [IgnoreMember]
        public List<string> RequiredPluginGUIDs { get; } = new List<string>();
        [IgnoreMember]
        public List<string> RequiredZipmodGUIDs { get; } = new List<string>();
        [IgnoreMember]
        public int Version => version;
        [IgnoreMember]
        public Dictionary<string, object> Data => data;
        [Key(0)]
        public int version;
        [Key(1)]
        public Dictionary<string, object> data = new Dictionary<string, object>();
    }
}
