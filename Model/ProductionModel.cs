using DevExpress.CodeParser;
using System;

namespace ProductionApi.Model
{
    public class ProductionModel
    {
        public string component_item_no { get; set; }
        public string customer_part_no { get; set; }    
        public string part_name { get; set;}
        public string Quantity { get; set;}  
       public string Sign { get; set; }
        public string document_date { get; set;}

        public string image_path { get; set; }
        
      

    }
}
