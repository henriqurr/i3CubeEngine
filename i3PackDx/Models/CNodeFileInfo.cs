namespace i3PackDx.Models
{
    public class CNodeFileInfo
    {
		public string Filename;
		public ulong Offset;
		public ulong Size;
		public ulong RawOffset;
		public bool Padded;

		public CNodeFileInfo(string Filename, ulong Offset, ulong Size)
		{
            this.Filename = Filename;
            this.Offset = Offset;
            this.Size = Size;
		}

		public CNodeFileInfo(string Filename, ulong Offset, ulong Size, ulong RawOffset, bool Padded)
		{
			this.Filename = Filename;
            this.Offset = Offset;
            this.Size = Size;
            this.RawOffset = RawOffset;
            this.Padded = Padded;
		}
	}
}