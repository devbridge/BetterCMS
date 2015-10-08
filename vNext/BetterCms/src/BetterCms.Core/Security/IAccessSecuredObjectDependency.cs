namespace BetterCms.Core.Security
{    
    public interface IAccessSecuredObjectDependency
    {
        IAccessSecuredObject SecuredObject { get; }
    }
}
