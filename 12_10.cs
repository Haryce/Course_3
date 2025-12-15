using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class Program {
    struct Stu { public string n; public int g; }
    
    static long f(int n) { return n < 2 ? 1 : n * f(n-1); }
    static void s(ref int a, ref int b) { int t = a; a = b; b = t; }
    static int m(int[] a) { int x = a[0]; for(int i=1; i<10; i++) if(a[i]>x) x=a[i]; return x; }
    static int fi(int[] a, int s, int v) { for(int i=0; i<s; i++) if(a[i]==v) return i; return -1; }  
    static void Main() {
        int x, n, a=0, b=0;
        
        //1
        Console.Write("x="); x=int.Parse(Console.ReadLine()); Console.WriteLine(x%2==0?"Чётное":"Нечётное");
        
        //2
        Console.Write("n="); n=int.Parse(Console.ReadLine()); for(int i=1; i<=n; i++) a+=i; Console.WriteLine("sum="+a);
        
        //3
        int[] arr = new int[10]; for(int i=0; i<10; i++) { Console.Write($"a{i}="); arr[i]=int.Parse(Console.ReadLine()); } Console.WriteLine("max="+m(arr));
        
        //4
        Console.Write("n="); n=int.Parse(Console.ReadLine()); Console.WriteLine("f("+n+")="+f(n));
        
        //5
        Console.Write("a="); a=int.Parse(Console.ReadLine()); Console.Write("b="); b=int.Parse(Console.ReadLine()); s(ref a, ref b); Console.WriteLine("a="+a+" b="+b);
        
        //6
        Console.Write("a="); a=int.Parse(Console.ReadLine()); Console.Write("b="); b=int.Parse(Console.ReadLine()); Console.Write("op="); string op=Console.ReadLine();
        switch(op) { case"+": Console.WriteLine(a+b); break; case"-": Console.WriteLine(a-b); break; case"*": Console.WriteLine(a*b); break; case"/": Console.WriteLine(b!=0?a/b:0); break; }
        
        //7
        Stu st = new Stu(); Console.Write("name="); st.n=Console.ReadLine(); Console.Write("grade="); st.g=int.Parse(Console.ReadLine()); Console.WriteLine(st.n+" "+st.g);
        
        //8
        class R { int w, h; public R(int w, int h) { this.w=w; this.h=h; } public int area() { return w*h; } public int peri() { return 2*(w+h); } }
        R r = new R(5, 3); Console.WriteLine("area="+r.area()+" peri="+r.peri());
        
        //9
        int[] ar = new int[5]; for(int i=0; i<5; i++) { Console.Write($"a{i}="); ar[i]=int.Parse(Console.ReadLine()); }
        Console.Write("rev:"); for(int i=4; i>=0; i--) Console.Write(ar[i]+" ");
        Console.WriteLine();
        
        //10
        Console.Write("val="); x=int.Parse(Console.ReadLine()); int idx=fi(ar, 5, x); Console.WriteLine("idx="+(idx<0?"-1":idx));
        
        Console.ReadKey();
    }
}
