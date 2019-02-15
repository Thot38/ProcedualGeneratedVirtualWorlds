using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class LSystem
{
    public string axiom = string.Empty;
    public int numberOfIterations = 1;

    public Dictionary<char, string> demRulz = new Dictionary<char, string>();

    #region Alphabet

    public const char CLOCKWISE = '-';
    public const char COUNTERCLOCKWISE = '+';
    public const char BITCHUP = '&';
    public const char BITCHDOWN = '^';
    public const char THEYSEEMEROLLINGLEFT = '\\';
    public const char THEYHATINRIGHT = '/';
    public const char TURNAROUNDANDHANDSUP = 't';
    public const char PUSH = '[';
    public const char POPDATBITCH = ']';
    public const char COLOR = 'c';

    
    #endregion


    public LSystem()
    {
        this.axiom = "ab";
        this.demRulz.Add('a', "aab");
        this.demRulz.Add('b', "baa");
        numberOfIterations = 3;
    }

    public string Generate()
    {
        var axiom = this.axiom;
        var result = new StringBuilder(axiom);
        var generationLog = new StringBuilder();

        generationLog.Append("Axion: ").Append(axiom);

        for (int i = 0; i < numberOfIterations; i++)
        {
            axiom = result.ToString();
            result = new StringBuilder();
            for (int j = 0; j < axiom.Length; j++)
            {
                var c = axiom[j];
                string rule;
                if (demRulz.TryGetValue(c, out rule))
                {
                    result.Append(rule);
                }
                else
                {
                    result.Append(c);
                }

                if (result.Length > 100000)
                {
                    Debug.LogError("Error: Yo niggah is too damn long, Bro!");
                    return "NudenJigger";
                }
                generationLog.Append("\n").Append(i + 1).Append(". Iteration: ").Append(result.ToString());
            }
        }
        Debug.Log(generationLog.ToString());
        return result.ToString();
    }

    public void AddRule(string newRule)
    {
        if (!newRule.Contains("="))
            return;

        var splitResult = newRule.Split('=');
        if (splitResult.Length == 2)
        {
            if (!demRulz.ContainsKey(splitResult[0][0]))
                demRulz.Add(splitResult[0][0], splitResult[1]);
            else Debug.LogWarning("Rule There Nigga!");
        }
        else
            Debug.LogWarning("You Nigga ain't not good at writing, Nigga!");
    }

    public void DeleteRule(char key)
    {
        demRulz.Remove(key);
    }

    public void EditRule(char key, string value)
    {
        if (demRulz.ContainsKey(key))
        {
            demRulz.Remove(key);
            demRulz.Add(key, value);
        }
    }
}
