using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public static class GetInterfaceExtensions
{
	public static T GetInterface<T>(this GameObject inObj) where T : class
	{
		return inObj.GetComponents<Component>().OfType<T>().FirstOrDefault();
	}
	
	public static IEnumerable<T> GetInterfaces<T>(this GameObject inObj) where T : class
	{
		return inObj.GetComponents<Component>().OfType<T>();
	}
}
