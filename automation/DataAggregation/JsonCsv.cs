using System;
using System.IO;
using Microsoft.Azure.WebJobs;
using Azure.Storage.Blobs;
using Microsoft.Extensions.Logging;
using CsvHelper;
using Newtonsoft.Json;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Data;
using System.Xml;
using System.Reflection.Metadata;
using Microsoft.Azure.Documents.SystemFunctions;

namespace DataAggregation
{
    [StorageAccount("BlobConnectionString")]
    public class JsonCsv
    {
        [FunctionName("CsvJsonExchange")]
        public async Task Run([BlobTrigger("input/azurecost/azurecost/{name}")] Stream myBlob, string name, ILogger log)
        {
            string jsonFromCsv = "";
            log.LogInformation($"C# Blob trigger function Processed blob\n Name:{name} \n Size: {myBlob.Length} Bytes");


            //Only convert CSV files
            if (name.Contains(".csv"))
            {
                log.LogInformation("Converting to JSON");
                jsonFromCsv = Convert(myBlob);
                //log.LogInformation(jsonFromCsv); 
            }
            else
            {
                log.LogInformation("Not a CSV");
            }
            try
            {
                var jsonResourceLevel = GetResourceLevel();
                var csvContent = BuildCsvContent(jsonFromCsv, jsonResourceLevel);
                byte[] dataAsBytes = csvContent.SelectMany(s => System.Text.Encoding.UTF8.GetBytes(s + Environment.NewLine)).ToArray();
                Stream outBlob = new MemoryStream();
                outBlob.Write(dataAsBytes);
                outBlob.Position = 0;
                //var fileName = System.Guid.NewGuid().ToString() + ".csv";
                var fileName = "outputdata.csv";
                var containerName = "output";
                var blobClient = new BlobContainerClient(Environment.GetEnvironmentVariable("BlobConnectionString"), containerName);
                var blob = blobClient.GetBlobClient(fileName);
                await blob.UploadAsync(outBlob);
                log.LogInformation("Blob " + fileName + " Uploaded Succsessfully");
            }
            catch (Exception ex)
            {
                log.LogInformation(ex.Message);
            }
        }
        private string Convert(Stream blob)
        {
            var csv = new CsvReader(new StreamReader(blob), CultureInfo.InvariantCulture);
            //csv.Configuration.BadDataFound = null; //null skips over bad data, a function can handle bad data as well
            csv.Read();
            csv.ReadHeader();
            var csvRecords = csv.GetRecords<object>().ToList();
            //Convert to JSON
            return JsonConvert.SerializeObject(csvRecords);
        }
        private string GetResourceLevel()
        {
            string resultString = "";
            try
            {
                HttpClient newClient = new HttpClient();
                HttpRequestMessage newRequest = new HttpRequestMessage(HttpMethod.Get, string.Format(Environment.GetEnvironmentVariable("ResourceLevelAPI")));
                HttpResponseMessage response = newClient.Send(newRequest);
                resultString = response.Content.ReadAsStringAsync().Result;
            }
            catch (Exception ex)
            {

                resultString = ex.Source.ToString() + ": " + ex.StackTrace.ToString();
            }
            return resultString;
        }
        private List<string> JsonStringToCSV(string jsonContent)
        {
            //used NewtonSoft json nuget package
            XmlNode xml = JsonConvert.DeserializeXmlNode("{records:{record:" + jsonContent + "}}");
            XmlDocument xmldoc = new XmlDocument();
            xmldoc.LoadXml(xml.InnerXml);
            XmlReader xmlReader = new XmlNodeReader(xml);
            DataSet dataSet = new DataSet();
            dataSet.ReadXml(xmlReader);
            var dataTable = dataSet.Tables[0];

            //Datatable to CSV
            var lines = new List<string>();
            string[] columnNames = dataTable.Columns.Cast<DataColumn>().
                                              Select(column => column.ColumnName).
                                              ToArray();
            var header = string.Join(",", columnNames);
            lines.Add(header);
            var valueLines = dataTable.AsEnumerable()
                               .Select(row => string.Join(",", row.ItemArray));
            lines.AddRange(valueLines);
            return lines;
        }
        private List<string> BuildCsvContent(string jsonFromCsv, string jsonResourceLevel)
        {
            JArray jr1 = JArray.Parse(jsonFromCsv);
            JArray jr2 = JArray.Parse(jsonResourceLevel);
            JArray jr3 = new JArray();

            foreach (JObject item1 in jr1)
            {
                JObject item3 = new JObject();
                var resourcegroupname1 = item1.GetValue("ResourceGroup");
                var subscriptionid1 = item1.GetValue("SubscriptionGuid");
                //if (!String.IsNullOrEmpty(resourcegroupname1.ToString()) || !String.IsNullOrEmpty(subscriptionid1.ToString()))
                //{
                foreach (JObject item2 in jr2)
                {
                    var resourcegroupname2 = item2.GetValue("resourcegroupname");
                    var subscriptionid2 = item2.GetValue("subscriptionid");
                    List<string> keys = item2.Properties().Select(p => p.Name).ToList();
                    //if (!String.IsNullOrEmpty(resourcegroupname2.ToString()) || !String.IsNullOrEmpty(subscriptionid2.ToString()))
                    //{
                    if (resourcegroupname1.ToString().Trim() == resourcegroupname2.ToString().Trim() && subscriptionid1.ToString().Trim() == subscriptionid2.ToString().Trim())
                    {
                        item3.TryAdd("resourceid", item1.GetValue("MeterId"));
                        item3.TryAdd("usagedate", item1.GetValue("UsageDateTime"));
                        item3.TryAdd("resourcename", item1.GetValue("InstanceId"));
                        item3.TryAdd("region", item1.GetValue("ResourceLocation"));
                        item3.TryAdd("resourcetype", item1.GetValue("MeterCategory"));
                        item3.TryAdd("cost", item1.GetValue("PreTaxCost"));
                        item3.TryAdd("projectname", item2.GetValue("projectname"));
                        item3.TryAdd("projectowneremail", item2.GetValue("projectowneremail"));
                        for (int i = 0; i < keys.Count; i++)
                        {
                            item3.TryAdd(keys[i].ToString(), item2.GetValue(keys[i].ToString()));
                        }
                        item3.Remove("resourcedetailid");
                        item3.Remove("leveldetailid");
                        jr3.Add(item3);
                    }
                }
                //}
            }
            //}
            var csvList = JsonStringToCSV(jr3.ToString());
            return csvList;
        }

        //    jr2.Merge(jr1, new JsonMergeSettings
        //{
        //    // union array values together to avoid duplicates
        //    MergeArrayHandling = MergeArrayHandling.Merge
        //});
        //foreach (JObject item in jr2)
        //{
        //    item.Remove("resourcedetailid");
        //    item.Remove("leveldetailid");
        //    var jToken = item.SelectToken("resourceid");
        //    if (jToken != null)
        //    {
        //        jr3.Add(item);
        //    }
        //}
      }
            
  }
