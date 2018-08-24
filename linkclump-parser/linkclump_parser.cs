using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace linkclump_parser
{
    public partial class linkclump_parser : Form
    {
        [DllImport("User32.dll")]
        private static extern int SetClipboardViewer(int hWndNewViewer);
        [DllImport("User32.dll", CharSet = CharSet.Auto)]
        private static extern bool ChangeClipboardChain(IntPtr hWndRemove, IntPtr hWndNewNext);
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern int SendMessage(IntPtr hwnd, int wMsg, IntPtr wParam, IntPtr lParam);
        private IntPtr nextClipboardViewer;
        private string fileName = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName +"\\clinkclump-" + Guid.NewGuid() + ".txt";
        private List<string> rawstrings = new List<string>();
        private bool removeonloaddata = false;
        public linkclump_parser()
        {
            InitializeComponent();
            nextClipboardViewer = (IntPtr)SetClipboardViewer((int)Handle);
            File.Create(fileName).Dispose();
        }
        private void Parseclipboarddata()
        {
            if (removeonloaddata)
            {
                IDataObject iData = new DataObject();
                iData = Clipboard.GetDataObject();

                if (iData.GetDataPresent(DataFormats.Text))
                {
                    var data = iData.GetData(DataFormats.Text).ToString()
                        .Split(new[] { Environment.NewLine }, StringSplitOptions.None);

                    data = data.Where(x => !string.IsNullOrEmpty(x)).Distinct().ToArray();
                    foreach (var val in data)
                    {
                        rawstrings.Add(val);
                    }
                    if (rawstrings.Count > 100)
                        executeSave();
                }
            }
            removeonloaddata = true;
        }
        private void executeSave()
        {
            if (File.Exists(fileName))
            {
                File.AppendAllLines(fileName, rawstrings.Distinct().ToList());
                rawstrings.Clear();
            }
            else
            {
                File.Create(fileName).Dispose();
                executeSave();
            }
        }
        protected override void WndProc(ref Message m)
        {
            const int WM_DRAWCLIPBOARD = 0x308;
            const int WM_CHANGECBCHAIN = 0x030D;

            switch (m.Msg)
            {
                case WM_DRAWCLIPBOARD:
                    Parseclipboarddata();
                    SendMessage(nextClipboardViewer, m.Msg, m.WParam,
                                m.LParam);
                    break;

                case WM_CHANGECBCHAIN:
                    if (m.WParam == nextClipboardViewer)
                        nextClipboardViewer = m.LParam;
                    else
                        SendMessage(nextClipboardViewer, m.Msg, m.WParam,
                                    m.LParam);
                    break;

                default:
                    base.WndProc(ref m);
                    break;
            }

        }
        protected override void Dispose(bool disposing)
        {
            if (rawstrings.Count > 0)
                executeSave();

            if (disposing && (components != null))
                components.Dispose();

            ChangeClipboardChain(Handle, nextClipboardViewer);
            base.Dispose(disposing);
        }
    }
}
