using Radzen.Blazor;

namespace FirmaDigitalSMART.Web.Utilidades
{
    public class ProgramadorEventosView<TItem> : RadzenScheduler<TItem>
    {
        public ProgramadorEventosView() : base()
        {
            TodayText = "Hoy";
        }
    }
}
