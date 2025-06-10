namespace FirstApi.Services;
using FirstApi.Interfaces;
public class FileService : IFileService
{
    private readonly string _basePath = "local_filestorage";
    public void CreateFile(string filename, string content)
    {
        System.IO.File.WriteAllText(filename, content);
    }

    public string ReadFile(string filename)
    {
        var content = System.IO.File.ReadAllText(filename);
        return content;
    }
}