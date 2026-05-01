using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

/*
 * This class is a reflection bootstrap which walks through the assembly before any other code runs, hunts down all the
 * EventFactories in that are supposed to spawn something, and puts them in their respective dictionaries in order to
 * ensure that they can be selected by the spawner.
 */

public static class FactoryBootstrap {
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType
        .BeforeSceneLoad)] // Makes sure that this function runs before any others.
    private static void Init(){
        Debug.Log("BOOTSTRAP START");

        FactoryBootstrap.RegisterFactories( // Register all factories descended from Positive
            typeof(EventFactoryPositive<,>),
            typeof(PositiveFactoryDictionary<,>)
        );

        FactoryBootstrap.RegisterFactories( // Register all factories descended from Negative
            typeof(EventFactoryNegative<,>),
            typeof(NegativeFactoryDictionary<,>)
        );

        FactoryBootstrap.RegisterFactories( // Register all factories descended from Boss
            typeof(EventFactoryBoss<,>),
            typeof(BossFactoryDictionary<,>)
        );

        Debug.Log("BOOTSTRAP COMPLETE");
    }

    // Register the factories descended from the input.
    private static void RegisterFactories(Type baseType, Type dictionary){
        // Get a list of factories.
        IEnumerable<Type> factoryTypes = FactoryBootstrap.EnumerateFactoryDescendents(baseType);

        // Iterate through the list and register them.
        foreach (var type in factoryTypes) {
            RegisterFactory(type, baseType, dictionary);
        }
    }

    // Get a list of all concrete factory types.
    private static IEnumerable<Type> EnumerateFactoryDescendents(Type baseType){
        return AppDomain.CurrentDomain.GetAssemblies() // Gets a list of factories in the assembly.
            .SelectMany(assembly => { // Some assemblies won't reflect
                try {
                    return assembly.GetTypes();
                }
                catch {
                    return Array.Empty<Type>();
                }
            })
            .Where(type => // Determines whether the factory is a concrete descendent of baseType.
                    !type.IsAbstract // Not abstract
                    && FactoryBootstrap.FindCloseBase(type, baseType) != null // Is a descendent.
            );
    }

    // Figures out what a type's base class actually is.
    private static Type FindCloseBase(Type type, Type openGenericBase){
        while (type != null && type != typeof(object)) {
            // Makes sure that the end of the inheritance chain hasn't been reached.
            if (
                type.IsGenericType
                && type.GetGenericTypeDefinition() == openGenericBase // Success condition.
            ) return type;

            type = type.BaseType; // Walk back up the inheritance tree.
        }

        return null; // Generic base class not found.
    }

    // Calls the constructor through reflective access.
    private static object CreateFactoryInstance(Type factoryType, Type openFactoryBaseType){
        var createMethod =
            factoryType.GetMethods(BindingFlags.Public | BindingFlags.Static) // An array of methods in the class.
                .FirstOrDefault(methodInfo =>
                    methodInfo.GetParameters().Length == 0
                    && factoryType.IsAssignableFrom(methodInfo.ReturnType)
                    && methodInfo.Name.StartsWith("Create")
                );

        if (createMethod != null) {
            return createMethod.Invoke(null, null);
        }

        
        return Activator.CreateInstance(factoryType);
    }

    private static bool EnsureRegistration(Type factoryType, object createdFactory){
        PropertyInfo shouldRegisterProp = factoryType.GetProperty(
            "ShouldRegister",
            BindingFlags.Public | BindingFlags.Instance
        );

        bool shouldRegister = shouldRegisterProp != null
                              && (bool)shouldRegisterProp.GetValue(createdFactory);

        if (!shouldRegister) {
            Debug.Log($"[FactoryBootstrap] Skipped {factoryType.Name}");
            return false;
        }
        
        return true;
    }

    private static MethodInfo CallStaticFactoryMethod(Type closedDictionaryType){
        return closedDictionaryType
            .GetMethods(BindingFlags.Public | BindingFlags.Static)
            .FirstOrDefault(methodInfo =>
                methodInfo.GetParameters().Length == 0 &&
                closedDictionaryType.IsAssignableFrom(methodInfo.ReturnType) &&
                methodInfo.Name.StartsWith("Create")
            );
    }

    private static void RegisterFactory(Type factoryType, Type openFactoryBaseType, Type openDictionaryType){
        try {
            // Step 1: Find the *closed* generic base that matches the marker type.
            Type closedFactoryBase = FindCloseBase(factoryType, openFactoryBaseType);

            if (closedFactoryBase == null) {
                // If we can't find a matching generic base, this type shouldn't be here.
                Debug.LogError($"[FactoryBootstrap] {factoryType.Name} does not close {openFactoryBaseType.Name}");
            
                // Failure condition.
                return;
            }

            // Step 2: Extract the actual generic arguments (T, U).
            // These define *which dictionary instance* this factory belongs to.
            Type[] genericArgs = closedFactoryBase.GetGenericArguments();
            Type eventType = genericArgs[0]; // T
            Type enumType = genericArgs[1]; // U
            
            // Step 3: Create an instance of the factory.
            // This supports both:
            //   - Static Create() pattern
            //   - Default constructor fallback
            object createdFactory = CreateFactoryInstance(factoryType, openFactoryBaseType);
            if (createdFactory == null) { // Factory could not be created.
                return;
            }

            // Step 4: Check whether this factory wants to be registered.
            // We can't cast to EventFactory<T,U>, so we use reflection.
            if (!EnsureRegistration(factoryType, createdFactory)) return;

            // Step 5: Close the dictionary type with the same <T, U>.
            // Example:
            //   PositiveFactoryDictionary<,> -> PositiveFactoryDictionary<HealthPickupEvent, PickupType>
            Type closedDictionaryType = openDictionaryType.MakeGenericType(eventType, enumType);

            // Step 6: Find the static "Create..." method (singleton accessor).
            // This is your manual singleton pattern.
            MethodInfo createDictionaryMethod = FactoryBootstrap.CallStaticFactoryMethod(closedDictionaryType);

            if (createDictionaryMethod == null) {
                Debug.LogError($"[FactoryBootstrap] No Create method found on {closedDictionaryType.Name}");
                return;
            }

            // Step 7: Get the dictionary instance (creates it if needed).
            object dictionaryInstance = createDictionaryMethod.Invoke(null, null);

            // Step 8: Locate the instance method that registers factories.
            MethodInfo addMethod = closedDictionaryType.GetMethod(
                "AddEventFactory",
                BindingFlags.Public | BindingFlags.Instance
            );

            if (addMethod == null) {
                Debug.LogError($"[FactoryBootstrap] No AddEventFactory found on {closedDictionaryType.Name}");
                return;
            }

            // Step 9: Register the factory into the correct typed dictionary.
            // This works because the generic types now match exactly.
            addMethod.Invoke(dictionaryInstance, new[] { createdFactory });

            Debug.Log($"[FactoryBootstrap] Registered {factoryType.Name}");
        }
        catch (Exception ex) {
            // Catch everything so one bad factory doesn't kill bootstrap.
            Debug.LogError($"[FactoryBootstrap] Error with {factoryType.Name}: {ex}");
        }
    }
}