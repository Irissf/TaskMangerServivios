using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TaskMangerServivios
{
    public partial class Form1 : Form
    {

        Process[] processes = Process.GetProcesses();
        Process precessId;
        private int id = 0;
        public Form1()
        {
            InitializeComponent();
        }

        //MOSTRAR=================================================================================================================================
        private void onCLick(object sender, EventArgs e)
        {
            textBox1.Text = "";
            textBox1.Text += string.Format("{0,-20}{1,-40}{2}", "PID", "Nombre proceso", "Titulo Ventana") + "\r\n";
            textBox1.Text += string.Format("============================================================================================") + "\r\n";
            //regla
            //textBox1.Text += string.Format("0----5---10---15---20---25---30---35---40---45---50---55---60---65---70---75---80---85\r\n");
            for (int i = 0; i < processes.Length; i++)
            {
                string title = processes[i].MainWindowTitle;
                if (title.Length > 30)
                {
                    title = title.Substring(0, 30) + "...";

                }
                string cut = processes[i].ProcessName;
                if (cut.Length > 30)
                {
                    cut = cut.Substring(0, 30) + "...";

                }
                textBox1.Text += string.Format("{0,-20}{1,-40}{2}", processes[i].Id, cut, title) + "\r\n";
            }

        }

        //MOSTRAR SELECCIONADO====================================================================================================================
        private void FindProcessOnClic(object sender, EventArgs e)
        {

            bool processExists = false;
            if (textBox2.Text.Length > 0)
            {
                if (isIntCOrrectId(textBox2.Text))
                {
                    id = Convert.ToInt32(textBox2.Text);
                    if (isIntCOrrectId(textBox2.Text))
                    {
                        foreach (Process p in processes)
                        {
                            if (p.Id == id)
                            {
                                processExists = true;
                                ProcessThreadCollection threads = p.Threads;
                                textBox1.Text = "";
                                textBox1.Text += string.Format("The process is :{0}\r\nWhich name is:{1} ", id, p.ProcessName);
                                try
                                {
                                    textBox1.Text += string.Format("\r\nThe process start :{0}", p.StartTime);

                                }
                                catch (Win32Exception)
                                {
                                    textBox1.Text += string.Format("\r\nno time");
                                }
                                textBox1.Text += string.Format("\r\nSubprocess :\r\n");
                                foreach (ProcessThread item in threads)
                                {
                                    try
                                    {
                                        textBox1.Text += string.Format("ID {0},{1}\r\n", item.Id, item.StartTime);
                                    }
                                    catch (Win32Exception)
                                    {
                                        textBox1.Text += string.Format("ID {0} no time\r\n", item.Id);
                                    }
                                }
                                try
                                {
                                    ProcessModuleCollection modules = p.Modules;
                                    textBox1.Text += string.Format("\r\nModules :\r\n");
                                    foreach (ProcessModule item in modules)
                                    {
                                        textBox1.Text += string.Format("Name module:{0}\r\nname file{1}\r\n", item.ModuleName, item.FileName);
                                        textBox1.Text += string.Format("_______________________________________________________________________________________________________\r\n", item.ModuleName, item.FileName);
                                    }
                                }
                                catch (Win32Exception)
                                {
                                    string.Format("cannot be accessed to modules");
                                }

                            }

                        }
                        if (!processExists)
                        {
                            textBox1.Text = "the process does not exists";

                        }

                    }
                    else
                    {
                        textBox1.Text = "Error id";
                    }
                }
                else
                {
                    textBox1.Text = "The process does not exist";
                }
            }
            else
            {
                textBox1.Text = string.Format("The process does not exist, nothing exists, only emptiness...");
            }
        }

        //BORRAR==================================================================================================================================
        private void CloseProcess(object sender, EventArgs e)
        {
            if (textBox2.Text.Length > 0)
            {
                if (isIntCOrrectId(textBox2.Text)) //esto sería mejor en una función a parte puesto que lo hacemos en cada botón
                {
                    id = Convert.ToInt32(textBox2.Text);
                    precessId = Process.GetProcessById(id);

                    try
                    {
                        if (precessId.CloseMainWindow())
                        {
                            textBox1.Text = "The process was closed";
                        }
                        else
                        {
                            textBox1.Text = "the process was not closed";
                        }
                    }
                    catch (Exception)
                    {
                        textBox1.Text = "Error";
                    }


                }
                else
                {
                    textBox1.Text = ("Eror process id");
                }
            }
            else
            {
                textBox1.Text = ("Process does not exists");
            }
        }

        //FUNCIONES===============================================================================================================================
        public bool isIntCOrrectId(string text)
        {
            try
            {
                int id = Convert.ToInt32(text);

            }
            catch (FormatException)
            {
                return false;
            }
            catch (OverflowException)
            {
                return false;
            }

            return true;
        }

        private void KillProcess(object sender, EventArgs e)
        {
            if (textBox2.Text.Length > 0)
            {
                if (isIntCOrrectId(textBox2.Text)) //esto sería mejor en una función a parte puesto que lo hacemos en cada botón
                {
                    id = Convert.ToInt32(textBox2.Text);
                    precessId = Process.GetProcessById(id);

                    try
                    {
                        precessId.Kill();

                    }
                    catch (Win32Exception)
                    {
                        textBox1.Text = "Error";
                    }
                }
                else
                {
                    textBox1.Text = ("Eror process id");
                }
            }
            else
            {
                textBox1.Text = ("Process does not exists");
            }
        }

        private void StartProcess(object sender, EventArgs e)
        {
            if (textBox2.Text.Length > 0)
            {
                if (File.Exists(textBox2.Text))
                {
                    try
                    {
                        Process p = Process.Start(Path.GetFileName(textBox2.Text));
                    }
                    catch (Win32Exception)
                    {
                        textBox1.Text = "I cannot start the process";
                    }
                }
                else
                {
                    try
                    {
                        Process p = Process.Start(textBox2.Text);
                    }
                    catch (Win32Exception)
                    {
                        textBox1.Text = "I cannot start the process";
                    }
                }
            }
            else
            {
                textBox1.Text = "Write me";
            }


        }
    }

}

