using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;
using System.Data;
using System;
using System.Diagnostics;
using System.Media;
using System.Threading;

namespace F4toA3Monitor
{
    class falconCustomLocator
    {

        public static void Start( monitorUi userDisplay )
        {
            FalconDataFormats source = new FalconDataFormats();

            Reader memReader = new Reader(source);

            Process[] processes = Process.GetProcessesByName("Falcon BMS");

            while (processes.Length < 1)
            {
                processes = Process.GetProcessesByName("Falcon BMS");

                System.Threading.Thread.Sleep(3000);
            }

            if (processes.Length > 0)
            {

                System.Diagnostics.Process eqproc = processes[0];

                int addrname = 0x4A2E848;

                while (true)
                {

                    MemoryLoc Pmhp3 = new MemoryLoc(eqproc, addrname);
                    // string nameData = Pmhp3.getString(100, false);
   
                    var data1 = memReader.GetCurrentData();

                    int newX = System.Convert.ToInt32((data1.y / 1640) - 450);
                    int newY = System.Convert.ToInt32((data1.x / 1640) - 1800);

                    userDisplay.updateLocationX(newX);
                    userDisplay.updateLocationY(newY);

                    System.Threading.Thread.Sleep(250);
                }
            }

            while (true)
            {
                System.Threading.Thread.Sleep(3000);
            }

        }
    }
}
