using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace HackathonApi;

public static class Utils
{
    public static E TransferData<E>(object basic)
    {
        var e = Activator.CreateInstance<E>();
        return TransferData<E>(e, basic);
    }

    public static E To<E>(this object basic)
    {
        return TransferData<E>(basic);
    }
    
    public static E TransferData<E>(E to, object from)
    {
        var baseType = from.GetType();
        var entityType = typeof(E);
        var entityProps = entityType.GetProperties();
        foreach (var property in baseType.GetProperties())
        {
            var value = property.GetValue(from);
            var curr = entityProps.FirstOrDefault(i => i.Name == property.Name);
            if (curr == null)
                continue;
            
            curr.SetValue(to, value);
        }

        return to;
    }

    static int Minimum(int a, int b, int c) => (a = a < b ? a : b) < c ? a : c;

    public static int LevenshteinDistance(string firstWord, string secondWord)
    {
        var n = firstWord.Length + 1;
        var m = secondWord.Length + 1;
        var matrixD = new int[n, m];

        const int deletionCost = 1;
        const int insertionCost = 1;

        for (var i = 0; i < n; i++)
        {
            matrixD[i, 0] = i;
        }

        for (var j = 0; j < m; j++)
        {
            matrixD[0, j] = j;
        }

        for (var i = 1; i < n; i++)
        {
            for (var j = 1; j < m; j++)
            {
                var substitutionCost = firstWord[i - 1] == secondWord[j - 1] ? 0 : 1;

                matrixD[i, j] = Minimum(matrixD[i - 1, j] + deletionCost, // удаление
                    matrixD[i, j - 1] + insertionCost, // вставка
                    matrixD[i - 1, j - 1] + substitutionCost); // замена
            }
        }

        return matrixD[n - 1, m - 1];
    }
}

public class AuthOptions
{
    public const string ISSUER = "HackathonApi"; // издатель токена
    public const string AUDIENCE = "HackathonUser"; // потребитель токена
    const string KEY = "9A807A6F-177A-4C82-8D0C-E0008E1EE910"; // ключ для шифрации

    public static SymmetricSecurityKey GetSymmetricSecurityKey() => new(Encoding.UTF8.GetBytes(KEY));
}