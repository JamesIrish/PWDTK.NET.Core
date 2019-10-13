using System;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Text;
using NUnit.Framework;

namespace Test.PWDTK.NET
{
    public class HexFunctionTests
    {
        [Test]
        public void GivenStandardByteArray_WhenFormattedToHex_ThenBothLegacyAndNewMethodsProduceTheSameResult()
        {
            // Arrange
            var salt = PWDTK_DOTNET451.PWDTK.GetRandomSalt();

            // Act
            var legacy = new SoapHexBinary(salt).ToString();
            var update = HashBytesToHexString(salt);

            // Assert
            Assert.AreEqual(legacy, update);
        }

        [Test]
        public void GivenHexString_WhenConvertedToBytes_ThenBothLegacyAndNewMethodsProduceTheSameResult()
        {
            // Arrange
            const string hex = @"B17E31B60452E9ECFBFEAC59A62C2A619C9E1632204FCDD49614DC0838C0F01B38D3D3E073C9BF551C063F4C7F815317FA9670DA9E0B8B4C57C047E6E1EFE1F6";

            // Act
            var legacy = SoapHexBinary.Parse(hex).Value;
            var update = HashHexStringToBytes(hex);

            // Assert
            Assert.AreEqual(legacy, update);
        }

        /// <summary>
        /// Converts the Byte array Hash into a Human Friendly HEX String
        /// </summary>
        /// <param name="hash">The Hash value to convert</param>
        /// <returns>A HEX String representation of the Hash value</returns>
        public static string HashBytesToHexString(byte[] hash)
        {
            var hex = new StringBuilder(hash.Length * 2);
            foreach (var b in hash)
                hex.AppendFormat("{0:x2}", b);
            return hex.ToString();
        }

        /// <summary>
        /// Converts the Hash Hex String into a Byte[] for computational processing
        /// </summary>
        /// <param name="hashHexString">The Hash Hex String to convert back to bytes</param>
        /// <returns>Essentially reverses the HashToHexString function, turns the String back into Bytes</returns>
        public static byte[] HashHexStringToBytes(string hashHexString)
        {
            var numberChars = hashHexString.Length;
            var bytes = new byte[numberChars / 2];
            for (var i = 0; i < numberChars; i += 2)
                bytes[i / 2] = Convert.ToByte(hashHexString.Substring(i, 2), 16);
            return bytes;
        }
    }
}