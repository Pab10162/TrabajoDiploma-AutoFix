using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class BLL_BackUpRestore_96_PP
    {
        private DAL_BackUpRestore_96_PP dalbackup = new DAL_BackUpRestore_96_PP();

        public static void Instalador(string instancia)
        {
            DAL_BackUpRestore_96_PP a = new DAL_BackUpRestore_96_PP();
            if (a.RealizarRestoreIniciar(instancia))
                new BLL_DigitoVerificador_96_PP().RecalcularDV();
        }

        public void RealizarBackup(string backupPath)
        {
            dalbackup.RealizarBackup(backupPath);
        }
        public void RealizarRestore(string restorePath)
        {
            dalbackup.RealizarRestore(restorePath);
        }
    }
}
