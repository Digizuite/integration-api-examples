using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Digizuite;
using Digizuite.BatchUpdate;
using Digizuite.BatchUpdate.Models;
using Digizuite.Cache;
using Digizuite.Extensions;
using Digizuite.HttpAbstraction;
using Digizuite.Logging;
using Digizuite.Metadata;
using Digizuite.Metadata.RequestModels.UpdateModels;
using Digizuite.Metadata.ResponseModels;
using Digizuite.Models;
using Digizuite.Models.Search;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using static DigizuiteClientExample.Digizuite.DigizuiteEnums;

namespace DigizuiteClientExample.Digizuite
{
    public class DigizuiteClient : IDigizuiteClient
    {
        private const string MEDIA_MANAGER_MENU_GUID = "BF0AD1A6-984A-494E-A227-9D70C6A864F9";

        private readonly HttpClient _httpClient;
        private readonly IBatchUpdateClient _batchUpdateClient;
        private readonly ISearchService _searchService;
        private readonly IAssetService _assetService;
        private readonly IUploadService _uploadService;
        private readonly IDamAuthenticationService _authService;
        private readonly IMetaFieldService _metaFieldService;
        private readonly IMetadataValueService _metadataValueService;
        private readonly ServiceHttpWrapper _serviceHttpWrapper;

        private int _portalMenuMetafieldLabelId = -1;
        private int _portalMenuMetafieldLId = -1;

        private readonly int _portalMenuLanguageId = 3; // 3: English by default

        public DigizuiteClient()
        {
            var config = new DigizuiteConfiguration()
            {
                BaseUrl = new Uri("--DAM-URL--"),
                SystemUsername = "--DAM-USER--",
                SystemPassword = "--DAM-USER-PASSWORD--"
            };

            ILogger<SearchService> searchLogger = new ConsoleLogger<SearchService>();
            ILogger<AssetService> _assetServiceLogger = new ConsoleLogger<AssetService>();
            ILogger<BatchUpdateClient> batchUpdateLogger = new ConsoleLogger<BatchUpdateClient>();
            ILogger<DamAuthenticationService> authLogger = new ConsoleLogger<DamAuthenticationService>();
            ILogger<RestClient> restLogger = new ConsoleLogger<RestClient>();
            ILogger<OldUploadService> uploadServiceLogger = new ConsoleLogger<OldUploadService>();
            ILogger<MetaFieldService> metaFieldLogger = new ConsoleLogger<MetaFieldService>();
            ILogger<MetadataValueService> metadataValueServiceLogger = new ConsoleLogger<MetadataValueService>();

            _httpClient = new HttpClient();
            DevServerConfigurations devConf = new DevServerConfigurations(config);
            _serviceHttpWrapper = new ServiceHttpWrapper(devConf, config, _httpClient, restLogger);
            _authService = new DamAuthenticationService(config, authLogger, _serviceHttpWrapper); // Might have changed in newest version
            _searchService = new SearchService(_serviceHttpWrapper, _authService, searchLogger);
            _uploadService = new OldUploadService(_authService, _serviceHttpWrapper, uploadServiceLogger, config);

            _batchUpdateClient = new BatchUpdateClient(batchUpdateLogger, _authService, config, _serviceHttpWrapper);
            _assetService = new AssetService(_searchService, _batchUpdateClient, _assetServiceLogger);

            _metaFieldService = new MetaFieldService(metaFieldLogger, _serviceHttpWrapper, _authService);
            _metadataValueService = new MetadataValueService(_authService, metadataValueServiceLogger, _serviceHttpWrapper);

            FetchMetafieldSettings();
        }

        public async Task<IEnumerable<OrderedTreeNode>> GetDigizuitetFolderList(string parentId)
        {
            var (client, request) = _serviceHttpWrapper.GetClientAndRequest(ServiceType.LegacyService,
                $"/api/tree/nodes/metafield/{_portalMenuMetafieldLId}");

            request.AddAccessKey(await _authService.GetAccessKey());

            RestResponse<OrderedTreeNodesResponse> orderedTreeNodeResp = client.GetAsync<OrderedTreeNodesResponse>(request, System.Threading.CancellationToken.None).Result;

            if (orderedTreeNodeResp.Data == null)
            {
                return new List<OrderedTreeNode>();
            }

            if (string.IsNullOrWhiteSpace(parentId))
            {
                return orderedTreeNodeResp.Data.items.Where(f => !f.ParentId.HasValue);
            }

            return orderedTreeNodeResp.Data.items.Where(f => f.ParentId == int.Parse(parentId));
        }

        public async Task<IEnumerable<Asset>> GetDigizuiteAssetList(int? assetTypeId, string parentId, string search, int page, int pageSize)
        {
            var parameters = new SearchParameters("GetAssets");

            int parentFolderId = 0;
            if (!string.IsNullOrWhiteSpace(parentId)
                && int.TryParse(parentId, out parentFolderId))
            {
                parameters.Add("sMenu", new List<int> { parentFolderId });
            }

            parameters.Add("sortDateDesc", "1");

            parameters.Add("freetext", search);

            if (assetTypeId.HasValue && assetTypeId.Value > 0)
            {
                parameters.Add("sAssetType", assetTypeId.Value);
            }

            //Remove crops!
            parameters.Add("sPrevref", "0");

            //Page and limit
            parameters.Page = page > -1 ? page : 1;
            parameters.PageSize = pageSize > -1 ? pageSize : 25;

            SearchResponse<Asset> results = null;

            try
            {
                results = await _searchService.Search<Asset>(parameters);
            }
            catch (Exception ex)
            {
                // TODO: Log exception
            }

            return results.Items;
        }

        public Task<Asset> GetDigizuiteAssetById(int assetId)
        {
            return _assetService.GetAssetByAssetId(assetId);
        }

        public async Task<List<MetaFieldResponse>> GetMetadataFields(List<Guid> metadaFieldIds)
        {
            var metaFields = await _metaFieldService.GetAllMetaFields();

            return metaFields.FindAll(mf => metadaFieldIds.Contains(mf.Guid));
        }

        public async Task<MetadataEditorResponse> GetMetadataForAssetItemId(int itemId)
        {
            var res = await _metadataValueService.GetRawMetadata(new GetMetadataRequest
            {
                ItemIds = new List<int>() {itemId}
            });

            return res;
        }

        private void FetchMetafieldSettings()
        {
            int portalMenuLanguageId = -1;
            int portalMenuMetafieldId = -1;
            var (client, request) = _serviceHttpWrapper.GetClientAndRequest(ServiceType.LegacyService,
                $"/api/metafield?metaFieldItemGuids={MEDIA_MANAGER_MENU_GUID}");

            request.AddAccessKey(_authService.GetAccessKey().Result);

            try
            {
                RestResponse<List<DigiMetaFieldResponse>> portalMenuMetafieldResp = client.GetAsync<List<DigiMetaFieldResponse>>(request, System.Threading.CancellationToken.None).Result;

                if (portalMenuMetafieldResp.IsSuccessful && portalMenuMetafieldResp.Data != null)
                {
                    portalMenuLanguageId = portalMenuMetafieldResp.Data.FirstOrDefault().Labels[_portalMenuLanguageId].LabelId;
                    portalMenuMetafieldId = portalMenuMetafieldResp.Data.FirstOrDefault().MetafieldId;
                }
            }
            catch (Exception ex)
            {
                // TODO: Log exception
            }

            _portalMenuMetafieldLabelId = portalMenuLanguageId;
            _portalMenuMetafieldLId = portalMenuMetafieldId;
        }

        public Task<UploadResponse> ReplaceAsset(Stream assetStream, string assetName, int assetId)
        {
            return _uploadService.Replace(assetStream, $"{assetName}.jpg", "INTEGRATION_BROKER", assetId, KeepMetadata.Keep, Overwrite.AddHistoryEntry);
        }

        public Task<UploadResponse> UploadNewAsset(Stream assetStream, string assetName)
        {
            return _uploadService.Upload(assetStream, $"{assetName}.jpg", "INRIVER_INTEGRATION_CONSID");
        }

        public Task UpdateDescription(int itemId, string description)
        {
            var update = new StringMetadataUpdate()
            {
                Value = description,
                MetaFieldItemGuid = Guid.Parse("c8bb4af3-1598-4ea4-8d7a-98d54eead977"),
                MetaFieldLabelId = 50727,
                SkipAutoTranslateBehavior = true,
                TargetItemIds = new HashSet<int>() { itemId }
            };

            return _metadataValueService.ApplyUpdate(new List<MetadataUpdate>() { update });
        }

        public Task UpdateKeyWords(int itemId, List<string> keywords)
        {
            var update = new EditMultiComboValueMetadataUpdate()
            {
                ComboValues = keywords,
                MetaFieldItemGuid = Guid.Parse("6afe78b7-3f24-49f3-bf95-24890ea62696"),
                MetaFieldLabelId = 10438,
                SkipAutoTranslateBehavior = true,
                TargetItemIds = new HashSet<int>() { itemId }
            };

            return _metadataValueService.ApplyUpdate(new List<MetadataUpdate>() { update }, _authService.GetAccessKey().Result, CancellationToken.None);
        }

        public Task AddAssetToFolder(int assetItemId, List<int> folderIds)
        {
            var batchUpdate = new Batch(new BatchPart
            {
                ItemIds = { assetItemId },
                Target = FieldType.Asset,
                BatchType = BatchType.ItemIdsValuesRowId,
                Values =
                {
                    new IntListBatchValue(FieldType.Metafield, folderIds, null)
                }
            });

            return _batchUpdateClient.ApplyBatch(batchUpdate);
        }
    }
}
