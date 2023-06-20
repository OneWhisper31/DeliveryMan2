using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PriorityQueueSimplify<T>
{
	Dictionary<T, float> _allElems = new Dictionary<T, float>();
	public int Count { get { return _allElems.Count; } }

	public void Enqueue(T elem, float cost)
	{
		if (!_allElems.ContainsKey(elem)) _allElems.Add(elem, cost);
		else _allElems[elem] = cost;
	}

	//Que es lo que hace .Dequeue?
	public T Dequeue()
	{
		T elem = default;
		float currentValue = Mathf.Infinity;
		//Consigo el primer elemento (el que tiene mas prioridad)
		foreach (var item in _allElems)
		{
			if (currentValue > item.Value)
			{
				elem = item.Key;
				currentValue = item.Value;
			}
		}
		//Lo quito del diccionario
		_allElems.Remove(elem);
		//Lo devuelvo
		return elem;
	}
}
