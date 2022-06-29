using System;
using System.Collections.Generic;
using System.Threading;

namespace i3CubeEngine
{
    public class TreeItem
    {
        public string ItemData { get; set; }
        public TreeItem ParentItem { get; private set; }
        public List<TreeItem> Childs { get; }

        public static int count = 0;

        public TreeItem(string ItemData, TreeItem ParentItem = null)
        {
            try
            {
                Interlocked.Increment(ref count);
                this.ParentItem = ParentItem;
                this.ItemData = ItemData;
                Childs = new List<TreeItem>(); //TODO capacity
                                               //Console.WriteLine("mChildItems.Capacity = {0}", mChildItems.Capacity);
            }
            catch (Exception ex)
            {
                CLogger.Exception(ex);
            }
        }

        public void AddChild(TreeItem child)
        {
            Childs.Add(child);
        }
    }
}