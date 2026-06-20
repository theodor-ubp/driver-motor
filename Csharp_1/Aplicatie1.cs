using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.IO.Ports;
using System.Runtime.InteropServices;

namespace Csharp_1
{
    public partial class Aplicatie1 : Form
    {

        //DEFINIRE VARIABILE GLOBALE
        //*********************************************************
        byte[] INAINTE = new byte[1];
        byte[] STOP = new byte[1];
        byte[] RESET = new byte[1];
        byte SPEED_TMP = 0; //viteza PWM

        //********************************************************


        public Aplicatie1()
        {
            InitializeComponent();
            //this.WindowState = FormWindowState.Maximized;

            List<String> tList = new List<String>();

            bluetoothBox.Items.Clear();

            foreach (string s in SerialPort.GetPortNames())
            {
                tList.Add(s);
            }

            tList.Sort();
            bluetoothBox.Items.AddRange(tList.ToArray());
            /*bluetoothBox.SelectedIndex = 0;*/

            serialPort.BaudRate = 9600;


            labelSpeed.Text = "Viteză de rotație: [rot/min]"/* + tBar.Value*12*/;
            STOP[0] = 0;
            RESET[0] = 2;
            SPEED_TMP = (byte)tBar.Value;
            //INAINTE[0] = (byte)(SPEED_TMP);
            INAINTE[0] = (byte)(10 + SPEED_TMP);
        }

        private void bluetoothBox_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.KeyCode == Keys.Down) || (e.KeyCode == Keys.Up))
                e.Handled = true;
        }



        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            serialPort.Write(STOP, 0, 0);
            serialPort.Close();
            timer.Stop();
        }

        private void tBar_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.KeyCode == Keys.Down) || (e.KeyCode == Keys.Up))
                e.Handled = true;
        }


        private void tBar_ValueChanged(object sender, EventArgs e)
        {
            SPEED_TMP = (byte)tBar.Value;
            INAINTE[0] =    (byte)(10 + SPEED_TMP);
            if (serialPort.IsOpen)
            {
                serialPort.Write(INAINTE, 0, 1);
            }
            labelSpeed.Text = "Viteză de rotație: [rot/min]"/* + tBar.Value*12*/;
        }

        private void BLUETOOTHstartButton_Click(object sender, EventArgs e)
        {
            if (bluetoothBox.SelectedText != "COM1" || bluetoothBox.SelectedText != "COM3")
            {
                if (serialPort.IsOpen)
                {
                    serialPort.Write(STOP, 0, 1);
                    serialPort.Close();
                    timer.Stop();
                    BLUETOOTHstartButton.Text = "Start";
                }
                else
                {
                    serialPort.Open();
                    timer.Start();
                    serialPort.Write(INAINTE, 0, 1);
                    BLUETOOTHstartButton.Text = "Stop";

                }
            }
            //{
            //    if (!serialPort.IsOpen)
            //    {
            //        serialPort.Open();
            //        timer.Start();
            //        serialPort.Write(INAINTE, 0, 1);
            //        BLUETOOTHstartButton.Text = "STOP";
            //    }
            //    else
            //    {
            //        serialPort.Close();
            //        timer.Stop();
            //        //serialPort.Write(STOP, 0, 1);
            //        BLUETOOTHstartButton.Text = "START";
            //    }
            //}
            else MessageBox.Show("Va rog sa alegeti portul");

        }

        private void bluetoothBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (bluetoothBox.SelectedText != "COM1" || bluetoothBox.SelectedText != "COM3")
            {
                serialPort.PortName = (string)bluetoothBox.SelectedItem;
            }
        }


        private DateTime ceas;
        private string senzor;



        void serialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            senzor = serialPort.ReadLine();
            this.Invoke(new EventHandler(displaydate_event));
        }

        private void displaydate_event(object sender, EventArgs e)
        {
            date1.AppendText("Senzor 1");
            date1.AppendText("\t");
            ceas = DateTime.Now;
            if(DateTime.Now.Day < 10)
                date1.AppendText("0" + DateTime.Now.Day.ToString() + ".");
            else date1.AppendText(DateTime.Now.Day.ToString() + ".");
            if(DateTime.Now.Month < 10)
                date1.AppendText("0" + DateTime.Now.Month.ToString() + ".");
            else date1.AppendText(DateTime.Now.Month.ToString() + ".");
            date1.AppendText(DateTime.Now.Year.ToString());
            date1.AppendText("\t");
            if(DateTime.Now.Hour < 10)
                date1.AppendText("0" + DateTime.Now.Hour.ToString() + ":");
            else date1.AppendText(DateTime.Now.Hour.ToString() + ":");
            if (DateTime.Now.Minute < 10)
                date1.AppendText("0" + DateTime.Now.Minute.ToString() + ":");
            else date1.AppendText(DateTime.Now.Minute.ToString() + ":");
            if (DateTime.Now.Second < 10)
                date1.AppendText("0" + DateTime.Now.Second.ToString() + ".");        
            else date1.AppendText(DateTime.Now.Second.ToString() + ".");
            if (DateTime.Now.Millisecond < 10 && DateTime.Now.Millisecond < 100)
                date1.AppendText("00" + DateTime.Now.Millisecond.ToString());
            else
            if (DateTime.Now.Millisecond > 10 && DateTime.Now.Millisecond < 100)
                date1.AppendText("0" + DateTime.Now.Millisecond.ToString());
            else
                date1.AppendText(DateTime.Now.Millisecond.ToString());          
            date1.AppendText("\t");
            date1.AppendText("Abatere:");
            date1.AppendText("\t");
            date1.AppendText(senzor);
            date1.AppendText("\t");
            date1.AppendText("mm");
            date1.AppendText("\n");
          int valori_date1 = Convert.ToInt32(senzor);
        }


        private void save_Click(object sender, EventArgs e)
        {
            string locatie = @"D:\date_senzor.txt";
            string numefisier1 = "date_senzor.txt";
            System.IO.File.WriteAllText(locatie, date1.Text);
            MessageBox.Show("Datele au fost salvate in " + locatie, "Salvare Date");
        }


      


 public struct KeyStateInfo
    {
        private Keys key;
        private bool isPressed;
        private bool isToggled;
 
        public KeyStateInfo(Keys key, bool ispressed, bool istoggled)
        {
            this.key = key;
            isPressed = ispressed;
            isToggled = istoggled;
        }
 
        public static KeyStateInfo Default
        {
            get
            {
                return new KeyStateInfo(Keys.None, false, false);
            }
        }
 
        public Keys Key
        {
            get { return key; }
        }
 
        public bool IsPressed
        {
            get { return isPressed; }
        }
 
        public bool IsToggled
        {
            get { return isToggled; }
        }
    }
    
public class KeyboardInfo
    {
        [DllImport("user32")]
        private static extern short GetKeyState(int vKey);

        private KeyboardInfo()
        {

        }

        public static KeyStateInfo GetKeyState(Keys key)
        {
            short keyState = GetKeyState((int)key);
            int low = Low(keyState), high = High(keyState);
            bool toggled = low == 1;
            bool pressed = high == 1;
            return new KeyStateInfo(key, pressed, toggled);
        }

        private static int High(int keyState)
        {
            return keyState > 0 ? keyState >> 0x10
                    : (keyState >> 0x10) & 0x1;
        }

        private static int Low(int keyState)
        {
            return keyState & 0xffff;
        }
    }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
        private void date1_TextChanged(object sender, EventArgs e)
        {

        }

        private void tBar_Scroll(object sender, EventArgs e)
        {

        }

        private void labelSpeed_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            MessageBox.Show("DC Motor Control System\n\nArduino + C# GUI\nTheodor Andrei, 2017","Despre");
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click_1(object sender, EventArgs e)
        {
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
        }

        private void button3_Click(object sender, EventArgs e)
        {
           MessageBox.Show("1. Înainte de a achiziționa date, puteți nota în caseta albă informații suplimentare (nume, model disc, etc.) pentru identificare simplificată.\n\n2. În cazul în care datele nu sunt afișate corect, încercați utilizarea unui alt program de vizualizare a textului, cum ar fi Wordpad.\n\n3. Datele achiziționate pot fi importate cu ușurință într-un program de calcul tabelar, cum ar fi Microsoft Excel.", "Info, sugestii");
        }

        private void button4_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Pentru a afla care este portul de comunicație între sistem și calculatorul dvs., accesați „Device Manager” din panoul de control al sistemului de operare, după ce ați conectat aparatul la o sursă de tensiune și ați realizat împerecherea Bluetooth.","Informații despre porturi");
        }

        private void button5_Click(object sender, EventArgs e)
        {
            date1.Clear();
            MessageBox.Show("Ați șters datele achiziționate pentru această sesiune!","Resetare");
        }

        private void button6_Click(object sender, EventArgs e)
        {
      
                serialPort.Write(RESET, 0, 1);
        }
    }
}
