namespace Kore.Web.Mvc.RoboUI.Expressions
{
    public class PropertyToken : IMemberAccessToken
    {
        private readonly string propertyName;

        public PropertyToken(string propertyName)
        {
            this.propertyName = propertyName;
        }

        public string PropertyName
        {
            get { return propertyName; }
        }
    }
}