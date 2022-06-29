using System.Collections.Generic;

namespace i3PackDx.Models
{
    public class CSingleNode
	{
		public string NodeName;
		public ulong Index;
		public ulong Offset;
		public ulong Size;
		public ulong FileCount;
		public ulong DirTableOffs;
        public bool isRoot = false;

		public List<int> ChildId = new List<int>(); //Tree node info

		public List<CNodeFileInfo> Files = new List<CNodeFileInfo>(); //File info

		public CSingleNode(string NodeName, ulong Index, ulong Offset, ulong Size, ulong FileCount)
		{
            this.NodeName = NodeName;
            this.Index = Index;
            this.Offset = Offset;
            this.Size = Size;
            this.FileCount = FileCount;
		}
    
        public bool HasChild() => ChildId.Count != 0;

        public bool IsLeaf() => ChildId.Count == 0;

        //public bool IsRoot() => NodeName == "/";
    }
}