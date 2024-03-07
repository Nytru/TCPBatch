using System.Net;
using System.Net.Sockets;
using System.Text;

var request = ParsePath(args);
var file = await File.ReadAllBytesAsync(request.FilePath);
var fileExtension = Path.GetExtension(request.FilePath).TrimStart('.').Append('.').ToArray();
var ex = Encoding.UTF8.GetBytes(fileExtension);

using var tcpClient = new TcpClient();
await tcpClient.ConnectAsync(request.EndPoint);
var stream = tcpClient.GetStream();

var content = ex.Concat(file).ToArray();
await stream.WriteAsync(content);

return;

PathRequest ParsePath(IReadOnlyCollection<string> strings)
{
    if (strings.Count != 2)
    {
        throw new Exception("There must exactly 2 parameter");
    }

    var path = strings.First();
    if (File.Exists(path) is false)
    {
        throw new FileNotFoundException($"No file with path: {path}");
    }

    var endPoint = IPEndPoint.Parse(strings.ElementAt(1));

    return new PathRequest(path, endPoint!);
}

internal record PathRequest(string FilePath, IPEndPoint EndPoint);