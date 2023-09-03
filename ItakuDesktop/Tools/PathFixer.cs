using System.IO;

public static class PathFixer
{
    public static string startPath;

    static PathFixer()
    {
        startPath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().CodeBase).Replace("file:\\", "");
    }

    public static string FixPath(this string str)
    {
        FixStartPath();
        str = str.Replace("\\", "/");
        if (string.IsNullOrWhiteSpace(startPath))
        {
            return str;
        }
        else
        {
            if (str.Contains(":/"))
            {
                return str.Replace("{ApplicationDir}", startPath);
            }
            else
            {
                string rps = str.Replace("{ApplicationDir}", startPath);
                return rps.Contains(":/") ? rps : Path.Combine(startPath, rps);
            }
        }
    }

    public static string FixStartPath()
    {
        if (startPath.EndsWith("\\") || startPath.EndsWith("/"))
        {
            startPath = startPath.TrimEnd('\\', '/');
        }
        return startPath;
    }
}