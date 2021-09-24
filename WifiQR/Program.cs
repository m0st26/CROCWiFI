using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Fclp;
using QRCoder;
using static QRCoder.PayloadGenerator;

namespace WifiQR
{
    class Program
    {
        static void Main(string[] args)
        {
            //string path = @"C:\123\test.txt"; //в прод вставить C:\Users\Administrator\Desktop\wifi\wifi.txt
            string path = @"C:\Users\Administrator\Desktop\wifi\wifi.txt";
            string password = "";
            string ssid = "DC_WiFi"; 
            //string ssid = "solo322 (5g)";
            string filename = @"C:\Users\Administrator\Desktop\wifi\wifi.png";
           
            try
            {
                using (StreamReader sr = new StreamReader(path))
                {
                    password = sr.ReadLine(); 
                    int lenght = password.Length;
                    int index = password.IndexOf("wifi")+7;
                    password = password.Replace(" ", "");
                    password = password.Substring(index);
                    password = password.Remove(8,4);
                    Console.WriteLine(password); 
                    //Console.ReadKey();
                }
            }

            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            var parser = new FluentCommandLineParser();
            parser.IsCaseSensitive = false;
            parser.Setup<string>('s', "SSID")
                .Callback(v => ssid = v);
            parser.Setup<string>('p', "password")
                .Callback(v => password = v);
            parser.Setup<string>('f', "filename")
                .Callback(v => filename = v);
            var result = parser.Parse(args);
            if (result.HasErrors)
            {
                Console.Error.WriteLine(result.ErrorText);
            }
            else
            {
                var obfuscatedPassword = string.Join(
                    string.Empty, 
                    Enumerable.Repeat('*', password.Length));
                Console.WriteLine($"{ssid}:{obfuscatedPassword}");
            }

            WiFi generator = new WiFi(ssid, password, WiFi.Authentication.WPA);
            string payload = generator.ToString();

            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(payload, QRCodeGenerator.ECCLevel.Q);
            QRCode qrCode = new QRCode(qrCodeData);
            var qrCodeAsBitmap = qrCode.GetGraphic(20);
            qrCodeAsBitmap.Save(filename, ImageFormat.Png);
            //Console.ReadKey();
        }
    }
}
