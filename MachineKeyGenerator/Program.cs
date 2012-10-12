using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.Windows;

namespace MachineKeyGenerator
{
    class Program
    {
        [STAThread]
        static void Main(string[] argv)
        {
            int len = 128;
            if (argv.Length > 0)
                len = int.Parse(argv[0]);
            byte[] buff = new byte[len / 2];
            RNGCryptoServiceProvider rng = new
                                    RNGCryptoServiceProvider();
            rng.GetBytes(buff);
            StringBuilder sb = new StringBuilder(len);
            for (int i = 0; i < buff.Length; i++)
                sb.Append(string.Format("{0:X2}", buff[i]));

            Clipboard.SetText(sb.ToString());

            Console.WriteLine("New key generated and copied to the clipboard:");
            Console.WriteLine(sb);
            Console.WriteLine();
            Console.WriteLine("Press ENTER to exit.");
            Console.ReadLine();
        }
    }
}
