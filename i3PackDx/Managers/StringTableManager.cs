using i3PackDx.Models;
using i3PackDx.Tools;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace i3PackDx.Managers
{
    public class StringTableManager
    {
        public static List<StringTable> stringTables = new List<StringTable>();
        public static string lastReg;

        public static void GetStringTables(Reader br, CPackHeader header)
        {
            try
            {
                br.offset = (int)header.StringTableOffset;
                byte[] buffer = br.ReadBytes((int)header.StringTableSize);

                using (StreamReader reader = new StreamReader(new MemoryStream(buffer)))
                {
                    for (int i = 0; i < header.StringTableCount; i++)
                    {
                        string str = reader.ReadLine();
                        if (!string.IsNullOrEmpty(str))
                        {
                            if (str.Length > 4 && str.Substring(0, 2) == "i3") //old i3Reg
                            {
                                lastReg = str;
                                stringTables.Add(new StringTable
                                {
                                    Name = str,
                                    Values = new List<string>()
                                });
                            }
                            else
                            {
                                for (int j = 0; j < stringTables.Count; j++)
                                {
                                    StringTable item = stringTables[j];
                                    if (item.Name == lastReg)
                                    {
                                        item.Values.Add(str);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch
            {
                throw new Exception("CollectStringTables: GetStringTables failed.");
            }
        }

        public static void WriteStringTables(BinaryWriter bw)
        {
            try
            {
                for (int i = 0; i < stringTables.Count; i++)
                {
                    var stringTable = stringTables[i];
                    bw.Write(Encoding.GetEncoding(Initial.Encoding).GetBytes(stringTable.Name));
                    bw.Write((ushort)2573);
                    for (int j = 0; j < stringTable.Values.Count; j++)
                    {
                        var item = stringTable.Values[j];
                        bw.Write(Encoding.GetEncoding(Initial.Encoding).GetBytes(item));
                        if ((i + 1) == stringTables.Count && (j + 1) == stringTable.Values.Count)
                        {
                            continue;
                        }
                        bw.Write((ushort)2573);
                    }
                }
            }
            catch
            {
                throw new Exception("CollectStringTables: WriteStringTables failed.");
            }
        }
    }
}