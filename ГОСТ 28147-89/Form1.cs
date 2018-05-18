using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace Encryption
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private uint[] ReadKeys()
        {
            uint[] res = new uint[8];
            try
            {
                res[0] = UInt32.Parse(textBox1.Text);
                res[1] = UInt32.Parse(textBox2.Text);
                res[2] = UInt32.Parse(textBox3.Text);
                res[3] = UInt32.Parse(textBox4.Text);
                res[4] = UInt32.Parse(textBox5.Text);
                res[5] = UInt32.Parse(textBox6.Text);
                res[6] = UInt32.Parse(textBox7.Text);
                res[7] = UInt32.Parse(textBox8.Text);
                return res;
            }
            catch
            {
                return null;
            }
        }
        private void encryptToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                    {
                        FileStream fs1 = new FileStream(openFileDialog1.FileName, FileMode.Open);
                        FileStream fs2 = new FileStream(saveFileDialog1.FileName, FileMode.OpenOrCreate);
                        BinaryReader br = new BinaryReader(fs1);
                        BinaryWriter bw = new BinaryWriter(fs2);
                        uint[] k = ReadKeys();
                        if (k == null)
                        {
                            MessageBox.Show("Неверно задан ключ");
                            return;
                        }
                        CryptAlgorithm ca = new CryptAlgorithm(k);
                        try
                        {
                            while (true)
                            {
                                ulong p = br.ReadUInt64();
                                ulong c = ca.EncryptBlock(p);
                                bw.Write(c);
                            }
                        }
                        catch
                        {
                            MessageBox.Show("Шифрование закончено");
                        }
                        fs1.Close();
                        fs2.Close();
                    }
                }
            }
            catch
            {
                MessageBox.Show("Ошибка, шифрование может быть не закончено");
            }
        }
        private void decryptToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                    {
                        FileStream fs1 = new FileStream(openFileDialog1.FileName, FileMode.Open);
                        FileStream fs2 = new FileStream(saveFileDialog1.FileName, FileMode.OpenOrCreate);
                        BinaryReader br = new BinaryReader(fs1);
                        BinaryWriter bw = new BinaryWriter(fs2);
                        uint[] k = ReadKeys();
                        if (k == null)
                        {
                            MessageBox.Show("Неверно задан ключ");
                            return;
                        }
                        CryptAlgorithm ca = new CryptAlgorithm(k);
                        try
                        {
                            while (true)
                            {
                                ulong c = br.ReadUInt64();
                                ulong p = ca.DecryptBlock(c);
                                bw.Write(p);
                            }
                        }
                        catch
                        {
                            MessageBox.Show("Дешифрование закончено");
                        }
                        fs1.Close();
                        fs2.Close();
                    }
                }
            }
            catch
            {
                MessageBox.Show("Ошибка, дешифрование может быть не закончено");
            }
        }
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}