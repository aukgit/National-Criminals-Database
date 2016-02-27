using System;
using System.IO;
using DevMvcComponent;
using DevMvcComponent.Encryption;
namespace PasswordEncrypt {
    class Program {
        static void Main(string[] args) {
            Mvc.Setup(System.Reflection.Assembly.GetExecutingAssembly());
            string passPhraseFileName = "passPhrase.txt";
            string encryptedFileName = "encrypted.txt";

            while (true) {
                var prevPassPhrase = File.Exists(passPhraseFileName) ? File.ReadAllText(passPhraseFileName) : "";
                var prevEncryptedtext = File.Exists(encryptedFileName) ? File.ReadAllText(encryptedFileName) : "";
                if (!string.IsNullOrEmpty(prevPassPhrase)) {
                    Console.WriteLine("Previous Encrypted String:");
                    Console.WriteLine(prevPassPhrase);
                    Console.WriteLine("Previous Encrypted String as Decrypted:");
                    string tempDecryptedstring = Pbkdf2Encryption.Decrypt(prevEncryptedtext, prevPassPhrase);
                    Console.WriteLine(tempDecryptedstring);
                }
                Console.WriteLine("Please enter a pass phrase to use:");
                string passphrase = Console.ReadLine();
                Console.WriteLine("Please enter a string to encrypt:");
                string encryptingString = Console.ReadLine();
                Console.WriteLine("");

                Console.WriteLine("Your encrypted string is:");
                string encryptedstring = Pbkdf2Encryption.Encrypt(encryptingString, passphrase);
                File.WriteAllText(encryptedFileName, encryptedstring);
                File.WriteAllText(passPhraseFileName, passphrase);

                Console.WriteLine("Saved respective strings into encrypted.txt and passPhrase.txt inside the executing folder.");
                Console.WriteLine(encryptedstring);
                Console.WriteLine("");

                Console.WriteLine("Your decrypted string is:");
                string decryptedstring = Pbkdf2Encryption.Decrypt(encryptedstring, passphrase);
                Console.WriteLine(decryptedstring);
                Console.WriteLine("");

                Console.WriteLine("Press any key to exit...");
                Console.ReadLine();
            }
        }
    }
}
