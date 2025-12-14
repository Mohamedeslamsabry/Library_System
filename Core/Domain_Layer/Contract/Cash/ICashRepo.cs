namespace Domain_Layer.Contract.Cash
{
    public interface ICashRepo
    {
        #region Get
        Task<string?> GetCashedDataAsync(string Key);
        #endregion

        #region Set
        Task SetDataAsync(string Key, string CashValue, TimeSpan TimeToLive);

        #endregion
    }
}
