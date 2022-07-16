namespace Lection_2_BL.Services.EncryptionService
{
    public interface IEncryptionService
    {
        string EncryptString(string plainText);
        string DecryptString(string cipherText);
    }
}