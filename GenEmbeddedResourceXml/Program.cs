using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace GenEmbeddedResourceXml
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("输入Views目录绝对地址：");
            var path = Console.ReadLine();
            //TODO 循环获取文件


            StringBuilder strRemoveXml = new StringBuilder();
            StringBuilder strEmbeddedXml = new StringBuilder();

            strRemoveXml.Append("<ItemGroup>" + Environment.NewLine);
            strEmbeddedXml.Append("<ItemGroup>" + Environment.NewLine);


            DirectoryInfo root = new DirectoryInfo(path);
            GetFiles(root, ref strRemoveXml, ref strEmbeddedXml);


            strRemoveXml.Append("</ItemGroup>" + Environment.NewLine);
            strEmbeddedXml.Append("</ItemGroup>" + Environment.NewLine);

            var csprojFile = Path.Combine(root.Parent.FullName,root.Parent.Name + ".csproj");
            var rootPath = root.Parent.FullName + "\\";
            var stEmbeddedResourceXml = @"<!--EmbeddedResoure_Begin-->" + Environment.NewLine 
                + strRemoveXml.ToString().Replace(rootPath, "") + Environment.NewLine 
                + strEmbeddedXml.ToString().Replace(rootPath, "") 
                + "<!--EmbeddedResoure_End-->" ;

            //打开 CSPROJ文件

            var strCsproj = System.IO.File.ReadAllText(csprojFile);
            
           
            Regex r = new Regex(@"\<!--EmbeddedResoure_Begin--\>([\s\S]*?)\<!--EmbeddedResoure_End--\>");
            strCsproj = r.Replace(strCsproj, stEmbeddedResourceXml, 1);

            //写入文件
            //FileStream fileStream = new FileStream(csprojFile, FileMode.Open);
            using (StreamWriter sw = new StreamWriter(csprojFile, false))
            {
                 
                sw.Write(strCsproj);
                sw.Flush();
                sw.Close();
            }

            Console.ReadLine();

            //DirectoryInfo root = new DirectoryInfo(path);
            //FileInfo[] files = root.GetFiles();

            //<ItemGroup>
            //< None Remove = "views\__system\gateway\action_status.html" />

            //         < ItemGroup >
            //< EmbeddedResource Include = "views\__system\gateway\action_status.html" >
            //   < CopyToOutputDirectory > Always </ CopyToOutputDirectory >
            // </ EmbeddedResource >
        }

        private static void GetFiles(DirectoryInfo dir, ref StringBuilder strRemoveXml, ref StringBuilder strEmbeddedXml)
        {
            var files = dir.GetFiles();
            if (files != null && files.Length > 0)
            {
                GetFileFullPath(files, ref strRemoveXml, ref strEmbeddedXml);
            }

            DirectoryInfo[] dics = dir.GetDirectories();
            if (dics != null && dics.Length > 0)
            {
                foreach (var dic in dics)
                {
                    GetFiles(dic, ref strRemoveXml, ref strEmbeddedXml);
                }
            }

        }

        private static void GetFileFullPath(FileInfo[] files, ref StringBuilder strRemoveXml, ref StringBuilder strEmbeddedXml)
        {
            foreach (var file in files)
            {
                strRemoveXml.Append(string.Format("<None Remove = \"{0}\" />"+ Environment.NewLine, file.FullName));
                strEmbeddedXml.Append(string.Format("<EmbeddedResource Include = \"{0}\">" + Environment.NewLine, file.FullName));
                strEmbeddedXml.Append("<CopyToOutputDirectory >Never</CopyToOutputDirectory>" + Environment.NewLine + "</EmbeddedResource>" + Environment.NewLine);
            }
        }
    }
}
