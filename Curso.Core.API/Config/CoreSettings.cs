using DevSnap.CommonLibrary.Settings;

namespace Curso.Core.Api
{
    public class CoreSettings : ServiceSetting
    {
        public readonly string JwtKey = "191da538-cb42-46e3-99df-16387ff958ce";
        public FileDataSettings FileDataSettings { get; set; }
    }
}
