namespace ArtportalenApp.Interfaces
{
    public interface IUser
    {
        string Id { get; }
        string Username { get; }
        string Email { get; }
        string Fullname { get; }
    }
}