namespace ArtportalenApp.Interfaces
{
    public interface IViewModelAware<TViewModel> where TViewModel : IViewModel
    {
        TViewModel ViewModel { get; set; }
    }
}
