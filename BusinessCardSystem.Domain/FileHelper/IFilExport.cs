namespace BizCardSystem.Domain.FileHelper;

public interface IFilExport<T>
{
    byte[] Export(List<T> data);
    bool CanParse(string extension);
}