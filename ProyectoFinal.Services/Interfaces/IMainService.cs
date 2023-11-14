using ProyectoFinal.Services.Services;
using ProyectoFinal.Tools.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoFinal.Services.Interfaces
{
    public interface IMainService
    {
        public void Log(string messsage);

        public void log(string messsage, logType type);
        public void writeEventLog(string message, logType type);

        public void sendEmail(MailClass mail, logType type);

        public void sendEmailComplete(object sender, AsyncCompletedEventArgs e);
        public void getLogFields(logType type, ref string logWorkFile, ref string logArchivePrefix);
    }
}
