using System;

public class A
{
    public string S { get; private set; }

    public A(string s)
    {
        S = s;
    }

    ~A()
    {
        Console.WriteLine("~A of '{0}'", S);
    }
}