using System.Collections.Generic;
using System.Linq;
using Kore.Collections;
using Kore.Web.Mvc.Resources;

namespace Kore.Web.Mvc.RoboUI
{
    public class RoboUICombinedResult : RoboUIResult
    {
        private readonly ICollection<RoboUIResult> results;

        public RoboUICombinedResult()
        {
            results = new List<RoboUIResult>();
        }

        public RoboUICombinedResult(params RoboUIResult[] results)
        {
            this.results = new List<RoboUIResult>(results);
        }

        public void Add(RoboUIResult result)
        {
            results.Add(result);
        }

        public override void GetAdditionalResources(ScriptRegister scriptRegister, StyleRegister styleRegister)
        {
            results.ForEach(x => x.GetAdditionalResources(scriptRegister, styleRegister));
        }

        public override bool OverrideExecuteResult()
        {
            if (results.Any(form => form != null && form.OverrideExecuteResult()))
            {
                return true;
            }

            return base.OverrideExecuteResult();
        }

        public override string GenerateView()
        {
            return string.Join("", results.Where(x => x != null).Select(f => f.GenerateView()));
        }
    }
}