using ExportAPI.ViewModel;

namespace ExportAPI.ExportPdf
{
    public class ExportClass
    {
        public Task GeneratePdfCv(CvViewModel cvViewModel)
        {
            var renderer = new ChromePdfRenderer();
            var pdfFromHtmlString = renderer.RenderHtmlAsPdf(@"<p>Hello World + " + cvViewModel.CategoryName +"</p>").SaveAs("test.pdf");
            pdfFromHtmlString.SaveAs("markup_with_assets.pdf");
            return Task.CompletedTask;
        }
    }
}
