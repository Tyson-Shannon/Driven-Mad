using System;
using System.Linq;
using System.Reflection;
using UnityEngine;

public static class FactoryBootstrap
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Init()
    {
        Debug.Log("BOOTSTRAP START");

        RegisterFactories(typeof(EventFactoryPositive));
        RegisterFactories(typeof(EventFactoryNegative));
        RegisterFactories(typeof(EventFactoryBoss));

        Debug.Log("BOOTSTRAP COMPLETE");
    }

    private static void RegisterFactories(Type baseType)
    {
        var factoryTypes = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(a => {
                try { return a.GetTypes(); }
                catch { return Array.Empty<Type>(); } // avoid reflection load crashes
            })
            .Where(t =>
                !t.IsAbstract &&
                baseType.IsAssignableFrom(t)
            );

        foreach (var type in factoryTypes)
        {
            try {
                object instance = CreateFactoryInstance(type);

                if (instance == null) {
                    Debug.LogError($"[FactoryBootstrap] Failed to create instance of {type.Name}");
                    continue;
                }

                RegisterFactory(baseType, instance);

                Debug.Log($"[FactoryBootstrap] Registered {type.Name}");
            }
            catch (Exception ex) {
                Debug.LogError($"[FactoryBootstrap] Error with {type.Name}: {ex}");
            }
        }
    }

    private static object CreateFactoryInstance(Type type)
    {
        // Prefer static Create() or CreateXyzFactory()
        var createMethod = type.GetMethods(BindingFlags.Public | BindingFlags.Static)
            .FirstOrDefault(m =>
                m.GetParameters().Length == 0 &&
                type.IsAssignableFrom(m.ReturnType) &&
                m.Name.StartsWith("Create")
            );

        if (createMethod != null) {
            return createMethod.Invoke(null, null);
        }

        // Fallback to default constructor
        return Activator.CreateInstance(type);
    }

    private static void RegisterFactory(Type baseType, object instance)
    {
        if (baseType == typeof(EventFactoryPositive)) {
            PositiveFactoryDictionary.Register((EventFactoryPositive)instance);
        }
        else if (baseType == typeof(EventFactoryNegative)) {
            NegativeFactoryDictionary.Register((EventFactoryNegative)instance);
        }
        else if (baseType == typeof(EventFactoryBoss)) {
            BossFactoryDictionary.Register((EventFactoryBoss)instance);
        }
        else {
            Debug.LogError($"[FactoryBootstrap] Unknown base type {baseType.Name}");
        }
    }
}