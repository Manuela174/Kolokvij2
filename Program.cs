using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kolokvi2ponavljanje
{
    class Program
    {

        static string pretvorbaMB(long B)
        {

            return ((float)B/(1024*1024)).ToString("0.00")+ "MB";
        }
        static void Main(string[] args)
        {
           string prvi= "";//prvi argument
            try
            {
                prvi = args[0];
            }
            catch
            {

            }

            string drugi = "";//drugi argument
            try
            {
                drugi = args[1];
            }
            catch
            {

            }

            if (drugi == "zaustavi")
            {
                Console.WriteLine("Drugi argument je zaustavljen");
            }
            else
            {
                Console.WriteLine("Drugi argument nije zaustavljen");
            }

            Process[] listaProcesa = Process.GetProcesses().OrderByDescending(proc=>proc.WorkingSet64).ToArray();
            Console.WriteLine("{0,-3} {1,10} {2,-25} {3,13} {4,-20}", "RBR", "PID", "Naziv procesa", "Memorija[MB]", "Proces zaustavlja");
            Console.WriteLine("=== ========== ========================= ============= =================");

            int brojac = 0;
            int brojzaustavljenih = 0;
            long selekt = 0;
            long oslobodena = 0;
            long svi = 0;
            

            foreach (Process proc in listaProcesa)
            {
                if(proc.ProcessName.ToLower() == prvi.ToLower())
                {
                    selekt += proc.WorkingSet64;
                    bool uspjesnoizvrsio = false;

                    try
                    {
                        proc.Kill();
                        brojzaustavljenih++;
                        oslobodena += proc.WorkingSet64;
                        uspjesnoizvrsio = true;
                    }
                    catch
                    {
                       
                    }
                    Console.WriteLine("{0,-3} {1,10} {2,-25} {3,13} {4,20}", brojac, proc.Id, proc.ProcessName, pretvorbaMB(proc.WorkingSet64), uspjesnoizvrsio?"DA":"NE");
                    brojac++;

                }

                if (prvi =="")
                {
                    Console.WriteLine("{0,-3} {1,10} {2,-25} {3,13}", brojac, proc.Id, proc.ProcessName, pretvorbaMB(proc.WorkingSet64));
                    brojac++;

                }
                svi += proc.WorkingSet64;
            }
            Console.WriteLine("=== ========== ========================= ============= =================");
            Console.WriteLine("Broj traženih/ Broj zaustavljenih/ Svi procesi: {0}/ {1}/ {2}",brojac, brojzaustavljenih,listaProcesa.Length);
            Console.WriteLine("Veličina selektiranih/ Oslobođena memorija/ Svi: {0}/ {1}/ {2}", pretvorbaMB(selekt), pretvorbaMB(oslobodena), pretvorbaMB(svi));
        }
    }
}
