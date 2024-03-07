using System.Net;
using System.Net.Sockets;
using System.Text;

var parsedArguments = ParseArgs(args);

ThreadPool.SetMaxThreads(parsedArguments.MaxThreads, parsedArguments.MaxThreads);

using var listener = new TcpListener(
    IPAddress.Parse("0.0.0.0"),
    parsedArguments.Port);

listener.Start();
Console.WriteLine("Server started on port: " + parsedArguments.Port);

while (true)
{
    using var client = listener.AcceptTcpClient();
    using var stream = client.GetStream();
    try
    {
        Continue(Consume(stream));
    }
    catch (Exception)
    {
        const string text = "too large file";
        var bytes = Encoding.UTF8.GetBytes(text);
        stream.Write(bytes);
    }
}

Arguments ParseArgs(string[] strings)
{
    try
    {
        var i = Array.IndexOf(strings, "--port");
        var port = i != -1 ? int.Parse(strings[i + 1]) : 8080;

        i = Array.IndexOf(strings, "--max-threads");
        var maxThreads = i != -1 ? int.Parse(strings[i + 1]) : int.MaxValue;

        i = Array.IndexOf(strings, "--size");
        var fileMaxSize = i != -1 ? int.Parse(strings[i + 1]) : int.MaxValue;

        i = Array.IndexOf(strings, "--path");
        var filePathSave = i != -1 ? (strings[i + 1]) : "./";

        return new Arguments(
            port,
            maxThreads,
            fileMaxSize,
            filePathSave);
    }
    catch (Exception e)
    {
        Console.WriteLine(e);
        Environment.Exit(-1);
        throw;
    }
}

void Continue(Task _)
{
}

async Task Consume(NetworkStream stream)
{
    var extension = GetExtension(stream);
    if (Directory.Exists(parsedArguments.FilePathSave) is false)
        Directory.CreateDirectory(parsedArguments.FilePathSave);

    var filePath = $"{parsedArguments.FilePathSave}{DateTime.Now:O}.{extension}";
    await using var fileStream = File.Create(filePath);

    var counter = 0;
    var buffer = new byte[512];
    var size = stream.Read(buffer);
    while (size > 0)
    {
        counter += size;
        if (counter > parsedArguments.FileMaxSize)
            throw new Exception("Too large file");

        fileStream.Write(buffer.Where(b => b != 0).ToArray());
        size = stream.Read(buffer);
    }
}

string GetExtension(NetworkStream networkStream)
{
    var ex = "";
    var @byte = networkStream.ReadByte();
    do
    {
        var s = Encoding.UTF8.GetString([(byte)@byte]);
        if (s.Equals("."))
            break;
        ex += s;
        @byte = networkStream.ReadByte();
    } while (@byte > 0);

    return ex;
}

internal record Arguments(
    int Port,
    int MaxThreads,
    int FileMaxSize,
    string FilePathSave);
