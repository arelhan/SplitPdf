using System;
using System.IO;
using System.Windows.Forms;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Diagnostics;

class Program
{
    [STAThread]
    static void Main()
    {
        string sourcePdfPath = GetFilePath();
        string outputPath = GetFolderPath();

        // Hedef klasör yoksa oluştur
        if (!Directory.Exists(outputPath))
        {
            Directory.CreateDirectory(outputPath);
        }

        // PdfReader nesnesi ile PDF dosyasını aç
        PdfReader reader = new PdfReader(sourcePdfPath);

        // Her bir sayfayı ayrı bir PDF olarak kaydet
        for (int i = 1; i <= reader.NumberOfPages; i++)
        {
            string newFilePath = Path.Combine(outputPath, "Sayfa" + i + ".pdf");
            int fileCount = 1;

            // Eğer dosya zaten varsa, yeni bir isim oluştur
            while (File.Exists(newFilePath))
            {
                newFilePath = Path.Combine(outputPath, "Sayfa" + i + "_" + fileCount + ".pdf");
                fileCount++;
            }

            Document document = new Document();
            PdfCopy copy = new PdfCopy(document, new FileStream(newFilePath, FileMode.Create));
            document.Open();
            copy.AddPage(copy.GetImportedPage(reader, i));
            document.Close();
        }

        Console.WriteLine("İşlem tamamlandı. Ayrılmış PDF'ler kaydedildi: " + outputPath);
        reader.Close();

        // Belirlenen klasörü aç
        Process.Start("explorer.exe", outputPath);
    }

    static string GetFilePath()
    {
        OpenFileDialog openFileDialog = new OpenFileDialog();
        openFileDialog.InitialDirectory = "c:\\";
        openFileDialog.Filter = "PDF files (*.pdf)|*.pdf|All files (*.*)|*.*";
        openFileDialog.FilterIndex = 1;
        openFileDialog.RestoreDirectory = true;

        if (openFileDialog.ShowDialog() == DialogResult.OK)
        {
            return openFileDialog.FileName;
        }
        return null;
    }

    static string GetFolderPath()
    {
        FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
        if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
        {
            return folderBrowserDialog.SelectedPath;
        }
        return null;
    }
}
