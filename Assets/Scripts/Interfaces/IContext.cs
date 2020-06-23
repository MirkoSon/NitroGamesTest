/// <summary>
/// Defines context used with the Dependency Injection. 
/// </summary>
public interface IContext
{
    IDependencyContainer dependencyContainer { get; }

    /// Binds all the dependencies to the Container.
    void Bind();

    /// Unbinds all the dependencies from the Container.
    void UnBind();
}