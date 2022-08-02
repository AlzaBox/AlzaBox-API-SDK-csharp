using CommandLine;

namespace AlzaBox.API.CliExample;

public class Options
{
    [Option('u', "username", Required = true)]
    public string Username { get; set; }

    [Option('p', "password", Required = true)]
    public string Password { get; set; }
        
    [Option('i', "clientid", Required = true)]
    public string ClientId { get; set; }
        
    [Option('s', "clientsecret", Required = true)]
    public string ClientSecret { get; set; }
}