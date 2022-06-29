using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;

namespace i3CubeEngine
{
    public static class FileSystem
    {
        #region File
        //https://stackoverflow.com/questions/5188527/how-to-deal-with-files-with-a-name-longer-than-259-characters
        public const string LONG_PATH_PREFIX = @"\\?\";

        public static string AddLongPathPrefix(this string path) => LONG_PATH_PREFIX + path;
        public static string WithoutLongPathPrefix(this string path) => path.Replace(LONG_PATH_PREFIX, "");

        public static void LoadFileTreeAsync(object itemNode)
        {
            try
            {
                TreeItem node = itemNode as TreeItem;
                if (!Directory.Exists(node.ItemData))
                {
                    return;
                }
                IEnumerable<string> items = null;
                try
                {
                    items = Directory.EnumerateFileSystemEntries(node.ItemData);
                    //ProgressBarTotal += items.Count();
                }
                catch (DirectoryNotFoundException)
                {
                    return;
                }
                foreach (string entry in items)
                {
                    try
                    {
                        //ProgressBarStep(1);
                        FileAttributes attr = File.GetAttributes(entry);
                        if (attr.HasFlag(FileAttributes.System))
                            continue;
                        try
                        {
                            Directory.GetAccessControl(entry);
                        }
                        catch (UnauthorizedAccessException)
                        {
                            continue;
                        }
                        TreeItem childNode;
                        if (attr.HasFlag(FileAttributes.Directory))
                        {
                            childNode = new TreeItem(entry, node);
                            node.AddChild(childNode);
                            ThreadPool.QueueUserWorkItem(new WaitCallback(LoadFileTreeAsync), childNode);
                        }
                        else
                        {
                            childNode = new TreeItem(entry, node);
                            node.AddChild(childNode);
                        }
                    }
                    catch (Exception ex)
                    {
                        CLogger.Exception(ex);
                    }
                }
                if (Program.form.listViewFiles.Items.Count == 0)
                {
                    if (node.ItemData.Contains(Program.form.CurrentPath))
                    {
                        Program.form.UpdateListView();
                    }
                }
            }
            catch (Exception ex)
            {
                CLogger.WriteLine(ex.ToString());
            }
        }

        public static TreeItem GetFileTreeNodeByPath(string path, TreeItem fileTree)
        {
            try
            {
                if (path == fileTree.ItemData)
                    return fileTree;
                string[] tokens = path.WithoutLongPathPrefix().Split(new char[] { '\\' }, StringSplitOptions.RemoveEmptyEntries);
                TreeItem currentNode = null;
                void nextNode(TreeItem node, int step)
                {
                    if (step >= tokens.Length)
                        return;
                    foreach (TreeItem childNode in node.Childs)
                    {
                        try
                        {
                            string name = childNode.ItemData.WithoutLongPathPrefix().Split(new char[] { '\\' }, StringSplitOptions.RemoveEmptyEntries).Last();
                            if (name == tokens[step])
                            {
                                currentNode = childNode;
                                nextNode(childNode, step + 1);
                            }
                        }
                        catch (Exception ex)
                        {
                            CLogger.Exception(ex);
                        }
                    }
                }
                nextNode(fileTree, 0);
                return currentNode;
            }
            catch (Exception ex)
            {
                CLogger.Exception(ex);
            }
            return null;
        }

        public static void CopyAndPasteDirectory(DirectoryInfo sourceDir, DirectoryInfo destDir)
        {
            try
            {
                for (DirectoryInfo dirInfo = destDir.Parent; dirInfo != null; dirInfo = dirInfo.Parent)
                {
                    if (dirInfo.FullName == sourceDir.FullName)
                        throw new Exception("Target folder is a subdirectory of the source folder.");
                }
                if (!Directory.Exists(destDir.FullName))
                    Directory.CreateDirectory(destDir.FullName);
                foreach (FileInfo fileInfo in sourceDir.GetFiles())
                {
                    fileInfo.CopyTo(Path.Combine(destDir.FullName, fileInfo.Name));
                }
                foreach (DirectoryInfo sourceSubDir in sourceDir.GetDirectories())
                {
                    DirectoryInfo destSubDir = destDir.CreateSubdirectory(sourceSubDir.Name);
                    CopyAndPasteDirectory(sourceSubDir, destSubDir);
                }
            }
            catch (Exception ex)
            {
                CLogger.Exception(ex);
            }
        }
        #endregion

        #region Utils
        public static string FileSizeStr(long fileSize, bool simple)
        {
            unchecked
            {
                string fileSizeStr = string.Empty;
                if (simple)
                {
                    string size = InsertPoint(Math.Ceiling(Math.Round(fileSize * 1.0 / 1024, 2, MidpointRounding.AwayFromZero)).ToString());
                    fileSizeStr = size + " KB";
                }
                else
                {
                    //if (fileSize < (1024 * 1024))
                    //{
                    //    fileSizeStr = (size = Math.Ceiling(Math.Round(fileSize * 1.0 / 1024, 2, MidpointRounding.ToEven)).ToString()) + " KB";
                    //}
                    //else if (fileSize >= (1024 * 1024) && fileSize < (1024 * 1024 * 1024))
                    //{
                    //    fileSizeStr = (size = Math.Ceiling(Math.Round(fileSize * 1.0 / (1024 * 1024), 2, MidpointRounding.ToEven)).ToString()) + " MB";
                    //}
                    //else if (fileSize >= (1024 * 1024 * 1024))
                    //{
                    //    fileSizeStr = (size = Math.Ceiling(Math.Round(fileSize * 1.0 / (1024 * 1024 * 1024), 2, MidpointRounding.ToEven)).ToString()) + " GB";
                    //}
                    fileSizeStr = SizeSuffix(fileSize);
                    string size = InsertPoint(Math.Round(fileSize * 1.0, 2, MidpointRounding.AwayFromZero).ToString());
                    fileSizeStr += $" ({size} bytes)";
                }
                return fileSizeStr;
            }
        }

        public static string InsertPoint(string text)
        {
            if (text.Length > 3)
            {
                int sequence = 0;
                for (int i = text.Length; i > 0; i--)
                {
                    if (sequence == 3)
                    {
                        text = text.Insert(i, ".");
                        sequence = 0;
                    }
                    sequence++;
                }
            }
            return text;
        }

        private static string SizeSuffix(long value)
        {
            /*
               if (decimalPlaces < 0) { throw new ArgumentOutOfRangeException("decimalPlaces"); }
        if (value < 0) { return "-" + SizeSuffix(-value); }
        if (value == 0) { return string.Format("{0:n" + decimalPlaces + "} bytes", 0); }

        // mag is 0 for bytes, 1 for KB, 2, for MB, etc.
        int mag = (int)Math.Log(value, 1024);

        // 1L << (mag * 10) == 2 ^ (10 * mag) 
        // [i.e. the number of bytes in the unit corresponding to mag]
        decimal adjustedSize = (decimal)value / (1L << (mag * 10));

        // make adjustment when the value is large enough that
        // it would round up to 1000 or more
        if (Math.Round(adjustedSize, decimalPlaces) >= 1000)
        {
            mag += 1;
            adjustedSize /= 1024;
        }

        return string.Format("{0:n" + decimalPlaces + "} {1}",
            adjustedSize,
            SizeSuffixes[mag]);*/
            string[] SizeSuffixes = { "bytes", "KB", "MB", "GB", "TB", "PB", "EB", "ZB", "YB" };
            if (value < 0) { return "-" + SizeSuffix(-value); }
            int i = 0;
            decimal dValue = value;
            while (Math.Round(dValue / 1024) >= 1)
            {
                dValue /= 1024;
                i++;
            }
            return string.Format("{0:n2} {1}", dValue, SizeSuffixes[i]).Replace(".", ",");
        }

        public static DateTime ConvertDateObjectToDateTime(string dateToConvert)
        {
            var value = new DateTime();
            if (!string.IsNullOrEmpty(dateToConvert))
            {
                int gmtIndex = dateToConvert.IndexOf("G", StringComparison.Ordinal);

                string newDate = dateToConvert.Substring(0, gmtIndex).Trim();

                value = DateTime.ParseExact(newDate, new[] { "ddd MMM dd yyyy HH:mm:ss", "yyyy-MM-dd'T'HH:mm:ss.fff'Z'" }, CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal);
                return value;
            }
            return value;
        }

        public static long GetDirectorySize(string dirPath, bool counting)
        {
            try
            {
                if (!Directory.Exists(dirPath))
                    throw new ArgumentException("This is not a directory");
                long getSize(string path)
                {
                    long size = 0;
                    try
                    {
                        DirectoryInfo directoryInfo = new DirectoryInfo(path);
                        FileInfo[] fileInfos = directoryInfo.GetFiles();
                        if (counting)
                        {
                            size += fileInfos.Count();
                        }
                        else if (fileInfos.Length > 0)
                        {
                            size += fileInfos.Sum(fileInfo => fileInfo.Length);
                        }

                        DirectoryInfo[] directoryInfos = directoryInfo.GetDirectories();
                        foreach (DirectoryInfo dirInfo in directoryInfos)
                        {
                            size += getSize(dirInfo.FullName);
                        }
                    }
                    catch (UnauthorizedAccessException)
                    {
                    }
                    //IEnumerable<FileInfo> fis = directoryInfo.EnumerateFiles("*", SearchOption.AllDirectories);
                    //foreach (FileInfo fi in fis)
                    //{
                    //    size += fi.Length;
                    //}
                    return size;
                }
                return getSize(dirPath);
            }
            catch (Exception ex)
            {
                CLogger.Exception(ex);
            }
            return 0;
        }

        public static bool IsValidFileName(string fileName)
        {
            const string errChar = "\\/:*?\"<>|";
            foreach (char ch in errChar)
            {
                if (fileName.Contains(ch.ToString()))
                    return false;
            }
            return true;
        }

        public static void RemoveEvents(object ctrl)
        {
            try
            {
                if (ctrl != null)
                {
                    Type t = ctrl.GetType();
                    EventInfo[] eventInfos = t.GetEvents();
                    foreach (EventInfo fieldInfo in eventInfos)
                    {
                        FieldInfo field = t.GetField(fieldInfo.Name, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static | BindingFlags.IgnoreCase);
                        if (field != null)
                        {
                            object value = field.GetValue(ctrl);
                            if (value != null)
                                fieldInfo.RemoveEventHandler(ctrl, value as Delegate);
                        }
                    }
                    GC.SuppressFinalize(ctrl);
                    GC.ReRegisterForFinalize(ctrl);
                    //ctrl = null;
                }
            }
            catch { }
        }

        public static void DestroyObject(object item)
        {
            try
            {
                if (item == null)
                    return;
                RemoveEvents(item);
                GC.ReRegisterForFinalize(item);
                item = null;
            }
            catch { }
        }
        #endregion

        #region ProgressBar
        static int progressBarIndex = -1, progressBarTotal = -1;

        public static int ProgressBarIndex
        {
            get => progressBarIndex;
            set
            {
                progressBarIndex = value;
            }
        }

        public static int ProgressBarTotal
        {
            get => progressBarTotal;
            set
            {
                progressBarTotal = value;
            }
        }

        public static void ProgressBarStep(int add) //1
        {
            try
            {
                var progressbar = Program.form.progressBar.ProgressBar;
                lock (progressbar)
                {
                    int bar = 0;
                    if (add == -1)
                    {
                        bar = add;

                        //progressbar.PerformStep();

                        progressBarIndex = -1;
                        progressBarTotal = -1;
                    }
                    else
                    {
                        ProgressBarIndex += add;

                        bar = (ProgressBarIndex / ProgressBarTotal) * 100;
                    }
                    if (bar > 100)
                        bar = 100;
                    if (bar < 0)
                        bar = 0;
                    if (progressbar.Parent.InvokeRequired)
                        progressbar.Parent.Invoke(new MethodInvoker(delegate { progressbar.Value = bar; }));
                    else
                        progressbar.Value = bar;
                    //CLogger.WriteLine($"BAR: {bar}%");
                    //if (bar == 100.0)
                    //{
                    //    ProgressBarStep(-1);
                    //}
                }
            }
            catch (Exception ex)
            {
                CLogger.WriteLine(ex.ToString());
            }
        }
        #endregion

        #region EngineFile
        public static bool EngineFile(FileInfo file)
        {
            switch (file.Extension.ToLower())
            {
                case ".i3pack": return true;

                //Texture
                case ".i3i": return true;

                //Graphics
                case ".i3gl": return true;

                //Interface
                case ".i3reftex": return true;
                case ".i3subset": return true;
                case ".i3uie": return true; //dec
                case ".i3uil": return true; //dec
                case ".i3uis": return true; //dec
                case ".i3vtex": return true;

                //Chara
                case ".i3chr": return true;

                //Font
                case ".i3fnt": return true; //dec
                case ".i3font": return true;
                case ".i3fontprj": return true; //dec
                case ".lbl": return true; //dec
                case ".str": return true;
                case ".dic": return true;

                //Config
                case ".env": return true;
                case ".ilt": return true; //dec
                case ".sif": return true;
            }
            return false;
        }

        public static bool EngineFile(string path) => EngineFile(new FileInfo(path));

        public static void StartEngineFile(string path)
        {
            try
            {
                FileInfo file = new FileInfo(path);
                switch (file.Extension.ToLower())
                {
                    case ".i3pack":
                        {
                            CLogger.WriteLine("Open i3Pack");
                            //Thread th = new Thread(() =>
                            //{                            
                            //});
                            //th.Start();

                            var form = new i3PackDx.i3PackDxView(path);
                            form.Show();
                            break;
                        }

                    //Texture
                    case ".i3i": break;

                    //Graphics
                    case ".i3gl": break;

                    //Interface
                    case ".i3reftex": break;
                    case ".i3subset": break;
                    case ".i3uie": break; //dec
                    case ".i3uil": break; //dec
                    case ".i3uis": break; //dec
                    case ".i3vtex": break;

                    //Chara
                    case ".i3chr": break;

                    //Font
                    case ".i3fnt": break; //dec
                    case ".i3font": break;
                    case ".i3fontprj": break; //dec
                    case ".lbl": break; //dec
                    case ".str": break;
                    case ".dic": break;

                    //Config
                    case ".env": break;
                    case ".ilt": break; //dec
                    case ".sif": break;
                }
            }
            catch (Exception ex)
            {
                CLogger.Exception(ex);
            }
        }
        #endregion
    }
}