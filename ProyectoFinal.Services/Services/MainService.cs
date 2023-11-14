using Microsoft.Extensions.Configuration;
using ProyectoFinal.Services.Interfaces;
using ProyectoFinal.Tools.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoFinal.Services.Services
{

    public enum logType
    {
        General = 0,
    }

    public class MainService : IMainService
    {
        private IConfiguration _config;
        public string logpath = string.Empty;
        public string logdirpath = string.Empty;
        public string logdatetimes = string.Empty;
        public string emailFrom = string.Empty;
        public string emailServer = string.Empty;

        public MainService(IConfiguration config)
        {
            try
            {
                _config = config;
                logpath = _config.GetSection("PARAMETROS:logpath").Value;
                logdirpath = _config.GetSection("PARAMETROS:logdirpath").Value;
                logdatetimes = _config.GetSection("PARAMETROS:logdatetimes").Value;
                emailFrom = _config.GetSection("PARAMETROS:emailFrom").Value;
                emailServer = _config.GetSection("PARAMETROS:emailServer").Value;
            }
            catch (Exception ex)
            {
                writeEventLog("Excepción al establecer directorios de logs: " + ex.Message, logType.General);
            }
        }


        public void Log(string messsage)
        {
            try
            {
                if (!Directory.Exists(logdirpath))
                {
                    Directory.CreateDirectory(logdirpath);
                }

                if (File.Exists(logpath))
                {
                    FileInfo fl = new FileInfo(logpath);
                    if (fl.Length >= 10485760)
                    {
                        string tmpfilename = fl.DirectoryName + @"\bitacora" + DateTime.Now.ToString("ddMMyyyyHHmmss") + ".log";
                        fl.MoveTo(tmpfilename);
                    }
                }

                FileStream fileStre = new FileStream(logpath, FileMode.Append, FileAccess.Write, FileShare.ReadWrite);
                TextWriter logFile = new StreamWriter(fileStre);
                logFile.WriteLine(DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + "|" + messsage);
                logFile.Close();
            }
            catch (Exception ex)
            {
                writeEventLog("Excepción al registrar en el log: " + ex.Message, logType.General);
            }
        }

        public void log(string messsage, logType type)
        {
            try
            {
                string logWorkFile = logpath;
                string logArchivePrefix = type.ToString();
                switch (type)
                {
                    case logType.General:
                        logWorkFile = logpath;
                        break;
                }
                if (!Directory.Exists(logdirpath))
                {
                    Directory.CreateDirectory(logdirpath);
                }

                if (File.Exists(logWorkFile))
                {
                    FileInfo fl = new FileInfo(logWorkFile);
                    if (fl.Length >= 10485760)
                    {
                        string tmpfilename = fl.DirectoryName + @"\" + logArchivePrefix + "bitacora" + DateTime.Now.ToString("ddMMyyyyHHmmss") + ".log";
                        fl.MoveTo(tmpfilename);
                    }
                }

                FileStream fileStre = new FileStream(logWorkFile, FileMode.Append, FileAccess.Write, FileShare.ReadWrite);
                TextWriter logFile = new StreamWriter(fileStre);
                logFile.WriteLine(DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + "|" + messsage);
                logFile.Close();
            }
            catch (Exception ex)
            {
                writeEventLog("Excepción al registrar en el log: " + ex.Message, type);
            }
        }

        public void writeEventLog(string message, logType type)
        {
            try
            {
                string cs = $"srvEstadia{type.ToString()}";
                if (!EventLog.SourceExists(cs))
                {
                    EventLog.CreateEventSource(cs, "Application");
                }
                EventLog.WriteEntry(cs, message);
            }
            catch (Exception ex)
            {
                writeEventLog("Excepción al escribir en el log: " + ex.Message, logType.General);
            }
        }



        public void sendEmail(MailClass mail, logType type)
        {
            try
            {
                MailMessage email = new MailMessage();
                email.From = new MailAddress(emailFrom);
                foreach (string toItem in mail.to)
                {
                    email.To.Add(new MailAddress(toItem));
                }

                if (mail.cc != null && mail.cc.Count() >= 1)
                    foreach (string ccemail in mail.cc)
                    {
                        if (!ccemail.Equals(""))
                            email.CC.Add(ccemail);
                    }

                if (mail.Files != null && mail.Files.Count >= 1)
                    foreach (var file in mail.Files)
                    {
                        email.Attachments.Add(new Attachment(new MemoryStream(file.File), file.FileName));
                    }

                email.Subject = mail.subject;
                string title = "💬  Notificación ProyectoFinal";
                string bodyText = mail.body;
                string link = "https://pruebas.com/";
                string linkText = "EstadiasAsipona";
                string noRespond = "Por favor, NO responda a este mensaje, es un envío automático.";
                string copyRight = "2023, pruebas todos los derechos reservados.";
                email.Body = MailClass.MailStringHtml(title, bodyText, link, linkText, noRespond, copyRight);
                email.IsBodyHtml = true;
                SmtpClient server = new SmtpClient(emailServer);
                server.SendCompleted += new SendCompletedEventHandler(sendEmailComplete);
                server.SendAsync(email, null);
            }
            catch (Exception ex)
            {
                writeEventLog("Excepcion enviando correo electronico: " + ex.Message, type);
            }
        }

        public void sendEmailComplete(object sender, AsyncCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                writeEventLog("Error enviando correo electronico: " + e.Error.Message, logType.General);
            }
        }


        public void getLogFields(logType type, ref string logWorkFile, ref string logArchivePrefix)
        {
            try
            {
                logWorkFile = logpath;
                logArchivePrefix = type.ToString();
                switch (type)
                {
                    case logType.General:
                        logWorkFile = logpath;
                        break;
                }
            }
            catch (Exception ex)
            {
                writeEventLog("Excepcion en getLogFields: " + ex.Message, logType.General);
            }
        }

    }
}