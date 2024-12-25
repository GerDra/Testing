// See https://aka.ms/new-console-template for more information

using System;
using System.Diagnostics;
using System.Numerics;
using System.Reflection.Emit;
using System.Reflection.Metadata;
using System.Security.Cryptography;
//using BigInteger = ulong;




class ClassPrime
{
    ulong number;
    public Stopwatch sw;
    public ClassPrime(){
        sw = new Stopwatch();
    }
    public static void LambdaMeter(string label, Action act)
    {
        var sw = new Stopwatch();
        sw.Start();
        act();
        sw.Stop();
        Console.WriteLine($"{label} : {sw.Elapsed}"); // Здесь логируем
    }
    public static bool IsPrime(ulong number)
        {
            for (ulong i = 2; i < number; i++)
            {
                if (number % i == 0)
                    return false;
            }
            return true;
        }
        
        public static bool MillerRabinTest(BigInteger n, ulong k)
        {
            // если n == 2 или n == 3 - эти числа простые, возвращаем true
            if (n == 2 || n == 3)
                return true;

            // если n < 2 или n четное - возвращаем false
            if (n < 2 || n % 2 == 0)
                return false;

            // представим n − 1 в виде (2^s)·t, где t нечётно, это можно сделать последовательным делением n - 1 на 2
            BigInteger t = n - 1;

            int s = 0;

            while (t % 2 == 0)
            {
                t /= 2;
                s += 1;
            }

            // повторить k раз
            for (ulong i = 0; i < k; i++)
            {
                // выберем случайное целое число a в отрезке [2, n − 2]
                RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();

                byte[] _a = new byte[n.ToByteArray().LongLength];

                BigInteger a;

                do
                {
                    rng.GetBytes(_a);
                    a = new BigInteger(_a);
                }
                while (a < 2 || a >= n - 2);

                // x ← a^t mod n, вычислим с помощью возведения в степень по модулю
                BigInteger x = BigInteger.ModPow(a, t, n);

                // если x == 1 или x == n − 1, то перейти на следующую итерацию цикла
                if (x == 1 || x == n - 1)
                    continue;

                // повторить s − 1 раз
                for (int r = 1; r < s; r++)
                {
                    // x ← x^2 mod n
                    x = BigInteger.ModPow(x, 2, n);

                    // если x == 1, то вернуть "составное"
                    if (x == 1)
                        return false;

                    // если x == n − 1, то перейти на следующую итерацию внешнего цикла
                    if (x == n - 1)
                        break;
                }

                if (x != n - 1)
                    return false;
            }

            // вернуть "вероятно простое"
            return true;
        }
    }
public partial class Program
    {

        // тест Миллера — Рабина на простоту числа
        // производится k раундов проверки числа n на простоту
        

        static void Main(string[] args)
        {
            ClassPrime clPrime = new ClassPrime();

            Random random = new Random();
            Console.WriteLine("Введите конечное значение диапазона 1...N и нажмите Enter");
            Console.WriteLine("N = ");
            if ((!ulong.TryParse(Console.ReadLine(), out ulong result)) || (result < 0))
                Console.WriteLine("Число должно быть положительным и целым");
            Console.WriteLine($"Простые числа из диапазона от 1 до {result}");
            int count = 0;
        TimeSpan[] timerPer = new TimeSpan[result-1];
        ulong j= 0;
        for (ulong i = 2; i <= result; i++)
            {
            clPrime.sw.Restart();
                if (ClassPrime.IsPrime(i))
                {
                    //Console.Write($"{i} ");
                    count++;

                }
                clPrime.sw.Stop();
            timerPer[j++] = clPrime.sw.Elapsed;
            }


            Console.WriteLine("");
            Console.WriteLine($"Найдено {count} простых чисел из диапазона от 1 до {result}");

            Console.WriteLine("");
            Console.WriteLine("Алгоритм Миллера");
            Console.WriteLine($"Простые числа из диапазона от 1 до {result}");
            
            count = 0;
            
            Console.WriteLine($"Найдено {result} простое число из диапазона от 1 до {result}");

        TimeSpan[] timerMil = new TimeSpan[result - 1];
        j = 0;
        for (ulong i = 2; i <= result; i++)
            {

                BigInteger bi = new BigInteger(i);
                clPrime.sw.Restart();
                
                if (ClassPrime.MillerRabinTest(bi, 1) == true)
                {
                    //Console.Write($"{i} ");
                    count++;
                }
            clPrime.sw.Stop();
            timerMil[j++] = clPrime.sw.Elapsed;
        }

            Console.WriteLine("");
            Console.WriteLine($"Найдено {count} простых чисел из диапазона от 1 до {result}");
        Console.WriteLine("");
        for (j = 0; j < result-1; j++)
        {
            Console.WriteLine($"{j+2} | {timerPer[j]} | {timerMil[j]}");
        }
        TimeSpan[] timerPer2 = new TimeSpan[34];
        TimeSpan[] timerMil2 = new TimeSpan[34];
        j = 0;
        count = 0;
        for (ulong i = 16; i <= 32; i++)
        {
            ulong x = (ulong)Math.Pow(2, i) - 1;
            clPrime.sw.Restart();
            ClassPrime.IsPrime(x);
            clPrime.sw.Stop();
            timerPer2[j] = clPrime.sw.Elapsed;
            if (ClassPrime.IsPrime(x)) {
                Console.WriteLine($"{x} Вычислялся {timerPer2[j]}");
                count++;
            }
            BigInteger bi = new BigInteger(x);
            clPrime.sw.Restart();
            ClassPrime.MillerRabinTest(bi,1);
            clPrime.sw.Stop();
            timerMil2[j++] = clPrime.sw.Elapsed;
            if (ClassPrime.MillerRabinTest(bi, 1))
            {
                Console.WriteLine($"{x} Вычислялся {timerMil2[j]}");
            }

            //x = (ulong)Math.Pow(2, i) + 1;
            x = x + 2;
            clPrime.sw.Restart();
            ClassPrime.IsPrime(x);
            clPrime.sw.Stop();
            timerPer2[j] = clPrime.sw.Elapsed;
            if (ClassPrime.IsPrime(x))
            {
                Console.WriteLine($"{x} Вычислялся {timerPer2[j]}"); count++;
            }
            bi = x;
            clPrime.sw.Restart();
            ClassPrime.MillerRabinTest(bi, 1);
            clPrime.sw.Stop();
            timerMil2[j++] = clPrime.sw.Elapsed;
            if (ClassPrime.MillerRabinTest(bi, 1))
            {
                Console.WriteLine($"{x} Вычислялся {timerMil2[j]}"); 
            }
        }
        Console.WriteLine($"\nНайдено {count} простых чисел из диапазона от 65535 до 4294967297\n");
        for (ulong i = 0; i < j; i++)
        {
            Console.WriteLine($"{i + 1} | {timerPer2[i]} | {timerMil2[i]}");
        }
        ulong randNum;
            Console.Write("Сгенерировано число {0,8:N0}", randNum=(ulong)random.Next((int)Math.Pow(2.0,32.0), (int)Math.Pow(2.0, 64.0)-1));

            Console.WriteLine("\nМетод перебора");
            
            //randNum = (ulong)Math.Pow(2.0, 61.0) - 1; // простое число Мерсена 2**61-1
            //Console.Write("Сгенерировано число {0,8:N0}", randNum);

            if (ClassPrime.IsPrime(randNum))
            {
                Console.Write($"{randNum} простое число");
            }
            else
            {
                Console.Write($"{randNum} составное число");
            }
            
            Console.WriteLine("\nТест Миллера-Рабина");
            if (ClassPrime.MillerRabinTest(randNum,1))
            {
                Console.Write($"{randNum} простое число");
            }
            else
            {
                Console.Write($"{randNum} составное число");
            }
            
            /*
            for (ulong i = result; i <= result; i++)
            {
                double t = (double)i;
                BigInteger logN = new BigInteger(t);
                if (MillerRabinTest(logN) == true)
                {
                    Console.Write($"{i} ");
                    count++;
                }
            }
            Console.WriteLine("");
            Console.WriteLine($"Найдено {count} простых чисел из диапазона от 1 до {result}");
            */
        }
    }
