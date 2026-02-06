namespace Service_Abstraction.Interfaces
{
    public interface ICashService
    {
        #region Get
        Task<string?> GetAsync(string key);
        #endregion

        #region Set
        Task SetAsync(string key, object value, TimeSpan TimeToLive);

        #endregion  
    }
}
