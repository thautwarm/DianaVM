using System;

public static class Tester {

    public enum Enum {

        E1, E2, E3, E4, E5, E6
    }
    public static Action<object>[] fs1;
    public static Action<object>[] fs2;

    public static Action<object>[] fs3;

    public static Action<object>[] fs4;

    public static Action<object>[] fs5;

    public static Action<object>[] fs6;
    public static void f(Enum i, object o, int j){
        switch(i){
            case Enum.E1: 
                fs1[j](o);
                return;
            case Enum.E2: 
                fs2[j](o);
                return;
            case Enum.E3: 
                fs3[j](o);
                return;
            case Enum.E4: 
                fs4[j](o);
                return;
            case Enum.E5: 
                fs5[j](o);
                return;
            case Enum.E6: 
                fs6[j](o);
                return;
            default:
                throw new NotImplementedException();
                
                    
        }
    }






}