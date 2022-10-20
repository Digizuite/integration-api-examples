using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigizuiteClientExample.Digizuite
{
    //public class WrikeUploadFile
    //{
    //    public int? DigiAssetId { get; set; }

    //    public string DigiAssetUrl { get; set; }

    //    public string Filename { get; set; }

    //    public List<int> FolderIds { get; set; }

    //    public int FileSizeBytes { get; set; }

    //    public Dictionary<string, string[]> Attributes { get; set; }
    //}

    //public class ChannelFolderResponse
    //{
    //    public int ItemId { get; set; }
    //    public int ChannelFolderId { get; set; }
    //    public int? ParentId { get; set; }
    //    public string Name { get; set; }
    //    public int AssetsInFolder { get; set; }
    //    public int AssetsInFolderRecursive { get; set; }
    //}

    public class DigiMetaFieldResponse
    {
        public int Type { get; set; }
        public bool SelectToRoot { get; set; }
        public int ItemId { get; set; }
        public int MetafieldId { get; set; }
        public string Guid { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int SortIndex { get; set; }
        public int VisibilityMetaFieldId { get; set; }
        public string VisibilityRegex { get; set; }
        public bool Required { get; set; }
        public bool Readonly { get; set; }
        public bool ShowInList { get; set; }
        public bool System { get; set; }
        public string AutoTranslate { get; set; }
        public int GroupId { get; set; }
        public int RestrictToAssetType { get; set; }
        public string UploadTagName { get; set; }
        public string Label { get; set; }
        public Dictionary<int, MetaFieldLabel> Labels
        {
            get;
            set;
        }
    }

    public class MetaFieldLabel
    {
        public int LabelId { get; set; }
        public int LanguageId { get; set; }
        public int MetafieldId { get; set; }
        public string Label { get; set; }
        public string Description { get; set; }
    }

    //public class TreeNodesResponse
    //{
    //    public List<TreeNode> Items { get; set; }
    //    public int ErrorCode { get; set; }
    //    public string ErrorMessage { get; set; }
    //    public bool HasError { get; set; }
    //}

    //public class TreeNode
    //{
    //    public bool HasChildren { get; set; }
    //    public string Label { get; set; }
    //    public int Id { get; set; }
    //    public int? ParentId { get; set; }
    //    public int Depth { get; set; }
    //    public string OptionValue { get; set; }
    //    public bool ShowCheck { get; set; }
    //    public int TreeNodeId { get; set; }
    //}

    public class OrderedTreeNodeLabel
    {
        public string Label { get; set; }
        public int LanguageId { get; set; }
        public int TreeValueId { get; set; }
    }

    public class OrderedTreeNode
    {
        public int TreeNodeId { get; set; }
        public string OptionValue { get; set; }
        public int? ParentId { get; set; }
        public int SortIndex { get; set; }
        public int MetaFieldId { get; set; }
        public int ItemId { get; set; }
        public string ItemGuid { get; set; }
        public int Depth { get; set; }
        public bool hasChildren { get; set; }
        public Dictionary<int, OrderedTreeNodeLabel> Labels { get; set; }
    }

    public class OrderedTreeNodesResponse
    {
        public List<OrderedTreeNode> items { get; set; }
        public int errorCode { get; set; }
        public string errorMessage { get; set; }
        public bool hasError { get; set; }
    }

}
