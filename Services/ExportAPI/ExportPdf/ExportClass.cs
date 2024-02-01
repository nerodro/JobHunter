using UserAPI.ViewModel;


namespace ExportAPI.ExportPdf
{
    public class ExportClass
    {
        public Task GeneratePdfCv(CvViewModel cvViewModel)
        {
            var renderer = new ChromePdfRenderer();
            var pdfFromUrl = renderer.RenderUrlAsPdf("https://getbootstrap.com/");
            pdfFromUrl.SaveAs("website.pdf");
            var pdfFromHtmlFile = renderer.RenderHtmlFileAsPdf("design.html");
            pdfFromHtmlFile.SaveAs("markup.pdf");
            var pdfFromHtmlString = renderer.RenderHtmlAsPdf(@"<p>Hello World + " + cvViewModel.CategoryName +"</p>", "C:/assets/");
            pdfFromHtmlString.SaveAs("markup_with_assets.pdf");
            return Task.CompletedTask;
        }
    }
}
