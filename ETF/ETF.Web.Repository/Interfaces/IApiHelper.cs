namespace ETF.Web.Repository.Interfaces
{
    public enum HttpMethod
    {
        Get,
        Post,
        Put,
        Delete
    }

    public interface IApiHelper
    {
        T GetResponse<T>(object[] args = null, object content = null);
    }
}
