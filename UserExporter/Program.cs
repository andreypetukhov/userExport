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

        }

        private static GSArray GetUsers(string apiKey, string secretKey,int quantity, string cursor = "")
        {
            string openCursor = string.Empty;
            int limit = 100;
            GSObject obj = new GSObject();

            obj.Put("query", "SELECT UID,identities.address,identities.provider,identities.mappedProviderUIDs,identities.isLoginIdentity,identities.allowsLogin,identities.isExpiredSession,identities.lastLoginTime,identities.photoURL,identities.thumbnailURL,identities.gender,identities.age,identities.birthDay,identities.birthMonth,identities.birthYear,identities.country,identities.state,identities.city,identities.zip,identities.profileURL,identities.proxiedEmail,identities.languages,identities.education,identities.honors,identities.publications,identities.patents,identities.certifications,identities.professionalHeadline,identities.bio,identities.industry,identities.specialties,identities.work,identities.skills,identities.religion,identities.politicalView,identities.interestedIn,identities.relationshipStatus,identities.hometown,identities.favorites,identities.likes,identities.followersCount,identities.followingCount,identities.username,identities.locale,identities.verified,identities.timezone,identities.missingPermissions,identities.samlData FROM accounts  limit " + limit.ToString());
            obj.Put("openCursor", "true");
            GSResponse res = new GSRequest(apiKey: apiKey, secretKey: secretKey, apiMethod: "accounts.search", clientParams: obj, useHTTPS: true).Send();
            GSArray array = res.GetArray("results", new GSArray());
            WriteToFile(array,"");
            openCursor = res.GetString("nextCursorId", String.Empty);
            GetUsers(apiKey, secretKey, quantity - limit, openCursor);
            return new GSArray();
        }

        private static void WriteToFile(GSArray array, string fileName)
        { 
            
        }
    }
}
