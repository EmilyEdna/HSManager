using HSManager.ObjectModels;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using XExten.Advance.LinqFramework;
using XExten.Advance.NetFramework;

namespace HSManager.Tools
{
    public static class HS2Betterrepack
    {
        public readonly static string Host = "https://sideload2.betterrepack.com/download";

        public static async Task<TreeInfo> GetRoot(string path = "/AISHS2/")
        {
            var data = (await NetFactoryExtension.Resolve<INetFactory>().AddNode(t =>
            {
                t.Node = path;
            }).SetBaseUri(Host).Build(t =>
            {
                t.UseBaseUri = true;
                t.UseCache = true;
            }).RunString()).FirstOrDefault();
            HtmlDocument html = new HtmlDocument();
            html.LoadHtml(data);


            TreeInfo tree = new TreeInfo
            {
                KeyValue = []
            };
            html.DocumentNode.SelectNodes("//tr[@class='odd' or @class='even']").ForEnumerEach((item, index) =>
            {
                var a = item.SelectSingleNode("td[@class='indexcolname']/a");

                var href = a.GetAttributeValue("href", "");

                string filesize = string.Empty;
                var size = item.SelectSingleNode("td[@class='indexcolsize']");
                if (size != null)
                {
                    filesize = size.InnerText;
                }


                if (index == 0)
                    tree.Root = HttpUtility.UrlDecode(href.Contains("/download") ? href.Replace("/download", "") : href);
                else
                {
                    var temp = new TreeChild
                    {
                        Key = a.InnerText,
                        Value = path + HttpUtility.UrlDecode(href),
                        Check = false,
                        Index = index,
                        FileSize = filesize.Contains("-") ? "" : filesize,
                    };


                    tree.KeyValue.Add(temp);
                }


            });

            if (path.Equals("/AISHS2/"))
                tree.Root = string.Empty;
            return tree;

        }
    }
}
