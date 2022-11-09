namespace Domedo.API.Database.Seeds
{
    public interface ISeeder
    {
        void Handle(IServiceScope scope);

    }
}
