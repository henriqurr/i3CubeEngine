namespace i3PackDx.Models
{
    public class CHeaderNodeInfo
    {
		public ulong BaseAddr;
		public ulong Index;
		public ulong Offset;
		public ulong Size;

		public CHeaderNodeInfo(ulong BaseAddr, ulong Index, ulong Offset, ulong Size)
		{
			this.BaseAddr = BaseAddr;
            this.Index = Index;
            this.Offset = Offset;
            this.Size = Size;
		}
	}
}