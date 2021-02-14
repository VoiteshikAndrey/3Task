using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace _3TASK
{
    class Program
    {
        static void Main(string[] args)
        {
            Random rnd = new Random();

            args = IsCorrect(args);

            int computer = rnd.Next(1, args.Length);

            string HMACkey = HMACkeyGenerator();
            string HMAC = HMACGenerator(args[computer-1], HMACkey);
            Console.WriteLine("HMAC: " + HMAC);

            int choise = Menu(args);
            if (choise == 0)
                return;

            Console.WriteLine("Your choise - " + args[choise-1]);
            Console.WriteLine("Computer choise - " + args[computer-1]);

            Console.WriteLine(Logic(choise, computer, args.Length));
            Console.WriteLine("HMAC key: " + HMACkey);
        }

        static int Menu(string[] arr)
        {
            int choise = -1;
            Console.WriteLine("\n-----------------");
            for (int i = 0; i < arr.Length; i++)
            {
                Console.WriteLine(Convert.ToString(i + 1) + " - " + arr[i]);
            }
            Console.WriteLine("0 - exit" +
                            "\n-----------------" +
                            "\nEnter your move: ");
            try
            {
                choise = Convert.ToInt32(Console.ReadLine());
                if (choise > arr.Length)
                {
                    Console.WriteLine("Invalid input!!!");
                    return Menu(arr);
                }
            }
            catch
            {
                Console.WriteLine("Invalid input!!!");
                return Menu(arr);
            }
           
            return choise;
        }

        static string[] IsCorrect(string[] arr)
        {
            if(arr.Length % 2 != 0 && arr.Length > 1 && !CheckForDuplicates(arr))
            {
                return arr;
            }
            else
            {
                Console.WriteLine("Input Error!!!" +
                    "\n1) The number of elements must be odd and greater than 1!!!" +
                    "\n2)Elements must not be repeated" +
                    "\nExample: 1 2 3 4 5 ");
                arr = Console.ReadLine().Split();
                return IsCorrect(arr);
            }
        }
        static bool CheckForDuplicates(string[] array)
        {
            HashSet<string> hashTable = new HashSet<string>();

            foreach (var item in array)
                if (!hashTable.Add(item))
                    return true;
            return false;
        }

        static string HMACGenerator(string str, string key)
        {
            byte[] bkey = Encoding.Default.GetBytes(key);
            using (var hmac = new HMACSHA256(bkey))
            {
                byte[] bstr = Encoding.Default.GetBytes(str);
                var bhash = hmac.ComputeHash(bstr);
                return BitConverter.ToString(bhash).Replace("-", string.Empty).ToUpper();
            }
        }

        static string HMACkeyGenerator()
        {
            RNGCryptoServiceProvider rngCsp = new RNGCryptoServiceProvider();

            byte[] bytekey = new byte[16];
            rngCsp.GetBytes(bytekey);
            string HMACkey = "";
            foreach (byte i in bytekey)
            {
                HMACkey += String.Format("{0:X2}", i);
            }

            return HMACkey;
        }

        static string Logic(int choise, int computer, int len)
        {
            string result = "";

            if (choise == computer)
                result = "did not win, but you didn’t lose either. Draw!!!";
            else if (Math.Abs(choise - computer) <= (len - 1) / 2)
            {
                if (choise > computer)
                    result = "win!";
                else
                    result = "lose(";
            }
            else
            {
                if (choise > computer)
                    result = "lose(";
                else
                    result = "win!";
            }

            return "You " + result;
        }
    }
}
