using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Digizuite.Services;
using Newtonsoft.Json;

namespace AWExternalActionAITagging.Controllers.Ximilar
{
    public class XimilarAIService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IAssetService _assetService;

        private const string ApiKey = "YOUR_API_KEY";

        public XimilarAIService(IHttpClientFactory httpClientFactory, IAssetService assetService)
        {
            _httpClientFactory = httpClientFactory;
            _assetService = assetService;
        }
        
        /// <summary>
        /// Gets asset by its asset id. From that asset it takes the image preview and fashion tags it
        /// </summary>
        ///  <param name="clientId"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<DigizuiteTagResponse> XimilarFashionAiTagging(string assetId)
        {
            
            // Get asset using the client
            var asset = await _assetService.GetAssetByAssetId(int.Parse(assetId));

            var records = new [] {new XimilarDetectTagsUrl(asset.ImagePreview)};
            var request = new XimilarDetectTagsRequest(records);
            
            // Use AI HTTP Client to move ahead
            var client = _httpClientFactory.CreateClient("AI_HTTP_CLIENT");
            var message = new HttpRequestMessage(HttpMethod.Post, "https://api.ximilar.com/tagging/fashion/v2/tags");
            message.Headers.Authorization = new AuthenticationHeaderValue("Token",  ApiKey);
            message.Content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
            var response = await client.SendAsync(message);
            
            // Validate and return tags
            var contentStream = await ValidateAndGetResponse(response);
            var tagsResponse = JsonConvert.DeserializeObject
                <XimilarFashionDetectTagsResponse>(contentStream);
            
            // Check if we have what we need.
            if (tagsResponse == null || !(tagsResponse.records?.Count() > 0))
            {
                return new DigizuiteTagResponse()
                {
                    errorMessage = "No tags found"
                };
            }
            
            // Gets the records
            var allTags = tagsResponse.records.First()._tags ?? new XimilarFashionDetectObject();

            // Go fetch all the specific types we are interested in
            var filteredCats = FilterAndReturnListFashionTags(allTags.Category, "category");
            var filteredColors = FilterAndReturnListFashionTags(allTags.Color, "color");
            var filteredStyle = FilterAndReturnListFashionTags(allTags.Style, "style");
            var filteredSubCats = FilterAndReturnListFashionTags(allTags.Subcategory, "subCategory");
            var filteredDesign = FilterAndReturnListFashionTags(allTags.Design, "design");
            var filteredClosure = FilterAndReturnListFashionTags(allTags.Closure, "closure");
            var filteredFastening = FilterAndReturnListFashionTags(allTags.Fastening, "fastening");
            var filteredType = FilterAndReturnListFashionTags(allTags.Type, "type");

            // Wrap it all up and send it
            var ximilarDetectTagObjects = filteredCats.Concat(filteredColors).Concat(filteredStyle)
                .Concat(filteredSubCats).Concat(filteredDesign).Concat(filteredClosure).Concat(filteredFastening).Concat(filteredType).ToList();
            var digizuiteTagRes = new DigizuiteTagResponse()
            {
                tags = ximilarDetectTagObjects.Select((tObject) => new DetectTagObject
                {
                    id = tObject.id,
                    name = tObject.name,
                    prob = tObject.prob,
                    type = tObject.type
                }),
                status = ximilarDetectTagObjects.Any() ? "Processed" : "NotRecognized",
                errorMessage = ""
            };

            return digizuiteTagRes;
        }

        #region MyRegion
        
        private static async Task<string> ValidateAndGetResponse(HttpResponseMessage response)
        {
            var contentStream = await response.Content.ReadAsStringAsync();
            
            Console.WriteLine($"contentStream: {contentStream}");

            // If response is not successful then send bad request
            if (!response.IsSuccessStatusCode)
            {
                var resObject = JsonConvert.DeserializeObject<object>(contentStream);
                throw new Exception(JsonConvert.SerializeObject(resObject));
            }

            return contentStream;
        }

        private static IEnumerable<XimilarFashionDetectTagObject> FilterAndReturnListFashionTags(IEnumerable<XimilarFashionDetectTagObject> subcategory, string type)
        {
            return subcategory.Where((label) => label.prob >= 0.6).Select(
                (cat) =>
                {
                    cat.type = type;
                    return cat;
                });
        }        

        #endregion
    }
}