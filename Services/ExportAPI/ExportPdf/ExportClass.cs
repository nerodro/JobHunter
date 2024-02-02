using ExportAPI.ViewModel;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace ExportAPI.ExportPdf
{
    public class ExportClass
    {
        
        public Task GeneratePdfCv(CvViewModel cvViewModel)
        {
            QuestPDF.Settings.License = LicenseType.Community;
            Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(2, QuestPDF.Infrastructure.Unit.Centimetre);
                    page.DefaultTextStyle(x => x.FontSize(20));
                    page.Header().Text("Hellow from Pdf " + cvViewModel.JobNmae)
                    .SemiBold();
                });
            }).GeneratePdf("C:/Users/nerodro/Downloads/test.pdf");
            return Task.CompletedTask;
        }
    }
}
