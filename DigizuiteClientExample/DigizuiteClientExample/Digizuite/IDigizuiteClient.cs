using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Digizuite;
using Digizuite.BatchUpdate;
using Digizuite.HttpAbstraction;
using Digizuite.Logging;
using Digizuite.Metadata;
using Digizuite.Metadata.RequestModels.UpdateModels;
using Digizuite.Metadata.ResponseModels;
using Digizuite.Models;
using Digizuite.Models.Search;

namespace DigizuiteClientExample.Digizuite
{
    public interface IDigizuiteClient
    {
        public Task<IEnumerable<OrderedTreeNode>> GetDigizuitetFolderList(string parentId);

        public Task<IEnumerable<Asset>> GetDigizuiteAssetList(int? assetTypeId, string parentId, string search, int page, int pageSize);

        public Task<Asset> GetDigizuiteAssetById(int assetId);

        public Task<List<MetaFieldResponse>> GetMetadataFields(List<Guid> metadaFieldIds);

        public Task<UploadResponse> ReplaceAsset(Stream assetStream, string assetName, int assetId);

        public Task<UploadResponse> UploadNewAsset(Stream assetStream, string assetName);

        public Task UpdateDescription(int itemId, string description);

        public Task UpdateKeyWords(int itemId, List<string> keywords);

        public Task AddAssetToFolder(int assetItemId, List<int> folderIds);
    }
}
