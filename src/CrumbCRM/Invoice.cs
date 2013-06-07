using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Drawing;

namespace CrumbCRM
{
    public class Invoice : Item
    {
        [Key]
        [Column("InvoiceID")]
        public int ID { get; set; }
        public string Title { get; set; }        
        public int InvoiceNumber { get; set; }        
        public string PurchaseOrder { get; set; }
        public string PaymentDetails { get; set; }
        public decimal SubValue { get; set; }
        public decimal TotalValue { get; set; }
        public DateTime? DueDate { get; set; }        
        public bool ExcludingVAT { get; set; }
        public DateTime? Deleted { get; set; }
        
        public Invoice()
        {
            CreatedDate = DateTime.Now;
        }

        public byte[] ToPDF()
        {
            try
            {
                byte[] pdfBytes;

                using (MemoryStream ms = new MemoryStream())
                {
                    iTextSharp.text.Document doc = new iTextSharp.text.Document();
                    PdfWriter writer = PdfWriter.GetInstance(doc, ms);
                    iTextSharp.text.Image jpg = iTextSharp.text.Image.GetInstance(HttpContext.Current.Server.MapPath("~/Content/Images/InvoiceSheet/blank_invoice.jpg"));
                    doc.SetPageSize(iTextSharp.text.PageSize.A4);
                    
                    jpg.ScaleToFit(iTextSharp.text.PageSize.A4.Width, iTextSharp.text.PageSize.A4.Height);
                    jpg.Alignment = iTextSharp.text.Image.UNDERLYING;
                    jpg.SetAbsolutePosition(10, -5);

                    doc.Open();
                    doc.Add(jpg);

                    Font defaultFont = new Font(Font.FontFamily.HELVETICA, 7);

                    var cb = writer.DirectContent;

                    ColumnText desc_col = new ColumnText(cb);
                    ColumnText item_col = new ColumnText(cb);
                    ColumnText item_price_col = new ColumnText(cb);
                    ColumnText purchase_col = new ColumnText(cb);
                    ColumnText invoice_col = new ColumnText(cb);
                    ColumnText invoice_date_col = new ColumnText(cb);
                    ColumnText total_sub_price_col = new ColumnText(cb);
                    ColumnText vat_col = new ColumnText(cb);
                    ColumnText total_price_col = new ColumnText(cb);
                    ColumnText job_title_col = new ColumnText(cb);

                    job_title_col.SetSimpleColumn(165, doc.Top - 200, 500, 200, 15, Element.ALIGN_TOP);


                    invoice_col.SetSimpleColumn(doc.Right - 110, doc.Top - 154.2f, 500, 100, 15, Element.ALIGN_TOP);
                    invoice_date_col.SetSimpleColumn(doc.Right - 110, doc.Top - 166.3f, 500, 100, 15, Element.ALIGN_TOP);

                    purchase_col.SetSimpleColumn(200, doc.Top - 270, 500, 100, 15, Element.ALIGN_TOP);
                    desc_col.SetSimpleColumn(165, doc.Top - 320, 415, 200, 15, Element.ALIGN_TOP);
                    item_col.SetSimpleColumn(165, doc.Top - 340, 415, 200, 15, Element.ALIGN_TOP);

                    item_price_col.SetSimpleColumn(doc.Right - 110, doc.Top - 340, 500, 100, 15, Element.ALIGN_TOP);
                    total_sub_price_col.SetSimpleColumn(doc.Right - 110, doc.Bottom - 100, 500, 145, 15, Element.ALIGN_TOP);
                    vat_col.SetSimpleColumn(doc.Right - 110, doc.Bottom - 100, 500, 125, 15, Element.ALIGN_TOP);
                    total_price_col.SetSimpleColumn(doc.Right - 110, doc.Bottom, 500, 105, 15, Element.ALIGN_TOP);

                    job_title_col.AddElement(CreateInfo(this.Title));
                    invoice_col.AddElement(CreateInfo(this.InvoiceNumber.ToString()));
                    invoice_date_col.AddElement(CreateInfo(this.CreatedDate.ToString("dd MMM yyyy")));
                    purchase_col.AddElement(CreateInfo(this.PurchaseOrder));
                    desc_col.AddElement(CreateInfoLight(this.Description));
                    vat_col.AddElement(CreateInfoLight("20%"));

                    // add invoice items to page
                    var items = this.Items;
                    foreach (var item in items)
                    {
                        item_col.AddElement(CreateInfoLight("item title"));
                        item_price_col.AddElement(CreateInfoLight(item.Value.ToString()));
                    }

                    //totals
                    total_sub_price_col.AddElement(CreateInfo(this.SubValue.ToString("C")));
                    total_price_col.AddElement(CreateInfo(this.TotalValue.ToString("C")));


                    job_title_col.Go();
                    purchase_col.Go();
                    invoice_col.Go();
                    invoice_date_col.Go();
                    desc_col.Go();
                    item_col.Go();
                    item_price_col.Go();
                    total_sub_price_col.Go();
                    total_price_col.Go();
                    vat_col.Go();
                 //   column9.Go();
                //    column10.Go();
                  //  column11.Go();

                    doc.Close();

                    pdfBytes = ms.ToArray();
                }

                return pdfBytes;
            }
            catch (Exception)
            {
                throw;
            }
        }
        private IElement CreateInfo(string details)
        {
            Font infoFont = new Font(Font.FontFamily.HELVETICA, 8, Font.BOLD);

            Paragraph paragraph = new Paragraph();
            paragraph.Add(new Phrase(details, infoFont));
            paragraph.Leading = 10;
            return paragraph;
        }
        private IElement CreateInfoLight(string details)
        {
            Font infoFont = new Font(Font.FontFamily.HELVETICA, 8, Font.NORMAL);

            Paragraph paragraph = new Paragraph();
            paragraph.Add(new Phrase(details, infoFont));
            paragraph.Leading = 10;
            return paragraph;
        }

        public byte[] Print()
        {
            byte[] pdfBytes = ToPDF();
            string filename = HttpContext.Current.Server.MapPath("~/Content/Temp/" + this.InvoiceNumber + ".pdf");

            if (File.Exists(filename))
                File.Delete(filename);

            using (FileStream fs = new FileStream(filename, FileMode.CreateNew, FileAccess.ReadWrite))
            {
                fs.Write(pdfBytes, 0, pdfBytes.Length);
            }

            //Process pdfProcess = new Process();
            //pdfProcess.StartInfo.FileName = @"C:\Program Files (x86)\Foxit Software\Foxit Reader\Foxit Reader.exe";
            //pdfProcess.StartInfo.Arguments = string.Format(@"-p {0}", filename);
            //pdfProcess.Start();

            return pdfBytes;
        }

        public List<InvoiceItem> Items { get; set; }
    }
}
