using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Encryption
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
    public class CryptAlgorithm
    {
        uint[] k;
        byte[,] s;
        public CryptAlgorithm(uint[] k)
        {
            this.k = k;
            this.s = new byte[8, 16]
            {
                { 4, 10, 9, 2, 13, 8, 0, 14, 6, 11, 1, 12, 7, 15, 5, 3 },
                { 14, 11, 4, 12, 6, 13, 15, 10, 2, 3, 8, 1, 0, 7, 5, 9 },
                { 5, 8, 1, 13, 10, 3, 4, 2, 14, 15, 12, 7, 6, 0, 9, 11 },
                { 7, 13, 10, 1, 0, 8, 9, 15, 14, 4, 6, 12, 11, 2, 5, 3 },
                { 6, 12, 7, 1, 5, 15, 13, 8, 4, 10, 9, 14, 0, 3, 11, 2 },
                { 4, 11, 10, 0, 7, 2, 1, 13, 3, 6, 8, 5, 9, 12, 15, 14 },
                { 13, 11, 4, 1, 3, 15, 5, 9, 0, 10, 14, 7, 6, 8, 2, 12 },
                { 1, 15, 13, 0, 5, 7, 10, 4, 9, 2, 3, 14, 6, 11, 8, 12 }
            };
        }
        public ulong EncryptBlock(ulong x)
        {
            uint left = (uint)(x >> 32);
            uint right = (uint)(x << 32 >> 32);
            ulong sum;
            for (int i = 0; i < 32; i++)
            {
                int j;
                if (i < 24)
                    j = i % 8;
                else
                    j = 31 - i;
                sum = left + k[j];
                sum = sum % uint.MaxValue;
                uint f = Func((uint)sum);
                uint next = f ^ right;
                right = left;
                left = next;                
            }
            sum = right;
            sum <<= 32;
            sum += left;
            return sum;
        }
        public ulong DecryptBlock(ulong x)
        {
            uint left = (uint)(x >> 32);
            uint right = (uint)(x << 32 >> 32);
            ulong sum;
            for (int i = 0; i < 32; i++)
            {
                int j;
                if (i < 8)
                    j = i;
                else
                    j = (31 - i) % 8;
                sum = left + k[j];
                sum = sum % uint.MaxValue;
                uint f = Func((uint)sum);
                uint next = f ^ right;
                right = left;
                left = next;
            }
            sum = right;
            sum <<= 32;
            sum += left;
            return sum;
        }
        public uint Func(uint x)
        {
            uint res = 0;
            for (int i = 0; i < 8; i++)
            {
                uint cur = x << 4 * i >> 28;
                res += (uint)(s[i, cur]) << 4 * (7 - i);
            }
            res = (res << 11) + (res >> 21);
            return res;
        }
    }
}