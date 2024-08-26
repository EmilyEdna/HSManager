using MessagePack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HSManager.Dto
{
    [MessagePackObject(true)]
    public class CharaInfo
    {
        public List<Info> lstInfo { get; set; }=new List<Info>();
    }

    [MessagePackObject(true)]
    public class Info
    {
        public string name { get; set; }
        public string version { get; set; }
        public long pos { get; set; }
        public long size { get; set; }
    }
}
