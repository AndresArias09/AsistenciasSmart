using Radzen.Blazor;

namespace RegistroAsistenciasSMART.Web.Utilidades
{
    public class ProgramadorEventosView<TItem> : RadzenScheduler<TItem>
    {
        public ProgramadorEventosView() : base()
        {
            TodayText = "Hoy";
        }
    }
}
