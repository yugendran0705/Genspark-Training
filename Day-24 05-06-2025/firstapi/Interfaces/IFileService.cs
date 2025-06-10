namespace FirstApi.Interfaces;

public interface IFileService
{

    public void CreateFile(string filename, string content);

    public string ReadFile(string filename);
}