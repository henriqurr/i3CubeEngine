using i3PackDx.Models;
using i3PackDx.Tools;
using System;
using System.Collections.Generic;

namespace i3PackDx.Managers
{
    public class HeaderManager
    {
        public static CPackHeader header = new CPackHeader();
        public static List<CHeaderNodeInfo> m_pvHeaderDirInfo = new List<CHeaderNodeInfo>();

        public static void GetHeader(Reader reader)
        {
            try
            {
                header.HeaderID = reader.ReadLong();
                header.VersionMajor = reader.ReadShort();
                header.VersionMinor = reader.ReadShort();
                header.StringTableCount = reader.ReadInt();
                header.StringTableOffset = reader.ReadLong();
                header.StringTableSize = reader.ReadLong();
                header.NodeCount = reader.ReadInt();
                header.DirTableOffset = reader.ReadLong();
                header.DirTableSize = reader.ReadLong();
                header.NodeSize = reader.ReadLong();
                header.N00D72CC0 = reader.ReadBytes(16);
                header._0x004C = reader.ReadBytes(108);
            }
            catch
            {
                throw new Exception("CollectHeaderInfo: GetHeader failed.");
            }
        }
    }
}