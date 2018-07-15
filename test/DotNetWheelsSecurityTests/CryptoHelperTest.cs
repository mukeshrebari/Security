using DotNetWheels.Core.IO.Extensions;
using DotNetWheels.Security;
using System;
using System.IO;
using System.Text;
using Xunit;

namespace DotNetWheelsSecurityTests
{
    public class CryptoHelperTest
    {
        [Fact]
        public void TestEncryptString()
        {
            String rawText = "I love you!";
            var result = CryptoHelper.Encrypt(rawText, "123");
            Assert.Equal("YzIyZmUwMjgyYzdiNDViMOMa8uurpqh1aDwgMHqoBys_", result.Value);
        }

        [Fact]
        public void TestDecryptString()
        {
            String encryptedText = "YzIyZmUwMjgyYzdiNDViMOMa8uurpqh1aDwgMHqoBys_";
            var result = CryptoHelper.Decrypt(encryptedText, "123");
            Assert.Equal("I love you!", result.Value);
        }

        [Fact]
        public void TestEncryptStream()
        {
            var rawData = Encoding.ASCII.GetBytes("I love you, My Girl!");

            Byte[] encryptedData = null;
            using (var ms = new MemoryStream(rawData))
            {
                var data = CryptoHelper.Encrypt(ms, "123");
                encryptedData = data.Value;
            }

            Assert.NotNull(encryptedData);

            var result = String.Join(",", encryptedData);
            Assert.Equal("52,56,101,50,54,53,100,98,49,50,49,102,99,48,99,56,83,226,11,63,178,55,68,174,9,205,157,216,7,56,248,101,162,46,139,22,242,158,10,68,203,157,30,116,157,219,247,110", result);
        }

        [Fact]
        public void TestDecryptStream()
        {
            String[] chars = "52,56,101,50,54,53,100,98,49,50,49,102,99,48,99,56,83,226,11,63,178,55,68,174,9,205,157,216,7,56,248,101,162,46,139,22,242,158,10,68,203,157,30,116,157,219,247,110".Split(',');
            Byte[] encryptedData = new Byte[chars.Length];
            for (var i = 0; i < chars.Length; i++)
            {
                encryptedData[i] = Byte.Parse(chars[i]);
            }

            var decryptedData = CryptoHelper.Decrypt(encryptedData, "123");
            var result = Encoding.ASCII.GetString(decryptedData.Value);

            Assert.Equal("I love you, My Girl!", result);
        }

        [Fact]
        public void TestEncryptFile()
        {
            String filePath = @"F:\需要加密的文件\IMG_2571.JPG";
            Byte[] encryptedData = null;

            using (var fs = File.Open(filePath, FileMode.Open, FileAccess.Read))
            {
                var data = CryptoHelper.Encrypt(fs, "1234");
                encryptedData = data.Value;
            }

            Assert.True(encryptedData != null && encryptedData.Length > 0);

            File.WriteAllBytes(@"F:\需要加密的文件\IMG_2571_enc.JPG", encryptedData);
        }

        [Fact]
        public void TestDecryptFile()
        {
            String encryptedFilePath = @"F:\需要加密的文件\IMG_2571_enc.JPG";
            Byte[] decryptedData = null;

            using (var fs = File.Open(encryptedFilePath, FileMode.Open, FileAccess.Read))
            {
                var data = CryptoHelper.Decrypt(fs, "1234");
                decryptedData = data.Value;
            }

            Assert.True(decryptedData != null && decryptedData.Length > 0);

            File.WriteAllBytes(@"F:\需要加密的文件\IMG_2571_enc_dec.JPG", decryptedData);
        }
    }
}
