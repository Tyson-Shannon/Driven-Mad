using System;
using System.Linq;
using System.Reflection;
using UnityEngine;

public static class FactoryBootstrap
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Init() {
        Debug.Log("BOOTSTRAP START");

        RegisterFactories(
            typeof(EventFactoryPositive),
            PositiveFactoryDictionary.CreatePositiveFactoryDictionary()
        );

        RegisterFactories(
            typeof(EventFactoryNegative),
            NegativeFactoryDictionary.CreateNegativeFactoryDictionary()
        );

        RegisterFactories(
            typeof(EventFactoryBoss),
            BossFactoryDictionary.CreateBossFactoryDictionary()
        );

        Debug.Log("BOOTSTRAP COMPLETE");
    }

    private static void RegisterFactories(Type baseType, FactoryDictionary dictionary) {
        var factoryTypes = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(a => {
                try { return a.GetTypes(); }
                catch { return Array.Empty<Type>(); }
            })
            .Where(t =>
                !t.IsAbstract &&
                baseType.IsAssignableFrom(t) &&
                t != baseType
            );

        foreach (var type in factoryTypes) {
            try {
                object created = CreateFactoryInstance(type);

                if (created is not EventFactory factory) {
                    Debug.LogError($"[FactoryBootstrap] {type.Name} is not an EventFactory");
                    continue;
                }

                if (!factory.ShouldRegister) {
                    Debug.Log($"[FactoryBootstrap] Skipped {type.Name}");
                    continue;
                }

                dictionary.AddEventFactory(factory);
                Debug.Log($"[FactoryBootstrap] Registered {type.Name}");
            }
            catch (Exception ex) {
                Debug.LogError($"[FactoryBootstrap] Error with {type.Name}: {ex}");
            }
        }
    }

    private static object CreateFactoryInstance(Type type) {
        var createMethod = type.GetMethods(BindingFlags.Public | BindingFlags.Static)
            .FirstOrDefault(m =>
                m.GetParameters().Length == 0 &&
                type.IsAssignableFrom(m.ReturnType) &&
                m.Name.StartsWith("Create")
            );

        if (createMethod != null) {
            return createMethod.Invoke(null, null);
        }

        return Activator.CreateInstance(type);
    }
}