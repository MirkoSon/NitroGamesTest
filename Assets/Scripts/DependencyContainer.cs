using System;
using System.Collections.Generic;

/// <summary>
/// This class is fundamental for the Dependency Injection of the entire game.
/// It stores all of the dependencies in a generic dictionary from where any of them can be resolved quickly and inexpensively.
/// </summary>
public class DependencyContainer : IDependencyContainer
{
    protected Dictionary<Type, object> _dependencies = new Dictionary<Type, object>();

    public  void Bind<T>(object implementation)
    {
        Type key = typeof(T);
        if (_dependencies.ContainsKey(key))
        {
            _dependencies[key] = implementation;
        }
        else
        {
            _dependencies.Add(key, implementation);
        }
    }

    public  T Resolve<T>()
    {
        return (T)_dependencies[typeof(T)];
    }

    public  void UnBind<T>()
    {
        _dependencies.Remove(typeof(T));
    }
}
