using i3PackDx.Models;
using i3PackDx.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace i3PackDx.Managers
{
    public class NodeManager
    {
		public static List<CSingleNode> m_pvPackNodes = new List<CSingleNode>();

		public static void GetNodeInfos(Reader reader, CPackHeader header)
        {
			try
			{
				for (int i = 0; i < header.NodeSize; i++)
				{
					byte[] pNodeBuffer = new byte[0x1C];
					long ulDirOffs = 0;
                    try
                    {
                        GetDirectoryInfoById(reader, ref pNodeBuffer, i, ref ulDirOffs);
                    }
                    catch
                    {
                        throw new Exception("CollectNodesInfo: GetDirectoryInfoById failed.");
                    }

					reader.offset = (int)ulDirOffs;
					CNodeInfo pNode = new CNodeInfo
					{
						Type = reader.ReadInt(),
						Index = reader.ReadLong(),
						Offset = reader.ReadLong(),
						Size = reader.ReadLong()
					};

					int ulNameSize = (byte)BitConverter.ToInt32(reader.buffer, (int)pNode.Offset);
					string csNodeName = Encoding.GetEncoding(Initial.Encoding).GetString(reader.buffer, (int)pNode.Offset + 1, ulNameSize);
					int ulBaseOffs = ulNameSize + 1 + 0x4C;
					List<int> vHasChild = new List<int>();
					ulong ulPaddedOffs = 0;

					for (int j = 0; ; j++)
					{
						uint ulChkChild = BitConverter.ToUInt32(reader.buffer, (int)pNode.Offset + ulBaseOffs + (j * 4));
						Console.WriteLine("ulChkChild:" + ulChkChild + " dwNodeSize:" + header.NodeSize);
						if (ulChkChild <= header.NodeSize)
						{
							vHasChild.Add((int)ulChkChild);
							ulPaddedOffs += 4;
						}
						else
						{
							break;
						}
					}
				
					byte[] ucFileInfo = new byte[8];
					Array.Copy(reader.buffer, (long)((ulong)pNode.Offset + (ulong)ulBaseOffs + ulPaddedOffs), ucFileInfo, 0, 8);
					BitRotate.Unshift(ucFileInfo, 0, 8, 3);

					ulong ulFileCount = (ulong)BitConverter.ToInt32(ucFileInfo, 4); //ToUint64
					CSingleNode Node = new CSingleNode(csNodeName, (ulong)pNode.Index, (ulong)pNode.Offset, (ulong)pNode.Size, ulFileCount);
					if (vHasChild.Count != 0)
						Node.ChildId = vHasChild;

					bool bPadded = false;
					for (int j = 0; j < (int)ulFileCount; j++)
					{
						uint ulRawOffs = (uint)(ulBaseOffs + (int)ulPaddedOffs + 8 + (j * (bPadded ? 0x5C : 0x5C - 0x10)));
						//uint ulOffs = BitConverter.ToUInt32(reader._buffer, (int)pNode.Offset + (int)ulRawOffs); //(int)pNode.Offset + (int)ulRawOffs
						//Console.WriteLine("ulOffs " + ulOffs); //só falta corrigir daqui pra baixo

						byte[] pFileInfoBuffer = new byte[0x5C];
						Array.Copy(reader.buffer, (long)((ulong)pNode.Offset + ulRawOffs), pFileInfoBuffer, 0, pFileInfoBuffer.Length);

						uint Ended = BitConverter.ToUInt32(pFileInfoBuffer, 0x58);
						if (Ended == 0x01000000) //0x1000000
                            bPadded = true;
						else
							bPadded = false;

                        BitRotate.Unshift(pFileInfoBuffer, 0, (bPadded ? 0x5C : 0x5C - 16), 2);
						Reader cFileReader = new Reader(pFileInfoBuffer);
						CPackFileInfo pFileInfo = new CPackFileInfo()
						{
							Filename = cFileReader.ReadString(32),
							_0x0020 = cFileReader.ReadBytes(20),
							N0193C181 = cFileReader.ReadUShort(),
							SizeOr_1 = cFileReader.ReadUShort(),
							OffsShift_1 = cFileReader.ReadUShort(),
							SizeShift_1 = cFileReader.ReadUShort(),
							_0x003C = cFileReader.ReadInt(),
							OffsOr_1 = cFileReader.ReadUShort(),
							N01970D81 = cFileReader.ReadInt(),
							SizeOr_2 = cFileReader.ReadUShort(),
							OffsShift_2 = cFileReader.ReadUShort(),
							SizeShift_2 = cFileReader.ReadUShort(),
							N01A26723 = cFileReader.ReadInt(),
							OffsOr_2 = cFileReader.ReadUShort(),
							N019E3220 = cFileReader.ReadBytes(3),
							Ended = cFileReader.ReadUInt(),
						}; // f7 3f95b7 // 3f9603

						ulong ulPushOffset = (ulong)(bPadded ? (pFileInfo.OffsShift_2 << 0x10) | pFileInfo.OffsOr_2 : (pFileInfo.OffsShift_1 << 0x10) | pFileInfo.OffsOr_1);
						ulong ulPushSize = (ulong)(bPadded ? (pFileInfo.SizeShift_2 << 0x10) | pFileInfo.SizeOr_2 : (pFileInfo.SizeShift_1 << 0x10) | pFileInfo.SizeOr_1);

						Node.Files.Add(new CNodeFileInfo(pFileInfo.Filename, ulPushOffset, ulPushSize, ulRawOffs, bPadded));

                        pFileInfoBuffer = null;
                        cFileReader = null;
                    }
					Node.DirTableOffs = (ulong)ulDirOffs;
					m_pvPackNodes.Add(Node);
				    HeaderManager.m_pvHeaderDirInfo.Add(new CHeaderNodeInfo((ulong)ulDirOffs, (ulong)pNode.Index, (ulong)pNode.Offset, (ulong)pNode.Size));

                    pNode = null;
                }
			}
			catch
			{
                throw new Exception("CollectNodesInfo: GetNodeInfos failed.");
            }
        }

		public static CSingleNode GetNodeById(ulong id) => m_pvPackNodes.Where(node => node.Index == id).FirstOrDefault();

        public static void GetDirectoryInfoById(Reader reader, ref byte[] pNodeBuffer, int index, ref long offset)
        {
            long ulOff = HeaderManager.header.DirTableOffset + (index * 0x1C); // 0x001C CNodeInfo Size
            reader.offset = (int)ulOff;
            pNodeBuffer = reader.ReadBytes(0x1C);
            offset = ulOff;
        }
    }
}