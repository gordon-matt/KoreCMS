namespace Kore.Web.Mvc.RoboUI.Expressions
{
    public class ExpressionBuilderOptions
    {
        public ExpressionBuilderOptions()
        {
            LiftMemberAccessToNull = true;
        }

        /// <summary>
        ///     Gets or sets a value indicating whether member access expression used
        ///     by this builder should be lifted to null. The default value is true;
        /// </summary>
        /// <value>
        ///     <c>true</c> if member access should be lifted to null; otherwise, <c>false</c>.
        /// </value>
        public bool LiftMemberAccessToNull { get; set; }

        public void CopyFrom(ExpressionBuilderOptions other)
        {
            LiftMemberAccessToNull = other.LiftMemberAccessToNull;
        }
    }
}