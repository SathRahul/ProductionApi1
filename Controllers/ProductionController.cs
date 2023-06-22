using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System;
using ProductionApi.service;
using ProductionApi.Model;
using DevExpress.XtraReports.UI;
using ReclassReports.Reports2;

namespace ProductionApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ProductionController : ControllerBase
    {
        private readonly AllProductionLogic _allPdProductionLogic;
        public ProductionController(IConfiguration configuration)
        {
            _allPdProductionLogic = new ProductionLogic(configuration);
        }


        //[HttpPost]
        //public async Task<IActionResult> GetProduction([FromBody] List<ProductionModel> model)
        //{
        //    string fileUrl = "", full_file_url = "";
        //    XtraReport report = null;
        //    try
        //    {


        //        DataTable dt = _allPdProductionLogic.ToDataTable(model);

        //        report = new production_output_report();
        //        report.ExportOptions.Pdf.Compressed = true;
        //        report.DataSource = dt;
        //        report.DataMember = "ProductionReport";
        //        await report.CreateDocumentAsync().ConfigureAwait(false);

        //        if (!Directory.Exists(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Reports", "Production Report")))
        //        {
        //            Directory.CreateDirectory(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Reports", "Production Report"));
        //        }
        //        string document_file_name = Guid.NewGuid().ToString();
        //        document_file_name = Regex.Replace(document_file_name, @"[^0-9a-zA-Z\.]", "");

        //        fileUrl = "Reports/Production Report/" + document_file_name + ".pdf";

        //        full_file_url = Directory.GetCurrentDirectory() + "/wwwroot/" + fileUrl;
        //        if (System.IO.File.Exists(full_file_url))
        //        {
        //            System.IO.File.Delete(full_file_url);
        //        }

        //        new PdfStreamingExporter(report, true).Export(Directory.GetCurrentDirectory() + "/wwwroot/" + fileUrl);
        //        dt.Dispose();
        //        Response.Headers.Add("filename", document_file_name + ".pdf");
        //        return File(System.IO.File.ReadAllBytes(Directory.GetCurrentDirectory() + "/wwwroot/" + fileUrl), "application/pdf", document_file_name + ".pdf");

        //    }
        //    catch (Exception ex)
        //    {
        //        return new JsonResult(ex.Message);
        //    }
        //    finally
        //    {
        //        if (System.IO.File.Exists(full_file_url))
        //        {
        //            System.IO.File.Delete(full_file_url);
        //        }
        //        if (report != null)
        //            report.Dispose();

        //    }
        //}

        [HttpPost]
        public async Task<IActionResult> ProductionOutputReport([FromBody] List<ProductionModel> model)
        {
            string fileUrl = "", full_file_url = "";
            string qr_code_Image_url = Directory.GetCurrentDirectory() + "/wwwroot/QRCODE/photos";
            XtraReport report = null;
            try
            {

                DataTable dt = await _allPdProductionLogic.ToDataTableAsync(model);

                //loop dt row   genrate url  pupdate url 
                Console.WriteLine(dt);
                //qr_code_Image_url = await _allPdProductionLogic.GenrateQRCode(model[0].customer_part_no, model[0].component_item_no + "," + model[0].document_date).ConfigureAwait(false);

                report = new production_output_report();
                report.ExportOptions.Pdf.Compressed = true;
                report.DataSource = dt;
                report.DataMember = "ProductionOutputReport";
           
           
                //report.Parameters["image_path"].Value = qr_code_Image_url;

                //report.Parameters["image_path"].Visible = false;
                await report.CreateDocumentAsync().ConfigureAwait(false);

                if (!Directory.Exists(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Reports", "ProductionOutput Report")))
                {
                    Directory.CreateDirectory(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Reports", "ProductionOutput Report"));
                }
                string document_file_name = Guid.NewGuid().ToString();
                document_file_name = Regex.Replace(document_file_name, @"[^0-9a-zA-Z\.]", "");

                fileUrl = "Reports/ProductionOutput Report/" + document_file_name + ".pdf";

                full_file_url = Directory.GetCurrentDirectory() + "/wwwroot/" + fileUrl;
                if (System.IO.File.Exists(full_file_url))
                {
                    System.IO.File.Delete(full_file_url);
                }

                new PdfStreamingExporter(report, true).Export(Directory.GetCurrentDirectory() + "/wwwroot/" + fileUrl);
                dt.Dispose();
                Response.Headers.Add("filename", document_file_name + ".pdf");
                return File(System.IO.File.ReadAllBytes(Directory.GetCurrentDirectory() + "/wwwroot/" + fileUrl), "application/pdf", document_file_name + ".pdf");

            }
            catch (Exception ex)
            {
                return new JsonResult(ex.Message);
            }
            finally
            {
                if (System.IO.File.Exists(full_file_url))
                {
                    System.IO.File.Delete(full_file_url);
                }
                if (System.IO.File.Exists(qr_code_Image_url))
                {
                    System.IO.File.Delete(qr_code_Image_url);
                }

                if (report != null)
                    report.Dispose();

            }
        }
    }
}
