using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TestHilos
{
    public partial class Form1 : Form
    {
        CancellationTokenSource cts = new CancellationTokenSource();
        delegate void SetTextCallback(string text);
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            textBox1.Text = string.Empty;
            button1.Enabled = false;
            button2.Enabled = true;
            ThreadPool.QueueUserWorkItem(new WaitCallback(DoSomeWork), cts.Token);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            button1.Enabled = true;
            button2.Enabled = false;
            cts.Cancel();
        }

         void DoSomeWork(object obj)
        {
            CancellationToken token = (CancellationToken)obj;
            int count = 0;
            while (!token.IsCancellationRequested)
            {
                count++;
                Thread.Sleep(1000);
                SetText($"LLamada {count} {Environment.NewLine}");
            }
            SetText($"Hilo terminado");
        }

        private  void SetText(string text)
        {
            if (this.textBox1.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(SetText);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                this.textBox1.Text += text;
            }
        }
    }
}
