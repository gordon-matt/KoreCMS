namespace Kore.Web.Mvc.RoboUI
{
    public class RoboUIContentResult : RoboUIResult
    {
        private readonly string content;

        public RoboUIContentResult(string content)
        {
            this.content = content;
        }

        #region Overrides of BaseRoboFormResult

        public override string GenerateView()
        {
            return content;
        }

        #endregion Overrides of BaseRoboFormResult
    }
}