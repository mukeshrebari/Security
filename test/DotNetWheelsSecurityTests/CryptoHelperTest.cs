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
            String result = CryptoHelper.Encrypt(rawText, "123");
            Assert.Equal("YzIyZmUwMjgyYzdiNDViMOMa8uurpqh1aDwgMHqoBys_", result);
        }

        [Fact]
        public void TestDecryptString()
        {
            String encryptedText = "YzIyZmUwMjgyYzdiNDViMOMa8uurpqh1aDwgMHqoBys_";
            String rawText = CryptoHelper.Decrypt(encryptedText, "123");
            Assert.Equal("I love you!", rawText);
        }

        [Fact]
        public void TestEncryptStream()
        {
            var rawData = Encoding.ASCII.GetBytes("I love you, My Girl!");

            Byte[] encryptedData = null;
            using (var ms = new MemoryStream(rawData))
            {
                encryptedData = CryptoHelper.Encrypt(ms, "123");
            }

            Assert.NotNull(encryptedData);

            var result = String.Join(",", encryptedData);
            Assert.Equal("52,56,101,50,54,53,100,98,49,50,49,102,99,48,99,56", result);
        }

        [Fact]
        public void TestDecryptStream()
        {
            String[] chars = "52,56,101,50,54,53,100,98,49,50,49,102,99,48,99,56".Split(',');
            Byte[] encryptedData = new Byte[chars.Length];
            for (var i = 0; i < chars.Length; i++)
            {
                encryptedData[i] = Byte.Parse(chars[i]);
            }

            var decryptedData = CryptoHelper.Decrypt(encryptedData, "123");
            var result = Encoding.UTF8.GetString(decryptedData);

            Assert.Equal("I love you, My Girl!", result);
        }
    }
}