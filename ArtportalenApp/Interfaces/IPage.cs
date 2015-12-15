using System.Diagnostics.Contracts;

namespace ArtportalenApp.Interfaces
{
    public interface IPage<TViewModel> where TViewModel : IViewModel
    {
        TViewModel ViewModel { get; set; }
    }
}
