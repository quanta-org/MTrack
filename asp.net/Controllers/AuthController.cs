using asp.net.Models;
using asp.net.Controllers;
using System.DirectoryServices.Protocols;
using System.DirectoryServices;
using Microsoft.AspNetCore.Mvc;

namespace asp.net.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    [HttpGet]
    public void Authenticate(String username, String password){
        String usernamewrapped = $"uid={username},dc=umich,dc=edu";

        var connection = new LdapConnection(new LdapDirectoryIdentifier("ldap.umich.edu", 389)){
            AuthType = AuthType.Basic,
            Credential = new(username, password)
        };

        //connection.SessionOptions.SecureSocketLayer = true;

        try{
            connection.Bind();
            Console.WriteLine("Good!");
        } catch (Exception ex){
            Console.WriteLine(ex);
            Console.WriteLine("Bad!");
        }



        /*
        try {
            using (var ldapConnection = new LdapConnection("ldap.umich.edu:389")){
                var networkCredential = new System.Net.NetworkCredential(username, password, "ldap.umich.edu");
                ldapConnection.SessionOptions.SecureSocketLayer = true;
                ldapConnection.AuthType = AuthType.Basic;
                ldapConnection.Bind(networkCredential);
            }

            Console.WriteLine("Success");
        } catch (LdapException ldapException) {
            Console.WriteLine(ldapException);
        }*/
    }
}