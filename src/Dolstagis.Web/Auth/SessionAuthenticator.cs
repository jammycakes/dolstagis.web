namespace Dolstagis.Web.Auth
{
    public class SessionAuthenticator : IAuthenticator
    {
        public string SessionKey { get; set; }

        public SessionAuthenticator()
        {
            this.SessionKey = "{C3F5F70A-8703-43FA-8947-5E519D7791D6}";
        }

        public IUser GetUser(RequestContext context)
        {
            if (context.Session == null) return null;
            object result;
            return context.Session.Items.TryGetValue(SessionKey, out result) ? result as IUser : null;
        }

        public void SetUser(RequestContext context, IUser user)
        {
            if (user != null)
            {
                context.Session.Items[SessionKey] = user;
            }
            else
            {
                context.Session.Items.Remove(SessionKey);
            }
        }
    }
}
