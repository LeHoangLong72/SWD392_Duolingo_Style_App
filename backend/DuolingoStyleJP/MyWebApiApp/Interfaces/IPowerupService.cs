namespace MyWebApiApp.Interfaces
{
    public interface IPowerupService
    {
        Task ActivatePowerupAsync(string userId, int userItemId);
        Task<int> ApplyPowerupsAsync(string userId, int baseXP);
    }
}
