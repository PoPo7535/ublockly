using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;

public class Manager : MonoBehaviour
{
    private static Manager instance;
    public static Manager Instnace
    {
        get
        {
            if (null == instance)
            {
                instance = FindObjectOfType<Manager>() ?? new GameObject().AddComponent<Manager>();;
                instance.Init();
            }
            return instance;
        }
    }

    private Dictionary<string, Test> dictionary;
    public Test test;
    public void Add(string objName)
    {
        if ("Null" == objName)
            return;
        
        var o = Instantiate(test, new Vector3(-10, 5, -5), Quaternion.identity);
        o.name = objName;
        dictionary.Add(objName, o);
    }
    public void Rename(string oldKey, string newKey)
    {
        if ("Null" == oldKey)
            return;
        
        var old = dictionary[oldKey];
        dictionary.Remove(oldKey);
        
        old.name = newKey;
        dictionary[newKey] = old;
    }
    
    public void Delete(string objName)
    {
        if ("Null" == objName)
            return;
        
        if (dictionary.ContainsKey(objName))
        {
            Destroy(dictionary[objName].gameObject);
            dictionary.Remove(objName);
        }
    }
    
    private void Init()
    {
        dictionary = new Dictionary<string, Test>();
    }
    
    public Test GetObject(string objName)
    {
        dictionary.TryGetValue(objName, out var obj);
        return obj;
    }

    public void Stop()
    {
        foreach (var obj in dictionary.Values)
        {
            obj.Stop();
        }
    }

    private void OnTransformParentChanged()
    {
        throw new NotImplementedException();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            Debug.Log(transform.hasChanged);
            transform.hasChanged = false;
        }

    }
}
