using ClosedXML.Excel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StockExampleApp.Data.Concrete;
using StockExampleApp.Entity;
using System.Data;

namespace StockExampleApp.Controllers
{
    public class ProductController : Controller
    {
        private readonly Context context;

        public ProductController(Context context)
        {
            this.context = context;
        }
        public async Task<IActionResult> Index() {
            
            var list = await context.Products.ToListAsync();
            ProductList Products = new ProductList();
            Products.productList = list;
            return View(Products);
        }

        [HttpPost]
        public async Task<IActionResult> Add(Product product)
        {
            if (product !=null)
            {
                await context.Products.AddAsync(product);
                await context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Ürün Başarıyla Eklendi.";
                return RedirectToAction("Index");
            }
            else
            {
                TempData["FailedMessage"] = "Ürün eklenemedi.";
                return View("Add", product);
            }
            
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }
        public async Task<IActionResult> VievHistory(int id)
        {
            var list = await context.Reports.Where(i=>i.productNo == id).ToListAsync();
            return View(list);
        }
        public IActionResult Edit(int id) {
            var item = context.Products.Where(i=>i.productId == id).FirstOrDefault();
            
            return View(item);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Product product,int dropQuantity)
        {
            int currentQuantity = product.productQuantity;
            if (currentQuantity > dropQuantity)
            {
                Report report = new Report();
                report.productNo = product.productId;
                report.productName = product.productName;
                report.actionTime = DateTime.Now;
                report.currentQuantity = currentQuantity - dropQuantity;
                report.droppedQuantity = dropQuantity;
                report.status = "Stoktan Düşüldü";

                product.productQuantity = currentQuantity - dropQuantity;
                context.Products.Update(product);

                await context.Reports.AddAsync(report);
                await context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Stok Başarıyla Çıkartıldı.";
                return RedirectToAction("Index");
            }
            else
            {
                TempData["FailedMessage"] = "Çıkartılcak Miktar, Stok Miktarından Fazla Olamaz.";
                return View("Edit",product);
            }
           
        }
        public IActionResult AddStock(int id)
        {
            var item = context.Products.Where(i => i.productId == id).FirstOrDefault();

            return View(item);
            
        }

        [HttpPost]
        public async Task<IActionResult> AddStock(Product product, int addedQuantity)
        {
            Report report = new Report();
            report.productNo = product.productId;
            report.productName = product.productName;
            report.actionTime = DateTime.Now;
            report.currentQuantity = product.productQuantity + addedQuantity;
            report.addedQuantity = addedQuantity;
            report.status = "Stok Eklendi";

            product.productQuantity = product.productQuantity + addedQuantity;
            try
            {
                context.Products.Update(product);

                await context.Reports.AddAsync(report);
                await context.SaveChangesAsync();
            }
            catch (Exception)
            {
                TempData["FailedMessage"] = "Stok Eklenemedi.";
                return View("AddStock",product);
            }
            TempData["SuccessMessage"] = "Stok Başarıyla Eklendi.";
            return RedirectToAction("Index");

        }
        public async Task<IActionResult> AllReports()
        {
            var list = await context.Reports.ToListAsync();
            return View(list);
        }

        [HttpPost]
        public IActionResult DownloadExcel()
        {
            DataTable dtExcel = new DataTable();
            dtExcel.Columns.Add("productNo", typeof(int));
            dtExcel.Columns.Add("productName", typeof(string));           
            dtExcel.Columns.Add("actionTime", typeof(string));
            dtExcel.Columns.Add("currentQuantity", typeof(int));
            dtExcel.Columns.Add("droppedQuantity", typeof(int));
            dtExcel.Columns.Add("addedQuantity", typeof(int));
            dtExcel.Columns.Add("status", typeof(string));

            var list = context.Reports.ToList();
            foreach (Report item in list)
            {
                DataRow dr = dtExcel.NewRow();
                dr["productNo"] = item.productNo;
                dr["productName"] = item.productName;
                dr["actionTime"] = item.actionTime.ToString("dd-mm-yyyy :h:mm");
                dr["currentQuantity"] = item.currentQuantity;
                dr["droppedQuantity"] = item.droppedQuantity;
                dr["addedQuantity"] = item.addedQuantity;
                dr["status"] = item.status;
                dtExcel.Rows.Add(dr);
            }
            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Report");
                worksheet.Cell(1, 1).InsertTable(dtExcel);
                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();
                    
                    // HTTP yanıtı olarak Excel dosyasını gönderin
                    try
                    {
                        return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Reports.xlsx");
                    }
                    finally { TempData["SuccessMessage"] = "Excel Indirimi Baslatildi."; }   
                }
            }

            
            
        }
    }
   
}
