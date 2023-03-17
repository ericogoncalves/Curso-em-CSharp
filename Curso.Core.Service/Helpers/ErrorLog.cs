using Curso.Core.Service.Helpers;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Linq;
using System.Text;

namespace System
{
    public static class ErrorLog
    {
        public static void Log(string texto)
        {
            try
            {
                string nomeLog = "log_" + DateTime.Now.ToString("yyyy_MM_dd") + ".txt";
                string nomePasta = Path.Combine(AppContext.BaseDirectory, "Logs");
                string fullPath = Path.Combine(nomePasta, nomeLog);

                using (StreamWriter writer = new StreamWriter(fullPath, true))
                {
                    writer.WriteLine("[" + DateTime.Now.ToString() + "] "! + texto);
                }
            }
            catch (Exception)
            {
            }
        }

        public static string[] ReadFile(string fileName, string folder = "")
        {
            StringBuilder str = new StringBuilder();
            try
            {
                string nomeLog = "log_" + DateTime.Now.ToString("yyyy_MM_dd") + ".txt";
                string nomePasta = Path.Combine(AppContext.BaseDirectory, "Logs");

                if (!string.IsNullOrEmpty(fileName))
                    nomeLog = fileName;

                //if (!string.IsNullOrEmpty(folder))
                //    fullPath = folder;

                string fullPath = Path.Combine(nomePasta, nomeLog);

                var lines = File.ReadAllLines(fullPath);
                foreach (var line in lines)
                    str.AppendLine(line);

                return lines;
            }
            catch (Exception)
            {
            }

            return null;
        }

        public static void LogCSV(string texto, string nomeArquivo = "")
        {
            try
            {
                string nomeLog = "log_" + DateTime.Now.ToString("yyyy_MM_dd") + ".csv";

                if (!string.IsNullOrEmpty(nomeArquivo))
                    nomeLog = nomeArquivo + DateTime.Now.ToString("yyyy_MM_dd") + ".csv";

                string nomePasta = Path.Combine(AppContext.BaseDirectory, "Logs");
                string fullPath = Path.Combine(nomePasta, nomeLog);

                using (StreamWriter writer = new StreamWriter(fullPath, true))
                {
                    writer.WriteLine("[" + DateTime.Now.ToString() + "] "! + texto);
                }
            }
            catch (Exception)
            {
            }
        }

        public static void GerarLogEvidencia(IHttpContextAccessor _httpContextAccessor, string mensagem)
        {
            Guid userChangeId = Guid.Empty;
            try
            {
                if (!string.IsNullOrEmpty(_httpContextAccessor.HttpContext.Request.Headers["Authorization"]))
                {
                    var jwtToken = JwtTokenHelper.DecodeToken(_httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString().Remove(0, 7));
                    userChangeId = new Guid(jwtToken.Claims.FirstOrDefault(x => x.Type == "jti").Value);
                    Log(mensagem + " " + "o usuário que realizou essa ação foi " + userChangeId);
                }
            }
            catch (Exception)
            {
                Log(mensagem + " " + "o usuário que realizou essa ação foi " + userChangeId);
            }
        }
    }
}