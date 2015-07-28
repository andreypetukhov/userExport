using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gigya.Socialize.SDK;


namespace UserExporter
{
    class Program
    {
        static void Main(string[] args)
        {
            string apiKey = string.Empty, secretKey = string.Empty, fileName = string.Empty, identities = string.Empty;
            int quantity = 0;
            Arguments CommandLine = new Arguments(args);

            if (CommandLine["a"] != null || CommandLine["apikey"] != null )
            {
                if (CommandLine["a"] != null)
                    apiKey = CommandLine["a"];
                else
                    apiKey = CommandLine["apikey"];
            }

            if (CommandLine["s"] != null || CommandLine["secretkey"] != null)
            {
                if (CommandLine["s"] != null)
                    secretKey = CommandLine["s"];
                else
                    secretKey = CommandLine["secretkey"];
            }

            if (CommandLine["q"] != null || CommandLine["quantity"] != null)
            {
                if (CommandLine["q"] != null)
                    quantity = Int32.Parse(CommandLine["q"]);
                else
                    quantity = Int32.Parse(CommandLine["quantity"]);
            }

            if (CommandLine["f"] != null || CommandLine["filename"] != null)
            {
                if (CommandLine["f"] != null)
                    fileName = CommandLine["f"];
                else
                    fileName = CommandLine["filename"];
            }

            identities = GetUsers(apiKey, secretKey, quantity);
            WriteToFile(identities, fileName);
            Console.WriteLine("export finished");
        }

        private static string GetUsers(string apiKey, string secretKey,int quantity, string cursor = "")
        {
            if (quantity < 1)
                return string.Empty;
            string openCursor = string.Empty;
            int limit = 100, counter = quantity;
             
            if (!(cursor.Equals("")))
            {
                openCursor = cursor;
            }
            GSObject obj = new GSObject();
            string result = string.Empty;
            if (cursor.Equals(""))
            {
                obj.Put("query", "SELECT UID,identities.address,identities.provider,identities.mappedProviderUIDs,identities.isLoginIdentity,identities.allowsLogin,identities.isExpiredSession,identities.lastLoginTime,identities.photoURL,identities.thumbnailURL,identities.gender,identities.age,identities.birthDay,identities.birthMonth,identities.birthYear,identities.country,identities.state,identities.city,identities.zip,identities.profileURL,identities.proxiedEmail,identities.languages,identities.education,identities.honors,identities.publications,identities.patents,identities.certifications,identities.professionalHeadline,identities.bio,identities.industry,identities.specialties,identities.work,identities.skills,identities.religion,identities.politicalView,identities.interestedIn,identities.relationshipStatus,identities.hometown,identities.favorites,identities.likes,identities.followersCount,identities.followingCount,identities.username,identities.locale,identities.verified,identities.timezone,identities.missingPermissions,identities.samlData FROM accounts  limit " + limit.ToString());
                obj.Put("openCursor", "true");     
            }
           
            obj.Put("cursorId", openCursor);

            GSResponse res = new GSRequest(apiKey: apiKey, secretKey: secretKey, apiMethod: "accounts.search", clientParams: obj, useHTTPS: true).Send();
            GSArray array = res.GetArray("results", new GSArray());
            
            foreach (GSObject component in array)
            {
                component.ToJsonString();
                result += component;
                result += "\n";
                counter--;
                if (counter == 0)
                    break;
            }
            
            openCursor = res.GetString("nextCursorId", String.Empty);
            if (quantity > limit)
                return result + GetUsers(apiKey, secretKey, quantity - array.Length, openCursor);
            else return result;
            
        }

        private static void WriteToFile(string array, string fileName)
        {
            System.IO.File.WriteAllText(fileName, array);

        }
    }
}
