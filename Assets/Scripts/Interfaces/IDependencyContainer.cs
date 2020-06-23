public interface IDependencyContainer
{
    /// <summary>
    /// Binds a contract Type to an instance
    /// </summary>
    /// <typeparam name="T">The Type to bind</typeparam>
    /// <param name="implementation">The implementation of the Type to bind</param>
    void Bind<T>(object implementation);

    /// <summary>
    /// Returns the implementation of the given contract Type
    /// </summary>
    /// <typeparam name="T">The Type that is binded to the implementation</typeparam>
    /// <returns>The implementation of the given type</returns>
    T Resolve<T>();

    /// <summary>
    /// Unbind the contract Type to its existing instance
    /// </summary>
    /// <typeparam name="T">The Type to unbind</typeparam>
    void UnBind<T>();
}