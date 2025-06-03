namespace FirstApi.Interfaces;
using FirstApi.Models;

public interface IEncryptionService
{
    public Task<EncryptModel> EncryptData(EncryptModel data);
}