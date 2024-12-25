using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Net.NetworkInformation;
// See https://aka.ms/new-console-template for more information
public class Analize
{
    public
    int numText, cntErr;
    public TimeSpan span;
    public int textLength;

    public Analize() { }
    public Analize(int inumText, TimeSpan ispan) {
        numText = inumText;
        span = ispan;
    }

    public Analize(int inumText, TimeSpan ispan, string text, string textForCheck)
    {
        numText = inumText;
        span = ispan;
        textLength = text.Length;
        errors(text, textForCheck);
    }

    ~Analize() { }

    public void errors(string text, string textForCheck)
    {
        cntErr = 0;
        if (text.Length < textForCheck.Length)
                cntErr += textForCheck.Length - text.Length;
        for (int i = 0; i < Math.Min(text.Length,textForCheck.Length); i++) {
            if (text[i] != textForCheck[i])
            {
                cntErr++;
            }
        }
    }

    public double speed() {
        return (double)textLength / Convert.ToDouble(span.TotalSeconds) * 60;
    }
};
public class Statistic
{
    public int count;
    public Analize[] stat;
    public Statistic() 
    { 
       
      count = 0;
      stat = Array.Empty<Analize>();
    }
    ~Statistic() { }
    public void Add(Analize A)
    {
        count++;
        Array.Resize(ref stat, count);
        stat[count-1]=A;
    }
    public double MiddleTime(int textLength) { 
    double sum = 0;
        for (int i = 0;i < count;i++)
        {
            sum = sum + stat[i].speed();
        }
        return sum/count;
    }
    public double BestTime() {
        double bt;
        bt = stat[0].speed();
        for (int i = 1;i < count; i++)
        {
            if (stat[i].speed() > bt)
                bt = stat[i].speed();
        }
        return bt;
    }
    public double WorseTime()
    {
        double  wt;
        wt = stat[0].speed();
        for (int i = 1; i < count; i++)
        {
            
            if (stat[i].speed() < wt)
                wt = stat[i].speed();
        }
        return wt;
    }
}
enum Lang{
    Ru=1,
    En=2
}

public class Program
{
    public static void Main(string[] args)
    {

        var dict = new Dictionary<Lang, string[]>();
        string[] optionsEn = new[] { "hello word!", "the quick brown fox jumps over the lazy dog", "Satisfaction", "explode" };
        string[] optionsRu = new[] { "Привет мир!", "съешь же ещё этих мягких французских булок, да выпей чаю!", "Удовлетворение", "Взрыв" };
        string[] optionsMas;
        dict.Add(Lang.Ru, optionsRu);
        dict.Add(Lang.En, optionsEn);
        int numText;
        Analize Anlz = new Analize();
        Statistic Stat = new Statistic(); 
        Random rand = new Random();
        Console.WriteLine("Choose language: ru or en");
        string langCh;
        do
        {
lab1:        langCh = Console.ReadLine();
            if (langCh == "ru")
                optionsMas = dict[Lang.Ru];
            else if (langCh == "en")
                optionsMas = dict[Lang.En];
            else
                { Console.WriteLine("Wrong language");
                  goto lab1;//return;
                 }
        } while (langCh != "ru" && langCh != "en");
         
        Console.WriteLine("Press enter to start test for speed!\nInput 'exit' to finish test");
        while (Console.ReadLine() != "exit")
        {
            numText = rand.Next(optionsMas.Length);
            var options = optionsMas[numText];
            Console.WriteLine(options);
            DateTime startedAt = DateTime.Now;
            string text = Console.ReadLine();
            if (text == "exit") { return; }
            if (text != options)
            {
                Console.WriteLine("Wrong input");
                //return;
            }
            TimeSpan span = DateTime.Now - startedAt;

            //Anlz = new Analize(numText, span);
            //Anlz.errors(text, options);

            Anlz = new Analize(numText, span, text, options);
            Stat.Add(Anlz);

            if (Anlz.cntErr > 0)
            {
                Console.WriteLine($"You make {Anlz.cntErr} mistakes");
            }
            Console.WriteLine($"You're time is {Math.Round(Anlz.span.TotalSeconds, 2)} seconds, speed - {Math.Round(Anlz.speed(),0)} sign per minute");
            Console.WriteLine($"You're midle speed is {Math.Round(Stat.MiddleTime(text.Length),0)}, " +
                $"best - {Math.Round(Stat.BestTime(),0)}, worse - {Math.Round(Stat.WorseTime(),0)}\nTry again?");
        }
        
    }
}