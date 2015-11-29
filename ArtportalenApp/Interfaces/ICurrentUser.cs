namespace ArtportalenApp.Interfaces
{
    public interface ICurrentUser : IUser
    {
        bool IsAutenticated { get; }
    }
}
