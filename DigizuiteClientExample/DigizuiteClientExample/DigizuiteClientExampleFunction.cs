using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using DigizuiteClientExample.Digizuite;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.OpenApi.Models;
using System.Net;
using System.Web.Http;
using System.Xml.Linq;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using Digizuite.Logging;
using static DigizuiteClientExample.Digizuite.DigizuiteEnums;
using System.Collections;
using System.Linq;
using Digizuite.Metadata.ResponseModels;
using Digizuite.Models;
using Digizuite;
using Microsoft.AspNetCore.Http.Internal;
using System.Net.Http;
using Microsoft.AspNetCore.WebUtilities;
using DigizuiteClientExample.Requests;

namespace DigizuiteClientExample
{
    public class DigizuiteClientExampleFunction
    {
        private readonly IDigizuiteClient _digizuteClient;
        private readonly ILogger<DigizuiteClientExampleFunction> _logger;

        public DigizuiteClientExampleFunction(IDigizuiteClient digizuteClient, ILogger<DigizuiteClientExampleFunction> logger)
        {
            _digizuteClient = digizuteClient;
            _logger = logger;
        }

        [FunctionName("GetAssetList")]
        [OpenApiOperation(operationId: "GetAssetList", tags: new[] { "Digizuite" })]
        [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "code", In = OpenApiSecurityLocationType.Query)]
        [OpenApiParameter("query", In = ParameterLocation.Query, Required = false, Type = typeof(string), Description = "Search string")]
        [OpenApiParameter("parentFolderId", In = ParameterLocation.Query, Required = false, Type = typeof(string), Description = "Parent Folder Id")]
        [OpenApiParameter("assetTypeId", In = ParameterLocation.Query, Required = false, Type = typeof(AssetTypeEnum), Description = "Asset Type Id (Image = 4)")]
        [OpenApiParameter("pageNumber", In = ParameterLocation.Query, Required = false, Type = typeof(int), Description = "Page number")]
        [OpenApiParameter("limit", In = ParameterLocation.Query, Required = false, Type = typeof(int), Description = "Maximum number of results")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(IEnumerable<OrderedTreeNode>), Description = "Search tags by query")]
        public async Task<IActionResult> GetAssetsList(
           [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "get-asset-list")] HttpRequest req)
        {
            try
            {
                string searchQuery = req.Query["query"];
                string parentFolderId = req.Query["parentFolderId"];
                int? assetTypeId = string.IsNullOrWhiteSpace(req.Query["assetTypeId"])? null : int.Parse(req.Query["assetTypeId"]);
                int.TryParse(req.Query["pageNumber"], out int pageNumber);
                
                if (!int.TryParse(req.Query["limit"], out int limit))
                {
                    limit = 25;
                }

                var assetList = await _digizuteClient.GetDigizuiteAssetList(assetTypeId, parentFolderId, searchQuery, pageNumber, limit);

                if (assetList != null)
                {
                    return new OkObjectResult(assetList);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Error Received {Value1}", ex);
            }

            return new BadRequestErrorMessageResult("Get Asset list error.");
        }

        [FunctionName("GetAsset")]
        [OpenApiOperation(operationId: "GetAsset", tags: new[] { "Digizuite" })]
        [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "code", In = OpenApiSecurityLocationType.Query)]
        [OpenApiParameter("assetId", In = ParameterLocation.Query, Required = true, Type = typeof(int), Description = "Asset Id")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(IEnumerable<OrderedTreeNode>), Description = "Search tags by query")]
        public async Task<IActionResult> GetAsset(
           [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "get-asset")] HttpRequest req)
        {
            try
            {
                if (int.TryParse(req.Query["assetId"], out int assetId))
                {
                    var assetList = await _digizuteClient.GetDigizuiteAssetById(assetId);

                    if (assetList != null)
                    {
                        return new OkObjectResult(assetList);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Error Received {Value1}", ex);
            }

            return new BadRequestErrorMessageResult("Get Asset error.");
        }

        [FunctionName("UpdateDescription")]
        [OpenApiOperation(operationId: "UpdateDescription", tags: new[] { "Digizuite" })]
        [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "code", In = OpenApiSecurityLocationType.Query)]
        [OpenApiParameter("assetId", In = ParameterLocation.Query, Required = true, Type = typeof(int), Description = "Asset Id")]
        [OpenApiParameter("description", In = ParameterLocation.Query, Required = false, Type = typeof(string), Description = "Description")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(List<MetaFieldResponse>), Description = "Update the description")]
        public async Task<IActionResult> UpdateDescription(
           [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "update-description")] HttpRequest req)
        {
            try
            {
                if (int.TryParse(req.Query["assetId"], out int assetId))
                {
                    await _digizuteClient.UpdateDescription(assetId, req.Query["description"]);
                    
                    return new OkObjectResult("Operation completed successfully.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Error Received {Value1}", ex);
            }

            return new BadRequestErrorMessageResult("Update Description error.");
        }

        [FunctionName("UpdateKeywords")]
        [OpenApiOperation(operationId: "UpdateKeywords", tags: new[] { "Digizuite" })]
        [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "code", In = OpenApiSecurityLocationType.Query)]
        [OpenApiParameter("assetId", In = ParameterLocation.Query, Required = true, Type = typeof(int), Description = "Asset Id")]
        [OpenApiParameter("keywords", In = ParameterLocation.Query, Required = false, Type = typeof(List<string>), Description = "List of keywords")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(List<MetaFieldResponse>), Description = "Update the keywords")]
        public async Task<IActionResult> UpdateKeywords(
           [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "update-keywords")] HttpRequest req)
        {
            try
            {
                if (int.TryParse(req.Query["assetId"], out int assetId) 
                    && req.Query.TryGetValue("keywords", out var keywordsParam))
                {
                    List<string> keywords = keywordsParam[0].Split(',').ToList();

                    await _digizuteClient.UpdateKeyWords(assetId, keywords);

                    return new OkObjectResult("Operation completed successfully.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Error Received {Value1}", ex);
            }

            return new BadRequestErrorMessageResult("Update Keywords error.");
        }

        [FunctionName("UploadNewAsset")]
        [OpenApiOperation(operationId: "UploadNewAsset", tags: new[] { "Digizuite" })]
        [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "code", In = OpenApiSecurityLocationType.Query)]
        [OpenApiRequestBody(contentType: "multipart/form-data", bodyType: typeof(AssetUploadRequest), Required = true, Description = "Asset file")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(string), Description = "Upload the asset")]
        public async Task<IActionResult> UploadNewAsset(
           [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "upload-new-asset")] HttpRequest req)
        {
            try
            {
                var formdata = await req.ReadFormAsync();
                string fileName = formdata["FileName"];
                var asset = req.Form.Files["File"];

                if (!string.IsNullOrWhiteSpace(fileName) && asset != null)
                {
                    UploadResponse result;
                    using (Stream stream = asset.OpenReadStream())
                    {
                        result = await _digizuteClient.UploadNewAsset(stream, fileName);
                    }

                    return new OkObjectResult(result);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Error Received {Value1}", ex);
            }

            return new BadRequestErrorMessageResult("Upload asset error.");
        }

        [FunctionName("ReplaceAsset")]
        [OpenApiOperation(operationId: "ReplaceAsset", tags: new[] { "Digizuite" })]
        [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "code", In = OpenApiSecurityLocationType.Query)]
        [OpenApiRequestBody(contentType: "multipart/form-data", bodyType: typeof(AssetReplaceRequest), Required = true, Description = "Asset file")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(string), Description = "Upload the asset")]
        public async Task<IActionResult> ReplaceAsset([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "replace-asset")] HttpRequest req)
        {
            try
            {
                var formdata = await req.ReadFormAsync();
                string fileName = formdata["FileName"];
                var asset = req.Form.Files["File"];

                if (!string.IsNullOrWhiteSpace(fileName) && int.TryParse(formdata["AssetId"], out int assetId) && asset != null)
                {
                    UploadResponse result;
                    using (Stream stream = asset.OpenReadStream())
                    {
                        result = await _digizuteClient.ReplaceAsset(stream, fileName, assetId);
                    }

                    return new OkObjectResult(result);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Error Received {Value1}", ex);
            }

            return new BadRequestErrorMessageResult("Replace asset error.");
        }

        [FunctionName("GetMetafields")]
        [OpenApiOperation(operationId: "GetMetafields", tags: new[] { "Digizuite" })]
        [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "code", In = OpenApiSecurityLocationType.Query)]
        [OpenApiParameter("metadataFieldGuids", In = ParameterLocation.Query, Required = false, Type = typeof(List<Guid>), Description = "List GUID's of Metadata fields (Title: 5eb3eefc-a043-410f-89b0-29ed3ef37078; Description: c8bb4af3-1598-4ea4-8d7a-98d54eead977; Keywords: 6afe78b7-3f24-49f3-bf95-24890ea62696)")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(List<MetaFieldResponse>), Description = "Search tags by query")]
        public async Task<IActionResult> GetMetafields(
           [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "get-metafields")] HttpRequest req)
        {
            try
            {
                string editableMetadataFieldIdsParam = req.Query["metadataFieldGuids"];

                List<Guid> editableMetadataFieldIds;

                if (!string.IsNullOrWhiteSpace(editableMetadataFieldIdsParam))
                {
                    editableMetadataFieldIds = editableMetadataFieldIdsParam.Split(',').Select(Guid.Parse).ToList();
                }
                else
                {
                    editableMetadataFieldIds = new List<Guid>();
                }

                var metadataFieldList = await _digizuteClient.GetMetadataFields(editableMetadataFieldIds);

                if (metadataFieldList != null)
                {
                    return new OkObjectResult(metadataFieldList);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Error Received {Value1}", ex);
            }

            return new BadRequestErrorMessageResult("Get Metafields error.");
        }

        [FunctionName("GetFolderList")]
        [OpenApiOperation(operationId: "GetFolderList", tags: new[] { "Digizuite" })]
        [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "code", In = OpenApiSecurityLocationType.Query)]
        [OpenApiParameter("parentFolderId", In = ParameterLocation.Query, Required = false, Type = typeof(string), Description = "Parent Folder Id")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(IEnumerable<OrderedTreeNode>), Description = "Get list of folders")]
        public async Task<IActionResult> GetFolderList(
          [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "get-folder-list")] HttpRequest req)
        {
            try
            {
                string parentFolderId = req.Query["parentFolderId"];

                var folderList = await _digizuteClient.GetDigizuitetFolderList(parentFolderId);

                if (folderList != null)
                {
                    return new OkObjectResult(folderList);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Error Received {Value1}", ex);
            }

            return new BadRequestErrorMessageResult("Get Folder list error.");
        }

        [FunctionName("AddAssetToFolders")]
        [OpenApiOperation(operationId: "AddAssetToFolders", tags: new[] { "Digizuite" })]
        [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "code", In = OpenApiSecurityLocationType.Query)]
        [OpenApiParameter("assetId", In = ParameterLocation.Query, Required = true, Type = typeof(int), Description = "Asset Id")]
        [OpenApiParameter("folderIds", In = ParameterLocation.Query, Required = false, Type = typeof(List<int>), Description = "List of folder Id's")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(List<MetaFieldResponse>), Description = "Add Asset to the folders")]
        public async Task<IActionResult> AddAssetToFolders(
           [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "add-asset-to-folders")] HttpRequest req)
        {
            try
            {
                if (int.TryParse(req.Query["assetId"], out int assetId)
                    && req.Query.TryGetValue("folderIds", out var folderIdsParam))
                {
                    List<int> folderIds = folderIdsParam[0].Split(',').Select(d => int.Parse(d)).ToList();

                    await _digizuteClient.AddAssetToFolder(assetId, folderIds);

                    return new OkObjectResult("Operation completed successfully.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Error Received {Value1}", ex);
            }

            return new BadRequestErrorMessageResult("Update Keywords error.");
        }
    }
}
