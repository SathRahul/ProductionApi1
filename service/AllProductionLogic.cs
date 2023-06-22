using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Threading.Tasks;
using System;
using ProductionApi.Model;
using ProductionApi.Controllers;
using QRCoder;
using System.Drawing.Imaging;
using System.Drawing;
using System.IO;


namespace ProductionApi.service
{
    public interface AllProductionLogic
    {

    
        Task<JsonResult> Productionreport(List<ProductionModel> model);
        public async Task<DataTable> ToDataTableAsync<T>(List<T> items)  // convert List object to datatable 
        {
            string date = "";
            string part_no = "";
            string qr_code_Image_url = "";
            try
            {
                DataTable dataTable = new DataTable(typeof(T).Name);
                //Get all the properties
                
                PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
                foreach (PropertyInfo prop in Props)
                {
                    //Defining type of data column gives proper data table 
                    Type type = prop.PropertyType.IsGenericType && prop.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>) ? Nullable.GetUnderlyingType(prop.PropertyType) : prop.PropertyType;
                    //Setting column names as Property names
                    dataTable.Columns.Add(prop.Name, type);
                }
                foreach (T item in items)
                {

                    object[] values = new object[Props.Length];
                    for (int i = 0; i < Props.Length; i++)
                    {
                        //inserting property values to datatable rows
                        values[i] = Props[i].GetValue(item, null);
                        if (Props[i].Name== "component_item_no")
                        {
                            part_no = (string)Props[i].GetValue(item, null);
                        }
                        if (Props[i].Name == "document_date")
                        {
                            date = (string)Props[i].GetValue(item, null);
                        }
                        if (values[i] == null) {
                            qr_code_Image_url = await GenrateQRCode(part_no, part_no + "," + date).ConfigureAwait(false);
                            values[i] = qr_code_Image_url;
                        }
                    }
                    dataTable.Rows.Add(values);
                }
                //put a breakpoint here and check datatable
                return dataTable;
            }
            catch (Exception)
            {
                DataTable dt = new DataTable();
                return dt;
            }

            finally
            {
                //if (System.IO.File.Exists(qr_code_Image_url))
                //{
                //    System.IO.File.Delete(qr_code_Image_url);
                //}
            }
        }
        public async Task<string> GenrateQRCode(string document_no, string qrcodetext)
        {
            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(qrcodetext, QRCodeGenerator.ECCLevel.Q);
            QRCode qrCode = new QRCode(qrCodeData);
            Bitmap qrCodeImage = qrCode.GetGraphic(20);
            if (!Directory.Exists(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "QRCODE")))
            {
                Directory.CreateDirectory(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "QRCODE"));
            }

            string fileUrl = Directory.GetCurrentDirectory() + "/wwwroot/QRCODE/photos" + document_no.Replace("/", "_") + ".png";
            if (File.Exists(fileUrl))
            {
                File.Delete(fileUrl);
            }
            qrCodeImage.Save(fileUrl, ImageFormat.Png);
            return fileUrl;

        }



    }
}
